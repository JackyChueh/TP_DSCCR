using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using TP_DSCCR.Models.Access;
using TP_DSCCR.Models.Entity;
using TP_DSCCR.ViewModels;

namespace TP_DSCCR.Models.Implement
{
    public class AHUImplement : EnterpriseLibrary
    {
        public AHUImplement(string connectionStringName) : base(connectionStringName) { }

        public AHURes PaginationRetrieve(AHUReq req)
        {
            AHURes res = new AHURes()
            {
                AHU = new List<AHU>(),
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
SELECT COUNT(1) FROM(SELECT TOP(@TOP) NULL AS N FROM AHU) A;
SELECT TOP(@TOP) SID,CDATE,AUTOID,DATETIME,LOCATION,DEVICE_ID,AHU01,AHU02,AHU03,AHU04,AHU05,AHU06,AHU07,AHU08,AHU09,AHU10,AHU11
  FROM AHU
";
                //string where = "";
                Db.AddInParameter(cmd, "TOP", DbType.Int32, 1000);


                //if (req.CASE.SN != null)
                //{
                //    where += " AND SN=@SN";
                //    Db.AddInParameter(cmd, "SN", DbType.String, req.CASE.SN);
                //}
                //if (!string.IsNullOrEmpty(req.CASE.NAME1))
                //{
                //    where += " AND (NAME1 LIKE @NAME OR NAME2 LIKE @NAME OR NAME3 LIKE @NAME OR NAME4 LIKE @NAME OR NAME5 LIKE @NAME OR NAME6 LIKE @NAME)";
                //    Db.AddInParameter(cmd, "NAME", DbType.String, "%" + req.CASE.NAME1 + "%");
                //}
                //if (!string.IsNullOrEmpty(req.CASE.TEL1))
                //{
                //    where += " AND (TEL1 LIKE @TEL OR TEL2 LIKE @TEL OR TEL3 LIKE @TEL OR TEL4 LIKE @TEL OR TEL5 LIKE @TEL OR TEL6 LIKE @TEL)";
                //    Db.AddInParameter(cmd, "TEL", DbType.String, "%" + req.CASE.TEL1 + "%");
                //}
                //if (!string.IsNullOrEmpty(req.CASE.TEACHER1))
                //{
                //    where += " AND (TEACHER1 LIKE @TEACHER OR TEACHER2 LIKE @TEACHER OR TEACHER3 LIKE @TEACHER)";
                //    Db.AddInParameter(cmd, "TEACHER", DbType.String, "%" + req.CASE.TEACHER1 + "%");
                //}

                //if (where.Length > 0)
                //{
                //    where = " WHERE" + where.Substring(4);
                //}

                //sql = String.Format(sql, where, order);
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
                                var row = new AHU
                                {
                                    SID = (int)reader["SID"],
                                    CDATE = (DateTime)reader["CDATE"],
                                    AUTOID = (int)reader["AUTOID"],
                                    DATETIME = reader["DATETIME"] as DateTime? ?? null,
                                    LOCATION = reader["LOCATION"] as string,
                                    DEVICE_ID = reader["DEVICE_ID"] as string,
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
                                res.AHU.Add(row);
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

        public AHURes GraphRetrieve(AHUReq req)
        {
            AHURes res = new AHURes()
            {
                AHUChartJS = new List<AHUChartJS>(),
            };

            using (DbCommand cmd = Db.CreateConnection().CreateCommand())
            {
                string sql = @"
 SELECT TOP(@TOP) CONVERT(VARCHAR(13),CDATE,120) AS CDATE
    ,CONVERT(DECIMAL(28,2),AVG(AHU01)) AS AHU01
    ,CONVERT(DECIMAL(28,2),AVG(AHU02)) AS AHU02
    ,CONVERT(DECIMAL(28,2),AVG(AHU03)) AS AHU03
    ,CONVERT(DECIMAL(28,2),AVG(AHU04)) AS AHU04
    ,CONVERT(DECIMAL(28,2),AVG(AHU05)) AS AHU05
    ,CONVERT(DECIMAL(28,2),AVG(AHU06)) AS AHU06
    ,CONVERT(DECIMAL(28,2),AVG(AHU07)) AS AHU07
    ,CONVERT(DECIMAL(28,2),AVG(AHU08)) AS AHU08
    ,CONVERT(DECIMAL(28,2),AVG(AHU09)) AS AHU09
    ,CONVERT(DECIMAL(28,2),AVG(AHU10)) AS AHU10
    ,CONVERT(DECIMAL(28,2),AVG(AHU11)) AS AHU11
FROM AHU
WHERE  CDATE>'2019-10-10'
GROUP BY CONVERT(VARCHAR(13),CDATE,120)
  ORDER BY CONVERT(VARCHAR(13),CDATE,120)
";
                Db.AddInParameter(cmd, "TOP", DbType.Int32, 1000);

                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sql;
                using (IDataReader reader = Db.ExecuteReader(cmd))
                {
                    while (reader.Read())
                    {
                        var row = new AHUChartJS
                        {
                            CDATE = reader["CDATE"] as string ,
                            AHU01 = (Decimal)reader["AHU01"],
                            AHU02 = (Decimal)reader["AHU02"],
                            AHU03 = (Decimal)reader["AHU03"],
                            AHU04 = (Decimal)reader["AHU04"],
                            AHU05 = (Decimal)reader["AHU05"],
                            AHU06 = (Decimal)reader["AHU06"],
                            AHU07 = (Decimal)reader["AHU07"],
                            AHU08 = (Decimal)reader["AHU08"],
                            AHU09 = (Decimal)reader["AHU09"],
                            AHU10 = (Decimal)reader["AHU10"],
                            AHU11 = (Decimal)reader["AHU11"]
                        };
                        res.AHUChartJS.Add(row);
                    }

                }
            }

            return res;
        }
    }
}