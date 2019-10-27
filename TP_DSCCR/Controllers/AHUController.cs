using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TP_DSCCR.ViewModels;
using TP_DSCCR.Models.Implement;
using Newtonsoft.Json;
using TP_DSCCR.Models.Enums;
using TP_DSCCR.Models.Entity;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.IO;

namespace TP_DSCCR.Controllers
{
    public class AHUController : BaseController
    {
        public ActionResult Data()
        {
            return View();
        }

        public ActionResult Graph()
        {
            return View();
        }

        [AcceptVerbs("POST")]
        public string AHURetrieve()
        {
            //System.Threading.Thread.Sleep(2000);
            AHUDataRes res = new AHUDataRes();
            try
            {
                string input = RequestData();
                Log("Req=" + input);
                AHUDataReq req = new AHUDataReq();
                JsonConvert.PopulateObject(input, req);

                res = new AHUImplement("TP_DSCCR").PaginationRetrieve(req);
                res.Result.State = ResultEnum.SUCCESS;
            }
            catch (Exception ex)
            {
                res.Result.State = ResultEnum.EXCEPTION_ERROR;
                Log("Err=" + ex.Message);
                Log(ex.StackTrace);
            }
            var json = JsonConvert.SerializeObject(res);
            Log("Res=" + json);
            return json;
        }

        [AcceptVerbs("POST")]
        public string AHUExcel()
        {
            //System.Threading.Thread.Sleep(2000);

            AHUExcelRes res = new AHUExcelRes();
            try
            {
                string input = RequestData();
                Log("Req=" + input);
                AHUExcelReq req = new AHUExcelReq();
                JsonConvert.PopulateObject(input, req);

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
            catch (Exception ex)
            {
                res.Result.State = ResultEnum.EXCEPTION_ERROR;
                Log("Err=" + ex.Message);
                Log(ex.StackTrace);
            }
            var json = JsonConvert.SerializeObject(res);
            Log("Res=" + json);
            return json;
        }

        [AcceptVerbs("POST")]
        public string AHUGraph()
        {
            AHUGraphRes res = new AHUGraphRes();
            try
            {
                string input = RequestData();
                Log("Req=" + input);
                AHUGraphReq req = new AHUGraphReq();
                JsonConvert.PopulateObject(input, req);

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
            catch (Exception ex)
            {
                res.Result.State = ResultEnum.EXCEPTION_ERROR;
                Log("Err=" + ex.Message);
                Log(ex.StackTrace);
            }
            var json = JsonConvert.SerializeObject(res);
            Log("Res=" + json);
            return json;
        }



    }
}