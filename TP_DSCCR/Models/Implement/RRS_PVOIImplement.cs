﻿using System;
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
    public class RRS_PVOIImplement : EnterpriseLibrary
    {
        public RRS_PVOIImplement(string connectionStringName) : base(connectionStringName) { }

        public RRS_PVOIDataRes PaginationRetrieve(RRS_PVOIDataReq req)
        {
            RRS_PVOIDataRes res = new RRS_PVOIDataRes()
            {
                RRS_PVOIData = new List<RRS_PVOIData>(),
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
    ,TP_SCC.dbo.PHRASE_NAME('RRS_PVOI_LOCATION',LOCATION,'RRS_PVOI') AS LOCATION
    ,TP_SCC.dbo.PHRASE_NAME('RRS_PVOI_DEVICE_ID',DEVICE_ID,LOCATION) AS DEVICE_ID
    ,{1}
    FROM RRS_PVOI
    {2}
    {3}
    ORDER BY CDATE
";
                string fields = "";
                switch (req.GROUP_BY_DT)
                {
                    case "DETAIL":
                        for (int i = 1; i < 8; i++)
                        {
                            {
                                fields += "TP_SCC.dbo.PHRASE_NAME('on_off', CONVERT(DECIMAL(28,0),RRS" + i.ToString("00") + "_PVOI01), default) AS RRS" + i.ToString("00") + "_PVOI01,";
                            }
                        }
                        break;
                    case "YEAR":
                    case "MONTH":
                    case "DAY":
                    case "HOUR":
                        for (int i = 1; i < 8; i++)
                        {
                            {
                                fields += "'-' AS RRS" + i.ToString("00") + "_PVOI01,";
                            }
                        }
                        break;
                    default:
                        for (int i = 1; i < 8; i++)
                        {
                            {
                                fields += "TP_SCC.dbo.PHRASE_NAME('on_off', CONVERT(DECIMAL(28,0),RRS" + i.ToString("00") + "_PVOI01), default) AS RRS" + i.ToString("00") + "_PVOI01,";
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
                if (!string.IsNullOrEmpty(req.RRS_PVOI.LOCATION))
                {
                    where += " AND LOCATION=@LOCATION";
                    Db.AddInParameter(cmd, "LOCATION", DbType.String, req.RRS_PVOI.LOCATION);
                }
                if (!string.IsNullOrEmpty(req.RRS_PVOI.DEVICE_ID))
                {
                    where += " AND DEVICE_ID=@DEVICE_ID";
                    Db.AddInParameter(cmd, "DEVICE_ID", DbType.String, req.RRS_PVOI.DEVICE_ID);
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
                            var row = new RRS_PVOIData
                            {
                                //SID = (int)dt.Rows[i]["SID"],
                                CDATE = dt.Rows[i]["CDATE"] as string,
                                //AUTOID = (int)dt.Rows[i]["AUTOID"],
                                //DATETIME = dt.Rows[i]["DATETIME"] as DateTime? ?? null,
                                LOCATION = dt.Rows[i]["LOCATION"] as string,
                                DEVICE_ID = dt.Rows[i]["DEVICE_ID"] as string,
                                RRS01_PVOI01 = dt.Rows[i]["RRS01_PVOI01"].ToString(),
                                RRS02_PVOI01 = dt.Rows[i]["RRS02_PVOI01"].ToString(),
                                RRS03_PVOI01 = dt.Rows[i]["RRS03_PVOI01"].ToString(),
                                RRS04_PVOI01 = dt.Rows[i]["RRS04_PVOI01"].ToString(),
                                RRS05_PVOI01 = dt.Rows[i]["RRS05_PVOI01"].ToString(),
                                RRS06_PVOI01 = dt.Rows[i]["RRS06_PVOI01"].ToString(),
                                RRS07_PVOI01 = dt.Rows[i]["RRS07_PVOI01"].ToString(),
                            };
                            res.RRS_PVOIData.Add(row);
                        }
                    }
                }
            }
            res.Pagination.EndTime = DateTime.Now;

            return res;
        }

        public MemoryStream ExcelRetrieve(RRS_PVOIExcelReq req)
        {
            MemoryStream ms = new MemoryStream();

            List<RRS_PVOIData> list = new List<RRS_PVOIData>();

            using (DbCommand cmd = Db.CreateConnection().CreateCommand())
            {
                string sql = @"
SELECT {0} AS CDATE
    ,TP_SCC.dbo.PHRASE_NAME('RRS_PVOI_LOCATION',LOCATION,'RRS_PVOI') AS LOCATION
    ,TP_SCC.dbo.PHRASE_NAME('RRS_PVOI_DEVICE_ID',DEVICE_ID,LOCATION) AS DEVICE_ID
    ,{1}
    FROM RRS_PVOI
    {2}
    {3}
    ORDER BY CDATE
";
                string fields = "";
                switch (req.GROUP_BY_DT)
                {
                    case "DETAIL":
                        for (int i = 1; i < 8; i++)
                        {
                            {
                                fields += "TP_SCC.dbo.PHRASE_NAME('on_off', CONVERT(DECIMAL(28,0),RRS" + i.ToString("00") + "_PVOI01), default) AS RRS" + i.ToString("00") + "_PVOI01,";
                            }
                        }
                        break;
                    case "YEAR":
                    case "MONTH":
                    case "DAY":
                    case "HOUR":
                        for (int i = 1; i < 8; i++)
                        {
                            {
                                fields += "'-' AS RRS" + i.ToString("00") + "_PVOI01,";
                            }
                        }
                        break;
                    default:
                        for (int i = 1; i < 8; i++)
                        {
                            {
                                fields += "TP_SCC.dbo.PHRASE_NAME('on_off', CONVERT(DECIMAL(28,0),RRS" + i.ToString("00") + "_PVOI01), default) AS RRS" + i.ToString("00") + "_PVOI01,";
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
                if (!string.IsNullOrEmpty(req.RRS_PVOI.LOCATION))
                {
                    where += " AND LOCATION=@LOCATION";
                    Db.AddInParameter(cmd, "LOCATION", DbType.String, req.RRS_PVOI.LOCATION);
                }
                if (!string.IsNullOrEmpty(req.RRS_PVOI.DEVICE_ID))
                {
                    where += " AND DEVICE_ID=@DEVICE_ID";
                    Db.AddInParameter(cmd, "DEVICE_ID", DbType.String, req.RRS_PVOI.DEVICE_ID);
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
                        var row = new RRS_PVOIData
                        {
                            CDATE = reader["CDATE"] as string,
                            LOCATION = reader["LOCATION"] as string,
                            DEVICE_ID = reader["DEVICE_ID"] as string,
                            RRS01_PVOI01 = reader["RRS01_PVOI01"].ToString(),
                            RRS02_PVOI01 = reader["RRS02_PVOI01"].ToString(),
                            RRS03_PVOI01 = reader["RRS03_PVOI01"].ToString(),
                            RRS04_PVOI01 = reader["RRS04_PVOI01"].ToString(),
                            RRS05_PVOI01 = reader["RRS05_PVOI01"].ToString(),
                            RRS06_PVOI01 = reader["RRS06_PVOI01"].ToString(),
                            RRS07_PVOI01 = reader["RRS07_PVOI01"].ToString(),
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

        private MemoryStream ExcelProduce(string GroupByDt, List<RRS_PVOIData> List)
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
                    //new Cell()
                    //{
                    //    CellValue = new CellValue("設備名稱"),
                    //    DataType = CellValues.String
                    //},
                    new Cell()
                    {
                        CellValue = new CellValue("電動閥控制運轉指示"),
                        DataType = CellValues.String
                    },
                    new Cell()
                    {
                        CellValue = new CellValue("花圃澆灌泵#1運轉指示(On/OFF)"),
                        DataType = CellValues.String
                    },
                    new Cell()
                    {
                        CellValue = new CellValue("花圃澆灌泵#2運轉指示(On/OFF)"),
                        DataType = CellValues.String
                    },
                    new Cell()
                    {
                        CellValue = new CellValue("副樓過濾泵#A運轉指示(On/OFF)"),
                        DataType = CellValues.String
                    },
                    new Cell()
                    {
                        CellValue = new CellValue("副樓過濾泵#B運轉指示(On/OFF)"),
                        DataType = CellValues.String
                    },
                    new Cell()
                    {
                        CellValue = new CellValue("副樓揚水泵#A運轉指示(On/OFF)"),
                        DataType = CellValues.String
                    },
                    new Cell()
                    {
                        CellValue = new CellValue("副樓揚水泵#B運轉指示(On/OFF)"),
                        DataType = CellValues.String
                    }
                );
                sheetData.AppendChild(row);

                foreach (RRS_PVOIData data in List)
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
                        //new Cell()
                        //{
                        //    CellValue = new CellValue(data.DEVICE_ID),
                        //    DataType = CellValues.String
                        //},
                        new Cell()
                        {
                            CellValue = new CellValue(data.RRS01_PVOI01),
                            DataType = CellValues.String
                        },
                        new Cell()
                        {
                            CellValue = new CellValue(data.RRS02_PVOI01),
                            DataType = CellValues.String
                        },
                        new Cell()
                        {
                            CellValue = new CellValue(data.RRS03_PVOI01),
                            DataType = CellValues.String
                        },
                        new Cell()
                        {
                            CellValue = new CellValue(data.RRS04_PVOI01),
                            DataType = CellValues.String
                        },
                        new Cell()
                        {
                            CellValue = new CellValue(data.RRS05_PVOI01),
                            DataType = CellValues.String
                        },
                        new Cell()
                        {
                            CellValue = new CellValue(data.RRS06_PVOI01),
                            DataType = CellValues.String
                        },
                        new Cell()
                        {
                            CellValue = new CellValue(data.RRS07_PVOI01),
                            DataType = CellValues.String
                        }
                    );
                    sheetData.AppendChild(row);
                }
            }

            return ms;
        }

        public RRS_PVOIGraphRes GraphRetrieve(RRS_PVOIGraphReq req)
        {
            RRS_PVOIGraphRes res = new RRS_PVOIGraphRes();

            List<RRS_PVOIChartJsData> list = new List<RRS_PVOIChartJsData>();

            using (DbCommand cmd = Db.CreateConnection().CreateCommand())
            {
                string sql = @"
SELECT {0} AS CDATE
    ,TP_SCC.dbo.PHRASE_NAME('RRS_PVOI_LOCATION',LOCATION,'RRS_PVOI') AS LOCATION
    ,TP_SCC.dbo.PHRASE_NAME('RRS_PVOI_DEVICE_ID',DEVICE_ID,LOCATION) AS DEVICE_ID
    ,{1}
    FROM RRS_PVOI
    {2}
    {3}
    {4}
";
                string field = string.Format("CONVERT(DECIMAL(28,1),AVG({0})) AS RRS_PVOI_VALUE", req.FIELD);

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
                if (!string.IsNullOrEmpty(req.RRS_PVOI.LOCATION))
                {
                    where += " AND LOCATION=@LOCATION";
                    Db.AddInParameter(cmd, "LOCATION", DbType.String, req.RRS_PVOI.LOCATION);
                }
                if (!string.IsNullOrEmpty(req.RRS_PVOI.DEVICE_ID))
                {
                    where += " AND DEVICE_ID=@DEVICE_ID";
                    Db.AddInParameter(cmd, "DEVICE_ID", DbType.String, req.RRS_PVOI.DEVICE_ID);
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
                        var row = new RRS_PVOIChartJsData
                        {
                            CDATE = reader["CDATE"] as string,
                            LOCATION = reader["LOCATION"] as string,
                            DEVICE_ID = reader["DEVICE_ID"] as string,
                            VALUE = reader["RRS_PVOI_VALUE"] as decimal? ?? null
                            //VALUE = (Decimal)reader["RRS_PVOI_VALUE"]
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

        private Chart ChartProduce(string ChartType, List<RRS_PVOIChartJsData> RRS_PVOIChartJsData, string FieldName, string GroupName)
        {
            Chart Chart = new Chart();

            #region chart.type
            Chart.type = ChartType;
            #endregion

            #region chart.data
            TP_DSCCR.ViewModels.Data Data = new TP_DSCCR.ViewModels.Data();

            #region chart.data.labels
            var query = from data in RRS_PVOIChartJsData
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
            foreach (RRS_PVOIChartJsData RRS_PVOIChartJS in RRS_PVOIChartJsData)
            {
                if (key == null)
                {
                    //key = RRS_PVOIChartJS.LOCATION + "_" + RRS_PVOIChartJS.DEVICE_ID;
                    key = RRS_PVOIChartJS.DEVICE_ID;
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
                else if (key != RRS_PVOIChartJS.DEVICE_ID)
                {
                    datasets.Add(ds);
                    key = RRS_PVOIChartJS.DEVICE_ID;
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
                ds.data.Add(RRS_PVOIChartJS.VALUE);
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