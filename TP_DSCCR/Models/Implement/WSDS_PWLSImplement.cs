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
    public class WSDS_PWLSImplement : EnterpriseLibrary
    {
        public WSDS_PWLSImplement(string connectionStringName) : base(connectionStringName) { }

        public WSDS_PWLSDataRes PaginationRetrieve(WSDS_PWLSDataReq req)
        {
            WSDS_PWLSDataRes res = new WSDS_PWLSDataRes()
            {
                WSDS_PWLSData = new List<WSDS_PWLSData>(),
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
    ,TP_SCC.dbo.PHRASE_NAME('WSDS_PWLS_LOCATION',LOCATION,default) AS LOCATION
    ,TP_SCC.dbo.PHRASE_NAME('WSDS_PWLS_DEVICE_ID',DEVICE_ID,LOCATION) AS DEVICE_ID
    ,{1}
    FROM WSDS_PWLS
    {2}
    {3}
";
                string fields = "";
                switch (req.GROUP_BY_DT)
                {
                    case "DETAIL":
                        fields += "TP_SCC.dbo.PHRASE_NAME('up_down', CONVERT(DECIMAL(28,1),WSDS_PWLS_STATUS), default) AS WSDS_PWLS_STATUS,";
                        break;
                    case "YEAR":
                    case "MONTH":
                    case "DAY":
                    case "HOUR":
                        fields += "'-' AS WSDS_PWLS_STATUS,";
                        break;
                    default:
                        fields += "TP_SCC.dbo.PHRASE_NAME('up_down', CONVERT(DECIMAL(28,1),WSDS_PWLS_STATUS), default) AS WSDS_PWLS_STATUS,";
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
                if (!string.IsNullOrEmpty(req.WSDS_PWLS.LOCATION))
                {
                    where += " AND LOCATION=@LOCATION";
                    Db.AddInParameter(cmd, "LOCATION", DbType.String, req.WSDS_PWLS.LOCATION);
                }
                if (!string.IsNullOrEmpty(req.WSDS_PWLS.DEVICE_ID))
                {
                    where += " AND DEVICE_ID=@DEVICE_ID";
                    Db.AddInParameter(cmd, "DEVICE_ID", DbType.String, req.WSDS_PWLS.DEVICE_ID);
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
                            var row = new WSDS_PWLSData
                            {
                                //SID = (int)dt.Rows[i]["SID"],
                                CDATE = dt.Rows[i]["CDATE"] as string,
                                //AUTOID = (int)dt.Rows[i]["AUTOID"],
                                //DATETIME = dt.Rows[i]["DATETIME"] as DateTime? ?? null,
                                LOCATION = dt.Rows[i]["LOCATION"] as string,
                                DEVICE_ID = dt.Rows[i]["DEVICE_ID"] as string,
                                WSDS_PWLS_STATUS = dt.Rows[i]["WSDS_PWLS_STATUS"].ToString(),
                            };
                            res.WSDS_PWLSData.Add(row);
                        }
                    }
                }
            }
            res.Pagination.EndTime = DateTime.Now;

            return res;
        }

        public MemoryStream ExcelRetrieve(WSDS_PWLSExcelReq req)
        {
            MemoryStream ms = new MemoryStream();

            List<WSDS_PWLSData> list = new List<WSDS_PWLSData>();

            using (DbCommand cmd = Db.CreateConnection().CreateCommand())
            {
                string sql = @"
SELECT {0} AS CDATE
    ,TP_SCC.dbo.PHRASE_NAME('WSDS_PWLS_LOCATION',LOCATION,default) AS LOCATION
    ,TP_SCC.dbo.PHRASE_NAME('WSDS_PWLS_DEVICE_ID',DEVICE_ID,LOCATION) AS DEVICE_ID
    ,{1}
    FROM WSDS_PWLS
    {2}
    {3}
";
                string fields = "";
                switch (req.GROUP_BY_DT)
                {
                    case "DETAIL":
                        fields += "TP_SCC.dbo.PHRASE_NAME('up_down', CONVERT(DECIMAL(28,1),WSDS_PWLS_STATUS), default) AS WSDS_PWLS_STATUS,";
                        break;
                    case "YEAR":
                    case "MONTH":
                    case "DAY":
                    case "HOUR":
                        fields += "'-' AS WSDS_PWLS_STATUS,";
                        break;
                    default:
                        fields += "TP_SCC.dbo.PHRASE_NAME('up_down', CONVERT(DECIMAL(28,1),WSDS_PWLS_STATUS), default) AS WSDS_PWLS_STATUS,";
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
                if (!string.IsNullOrEmpty(req.WSDS_PWLS.LOCATION))
                {
                    where += " AND LOCATION=@LOCATION";
                    Db.AddInParameter(cmd, "LOCATION", DbType.String, req.WSDS_PWLS.LOCATION);
                }
                if (!string.IsNullOrEmpty(req.WSDS_PWLS.DEVICE_ID))
                {
                    where += " AND DEVICE_ID=@DEVICE_ID";
                    Db.AddInParameter(cmd, "DEVICE_ID", DbType.String, req.WSDS_PWLS.DEVICE_ID);
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
                        var row = new WSDS_PWLSData
                        {
                            CDATE = reader["CDATE"] as string,
                            LOCATION = reader["LOCATION"] as string,
                            DEVICE_ID = reader["DEVICE_ID"] as string,
                            WSDS_PWLS_STATUS = reader["WSDS_PWLS_STATUS"].ToString(),
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

        private MemoryStream ExcelProduce(string GroupByDt, List<WSDS_PWLSData> List)
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
                        CellValue = new CellValue("狀態"),
                        DataType = CellValues.String
                    }
                );
                sheetData.AppendChild(row);

                foreach (WSDS_PWLSData data in List)
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
                            CellValue = new CellValue(data.WSDS_PWLS_STATUS),
                            DataType = CellValues.String
                        }
                    );
                    sheetData.AppendChild(row);
                }
            }

            return ms;
        }

        public WSDS_PWLSGraphRes GraphRetrieve(WSDS_PWLSGraphReq req)
        {
            WSDS_PWLSGraphRes res = new WSDS_PWLSGraphRes();

            List<WSDS_PWLSChartJsData> list = new List<WSDS_PWLSChartJsData>();

            using (DbCommand cmd = Db.CreateConnection().CreateCommand())
            {
                string sql = @"
SELECT {0} AS CDATE
    ,TP_SCC.dbo.PHRASE_NAME('WSDS_PWLS_LOCATION',LOCATION,default) AS LOCATION
    ,TP_SCC.dbo.PHRASE_NAME('WSDS_PWLS_DEVICE_ID',DEVICE_ID,LOCATION) AS DEVICE_ID
    ,{1}
    FROM WSDS_PWLS
    {2}
    {3}
    {4}
";
                string field = string.Format("CONVERT(DECIMAL(28,1),AVG({0})) AS WSDS_PWLS_VALUE", req.FIELD);

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
                if (!string.IsNullOrEmpty(req.WSDS_PWLS.LOCATION))
                {
                    where += " AND LOCATION=@LOCATION";
                    Db.AddInParameter(cmd, "LOCATION", DbType.String, req.WSDS_PWLS.LOCATION);
                }
                if (!string.IsNullOrEmpty(req.WSDS_PWLS.DEVICE_ID))
                {
                    where += " AND DEVICE_ID=@DEVICE_ID";
                    Db.AddInParameter(cmd, "DEVICE_ID", DbType.String, req.WSDS_PWLS.DEVICE_ID);
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
                        var row = new WSDS_PWLSChartJsData
                        {
                            CDATE = reader["CDATE"] as string,
                            LOCATION = reader["LOCATION"] as string,
                            DEVICE_ID = reader["DEVICE_ID"] as string,
                            VALUE = reader["WSDS_PWLS_VALUE"] as decimal? ?? null
                            //VALUE = (Decimal)reader["WSDS_PWLS_VALUE"]
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

        private Chart ChartProduce(string ChartType, List<WSDS_PWLSChartJsData> WSDS_PWLSChartJsData, string FieldName, string GroupName)
        {
            Chart Chart = new Chart();

            #region chart.type
            Chart.type = ChartType;
            #endregion

            #region chart.data
            TP_DSCCR.ViewModels.Data Data = new TP_DSCCR.ViewModels.Data();

            #region chart.data.labels
            var query = from data in WSDS_PWLSChartJsData
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
            foreach (WSDS_PWLSChartJsData WSDS_PWLSChartJS in WSDS_PWLSChartJsData)
            {
                if (key == null)
                {
                    //key = WSDS_PWLSChartJS.LOCATION + "_" + WSDS_PWLSChartJS.DEVICE_ID;
                    key = WSDS_PWLSChartJS.DEVICE_ID;
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
                else if (key != WSDS_PWLSChartJS.DEVICE_ID)
                {
                    datasets.Add(ds);
                    key = WSDS_PWLSChartJS.DEVICE_ID;
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
                ds.data.Add(WSDS_PWLSChartJS.VALUE);
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