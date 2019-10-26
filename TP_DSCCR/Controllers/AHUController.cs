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
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace TP_DSCCR.Controllers
{
    public class AHUController : BaseController
    {
        public ActionResult Data()
        {
            return View();
        }

        public ActionResult Graph()
        {
            return View();
        }

        [AcceptVerbs("POST")]
        public string AHURetrieve(AHUDataReq req)
        {
            //System.Threading.Thread.Sleep(2000);
            AHUDataRes res = new AHUDataRes();
            try
            {
                Log("Req=" + JsonConvert.SerializeObject(req));

                res = new AHUImplement("TP_DSCCR").PaginationRetrieve(req);
                res.Result.State = ResultEnum.SUCCESS;
            }
            catch (Exception ex)
            {
                res.Result.State = ResultEnum.EXCEPTION_ERROR;
                Log("Err=" + ex.Message);
                Log(ex.StackTrace);
            }
            var json = JsonConvert.SerializeObject(res);
            Log("Res=" + json);
            return json;
        }

        [AcceptVerbs("POST")]
        public string AHUGraph()
        {
            AHUGraphRes res = new AHUGraphRes();
            try
            {
                string input = RequestData();
                Log(input);
                AHUGraphReq req = new AHUGraphReq();
                JsonConvert.PopulateObject(input, req);

                res = new AHUImplement("TP_DSCCR").GraphRetrieve(req);

                res.Result.State = ResultEnum.SUCCESS;
            }
            catch (Exception ex)
            {
                res.Result.State = ResultEnum.EXCEPTION_ERROR;
                Log("Err=" + ex.Message);
                Log(ex.StackTrace);
            }
            var json = JsonConvert.SerializeObject(res);
            Log("Res=" + json);
            return json;
        }

    }
}