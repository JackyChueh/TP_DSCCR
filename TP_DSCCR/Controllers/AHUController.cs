using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TP_DSCCR.ViewModels;
using TP_DSCCR.Models.Implement;
using Newtonsoft.Json;
using TP_DSCCR.Models.Enums;

namespace TP_DSCCR.Controllers
{
    public class AHUController : BaseController
    {
        // GET: AirHandleUnit
        public ActionResult Index()
        {
            return View();
        }

        [AcceptVerbs("POST")]
        public string AHURetrieve(AHUReq req)
        {
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
    }
}