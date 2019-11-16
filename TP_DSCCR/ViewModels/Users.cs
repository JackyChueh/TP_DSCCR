using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using TP_DSCCR.Models.Entity;

namespace TP_DSCCR.ViewModels
{
    public class UsersRetrieveReq
    {
        public USERS USERS { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
    public class UsersRetrieveRes : BaseResponse
    {
        public List<USERS> USERS { get; set; }
        public Pagination Pagination { get; set; }
    }

   public class UsersModifyReq
    {
        public USERS USERS { get; set; }
    }
    public class UsersModifyRes : BaseResponse
    {
        public USERS USERS { get; set; }
    }
}