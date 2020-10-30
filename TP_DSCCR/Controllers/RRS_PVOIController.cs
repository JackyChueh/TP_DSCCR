using System;
using System.Web.Mvc;
using System.IO;
using Newtonsoft.Json;
using TP_DSCCR.ViewModels;
using TP_DSCCR.Models.Implement;
using TP_DSCCR.Models.Enums;

namespace TP_DSCCR.Controllers
{
    public class RRS_PVOIController : BaseController
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
        public string RRS_PVOIRetrieve()
        {
            //System.Threading.Thread.Sleep(2000);
            RRS_PVOIDataRes res = new RRS_PVOIDataRes();
            try
            {
                if (Session["ID"] == null)
                {
                    res.Result.State = ResultEnum.SESSION_TIMEOUT;
                }
                else
                {
                    string input = RequestData();
                    Log("RRS_PVOIDataReq=" + input);
                    RRS_PVOIDataReq req = new RRS_PVOIDataReq();
                    JsonConvert.PopulateObject(input, req);

                    DateTime dt;
                    if (DateTime.TryParse(req.SDATE.ToString(), out dt) && DateTime.TryParse(req.EDATE.ToString(), out dt))
                    {
                        DateTime? temp;
                        if (req.SDATE > req.EDATE)
                        {
                            temp = req.SDATE;
                            req.SDATE = req.EDATE;
                            req.EDATE = temp;
                        }
                        req.EDATE = req.EDATE.Value.AddMinutes(1);
                    }

                    res = new RRS_PVOIImplement("TP_DSCCR").PaginationRetrieve(req);
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
            Log("RRS_PVOIDataRes=" + json);
            return json;
        }

        [AcceptVerbs("POST")]
        public string RRS_PVOIExcel()
        {
            RRS_PVOIExcelRes res = new RRS_PVOIExcelRes();
            try
            {
                if (Session["ID"] == null)
                {
                    res.Result.State = ResultEnum.SESSION_TIMEOUT;
                }
                else
                {
                    string input = RequestData();
                    Log("RRS_PVOIExcelReq=" + input);
                    RRS_PVOIExcelReq req = new RRS_PVOIExcelReq();
                    JsonConvert.PopulateObject(input, req);

                    DateTime dt;
                    if (DateTime.TryParse(req.SDATE.ToString(), out dt) && DateTime.TryParse(req.EDATE.ToString(), out dt))
                    {
                        DateTime? temp;
                        if (req.SDATE > req.EDATE)
                        {
                            temp = req.SDATE;
                            req.SDATE = req.EDATE;
                            req.EDATE = temp;
                        }
                        req.EDATE = req.EDATE.Value.AddMinutes(1);
                    }

                    MemoryStream MemoryStream = new RRS_PVOIImplement("TP_DSCCR").ExcelRetrieve(req);

                    if (MemoryStream.Length > 0)
                    {
                        string DataId = Guid.NewGuid().ToString();
                        TempData[DataId] = MemoryStream.ToArray();
                        MemoryStream.Dispose();

                        res.DataId = DataId;
                        res.FileName = "雨水-水泵及閥運轉狀態.xlsx";
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
            Log("RRS_PVOIExcelRes=" + json);
            return json;
        }

        [AcceptVerbs("POST")]
        public string RRS_PVOIGraph()
        {
            RRS_PVOIGraphRes res = new RRS_PVOIGraphRes();
            try
            {
                if (Session["ID"] == null)
                {
                    res.Result.State = ResultEnum.SESSION_TIMEOUT;
                }
                else
                {
                    string input = RequestData();
                    Log("RRS_PVOIGraphReq=" + input);
                    RRS_PVOIGraphReq req = new RRS_PVOIGraphReq();
                    JsonConvert.PopulateObject(input, req);

                    DateTime dt;
                    if (DateTime.TryParse(req.SDATE.ToString(), out dt) && DateTime.TryParse(req.EDATE.ToString(), out dt))
                    {
                        DateTime? temp;
                        if (req.SDATE > req.EDATE)
                        {
                            temp = req.SDATE;
                            req.SDATE = req.EDATE;
                            req.EDATE = temp;
                        }
                        req.EDATE = req.EDATE.Value.AddMinutes(1);
                    }

                    res = new RRS_PVOIImplement("TP_DSCCR").GraphRetrieve(req);
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
            Log("RRS_PVOIGraphRes=" + json);
            return json;
        }



    }
}