using System;

namespace TP_DSCCR.Models.Entity
{
    public class ALERT_CONFIG
    {
        public int? SID { get; set; }
        public string MODE { get; set; }
        public string DATA_TYPE { get; set; }
        public string LOCATION { get; set; }
        public string DEVICE_ID { get; set; }
        public string DATA_FIELD { get; set; }
        public Single? MAX_VALUE { get; set; }
        public Single? MIN_VALUE { get; set; }
        public int? CHECK_INTERVAL { get; set; }
        public int? ALERT_INTERVAL { get; set; }
        public bool SUN { get; set; }
        public TimeSpan? SUN_STIME { get; set; }
        public TimeSpan? SUN_ETIME { get; set; }
        public bool MON { get; set; }
        public TimeSpan? MON_STIME { get; set; }
        public TimeSpan? MON_ETIME { get; set; }
        public bool TUE { get; set; }
        public TimeSpan? TUE_STIME { get; set; }
        public TimeSpan? TUE_ETIME { get; set; }
        public bool WED { get; set; }
        public TimeSpan? WED_STIME { get; set; }
        public TimeSpan? WED_ETIME { get; set; }
        public bool THU { get; set; }
        public TimeSpan? THU_STIME { get; set; }
        public TimeSpan? THU_ETIME { get; set; }
        public bool FRI { get; set; }
        public TimeSpan? FRI_STIME { get; set; }
        public TimeSpan? FRI_ETIME { get; set; }
        public bool STA { get; set; }
        public TimeSpan? STA_STIME { get; set; }
        public TimeSpan? STA_ETIME { get; set; }
        public DateTime? CHECK_DATE { get; set; }
        public DateTime? ALERT_DATE { get; set; }
        public string MAIL_TO { get; set; }
        public bool CHECK_HR_CALENDAR { get; set; }
        public DateTime? CDATE { get; set; }
        public string CUSER { get; set; }
        public DateTime? MDATE { get; set; }
        public string MUSER { get; set; }
    }
}