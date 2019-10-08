using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TP_DSCCR.Models.Entity
{
    public class AHU
    {
        public int SID { get; set; }
        public DateTime CDATE { get; set; }
        public int AUTOID { get; set; }
        public DateTime? DATETIME { get; set; }
        public string LOCATION { get; set; }
        public string DEVICE_ID { get; set; }
        public Single? AHU01 { get; set; }
        public Single? AHU02 { get; set; }
        public Single? AHU03 { get; set; }
        public Single? AHU04 { get; set; }
        public Single? AHU05 { get; set; }
        public Single? AHU06 { get; set; }
        public Single? AHU07 { get; set; }
        public Single? AHU08 { get; set; }
        public Single? AHU09 { get; set; }
        public Single? AHU10 { get; set; }
        public Single? AHU11 { get; set; }
    }
}