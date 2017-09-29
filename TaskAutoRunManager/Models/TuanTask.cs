using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TaskAutoRunManager.Models
{
    public class TuanTask
    {
        private string _id = "";

        /// <summary>
        /// 任务编号
        /// </summary>
        public string Id {
            get { return _id; }
            set { _id = value; }
        }

        private string _name = "";
        /// <summary>
        /// 任务名称
        /// </summary>
        public string Name {
            get { return _name; }
            set { _name = value; } 
        }

        private string _path = "";
        /// <summary>
        /// 任务地址
        /// </summary>
        public string Path
        {
            get { return _path; }
            set { _path = value; }
        }
        /// <summary>
        /// 任务类型 1：每隔一段时间执行 2：每天定点执行
        /// </summary>
        public int RunType { get; set; }

        /// <summary>
        /// 每隔一段时间
        /// </summary>
        public int RunMiniteOf { get; set; }
        private string _runTimeAt = "";
        /// <summary>
        /// 每天定点
        /// </summary>
        public string RunTimeAt
        {
            get { return _runTimeAt; }
            set { _runTimeAt = value; }
        }
        private string _author = "";
        /// <summary>
        /// 作者
        /// </summary>
        public string Author
        {
            get { return _author; }
            set { _author = value; }
        }

        /// <summary>
        /// 最后一次运行时间
        /// </summary>
        public DateTime LastRunTime { get; set; }

        /// <summary>
        /// 启用或停用 1：启用 2：停用
        /// </summary>
        public int IsOpen { get; set; }

     
    }

    public class TuanTaskList
    {
        /// <summary>
        /// 数据列表
        /// </summary>
        public List<TuanTask> tuanTaskList = new List<TuanTask>();
    }
}