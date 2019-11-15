using System;
using System.Web.Mvc;
using System.IO;
using Newtonsoft.Json;
using TP_DSCCR.ViewModels;
using TP_DSCCR.Models.Implement;
using TP_DSCCR.Models.Enums;

namespace TP_DSCCR.Controllers
{
    public class WSDS_PWLSController : BaseController
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
        public string WSDS_PWLSRetrieve()
        {
            //System.Threading.Thread.Sleep(2000);
            WSDS_PWLSDataRes res = new WSDS_PWLSDataRes();
            try
            {
                if (Session["ID"] == null)
                {
                    res.Result.State = ResultEnum.SESSION_TIMEOUT;
                }
                else
                {
                    string input = RequestData();
                    Log("WSDS_PWLSDataReq=" + input);
                    WSDS_PWLSDataReq req = new WSDS_PWLSDataReq();
                    JsonConvert.PopulateObject(input, req);

                    res = new WSDS_PWLSImplement("TP_DSCCR").PaginationRetrieve(req);
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
            Log("WSDS_PWLSDataRes=" + json);
            return json;
        }

        [AcceptVerbs("POST")]
        public string WSDS_PWLSExcel()
        {
            WSDS_PWLSExcelRes res = new WSDS_PWLSExcelRes();
            try
            {
                if (Session["ID"] == null)
                {
                    res.Result.State = ResultEnum.SESSION_TIMEOUT;
                }
                else
                {
                    string input = RequestData();
                    Log("WSDS_PWLSExcelReq=" + input);
                    WSDS_PWLSExcelReq req = new WSDS_PWLSExcelReq();
                    JsonConvert.PopulateObject(input, req);

                    MemoryStream MemoryStream = new WSDS_PWLSImplement("TP_DSCCR").ExcelRetrieve(req);

                    if (MemoryStream.Length > 0)
                    {
                        string DataId = Guid.NewGuid().ToString();
                        TempData[DataId] = MemoryStream.ToArray();
                        MemoryStream.Dispose();

                        res.DataId = DataId;
                        res.FileName = "給排水-水泵及水位警報.xlsx";
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
            Log("WSDS_PWLSExcelRes=" + json);
            return json;
        }

        [AcceptVerbs("POST")]
        public string WSDS_PWLSGraph()
        {
            WSDS_PWLSGraphRes res = new WSDS_PWLSGraphRes();
            try
            {
                if (Session["ID"] == null)
                {
                    res.Result.State = ResultEnum.SESSION_TIMEOUT;
                }
                else
                {
                    string input = RequestData();
                    Log("WSDS_PWLSGraphReq=" + input);
                    WSDS_PWLSGraphReq req = new WSDS_PWLSGraphReq();
                    JsonConvert.PopulateObject(input, req);

                    res = new WSDS_PWLSImplement("TP_DSCCR").GraphRetrieve(req);
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
            Log("WSDS_PWLSGraphRes=" + json);
            return json;
        }



    }
}