using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace TP_DSCCR.Models.Enums
{
    public enum ResultEnum : int
    {
        [Description("交易成功")]
        SUCCESS = 0,
        [Description("交易失敗")]
        FAIL = -1,
        [Description("缺少必要參數")]
        LACK_PARAMETER = -2,
        [Description("參數內容錯誤")]
        PARAMETER_ERROR = -3,
        [Description("系統錯誤")]
        SYSTEM_ERROR = -4,
        [Description("執行錯誤")]
        EXCEPTION_ERROR = -5,
        [Description("查無資料")]
        DATA_NOT_FOUND = -6,
        [Description("帳號或密碼錯誤")]
        LOGIN_FAIL = -7

    }

}
