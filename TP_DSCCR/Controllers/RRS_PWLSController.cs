using System;
using System.Web.Mvc;
using System.IO;
using Newtonsoft.Json;
using TP_DSCCR.ViewModels;
using TP_DSCCR.Models.Implement;
using TP_DSCCR.Models.Enums;

namespace TP_DSCCR.Controllers
{
    public class RRS_PWLSController : BaseController
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
        public string RRS_PWLSRetrieve()
        {
            //System.Threading.Thread.Sleep(2000);
            RRS_PWLSDataRes res = new RRS_PWLSDataRes();
            try
            {
                if (Session["ID"] == null)
                {
                    res.Result.State = ResultEnum.SESSION_TIMEOUT;
                }
                else
                {
                    string input = RequestData();
                    Log("RRS_PWLSDataReq=" + input);
                    RRS_PWLSDataReq req = new RRS_PWLSDataReq();
                    JsonConvert.PopulateObject(input, req);

                    res = new RRS_PWLSImplement("TP_DSCCR").PaginationRetrieve(req);
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
            Log("RRS_PWLSDataRes=" + json);
            return json;
        }

        [AcceptVerbs("POST")]
        public string RRS_PWLSExcel()
        {
            RRS_PWLSExcelRes res = new RRS_PWLSExcelRes();
            try
            {
                if (Session["ID"] == null)
                {
                    res.Result.State = ResultEnum.SESSION_TIMEOUT;
                }
                else
                {
                    string input = RequestData();
                    Log("RRS_PWLSExcelReq=" + input);
                    RRS_PWLSExcelReq req = new RRS_PWLSExcelReq();
                    JsonConvert.PopulateObject(input, req);

                    MemoryStream MemoryStream = new RRS_PWLSImplement("TP_DSCCR").ExcelRetrieve(req);

                    if (MemoryStream.Length > 0)
                    {
                        string DataId = Guid.NewGuid().ToString();
                        TempData[DataId] = MemoryStream.ToArray();
                        MemoryStream.Dispose();

                        res.DataId = DataId;
                        res.FileName = "雨水-水泵及水位警報.xlsx";
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
            Log("RRS_PWLSExcelRes=" + json);
            return json;
        }

        [AcceptVerbs("POST")]
        public string RRS_PWLSGraph()
        {
            RRS_PWLSGraphRes res = new RRS_PWLSGraphRes();
            try
            {
                if (Session["ID"] == null)
                {
                    res.Result.State = ResultEnum.SESSION_TIMEOUT;
                }
                else
                {
                    string input = RequestData();
                    Log("RRS_PWLSGraphReq=" + input);
                    RRS_PWLSGraphReq req = new RRS_PWLSGraphReq();
                    JsonConvert.PopulateObject(input, req);

                    res = new RRS_PWLSImplement("TP_DSCCR").GraphRetrieve(req);
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
            Log("RRS_PWLSGraphRes=" + json);
            return json;
        }



    }
}