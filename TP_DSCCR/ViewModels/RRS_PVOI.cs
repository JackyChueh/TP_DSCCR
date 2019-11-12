using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace TP_DSCCR.ViewModels
{
    public class RRS_PVOIData
    {
        public string CDATE { get; set; }
        public string LOCATION { get; set; }
        public string DEVICE_ID { get; set; }
        public string RRS01_PVOI01 { get; set; }
        public string RRS02_PVOI01 { get; set; }
        public string RRS03_PVOI01 { get; set; }
        public string RRS04_PVOI01 { get; set; }
        public string RRS05_PVOI01 { get; set; }
        public string RRS06_PVOI01 { get; set; }
        public string RRS07_PVOI01 { get; set; }
    }
    public class RRS_PVOIDataReq
    {
        public RRS_PVOIData RRS_PVOI { get; set; }
        public DateTime? SDATE { get; set; }
        public DateTime? EDATE { get; set; }
        public string FIELD { get; set; }
        public string GROUP_BY_DT { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
    public class RRS_PVOIDataRes : BaseResponse
    {
        public List<RRS_PVOIData> RRS_PVOIData { get; set; }
        public Pagination Pagination { get; set; }
    }

    public class RRS_PVOIChartJsData
    {
        public string CDATE { get; set; }
        public string LOCATION { get; set; }
        public string DEVICE_ID { get; set; }
        public Decimal? VALUE { get; set; }
    }
    public class RRS_PVOIGraphReq
    {
        public RRS_PVOIData RRS_PVOI { get; set; }
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
                    if ("RRS01_PVOI01,RRS02_PVOI01,RRS03_PVOI01,RRS04_PVOI01,RRS05_PVOI01,RRS06_PVOI01,RRS07_PVOI01".IndexOf(value) < 0)
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
    public class RRS_PVOIGraphRes : BaseResponse
    {
        public Chart Chart { get; set; }
    }

    public class RRS_PVOIExcelReq
    {
        public RRS_PVOIData RRS_PVOI { get; set; }
        public DateTime? SDATE { get; set; }
        public DateTime? EDATE { get; set; }
        public string FIELD { get; set; }
        public string GROUP_BY_DT { get; set; }
    }
    public class RRS_PVOIExcelRes : BaseResponse
    {
        public string DataId { get; set; }
        public string FileName { get; set; }

    }


}