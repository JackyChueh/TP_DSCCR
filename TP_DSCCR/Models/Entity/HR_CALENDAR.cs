using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TP_DSCCR.Models.Entity
{
    public class HR_CALENDAR
    {
        public int? SN { get; set; }
        public DateTime? HR_DATE { get; set; }
        public string DATE_TYPE { get; set; }
        public string MEMO { get; set; }
        public DateTime? CDATE { get; set; }
        public string CUSER { get; set; }
        public DateTime? MDATE { get; set; }
        public string MUSER { get; set; }
    }
}