using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TP_DSCCR.Models.Implement;
using TP_DSCCR.Models.Data;

namespace TP_DSCCR.Controllers
{
    public class MainController : Controller
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
    }
}