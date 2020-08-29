using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TP_DSCCR.Controllers
{
    public class ALERT_CONFIGController : Controller
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
    }
}