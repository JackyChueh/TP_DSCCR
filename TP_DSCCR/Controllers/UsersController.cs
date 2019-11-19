using System;
using System.Web.Mvc;
using System.IO;
using Newtonsoft.Json;
using TP_DSCCR.ViewModels;
using TP_DSCCR.Models.Implement;
using TP_DSCCR.Models.Enums;

namespace TP_DSCCR.Controllers
{
    public class UsersController : BaseController
    {
        // GET: Users
        public ActionResult UsersIndex()
        {
            if (Session["ID"] == null)
            {
                return RedirectToAction("Login", "Main");
            }
            return View();
        }

        public string UsersRetrieve()
        {
            UsersRetrieveRes res = new UsersRetrieveRes();
            try
            {
                if (Session["ID"] == null)
                {
                    res.Result.State = ResultEnum.SESSION_TIMEOUT;
                }
                else
                {
                    string input = RequestData();
                    Log("UsersDataAccessReq=" + input);
                    UsersRetrieveReq req = new UsersRetrieveReq();
                    JsonConvert.PopulateObject(input, req);

                    res = new UsersImplement("TP_SCC").PaginationRetrieve(req);
                    res.Result.State = ResultEnum.SUCCESS;
                }
            }
            catch (Exception ex)
            {
                res.Result.State = ResultEnum.EXCEPTION_ERROR;
                Log("Err=" + ex.Message);
                Log(ex.StackTrace);
            }
            var json = JsonConvert.SerializeObject(res);
            Log("UsersDataAccessRes=" + json);
            return json;
        }

        public string UsersQuery()
        {
            UsersModifyRes res = new UsersModifyRes();
            try
            {
                if (Session["ID"] == null)
                {
                    res.Result.State = ResultEnum.SESSION_TIMEOUT;
                }
                else
                {
                    string input = RequestData();
                    Log("UsersDataAccessReq=" + input);
                    UsersModifyReq req = new UsersModifyReq();
                    JsonConvert.PopulateObject(input, req);

                    var USERS = new UsersImplement("TP_SCC").ModificationQuery(req);
                    if (USERS != null)
                    {
                        res.USERS = USERS;
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
            Log("UsersDataAccessRes=" + json);
            return json;
        }

        public string UsersCreate()
        {
            UsersModifyRes res = new UsersModifyRes();
            try
            {
                if (Session["ID"] == null)
                {
                    res.Result.State = ResultEnum.SESSION_TIMEOUT;
                }
                else
                {
                    string input = RequestData();
                    Log("UsersDataAccessReq=" + input);
                    UsersModifyReq req = new UsersModifyReq();
                    JsonConvert.PopulateObject(input, req);

                    req.USERS.CUSER = Session["ID"].ToString();
                    req.USERS.MUSER = Session["ID"].ToString();

                    int effect = new UsersImplement("TP_SCC").DataCreate(req);
                    if (effect > 0)
                    {
                        res.USERS = req.USERS;
                        res.Result.State = ResultEnum.CREATE_SUCCESS;
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
            var json = JsonConvert.SerializeObject(res);
            Log("UsersDataAccessRes=" + json);
            return json;
        }

        public string UsersUpdate()
        {
            UsersModifyRes res = new UsersModifyRes();
            try
            {
                if (Session["ID"] == null)
                {
                    res.Result.State = ResultEnum.SESSION_TIMEOUT;
                }
                else
                {
                    string input = RequestData();
                    Log("UsersDataAccessReq=" + input);
                    UsersModifyReq req = new UsersModifyReq();
                    JsonConvert.PopulateObject(input, req);

                    req.USERS.MUSER = Session["ID"].ToString();

                    int effect = new UsersImplement("TP_SCC").DataUpdate(req);
                    if (effect > 0)
                    {
                        res.USERS = req.USERS;
                        res.Result.State = ResultEnum.UPDATE_SUCCESS;
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
            var json = JsonConvert.SerializeObject(res);
            Log("UsersDataAccessRes=" + json);
            return json;
        }

        public string UsersDelete()
        {
            UsersModifyRes res = new UsersModifyRes();
            try
            {
                if (Session["ID"] == null)
                {
                    res.Result.State = ResultEnum.SESSION_TIMEOUT;
                }
                else
                {
                    string input = RequestData();
                    Log("UsersDataAccessReq=" + input);
                    UsersModifyReq req = new UsersModifyReq();
                    JsonConvert.PopulateObject(input, req);

                    req.USERS.MUSER = Session["ID"].ToString();

                    int effect = new UsersImplement("TP_SCC").DataDelete(req);
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
            var json = JsonConvert.SerializeObject(res);
            Log("UsersDataAccessRes=" + json);
            return json;
        }

        public string UsersReset()
        {
            UsersModifyRes res = new UsersModifyRes();
            try
            {
                if (Session["ID"] == null)
                {
                    res.Result.State = ResultEnum.SESSION_TIMEOUT;
                }
                else
                {
                    string input = RequestData();
                    Log("UsersDataAccessReq=" + input);
                    UsersModifyReq req = new UsersModifyReq();
                    JsonConvert.PopulateObject(input, req);

                    req.USERS.PASSWORD = req.USERS.ID;
                    req.USERS.FORCE_PWD = 1;
                    req.USERS.MUSER = Session["ID"].ToString();
                    
                    int effect = new UsersImplement("TP_SCC").DataReset(req);
                    if (effect > 0)
                    {
                        res.USERS = req.USERS;
                        res.USERS.PASSWORD = null;
                        res.Result.State = ResultEnum.RESET_SUCCESS;
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
            var json = JsonConvert.SerializeObject(res);
            Log("UsersDataAccessRes=" + json);
            return json;
        }

    }
}