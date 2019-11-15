using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace TP_DSCCR.ViewModels
{
    public class WSDS_PVOIData
    {
        public string CDATE { get; set; }
        public string LOCATION { get; set; }
        public string DEVICE_ID { get; set; }
        public string WSDS_PVOI_STATUS { get; set; }
    }
    public class WSDS_PVOIDataReq
    {
        public WSDS_PVOIData WSDS_PVOI { get; set; }
        public DateTime? SDATE { get; set; }
        public DateTime? EDATE { get; set; }
        public string FIELD { get; set; }
        public string GROUP_BY_DT { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
    public class WSDS_PVOIDataRes : BaseResponse
    {
        public List<WSDS_PVOIData> WSDS_PVOIData { get; set; }
        public Pagination Pagination { get; set; }
    }

    public class WSDS_PVOIChartJsData
    {
        public string CDATE { get; set; }
        public string LOCATION { get; set; }
        public string DEVICE_ID { get; set; }
        public Decimal? VALUE { get; set; }
    }
    public class WSDS_PVOIGraphReq
    {
        public WSDS_PVOIData WSDS_PVOI { get; set; }
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
                    if ("WSDS_PVOI_STATUS".IndexOf(value) < 0)
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
    public class WSDS_PVOIGraphRes : BaseResponse
    {
        public Chart Chart { get; set; }
    }

    public class WSDS_PVOIExcelReq
    {
        public WSDS_PVOIData WSDS_PVOI { get; set; }
        public DateTime? SDATE { get; set; }
        public DateTime? EDATE { get; set; }
        public string FIELD { get; set; }
        public string GROUP_BY_DT { get; set; }
    }
    public class WSDS_PVOIExcelRes : BaseResponse
    {
        public string DataId { get; set; }
        public string FileName { get; set; }

    }


}