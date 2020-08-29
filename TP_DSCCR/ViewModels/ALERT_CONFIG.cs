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
    public class ALERT_CONFIGRetrieveRes : BaseResponse
    {
        public List<ALERT_CONFIG> ALERT_CONFIG { get; set; }
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