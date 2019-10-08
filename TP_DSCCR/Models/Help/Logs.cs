using System;
using System.IO;
using System.Threading;

namespace TP_DSCCR.Models.Help
{
    public static class Logs
    {
        //private static Mutex _Lock = new Mutex();
        private static readonly object _syncObject = new object();
        private static string _LogPath = System.AppDomain.CurrentDomain.BaseDirectory + "log\\";

        /// <summary>
        /// 寫入記錄檔
        /// </summary>
        /// <param name="SubFolder">資料目錄</param>
        /// <param name="Message">資料訊息</param>
        public static void Write(string Message)
        {
            string Path = _LogPath;
            String FileName = DateTime.Now.ToString("yyyyMMdd") + ".txt";
            FileWrite(Path + FileName, Message);
        }
        /// <summary>
        /// 寫入記錄檔(指定路徑)
        /// </summary>
        /// <param name="SubFolder">資料目錄</param>
        /// <param name="Message">資料訊息</param>
        public static void Write(string SubFolder, string Message)
        {
            string Path = _LogPath + SubFolder + "\\";
            String FileName = DateTime.Now.ToString("yyyyMMdd") + ".txt";
            FileWrite(Path + FileName, Message);
        }
        /// <summary>
        /// 寫入記錄檔(指定路徑)
        /// </summary>
        /// <param name="SubFolder">資料目錄</param>
        /// <param name="Message">資料訊息</param>
        public static void Write(string SubFolder, string Format, params object[] args)
        {
            string Path = _LogPath + SubFolder + "\\";
            String FileName = DateTime.Now.ToString("yyyyMMdd") + ".txt";
            FileWrite(Path + FileName, string.Format(Format, args));
        }


        /// <summary>
        /// 寫入記錄檔(指定路徑&檔名)
        /// </summary>
        /// <param name="Path">路徑</param>
        /// <param name="FileName">檔名</param>
        /// <param name="Message">資料訊息</param>
        public static void Write(string Path, string FileName, string Message)
        {
            FileWrite(Path + FileName, Message);
        }

        private static void FileWrite(string FullFileName, string Message)
        {
            lock (_syncObject)
            {
                try
                {
                    //_Lock.WaitOne();

                    if (!Directory.Exists(Path.GetDirectoryName(FullFileName)))
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(FullFileName));
                    }

                    using (FileStream fs = new FileStream(FullFileName, FileMode.Append, FileAccess.Write))
                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        sw.WriteLine(DateTime.Now.ToString("HH:mm:ss fff") + " : " + Message);
                        sw.Flush();
                    }
                }
                catch
                {

                }
                finally
                {
                    //_Lock.ReleaseMutex();
                }
            }
        }

    }
}