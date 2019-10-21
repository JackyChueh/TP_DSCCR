using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TP_DSCCR.Models.Entity;

namespace TP_DSCCR.ViewModels
{
    public class AHUReq
    {
        public AHU AHU { get; set; }
        public DateTime? SDATE { get; set; }
        public DateTime? EDATE { get; set; }
        public string FIELD { get; set; }
        public string GROUP_BY_DT { get; set; }
        public string GRAPH_TYPE { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
    public class AHURes : BaseResponse
    {
        public List<AHU> AHU { get; set; }
        public List<AHUChartJS> AHUChartJS { get; set; }
        public ChartLine ChartLine { get; set; }
        public Pagination Pagination { get; set; }
    }
    public class AHUChartJS
    {
        public string CDATE { get; set; }
        public string LOCATION { get; set; }
        public string DEVICE_ID { get; set; }
        public Decimal AHU_VALUE { get; set; }
        //public Decimal AHU02 { get; set; }
        //public Decimal AHU03 { get; set; }
        //public Decimal AHU04 { get; set; }
        //public Decimal AHU05 { get; set; }
        //public Decimal AHU06 { get; set; }
        //public Decimal AHU07 { get; set; }
        //public Decimal AHU08 { get; set; }
        //public Decimal AHU09 { get; set; }
        //public Decimal AHU10 { get; set; }
        //public Decimal AHU11 { get; set; }
    }

    public class AHU
    {
        //public int SID { get; set; }
        public string CDATE { get; set; }
        //public int AUTOID { get; set; }
        //public DateTime? DATETIME { get; set; }
        public string LOCATION { get; set; }
        public string DEVICE_ID { get; set; }
        public decimal? AHU01 { get; set; }
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
}