using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace TaskAutoRunManager.DataAccess
{
    public class config
    {
        /// <summary>
        /// 读数据库
        /// </summary>
        public static string connstrRead
        {
            get { return getSetting("ConnectionString_Read"); }
        }

        /// <summary>
        /// 写数据库
        /// </summary>
        public static string connstrWrite
        {
            get { return getSetting("ConnectionString_Write"); }
        }

       /// <summary>
       /// 获取配置信息
       /// </summary>
       /// <param name="connstr"></param>
       /// <returns></returns>
        public static string getSetting(string connstr)
        {
            return ConfigurationManager.AppSettings[connstr];
        }
    }
}