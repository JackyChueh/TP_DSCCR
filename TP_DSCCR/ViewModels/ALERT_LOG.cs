using System;
using System.Collections.Generic;
using TP_DSCCR.Models.Entity;

namespace TP_DSCCR.ViewModels
{
    public class ALERT_LOGData
    {
        public Int64 SID { get; set; }
        public string DATA_TYPE { get; set; }
        public string LOCATION { get; set; }
        public string DEVICE_ID { get; set; }
        public string DATA_FIELD { get; set; }
        public string MAX_VALUE { get; set; }
        public string MIN_VALUE { get; set; }
        public string ALERT_VALUE { get; set; }
        public DateTime CHECK_DATE { get; set; }
    }

    public class ALERT_LOGRetrieveReq
    {
        public ALERT_LOG ALERT_LOG { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public DateTime? SDATE { get; set; }
        public DateTime? EDATE { get; set; }
    }

    public class ALERT_LOG_RETRIEVE
    {
        public Int64? SID { get; set; }
        public string DATA_TYPE { get; set; }
        public string LOCATION { get; set; }
        public string DEVICE_ID { get; set; }
        public string DATA_FIELD { get; set; }
        public string MAX_VALUE { get; set; }
        public string MIN_VALUE { get; set; }
        public string ALERT_VALUE { get; set; }
        public DateTime? CHECK_DATE { get; set; }
    }
    public class ALERT_LOGRetrieveRes : BaseResponse
    {
        public List<ALERT_LOG_RETRIEVE> ALERT_LOG { get; set; }
        public Pagination Pagination { get; set; }
    }

    public class ALERT_LOGModifyReq
    {
        public ALERT_LOG ALERT_LOG { get; set; }
    }
    public class ALERT_LOGModifyRes : BaseResponse
    {
        public ALERT_LOG ALERT_LOG { get; set; }
    }

    public class ALERT_LOGExcelReq
    {
        public ALERT_LOG ALERT_LOG { get; set; }
        public DateTime? SDATE { get; set; }
        public DateTime? EDATE { get; set; }
    }
    public class ALERT_LOGExcelRes : BaseResponse
    {
        public string DataId { get; set; }
        public string FileName { get; set; }

    }
}