using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace TP_DSCCR.Models.Enums
{
    public enum ResultEnum : int
    {
        [Description("密碼重置完成")]
        RESET_SUCCESS = 5,
        [Description("設定完成")]
        SET_SUCCESS = 4,
        [Description("資料刪除完成")]
        DELETE_SUCCESS = 3,
        [Description("資料修改完成")]
        UPDATE_SUCCESS = 2,
        [Description("資料新增成功")]
        CREATE_SUCCESS = 1,
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
        LOGIN_FAIL = -7,
        [Description("連線逾時")]
        SESSION_TIMEOUT = -8,
        [Description("帳號尚未設定群組")]
        GRANTS_NOT_FOUND = -9,
        [Description("資料已存在")]
        DATA_DUPLICATION = -10

    }

}
