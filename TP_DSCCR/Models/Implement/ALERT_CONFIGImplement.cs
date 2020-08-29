using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using TP_DSCCR.Models.Access;
using TP_DSCCR.Models.Entity;
using TP_DSCCR.ViewModels;
using System.IO;

namespace TP_DSCCR.Models.Implement
{
    public class ALERT_CONFIGImplement : EnterpriseLibrary
    {
        public ALERT_CONFIGImplement(string connectionStringName) : base(connectionStringName) { }

        public ALERT_CONFIGRetrieveRes PaginationRetrieve(ALERT_CONFIGRetrieveReq Req)
        {
            ALERT_CONFIGRetrieveRes res = new ALERT_CONFIGRetrieveRes()
            {
                ALERT_CONFIG = new List<ALERT_CONFIG>(),
                Pagination = new Pagination
                {
                    PageCount = 0,
                    RowCount = 0,
                    PageNumber = 0,
                    MinNumber = 0,
                    MaxNumber = 0,
                    StartTime = DateTime.Now
                }
            };

            using (DbCommand cmd = Db.CreateConnection().CreateCommand())
            {
                string sql = @"
SELECT TOP(@TOP) SN,HR_DATE,TP_SCC.dbo.PHRASE_NAME('DATE_TYPE',DATE_TYPE,default) AS DATE_TYPE,MEMO,CDATE,CUSER,MDATE,MUSER
    FROM ALERT_CONFIG
    {0}
    ORDER BY HR_DATE DESC
";

                //Db.AddInParameter(cmd, "TOP", DbType.Int32, 1000);

                string where = "";
                Db.AddInParameter(cmd, "TOP", DbType.Int32, 1000);

                if (Req.HR_DATE_START !=null)
                {
                    where += " AND HR_DATE>=@HR_DATE_START";
                    Db.AddInParameter(cmd, "HR_DATE_START", DbType.Date, Req.HR_DATE_START);
                }
                if (Req.HR_DATE_END != null)
                {
                    where += " AND HR_DATE<=@HR_DATE_END";
                    Db.AddInParameter(cmd, "HR_DATE_END", DbType.Date, Req.HR_DATE_END);
                }
                if (!string.IsNullOrEmpty(Req.ALERT_CONFIG.DATE_TYPE))
                {
                    where += " AND DATE_TYPE=@DATE_TYPE";
                    Db.AddInParameter(cmd, "DATE_TYPE", DbType.String, Req.ALERT_CONFIG.DATE_TYPE);
                }
                if (where.Length > 0)
                {
                    where = " WHERE" + where.Substring(4);
                }

                sql = String.Format(sql, where);
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sql;
                using (DataTable dt = Db.ExecuteDataSet(cmd).Tables[0])
                {
                    res.Pagination.RowCount = dt.Rows.Count;
                    res.Pagination.PageCount = Convert.ToInt32(Math.Ceiling(1.0 * res.Pagination.RowCount / Req.PageSize));
                    res.Pagination.PageNumber = Req.PageNumber < 1 ? 1 : Req.PageNumber;
                    res.Pagination.PageNumber = Req.PageNumber > res.Pagination.PageCount ? res.Pagination.PageCount : res.Pagination.PageNumber;
                    res.Pagination.MinNumber = (res.Pagination.PageNumber - 1) * Req.PageSize + 1;
                    res.Pagination.MaxNumber = res.Pagination.PageNumber * Req.PageSize;
                    res.Pagination.MaxNumber = res.Pagination.MaxNumber > res.Pagination.RowCount ? res.Pagination.RowCount : res.Pagination.MaxNumber;

                    if (dt.Rows.Count > 0)
                    {
                        for (int i = res.Pagination.MinNumber - 1; i < res.Pagination.MaxNumber; i++)
                        {
                            var row = new ALERT_CONFIG
                            {
                                SN = (int)dt.Rows[i]["SN"],
                                HR_DATE = (DateTime)dt.Rows[i]["HR_DATE"],
                                DATE_TYPE = dt.Rows[i]["DATE_TYPE"] as string,
                                MEMO = dt.Rows[i]["MEMO"] as string,
                                CDATE = dt.Rows[i]["CDATE"] as DateTime? ?? null,
                                CUSER = dt.Rows[i]["CUSER"] as string,
                                MDATE = dt.Rows[i]["MDATE"] as DateTime? ?? null,
                                MUSER = dt.Rows[i]["MUSER"] as string,
                            };
                            res.ALERT_CONFIG.Add(row);
                        }
                    }
                }
            }
            res.Pagination.EndTime = DateTime.Now;

            return res;
        }

        public ALERT_CONFIGModifyRes ModificationQuery(ALERT_CONFIGModifyReq Req)
        {
            ALERT_CONFIGModifyRes res = new ALERT_CONFIGModifyRes();

            using (DbCommand cmd = Db.CreateConnection().CreateCommand())
            {
                string sql = @"
SELECT SN,HR_DATE,DATE_TYPE,MEMO,CDATE,CUSER,MDATE,MUSER
    FROM ALERT_CONFIG
    WHERE SN=@SN
        ";
                Db.AddInParameter(cmd, "SN", DbType.Int32, Req.ALERT_CONFIG.SN);

                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sql;
                using (IDataReader reader = Db.ExecuteReader(cmd))
                {
                    if (reader.Read())
                    {
                        ALERT_CONFIG ALERT_CONFIG = new ALERT_CONFIG
                        {
                            SN = (int)reader["SN"],
                            HR_DATE = reader["HR_DATE"] as DateTime?,
                            DATE_TYPE = reader["DATE_TYPE"] as string,
                            MEMO = reader["MEMO"] as string,
                            CDATE = reader["CDATE"] as DateTime?,
                            CUSER = reader["CUSER"] as string,
                            MDATE = reader["MDATE"] as DateTime?,
                            MUSER = reader["MUSER"] as string
                        };
                        res.ALERT_CONFIG = ALERT_CONFIG;
                    }
                }
            }

            return res;
        }

        public bool DataCreate(ALERT_CONFIGModifyReq req)
        {
            int effect = 0;

            using (DbCommand cmd = Db.CreateConnection().CreateCommand())
            {
                string sql = @"
SET @SN = NEXT VALUE FOR [ALERT_CONFIG_SEQ]
INSERT ALERT_CONFIG (SN,HR_DATE,DATE_TYPE,MEMO,CDATE,CUSER,MDATE,MUSER)
    VALUES (@SN,@HR_DATE,@DATE_TYPE,@MEMO,GETDATE(),@CUSER,GETDATE(),@MUSER);
        ";
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sql;

                Db.AddInParameter(cmd, "HR_DATE", DbType.Date, req.ALERT_CONFIG.HR_DATE);
                Db.AddInParameter(cmd, "DATE_TYPE", DbType.String, req.ALERT_CONFIG.DATE_TYPE);
                Db.AddInParameter(cmd, "MEMO", DbType.String, req.ALERT_CONFIG.MEMO);
                Db.AddInParameter(cmd, "CUSER", DbType.String, req.ALERT_CONFIG.CUSER);
                Db.AddInParameter(cmd, "MUSER", DbType.String, req.ALERT_CONFIG.MUSER);
                Db.AddOutParameter(cmd, "SN", DbType.Int32, 1);

                effect = Db.ExecuteNonQuery(cmd);
                req.ALERT_CONFIG.SN = Db.GetParameterValue(cmd, "SN") as Int32? ?? null;
                return true;
            }
        }

        public bool DataUpdate(ALERT_CONFIGModifyReq req)
        {
            int effect = 0;

            using (DbCommand cmd = Db.CreateConnection().CreateCommand())
            {
                string sql = @"
UPDATE ALERT_CONFIG
    SET HR_DATE=@HR_DATE,DATE_TYPE=@DATE_TYPE,MEMO=@MEMO,MDATE=GETDATE(),MUSER=@MUSER
    WHERE SN=@SN;
";
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sql;

                Db.AddInParameter(cmd, "HR_DATE", DbType.Date, req.ALERT_CONFIG.HR_DATE);
                Db.AddInParameter(cmd, "DATE_TYPE", DbType.String, req.ALERT_CONFIG.DATE_TYPE);
                Db.AddInParameter(cmd, "MEMO", DbType.String, req.ALERT_CONFIG.MEMO);
                Db.AddInParameter(cmd, "MUSER", DbType.String, req.ALERT_CONFIG.MUSER);
                Db.AddInParameter(cmd, "SN", DbType.String, req.ALERT_CONFIG.SN);

                effect = Db.ExecuteNonQuery(cmd);
            }

            return effect == 1;
        }

        public int DataDelete(ALERT_CONFIGModifyReq req)
        {
            int count = 0;
            using (DbCommand cmd = Db.CreateConnection().CreateCommand())
            {
                string sql = @"
DELETE FROM ALERT_CONFIG
    WHERE SN=@SN;
        ";
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sql;
                Db.AddInParameter(cmd, "SN", DbType.String, req.ALERT_CONFIG.SN);
                count = Db.ExecuteNonQuery(cmd);
            }
            return count;
        }

        public bool DataDuplicate(ALERT_CONFIGModifyReq Req)
        {
            bool yn = false;
            using (DbCommand cmd = Db.CreateConnection().CreateCommand())
            {
                string sql = @"
SELECT 1
    FROM ALERT_CONFIG
    WHERE HR_DATE=@HR_DATE
        ";
                Db.AddInParameter(cmd, "HR_DATE", DbType.Date, Req.ALERT_CONFIG.HR_DATE);

                if (Req.ALERT_CONFIG.SN != null)
                {
                    sql += " AND SN<>@SN";
                    Db.AddInParameter(cmd, "SN", DbType.Int32, Req.ALERT_CONFIG.SN);
                }

                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sql;
                using (IDataReader reader = Db.ExecuteReader(cmd))
                {
                    if (reader.Read())
                    {
                        yn = true;
                    }
                }
            }
            return yn;
        }
    }
}