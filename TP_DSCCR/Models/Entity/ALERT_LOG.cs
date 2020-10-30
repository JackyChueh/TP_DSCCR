using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TP_DSCCR.Models.Entity
{
    public class ALERT_LOG
    {
        public int? SID { get; set; }
        public string DATA_TYPE { get; set; }
        public string LOCATION { get; set; }
        public string DEVICE_ID { get; set; }
        public string DATA_FIELD { get; set; }
        public Single? MAX_VALUE { get; set; }
        public Single? MIN_VALUE { get; set; }
        public Single? ALERT_VALUE { get; set; }
        public DateTime? CHECK_DATE { get; set; }
    }
}