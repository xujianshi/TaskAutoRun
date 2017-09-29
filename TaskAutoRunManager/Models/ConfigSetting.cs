using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TaskAutoRunManager.Models
{
    public class ConfigSetting
    {
        public string Id { get; set; }
        public string Value { get; set; }
        /// <summary>
        /// 分组
        /// </summary>
        public string Fenzu { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Title { get; set; }
    }

    public class ConfigSettingList
    {
        /// <summary>
        /// 数据列表
        /// </summary>
        public List<Models.ConfigSetting> CsList = new List<Models.ConfigSetting>();
    }
}