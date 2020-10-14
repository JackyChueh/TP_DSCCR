using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace TP_DSCCR.ViewModels
{
    public class MSPCAIData
    {
        public string CDATE { get; set; }
        public string LOCATION { get; set; }
        public string DEVICE_ID { get; set; }
        public string WATER_TOWER { get; set; }
        public string SEF16 { get; set; }
        public string SEF17 { get; set; }
        public string SEF18 { get; set; }
    }
    public class MSPCAIDataReq
    {
        public MSPCAIData MSPCAI { get; set; }
        public DateTime? SDATE { get; set; }
        public DateTime? EDATE { get; set; }
        public string FIELD { get; set; }
        public string GROUP_BY_DT { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
    public class MSPCAIDataRes : BaseResponse
    {
        public List<MSPCAIData> MSPCAIData { get; set; }
        public Pagination Pagination { get; set; }
    }

    public class MSPCAIChartJsData
    {
        public string CDATE { get; set; }
        public string LOCATION { get; set; }
        public string DEVICE_ID { get; set; }
        public Decimal? VALUE { get; set; }
    }
    public class MSPCAIGraphReq
    {
        public MSPCAIData MSPCAI { get; set; }
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
                    if ("SEF16,SEF17,SEF18".IndexOf(value) < 0)
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
    public class MSPCAIGraphRes : BaseResponse
    {
        public Chart Chart { get; set; }
    }

    public class MSPCAIExcelReq
    {
        public MSPCAIData MSPCAI { get; set; }
        public DateTime? SDATE { get; set; }
        public DateTime? EDATE { get; set; }
        public string FIELD { get; set; }
        public string GROUP_BY_DT { get; set; }
    }
    public class MSPCAIExcelRes : BaseResponse
    {
        public string DataId { get; set; }
        public string FileName { get; set; }

    }


}