using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using TP_DSCCR.Models.Access;
using TP_DSCCR.ViewModels;
using System.IO;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace TP_DSCCR.Models.Implement
{
    public class ZP1Implement : EnterpriseLibrary
    {
        public ZP1Implement(string connectionStringName) : base(connectionStringName) { }

        public ZP1DataRes PaginationRetrieve(ZP1DataReq req)
        {
            ZP1DataRes res = new ZP1DataRes()
            {
                ZP1Data = new List<ZP1Data>(),
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
SELECT {0} AS CDATE
    ,TP_SCC.dbo.PHRASE_NAME('ZP1_LOCATION',LOCATION,default) AS LOCATION
    ,TP_SCC.dbo.PHRASE_NAME('ZP1_DEVICE_ID',DEVICE_ID,LOCATION) AS DEVICE_ID
    ,{1}
    FROM ZP1
    {2}
    {3}
";
                string fields = "";
                switch (req.GROUP_BY_DT)
                {
                    case "DETAIL":
                        for (int i = 1; i < 12; i++)
                        {
                            if (i == 3)
                            { 
                            }
                            else if (i == 7)
                            {
                                fields += "TP_SCC.dbo.PHRASE_NAME('running', CONVERT(DECIMAL(28,1),ZP1" + i.ToString("00") + "), default) AS ZP1" + i.ToString("00") + ",";
                            }
                            else if (i == 8)
                            {
                                fields += "TP_SCC.dbo.PHRASE_NAME('function_fail', CONVERT(DECIMAL(28,1),ZP1" + i.ToString("00") + "), default) AS ZP1" + i.ToString("00") + ",";
                            }
                            else if (i == 9)
                            {
                                fields += "TP_SCC.dbo.PHRASE_NAME('switch_status', CONVERT(DECIMAL(28,1),ZP1" + i.ToString("00") + "), default) AS ZP1" + i.ToString("00") + ",";
                            }
                            else if (i == 10)
                            {
                                fields += "TP_SCC.dbo.PHRASE_NAME('open_close', CONVERT(DECIMAL(28,1),ZP1" + i.ToString("00") + "), default) AS ZP1" + i.ToString("00") + ",";
                            }
                            else
                            {
                                fields += "CONVERT(DECIMAL(28,1),ZP1" + i.ToString("00") + ") AS ZP1" + i.ToString("00") + ",";
                            }
                        }
                        break;
                    case "YEAR":
                    case "MONTH":
                    case "DAY":
                    case "HOUR":
                        for (int i = 1; i < 12; i++)
                        {
                            if (i == 3)
                            {
                            }
                            else if (i == 7)
                            {
                                fields += "'-' AS ZP1" + i.ToString("00") + ",";
                            }
                            else if (i == 8)
                            {
                                fields += "'-' AS ZP1" + i.ToString("00") + ",";
                            }
                            else if (i == 9)
                            {
                                fields += "'-' AS ZP1" + i.ToString("00") + ",";
                            }
                            else if (i == 10)
                            {
                                fields += "'-' AS ZP1" + i.ToString("00") + ",";
                            }
                            else
                            {
                                fields += "CONVERT(DECIMAL(28,1),AVG(ZP1" + i.ToString("00") + ")) AS ZP1" + i.ToString("00") + ",";
                            }
                        }
                        break;
                    default:
                        for (int i = 1; i < 12; i++)
                        {
                            if (i == 3)
                            {
                            }
                            else if (i == 7)
                            {
                                fields += "TP_SCC.dbo.PHRASE_NAME('running', CONVERT(DECIMAL(28,1),ZP1" + i.ToString("00") + "), default) AS ZP1" + i.ToString("00") + ",";
                            }
                            else if (i == 8)
                            {
                                fields += "TP_SCC.dbo.PHRASE_NAME('function_fail', CONVERT(DECIMAL(28,1),ZP1" + i.ToString("00") + "), default) AS ZP1" + i.ToString("00") + ",";
                            }
                            else if (i == 9)
                            {
                                fields += "TP_SCC.dbo.PHRASE_NAME('switch_status', CONVERT(DECIMAL(28,1),ZP1" + i.ToString("00") + "), default) AS ZP1" + i.ToString("00") + ",";
                            }
                            else if (i == 10)
                            {
                                fields += "TP_SCC.dbo.PHRASE_NAME('open_close', CONVERT(DECIMAL(28,1),ZP1" + i.ToString("00") + "), default) AS ZP1" + i.ToString("00") + ",";
                            }
                            else
                            {
                                fields += "CONVERT(DECIMAL(28,1),ZP1" + i.ToString("00") + ") AS ZP1" + i.ToString("00") + ",";
                            }
                        }
                        break;
                }

                string groupByDT = null;
                string group = null;
                switch (req.GROUP_BY_DT)
                {
                    case "DETAIL":
                        groupByDT = "CONVERT(VARCHAR(20),CDATE,120)";
                        group = "";
                        break;
                    case "YEAR":
                        groupByDT = "CONVERT(VARCHAR(4),CDATE,120)";
                        group = string.Format("GROUP BY {0},LOCATION,DEVICE_ID", groupByDT);
                        break;
                    case "MONTH":
                        groupByDT = "CONVERT(VARCHAR(7),CDATE,120)";
                        group = string.Format("GROUP BY {0},LOCATION,DEVICE_ID", groupByDT);
                        break;
                    case "DAY":
                        groupByDT = "CONVERT(VARCHAR(10),CDATE,120)";
                        group = string.Format("GROUP BY {0},LOCATION,DEVICE_ID", groupByDT);
                        break;
                    case "HOUR":
                        groupByDT = "CONVERT(VARCHAR(13),CDATE,120)";
                        group = string.Format("GROUP BY {0},LOCATION,DEVICE_ID", groupByDT);
                        break;
                    default:
                        groupByDT = "CONVERT(VARCHAR(20),CDATE,120)";
                        group = "";
                        break;
                }


                //Db.AddInParameter(cmd, "TOP", DbType.Int32, 1000);

                string where = "";
                if (req.SDATE != null)
                {
                    where += " AND CDATE>=@SDATE";
                    Db.AddInParameter(cmd, "SDATE", DbType.DateTime, req.SDATE);
                }
                if (req.EDATE != null)
                {
                    where += " AND CDATE<=@EDATE";
                    Db.AddInParameter(cmd, "EDATE", DbType.DateTime, req.EDATE);
                }
                if (!string.IsNullOrEmpty(req.ZP1.LOCATION))
                {
                    where += " AND LOCATION=@LOCATION";
                    Db.AddInParameter(cmd, "LOCATION", DbType.String, req.ZP1.LOCATION);
                }
                if (!string.IsNullOrEmpty(req.ZP1.DEVICE_ID))
                {
                    where += " AND DEVICE_ID=@DEVICE_ID";
                    Db.AddInParameter(cmd, "DEVICE_ID", DbType.String, req.ZP1.DEVICE_ID);
                }
                if (where.Length > 0)
                {
                    where = " WHERE" + where.Substring(4);
                }

                sql = String.Format(sql, groupByDT, fields.TrimEnd(','), where, group);
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sql;
                using (DataTable dt = Db.ExecuteDataSet(cmd).Tables[0])
                {
                    res.Pagination.RowCount = dt.Rows.Count;
                    res.Pagination.PageCount = Convert.ToInt32(Math.Ceiling(1.0 * res.Pagination.RowCount / req.PageSize));
                    res.Pagination.PageNumber = req.PageNumber < 1 ? 1 : req.PageNumber;
                    res.Pagination.PageNumber = req.PageNumber > res.Pagination.PageCount ? res.Pagination.PageCount : res.Pagination.PageNumber;
                    res.Pagination.MinNumber = (res.Pagination.PageNumber - 1) * req.PageSize + 1;
                    res.Pagination.MaxNumber = res.Pagination.PageNumber * req.PageSize;
                    res.Pagination.MaxNumber = res.Pagination.MaxNumber > res.Pagination.RowCount ? res.Pagination.RowCount : res.Pagination.MaxNumber;

                    if (dt.Rows.Count > 0)
                    {
                        for (int i = res.Pagination.MinNumber - 1; i < res.Pagination.MaxNumber; i++)
                        {
                            var row = new ZP1Data
                            {
                                //SID = (int)dt.Rows[i]["SID"],
                                CDATE = dt.Rows[i]["CDATE"] as string,
                                //AUTOID = (int)dt.Rows[i]["AUTOID"],
                                //DATETIME = dt.Rows[i]["DATETIME"] as DateTime? ?? null,
                                LOCATION = dt.Rows[i]["LOCATION"] as string,
                                DEVICE_ID = dt.Rows[i]["DEVICE_ID"] as string,
                                ZP101 = dt.Rows[i]["ZP101"] as decimal? ?? null,
                                ZP102 = dt.Rows[i]["ZP102"] as decimal? ?? null,
                                ZP104 = dt.Rows[i]["ZP104"] as decimal? ?? null,
                                ZP105 = dt.Rows[i]["ZP105"] as decimal? ?? null,
                                ZP106 = dt.Rows[i]["ZP106"] as decimal? ?? null,
                                ZP107 = dt.Rows[i]["ZP107"].ToString(),
                                ZP108 = dt.Rows[i]["ZP108"].ToString(),
                                ZP109 = dt.Rows[i]["ZP109"].ToString(),
                                ZP110 = dt.Rows[i]["ZP110"].ToString(),
                                ZP111 = dt.Rows[i]["ZP111"] as decimal? ?? null,
                            };
                            res.ZP1Data.Add(row);
                        }
                    }
                }
            }
            res.Pagination.EndTime = DateTime.Now;

            return res;
        }

        public MemoryStream ExcelRetrieve(ZP1ExcelReq req)
        {
            MemoryStream ms = new MemoryStream();

            List<ZP1Data> list = new List<ZP1Data>();

            using (DbCommand cmd = Db.CreateConnection().CreateCommand())
            {
                string sql = @"
SELECT {0} AS CDATE
    ,TP_SCC.dbo.PHRASE_NAME('ZP1_LOCATION',LOCATION,default) AS LOCATION
    ,TP_SCC.dbo.PHRASE_NAME('ZP1_DEVICE_ID',DEVICE_ID,LOCATION) AS DEVICE_ID
    ,{1}
    FROM ZP1
    {2}
    {3}
";
                string fields = "";
                switch (req.GROUP_BY_DT)
                {
                    case "DETAIL":
                        for (int i = 1; i < 12; i++)
                        {
                            if (i == 3)
                            {
                            }
                            else if (i == 7)
                            {
                                fields += "TP_SCC.dbo.PHRASE_NAME('running', CONVERT(DECIMAL(28,1),ZP1" + i.ToString("00") + "), default) AS ZP1" + i.ToString("00") + ",";
                            }
                            else if (i == 8)
                            {
                                fields += "TP_SCC.dbo.PHRASE_NAME('function_fail', CONVERT(DECIMAL(28,1),ZP1" + i.ToString("00") + "), default) AS ZP1" + i.ToString("00") + ",";
                            }
                            else if (i == 9)
                            {
                                fields += "TP_SCC.dbo.PHRASE_NAME('switch_status', CONVERT(DECIMAL(28,1),ZP1" + i.ToString("00") + "), default) AS ZP1" + i.ToString("00") + ",";
                            }
                            else if (i == 10)
                            {
                                fields += "TP_SCC.dbo.PHRASE_NAME('open_close', CONVERT(DECIMAL(28,1),ZP1" + i.ToString("00") + "), default) AS ZP1" + i.ToString("00") + ",";
                            }
                            else
                            {
                                fields += "CONVERT(DECIMAL(28,1),ZP1" + i.ToString("00") + ") AS ZP1" + i.ToString("00") + ",";
                            }
                        }
                        break;
                    case "YEAR":
                    case "MONTH":
                    case "DAY":
                    case "HOUR":
                        for (int i = 1; i < 12; i++)
                        {
                            if (i == 3)
                            {
                            }
                            else if (i == 7)
                            {
                                fields += "'-' AS ZP1" + i.ToString("00") + ",";
                            }
                            else if (i == 8)
                            {
                                fields += "'-' AS ZP1" + i.ToString("00") + ",";
                            }
                            else if (i == 9)
                            {
                                fields += "'-' AS ZP1" + i.ToString("00") + ",";
                            }
                            else if (i == 10)
                            {
                                fields += "'-' AS ZP1" + i.ToString("00") + ",";
                            }
                            else
                            {
                                fields += "CONVERT(DECIMAL(28,1),AVG(ZP1" + i.ToString("00") + ")) AS ZP1" + i.ToString("00") + ",";
                            }
                        }
                        break;
                    default:
                        for (int i = 1; i < 12; i++)
                        {
                            if (i == 3)
                            {
                            }
                            else if (i == 7)
                            {
                                fields += "TP_SCC.dbo.PHRASE_NAME('running', CONVERT(DECIMAL(28,1),ZP1" + i.ToString("00") + "), default) AS ZP1" + i.ToString("00") + ",";
                            }
                            else if (i == 8)
                            {
                                fields += "TP_SCC.dbo.PHRASE_NAME('function_fail', CONVERT(DECIMAL(28,1),ZP1" + i.ToString("00") + "), default) AS ZP1" + i.ToString("00") + ",";
                            }
                            else if (i == 9)
                            {
                                fields += "TP_SCC.dbo.PHRASE_NAME('switch_status', CONVERT(DECIMAL(28,1),ZP1" + i.ToString("00") + "), default) AS ZP1" + i.ToString("00") + ",";
                            }
                            else if (i == 10)
                            {
                                fields += "TP_SCC.dbo.PHRASE_NAME('open_close', CONVERT(DECIMAL(28,1),ZP1" + i.ToString("00") + "), default) AS ZP1" + i.ToString("00") + ",";
                            }
                            else
                            {
                                fields += "CONVERT(DECIMAL(28,1),ZP1" + i.ToString("00") + ") AS ZP1" + i.ToString("00") + ",";
                            }
                        }
                        break;
                }

                string groupByDT = null;
                string group = null;
                switch (req.GROUP_BY_DT)
                {
                    case "DETAIL":
                        groupByDT = "CONVERT(VARCHAR(20),CDATE,120)";
                        group = "";
                        break;
                    case "YEAR":
                        groupByDT = "CONVERT(VARCHAR(4),CDATE,120)";
                        group = string.Format("GROUP BY {0},LOCATION,DEVICE_ID", groupByDT);
                        break;
                    case "MONTH":
                        groupByDT = "CONVERT(VARCHAR(7),CDATE,120)";
                        group = string.Format("GROUP BY {0},LOCATION,DEVICE_ID", groupByDT);
                        break;
                    case "DAY":
                        groupByDT = "CONVERT(VARCHAR(10),CDATE,120)";
                        group = string.Format("GROUP BY {0},LOCATION,DEVICE_ID", groupByDT);
                        break;
                    case "HOUR":
                        groupByDT = "CONVERT(VARCHAR(13),CDATE,120)";
                        group = string.Format("GROUP BY {0},LOCATION,DEVICE_ID", groupByDT);
                        break;
                    default:
                        groupByDT = "CONVERT(VARCHAR(20),CDATE,120)";
                        group = "";
                        break;
                }

                //Db.AddInParameter(cmd, "TOP", DbType.Int32, 1000);

                string where = "";
                if (req.SDATE != null)
                {
                    where += " AND CDATE>=@SDATE";
                    Db.AddInParameter(cmd, "SDATE", DbType.DateTime, req.SDATE);
                }
                if (req.EDATE != null)
                {
                    where += " AND CDATE<=@EDATE";
                    Db.AddInParameter(cmd, "EDATE", DbType.DateTime, req.EDATE);
                }
                if (!string.IsNullOrEmpty(req.ZP1.LOCATION))
                {
                    where += " AND LOCATION=@LOCATION";
                    Db.AddInParameter(cmd, "LOCATION", DbType.String, req.ZP1.LOCATION);
                }
                if (!string.IsNullOrEmpty(req.ZP1.DEVICE_ID))
                {
                    where += " AND DEVICE_ID=@DEVICE_ID";
                    Db.AddInParameter(cmd, "DEVICE_ID", DbType.String, req.ZP1.DEVICE_ID);
                }
                if (where.Length > 0)
                {
                    where = " WHERE" + where.Substring(4);
                }

                sql = String.Format(sql, groupByDT, fields.TrimEnd(','), where, group);
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sql;

                using (IDataReader reader = Db.ExecuteReader(cmd))
                {
                    while (reader.Read())
                    {
                        var row = new ZP1Data
                        {
                            CDATE = reader["CDATE"] as string,
                            LOCATION = reader["LOCATION"] as string,
                            DEVICE_ID = reader["DEVICE_ID"] as string,
                            ZP101 = reader["ZP101"] as decimal? ?? null,
                            ZP102 = reader["ZP102"] as decimal? ?? null,
                            ZP104 = reader["ZP104"] as decimal? ?? null,
                            ZP105 = reader["ZP105"] as decimal? ?? null,
                            ZP106 = reader["ZP106"] as decimal? ?? null,
                            ZP107 = reader["ZP107"].ToString(),
                            ZP108 = reader["ZP108"].ToString(),
                            ZP109 = reader["ZP109"].ToString(),
                            ZP110 = reader["ZP110"].ToString(),
                            ZP111 = reader["ZP111"] as decimal? ?? null,
                        };
                        list.Add(row);
                    }
                }
            }
            if (list.Count > 0)
            {
                ms = ExcelProduce(req.GROUP_BY_DT, list);
            }
            return ms;
        }

        private MemoryStream ExcelProduce(string GroupByDt, List<ZP1Data> List)
        {
            MemoryStream ms = new MemoryStream();

            using (SpreadsheetDocument SpreadsheetDocument = SpreadsheetDocument.Create(ms, SpreadsheetDocumentType.Workbook))
            {
                WorkbookPart WorkbookPart = SpreadsheetDocument.AddWorkbookPart();
                WorkbookPart.Workbook = new Workbook();
                WorksheetPart WorksheetPart = WorkbookPart.AddNewPart<WorksheetPart>();
                //WorkbookStylesPart WorkbookStylesPart = WorkbookPart.AddNewPart<WorkbookStylesPart>();
                WorksheetPart.Worksheet = new Worksheet(new SheetData());
                Sheets Sheets = WorkbookPart.Workbook.AppendChild(new Sheets());

                Sheets.Append(new Sheet()
                {
                    Id = WorkbookPart.GetIdOfPart(WorksheetPart),
                    SheetId = 1,
                    Name = "Sheet 1"
                });

                var sheetData = WorksheetPart.Worksheet.GetFirstChild<SheetData>();

                var row = new Row();
                row.Append(
                    new Cell()
                    {
                        CellValue = new CellValue("日期"),
                        DataType = CellValues.String
                    },
                    new Cell()
                    {
                        CellValue = new CellValue("時間"),
                        DataType = CellValues.String
                    },
                    //new Cell()
                    //{
                    //    CellValue = new CellValue("位置"),
                    //    DataType = CellValues.String
                    //},
                    new Cell()
                    {
                        CellValue = new CellValue("設備名稱"),
                        DataType = CellValues.String
                    },
                    new Cell()
                    {
                        CellValue = new CellValue("冰水瞬間流量(lpm) (ZP1及ZP2總管)"),
                        DataType = CellValues.String
                    },
                    new Cell()
                    {
                        CellValue = new CellValue("冰水水路壓差 (ZP1及ZP2總管)"),
                        DataType = CellValues.String
                    },
                    new Cell()
                    {
                        CellValue = new CellValue("*冰水總出水溫(℃) (ZP1及ZP2總管)"),
                        DataType = CellValues.String
                    },
                    new Cell()
                    {
                        CellValue = new CellValue("*冰水總回水溫(℃) (ZP1及ZP2總管)"),
                        DataType = CellValues.String
                    },
                    new Cell()
                    {
                        CellValue = new CellValue("旁通控制閥閥位(%) (ZP1及ZP2總管)"),
                        DataType = CellValues.String
                    },
                    new Cell()
                    {
                        CellValue = new CellValue("運轉指示(On/OFF)"),
                        DataType = CellValues.String
                    },
                    new Cell()
                    {
                        CellValue = new CellValue("故障跳脫"),
                        DataType = CellValues.String
                    },
                    new Cell()
                    {
                        CellValue = new CellValue("旋鈕檔位狀態"),
                        DataType = CellValues.String
                    },
                    new Cell()
                    {
                        CellValue = new CellValue("*壓差開關"),
                        DataType = CellValues.String
                    },
                   new Cell()
                   {
                       CellValue = new CellValue("*變頻器運轉指示頻率(Hz)"),
                       DataType = CellValues.String
                   }
                );
                sheetData.AppendChild(row);

                foreach (ZP1Data data in List)
                {
                    //DateTime dt = DateTime.Parse(data.CDATE);
                    string date = "";
                    string time = "";
                    switch (GroupByDt)
                    {
                        case "DETAIL":
                            date = data.CDATE.Substring(0, 10);
                            time = data.CDATE.Substring(11, 5);
                            break;
                        case "YEAR":
                            date = data.CDATE.Substring(0, 4);
                            break;
                        case "MONTH":
                            date = data.CDATE.Substring(0, 7);
                            break;
                        case "DAY":
                            date = data.CDATE.Substring(0, 10);
                            break;
                        case "HOUR":
                            date = data.CDATE.Substring(0, 10);
                            time = data.CDATE.Substring(11, 2);
                            break;
                        default:
                            break;
                    }
                    row = new Row();
                    row.Append(
                        new Cell()
                        {
                            CellValue = new CellValue(date),
                            DataType = CellValues.String
                        },
                        new Cell()
                        {
                            CellValue = new CellValue(time),
                            DataType = CellValues.String
                        },
                        //new Cell()
                        //{
                        //    CellValue = new CellValue(data.LOCATION),
                        //    DataType = CellValues.String
                        //},
                        new Cell()
                        {
                            CellValue = new CellValue(data.DEVICE_ID),
                            DataType = CellValues.String
                        },
                        new Cell()
                        {
                            CellValue = new CellValue(data.ZP101.ToString()),
                            DataType = CellValues.Number
                        },
                        new Cell()
                        {
                            CellValue = new CellValue(data.ZP102.ToString()),
                            DataType = CellValues.Number
                        },
                        new Cell()
                        {
                            CellValue = new CellValue(data.ZP104.ToString()),
                            DataType = CellValues.Number
                        },
                        new Cell()
                        {
                            CellValue = new CellValue(data.ZP105.ToString()),
                            DataType = CellValues.Number
                        },
                        new Cell()
                        {
                            CellValue = new CellValue(data.ZP106.ToString()),
                            DataType = CellValues.Number
                        },
                        new Cell()
                        {
                            CellValue = new CellValue(data.ZP107),
                            DataType = CellValues.String
                        },
                        new Cell()
                        {
                            CellValue = new CellValue(data.ZP108),
                            DataType = CellValues.String
                        },
                        new Cell()
                        {
                            CellValue = new CellValue(data.ZP109),
                            DataType = CellValues.String
                        },
                        new Cell()
                        {
                            CellValue = new CellValue(data.ZP110),
                            DataType = CellValues.String
                        },
                        new Cell()
                        {
                            CellValue = new CellValue(data.ZP111.ToString()),
                            DataType = CellValues.Number
                        }
                    );
                    sheetData.AppendChild(row);
                }
            }

            return ms;
        }

        public ZP1GraphRes GraphRetrieve(ZP1GraphReq req)
        {
            ZP1GraphRes res = new ZP1GraphRes();

            List<ZP1ChartJsData> list = new List<ZP1ChartJsData>();

            using (DbCommand cmd = Db.CreateConnection().CreateCommand())
            {
                string sql = @"
SELECT {0} AS CDATE
    ,TP_SCC.dbo.PHRASE_NAME('ZP1_LOCATION',LOCATION,default) AS LOCATION
    ,TP_SCC.dbo.PHRASE_NAME('ZP1_DEVICE_ID',DEVICE_ID,LOCATION) AS DEVICE_ID
    ,{1}
    FROM ZP1
    {2}
    {3}
    {4}
";
                string field = string.Format("CONVERT(DECIMAL(28,1),AVG({0})) AS ZP1_VALUE", req.FIELD);

                string groupByDT = null;
                string group = null;
                switch (req.GROUP_BY_DT)
                {
                    case "DETAIL":
                        groupByDT = "CONVERT(VARCHAR(16),CDATE,120)";
                        group = string.Format("GROUP BY {0},LOCATION,DEVICE_ID", groupByDT);
                        break;
                    case "YEAR":
                        groupByDT = "CONVERT(VARCHAR(4),CDATE,120)";
                        group = string.Format("GROUP BY {0},LOCATION,DEVICE_ID", groupByDT);
                        break;
                    case "MONTH":
                        groupByDT = "CONVERT(VARCHAR(7),CDATE,120)";
                        group = string.Format("GROUP BY {0},LOCATION,DEVICE_ID", groupByDT);
                        break;
                    case "DAY":
                        groupByDT = "CONVERT(VARCHAR(10),CDATE,120)";
                        group = string.Format("GROUP BY {0},LOCATION,DEVICE_ID", groupByDT);
                        break;
                    case "HOUR":
                        groupByDT = "CONVERT(VARCHAR(13),CDATE,120)";
                        group = string.Format("GROUP BY {0},LOCATION,DEVICE_ID", groupByDT);
                        break;
                    default:
                        groupByDT = "CONVERT(VARCHAR(16),CDATE,120)";
                        group = "";
                        break;
                }

                string order = null;
                order = string.Format("ORDER BY LOCATION,DEVICE_ID,{0}", groupByDT);

                //Db.AddInParameter(cmd, "TOP", DbType.Int32, 1000);

                string where = "";
                if (req.SDATE != null)
                {
                    where += " AND CDATE>=@SDATE";
                    Db.AddInParameter(cmd, "SDATE", DbType.DateTime, req.SDATE);
                }
                if (req.EDATE != null)
                {
                    where += " AND CDATE<=@EDATE";
                    Db.AddInParameter(cmd, "EDATE", DbType.DateTime, req.EDATE);
                }
                if (!string.IsNullOrEmpty(req.ZP1.LOCATION))
                {
                    where += " AND LOCATION=@LOCATION";
                    Db.AddInParameter(cmd, "LOCATION", DbType.String, req.ZP1.LOCATION);
                }
                if (!string.IsNullOrEmpty(req.ZP1.DEVICE_ID))
                {
                    where += " AND DEVICE_ID=@DEVICE_ID";
                    Db.AddInParameter(cmd, "DEVICE_ID", DbType.String, req.ZP1.DEVICE_ID);
                }
                if (where.Length > 0)
                {
                    where = " WHERE" + where.Substring(4);
                }

                sql = String.Format(sql, groupByDT, field, where, group, order);

                string ClassName = this.GetType().Name;
                System.Diagnostics.StackTrace stackTrace = new System.Diagnostics.StackTrace();
                string CallerMethodName = stackTrace.GetFrame(1).GetMethod().Name;
                //Log(ClassName + "\\" + CallerMethodName, Text);
                TP_DSCCR.Models.Help.Logs.Write(ClassName + "\\" + CallerMethodName, sql);

                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sql;
                using (IDataReader reader = Db.ExecuteReader(cmd))
                {
                    while (reader.Read())
                    {
                        var row = new ZP1ChartJsData
                        {
                            CDATE = reader["CDATE"] as string,
                            LOCATION = reader["LOCATION"] as string,
                            DEVICE_ID = reader["DEVICE_ID"] as string,
                            VALUE = reader["ZP1_VALUE"] as decimal? ?? null
                            //VALUE = (Decimal)reader["ZP1_VALUE"]
                        };
                        list.Add(row);
                    }
                }
            }
            if (list.Count > 0)
            {
                res.Chart = ChartProduce(req.GRAPH_TYPE, list, req.FIELD_NAME, req.GROUP_BY_DT_NAME);
            }
            return res;
        }

        private Chart ChartProduce(string ChartType, List<ZP1ChartJsData> ZP1ChartJsData, string FieldName, string GroupName)
        {
            Chart Chart = new Chart();

            #region chart.type
            Chart.type = ChartType;
            #endregion

            #region chart.data
            TP_DSCCR.ViewModels.Data Data = new TP_DSCCR.ViewModels.Data();

            #region chart.data.labels
            var query = from data in ZP1ChartJsData
                        group data by data.CDATE;

            List<string> labels = new List<string>();
            foreach (var q in query)
            {
                labels.Add(q.Key);
            }
            Data.labels = labels;
            #endregion

            #region chart.data.datasets
            List<Dataset> datasets = new List<Dataset>();

            Random random = new Random();
            string key = null;
            Dataset ds = null;
            string rgb = null;
            foreach (ZP1ChartJsData ZP1ChartJS in ZP1ChartJsData)
            {
                if (key == null)
                {
                    //key = ZP1ChartJS.LOCATION + "_" + ZP1ChartJS.DEVICE_ID;
                    key = ZP1ChartJS.DEVICE_ID;
                    rgb = "rgb(" + random.Next(0, 255) + "," + random.Next(0, 255) + "," + random.Next(0, 255) + ")";
                    ds = new Dataset()
                    {
                        label = key,
                        fill = false,
                        backgroundColor = rgb,
                        borderColor = rgb,
                        data = new List<Decimal?> { }
                    };
                }
                else if (key != ZP1ChartJS.DEVICE_ID)
                {
                    datasets.Add(ds);
                    key = ZP1ChartJS.DEVICE_ID;
                    rgb = "rgb(" + random.Next(0, 255) + "," + random.Next(0, 255) + "," + random.Next(0, 255) + ")";
                    ds = new Dataset()
                    {
                        label = key,
                        fill = false,
                        backgroundColor = rgb,
                        borderColor = rgb,
                        data = new List<Decimal?> { }
                    };

                }
                ds.data.Add(ZP1ChartJS.VALUE);
            }
            datasets.Add(ds);
            Data.datasets = datasets;

            Chart.data = Data;
            #endregion

            #endregion

            #region chart.options

            Options Options = new Options()
            {
                responsive = true,
                maintainAspectRatio = false,
                title = new Title()
                {
                    display = true,
                    text = FieldName + " - " + GroupName
                },
                scales = new Scales()
                {
                    xAxes = new List<Axes>()
                    {
                        new Axes()
                        {
                            scaleLabel= new ScaleLabel()
                            {
                                display = true,
                                labelString = "時間間隔(" + GroupName +")"
                            }
                        }
                    },
                    yAxes = new List<Axes>()
                    {
                        new Axes()
                        {
                            scaleLabel= new ScaleLabel()
                            {
                                display = true,
                                labelString = FieldName
                            }
                        }
                    }
                }
            };

            Chart.options = Options;

            #endregion

            return Chart;
        }
    }
}