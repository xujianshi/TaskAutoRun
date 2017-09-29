using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using FtpLib;

namespace TaskAutoRunManager.Common
{
    public class FtpHelper
    {
        private string server;
        private string userName;
        private string password;

        public FtpHelper(string vServer, string vName, string vPwd)
        {
            server = vServer;
            userName = vName;
            password = vPwd;
        }

        /// <summary>
        /// 文件上传
        /// </summary>
        /// <param name="filepath">本地路径</param>
        /// <param name="serverFolder">服务器相对路径</param>
        /// <param name="serverName"></param>
        /// <returns></returns>
        public bool Upload(string filepath, string serverFolder, string serverName)
        {
            var info = new FileInfo(filepath);
            if (!info.Exists)
            {
                return false;
            }
            using (var ftp = new FtpConnection(server, userName, password))
            {
                ftp.Open();
                ftp.Login();
                try
                {
                    ftp.SetCurrentDirectory(serverFolder);
                    ftp.PutFile(filepath, serverName + info.Extension);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }
    }
}