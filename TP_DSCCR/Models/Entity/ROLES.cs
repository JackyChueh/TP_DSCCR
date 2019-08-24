using System;

namespace TP_DSCCR.Models.Entity
{
    /// <summary>
    /// 系統角色
    /// </summary>
    public class ROLES
    {
        public int? SN { get; set; }
        public string NAME { get; set; }
        public string MODE { get; set; }
        public DateTime? CDATE { get; set; }
        public string CUSER { get; set; }
        public DateTime? MDATE { get; set; }
        public string MUSER { get; set; }
    }
}
