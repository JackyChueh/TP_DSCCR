using System;
using System.Collections.Generic;
using TP_DSCCR.Models.Entity;

namespace TP_DSCCR.ViewModels
{
    public class HR_CALENDARRetrieveReq
    {
        public HR_CALENDAR HR_CALENDAR { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public DateTime? HR_DATE_START { get; set; }
        public DateTime? HR_DATE_END { get; set; }
    }
    public class HR_CALENDARRetrieveRes : BaseResponse
    {
        public List<HR_CALENDAR> HR_CALENDAR { get; set; }
        public Pagination Pagination { get; set; }
    }

   public class HR_CALENDARModifyReq
    {
        public HR_CALENDAR HR_CALENDAR { get; set; }
    }
    public class HR_CALENDARModifyRes : BaseResponse
    {
        public HR_CALENDAR HR_CALENDAR { get; set; }
    }
}