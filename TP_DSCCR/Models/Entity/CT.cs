using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TP_DSCCR.Models.Entity
{
    public class CT
    {
        public int SID { get; set; }
        public DateTime CDATE { get; set; }
        public int AUTOID { get; set; }
        public DateTime? DATETIME { get; set; }
        public string LOCATION { get; set; }
        public string DEVICE_ID { get; set; }
        public Single? CT01 { get; set; }
        public Single? CT02 { get; set; }
        public Single? CT03 { get; set; }
        public Single? CT04 { get; set; }
        public Single? CT05 { get; set; }
        public Single? CT06 { get; set; }
        public Single? CT07 { get; set; }
    }

}