using System;
using System.Web.Mvc;
using Newtonsoft.Json;
using TP_DSCCR.ViewModels;
using TP_DSCCR.Models.Implement;
using TP_DSCCR.Models.Enums;

namespace TP_DSCCR.Controllers
{
    public class HR_CALENDARController : BaseController
    {
        // GET: HR_CALENDAR
        public ActionResult HR_CALENDARIndex()
        {
            if (Session["ID"] == null)
            {
                return RedirectToAction("Login", "Main");
            }
            return View();
        }

        public string HR_CALENDARRetrieve()
        {
            HR_CALENDARRetrieveRes res = new HR_CALENDARRetrieveRes();
            try
            {
                if (Session["ID"] == null)
                {
                    res.Result.State = ResultEnum.SESSION_TIMEOUT;
                }
                else
                {
                    string input = RequestData();
                    Log("HR_CALENDARRetrieveReq=" + input);
                    HR_CALENDARRetrieveReq req = new HR_CALENDARRetrieveReq();
                    JsonConvert.PopulateObject(input, req);

                    DateTime sDate = DateTime.Parse(req.YEAR + "-01-01");
                    DateTime eDate = sDate.AddYears(1);

                    res = new HR_CALENDARImplement("TP_ALERT").PaginationRetrieve(sDate, eDate);
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
            Log("HR_CALENDARRetrieveRes=" + json);
            return json;
        }

        public string HR_CALENDARQuery()
        {
            HR_CALENDARModifyRes res = new HR_CALENDARModifyRes();
            try
            {
                if (Session["ID"] == null)
                {
                    res.Result.State = ResultEnum.SESSION_TIMEOUT;
                }
                else
                {
                    string input = RequestData();
                    Log("HR_CALENDARQueryReq=" + input);
                    HR_CALENDARModifyReq req = new HR_CALENDARModifyReq();
                    JsonConvert.PopulateObject(input, req);

                    res = new HR_CALENDARImplement("TP_ALERT").ModificationQuery(req);
                    if (res.HR_CALENDAR != null)
                    {
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
            var json = JsonConvert.SerializeObject(res, Formatting.None,
                new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            Log("HR_CALENDARQueryRes=" + json);
            return json;
        }

        public string HR_CALENDARCreateYear()
        {
            HR_CALENDARModifyRes res = new HR_CALENDARModifyRes();
            try
            {
                if (Session["ID"] == null)
                {
                    res.Result.State = ResultEnum.SESSION_TIMEOUT;
                }
                else
                {
                    string input = RequestData();
                    Log("HR_CALENDARCreateReq=" + input);
                    HR_CALENDARModifyReq req = new HR_CALENDARModifyReq();
                    JsonConvert.PopulateObject(input, req);

                    DateTime sDate = DateTime.Parse(req.YEAR + "-01-01");
                    DateTime eDate = sDate.AddYears(1);

                    bool yn = new HR_CALENDARImplement("TP_ALERT").DataDuplicateYear(sDate, eDate);
                    if (yn)
                    {
                        res.Result.State = ResultEnum.DATA_DUPLICATION;
                    }
                    else
                    {
                        //req.HR_CALENDAR.CUSER = Session["ID"].ToString();
                        //req.HR_CALENDAR.MUSER = Session["ID"].ToString();

                        bool success = new HR_CALENDARImplement("TP_ALERT").DataCreateYear(sDate, eDate, Session["ID"].ToString());
                        if (success)
                        {
                            res.Result.State = ResultEnum.CREATE_SUCCESS;
                        }
                        else
                        {
                            res.Result.State = ResultEnum.FAIL;
                        }
                    }
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
            Log("HR_CALENDARCreateRes=" + json);
            return json;
        }

        public string HR_CALENDARCreate()
        {
            HR_CALENDARModifyRes res = new HR_CALENDARModifyRes();
            try
            {
                if (Session["ID"] == null)
                {
                    res.Result.State = ResultEnum.SESSION_TIMEOUT;
                }
                else
                {
                    string input = RequestData();
                    Log("HR_CALENDARCreateReq=" + input);
                    HR_CALENDARModifyReq req = new HR_CALENDARModifyReq();
                    JsonConvert.PopulateObject(input, req);

                    bool yn = new HR_CALENDARImplement("TP_ALERT").DataDuplicate(req);
                    if (yn)
                    {
                        res.Result.State = ResultEnum.DATA_DUPLICATION;
                    }
                    else
                    {
                        req.HR_CALENDAR.CUSER = Session["ID"].ToString();
                        req.HR_CALENDAR.MUSER = Session["ID"].ToString();

                        bool success = new HR_CALENDARImplement("TP_ALERT").DataCreate(req);
                        if (success)
                        {
                            res.HR_CALENDAR = req.HR_CALENDAR;
                            res.Result.State = ResultEnum.CREATE_SUCCESS;
                        }
                        else
                        {
                            res.Result.State = ResultEnum.FAIL;
                        }
                    }
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
            Log("HR_CALENDARCreateRes=" + json);
            return json;
        }

        public string HR_CALENDARUpdate()
        {
            HR_CALENDARModifyRes res = new HR_CALENDARModifyRes();
            try
            {
                if (Session["ID"] == null)
                {
                    res.Result.State = ResultEnum.SESSION_TIMEOUT;
                }
                else
                {
                    string input = RequestData();
                    Log("HR_CALENDARUpdateReq=" + input);
                    HR_CALENDARModifyReq req = new HR_CALENDARModifyReq();
                    JsonConvert.PopulateObject(input, req);

                    bool yn = new HR_CALENDARImplement("TP_ALERT").DataDuplicate(req);
                    if (yn)
                    {
                        res.Result.State = ResultEnum.DATA_DUPLICATION;
                    }
                    else
                    {
                        req.HR_CALENDAR.MUSER = Session["ID"].ToString();

                        bool success = new HR_CALENDARImplement("TP_ALERT").DataUpdate(req);
                        if (success)
                        {
                            res.HR_CALENDAR = req.HR_CALENDAR;
                            res.Result.State = ResultEnum.UPDATE_SUCCESS;
                        }
                        else
                        {
                            res.Result.State = ResultEnum.FAIL;
                        }
                    }
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
            Log("HR_CALENDARUpdateRes=" + json);
            return json;
        }

        public string HR_CALENDARDelete()
        {
            HR_CALENDARModifyRes res = new HR_CALENDARModifyRes();
            try
            {
                if (Session["ID"] == null)
                {
                    res.Result.State = ResultEnum.SESSION_TIMEOUT;
                }
                else
                {
                    string input = RequestData();
                    Log("HR_CALENDARDeleteReq=" + input);
                    HR_CALENDARModifyReq req = new HR_CALENDARModifyReq();
                    JsonConvert.PopulateObject(input, req);

                    req.HR_CALENDAR.MUSER = Session["ID"].ToString();

                    int effect = new HR_CALENDARImplement("TP_ALERT").DataDelete(req);
                    if (effect > 0)
                    {
                        res.Result.State = ResultEnum.DELETE_SUCCESS;
                    }
                    else
                    {
                        res.Result.State = ResultEnum.FAIL;
                    }
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
            Log("HR_CALENDARDeleteRes=" + json);
            return json;
        }

    }
}