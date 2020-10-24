using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using TP_DSCCR.Models.Access;
using TP_DSCCR.Models.Entity;
using TP_DSCCR.ViewModels;
using System.IO;

namespace TP_DSCCR.Models.Implement
{
    public class ALERT_CONFIGImplement : EnterpriseLibrary
    {
        public ALERT_CONFIGImplement(string connectionStringName) : base(connectionStringName) { }

        public ALERT_CONFIGRetrieveRes PaginationRetrieve(ALERT_CONFIGRetrieveReq Req)
        {
            ALERT_CONFIGRetrieveRes res = new ALERT_CONFIGRetrieveRes()
            {
                ALERT_CONFIG = new List<ALERT_CONFIG_RETRIEVE>(),
                Pagination = new Pagination
                {
                    PageCount = 0,
                    RowCount = 0,
                    PageNumber = 0,
                    MinNumber = 0,
                    MaxNumber = 0,
                    StartTime = DateTime.Now
                }
            };

            using (DbCommand cmd = Db.CreateConnection().CreateCommand())
            {
                //                string sql = @"
                //SELECT TOP(100) SID,TP_SCC.dbo.PHRASE_NAME('mode',MODE,default) AS MODE, TP_SCC.dbo.PHRASE_NAME('DATA_TYPE',DATA_TYPE,default) AS DATA_TYPE
                //	,TP_SCC.dbo.PHRASE_NAME(DATA_TYPE+'_LOCATION',LOCATION,DATA_TYPE) AS LOCATION, TP_SCC.dbo.PHRASE_NAME(DATA_TYPE+'_DEVICE_ID',DEVICE_ID,LOCATION) AS DEVICE_ID
                //	, TP_SCC.dbo.PHRASE_NAME(DATA_TYPE+'_DATA_FIELD',DATA_FIELD,DATA_TYPE) AS DATA_FIELD,MAX_VALUE,MIN_VALUE,CHECK_INTERVAL,ALERT_INTERVAL
                //	,SUN,SUN_STIME,SUN_ETIME,MON,MON_STIME,MON_ETIME,TUE,TUE_STIME,TUE_ETIME,WED,WED_STIME,WED_ETIME,THU,THU_STIME
                //	,THU_ETIME,FRI,FRI_STIME,FRI_ETIME,STA,STA_STIME,STA_ETIME,CHECK_DATE,ALERT_DATE,MAIL_TO,CHECK_HR_CALENDAR
                //FROM ALERT_CONFIG
                //    {0}
                //";
                string sql = @"
SELECT TOP(100) SID,TP_SCC.dbo.PHRASE_NAME('mode',MODE,default) AS MODE, TP_SCC.dbo.PHRASE_NAME('DATA_TYPE',DATA_TYPE,default) AS DATA_TYPE
	,TP_SCC.dbo.PHRASE_NAME(DATA_TYPE+'_LOCATION',LOCATION,DATA_TYPE) AS LOCATION, TP_SCC.dbo.PHRASE_NAME(DATA_TYPE+'_DEVICE_ID',DEVICE_ID,LOCATION) AS DEVICE_ID
	, TP_SCC.dbo.PHRASE_NAME(DATA_TYPE+'_DATA_FIELD',DATA_FIELD,DATA_TYPE) AS DATA_FIELD
    ,CASE 
	    WHEN [DATA_TYPE]='AHU' AND [DATA_FIELD]='AHU01' THEN TP_SCC.[dbo].[PHRASE_NAME]('function_fail_AHU',[MAX_VALUE],[LOCATION]) 
	    WHEN [DATA_TYPE]='AHU' AND [DATA_FIELD]='AHU02' THEN TP_SCC.[dbo].[PHRASE_NAME]('switch_status',[MAX_VALUE],DEFAULT) 
	    WHEN [DATA_TYPE]='Chiller' AND [DATA_FIELD]='Chiller07' THEN TP_SCC.[dbo].[PHRASE_NAME]('running',[MAX_VALUE],DEFAULT) 
	    WHEN [DATA_TYPE]='Chiller' AND [DATA_FIELD]='Chiller08' THEN TP_SCC.[dbo].[PHRASE_NAME]('function_fail',[MAX_VALUE],DEFAULT) 
	    WHEN [DATA_TYPE]='Chiller' AND [DATA_FIELD]='Chiller09' THEN TP_SCC.[dbo].[PHRASE_NAME]('switch_status',[MAX_VALUE],DEFAULT) 
	    WHEN [DATA_TYPE]='COP' AND [DATA_FIELD]='COP01' THEN TP_SCC.[dbo].[PHRASE_NAME]('running',[MAX_VALUE],DEFAULT) 
	    WHEN [DATA_TYPE]='COP' AND [DATA_FIELD]='COP02' THEN TP_SCC.[dbo].[PHRASE_NAME]('function_fail',[MAX_VALUE],DEFAULT) 
	    WHEN [DATA_TYPE]='COP' AND [DATA_FIELD]='COP03' THEN TP_SCC.[dbo].[PHRASE_NAME]('open_close',[MAX_VALUE],DEFAULT) 
	    WHEN [DATA_TYPE]='COP' AND [DATA_FIELD]='COP04' THEN TP_SCC.[dbo].[PHRASE_NAME]('switch_status',[MAX_VALUE],DEFAULT) 
	    WHEN [DATA_TYPE]='CP' AND [DATA_FIELD]='CP01' THEN TP_SCC.[dbo].[PHRASE_NAME]('running',[MAX_VALUE],DEFAULT) 
	    WHEN [DATA_TYPE]='CP' AND [DATA_FIELD]='CP02' THEN TP_SCC.[dbo].[PHRASE_NAME]('function_fail',[MAX_VALUE],DEFAULT) 
	    WHEN [DATA_TYPE]='CP' AND [DATA_FIELD]='CP03' THEN TP_SCC.[dbo].[PHRASE_NAME]('switch_status',[MAX_VALUE],DEFAULT) 
	    WHEN [DATA_TYPE]='CP' AND [DATA_FIELD]='CP04' THEN TP_SCC.[dbo].[PHRASE_NAME]('open_close',[MAX_VALUE],DEFAULT) 
	    WHEN [DATA_TYPE]='ZP1' AND [DATA_FIELD]='ZP107' THEN TP_SCC.[dbo].[PHRASE_NAME]('running',[MAX_VALUE],DEFAULT) 
	    WHEN [DATA_TYPE]='ZP1' AND [DATA_FIELD]='ZP108' THEN TP_SCC.[dbo].[PHRASE_NAME]('function_fail',[MAX_VALUE],DEFAULT) 
	    WHEN [DATA_TYPE]='ZP1' AND [DATA_FIELD]='ZP109' THEN TP_SCC.[dbo].[PHRASE_NAME]('switch_status',[MAX_VALUE],DEFAULT) 
	    WHEN [DATA_TYPE]='ZP1' AND [DATA_FIELD]='ZP110' THEN TP_SCC.[dbo].[PHRASE_NAME]('open_close',[MAX_VALUE],DEFAULT) 
	    WHEN [DATA_TYPE]='CT' AND [DATA_FIELD]='CT01' THEN TP_SCC.[dbo].[PHRASE_NAME]('running',[MAX_VALUE],DEFAULT) 
	    WHEN [DATA_TYPE]='CT' AND [DATA_FIELD]='CT02' THEN TP_SCC.[dbo].[PHRASE_NAME]('function_fail',[MAX_VALUE],DEFAULT) 
	    WHEN [DATA_TYPE]='CT' AND [DATA_FIELD]='CT03' THEN TP_SCC.[dbo].[PHRASE_NAME]('switch_status',[MAX_VALUE],DEFAULT) 
		WHEN [DATA_TYPE]='WSDS_PVOI' AND [DATA_FIELD]='WSDS_PVOI_STATUS' THEN TP_SCC.[dbo].[PHRASE_NAME]('on_off',[MAX_VALUE],DEFAULT) 
		WHEN [DATA_TYPE]='WSDS_PWLS' AND [DATA_FIELD]='WSDS_PWLS_STATUS' THEN TP_SCC.[dbo].[PHRASE_NAME]('up_down',[MAX_VALUE],DEFAULT) 
		WHEN [DATA_TYPE]='RRS_PVOI' AND [DATA_FIELD]>='RRS01_PVOI01' AND [DATA_FIELD]<='RRS07_PVOI01' THEN TP_SCC.[dbo].[PHRASE_NAME]('on_off',[MAX_VALUE],DEFAULT) 
		WHEN [DATA_TYPE]='RRS_PWLS' AND [DATA_FIELD]>='RRS01_PWLS01' AND [DATA_FIELD]<='RRS13_PWLS01' THEN TP_SCC.[dbo].[PHRASE_NAME]('up_down',[MAX_VALUE],DEFAULT) 
        WHEN [DATA_TYPE]='MSPCSTATS' AND [DATA_FIELD]>='SEF01' AND [DATA_FIELD]<='SEF08' THEN TP_SCC.[dbo].[PHRASE_NAME]('on_off',[MAX_VALUE],DEFAULT) 
        WHEN [DATA_TYPE]='MSPCALARS' AND [DATA_FIELD]>='SEF09' AND [DATA_FIELD]<='SEF15' THEN TP_SCC.[dbo].[PHRASE_NAME]('alert_onoff',[MAX_VALUE],DEFAULT) 
    ELSE
	    CONVERT(VARCHAR(8000),[MAX_VALUE])
    END AS MAX_VALUE
    ,CASE 
	    WHEN [DATA_TYPE]='AHU' AND [DATA_FIELD]='AHU01' THEN TP_SCC.[dbo].[PHRASE_NAME]('function_fail_AHU',[MIN_VALUE],[LOCATION]) 
	    WHEN [DATA_TYPE]='AHU' AND [DATA_FIELD]='AHU02' THEN TP_SCC.[dbo].[PHRASE_NAME]('switch_status',[MIN_VALUE],DEFAULT) 
	    WHEN [DATA_TYPE]='Chiller' AND [DATA_FIELD]='Chiller07' THEN TP_SCC.[dbo].[PHRASE_NAME]('running',[MIN_VALUE],DEFAULT) 
	    WHEN [DATA_TYPE]='Chiller' AND [DATA_FIELD]='Chiller08' THEN TP_SCC.[dbo].[PHRASE_NAME]('function_fail',[MIN_VALUE],DEFAULT) 
	    WHEN [DATA_TYPE]='Chiller' AND [DATA_FIELD]='Chiller09' THEN TP_SCC.[dbo].[PHRASE_NAME]('switch_status',[MIN_VALUE],DEFAULT) 
	    WHEN [DATA_TYPE]='COP' AND [DATA_FIELD]='COP01' THEN TP_SCC.[dbo].[PHRASE_NAME]('running',[MIN_VALUE],DEFAULT) 
	    WHEN [DATA_TYPE]='COP' AND [DATA_FIELD]='COP02' THEN TP_SCC.[dbo].[PHRASE_NAME]('function_fail',[MIN_VALUE],DEFAULT) 
	    WHEN [DATA_TYPE]='COP' AND [DATA_FIELD]='COP03' THEN TP_SCC.[dbo].[PHRASE_NAME]('open_close',[MIN_VALUE],DEFAULT) 
	    WHEN [DATA_TYPE]='COP' AND [DATA_FIELD]='COP04' THEN TP_SCC.[dbo].[PHRASE_NAME]('switch_status',[MIN_VALUE],DEFAULT) 
	    WHEN [DATA_TYPE]='CP' AND [DATA_FIELD]='CP01' THEN TP_SCC.[dbo].[PHRASE_NAME]('running',[MIN_VALUE],DEFAULT) 
	    WHEN [DATA_TYPE]='CP' AND [DATA_FIELD]='CP02' THEN TP_SCC.[dbo].[PHRASE_NAME]('function_fail',[MIN_VALUE],DEFAULT) 
	    WHEN [DATA_TYPE]='CP' AND [DATA_FIELD]='CP03' THEN TP_SCC.[dbo].[PHRASE_NAME]('switch_status',[MIN_VALUE],DEFAULT) 
	    WHEN [DATA_TYPE]='CP' AND [DATA_FIELD]='CP04' THEN TP_SCC.[dbo].[PHRASE_NAME]('open_close',[MIN_VALUE],DEFAULT) 
	    WHEN [DATA_TYPE]='ZP1' AND [DATA_FIELD]='ZP107' THEN TP_SCC.[dbo].[PHRASE_NAME]('running',[MIN_VALUE],DEFAULT) 
	    WHEN [DATA_TYPE]='ZP1' AND [DATA_FIELD]='ZP108' THEN TP_SCC.[dbo].[PHRASE_NAME]('function_fail',[MIN_VALUE],DEFAULT) 
	    WHEN [DATA_TYPE]='ZP1' AND [DATA_FIELD]='ZP109' THEN TP_SCC.[dbo].[PHRASE_NAME]('switch_status',[MIN_VALUE],DEFAULT) 
	    WHEN [DATA_TYPE]='ZP1' AND [DATA_FIELD]='ZP110' THEN TP_SCC.[dbo].[PHRASE_NAME]('open_close',[MIN_VALUE],DEFAULT) 
	    WHEN [DATA_TYPE]='CT' AND [DATA_FIELD]='CT01' THEN TP_SCC.[dbo].[PHRASE_NAME]('running',[MIN_VALUE],DEFAULT) 
	    WHEN [DATA_TYPE]='CT' AND [DATA_FIELD]='CT02' THEN TP_SCC.[dbo].[PHRASE_NAME]('function_fail',[MIN_VALUE],DEFAULT) 
	    WHEN [DATA_TYPE]='CT' AND [DATA_FIELD]='CT03' THEN TP_SCC.[dbo].[PHRASE_NAME]('switch_status',[MIN_VALUE],DEFAULT) 
		WHEN [DATA_TYPE]='WSDS_PVOI' AND [DATA_FIELD]='WSDS_PVOI_STATUS' THEN TP_SCC.[dbo].[PHRASE_NAME]('on_off',[MIN_VALUE],DEFAULT) 
		WHEN [DATA_TYPE]='WSDS_PWLS' AND [DATA_FIELD]='WSDS_PWLS_STATUS' THEN TP_SCC.[dbo].[PHRASE_NAME]('up_down',[MIN_VALUE],DEFAULT) 
		WHEN [DATA_TYPE]='RRS_PVOI' AND [DATA_FIELD]>='RRS01_PVOI01' AND [DATA_FIELD]<='RRS07_PVOI01' THEN TP_SCC.[dbo].[PHRASE_NAME]('on_off',[MIN_VALUE],DEFAULT) 
		WHEN [DATA_TYPE]='RRS_PWLS' AND [DATA_FIELD]>='RRS01_PWLS01' AND [DATA_FIELD]<='RRS13_PWLS01' THEN TP_SCC.[dbo].[PHRASE_NAME]('up_down',[MIN_VALUE],DEFAULT) 
        WHEN [DATA_TYPE]='MSPCSTATS' AND [DATA_FIELD]>='SEF01' AND [DATA_FIELD]<='SEF08' THEN TP_SCC.[dbo].[PHRASE_NAME]('on_off',[MIN_VALUE],DEFAULT) 
        WHEN [DATA_TYPE]='MSPCALARS' AND [DATA_FIELD]>='SEF09' AND [DATA_FIELD]<='SEF15' THEN TP_SCC.[dbo].[PHRASE_NAME]('alert_onoff',[MIN_VALUE],DEFAULT) 
    ELSE
	    CONVERT(VARCHAR(8000),[MIN_VALUE])
    END AS MIN_VALUE
    ,CHECK_INTERVAL
FROM ALERT_CONFIG
    {0}
";
                //Db.AddInParameter(cmd, "TOP", DbType.Int32, 1000);

                string where = "";
                Db.AddInParameter(cmd, "TOP", DbType.Int32, 1000);

                if (!string.IsNullOrEmpty(Req.ALERT_CONFIG.DATA_TYPE))
                {
                    where += " AND DATA_TYPE=@DATA_TYPE";
                    Db.AddInParameter(cmd, "DATA_TYPE", DbType.String, Req.ALERT_CONFIG.DATA_TYPE);
                }
                if (!string.IsNullOrEmpty(Req.ALERT_CONFIG.LOCATION))
                {
                    where += " AND LOCATION=@LOCATION";
                    Db.AddInParameter(cmd, "LOCATION", DbType.String, Req.ALERT_CONFIG.LOCATION);
                }
                if (!string.IsNullOrEmpty(Req.ALERT_CONFIG.DEVICE_ID))
                {
                    where += " AND DEVICE_ID=@DEVICE_ID";
                    Db.AddInParameter(cmd, "DEVICE_ID", DbType.String, Req.ALERT_CONFIG.DEVICE_ID);
                }
                if (!string.IsNullOrEmpty(Req.ALERT_CONFIG.DATA_FIELD))
                {
                    where += " AND DATA_FIELD=@DATA_FIELD";
                    Db.AddInParameter(cmd, "DATA_FIELD", DbType.String, Req.ALERT_CONFIG.DATA_FIELD);
                }
                if (!string.IsNullOrEmpty(Req.ALERT_CONFIG.MODE))
                {
                    where += " AND MODE=@MODE";
                    Db.AddInParameter(cmd, "MODE", DbType.String, Req.ALERT_CONFIG.MODE);
                }
                if (where.Length > 0)
                {
                    where = " WHERE" + where.Substring(4);
                }

                sql = String.Format(sql, where);
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sql;
                using (DataTable dt = Db.ExecuteDataSet(cmd).Tables[0])
                {
                    res.Pagination.RowCount = dt.Rows.Count;
                    res.Pagination.PageCount = Convert.ToInt32(Math.Ceiling(1.0 * res.Pagination.RowCount / Req.PageSize));
                    res.Pagination.PageNumber = Req.PageNumber < 1 ? 1 : Req.PageNumber;
                    res.Pagination.PageNumber = Req.PageNumber > res.Pagination.PageCount ? res.Pagination.PageCount : res.Pagination.PageNumber;
                    res.Pagination.MinNumber = (res.Pagination.PageNumber - 1) * Req.PageSize + 1;
                    res.Pagination.MaxNumber = res.Pagination.PageNumber * Req.PageSize;
                    res.Pagination.MaxNumber = res.Pagination.MaxNumber > res.Pagination.RowCount ? res.Pagination.RowCount : res.Pagination.MaxNumber;

                    if (dt.Rows.Count > 0)
                    {
                        for (int i = res.Pagination.MinNumber - 1; i < res.Pagination.MaxNumber; i++)
                        {
                            var row = new ALERT_CONFIG_RETRIEVE
                            {
                                SID = (int)dt.Rows[i]["SID"],
                                MODE = dt.Rows[i]["MODE"] as string,
                                DATA_TYPE = dt.Rows[i]["DATA_TYPE"] as string,
                                LOCATION = dt.Rows[i]["LOCATION"] as string,
                                DEVICE_ID = dt.Rows[i]["DEVICE_ID"] as string,
                                DATA_FIELD = dt.Rows[i]["DATA_FIELD"] as string,
                                MAX_VALUE = dt.Rows[i]["MAX_VALUE"] as string,
                                MIN_VALUE = dt.Rows[i]["MIN_VALUE"] as string,
                                //CHECK_INTERVAL = dt.Rows[i]["CHECK_INTERVAL"] as int? ?? null,
                                //ALERT_INTERVAL = dt.Rows[i]["ALERT_INTERVAL"] as int? ?? null,
                                //SUN = dt.Rows[i]["SUN"] as bool? ?? null,
                                //SUN_STIME = dt.Rows[i]["SUN_STIME"] as TimeSpan? ?? null,
                                //SUN_ETIME = dt.Rows[i]["SUN_ETIME"] as TimeSpan? ?? null,
                                //MON = dt.Rows[i]["MON"] as bool? ?? null,
                                //MON_STIME = dt.Rows[i]["MON_STIME"] as TimeSpan? ?? null,
                                //MON_ETIME = dt.Rows[i]["MON_ETIME"] as TimeSpan? ?? null,
                                //TUE = dt.Rows[i]["TUE"] as bool? ?? null,
                                //TUE_STIME = dt.Rows[i]["TUE_STIME"] as TimeSpan? ?? null,
                                //TUE_ETIME = dt.Rows[i]["TUE_ETIME"] as TimeSpan? ?? null,
                                //WED = dt.Rows[i]["WED"] as bool? ?? null,
                                //WED_STIME = dt.Rows[i]["WED_STIME"] as TimeSpan? ?? null,
                                //WED_ETIME = dt.Rows[i]["WED_ETIME"] as TimeSpan? ?? null,
                                //THU = dt.Rows[i]["THU"] as bool? ?? null,
                                //THU_STIME = dt.Rows[i]["THU_STIME"] as TimeSpan? ?? null,
                                //THU_ETIME = dt.Rows[i]["THU_ETIME"] as TimeSpan? ?? null,
                                //FRI = dt.Rows[i]["FRI"] as bool? ?? null,
                                //FRI_STIME = dt.Rows[i]["FRI_STIME"] as TimeSpan? ?? null,
                                //FRI_ETIME = dt.Rows[i]["FRI_ETIME"] as TimeSpan? ?? null,
                                //STA = dt.Rows[i]["STA"] as bool? ?? null,
                                //STA_STIME = dt.Rows[i]["STA_STIME"] as TimeSpan? ?? null,
                                //STA_ETIME = dt.Rows[i]["STA_ETIME"] as TimeSpan? ?? null,
                                //CHECK_DATE = dt.Rows[i]["CHECK_DATE"] as DateTime? ?? null,
                                //ALERT_DATE = dt.Rows[i]["ALERT_DATE"] as DateTime? ?? null,
                                //MAIL_TO = dt.Rows[i]["MAIL_TO"] as string,
                                //CHECK_HR_CALENDAR = dt.Rows[i]["CHECK_HR_CALENDAR"] as bool? ?? null,
                                //CDATE = dt.Rows[i]["CDATE"] as DateTime? ?? null,
                                //CUSER = dt.Rows[i]["CUSER"] as string,
                                //MDATE = dt.Rows[i]["MDATE"] as DateTime? ?? null,
                                //MUSER = dt.Rows[i]["MUSER"] as string,
                            };
                            res.ALERT_CONFIG.Add(row);
                        }
                    }
                }
            }
            res.Pagination.EndTime = DateTime.Now;

            return res;
        }

        public ALERT_CONFIGModifyRes ModificationQuery(ALERT_CONFIGModifyReq Req)
        {
            ALERT_CONFIGModifyRes res = new ALERT_CONFIGModifyRes();

            using (DbCommand cmd = Db.CreateConnection().CreateCommand())
            {
                string sql = @"
SELECT SID,MODE,DATA_TYPE,LOCATION,DEVICE_ID,DATA_FIELD,MAX_VALUE,MIN_VALUE,CHECK_INTERVAL,ALERT_INTERVAL
    ,SUN,SUN_STIME,SUN_ETIME,MON,MON_STIME,MON_ETIME,TUE,TUE_STIME,TUE_ETIME,WED,WED_STIME,WED_ETIME,THU,THU_STIME
    ,THU_ETIME,FRI,FRI_STIME,FRI_ETIME,STA,STA_STIME,STA_ETIME,CHECK_DATE,ALERT_DATE,MAIL_TO,CHECK_HR_CALENDAR
    ,CDATE,CUSER,MDATE,MUSER
    FROM ALERT_CONFIG
    WHERE SID=@SID
        ";
                Db.AddInParameter(cmd, "SID", DbType.Int32, Req.ALERT_CONFIG.SID);

                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sql;
                using (IDataReader reader = Db.ExecuteReader(cmd))
                {
                    if (reader.Read())
                    {
                        ALERT_CONFIG ALERT_CONFIG = new ALERT_CONFIG
                        {
                            SID = (int)reader["SID"],
                            MODE = reader["MODE"] as string,
                            DATA_TYPE = reader["DATA_TYPE"] as string,
                            LOCATION = reader["LOCATION"] as string,
                            DEVICE_ID = reader["DEVICE_ID"] as string,
                            DATA_FIELD = reader["DATA_FIELD"] as string,
                            MAX_VALUE = reader["MAX_VALUE"] as Single? ?? null,
                            MIN_VALUE = reader["MIN_VALUE"] as Single? ?? null,
                            CHECK_INTERVAL = reader["CHECK_INTERVAL"] as int? ?? null,
                            ALERT_INTERVAL = reader["ALERT_INTERVAL"] as int? ?? null,
                            SUN = (Boolean)reader["SUN"],
                            SUN_STIME = reader["SUN_STIME"] as TimeSpan? ?? null,
                            SUN_ETIME = reader["SUN_ETIME"] as TimeSpan? ?? null,
                            MON = (Boolean)reader["MON"],
                            MON_STIME = reader["MON_STIME"] as TimeSpan? ?? null,
                            MON_ETIME = reader["MON_ETIME"] as TimeSpan? ?? null,
                            TUE = (Boolean)reader["TUE"],
                            TUE_STIME = reader["TUE_STIME"] as TimeSpan? ?? null,
                            TUE_ETIME = reader["TUE_ETIME"] as TimeSpan? ?? null,
                            WED = (Boolean)reader["WED"],
                            WED_STIME = reader["WED_STIME"] as TimeSpan? ?? null,
                            WED_ETIME = reader["WED_ETIME"] as TimeSpan? ?? null,
                            THU = (Boolean)reader["THU"],
                            THU_STIME = reader["THU_STIME"] as TimeSpan? ?? null,
                            THU_ETIME = reader["THU_ETIME"] as TimeSpan? ?? null,
                            FRI = (Boolean)reader["FRI"],
                            FRI_STIME = reader["FRI_STIME"] as TimeSpan? ?? null,
                            FRI_ETIME = reader["FRI_ETIME"] as TimeSpan? ?? null,
                            STA = (Boolean)reader["STA"],
                            STA_STIME = reader["STA_STIME"] as TimeSpan? ?? null,
                            STA_ETIME = reader["STA_ETIME"] as TimeSpan? ?? null,
                            CHECK_DATE = reader["CHECK_DATE"] as DateTime? ?? null,
                            ALERT_DATE = reader["ALERT_DATE"] as DateTime? ?? null,
                            MAIL_TO = reader["MAIL_TO"] as string,
                            CHECK_HR_CALENDAR = (Boolean)reader["CHECK_HR_CALENDAR"],
                            CDATE = reader["CDATE"] as DateTime? ?? null,
                            CUSER = reader["CUSER"] as string,
                            MDATE = reader["MDATE"] as DateTime? ?? null,
                            MUSER = reader["MUSER"] as string,
                        };
                        res.ALERT_CONFIG = ALERT_CONFIG;
                    }
                }
            }

            return res;
        }

        public bool DataCreate(ALERT_CONFIGModifyReq req)
        {
            int effect = 0;

            using (DbCommand cmd = Db.CreateConnection().CreateCommand())
            {
                string sql = @"
        SET @SID = NEXT VALUE FOR [ALERT_CONFIG_SEQ]
        INSERT ALERT_CONFIG (
            SID,MODE,DATA_TYPE,LOCATION,DEVICE_ID,DATA_FIELD,MAX_VALUE,MIN_VALUE,CHECK_INTERVAL,ALERT_INTERVAL
            ,SUN,SUN_STIME,SUN_ETIME,MON,MON_STIME,MON_ETIME,TUE,TUE_STIME,TUE_ETIME,WED,WED_STIME,WED_ETIME,THU,THU_STIME
            ,THU_ETIME,FRI,FRI_STIME,FRI_ETIME,STA,STA_STIME,STA_ETIME,CHECK_DATE,ALERT_DATE,MAIL_TO,CHECK_HR_CALENDAR
            ,CDATE,CUSER,MDATE,MUSER)
        VALUES (
            @SID,@MODE,@DATA_TYPE,@LOCATION,@DEVICE_ID,@DATA_FIELD,@MAX_VALUE,@MIN_VALUE,@CHECK_INTERVAL,@ALERT_INTERVAL
            ,@SUN,@SUN_STIME,@SUN_ETIME,@MON,@MON_STIME,@MON_ETIME,@TUE,@TUE_STIME,@TUE_ETIME,@WED,@WED_STIME,@WED_ETIME,@THU,@THU_STIME
            ,@THU_ETIME,@FRI,@FRI_STIME,@FRI_ETIME,@STA,@STA_STIME,@STA_ETIME,GETDATE(),GETDATE(),@MAIL_TO,@CHECK_HR_CALENDAR
            ,GETDATE(),@CUSER,GETDATE(),@MUSER);
                ";

//                sql = @"
//        SET @SID = NEXT VALUE FOR [ALERT_CONFIG_SEQ]
//        INSERT ALERT_CONFIG (
//            SID,MODE,DATA_TYPE,LOCATION,DEVICE_ID,DATA_FIELD,MAX_VALUE,MIN_VALUE,CHECK_INTERVAL,ALERT_INTERVAL
//            ,SUN,SUN_STIME,SUN_ETIME
//)
//        VALUES (
//            @SID,@MODE,@DATA_TYPE,@LOCATION,@DEVICE_ID,@DATA_FIELD,@MAX_VALUE,@MIN_VALUE,@CHECK_INTERVAL,@ALERT_INTERVAL
//            ,@SUN,@SUN_STIME,@SUN_ETIME
//);
//                ";

                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sql;

                Db.AddInParameter(cmd, "MODE", DbType.String, req.ALERT_CONFIG.MODE);
                Db.AddInParameter(cmd, "DATA_TYPE", DbType.String, req.ALERT_CONFIG.DATA_TYPE);
                Db.AddInParameter(cmd, "LOCATION", DbType.String, req.ALERT_CONFIG.LOCATION);
                Db.AddInParameter(cmd, "DEVICE_ID", DbType.String, req.ALERT_CONFIG.DEVICE_ID);
                Db.AddInParameter(cmd, "DATA_FIELD", DbType.String, req.ALERT_CONFIG.DATA_FIELD);
                Db.AddInParameter(cmd, "MAX_VALUE", DbType.Single, req.ALERT_CONFIG.MAX_VALUE);
                Db.AddInParameter(cmd, "MIN_VALUE", DbType.Single, req.ALERT_CONFIG.MIN_VALUE);
                Db.AddInParameter(cmd, "CHECK_INTERVAL", DbType.Int32, req.ALERT_CONFIG.CHECK_INTERVAL);
                Db.AddInParameter(cmd, "ALERT_INTERVAL", DbType.Int32, req.ALERT_CONFIG.ALERT_INTERVAL == null ? (object)DBNull.Value : req.ALERT_CONFIG.ALERT_INTERVAL);
                Db.AddInParameter(cmd, "SUN", DbType.Boolean, req.ALERT_CONFIG.SUN);
                Db.AddInParameter(cmd, "SUN_STIME", DbType.Time, req.ALERT_CONFIG.SUN_STIME == null ? (object)DBNull.Value : req.ALERT_CONFIG.SUN_STIME.ToString());
                Db.AddInParameter(cmd, "SUN_ETIME", DbType.Time, req.ALERT_CONFIG.SUN_ETIME == null ? (object)DBNull.Value : req.ALERT_CONFIG.SUN_ETIME.ToString());
                Db.AddInParameter(cmd, "MON", DbType.Boolean, req.ALERT_CONFIG.MON);
                Db.AddInParameter(cmd, "MON_STIME", DbType.Time, req.ALERT_CONFIG.MON_STIME == null ? (object)DBNull.Value : req.ALERT_CONFIG.MON_STIME.ToString());
                Db.AddInParameter(cmd, "MON_ETIME", DbType.Time, req.ALERT_CONFIG.MON_ETIME == null ? (object)DBNull.Value : req.ALERT_CONFIG.MON_ETIME.ToString());
                Db.AddInParameter(cmd, "TUE", DbType.Boolean, req.ALERT_CONFIG.TUE);
                Db.AddInParameter(cmd, "TUE_STIME", DbType.Time, req.ALERT_CONFIG.TUE_STIME == null ? (object)DBNull.Value : req.ALERT_CONFIG.TUE_STIME.ToString());
                Db.AddInParameter(cmd, "TUE_ETIME", DbType.Time, req.ALERT_CONFIG.TUE_ETIME == null ? (object)DBNull.Value : req.ALERT_CONFIG.TUE_ETIME.ToString());
                Db.AddInParameter(cmd, "WED", DbType.Boolean, req.ALERT_CONFIG.WED);
                Db.AddInParameter(cmd, "WED_STIME", DbType.Time, req.ALERT_CONFIG.WED_STIME == null ? (object)DBNull.Value : req.ALERT_CONFIG.WED_STIME.ToString());
                Db.AddInParameter(cmd, "WED_ETIME", DbType.Time, req.ALERT_CONFIG.WED_ETIME == null ? (object)DBNull.Value : req.ALERT_CONFIG.WED_ETIME.ToString());
                Db.AddInParameter(cmd, "THU", DbType.Boolean, req.ALERT_CONFIG.THU);
                Db.AddInParameter(cmd, "THU_STIME", DbType.Time, req.ALERT_CONFIG.THU_STIME == null ? (object)DBNull.Value : req.ALERT_CONFIG.THU_STIME.ToString());
                Db.AddInParameter(cmd, "THU_ETIME", DbType.Time, req.ALERT_CONFIG.THU_ETIME == null ? (object)DBNull.Value : req.ALERT_CONFIG.THU_ETIME.ToString());
                Db.AddInParameter(cmd, "FRI", DbType.Boolean, req.ALERT_CONFIG.FRI);
                Db.AddInParameter(cmd, "FRI_STIME", DbType.Time, req.ALERT_CONFIG.FRI_STIME == null ? (object)DBNull.Value : req.ALERT_CONFIG.FRI_STIME.ToString());
                Db.AddInParameter(cmd, "FRI_ETIME", DbType.Time, req.ALERT_CONFIG.FRI_ETIME == null ? (object)DBNull.Value : req.ALERT_CONFIG.FRI_ETIME.ToString());
                Db.AddInParameter(cmd, "STA", DbType.Boolean, req.ALERT_CONFIG.STA);
                Db.AddInParameter(cmd, "STA_STIME", DbType.Time, req.ALERT_CONFIG.STA_STIME == null ? (object)DBNull.Value : req.ALERT_CONFIG.STA_STIME.ToString());
                Db.AddInParameter(cmd, "STA_ETIME", DbType.Time, req.ALERT_CONFIG.STA_ETIME == null ? (object)DBNull.Value : req.ALERT_CONFIG.STA_ETIME.ToString());
                //Db.AddInParameter(cmd, "CHECK_DATE", DbType.Date, req.ALERT_CONFIG.CHECK_DATE);
                //Db.AddInParameter(cmd, "ALERT_DATE", DbType.Date, req.ALERT_CONFIG.ALERT_DATE);
                Db.AddInParameter(cmd, "MAIL_TO", DbType.String, req.ALERT_CONFIG.MAIL_TO);
                Db.AddInParameter(cmd, "CHECK_HR_CALENDAR", DbType.Boolean, req.ALERT_CONFIG.CHECK_HR_CALENDAR);
                //Db.AddInParameter(cmd, "CDATE", DbType.Date, req.ALERT_CONFIG.CDATE);
                Db.AddInParameter(cmd, "CUSER", DbType.String, req.ALERT_CONFIG.CUSER);
                //Db.AddInParameter(cmd, "MDATE", DbType.Date, req.ALERT_CONFIG.MDATE);
                Db.AddInParameter(cmd, "MUSER", DbType.String, req.ALERT_CONFIG.MUSER);
                Db.AddOutParameter(cmd, "SID", DbType.Int32, 1);

                effect = Db.ExecuteNonQuery(cmd);
                req.ALERT_CONFIG.SID = Db.GetParameterValue(cmd, "SID") as Int32? ?? null;
                return true;
            }
        }

        public bool DataUpdate(ALERT_CONFIGModifyReq req)
        {
            int effect = 0;

            using (DbCommand cmd = Db.CreateConnection().CreateCommand())
            {
                string sql = @"
UPDATE ALERT_CONFIG
    SET MODE=@MODE
    ,DATA_TYPE=@DATA_TYPE
    ,LOCATION=@LOCATION
    ,DEVICE_ID=@DEVICE_ID
    ,DATA_FIELD=@DATA_FIELD
    ,MAX_VALUE=@MAX_VALUE
    ,MIN_VALUE=@MIN_VALUE
    ,CHECK_INTERVAL=@CHECK_INTERVAL
    ,ALERT_INTERVAL=@ALERT_INTERVAL
    ,SUN=@SUN
    ,SUN_STIME=@SUN_STIME
    ,SUN_ETIME=@SUN_ETIME
    ,MON=@MON
    ,MON_STIME=@MON_STIME
    ,MON_ETIME=@MON_ETIME
    ,TUE=@TUE
    ,TUE_STIME=@TUE_STIME
    ,TUE_ETIME=@TUE_ETIME
    ,WED=@WED
    ,WED_STIME=@WED_STIME
    ,WED_ETIME=@WED_ETIME
    ,THU=@THU
    ,THU_STIME=@THU_STIME
    ,THU_ETIME=@THU_ETIME
    ,FRI=@FRI
    ,FRI_STIME=@FRI_STIME
    ,FRI_ETIME=@FRI_ETIME
    ,STA=@STA
    ,STA_STIME=@STA_STIME
    ,STA_ETIME=@STA_ETIME
    ,CHECK_DATE=GETDATE()
    ,ALERT_DATE=GETDATE()
    ,MAIL_TO=@MAIL_TO
    ,CHECK_HR_CALENDAR=@CHECK_HR_CALENDAR
    ,MDATE=GETDATE()
    ,MUSER=@MUSER
WHERE SID=@SID;
        ";
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sql;

                Db.AddInParameter(cmd, "SID", DbType.String, req.ALERT_CONFIG.SID);
                Db.AddInParameter(cmd, "MODE", DbType.String, req.ALERT_CONFIG.MODE);
                Db.AddInParameter(cmd, "DATA_TYPE", DbType.String, req.ALERT_CONFIG.DATA_TYPE);
                Db.AddInParameter(cmd, "LOCATION", DbType.String, req.ALERT_CONFIG.LOCATION);
                Db.AddInParameter(cmd, "DEVICE_ID", DbType.String, req.ALERT_CONFIG.DEVICE_ID);
                Db.AddInParameter(cmd, "DATA_FIELD", DbType.String, req.ALERT_CONFIG.DATA_FIELD);
                Db.AddInParameter(cmd, "MAX_VALUE", DbType.Single, req.ALERT_CONFIG.MAX_VALUE);
                Db.AddInParameter(cmd, "MIN_VALUE", DbType.Single, req.ALERT_CONFIG.MIN_VALUE);
                Db.AddInParameter(cmd, "CHECK_INTERVAL", DbType.Int32, req.ALERT_CONFIG.CHECK_INTERVAL);
                Db.AddInParameter(cmd, "ALERT_INTERVAL", DbType.Int32, req.ALERT_CONFIG.ALERT_INTERVAL == null ? (object)DBNull.Value : req.ALERT_CONFIG.ALERT_INTERVAL);
                Db.AddInParameter(cmd, "SUN", DbType.Boolean, req.ALERT_CONFIG.SUN);
                Db.AddInParameter(cmd, "SUN_STIME", DbType.Time, req.ALERT_CONFIG.SUN_STIME == null ? (object)DBNull.Value : req.ALERT_CONFIG.SUN_STIME.ToString());
                Db.AddInParameter(cmd, "SUN_ETIME", DbType.Time, req.ALERT_CONFIG.SUN_ETIME == null ? (object)DBNull.Value : req.ALERT_CONFIG.SUN_ETIME.ToString());
                Db.AddInParameter(cmd, "MON", DbType.Boolean, req.ALERT_CONFIG.MON);
                Db.AddInParameter(cmd, "MON_STIME", DbType.Time, req.ALERT_CONFIG.MON_STIME == null ? (object)DBNull.Value : req.ALERT_CONFIG.MON_STIME.ToString());
                Db.AddInParameter(cmd, "MON_ETIME", DbType.Time, req.ALERT_CONFIG.MON_ETIME == null ? (object)DBNull.Value : req.ALERT_CONFIG.MON_ETIME.ToString());
                Db.AddInParameter(cmd, "TUE", DbType.Boolean, req.ALERT_CONFIG.TUE);
                Db.AddInParameter(cmd, "TUE_STIME", DbType.Time, req.ALERT_CONFIG.TUE_STIME == null ? (object)DBNull.Value : req.ALERT_CONFIG.TUE_STIME.ToString());
                Db.AddInParameter(cmd, "TUE_ETIME", DbType.Time, req.ALERT_CONFIG.TUE_ETIME == null ? (object)DBNull.Value : req.ALERT_CONFIG.TUE_ETIME.ToString());
                Db.AddInParameter(cmd, "WED", DbType.Boolean, req.ALERT_CONFIG.WED);
                Db.AddInParameter(cmd, "WED_STIME", DbType.Time, req.ALERT_CONFIG.WED_STIME == null ? (object)DBNull.Value : req.ALERT_CONFIG.WED_STIME.ToString());
                Db.AddInParameter(cmd, "WED_ETIME", DbType.Time, req.ALERT_CONFIG.WED_ETIME == null ? (object)DBNull.Value : req.ALERT_CONFIG.WED_ETIME.ToString());
                Db.AddInParameter(cmd, "THU", DbType.Boolean, req.ALERT_CONFIG.THU);
                Db.AddInParameter(cmd, "THU_STIME", DbType.Time, req.ALERT_CONFIG.THU_STIME == null ? (object)DBNull.Value : req.ALERT_CONFIG.THU_STIME.ToString());
                Db.AddInParameter(cmd, "THU_ETIME", DbType.Time, req.ALERT_CONFIG.THU_ETIME == null ? (object)DBNull.Value : req.ALERT_CONFIG.THU_ETIME.ToString());
                Db.AddInParameter(cmd, "FRI", DbType.Boolean, req.ALERT_CONFIG.FRI);
                Db.AddInParameter(cmd, "FRI_STIME", DbType.Time, req.ALERT_CONFIG.FRI_STIME == null ? (object)DBNull.Value : req.ALERT_CONFIG.FRI_STIME.ToString());
                Db.AddInParameter(cmd, "FRI_ETIME", DbType.Time, req.ALERT_CONFIG.FRI_ETIME == null ? (object)DBNull.Value : req.ALERT_CONFIG.FRI_ETIME.ToString());
                Db.AddInParameter(cmd, "STA", DbType.Boolean, req.ALERT_CONFIG.STA);
                Db.AddInParameter(cmd, "STA_STIME", DbType.Time, req.ALERT_CONFIG.STA_STIME == null ? (object)DBNull.Value : req.ALERT_CONFIG.STA_STIME.ToString());
                Db.AddInParameter(cmd, "STA_ETIME", DbType.Time, req.ALERT_CONFIG.STA_ETIME == null ? (object)DBNull.Value : req.ALERT_CONFIG.STA_ETIME.ToString());
                //Db.AddInParameter(cmd, "CHECK_DATE", DbType.Date, req.ALERT_CONFIG.CHECK_DATE);
                //Db.AddInParameter(cmd, "ALERT_DATE", DbType.Date, req.ALERT_CONFIG.ALERT_DATE);
                Db.AddInParameter(cmd, "MAIL_TO", DbType.String, req.ALERT_CONFIG.MAIL_TO);
                Db.AddInParameter(cmd, "CHECK_HR_CALENDAR", DbType.Boolean, req.ALERT_CONFIG.CHECK_HR_CALENDAR);
                //Db.AddInParameter(cmd, "CDATE", DbType.Date, req.ALERT_CONFIG.CDATE);
                //Db.AddInParameter(cmd, "CUSER", DbType.String, req.ALERT_CONFIG.CUSER);
                //Db.AddInParameter(cmd, "MDATE", DbType.Date, req.ALERT_CONFIG.MDATE);
                Db.AddInParameter(cmd, "MUSER", DbType.String, req.ALERT_CONFIG.MUSER);

                effect = Db.ExecuteNonQuery(cmd);
            }

            return effect == 1;
        }

        public int DataDelete(ALERT_CONFIGModifyReq req)
        {
            int count = 0;
            using (DbCommand cmd = Db.CreateConnection().CreateCommand())
            {
                string sql = @"
        DELETE FROM ALERT_CONFIG
            WHERE SID=@SID;
                ";
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sql;
                Db.AddInParameter(cmd, "SID", DbType.String, req.ALERT_CONFIG.SID);
                count = Db.ExecuteNonQuery(cmd);
            }
            return count;
        }

        public bool DataDuplicate(ALERT_CONFIGModifyReq Req)
        {
            bool yn = false;
            using (DbCommand cmd = Db.CreateConnection().CreateCommand())
            {
                string sql = @"
        SELECT 1
            FROM ALERT_CONFIG
            WHERE DATA_TYPE=@DATA_TYPE AND LOCATION=@LOCATION AND DEVICE_ID=@DEVICE_ID AND DATA_FIELD=@DATA_FIELD
                ";
                Db.AddInParameter(cmd, "DATA_TYPE", DbType.String, Req.ALERT_CONFIG.DATA_TYPE);
                Db.AddInParameter(cmd, "LOCATION", DbType.String, Req.ALERT_CONFIG.LOCATION);
                Db.AddInParameter(cmd, "DEVICE_ID", DbType.String, Req.ALERT_CONFIG.DEVICE_ID);
                Db.AddInParameter(cmd, "DATA_FIELD", DbType.String, Req.ALERT_CONFIG.DATA_FIELD);

                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sql;
                using (IDataReader reader = Db.ExecuteReader(cmd))
                {
                    if (reader.Read())
                    {
                        yn = true;
                    }
                }
            }
            return yn;
        }
    }
}