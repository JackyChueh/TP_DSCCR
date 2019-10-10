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
                Data Data = new Data();
                List<string> labels = new List<string>();
                List<Dataset> Dataset = new List<Dataset>();
                Dataset dsAHU05 = new Dataset()
                {
                    label = "回風溫度",
                    fill = false,
                    backgroundColor = "rgb(255, 99, 132)",
                    borderColor = "rgb(255, 99, 132)",
                    data = new List<Decimal> { }
                };
                foreach (AHUChartJS AHUChartJS in res.AHUChartJS)
                {
                    labels.Add(AHUChartJS.CDATE.Substring(11, 2));
                    dsAHU05.data.Add(AHUChartJS.AHU05);
                }
                Dataset.Add(dsAHU05);
                Data.datasets = Dataset;
                Data.labels = labels;

                ChartLine ChartLine = new ChartLine()
                {
                    type = "line",
                    data = Data,
                    options = new Options() {
                        responsive = true,
                        maintainAspectRatio = false,
                        title = new Title() {
                            display = true,
                            text= "折線圖範例"
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