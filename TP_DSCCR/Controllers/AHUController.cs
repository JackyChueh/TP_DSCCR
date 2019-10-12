using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TP_DSCCR.ViewModels;
using TP_DSCCR.Models.Implement;
using Newtonsoft.Json;
using TP_DSCCR.Models.Enums;
using TP_DSCCR.Models.Entity;

namespace TP_DSCCR.Controllers
{
    public class AHUController : BaseController
    {
        // GET: AirHandleUnit
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Graph()
        {
            return View();
        }


        [AcceptVerbs("POST")]
        public string AHURetrieve(AHUReq req)
        {
            //System.Threading.Thread.Sleep(1000);
            AHURes res = new AHURes();
            try
            {
                Log("Req=" + JsonConvert.SerializeObject(req));

                res = new AHUImplement("TP_DSCCR").PaginationRetrieve(req);
                res.Result.State = ResultEnum.SUCCESS;
            }
            catch (Exception ex)
            {
                Log("Err=" + ex.Message);
                Log(ex.StackTrace);
                //res.ReturnStatus = new ReturnStatus(ReturnCode.SERIOUS_ERROR);
            }
            var json = JsonConvert.SerializeObject(res);
            Log("Res=" + json);
            return json;
        }

        [AcceptVerbs("POST")]
        public string AHUGraph(AHUReq req)
        {
            AHURes res = new AHURes();
            try
            {
                Log("Req=" + JsonConvert.SerializeObject(req));

                res = new AHUImplement("TP_DSCCR").GraphRetrieve(req);

                var query = from data in res.AHUChartJS
                            group data by data.CDATE;


                Data Data = new Data();
                List<string> labels = new List<string>();
                foreach (var q in query)
                {
                    labels.Add(q.Key);
                }

                List<Dataset> Dataset = new List<Dataset>();
                //Dataset ds004F01 = new Dataset()
                //{
                //    label = "004F,01",
                //    fill = false,
                //    backgroundColor = "rgb(255, 99, 132)",
                //    borderColor = "rgb(255, 99, 132)",
                //    data = new List<Decimal> { }
                //};

                string key = null;
                Dataset ds = null;
                Random random = new Random();
                string rgb = null;

                foreach (AHUChartJS AHUChartJS in res.AHUChartJS)
                {
                    if (key == null )
                    {
                        key = AHUChartJS.LOCATION + AHUChartJS.DEVICE_ID;
                        rgb = "rgb(" + random.Next(0, 255) + "," + random.Next(0, 255) + "," + random.Next(0, 255) + ")";
                        ds = new Dataset()
                        {
                            label = key,
                            fill = false,
                            backgroundColor = rgb,
                            borderColor = rgb,
                            data = new List<Decimal> { }
                        };
                    }
                    else if (key != AHUChartJS.LOCATION + AHUChartJS.DEVICE_ID)
                    {
                        Dataset.Add(ds);
                        key = AHUChartJS.LOCATION + AHUChartJS.DEVICE_ID;
                        rgb = "rgb(" + random.Next(0, 255) + "," + random.Next(0, 255) + "," + random.Next(0, 255) + ")";
                        ds = new Dataset()
                        {
                            label = key,
                            fill = false,
                            backgroundColor = rgb,
                            borderColor = rgb,
                            data = new List<Decimal> { }
                        };

                    }

                    //if (key != AHUChartJS.LOCATION + AHUChartJS.DEVICE_ID)
                    //{
                    //    key = AHUChartJS.LOCATION + AHUChartJS.DEVICE_ID;


                    //}
                    //labels.Add(AHUChartJS.CDATE.Substring(11, 2));
                    //ds.label = AHUChartJS.LOCATION + AHUChartJS.DEVICE_ID;
                    ds.data.Add(AHUChartJS.AHU_VALUE);

                 
                }
                Dataset.Add(ds);

                Data.datasets = Dataset;
                Data.labels = labels;

                ChartLine ChartLine = new ChartLine()
                {
                    type = req.GRAPH_TYPE,
                    data = Data,
                    options = new Options() {
                        responsive = true,
                        maintainAspectRatio = false,
                        title = new Title() {
                            display = true,
                            text= "回風溫度"
                        }
                    }
                };
                res.ChartLine = ChartLine;

                res.Result.State = ResultEnum.SUCCESS;
            }
            catch (Exception ex)
            {
                Log("Err=" + ex.Message);
                Log(ex.StackTrace);
                //res.ReturnStatus = new ReturnStatus(ReturnCode.SERIOUS_ERROR);
            }
            var json = JsonConvert.SerializeObject(res);
            Log("Res=" + json);
            return json;
        }

    }
}