using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace TP_DSCCR.ViewModels
{
    public class ChillerData
    {
        public string CDATE { get; set; }
        public string LOCATION { get; set; }
        public string DEVICE_ID { get; set; }
        public decimal? Chiller01 { get; set; }
        public decimal? Chiller02 { get; set; }
        public decimal? Chiller03 { get; set; }
        public decimal? Chiller04 { get; set; }
        public decimal? Chiller05 { get; set; }
        public decimal? Chiller06 { get; set; }
        public string Chiller07 { get; set; }
        public string Chiller08 { get; set; }
        public decimal? Chiller09 { get; set; }
        public decimal? Chiller10 { get; set; }
    }
    public class ChillerDataReq
    {
        public ChillerData Chiller { get; set; }
        public DateTime? SDATE { get; set; }
        public DateTime? EDATE { get; set; }
        public string FIELD { get; set; }
        public string GROUP_BY_DT { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
    public class ChillerDataRes : BaseResponse
    {
        public List<ChillerData> ChillerData { get; set; }
        public Pagination Pagination { get; set; }
    }

    public class ChillerChartJsData
    {
        public string CDATE { get; set; }
        public string LOCATION { get; set; }
        public string DEVICE_ID { get; set; }
        public Decimal? VALUE { get; set; }
    }
    public class ChillerGraphReq
    {
        public ChillerData Chiller { get; set; }
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
                    if ("Chiller01,Chiller02,Chiller03,Chiller04,Chiller05,Chiller06,Chiller07,Chiller08,Chiller09,Chiller10".IndexOf(value) < 0)
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
    public class ChillerGraphRes : BaseResponse
    {
        public Chart Chart { get; set; }
    }

    public class ChillerExcelReq
    {
        public ChillerData Chiller { get; set; }
        public DateTime? SDATE { get; set; }
        public DateTime? EDATE { get; set; }
        public string FIELD { get; set; }
        public string GROUP_BY_DT { get; set; }
    }
    public class ChillerExcelRes : BaseResponse
    {
        public string DataId { get; set; }
        public string FileName { get; set; }

    }


}