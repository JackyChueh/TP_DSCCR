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
    public class MSPCSTATSImplement : EnterpriseLibrary
    {
        public MSPCSTATSImplement(string connectionStringName) : base(connectionStringName) { }

        public MSPCSTATSDataRes PaginationRetrieve(MSPCSTATSDataReq req)
        {
            MSPCSTATSDataRes res = new MSPCSTATSDataRes()
            {
                MSPCSTATSData = new List<MSPCSTATSData>(),
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
    ,TP_SCC.dbo.PHRASE_NAME('MSPCSTATS_LOCATION',LOCATION,'MSPCSTATS') AS LOCATION
    ,TP_SCC.dbo.PHRASE_NAME('MSPCSTATS_DEVICE_ID',DEVICE_ID,LOCATION) AS DEVICE_ID
    ,TP_SCC.dbo.PHRASE_NAME('WATER_TOWER',WATER_TOWER,default) AS WATER_TOWER
    ,{1}
    FROM MSPCSTATS
    {2}
    {3}
    ODER BY CDATE
";
                string fields = "";
                switch (req.GROUP_BY_DT)
                {
                    case "DETAIL":
                        for (int i = 1; i < 9; i++)
                        {
                            if (i < 9)
                            {
                                fields += "TP_SCC.dbo.PHRASE_NAME('on_off', CONVERT(DECIMAL(28,0),SEF" + i.ToString("00") + "), default) AS SEF" + i.ToString("00") + ",";
                            }
                            else
                            {
                                fields += "CONVERT(DECIMAL(28,1),SEF" + i.ToString("00") + ") AS SEF" + i.ToString("00") + ",";
                            }
                        }
                        break;
                    case "YEAR":
                    case "MONTH":
                    case "DAY":
                    case "HOUR":
                        for (int i = 1; i < 9; i++)
                        {
                            if (i < 9)
                            {
                                fields += "'-' AS SEF" + i.ToString("00") + ",";
                            }
                            else
                            {
                                fields += "CONVERT(DECIMAL(28,1),AVG(SEF" + i.ToString("00") + ")) AS SEF" + i.ToString("00") + ",";
                            }
                        }
                        break;
                    default:
                        for (int i = 1; i < 9; i++)
                        {
                            if (i < 9)
                            {
                                fields += "TP_SCC.dbo.PHRASE_NAME('on_off', CONVERT(DECIMAL(28,0),SEF" + i.ToString("00") + "), default) AS SEF" + i.ToString("00") + ",";
                            }
                            else
                            {
                                fields += "CONVERT(DECIMAL(28,1),SEF" + i.ToString("00") + ") AS SEF" + i.ToString("00") + ",";
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
                        group = string.Format("GROUP BY {0},LOCATION,DEVICE_ID,WATER_TOWER", groupByDT);
                        break;
                    case "MONTH":
                        groupByDT = "CONVERT(VARCHAR(7),CDATE,120)";
                        group = string.Format("GROUP BY {0},LOCATION,DEVICE_ID,WATER_TOWER", groupByDT);
                        break;
                    case "DAY":
                        groupByDT = "CONVERT(VARCHAR(10),CDATE,120)";
                        group = string.Format("GROUP BY {0},LOCATION,DEVICE_ID,WATER_TOWER", groupByDT);
                        break;
                    case "HOUR":
                        groupByDT = "CONVERT(VARCHAR(13),CDATE,120)";
                        group = string.Format("GROUP BY {0},LOCATION,DEVICE_ID,WATER_TOWER", groupByDT);
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
                if (!string.IsNullOrEmpty(req.MSPCSTATS.LOCATION))
                {
                    where += " AND LOCATION=@LOCATION";
                    Db.AddInParameter(cmd, "LOCATION", DbType.String, req.MSPCSTATS.LOCATION);
                }
                if (!string.IsNullOrEmpty(req.MSPCSTATS.DEVICE_ID))
                {
                    where += " AND DEVICE_ID=@DEVICE_ID";
                    Db.AddInParameter(cmd, "DEVICE_ID", DbType.String, req.MSPCSTATS.DEVICE_ID);
                }
                if (!string.IsNullOrEmpty(req.MSPCSTATS.WATER_TOWER))
                {
                    where += " AND WATER_TOWER=@WATER_TOWER";
                    Db.AddInParameter(cmd, "WATER_TOWER", DbType.String, req.MSPCSTATS.WATER_TOWER);
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
                            var row = new MSPCSTATSData
                            {
                                //SID = (int)dt.Rows[i]["SID"],
                                CDATE = dt.Rows[i]["CDATE"] as string,
                                //AUTOID = (int)dt.Rows[i]["AUTOID"],
                                //DATETIME = dt.Rows[i]["DATETIME"] as DateTime? ?? null,
                                LOCATION = dt.Rows[i]["LOCATION"] as string,
                                DEVICE_ID = dt.Rows[i]["DEVICE_ID"] as string,
                                WATER_TOWER = dt.Rows[i]["WATER_TOWER"] as string,
                                SEF01 = dt.Rows[i]["SEF01"].ToString(),
                                SEF02 = dt.Rows[i]["SEF02"].ToString(),
                                SEF03 = dt.Rows[i]["SEF03"].ToString(),
                                SEF04 = dt.Rows[i]["SEF04"].ToString(),
                                SEF05 = dt.Rows[i]["SEF05"].ToString(),
                                SEF06 = dt.Rows[i]["SEF06"].ToString(),
                                SEF07 = dt.Rows[i]["SEF07"].ToString(),
                                SEF08 = dt.Rows[i]["SEF08"].ToString()
                            };
                            res.MSPCSTATSData.Add(row);
                        }
                    }
                }
            }
            res.Pagination.EndTime = DateTime.Now;

            return res;
        }

        public MemoryStream ExcelRetrieve(MSPCSTATSExcelReq req)
        {
            MemoryStream ms = new MemoryStream();

            List<MSPCSTATSData> list = new List<MSPCSTATSData>();

            using (DbCommand cmd = Db.CreateConnection().CreateCommand())
            {
                string sql = @"
SELECT {0} AS CDATE
    ,TP_SCC.dbo.PHRASE_NAME('MSPCSTATS_LOCATION',LOCATION,'MSPCSTATS') AS LOCATION
    ,TP_SCC.dbo.PHRASE_NAME('MSPCSTATS_DEVICE_ID',DEVICE_ID,LOCATION) AS DEVICE_ID
    ,TP_SCC.dbo.PHRASE_NAME('WATER_TOWER',WATER_TOWER,default) AS WATER_TOWER
    ,{1}
    FROM MSPCSTATS
    {2}
    {3}
    ODER BY CDATE
";
                string fields = "";
                switch (req.GROUP_BY_DT)
                {
                    case "DETAIL":
                        for (int i = 1; i < 9; i++)
                        {
                            if (i < 9)
                            {
                                fields += "TP_SCC.dbo.PHRASE_NAME('on_off', CONVERT(DECIMAL(28,0),SEF" + i.ToString("00") + "), default) AS SEF" + i.ToString("00") + ",";
                            }
                            else
                            {
                                fields += "CONVERT(DECIMAL(28,1),SEF" + i.ToString("00") + ") AS SEF" + i.ToString("00") + ",";
                            }
                        }
                        break;
                    case "YEAR":
                    case "MONTH":
                    case "DAY":
                    case "HOUR":
                        for (int i = 1; i < 9; i++)
                        {
                            if (i < 9)
                            {
                                fields += "'-' AS SEF" + i.ToString("00") + ",";
                            }
                            else
                            {
                                fields += "CONVERT(DECIMAL(28,1),AVG(SEF" + i.ToString("00") + ")) AS SEF" + i.ToString("00") + ",";
                            }
                        }
                        break;
                    default:
                        for (int i = 1; i < 9; i++)
                        {
                            if (i < 9)
                            {
                                fields += "TP_SCC.dbo.PHRASE_NAME('on_off', CONVERT(DECIMAL(28,0),SEF" + i.ToString("00") + "), default) AS SEF" + i.ToString("00") + ",";
                            }
                            else
                            {
                                fields += "CONVERT(DECIMAL(28,1),SEF" + i.ToString("00") + ") AS SEF" + i.ToString("00") + ",";
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
                        group = string.Format("GROUP BY {0},LOCATION,DEVICE_ID,WATER_TOWER", groupByDT);
                        break;
                    case "MONTH":
                        groupByDT = "CONVERT(VARCHAR(7),CDATE,120)";
                        group = string.Format("GROUP BY {0},LOCATION,DEVICE_ID,WATER_TOWER", groupByDT);
                        break;
                    case "DAY":
                        groupByDT = "CONVERT(VARCHAR(10),CDATE,120)";
                        group = string.Format("GROUP BY {0},LOCATION,DEVICE_ID,WATER_TOWER", groupByDT);
                        break;
                    case "HOUR":
                        groupByDT = "CONVERT(VARCHAR(13),CDATE,120)";
                        group = string.Format("GROUP BY {0},LOCATION,DEVICE_ID,WATER_TOWER", groupByDT);
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
                if (!string.IsNullOrEmpty(req.MSPCSTATS.LOCATION))
                {
                    where += " AND LOCATION=@LOCATION";
                    Db.AddInParameter(cmd, "LOCATION", DbType.String, req.MSPCSTATS.LOCATION);
                }
                if (!string.IsNullOrEmpty(req.MSPCSTATS.DEVICE_ID))
                {
                    where += " AND DEVICE_ID=@DEVICE_ID";
                    Db.AddInParameter(cmd, "DEVICE_ID", DbType.String, req.MSPCSTATS.DEVICE_ID);
                }
                if (!string.IsNullOrEmpty(req.MSPCSTATS.WATER_TOWER))
                {
                    where += " AND WATER_TOWER=@WATER_TOWER";
                    Db.AddInParameter(cmd, "WATER_TOWER", DbType.String, req.MSPCSTATS.WATER_TOWER);
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
                        var row = new MSPCSTATSData
                        {
                            CDATE = reader["CDATE"] as string,
                            LOCATION = reader["LOCATION"] as string,
                            DEVICE_ID = reader["DEVICE_ID"] as string,
                            WATER_TOWER = reader["WATER_TOWER"] as string,
                            SEF01 = reader["SEF01"].ToString(),
                            SEF02 = reader["SEF02"].ToString(),
                            SEF04 = reader["SEF04"].ToString(),
                            SEF05 = reader["SEF05"].ToString(),
                            SEF06 = reader["SEF06"].ToString(),
                            SEF07 = reader["SEF07"].ToString(),
                            SEF08 = reader["SEF08"].ToString()
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

        private MemoryStream ExcelProduce(string GroupByDt, List<MSPCSTATSData> List)
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
                    new Cell()
                    {
                        CellValue = new CellValue("位置"),
                        DataType = CellValues.String
                    },
                    new Cell()
                    {
                        CellValue = new CellValue("設備名稱"),
                        DataType = CellValues.String
                    },
                    new Cell()
                    {
                        CellValue = new CellValue("*時間控制(On/OFF)"),
                        DataType = CellValues.String
                    },
                    new Cell()
                    {
                        CellValue = new CellValue("*溫度控制(On/OFF)"),
                        DataType = CellValues.String
                    },
                    new Cell()
                    {
                        CellValue = new CellValue("*旋鈕檔位狀態"),
                        DataType = CellValues.String
                    },
                    new Cell()
                    {
                        CellValue = new CellValue("電源運轉指示(On/OFF)"),
                        DataType = CellValues.String
                    },
                    new Cell()
                    {
                        CellValue = new CellValue("*運轉指示(On/OFF)"),
                        DataType = CellValues.String
                    },
                    new Cell()
                    {
                        CellValue = new CellValue("*運轉指示(On/OFF)(第1壓縮機)"),
                        DataType = CellValues.String
                    },
                    new Cell()
                    {
                        CellValue = new CellValue("*運轉指示(On/OFF)(第2壓縮機)"),
                        DataType = CellValues.String
                    },
                    new Cell()
                    {
                        CellValue = new CellValue("*風扇運轉指示(On/OFF)"),
                        DataType = CellValues.String
                    }
                );
                sheetData.AppendChild(row);

                foreach (MSPCSTATSData data in List)
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
                        new Cell()
                        {
                            CellValue = new CellValue(data.LOCATION),
                            DataType = CellValues.String
                        },
                        new Cell()
                        {
                            CellValue = new CellValue(data.DEVICE_ID),
                            DataType = CellValues.String
                        },
                        new Cell()
                        {
                            CellValue = new CellValue(data.SEF01),
                            DataType = CellValues.String
                        },
                        new Cell()
                        {
                            CellValue = new CellValue(data.SEF02),
                            DataType = CellValues.String
                        },
                        new Cell()
                        {
                            CellValue = new CellValue(data.SEF03),
                            DataType = CellValues.String
                        },
                        new Cell()
                        {
                            CellValue = new CellValue(data.SEF04),
                            DataType = CellValues.String
                        },
                        new Cell()
                        {
                            CellValue = new CellValue(data.SEF05),
                            DataType = CellValues.String
                        },
                        new Cell()
                        {
                            CellValue = new CellValue(data.SEF06),
                            DataType = CellValues.String
                        },
                        new Cell()
                        {
                            CellValue = new CellValue(data.SEF07),
                            DataType = CellValues.String
                        },
                        new Cell()
                        {
                            CellValue = new CellValue(data.SEF08),
                            DataType = CellValues.String
                        }
                    );
                    sheetData.AppendChild(row);
                }
            }

            return ms;
        }

        public MSPCSTATSGraphRes GraphRetrieve(MSPCSTATSGraphReq req)
        {
            MSPCSTATSGraphRes res = new MSPCSTATSGraphRes();

            List<MSPCSTATSChartJsData> list = new List<MSPCSTATSChartJsData>();

            using (DbCommand cmd = Db.CreateConnection().CreateCommand())
            {
                string sql = @"
SELECT {0} AS CDATE
    ,TP_SCC.dbo.PHRASE_NAME('MSPCSTATS_LOCATION',LOCATION,'MSPCSTATS') AS LOCATION
    ,TP_SCC.dbo.PHRASE_NAME('MSPCSTATS_DEVICE_ID',DEVICE_ID,LOCATION) AS DEVICE_ID
    ,TP_SCC.dbo.PHRASE_NAME('WATER_TOWER',WATER_TOWER,default) AS WATER_TOWER
    ,{1}
    FROM MSPCSTATS
    {2}
    {3}
    {4}
";
                string field = string.Format("CONVERT(DECIMAL(28,1),AVG({0})) AS MSPCSTATS_VALUE", req.FIELD);

                string groupByDT = null;
                string group = null;
                switch (req.GROUP_BY_DT)
                {
                    case "DETAIL":
                        groupByDT = "CONVERT(VARCHAR(16),CDATE,120)";
                        group = string.Format("GROUP BY {0},LOCATION,DEVICE_ID,WATER_TOWER", groupByDT);
                        break;
                    case "YEAR":
                        groupByDT = "CONVERT(VARCHAR(4),CDATE,120)";
                        group = string.Format("GROUP BY {0},LOCATION,DEVICE_ID,WATER_TOWER", groupByDT);
                        break;
                    case "MONTH":
                        groupByDT = "CONVERT(VARCHAR(7),CDATE,120)";
                        group = string.Format("GROUP BY {0},LOCATION,DEVICE_ID,WATER_TOWER", groupByDT);
                        break;
                    case "DAY":
                        groupByDT = "CONVERT(VARCHAR(10),CDATE,120)";
                        group = string.Format("GROUP BY {0},LOCATION,DEVICE_ID,WATER_TOWER", groupByDT);
                        break;
                    case "HOUR":
                        groupByDT = "CONVERT(VARCHAR(13),CDATE,120)";
                        group = string.Format("GROUP BY {0},LOCATION,DEVICE_ID,WATER_TOWER", groupByDT);
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
                if (!string.IsNullOrEmpty(req.MSPCSTATS.LOCATION))
                {
                    where += " AND LOCATION=@LOCATION";
                    Db.AddInParameter(cmd, "LOCATION", DbType.String, req.MSPCSTATS.LOCATION);
                }
                if (!string.IsNullOrEmpty(req.MSPCSTATS.DEVICE_ID))
                {
                    where += " AND DEVICE_ID=@DEVICE_ID";
                    Db.AddInParameter(cmd, "DEVICE_ID", DbType.String, req.MSPCSTATS.DEVICE_ID);
                }
                if (!string.IsNullOrEmpty(req.MSPCSTATS.WATER_TOWER))
                {
                    where += " AND WATER_TOWER=@WATER_TOWER";
                    Db.AddInParameter(cmd, "WATER_TOWER", DbType.String, req.MSPCSTATS.WATER_TOWER);
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
                        var row = new MSPCSTATSChartJsData
                        {
                            CDATE = reader["CDATE"] as string,
                            LOCATION = reader["LOCATION"] as string,
                            DEVICE_ID = reader["DEVICE_ID"] as string,
                            VALUE = reader["MSPCSTATS_VALUE"] as decimal? ?? null
                            //VALUE = (Decimal)reader["MSPCSTATS_VALUE"]
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

        private Chart ChartProduce(string ChartType, List<MSPCSTATSChartJsData> MSPCSTATSChartJsData, string FieldName, string GroupName)
        {
            Chart Chart = new Chart();

            #region chart.type
            Chart.type = ChartType;
            #endregion

            #region chart.data
            TP_DSCCR.ViewModels.Data Data = new TP_DSCCR.ViewModels.Data();

            #region chart.data.labels
            var query = from data in MSPCSTATSChartJsData
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
            foreach (MSPCSTATSChartJsData MSPCSTATSChartJS in MSPCSTATSChartJsData)
            {
                if (key == null)
                {
                    //key = MSPCSTATSChartJS.LOCATION + "_" + MSPCSTATSChartJS.DEVICE_ID;
                    key = MSPCSTATSChartJS.DEVICE_ID;
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
                else if (key != MSPCSTATSChartJS.DEVICE_ID)
                {
                    datasets.Add(ds);
                    key = MSPCSTATSChartJS.DEVICE_ID;
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
                ds.data.Add(MSPCSTATSChartJS.VALUE);
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