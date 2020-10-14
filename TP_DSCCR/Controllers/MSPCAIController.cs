using System;
using System.Web.Mvc;
using System.IO;
using Newtonsoft.Json;
using TP_DSCCR.ViewModels;
using TP_DSCCR.Models.Implement;
using TP_DSCCR.Models.Enums;

namespace TP_DSCCR.Controllers
{
    public class MSPCAIController : BaseController
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
        public string MSPCAIRetrieve()
        {
            //System.Threading.Thread.Sleep(2000);
            MSPCAIDataRes res = new MSPCAIDataRes();
            try
            {
                if (Session["ID"] == null)
                {
                    res.Result.State = ResultEnum.SESSION_TIMEOUT;
                }
                else
                {
                    string input = RequestData();
                    Log("MSPCAIDataReq=" + input);
                    MSPCAIDataReq req = new MSPCAIDataReq();
                    JsonConvert.PopulateObject(input, req);

                    res = new MSPCAIImplement("TP_DSCCR").PaginationRetrieve(req);
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
            Log("MSPCAIDataRes=" + json);
            return json;
        }

        [AcceptVerbs("POST")]
        public string MSPCAIExcel()
        {
            MSPCAIExcelRes res = new MSPCAIExcelRes();
            try
            {
                if (Session["ID"] == null)
                {
                    res.Result.State = ResultEnum.SESSION_TIMEOUT;
                }
                else
                {
                    string input = RequestData();
                    Log("MSPCAIExcelReq=" + input);
                    MSPCAIExcelReq req = new MSPCAIExcelReq();
                    JsonConvert.PopulateObject(input, req);

                    MemoryStream MemoryStream = new MSPCAIImplement("TP_DSCCR").ExcelRetrieve(req);

                    if (MemoryStream.Length > 0)
                    {
                        string DataId = Guid.NewGuid().ToString();
                        TempData[DataId] = MemoryStream.ToArray();
                        MemoryStream.Dispose();

                        res.DataId = DataId;
                        res.FileName = "箱冷AI.xlsx";
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
            Log("MSPCAIExcelRes=" + json);
            return json;
        }

        [AcceptVerbs("POST")]
        public string MSPCAIGraph()
        {
            MSPCAIGraphRes res = new MSPCAIGraphRes();
            try
            {
                if (Session["ID"] == null)
                {
                    res.Result.State = ResultEnum.SESSION_TIMEOUT;
                }
                else
                {
                    string input = RequestData();
                    Log("MSPCAIGraphReq=" + input);
                    MSPCAIGraphReq req = new MSPCAIGraphReq();
                    JsonConvert.PopulateObject(input, req);

                    res = new MSPCAIImplement("TP_DSCCR").GraphRetrieve(req);
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
            Log("MSPCAIGraphRes=" + json);
            return json;
        }



    }
}