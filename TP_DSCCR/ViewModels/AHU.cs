using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace TP_DSCCR.ViewModels
{
    public class AHUData
    {
        public string CDATE { get; set; }
        public string LOCATION { get; set; }
        public string DEVICE_ID { get; set; }
        public string AHU01 { get; set; }
        public decimal? AHU02 { get; set; }
        public decimal? AHU03 { get; set; }
        public decimal? AHU04 { get; set; }
        public decimal? AHU05 { get; set; }
        public decimal? AHU06 { get; set; }
        public decimal? AHU07 { get; set; }
        public decimal? AHU08 { get; set; }
        public decimal? AHU09 { get; set; }
        public decimal? AHU10 { get; set; }
        public decimal? AHU11 { get; set; }
    }
    public class AHUDataReq
    {
        public AHUData AHU { get; set; }
        public DateTime? SDATE { get; set; }
        public DateTime? EDATE { get; set; }
        public string FIELD { get; set; }
        public string GROUP_BY_DT { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
    public class AHUDataRes : BaseResponse
    {
        public List<AHUData> AHUData { get; set; }
        public Pagination Pagination { get; set; }
    }

    public class AHUChartJsData
    {
        public string CDATE { get; set; }
        public string LOCATION { get; set; }
        public string DEVICE_ID { get; set; }
        public Decimal VALUE { get; set; }
    }
    public class AHUGraphReq
    {
        public AHUData AHU { get; set; }
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
                    if ("AHU01,AHU02,AHU03,AHU04,AHU05,AHU06,AHU07,AHU08,AHU09,AHU10,AHU11".IndexOf(value) < 0)
                    {
                        throw new ArgumentException("FIELD invalid domain value");
                    }
                }
                _FIELD = value;
            }
        }

        public string FIELD_NAME { get; set; }
        public string GROUP_BY_DT { get; set; }
        public string GRAPH_TYPE { get; set; }
    }
    public class AHUGraphRes : BaseResponse
    {
        public Chart Chart { get; set; }
    }



    
}