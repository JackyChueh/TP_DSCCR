using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace TP_DSCCR.ViewModels
{
    public class MSPCSTATSData
    {
        public string CDATE { get; set; }
        public string LOCATION { get; set; }
        public string DEVICE_ID { get; set; }
        public string WATER_TOWER { get; set; }
        public string SEF01 { get; set; }
        public string SEF02 { get; set; }
        public string SEF03 { get; set; }
        public string SEF04 { get; set; }
        public string SEF05 { get; set; }
        public string SEF06 { get; set; }
        public string SEF07 { get; set; }
        public string SEF08 { get; set; }
    }
    public class MSPCSTATSDataReq
    {
        public MSPCSTATSData MSPCSTATS { get; set; }
        public DateTime? SDATE { get; set; }
        public DateTime? EDATE { get; set; }
        public string FIELD { get; set; }
        public string GROUP_BY_DT { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
    public class MSPCSTATSDataRes : BaseResponse
    {
        public List<MSPCSTATSData> MSPCSTATSData { get; set; }
        public Pagination Pagination { get; set; }
    }

    public class MSPCSTATSChartJsData
    {
        public string CDATE { get; set; }
        public string LOCATION { get; set; }
        public string DEVICE_ID { get; set; }
        public Decimal? VALUE { get; set; }
    }
    public class MSPCSTATSGraphReq
    {
        public MSPCSTATSData MSPCSTATS { get; set; }
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
                    if ("SEF01,SEF02,SEF03,SEF04,SEF05,SEF06,SEF07,SEF08".IndexOf(value) < 0)
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
    public class MSPCSTATSGraphRes : BaseResponse
    {
        public Chart Chart { get; set; }
    }

    public class MSPCSTATSExcelReq
    {
        public MSPCSTATSData MSPCSTATS { get; set; }
        public DateTime? SDATE { get; set; }
        public DateTime? EDATE { get; set; }
        public string FIELD { get; set; }
        public string GROUP_BY_DT { get; set; }
    }
    public class MSPCSTATSExcelRes : BaseResponse
    {
        public string DataId { get; set; }
        public string FileName { get; set; }

    }


}