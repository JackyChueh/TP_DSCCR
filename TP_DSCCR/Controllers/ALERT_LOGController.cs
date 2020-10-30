using System;
using System.Web.Mvc;
using Newtonsoft.Json;
using TP_DSCCR.ViewModels;
using TP_DSCCR.Models.Implement;
using TP_DSCCR.Models.Enums;

namespace TP_DSCCR.Controllers
{
    public class ALERT_LOGController : BaseController
    {
        // GET:ALERT_LOG
        public ActionResult ALERT_LOGIndex()
        {
            if (Session["ID"] == null)
            {
                return RedirectToAction("Login", "Main");
            }
            return View();
        }

        public string ALERT_LOGRetrieve()
        {
            ALERT_LOGRetrieveRes res = new ALERT_LOGRetrieveRes();
            try
            {
                if (Session["ID"] == null)
                {
                    res.Result.State = ResultEnum.SESSION_TIMEOUT;
                }
                else
                {
                    string input = RequestData();
                    Log("ALERT_LOGRetrieveReq=" + input);
                    ALERT_LOGRetrieveReq req = new ALERT_LOGRetrieveReq();
                    JsonConvert.PopulateObject(input, req);

                    DateTime dt;
                    if (DateTime.TryParse(req.SDATE.ToString(), out dt) && DateTime.TryParse(req.EDATE.ToString(), out dt))
                    {
                        DateTime? temp;
                        if (req.SDATE > req.EDATE)
                        {
                            temp = req.SDATE;
                            req.SDATE = req.EDATE;
                            req.EDATE = temp;
                        }
                        req.EDATE = req.EDATE.Value.AddMinutes(1);
                    }

                    res = new ALERT_LOGImplement("TP_ALERT").PaginationRetrieve(req);
                    res.Result.State = ResultEnum.SUCCESS;
                }
            }
            catch (Exception ex)
            {
                res.Result.State = ResultEnum.EXCEPTION_ERROR;
                Log("Err=" + ex.Message);
                Log(ex.StackTrace);
            }
            var json = JsonConvert.SerializeObject(res, Formatting.None,
                new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            Log("ALERT_LOGRetrieveRes=" + json);
            return json;
        }
    }
}