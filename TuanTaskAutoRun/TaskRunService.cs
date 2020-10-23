using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.ServiceProcess;
using System.Threading;

namespace TaskAutoRun
{
    partial class TaskRunService : ServiceBase
    {
        private bool state = false;
        private List<Xtask> taskList = new List<Xtask>();
        private string connstr = ConfigurationSettings.AppSettings["taskdb"];

        public TaskRunService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            state = true;
            Thread t = new Thread(RunTaskMethod);
            t.Start();
        }

        protected override void OnStop()
        {
            state = false;
        }

        private void RunTaskMethod()
        {
            while (state)
            {
                taskList.Clear();
                string sql = @"SELECT [id]
                                 ,[name]
                                 ,[path]
                                 ,[runType]
                                 ,[runMiniteOf]
                                 ,[runTimeAt]
                                 ,[Author]
                                 ,[LastRunTime]
                                 ,[isOpen]
                                 FROM [TaskAutoRun] with(nolock) ";
                var dtTemp = DBHelper.CreateTable(connstr, sql);
                foreach (DataRow dr in dtTemp.Rows)
                {
                    taskList.Add(new Xtask(dr));
                }
                foreach (var xtask in taskList)
                {
                    if (xtask.IsOpen && xtask.NextExcuteTime < DateTime.Now)
                    {
                        xtask.Excute(connstr);
                    }
                }
                Thread.Sleep(500);
            }
        }
    }
}