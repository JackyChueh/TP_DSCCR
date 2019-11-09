using System;
using System.Web.Mvc;
using System.IO;
using Newtonsoft.Json;
using TP_DSCCR.ViewModels;
using TP_DSCCR.Models.Implement;
using TP_DSCCR.Models.Enums;

namespace TP_DSCCR.Controllers
{
    public class CPController : BaseController
    {
        public ActionResult Data()
        {
            if (Session["ID"] == null)
            {
                return RedirectToAction("Login", "Main");
            }
            return View();
        }

        public ActionResult Graph()
        {
            if (Session["ID"] == null)
            {
                return RedirectToAction("Login", "Main");
            }
            return View();
        }

        [AcceptVerbs("POST")]
        public string CPRetrieve()
        {
            //System.Threading.Thread.Sleep(2000);
            CPDataRes res = new CPDataRes();
            try
            {
                if (Session["ID"] == null)
                {
                    res.Result.State = ResultEnum.SESSION_TIMEOUT;
                }
                else
                {
                    string input = RequestData();
                    Log("CPDataReq=" + input);
                    CPDataReq req = new CPDataReq();
                    JsonConvert.PopulateObject(input, req);

                    res = new CPImplement("TP_DSCCR").PaginationRetrieve(req);
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
            Log("CPDataRes=" + json);
            return json;
        }

        [AcceptVerbs("POST")]
        public string CPExcel()
        {
            CPExcelRes res = new CPExcelRes();
            try
            {
                if (Session["ID"] == null)
                {
                    res.Result.State = ResultEnum.SESSION_TIMEOUT;
                }
                else
                {
                    string input = RequestData();
                    Log("CPExcelReq=" + input);
                    CPExcelReq req = new CPExcelReq();
                    JsonConvert.PopulateObject(input, req);

                    MemoryStream MemoryStream = new CPImplement("TP_DSCCR").ExcelRetrieve(req);

                    if (MemoryStream.Length > 0)
                    {
                        string DataId = Guid.NewGuid().ToString();
                        TempData[DataId] = MemoryStream.ToArray();
                        MemoryStream.Dispose();

                        res.DataId = DataId;
                        res.FileName = "冰水泵.xlsx";
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
            Log("CPExcelRes=" + json);
            return json;
        }

        [AcceptVerbs("POST")]
        public string CPGraph()
        {
            CPGraphRes res = new CPGraphRes();
            try
            {
                if (Session["ID"] == null)
                {
                    res.Result.State = ResultEnum.SESSION_TIMEOUT;
                }
                else
                {
                    string input = RequestData();
                    Log("CPGraphReq=" + input);
                    CPGraphReq req = new CPGraphReq();
                    JsonConvert.PopulateObject(input, req);

                    res = new CPImplement("TP_DSCCR").GraphRetrieve(req);
                    if (res.Chart == null)
                    {
                        res.Result.State = ResultEnum.DATA_NOT_FOUND;
                    }
                    else
                    {
                        res.Result.State = ResultEnum.SUCCESS;
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
            Log("CPGraphRes=" + json);
            return json;
        }



    }
}