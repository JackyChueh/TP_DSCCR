using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace TP_DSCCR.ViewModels
{
    public class MSPCALARSData
    {
        public string CDATE { get; set; }
        public string LOCATION { get; set; }
        public string DEVICE_ID { get; set; }
        public string WATER_TOWER { get; set; }
        public string SEF09 { get; set; }
        public string SEF10 { get; set; }
        public string SEF11 { get; set; }
        public string SEF12 { get; set; }
        public string SEF13 { get; set; }
        public string SEF14 { get; set; }
        public string SEF15 { get; set; }
    }
    public class MSPCALARSDataReq
    {
        public MSPCALARSData MSPCALARS { get; set; }
        public DateTime? SDATE { get; set; }
        public DateTime? EDATE { get; set; }
        public string FIELD { get; set; }
        public string GROUP_BY_DT { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
    public class MSPCALARSDataRes : BaseResponse
    {
        public List<MSPCALARSData> MSPCALARSData { get; set; }
        public Pagination Pagination { get; set; }
    }

    public class MSPCALARSChartJsData
    {
        public string CDATE { get; set; }
        public string LOCATION { get; set; }
        public string DEVICE_ID { get; set; }
        public Decimal? VALUE { get; set; }
    }
    public class MSPCALARSGraphReq
    {
        public MSPCALARSData MSPCALARS { get; set; }
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
                    if ("SEF09,SEF10,SEF11,SEF12,SEF13,SEF14,SEF15".IndexOf(value) < 0)
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
    public class MSPCALARSGraphRes : BaseResponse
    {
        public Chart Chart { get; set; }
    }

    public class MSPCALARSExcelReq
    {
        public MSPCALARSData MSPCALARS { get; set; }
        public DateTime? SDATE { get; set; }
        public DateTime? EDATE { get; set; }
        public string FIELD { get; set; }
        public string GROUP_BY_DT { get; set; }
    }
    public class MSPCALARSExcelRes : BaseResponse
    {
        public string DataId { get; set; }
        public string FileName { get; set; }

    }


}