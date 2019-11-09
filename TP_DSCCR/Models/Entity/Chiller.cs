using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TP_DSCCR.Models.Entity
{
    public class Chiller
    {
        public int SID { get; set; }
        public DateTime CDATE { get; set; }
        public int AUTOID { get; set; }
        public DateTime? DATETIME { get; set; }
        public string LOCATION { get; set; }
        public string DEVICE_ID { get; set; }
        public Single? Chiller01 { get; set; }
        public Single? Chiller02 { get; set; }
        public Single? Chiller03 { get; set; }
        public Single? Chiller04 { get; set; }
        public Single? Chiller05 { get; set; }
        public Single? Chiller06 { get; set; }
        public Single? Chiller07 { get; set; }
        public Single? Chiller08 { get; set; }
        public Single? Chiller09 { get; set; }
        public Single? Chiller10 { get; set; }
    }

}