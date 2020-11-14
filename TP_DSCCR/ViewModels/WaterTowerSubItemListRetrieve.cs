using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TP_DSCCR.ViewModels
{
    public class WaterTowerSubItemList
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public string Desc { get; set; }
    }

    public class WaterTowerSubItemListRetrieveReq
{
        public string PhraseGroup { get; set; }

        //[DisplayFormat(ConvertEmptyStringToNull = false)]
        //public string ParentKey { get; set; }
        public string WaterTowerKey { get; set; }
    }

    public class WaterTowerSubItemListRetrieveRes : BaseResponse
    {
        public List<WaterTowerSubItemList> SubItemList;
    }
}