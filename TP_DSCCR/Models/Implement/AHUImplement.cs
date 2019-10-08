using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using TP_DSCCR.Models.Access;
using TP_DSCCR.Models.Entity;
using TP_DSCCR.Models.Data;

namespace TP_DSCCR.Models.Implement
{
    public class AHUImplement : EnterpriseLibrary
    {
        public AHUImplement(string connectionStringName) : base(connectionStringName) { }

        public CaseRetrieveRes PaginationRetrieve(CaseRetrieveReq req)
        {
            CaseRetrieveRes res = new CaseRetrieveRes()
            {
                CASE = new List<CASE>(),
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
                //SELECT COUNT(1) FROM(SELECT TOP(@TOP) NULL AS N FROM CASE_OWNER{ 0}) A;
                //SELECT TOP(@TOP) SN,NAME1,NAME2,NAME3,NAME4,NAME5,NAME6,TEL1,TEL2,TEL3,TEL4,TEL5,TEL6,YEARS_OLD,ADDRESS,TEACHER1,TEACHER2,TEACHER3,FAMILY,FAMILY_FILE,RESIDENCE,NOTE,PROBLEM,EXPERIENCE,SUGGEST,MERGE_REASON
                //    FROM CASE_OWNER{ 0}
                //{ 1};

                string sql = @"
SELECT SID,CDATE,AUTOID,DATETIME,LOCATION,DEVICE_ID,AHU01,AHU02,AHU03,AHU04,AHU05,AHU06,AHU07,AHU08,AHU09,AHU10,AHU11
  FROM AHU
";
                string where = "";
                Db.AddInParameter(cmd, "TOP", DbType.Int32, 1000);


                if (req.CASE.SN != null)
                {
                    where += " AND SN=@SN";
                    Db.AddInParameter(cmd, "SN", DbType.String, req.CASE.SN);
                }
                if (!string.IsNullOrEmpty(req.CASE.NAME1))
                {
                    where += " AND (NAME1 LIKE @NAME OR NAME2 LIKE @NAME OR NAME3 LIKE @NAME OR NAME4 LIKE @NAME OR NAME5 LIKE @NAME OR NAME6 LIKE @NAME)";
                    Db.AddInParameter(cmd, "NAME", DbType.String, "%" + req.CASE.NAME1 + "%");
                }
                if (!string.IsNullOrEmpty(req.CASE.TEL1))
                {
                    where += " AND (TEL1 LIKE @TEL OR TEL2 LIKE @TEL OR TEL3 LIKE @TEL OR TEL4 LIKE @TEL OR TEL5 LIKE @TEL OR TEL6 LIKE @TEL)";
                    Db.AddInParameter(cmd, "TEL", DbType.String, "%" + req.CASE.TEL1 + "%");
                }
                if (!string.IsNullOrEmpty(req.CASE.TEACHER1))
                {
                    where += " AND (TEACHER1 LIKE @TEACHER OR TEACHER2 LIKE @TEACHER OR TEACHER3 LIKE @TEACHER)";
                    Db.AddInParameter(cmd, "TEACHER", DbType.String, "%" + req.CASE.TEACHER1 + "%");
                }
                if (!string.IsNullOrEmpty(req.CASE.YEARS_OLD))
                {
                    where += " AND YEARS_OLD LIKE @YEARS_OLD";
                    Db.AddInParameter(cmd, "YEARS_OLD", DbType.String, "%" + req.CASE.YEARS_OLD + "%");
                }
                if (!string.IsNullOrEmpty(req.CASE.ADDRESS))
                {
                    where += " AND ADDRESS LIKE @ADDRESS";
                    Db.AddInParameter(cmd, "ADDRESS", DbType.String, "%" + req.CASE.ADDRESS + "%");
                }
                if (!string.IsNullOrEmpty(req.CASE.FAMILY))
                {
                    where += " AND FAMILY LIKE @FAMILY";
                    Db.AddInParameter(cmd, "FAMILY", DbType.String, "%" + req.CASE.FAMILY + "%");
                }
                if (!string.IsNullOrEmpty(req.CASE.RESIDENCE))
                {
                    where += " AND RESIDENCE LIKE @RESIDENCE";
                    Db.AddInParameter(cmd, "RESIDENCE", DbType.String, "%" + req.CASE.RESIDENCE + "%");
                }
                if (!string.IsNullOrEmpty(req.CASE.NOTE))
                {
                    where += " AND NOTE LIKE @NOTE";
                    Db.AddInParameter(cmd, "NOTE", DbType.String, "%" + req.CASE.NOTE + "%");
                }
                if (!string.IsNullOrEmpty(req.CASE.PROBLEM))
                {
                    where += " AND PROBLEM LIKE @PROBLEM";
                    Db.AddInParameter(cmd, "PROBLEM", DbType.String, "%" + req.CASE.PROBLEM + "%");
                }
                if (!string.IsNullOrEmpty(req.CASE.EXPERIENCE))
                {
                    where += " AND EXPERIENCE LIKE @EXPERIENCE";
                    Db.AddInParameter(cmd, "EXPERIENCE", DbType.String, "%" + req.CASE.EXPERIENCE + "%");
                }
                if (!string.IsNullOrEmpty(req.CASE.SUGGEST))
                {
                    where += " AND SUGGEST LIKE @SUGGEST";
                    Db.AddInParameter(cmd, "SUGGEST", DbType.String, "%" + req.CASE.SUGGEST + "%");
                }


                if (!string.IsNullOrEmpty(req.CASE.CASE_STATUS))
                {
                    where += " AND SN IN (SELECT DISTINCT CASE_SN FROM INTERVIEW WHERE CASE_STATUS=@CASE_STATUS)";
                    Db.AddInParameter(cmd, "CASE_STATUS", DbType.String, req.CASE.CASE_STATUS);
                }
                if (!string.IsNullOrEmpty(req.CASE.CONTACT_TIME))
                {
                    where += " AND SN IN (SELECT DISTINCT CASE_SN FROM INTERVIEW WHERE CONTACT_TIME=@CONTACT_TIME)";
                    Db.AddInParameter(cmd, "CONTACT_TIME", DbType.String, req.CASE.CONTACT_TIME);
                }
                if (req.CASE.START_DATE != null)
                {
                    where += " AND SN IN (SELECT DISTINCT CASE_SN FROM INTERVIEW WHERE INCOMING_DATE>=@START_DATE)";
                    Db.AddInParameter(cmd, "START_DATE", DbType.DateTime2, req.CASE.START_DATE);
                }
                if (req.CASE.END_DATE != null)
                {
                    where += " AND SN IN (SELECT DISTINCT CASE_SN FROM INTERVIEW WHERE INCOMING_DATE<=@END_DATE)";
                    Db.AddInParameter(cmd, "END_DATE", DbType.DateTime2, req.CASE.END_DATE);
                }
                if (!string.IsNullOrEmpty(req.CASE.NAME))
                {
                    where += " AND SN IN (SELECT DISTINCT CASE_SN FROM INTERVIEW WHERE NAME LIKE @NAME)";
                    Db.AddInParameter(cmd, "NAME", DbType.String, "%" + req.CASE.NAME + "%");
                }
                if (!string.IsNullOrEmpty(req.CASE.TEL))
                {
                    where += " AND SN IN (SELECT DISTINCT CASE_SN FROM INTERVIEW WHERE TEL LIKE @TEL)";
                    Db.AddInParameter(cmd, "TEL", DbType.String, "%" + req.CASE.TEL + "%");
                }
                if (!string.IsNullOrEmpty(req.CASE.VOLUNTEER_SN))
                {
                    where += " AND SN IN (SELECT DISTINCT CASE_SN FROM INTERVIEW WHERE VOLUNTEER_SN=@VOLUNTEER_SN)";
                    Db.AddInParameter(cmd, "VOLUNTEER_SN", DbType.String, req.CASE.VOLUNTEER_SN);
                }

                if (where.Length > 0)
                {
                    where = " WHERE" + where.Substring(4);
                }

                string[] orderColumn = { "SN", "TEL1", "TEL2", "TEL3", "TEL4", "TEL5", "TEL6", "NAME1", "NAME2", "NAME3", "NAME4", "NAME5", "NAME6", "TEACHER1", "TEACHER2", "TEACHER3" };
                string[] ascDesc = { "ASC", "DESC", "" };

                string order = "";

                if (!string.IsNullOrEmpty(req.CASE.ORDER_BY))
                {
                    if (Array.IndexOf(orderColumn, req.CASE.ORDER_BY) > -1 && Array.IndexOf(ascDesc, req.CASE.ASC_DESC) > -1)
                    {
                        order = " ORDER BY " + req.CASE.ORDER_BY + " " + req.CASE.ASC_DESC;
                    }
                }


                sql = String.Format(sql, where, order);
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sql;
                using (IDataReader reader = Db.ExecuteReader(cmd))
                {
                    reader.Read();
                    int.TryParse(reader[0].ToString(), out res.Pagination.RowCount);
                    if (res.Pagination.RowCount > 0)
                    {
                        reader.NextResult();

                        res.Pagination.PageCount = Convert.ToInt32(Math.Ceiling(1.0 * res.Pagination.RowCount / req.PageSize));
                        res.Pagination.PageNumber = req.PageNumber < 1 ? 1 : req.PageNumber;
                        res.Pagination.PageNumber = req.PageNumber > res.Pagination.PageCount ? res.Pagination.PageCount : res.Pagination.PageNumber;
                        res.Pagination.MinNumber = (res.Pagination.PageNumber - 1) * req.PageSize + 1;
                        res.Pagination.MaxNumber = res.Pagination.PageNumber * req.PageSize;
                        res.Pagination.MaxNumber = res.Pagination.MaxNumber > res.Pagination.RowCount ? res.Pagination.RowCount : res.Pagination.MaxNumber;

                        int i = 0;
                        while (reader.Read())
                        {
                            i++;
                            if (i >= res.Pagination.MinNumber && i <= res.Pagination.MaxNumber)
                            {
                                var row = new CASE
                                {
                                    SN = reader["SN"] as int? ?? null,
                                    NAME1 = reader["NAME1"] as string,
                                    NAME2 = reader["NAME2"] as string,
                                    NAME3 = reader["NAME3"] as string,
                                    NAME4 = reader["NAME4"] as string,
                                    NAME5 = reader["NAME5"] as string,
                                    NAME6 = reader["NAME6"] as string,
                                    TEL1 = reader["TEL1"] as string,
                                    TEL2 = reader["TEL2"] as string,
                                    TEL3 = reader["TEL3"] as string,
                                    TEL4 = reader["TEL4"] as string,
                                    TEL5 = reader["TEL5"] as string,
                                    TEL6 = reader["TEL6"] as string,
                                    YEARS_OLD = reader["YEARS_OLD"] as string,
                                    ADDRESS = reader["ADDRESS"] as string,
                                    TEACHER1 = reader["TEACHER1"] as string,
                                    TEACHER2 = reader["TEACHER2"] as string,
                                    TEACHER3 = reader["TEACHER3"] as string,
                                    FAMILY = reader["FAMILY"] as string,
                                    FAMILY_FILE = reader["FAMILY_FILE"] as string,
                                    RESIDENCE = reader["RESIDENCE"] as string,
                                    NOTE = reader["NOTE"] as string,
                                    PROBLEM = reader["PROBLEM"] as string,
                                    EXPERIENCE = reader["EXPERIENCE"] as string,
                                    SUGGEST = reader["SUGGEST"] as string,
                                    MERGE_REASON = reader["MERGE_REASON"] as string
                                };
                                res.CASE.Add(row);
                            }
                            else if (i > res.Pagination.MaxNumber)
                            {
                                reader.Close();
                                break;
                            }
                        }
                    }
                }
            }
            res.Pagination.EndTime = DateTime.Now;

            return res;
        }

        public AHU ModificationQuery(int? SN)
        {
            DbConnection conn = null;
            DbTransaction trans = null;
            DbCommand cmd = null;
            string sql = null;
            AHU AHU = null;
            try
            {
                conn = Db.CreateConnection();
                conn.Open();

                #region AHU
                sql = @"
SELECT SID,CDATE,AUTOID,DATETIME,LOCATION,DEVICE_ID,AHU01,AHU02,AHU03,AHU04,AHU05,AHU06,AHU07,AHU08,AHU09,AHU10,AHU11
  FROM AHU
";
                cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sql;
                Db.AddInParameter(cmd, "SN", DbType.Int32, SN);
                using (IDataReader reader = Db.ExecuteReader(cmd))
                {
                    while (reader.Read())
                    {
                        AHU = new AHU
                        {
                            SID = (int)reader["SID"],
                            CDATE = (DateTime)reader["DATETIME"],
                            AUTOID = (int)reader["AUTOID"],
                            DATETIME = reader["DATETIME"] as DateTime? ?? null,
                            AHU01 = reader["AHU01"] as Single? ?? null,
                            AHU02 = reader["AHU02"] as Single? ?? null,
                            AHU03 = reader["AHU03"] as Single? ?? null,
                            AHU04 = reader["AHU04"] as Single? ?? null,
                            AHU05 = reader["AHU05"] as Single? ?? null,
                            AHU06 = reader["AHU06"] as Single? ?? null,
                            AHU07 = reader["AHU07"] as Single? ?? null,
                            AHU08 = reader["AHU08"] as Single? ?? null,
                            AHU09 = reader["AHU09"] as Single? ?? null,
                            AHU10 = reader["AHU10"] as Single? ?? null,
                            AHU11 = reader["AHU11"] as Single? ?? null
                        };
                    }
                }
                #endregion

            }
            catch (Exception ex)
            {
                trans.Rollback();
                throw;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
            return AHU;
        }
    }
}