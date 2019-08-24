using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TP_DSCCR.Models.Entity
{
    /// <summary>
    /// 系統功能
    /// </summary>
    public class FUNCTIONS
    {
        public int? ROWNUM { get; set; }
        public Int16? SN { get; set; }
        public string NAME { get; set; }
        public string MODE { get; set; }
        public string CATEGORY { get; set; }
        public string URL { get; set; }
        public Int16? PARENT_SN { get; set; }
        public Int16? SORT { get; set; }
        public string IMG { get; set; }
        public DateTime? CDATE { get; set; }
        public Int16? CUSER { get; set; }
        public DateTime? MDATE { get; set; }
        public Int16? MUSER { get; set; }
    }

}