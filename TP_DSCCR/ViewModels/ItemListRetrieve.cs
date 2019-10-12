using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TP_DSCCR.ViewModels
{
    public class ItemList
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public string Desc { get; set; }
    }

    public class ItemListRetrieveReq
    {
        public string[] TableItem { get; set; }
        public string[] PhraseGroup { get; set; }
    }

    public class ItemListRetrieveRes : BaseResponse
    {
        public object ItemList;
    }
}