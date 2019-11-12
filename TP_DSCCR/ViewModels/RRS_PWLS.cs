using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace TP_DSCCR.ViewModels
{
    public class RRS_PWLSData
    {
        public string CDATE { get; set; }
        public string LOCATION { get; set; }
        public string DEVICE_ID { get; set; }
        public string RRS01_PWLS01 { get; set; }
        public string RRS02_PWLS01 { get; set; }
        public string RRS03_PWLS01 { get; set; }
        public string RRS04_PWLS01 { get; set; }
        public string RRS05_PWLS01 { get; set; }
        public string RRS06_PWLS01 { get; set; }
        public string RRS07_PWLS01 { get; set; }
        public string RRS08_PWLS01 { get; set; }
        public string RRS09_PWLS01 { get; set; }
        public string RRS10_PWLS01 { get; set; }
        public string RRS11_PWLS01 { get; set; }
        public string RRS12_PWLS01 { get; set; }
        public string RRS13_PWLS01 { get; set; }
    }
    public class RRS_PWLSDataReq
    {
        public RRS_PWLSData RRS_PWLS { get; set; }
        public DateTime? SDATE { get; set; }
        public DateTime? EDATE { get; set; }
        public string FIELD { get; set; }
        public string GROUP_BY_DT { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
    public class RRS_PWLSDataRes : BaseResponse
    {
        public List<RRS_PWLSData> RRS_PWLSData { get; set; }
        public Pagination Pagination { get; set; }
    }

    public class RRS_PWLSChartJsData
    {
        public string CDATE { get; set; }
        public string LOCATION { get; set; }
        public string DEVICE_ID { get; set; }
        public Decimal? VALUE { get; set; }
    }
    public class RRS_PWLSGraphReq
    {
        public RRS_PWLSData RRS_PWLS { get; set; }
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
                    if ("RRS01_PWLS01,RRS02_PWLS01,RRS03_PWLS01,RRS04_PWLS01,RRS05_PWLS01,RRS06_PWLS01,RRS07_PWLS01,RRS08_PWLS01,RRS09_PWLS01,RRS10_PWLS01,RRS11_PWLS01,RRS12_PWLS01,RRS13_PWLS01".IndexOf(value) < 0)
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
    public class RRS_PWLSGraphRes : BaseResponse
    {
        public Chart Chart { get; set; }
    }

    public class RRS_PWLSExcelReq
    {
        public RRS_PWLSData RRS_PWLS { get; set; }
        public DateTime? SDATE { get; set; }
        public DateTime? EDATE { get; set; }
        public string FIELD { get; set; }
        public string GROUP_BY_DT { get; set; }
    }
    public class RRS_PWLSExcelRes : BaseResponse
    {
        public string DataId { get; set; }
        public string FileName { get; set; }

    }


}