using System;
using System.Web.Mvc;
using System.IO;
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

        [AcceptVerbs("POST")]
        public string ALERT_LOGExcel()
        {
            ALERT_LOGExcelRes res = new ALERT_LOGExcelRes();
            try
            {
                if (Session["ID"] == null)
                {
                    res.Result.State = ResultEnum.SESSION_TIMEOUT;
                }
                else
                {
                    string input = RequestData();
                    Log("ALERT_LOGExcelReq=" + input);
                    ALERT_LOGExcelReq req = new ALERT_LOGExcelReq();
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
                    MemoryStream MemoryStream = new ALERT_LOGImplement("TP_ALERT").ExcelRetrieve(req);

                    if (MemoryStream.Length > 0)
                    {
                        string DataId = Guid.NewGuid().ToString();
                        TempData[DataId] = MemoryStream.ToArray();
                        MemoryStream.Dispose();

                        res.DataId = DataId;
                        res.FileName = "警報記錄查詢.xlsx";
                        res.Result.State = ResultEnum.SUCCESS;
                    }
                    else
                    {
                        res.Result.State = ResultEnum.DATA_NOT_FOUND;
                    }
                }
            }
            catch (Exception ex)
            {
                res.Result.State = ResultEnum.EXCEPTION_ERROR;
                Log("Err=" + ex.Message);
                Log(ex.StackTrace);
            }
            var json = JsonConvert.SerializeObject(res);
            Log("ALERT_LOGExcelRes=" + json);
            return json;
        }


    }
}