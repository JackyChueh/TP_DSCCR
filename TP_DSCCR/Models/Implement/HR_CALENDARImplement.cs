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
    public class HR_CALENDARImplement : EnterpriseLibrary
    {
        public HR_CALENDARImplement(string connectionStringName) : base(connectionStringName) { }

        public HR_CALENDARRetrieveRes PaginationRetrieve(DateTime StartDate, DateTime EndDate)
        {
            HR_CALENDARRetrieveRes res = new HR_CALENDARRetrieveRes()
            {
                HR_CALENDAR = new List<HR_CALENDAR>(),
            };

            using (DbCommand cmd = Db.CreateConnection().CreateCommand())
            {
                string sql = @"
SELECT TOP(@TOP) SN,HR_DATE,TP_SCC.dbo.PHRASE_NAME('DATE_TYPE',DATE_TYPE,default) AS DATE_TYPE,MEMO,CDATE,CUSER,MDATE,MUSER
    FROM HR_CALENDAR
    WHERE HR_DATE>=@HR_DATE_START AND HR_DATE<@HR_DATE_END
    ORDER BY HR_DATE
";
                //Db.AddInParameter(cmd, "TOP", DbType.Int32, 1000);
                string where = "";
                Db.AddInParameter(cmd, "TOP", DbType.Int32, 1000);
                Db.AddInParameter(cmd, "HR_DATE_START", DbType.Date, StartDate);
                Db.AddInParameter(cmd, "HR_DATE_END", DbType.Date, EndDate);
              
                if (where.Length > 0)
                {
                    where = " WHERE" + where.Substring(4);
                }

                sql = String.Format(sql, where);
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sql;
                using (DataTable dt = Db.ExecuteDataSet(cmd).Tables[0])
                {
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            var row = new HR_CALENDAR
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
                            res.HR_CALENDAR.Add(row);
                        }
                    }
                }
            }

            return res;
        }

        public HR_CALENDARModifyRes ModificationQuery(HR_CALENDARModifyReq Req)
        {
            HR_CALENDARModifyRes res = new HR_CALENDARModifyRes();

            using (DbCommand cmd = Db.CreateConnection().CreateCommand())
            {
                string sql = @"
SELECT SN,HR_DATE,DATE_TYPE,MEMO,CDATE,CUSER,MDATE,MUSER
    FROM HR_CALENDAR
    WHERE {0}
        ";
                string where = "";
                if (Req.HR_CALENDAR.SN != null)
                {
                    where += " AND SN = @SN";
                    Db.AddInParameter(cmd, "SN", DbType.Int32, Req.HR_CALENDAR.SN);
                }

                if (Req.HR_CALENDAR.HR_DATE != null)
                {
                    where += " AND HR_DATE = @HR_DATE";
                    Db.AddInParameter(cmd, "HR_DATE", DbType.Date, Req.HR_CALENDAR.HR_DATE);
                }
                where = where.Substring(4);
                sql = string.Format(sql, where);
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sql;
                using (IDataReader reader = Db.ExecuteReader(cmd))
                {
                    if (reader.Read())
                    {
                        HR_CALENDAR HR_CALENDAR = new HR_CALENDAR
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
                        res.HR_CALENDAR = HR_CALENDAR;
                    }
                }
            }

            return res;
        }

        public bool DataCreateYear(DateTime StartDate, DateTime EndDate, string UserId)
        {
            int effect = 0;

            using (DbCommand cmd = Db.CreateConnection().CreateCommand())
            {

                string sql = @"
SET @SN = NEXT VALUE FOR [HR_CALENDAR_SEQ]
INSERT HR_CALENDAR (SN,HR_DATE,DATE_TYPE,CDATE,CUSER,MDATE,MUSER)
    VALUES (@SN,@HR_DATE,@DATE_TYPE,GETDATE(),@CUSER,GETDATE(),@MUSER);
        ";
                Db.AddInParameter(cmd, "HR_DATE", DbType.Date);
                Db.AddInParameter(cmd, "DATE_TYPE", DbType.String);
                Db.AddInParameter(cmd, "CUSER", DbType.String, UserId);
                Db.AddInParameter(cmd, "MUSER", DbType.String, UserId);
                Db.AddOutParameter(cmd, "SN", DbType.Int32, 1);

                while (StartDate < EndDate)
                {
                    if (StartDate.DayOfWeek == DayOfWeek.Sunday || StartDate.DayOfWeek == DayOfWeek.Saturday)
                    {
                        Db.SetParameterValue(cmd, "HR_DATE", StartDate);
                        Db.SetParameterValue(cmd, "DATE_TYPE", 'H');

                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = sql;
                        effect += Db.ExecuteNonQuery(cmd);
                    }
                    else
                    {
                        Db.SetParameterValue(cmd, "HR_DATE", StartDate);
                        Db.SetParameterValue(cmd, "DATE_TYPE", 'W');

                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = sql;
                        effect += Db.ExecuteNonQuery(cmd);
                    }
                    StartDate = StartDate.AddDays(1);
                }
                return true;
            }
        }

        public bool DataCreate(HR_CALENDARModifyReq req)
        {
            int effect = 0;

            using (DbCommand cmd = Db.CreateConnection().CreateCommand())
            {
                string sql = @"
SET @SN = NEXT VALUE FOR [HR_CALENDAR_SEQ]
INSERT HR_CALENDAR (SN,HR_DATE,DATE_TYPE,MEMO,CDATE,CUSER,MDATE,MUSER)
    VALUES (@SN,@HR_DATE,@DATE_TYPE,@MEMO,GETDATE(),@CUSER,GETDATE(),@MUSER);
        ";
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sql;

                Db.AddInParameter(cmd, "HR_DATE", DbType.Date, req.HR_CALENDAR.HR_DATE);
                Db.AddInParameter(cmd, "DATE_TYPE", DbType.String, req.HR_CALENDAR.DATE_TYPE);
                Db.AddInParameter(cmd, "MEMO", DbType.String, req.HR_CALENDAR.MEMO);
                Db.AddInParameter(cmd, "CUSER", DbType.String, req.HR_CALENDAR.CUSER);
                Db.AddInParameter(cmd, "MUSER", DbType.String, req.HR_CALENDAR.MUSER);
                Db.AddOutParameter(cmd, "SN", DbType.Int32, 1);

                effect = Db.ExecuteNonQuery(cmd);
                req.HR_CALENDAR.SN = Db.GetParameterValue(cmd, "SN") as Int32? ?? null;
                return true;
            }
        }

        public bool DataUpdate(HR_CALENDARModifyReq req)
        {
            int effect = 0;

            using (DbCommand cmd = Db.CreateConnection().CreateCommand())
            {
                string sql = @"
UPDATE HR_CALENDAR
    SET HR_DATE=@HR_DATE,DATE_TYPE=@DATE_TYPE,MEMO=@MEMO,MDATE=GETDATE(),MUSER=@MUSER
    WHERE SN=@SN;
";
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sql;

                Db.AddInParameter(cmd, "HR_DATE", DbType.Date, req.HR_CALENDAR.HR_DATE);
                Db.AddInParameter(cmd, "DATE_TYPE", DbType.String, req.HR_CALENDAR.DATE_TYPE);
                Db.AddInParameter(cmd, "MEMO", DbType.String, req.HR_CALENDAR.MEMO);
                Db.AddInParameter(cmd, "MUSER", DbType.String, req.HR_CALENDAR.MUSER);
                Db.AddInParameter(cmd, "SN", DbType.String, req.HR_CALENDAR.SN);

                effect = Db.ExecuteNonQuery(cmd);
            }

            return effect == 1;
        }

        public int DataDelete(HR_CALENDARModifyReq req)
        {
            int count = 0;
            using (DbCommand cmd = Db.CreateConnection().CreateCommand())
            {
                string sql = @"
DELETE FROM HR_CALENDAR
    WHERE SN=@SN;
        ";
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sql;
                Db.AddInParameter(cmd, "SN", DbType.String, req.HR_CALENDAR.SN);
                count = Db.ExecuteNonQuery(cmd);
            }
            return count;
        }

        public bool DataDuplicateYear(DateTime StartDate, DateTime EndDate)
        {
            bool yn = false;

            using (DbCommand cmd = Db.CreateConnection().CreateCommand())
            {
                string sql = @"
SELECT 1
    FROM HR_CALENDAR
    WHERE HR_DATE>=@SDATE AND HR_DATE<@EDATE
        ";
                Db.AddInParameter(cmd, "SDATE", DbType.Date, StartDate);
                Db.AddInParameter(cmd, "EDATE", DbType.Date, EndDate);

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

        public bool DataDuplicate(HR_CALENDARModifyReq Req)
        {
            bool yn = false;
            using (DbCommand cmd = Db.CreateConnection().CreateCommand())
            {
                string sql = @"
SELECT 1
    FROM HR_CALENDAR
    WHERE HR_DATE=@HR_DATE
        ";
                Db.AddInParameter(cmd, "HR_DATE", DbType.Date, Req.HR_CALENDAR.HR_DATE);

                if (Req.HR_CALENDAR.SN != null)
                {
                    sql += " AND SN<>@SN";
                    Db.AddInParameter(cmd, "SN", DbType.Int32, Req.HR_CALENDAR.SN);
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