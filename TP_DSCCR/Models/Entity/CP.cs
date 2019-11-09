using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TP_DSCCR.Models.Entity
{
    public class CP
    {
        public int SID { get; set; }
        public DateTime CDATE { get; set; }
        public int AUTOID { get; set; }
        public DateTime? DATETIME { get; set; }
        public string LOCATION { get; set; }
        public string DEVICE_ID { get; set; }
        public Single? CP01 { get; set; }
        public Single? CP02 { get; set; }
        public Single? CP03 { get; set; }
        public Single? CP04 { get; set; }
        public Single? CP05 { get; set; }
        public Single? CP06 { get; set; }
        public Single? CP07 { get; set; }
    }

}