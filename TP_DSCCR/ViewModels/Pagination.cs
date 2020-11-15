using System;
namespace TP_DSCCR.ViewModels
{
    public class Pagination
    {
        public int PageNumber { get;set; }
        public int PageCount { get; set; }
        public int RowCount { get; set; }
        public int MinNumber { get; set; }
        public int MaxNumber { get; set; }
        //public object Data;
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
    }
}