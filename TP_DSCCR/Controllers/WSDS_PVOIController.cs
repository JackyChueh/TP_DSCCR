using System;
using System.Web.Mvc;
using System.IO;
using Newtonsoft.Json;
using TP_DSCCR.ViewModels;
using TP_DSCCR.Models.Implement;
using TP_DSCCR.Models.Enums;

namespace TP_DSCCR.Controllers
{
    public class WSDS_PVOIController : BaseController
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
        public string WSDS_PVOIRetrieve()
        {
            //System.Threading.Thread.Sleep(2000);
            WSDS_PVOIDataRes res = new WSDS_PVOIDataRes();
            try
            {
                if (Session["ID"] == null)
                {
                    res.Result.State = ResultEnum.SESSION_TIMEOUT;
                }
                else
                {
                    string input = RequestData();
                    Log("WSDS_PVOIDataReq=" + input);
                    WSDS_PVOIDataReq req = new WSDS_PVOIDataReq();
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

                    res = new WSDS_PVOIImplement("TP_DSCCR").PaginationRetrieve(req);
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
            Log("WSDS_PVOIDataRes=" + json);
            return json;
        }

        [AcceptVerbs("POST")]
        public string WSDS_PVOIExcel()
        {
            WSDS_PVOIExcelRes res = new WSDS_PVOIExcelRes();
            try
            {
                if (Session["ID"] == null)
                {
                    res.Result.State = ResultEnum.SESSION_TIMEOUT;
                }
                else
                {
                    string input = RequestData();
                    Log("WSDS_PVOIExcelReq=" + input);
                    WSDS_PVOIExcelReq req = new WSDS_PVOIExcelReq();
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

                    MemoryStream MemoryStream = new WSDS_PVOIImplement("TP_DSCCR").ExcelRetrieve(req);

                    if (MemoryStream.Length > 0)
                    {
                        string DataId = Guid.NewGuid().ToString();
                        TempData[DataId] = MemoryStream.ToArray();
                        MemoryStream.Dispose();

                        res.DataId = DataId;
                        res.FileName = "給排水-水泵及閥運轉狀態.xlsx";
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
            Log("WSDS_PVOIExcelRes=" + json);
            return json;
        }

        [AcceptVerbs("POST")]
        public string WSDS_PVOIGraph()
        {
            WSDS_PVOIGraphRes res = new WSDS_PVOIGraphRes();
            try
            {
                if (Session["ID"] == null)
                {
                    res.Result.State = ResultEnum.SESSION_TIMEOUT;
                }
                else
                {
                    string input = RequestData();
                    Log("WSDS_PVOIGraphReq=" + input);
                    WSDS_PVOIGraphReq req = new WSDS_PVOIGraphReq();
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

                    res = new WSDS_PVOIImplement("TP_DSCCR").GraphRetrieve(req);
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
            Log("WSDS_PVOIGraphRes=" + json);
            return json;
        }



    }
}