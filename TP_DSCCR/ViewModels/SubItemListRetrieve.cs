﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TP_DSCCR.ViewModels
{
    public class SubItemList
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public string Desc { get; set; }
    }

    public class SubItemListRetrieveReq
    {
        public string PhraseGroup { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string ParentKey { get; set; }
    }

    public class SubItemListRetrieveRes : BaseResponse
    {
        public List<SubItemList> SubItemList;
    }
}