﻿using System;
using System.Web.Mvc;
using System.IO;
using Newtonsoft.Json;
using TP_DSCCR.ViewModels;
using TP_DSCCR.Models.Implement;
using TP_DSCCR.Models.Enums;

namespace TP_DSCCR.Controllers
{
    public class AHUController : BaseController
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
        public string AHURetrieve()
        {
            //System.Threading.Thread.Sleep(2000);
            AHUDataRes res = new AHUDataRes();
            try
            {
                if (Session["ID"]==null)
                {
                    res.Result.State = ResultEnum.SESSION_TIMEOUT;
                }
                else
                {
                    string input = RequestData();
                    Log("AHUDataReq=" + input);
                    AHUDataReq req = new AHUDataReq();
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
                    res = new AHUImplement("TP_DSCCR").PaginationRetrieve(req);
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
            Log("AHUDataRes=" + json);
            return json;
        }

        [AcceptVerbs("POST")]
        public string AHUExcel()
        {
            AHUExcelRes res = new AHUExcelRes();
            try
            {
                if (Session["ID"] == null)
                {
                    res.Result.State = ResultEnum.SESSION_TIMEOUT;
                }
                else
                {
                    string input = RequestData();
                    Log("AHUExcelReq=" + input);
                    AHUExcelReq req = new AHUExcelReq();
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
                    MemoryStream MemoryStream = new AHUImplement("TP_DSCCR").ExcelRetrieve(req);

                    if (MemoryStream.Length > 0)
                    {
                        string DataId = Guid.NewGuid().ToString();
                        TempData[DataId] = MemoryStream.ToArray();
                        MemoryStream.Dispose();

                        res.DataId = DataId;
                        res.FileName = "空調箱.xlsx";
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
            Log("AHUExcelRes=" + json);
            return json;
        }

        [AcceptVerbs("POST")]
        public string AHUGraph()
        {
            AHUGraphRes res = new AHUGraphRes();
            try
            {
                if (Session["ID"] == null)
                {
                    res.Result.State = ResultEnum.SESSION_TIMEOUT;
                }
                else
                {
                    string input = RequestData();
                    Log("AHUGraphReq=" + input);
                    AHUGraphReq req = new AHUGraphReq();
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
                    res = new AHUImplement("TP_DSCCR").GraphRetrieve(req);
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
            Log("AHUGraphRes=" + json);
            return json;
        }



    }
}