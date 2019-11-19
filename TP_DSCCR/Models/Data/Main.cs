using System;
using System.Collections.Generic;
using TP_DSCCR.Models.Entity;

namespace TP_DSCCR.Models.Data
{
    public class Main
    {
        public class SidebarReq
        {

        }
        public class SidebarRes
        {
            public List<SidebarItem> SidebarItem { get; set; }
        }
        public class SidebarItem
        {
            public FUNCTIONS FUNCTIONS { get; set; }
            public int? ALLOW { get; set; }
        }

        public class UserInfo
        { 
            public USERS USERS { get; set; }
            public ROLES ROLES { get; set; } 
        }
    }
}