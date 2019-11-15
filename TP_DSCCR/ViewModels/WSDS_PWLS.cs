using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace TP_DSCCR.ViewModels
{
    public class WSDS_PWLSData
    {
        public string CDATE { get; set; }
        public string LOCATION { get; set; }
        public string DEVICE_ID { get; set; }
        public string WSDS_PWLS_STATUS { get; set; }
    }
    public class WSDS_PWLSDataReq
    {
        public WSDS_PWLSData WSDS_PWLS { get; set; }
        public DateTime? SDATE { get; set; }
        public DateTime? EDATE { get; set; }
        public string FIELD { get; set; }
        public string GROUP_BY_DT { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
    public class WSDS_PWLSDataRes : BaseResponse
    {
        public List<WSDS_PWLSData> WSDS_PWLSData { get; set; }
        public Pagination Pagination { get; set; }
    }

    public class WSDS_PWLSChartJsData
    {
        public string CDATE { get; set; }
        public string LOCATION { get; set; }
        public string DEVICE_ID { get; set; }
        public Decimal? VALUE { get; set; }
    }
    public class WSDS_PWLSGraphReq
    {
        public WSDS_PWLSData WSDS_PWLS { get; set; }
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
                    if ("WSDS_PWLS_STATUS".IndexOf(value) < 0)
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
    public class WSDS_PWLSGraphRes : BaseResponse
    {
        public Chart Chart { get; set; }
    }

    public class WSDS_PWLSExcelReq
    {
        public WSDS_PWLSData WSDS_PWLS { get; set; }
        public DateTime? SDATE { get; set; }
        public DateTime? EDATE { get; set; }
        public string FIELD { get; set; }
        public string GROUP_BY_DT { get; set; }
    }
    public class WSDS_PWLSExcelRes : BaseResponse
    {
        public string DataId { get; set; }
        public string FileName { get; set; }

    }


}