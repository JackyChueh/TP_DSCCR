using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TP_DSCCR.ViewModels
{

    public class Dataset
    {
        public string label { get; set; }
        public bool fill { get; set; }
        public string backgroundColor { get; set; }
        public string borderColor { get; set; }
        public List<Decimal> data { get; set; }
        //public List<int?> borderDash { get; set; }
    }

    public class Data
    {
        public List<string> labels { get; set; }
        public List<Dataset> datasets { get; set; }
    }

    public class Title
    {
        public bool display { get; set; }
        public string text { get; set; }
    }

    public class Options
    {
        public bool responsive { get; set; }
        public Title title { get; set; }
    }

    public class Chart
    {
        public string type { get; set; }
        public Data data { get; set; }
        public Options options { get; set; }
    }
}