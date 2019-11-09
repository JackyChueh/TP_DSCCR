using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace TP_DSCCR.ViewModels
{
    public class ZP1Data
    {
        public string CDATE { get; set; }
        public string LOCATION { get; set; }
        public string DEVICE_ID { get; set; }
        public decimal? ZP101 { get; set; }
        public decimal? ZP102 { get; set; }
        public decimal? ZP103 { get; set; }
        public decimal? ZP104 { get; set; }
        public decimal? ZP105 { get; set; }
        public decimal? ZP106 { get; set; }
        public string ZP107 { get; set; }
        public string ZP108 { get; set; }
        public decimal? ZP109 { get; set; }
        public decimal? ZP110 { get; set; }
        public decimal? ZP111 { get; set; }
    }
    public class ZP1DataReq
    {
        public ZP1Data ZP1 { get; set; }
        public DateTime? SDATE { get; set; }
        public DateTime? EDATE { get; set; }
        public string FIELD { get; set; }
        public string GROUP_BY_DT { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
    public class ZP1DataRes : BaseResponse
    {
        public List<ZP1Data> ZP1Data { get; set; }
        public Pagination Pagination { get; set; }
    }

    public class ZP1ChartJsData
    {
        public string CDATE { get; set; }
        public string LOCATION { get; set; }
        public string DEVICE_ID { get; set; }
        public Decimal? VALUE { get; set; }
    }
    public class ZP1GraphReq
    {
        public ZP1Data ZP1 { get; set; }
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
                    if ("ZP101,ZP102,ZP104,ZP105,ZP106,ZP107,ZP108,ZP109,ZP110,ZP110,ZP111".IndexOf(value) < 0)
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
    public class ZP1GraphRes : BaseResponse
    {
        public Chart Chart { get; set; }
    }

    public class ZP1ExcelReq
    {
        public ZP1Data ZP1 { get; set; }
        public DateTime? SDATE { get; set; }
        public DateTime? EDATE { get; set; }
        public string FIELD { get; set; }
        public string GROUP_BY_DT { get; set; }
    }
    public class ZP1ExcelRes : BaseResponse
    {
        public string DataId { get; set; }
        public string FileName { get; set; }

    }


}