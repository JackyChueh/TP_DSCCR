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
    public class ALERT_LOGImplement : EnterpriseLibrary
    {
        public ALERT_LOGImplement(string connectionStringName) : base(connectionStringName) { }

        public ALERT_LOGRetrieveRes PaginationRetrieve(ALERT_LOGRetrieveReq Req)
        {
            ALERT_LOGRetrieveRes res = new ALERT_LOGRetrieveRes()
            {
                ALERT_LOG = new List<ALERT_LOG_RETRIEVE>(),
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
SELECT SID, TP_SCC.dbo.PHRASE_NAME('DATA_TYPE',DATA_TYPE,default) AS DATA_TYPE
	,TP_SCC.dbo.PHRASE_NAME(DATA_TYPE+'_LOCATION',LOCATION,DATA_TYPE) AS LOCATION, TP_SCC.dbo.PHRASE_NAME(DATA_TYPE+'_DEVICE_ID',DEVICE_ID,LOCATION) AS DEVICE_ID
	, TP_SCC.dbo.PHRASE_NAME(DATA_TYPE+'_DATA_FIELD',DATA_FIELD,DATA_TYPE) AS DATA_FIELD
	, TP_SCC.dbo.DATA_NAME(DATA_TYPE,LOCATION,DATA_FIELD,MAX_VALUE) AS MAX_VALUE
    , TP_SCC.dbo.DATA_NAME(DATA_TYPE,LOCATION,DATA_FIELD,MIN_VALUE) AS MIN_VALUE
	, TP_SCC.dbo.DATA_NAME(DATA_TYPE,LOCATION,DATA_FIELD,ALERT_VALUE) AS ALERT_VALUE
    ,CHECK_DATE
FROM ALERT_LOG
    {0}
";
                string where = "";
                //Db.AddInParameter(cmd, "TOP", DbType.Int32, 1000);
                if (Req.SDATE != null)
                {
                    where += " AND CHECK_DATE>=@SDATE";
                    Db.AddInParameter(cmd, "SDATE", DbType.DateTime, Req.SDATE);
                }
                if (Req.EDATE != null)
                {
                    where += " AND CHECK_DATE<=@EDATE";
                    Db.AddInParameter(cmd, "EDATE", DbType.DateTime, Req.EDATE);
                }
                if (!string.IsNullOrEmpty(Req.ALERT_LOG.DATA_TYPE))
                {
                    where += " AND DATA_TYPE=@DATA_TYPE";
                    Db.AddInParameter(cmd, "DATA_TYPE", DbType.String, Req.ALERT_LOG.DATA_TYPE);
                }
                if (!string.IsNullOrEmpty(Req.ALERT_LOG.LOCATION))
                {
                    where += " AND LOCATION=@LOCATION";
                    Db.AddInParameter(cmd, "LOCATION", DbType.String, Req.ALERT_LOG.LOCATION);
                }
                if (!string.IsNullOrEmpty(Req.ALERT_LOG.DEVICE_ID))
                {
                    where += " AND DEVICE_ID=@DEVICE_ID";
                    Db.AddInParameter(cmd, "DEVICE_ID", DbType.String, Req.ALERT_LOG.DEVICE_ID);
                }
                if (!string.IsNullOrEmpty(Req.ALERT_LOG.DATA_FIELD))
                {
                    where += " AND DATA_FIELD=@DATA_FIELD";
                    Db.AddInParameter(cmd, "DATA_FIELD", DbType.String, Req.ALERT_LOG.DATA_FIELD);
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
                            ALERT_LOG_RETRIEVE row = new ALERT_LOG_RETRIEVE();
                            row.SID = (Int64)dt.Rows[i]["SID"];
                            row.DATA_TYPE = dt.Rows[i]["DATA_TYPE"] as string;
                            row.LOCATION = dt.Rows[i]["LOCATION"] as string;
                            row.DEVICE_ID = dt.Rows[i]["DEVICE_ID"] as string;
                            row.DATA_FIELD = dt.Rows[i]["DATA_FIELD"] as string;
                            row.MAX_VALUE = dt.Rows[i]["MAX_VALUE"] as string;
                            row.MIN_VALUE = dt.Rows[i]["MIN_VALUE"] as string;
                            row.ALERT_VALUE = dt.Rows[i]["ALERT_VALUE"] as string;
                            row.CHECK_DATE = dt.Rows[i]["CHECK_DATE"] as DateTime? ?? null;
                            res.ALERT_LOG.Add(row);
                        }
                    }
                }
            }
            res.Pagination.EndTime = DateTime.Now;

            return res;
        }
    }
}