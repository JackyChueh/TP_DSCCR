using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TP_DSCCR.Models.Implement;
using TP_DSCCR.Models.Data;
using TP_DSCCR.ViewModels;
using TP_DSCCR.Models.Enums;
using Newtonsoft.Json;

namespace TP_DSCCR.Controllers
{
    public class MainController : BaseController
    {
        // GET: Main
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Sidebar()
        {
            Main.SidebarRes res = new Main.SidebarRes();
            res.SidebarItem= new AuthorityImplement("TP_SCC").UserFunctionAuthority();
            return View(res);
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
            var settings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore,
            };
            var json = JsonConvert.SerializeObject(res, Formatting.Indented);
            Log("Res=" + json);
            return json;
        }
    }
}