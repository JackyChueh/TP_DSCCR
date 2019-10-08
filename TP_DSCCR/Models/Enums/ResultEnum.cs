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
        [Description("嚴重系統錯誤")]
        SERIOUS_ERROR = -3,
        [Description("連線逾時")]
        CONNECT_TIMEOUT = -4,
        [Description("參數內容錯誤")]
        PARAMETER_ERROR = -5,
        [Description("參數內容錯誤")]
        OrderError1 = -6,
        [Description("庫存不足，請減少訂購數量！")]
        OrderError2 = -7,
        [Description("查無資料")]
        FIND_NO_DATA = -8,
        [Description("資料重複")]
        DUPLICATED_DATA = -9,
        [Description("帳號或密碼錯誤")]
        LOGIN_ERROR = -10,
        [Description("此類別正在被使用中")]
        DATA_IS_USED = -11,
        [Description("無效的優惠券序號")]
        PCODE_ERROR = -12

    }

}
