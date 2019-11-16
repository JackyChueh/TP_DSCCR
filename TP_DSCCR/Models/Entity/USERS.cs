using System;

namespace TP_DSCCR.Models.Entity
{
    /// <summary>
    /// 使用者
    /// </summary>
    public class USERS
    {
        public int? SN { get; set; }
        public string ID { get; set; }
        public string NAME { get; set; }
        public string PASSWORD { get; set; }
        public string EMAIL { get; set; }
        public string MODE { get; set; }
        public string MEMO { get; set; }
        public DateTime? CDATE { get; set; }
        public string CUSER { get; set; }
        public DateTime? MDATE { get; set; }
        public string MUSER { get; set; }
        public int? FORCE_PWD { get; set; }
    }
}
