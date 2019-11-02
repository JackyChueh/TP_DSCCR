using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Newtonsoft.Json;
using TP_DSCCR.Models.Implement;
using TP_DSCCR.Models.Data;
using TP_DSCCR.ViewModels;
using TP_DSCCR.Models.Enums;
using TP_DSCCR.Models.Entity;


namespace TP_DSCCR.Controllers
{
    public class MainController : BaseController
    {
        public ActionResult Error()
        {
            return View();
        }

        public ActionResult Login()
        {
            Session["ID"] = null;
            return View();
        }

        public ActionResult Dashboard()
        {
            if (Session["ID"] == null)
            {
                return View("Error");
            }
            return View();
        }

        public ActionResult Sidebar()
        {
            Main.SidebarRes res = new Main.SidebarRes();
            res.SidebarItem = new AuthorityImplement("TP_SCC").UserFunctionAuthority();
            return View(res);
        }

        [AcceptVerbs("POST")]
        public string LoginCheck()
        {
            LoginCheckRes res = new LoginCheckRes();
            try
            {
                string input = RequestData();
                Log("LoginCheckReq=" + input);
                LoginCheckReq req = new LoginCheckReq();
                JsonConvert.PopulateObject(input, req);

                if (!(string.IsNullOrEmpty(req.USERS.ID) || string.IsNullOrEmpty(req.USERS.PASSWORD)))
                {
                    res = new AuthorityImplement("TP_SCC").LoginCheck(req);
                }

                if (res.USERS == null)
                {
                    res.Result.State = ResultEnum.LOGIN_FAIL;
                }
                else
                {
                    Session["ID"] = res.USERS.ID;
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
            Log("LoginCheckRes=" + json);
            return json;
        }

        public string ItemListRetrieve(ItemListRetrieveReq req)
        {
            ItemListRetrieveRes res = new ItemListRetrieveRes();
            try
            {
                res = new ItemListImplement("TP_SCC").ItemListQuery(req);
                res.Result.State = ResultEnum.SUCCESS;
            }
            catch (Exception ex)
            {
                Log("Err=" + ex.Message);
                Log(ex.StackTrace);
                res.Result.State = ResultEnum.FAIL;
            }
            var json = JsonConvert.SerializeObject(res);
            Log("Res=" + json);
            return json;
        }

        public string SubItemListRetrieve(SubItemListRetrieveReq req)
        {
            SubItemListRetrieveRes res = new SubItemListRetrieveRes();
            try
            {
                res.SubItemList = new ItemListImplement("TP_SCC").SubItemListQuery(req.PhraseGroup, req.ParentKey);
                res.Result.State = ResultEnum.SUCCESS;
            }
            catch (Exception ex)
            {
                Log("Err=" + ex.Message);
                Log(ex.StackTrace);
                res.Result.State = ResultEnum.FAIL;
            }
            var json = JsonConvert.SerializeObject(res);
            Log("Res=" + json);
            return json;
        }

        [HttpGet]
        public ActionResult ExcelDownload(string DataId, string FileName)
        {
            try
            {
                if (TempData[DataId] != null)
                {
                    byte[] data = TempData[DataId] as byte[];
                    return File(data, "application/vnd.ms-excel", FileName);
                }
                else
                {
                    Log("TempData[{0}] is null", DataId);
                }
            }
            catch (Exception ex)
            {
                Log("Err=" + ex.Message);
                Log(ex.StackTrace);
            }
            return new EmptyResult();
        }
    }

}