using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TP_DSCCR.Models.Entity
{
    /// <summary>
    /// 系統功能權限設定
    /// </summary>
    public partial class AUTHORITY
    {
        public int? SN { get; set; }
        public Int16? USERS_SN { get; set; }
        public int? ROLES_SN { get; set; }
        public Int16? FUNCTIONS_SN { get; set; }
        public DateTime? CDATE { get; set; }
        public Int16? CUSER { get; set; }
        public DateTime? MDATE { get; set; }
        public Int16? MUSER { get; set; }
    }


}