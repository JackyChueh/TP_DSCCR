using System;
using System.Collections.Generic;
using TP_DSCCR.Models.Entity;

namespace TP_DSCCR.ViewModels
{
    public class ALERT_CONFIGRetrieveReq
    {
        public ALERT_CONFIG ALERT_CONFIG { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public DateTime? HR_DATE_START { get; set; }
        public DateTime? HR_DATE_END { get; set; }
    }

    public class ALERT_CONFIG_RETRIEVE
    {
        public int? SID { get; set; }
        public string MODE { get; set; }
        public string DATA_TYPE { get; set; }
        public string LOCATION { get; set; }
        public string DEVICE_ID { get; set; }
        public string DATA_FIELD { get; set; }
        public string MAX_VALUE { get; set; }
        public string MIN_VALUE { get; set; }
        public int CHECK_INTERVAL { get; set; }
    }
    public class ALERT_CONFIGRetrieveRes : BaseResponse
    {
        public List<ALERT_CONFIG_RETRIEVE> ALERT_CONFIG { get; set; }
        public Pagination Pagination { get; set; }
    }

    public class ALERT_CONFIGModifyReq
    {
        public ALERT_CONFIG ALERT_CONFIG { get; set; }
    }
    public class ALERT_CONFIGModifyRes : BaseResponse
    {
        public ALERT_CONFIG ALERT_CONFIG { get; set; }
    }
}