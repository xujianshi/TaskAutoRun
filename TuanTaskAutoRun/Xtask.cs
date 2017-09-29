using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;
using SoufunLab.Framework;
using SoufunLab.Framework.Data;

namespace TaskAutoRun
{
    public class Xtask
    {
        public Xtask(DataRow row)
        {
            TaskId = SlConvert.TryToString(row["id"]);
            Path = SlConvert.TryToString(row["path"]);
            IsOpen = SlConvert.TryToInt16(row["IsOpen"]) == 1;
            LastExcuteTime = SlConvert.TryToDateTime(row["LastRunTime"], DateTime.Now.AddDays(-1));
            
            int runType = SlConvert.TryToInt16(row["runType"]);
            if (runType == 1) //每隔一段时间跑一次
            {
               int  runMiniteOf = SlConvert.TryToInt16(row["RunMiniteOf"]);
                NextExcuteTime = LastExcuteTime.AddMinutes(runMiniteOf);
            }
            else //每天定时跑一次
            {
                //如果今天跑过，则不再跑
                NextExcuteTime = SlConvert.TryToDateTime(DateTime.Now.ToString("yyyy-MM-dd") + "  " + row["RunTimeAt"]);
                if (LastExcuteTime.ToString("yyyy-M-d") == DateTime.Now.ToString("yyyy-M-d"))
                {
                    NextExcuteTime = NextExcuteTime.AddDays(1);
                }
            }
        }
        
        public bool IsOpen { get; set; }
        
        public string Path { get; set; }

        public DateTime NextExcuteTime { get; set; }

        public DateTime LastExcuteTime { get; set; }
        
        public string TaskId { get; set; }
        
        public void Excute(string databaseConnetionString)
        {
            bool runResult = false;
            if (Path.EndsWith(".exe") && File.Exists(Path))
            {
                runResult = RunApp(Path); //打开此文件。
            }
            if ( Path.EndsWith(".bat") && File.Exists(Path))
            {
                runResult = RunBat(Path); //打开此文件。
            }
            else if (Path.StartsWith("http"))
            {
                runResult = RunWeb(Path); //打开此文件。
            }
            if (runResult)
            {
                //更新数据库执行时间
                string sql = "update [TaskAutoRun] set [LastRunTime]=getdate() where id='{0}'";
                sql = string.Format(sql, TaskId);
                SlDatabase.ExecuteNonQuery(databaseConnetionString, sql);
            }
        }
        
        void WriteLog(string message)
        {
            try
            {
                string path = Application.StartupPath+ "/" + DateTime.Now.ToString("yyyyMM") + ".txt";
                var writer = new StreamWriter(path, true, Encoding.UTF8);
                writer.WriteLine("时间：" + DateTime.Now + " \r\n记录：" + message);
                writer.WriteLine("--------------------------------------------------------------------------");
                writer.Flush();
                writer.Close();
                writer.Dispose();
            }
            catch
            {
            }
        }

        bool RunApp(string file)
        {
            try
            {
                string str = file.Substring(file.LastIndexOf(@"\") + 1, (file.LastIndexOf(".exe") - file.LastIndexOf(@"\")) + 3);
                string str2 = file.Substring(0, file.LastIndexOf(@"\") + 1);
                string str3 = file.Substring(file.LastIndexOf(".exe") + 4);
                if (Process.GetProcessesByName(str.Replace(".exe", "")).Length == 0)
                {
                    Process process = new Process();
                    ProcessStartInfo info = new ProcessStartInfo
                    {
                        FileName = str,
                        WorkingDirectory = str2,
                        Arguments = str3
                    };
                    process.StartInfo = info;
                    process.Start();
                    WriteLog(Path);
                }
                else
                {
                    return false;
                }
            }
            catch(Exception ex)
            {
                WriteLog(ex.ToString());
                return false;
            }
            return true;
        }

        bool RunBat(string file)
        {
            try
            {
                string str = file.Substring(file.LastIndexOf(@"\") + 1, (file.LastIndexOf(".bat") - file.LastIndexOf(@"\")) + 3);
                string str2 = file.Substring(0, file.LastIndexOf(@"\") + 1);
                string str3 = file.Substring(file.LastIndexOf(".bat") + 4);
                if (Process.GetProcessesByName(str.Replace(".bat", "")).Length == 0)
                {
                    Process process = new Process();
                    ProcessStartInfo info = new ProcessStartInfo
                    {
                        FileName = str,
                        WorkingDirectory = str2,
                        Arguments = str3
                    };
                    process.StartInfo = info;
                    process.Start();
                    WriteLog(Path);
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                WriteLog(ex.ToString());
                return false;
            }
            return true;
        }
        bool RunWeb(string url)
        {
            string html = NetHelper.GetXml(url);
            if (!string.IsNullOrEmpty(html))
            {
                return true;
            }
            return false;
        }
    }
}
