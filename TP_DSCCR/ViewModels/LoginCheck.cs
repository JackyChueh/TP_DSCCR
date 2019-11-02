using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using TP_DSCCR.Models.Entity;

namespace TP_DSCCR.ViewModels
{
    public class LoginCheckReq
    {
        public USERS USERS { get; set; }
    }

    public class LoginCheckRes : BaseResponse
    {

    }
}