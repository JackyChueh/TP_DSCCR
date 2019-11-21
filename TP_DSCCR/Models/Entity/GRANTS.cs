using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TP_DSCCR.Models.Entity
{
    public class GRANTS
    {
        public int? ROLES_SN { get; set; }
        public int? USERS_SN { get; set; }
        public DateTime? CDATE { get; set; }
        public string CUSER { get; set; }
        public DateTime? MDATE { get; set; }
        public string MUSER { get; set; }
    }
}