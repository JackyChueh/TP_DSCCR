using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TP_DSCCR.Models.Entity;

namespace TP_DSCCR.ViewModels
{
    public class AHUReq
    {
        public AHU AHU { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
    public class AHURes : BaseResponse
    {
        public List<AHU> AHU { get; set; }
        public Pagination Pagination { get; set; }
    }
}