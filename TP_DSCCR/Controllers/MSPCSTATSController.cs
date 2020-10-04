using System;
using System.Web.Mvc;
using System.IO;
using Newtonsoft.Json;
using TP_DSCCR.ViewModels;
using TP_DSCCR.Models.Implement;
using TP_DSCCR.Models.Enums;

namespace TP_DSCCR.Controllers
{
    public class MSPCSTATSController : BaseController
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
        public string MSPCSTATSRetrieve()
        {
            //System.Threading.Thread.Sleep(2000);
            MSPCSTATSDataRes res = new MSPCSTATSDataRes();
            try
            {
                if (Session["ID"] == null)
                {
                    res.Result.State = ResultEnum.SESSION_TIMEOUT;
                }
                else
                {
                    string input = RequestData();
                    Log("MSPCSTATSDataReq=" + input);
                    MSPCSTATSDataReq req = new MSPCSTATSDataReq();
                    JsonConvert.PopulateObject(input, req);

                    res = new MSPCSTATSImplement("TP_DSCCR").PaginationRetrieve(req);
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
            Log("MSPCSTATSDataRes=" + json);
            return json;
        }

        [AcceptVerbs("POST")]
        public string MSPCSTATSExcel()
        {
            MSPCSTATSExcelRes res = new MSPCSTATSExcelRes();
            try
            {
                if (Session["ID"] == null)
                {
                    res.Result.State = ResultEnum.SESSION_TIMEOUT;
                }
                else
                {
                    string input = RequestData();
                    Log("MSPCSTATSExcelReq=" + input);
                    MSPCSTATSExcelReq req = new MSPCSTATSExcelReq();
                    JsonConvert.PopulateObject(input, req);

                    MemoryStream MemoryStream = new MSPCSTATSImplement("TP_DSCCR").ExcelRetrieve(req);

                    if (MemoryStream.Length > 0)
                    {
                        string DataId = Guid.NewGuid().ToString();
                        TempData[DataId] = MemoryStream.ToArray();
                        MemoryStream.Dispose();

                        res.DataId = DataId;
                        res.FileName = "區域泵.xlsx";
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
            Log("MSPCSTATSExcelRes=" + json);
            return json;
        }

        [AcceptVerbs("POST")]
        public string MSPCSTATSGraph()
        {
            MSPCSTATSGraphRes res = new MSPCSTATSGraphRes();
            try
            {
                if (Session["ID"] == null)
                {
                    res.Result.State = ResultEnum.SESSION_TIMEOUT;
                }
                else
                {
                    string input = RequestData();
                    Log("MSPCSTATSGraphReq=" + input);
                    MSPCSTATSGraphReq req = new MSPCSTATSGraphReq();
                    JsonConvert.PopulateObject(input, req);

                    res = new MSPCSTATSImplement("TP_DSCCR").GraphRetrieve(req);
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
            Log("MSPCSTATSGraphRes=" + json);
            return json;
        }



    }
}