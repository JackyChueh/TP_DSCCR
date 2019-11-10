using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TP_DSCCR.Models.Entity
{
    public class COP
    {
        public int SID { get; set; }
        public DateTime CDATE { get; set; }
        public int AUTOID { get; set; }
        public DateTime? DATETIME { get; set; }
        public string LOCATION { get; set; }
        public string DEVICE_ID { get; set; }
        public Single? COP01 { get; set; }
        public Single? COP02 { get; set; }
        public Single? COP03 { get; set; }
        public Single? COP04 { get; set; }
        public Single? COP05 { get; set; }
    }

}