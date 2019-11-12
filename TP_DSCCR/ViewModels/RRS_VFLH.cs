using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace TP_DSCCR.ViewModels
{
    public class RRS_VFLHData
    {
        public string CDATE { get; set; }
        public string LOCATION { get; set; }
        public string DEVICE_ID { get; set; }
        public decimal? RRS01_VFLH01 { get; set; }
        public decimal? RRS02_VFLH01 { get; set; }
        public decimal? RRS03_VFLH01 { get; set; }
        public decimal? RRS04_VFLH01 { get; set; }
        public decimal? RRS05_VFLH01 { get; set; }
        public decimal? RRS06_VFLH01 { get; set; }
    }
    public class RRS_VFLHDataReq
    {
        public RRS_VFLHData RRS_VFLH { get; set; }
        public DateTime? SDATE { get; set; }
        public DateTime? EDATE { get; set; }
        public string FIELD { get; set; }
        public string GROUP_BY_DT { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
    public class RRS_VFLHDataRes : BaseResponse
    {
        public List<RRS_VFLHData> RRS_VFLHData { get; set; }
        public Pagination Pagination { get; set; }
    }

    public class RRS_VFLHChartJsData
    {
        public string CDATE { get; set; }
        public string LOCATION { get; set; }
        public string DEVICE_ID { get; set; }
        public Decimal? VALUE { get; set; }
    }
    public class RRS_VFLHGraphReq
    {
        public RRS_VFLHData RRS_VFLH { get; set; }
        public DateTime? SDATE { get; set; }
        public DateTime? EDATE { get; set; }

        private string _FIELD;
        [JsonProperty(PropertyName = "FIELD", Required = Required.Always)]
        public string FIELD
        {
            get
            {
                return _FIELD;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException("FIELD is null or empty");
                }
                else
                {
                    if ("RRS01_VFLH01,RRS02_VFLH01,RRS03_VFLH01,RRS04_VFLH01,RRS05_VFLH01,RRS06_VFLH01".IndexOf(value) < 0)
                    {
                        throw new ArgumentException("FIELD invalid domain value");
                    }
                }
                _FIELD = value;
            }
        }

        public string FIELD_NAME { get; set; }
        public string GROUP_BY_DT { get; set; }
        public string GROUP_BY_DT_NAME { get; set; }
        public string GRAPH_TYPE { get; set; }
    }
    public class RRS_VFLHGraphRes : BaseResponse
    {
        public Chart Chart { get; set; }
    }

    public class RRS_VFLHExcelReq
    {
        public RRS_VFLHData RRS_VFLH { get; set; }
        public DateTime? SDATE { get; set; }
        public DateTime? EDATE { get; set; }
        public string FIELD { get; set; }
        public string GROUP_BY_DT { get; set; }
    }
    public class RRS_VFLHExcelRes : BaseResponse
    {
        public string DataId { get; set; }
        public string FileName { get; set; }

    }


}