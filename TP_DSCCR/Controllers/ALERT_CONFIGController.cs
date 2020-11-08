using System;
using System.Web.Mvc;
using Newtonsoft.Json;
using TP_DSCCR.ViewModels;
using TP_DSCCR.Models.Implement;
using TP_DSCCR.Models.Enums;

namespace TP_DSCCR.Controllers
{
    public class ALERT_CONFIGController : BaseController
    {
        // GET: ALERT_CONFIG
        public ActionResult ALERT_CONFIGIndex()
        {
            if (Session["ID"] == null)
            {
                return RedirectToAction("Login", "Main");
            }
            return View();
        }

        public string ALERT_CONFIGRetrieve()
        {
            ALERT_CONFIGRetrieveRes res = new ALERT_CONFIGRetrieveRes();
            try
            {
                if (Session["ID"] == null)
                {
                    res.Result.State = ResultEnum.SESSION_TIMEOUT;
                }
                else
                {
                    string input = RequestData();
                    Log("ALERT_CONFIGRetrieveReq=" + input);
                    ALERT_CONFIGRetrieveReq req = new ALERT_CONFIGRetrieveReq();
                    JsonConvert.PopulateObject(input, req);

                    res = new ALERT_CONFIGImplement("TP_ALERT").PaginationRetrieve(req);
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
            Log("ALERT_CONFIGRetrieveRes=" + json);
            return json;
        }

        public string ALERT_CONFIGQuery()
        {
            ALERT_CONFIGModifyRes res = new ALERT_CONFIGModifyRes();
            try
            {
                if (Session["ID"] == null)
                {
                    res.Result.State = ResultEnum.SESSION_TIMEOUT;
                }
                else
                {
                    string input = RequestData();
                    Log("ALERT_CONFIGQueryReq=" + input);
                    ALERT_CONFIGModifyReq req = new ALERT_CONFIGModifyReq();
                    JsonConvert.PopulateObject(input, req);

                    res = new ALERT_CONFIGImplement("TP_ALERT").ModificationQuery(req);
                    if (res.ALERT_CONFIG != null)
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
            Log("ALERT_CONFIGQueryRes=" + json);
            return json;
        }

        public string ALERT_CONFIGCreate()
        {
            ALERT_CONFIGModifyRes res = new ALERT_CONFIGModifyRes();
            try
            {
                if (Session["ID"] == null)
                {
                    res.Result.State = ResultEnum.SESSION_TIMEOUT;
                }
                else
                {
                    string input = RequestData();
                    Log("ALERT_CONFIGCreateReq=" + input);
                    ALERT_CONFIGModifyReq req = new ALERT_CONFIGModifyReq();
                    JsonConvert.PopulateObject(input, req);

                    int? sid = new ALERT_CONFIGImplement("TP_ALERT").DataDuplicate(req);
                    if (sid !=null)
                    {
                        res.Result.State = ResultEnum.DATA_DUPLICATION;
                    }
                    else
                    {
                        req.ALERT_CONFIG.CUSER = Session["ID"].ToString();
                        req.ALERT_CONFIG.MUSER = Session["ID"].ToString();

                        req.ALERT_CONFIG.CHECK_DATE = DateTime.Now.AddMinutes(-(req.ALERT_CONFIG.CHECK_INTERVAL + 1));

                        bool success = new ALERT_CONFIGImplement("TP_ALERT").DataCreate(req);
                        if (success)
                        {
                            res.ALERT_CONFIG = req.ALERT_CONFIG;
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
            Log("ALERT_CONFIGCreateRes=" + json);
            return json;
        }

        public string ALERT_CONFIGUpdate()
        {
            ALERT_CONFIGModifyRes res = new ALERT_CONFIGModifyRes();
            try
            {
                if (Session["ID"] == null)
                {
                    res.Result.State = ResultEnum.SESSION_TIMEOUT;
                }
                else
                {
                    string input = RequestData();
                    Log("ALERT_CONFIGUpdateReq=" + input);
                    ALERT_CONFIGModifyReq req = new ALERT_CONFIGModifyReq();
                    JsonConvert.PopulateObject(input, req);

                    int? sid = new ALERT_CONFIGImplement("TP_ALERT").DataDuplicate(req);
                    if (sid !=null && req.ALERT_CONFIG.SID != sid)
                    {
                        res.Result.State = ResultEnum.DATA_DUPLICATION;
                    }
                    else
                    {
                        req.ALERT_CONFIG.MUSER = Session["ID"].ToString();
                        req.ALERT_CONFIG.CHECK_DATE = DateTime.Now.AddMinutes(-(req.ALERT_CONFIG.CHECK_INTERVAL + 1));

                        bool success = new ALERT_CONFIGImplement("TP_ALERT").DataUpdate(req);
                        if (success)
                        {
                            res.ALERT_CONFIG = req.ALERT_CONFIG;
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
            Log("ALERT_CONFIGUpdateRes=" + json);
            return json;
        }

        public string ALERT_CONFIGDelete()
        {
            ALERT_CONFIGModifyRes res = new ALERT_CONFIGModifyRes();
            try
            {
                if (Session["ID"] == null)
                {
                    res.Result.State = ResultEnum.SESSION_TIMEOUT;
                }
                else
                {
                    string input = RequestData();
                    Log("ALERT_CONFIGDeleteReq=" + input);
                    ALERT_CONFIGModifyReq req = new ALERT_CONFIGModifyReq();
                    JsonConvert.PopulateObject(input, req);

                    req.ALERT_CONFIG.MUSER = Session["ID"].ToString();

                    int effect = new ALERT_CONFIGImplement("TP_ALERT").DataDelete(req);
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
            Log("ALERT_CONFIGDeleteRes=" + json);
            return json;
        }
    }
}