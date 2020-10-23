using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;

namespace TaskAutoRun
{
    public class NetHelper
    {
        /// <summary>
        /// XML转DataSet
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static DataSet GetDataSet(string data)
        {
            try
            {
                DataSet ds = new DataSet();
                data = ReplaceLowOrderASCIICharacters(data);
                StringReader txtReader = new StringReader(data);
                XmlTextReader xmlReader = new XmlTextReader(txtReader);
                ds.ReadXml(xmlReader);
                return ds;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 获得远程地址xml
        /// </summary>
        /// <param name="target">远程Url</param>
        /// <param name="vEncoding">编码方式</param>
        /// <returns>xml编码</returns>
        public static string GetXml(string target)
        {
            return GetXml(target, Encoding.GetEncoding("gb2312"));
        }

        /// <summary>
        /// 获得远程地址xml
        /// </summary>
        /// <param name="target">远程Url</param>
        /// <param name="vEncoding">编码方式</param>
        /// <param name="vTimeout"></param>
        /// <param name="vReadWriteTimeout"></param>
        /// <returns>xml编码</returns>
        public static string GetXml(string target, Encoding vEncoding, int vTimeout = 10, int vReadWriteTimeout = 10)
        {
            var sb = new StringBuilder();
            sb.AppendLine(target);
            try
            {
                var request = WebRequest.Create(target) as HttpWebRequest;
                if (request != null)
                {
                    request.Method = "Get";
                    request.Timeout = vTimeout * 1000;
                    request.ReadWriteTimeout = vReadWriteTimeout * 1000;
                    var response = request.GetResponse();
                    var stream = response.GetResponseStream();
                    if (stream != null)
                    {
                        using (var responseReader = new StreamReader(stream, vEncoding))
                        {
                            String data = responseReader.ReadToEnd();
                            sb.AppendLine(data);
                            return data;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                sb.AppendLine(ex.ToString());
            }
            return string.Empty;
        }

        /// <summary>
        /// 添加UserAgent参数  add by liap 2015-10-12
        /// </summary>
        /// <param name="target"></param>
        /// <param name="vEncoding"></param>
        /// <param name="vTimeout"></param>
        /// <param name="vReadWriteTimeout"></param>
        /// <returns></returns>
        public static string GetXml_Agent(string target, Encoding vEncoding, int vTimeout, int vReadWriteTimeout)
        {
            StringBuilder sb = new StringBuilder(target);
            try
            {
                HttpWebRequest Request = System.Net.WebRequest.Create(target) as HttpWebRequest;
                Request.Method = "Get";
                Request.UserAgent = "0";
                Request.Timeout = vTimeout * 1000;
                Request.ReadWriteTimeout = vReadWriteTimeout * 1000;
                WebResponse response = Request.GetResponse();
                var stream = response.GetResponseStream();
                if (stream != null)
                {
                    using (var responseReader = new StreamReader(stream, vEncoding))
                    {
                        String data = responseReader.ReadToEnd();
                        sb.AppendLine(data);
                        return data;
                    }
                }
            }
            catch (Exception ex)
            {
                sb.AppendLine(ex.ToString());
            }
            return string.Empty;
        }

        public static DataSet DownLoadData(string target, Encoding vEncoding)
        {
            return DownLoadData(target, vEncoding, 10, 10);
        }

        public static DataSet DownLoadData(string target, Encoding vEncoding, int vTimeout, int vReadWriteTimeout)
        {
            string html = GetXml(target, vEncoding, vTimeout, vReadWriteTimeout);
            return GetDataSet(html);
        }

        public static string DownLoadStrData(string target, Encoding vEncoding, int vTimeOut, int vReadWriteTimeOut)
        {
            return GetXml(target, vEncoding, vTimeOut, vReadWriteTimeOut);
        }

        /// <summary>
        /// 获取远程接口数据-表结构
        /// </summary>
        /// <param name="vUrl">目标接口</param>
        /// <param name="tableName">表名称</param>
        /// <returns>数据表结果集</returns>
        public static DataSet DownLoadRemoteData(string vUrl)
        {
            return DownLoadData(vUrl, Encoding.GetEncoding("gb2312"));
        }

        /// <summary>
        /// 获取远程接口数据-表结构
        /// </summary>
        /// <param name="target">目标接口</param>
        /// <param name="tableName">表名称</param>
        /// <returns>数据表结果集</returns>
        public static DataSet DownLoadRemoteDataUtf8(string target)
        {
            return DownLoadData(target, Encoding.UTF8);
        }

        /// <summary>
        /// 获取远程接口返回-字符串
        /// add by zhanghl  2015-12-1
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static string DownLoadRemoteStrUtf8(string target)
        {
            return DownLoadStrData(target, Encoding.UTF8, 5, 5);
        }

        /// <summary>
        /// 获取远程接口数据-表结构
        /// </summary>
        /// <param name="vUrl">目标接口</param>
        /// <param name="tableName">表名称</param>
        /// <param name="vEnCodeName"></param>
        /// <returns>数据表结果集</returns>
        public static DataTable DownLoadRemoteData(string vUrl, string vTableName, string vEnCodeName)
        {
            try
            {
                DataSet ds = DownLoadData(vUrl, Encoding.GetEncoding(vEnCodeName));
                if (ds != null && ds.Tables.Count > 0)
                {
                    return ds.Tables[vTableName].Copy();
                }
            }
            catch (Exception ex)
            {
            }
            return null;
        }

        /// <summary>
        /// 过滤低位非打印字符
        /// </summary>
        /// <param name="tmp"></param>
        /// <returns></returns>
        public static string ReplaceLowOrderASCIICharacters(string tmp)
        {
            StringBuilder info = new StringBuilder();
            foreach (char cc in tmp)
            {
                int ss = (int)cc;
                if (((ss >= 0) && (ss <= 8)) || ((ss >= 11) && (ss <= 12)) || ((ss >= 14) && (ss <= 32)))
                    info.AppendFormat(" ", ss);//&#x{0:X};
                else info.Append(cc);
            }
            return info.ToString();
        }

        /// <summary>
        /// 获取远程接口数据-表结构
        /// </summary>
        /// <param name="vUrl">目标接口</param>
        /// <param name="tableName">表名称</param>
        /// <returns>数据表结果集</returns>
        public static DataTable DownLoadRemoteData(string vUrl, string tableName)
        {
            DataSet ds = DownLoadData(vUrl, Encoding.GetEncoding("gb2312"));
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[tableName] != null)
            {
                return ds.Tables[tableName].Copy();
            }
            return null;
        }

        /// <summary>
        /// 获取远程接口数据-字符串
        /// </summary>
        /// <param name="target">目标接口</param>
        /// <returns>字符结果集</returns>
        public static String DownLoadRemoteDataStr(string target)
        {
            return GetXml(target, Encoding.GetEncoding("gb2312"));
        }

        /// <summary>
        /// 向某个Url提交数据并取得该地址返回的数据
        /// </summary>
        /// <param name="url"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static string PostData(string url, string param)
        {
            #region post数据

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Timeout = 120000;
            byte[] requestbytes = System.Text.Encoding.ASCII.GetBytes(param);
            req.Method = "post";
            req.ContentType = "application/x-www-form-urlencoded";
            req.ContentLength = requestbytes.Length;
            System.IO.Stream requeststream = req.GetRequestStream();
            requeststream.Write(requestbytes, 0, requestbytes.Length);
            requeststream.Close();
            HttpWebResponse res = (HttpWebResponse)req.GetResponse();
            System.IO.StreamReader sr = new System.IO.StreamReader(res.GetResponseStream(), System.Text.Encoding.Default);
            String backstr = sr.ReadToEnd();
            sr.Close();
            res.Close();
            return backstr;

            #endregion post数据
        }

        /// <summary>
        /// 想某个urlpost请求，返回二进制数据
        /// </summary>
        /// <param name="url"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static byte[] GetBytesByPost(string url, string param, Encoding vCodeType)
        {
            try
            {
                List<byte> datalist = new List<byte>();
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                req.Timeout = 120000;
                byte[] requestbytes = System.Text.Encoding.ASCII.GetBytes(param);
                req.Method = "post";
                req.ContentType = "application/x-www-form-urlencoded";
                req.ContentLength = requestbytes.Length;
                Stream requeststream = req.GetRequestStream();
                requeststream.Write(requestbytes, 0, requestbytes.Length);
                requeststream.Close();
                // 获取对应HTTP请求的响应
                HttpWebResponse response = (HttpWebResponse)req.GetResponse();
                // 获取响应流
                Stream responseStream = response.GetResponseStream();
                // 对接响应流(以"GBK"字符集)
                StreamReader sReader = new StreamReader(responseStream, vCodeType);
                // 开始读取数据
                Char[] sReaderBuffer = new Char[256];
                int count = sReader.Read(sReaderBuffer, 0, 256);
                while (count > 0)
                {
                    String tempStr = new String(sReaderBuffer, 0, count);
                    datalist.AddRange(vCodeType.GetBytes(tempStr));
                    count = sReader.Read(sReaderBuffer, 0, 256);
                }
                // 读取结束
                sReader.Close();
                return datalist.ToArray();
            }
            catch (Exception ex)
            {
                return new byte[0];
            }
        }

        /// <summary>
        /// 向某个url提交数据并读取该地址返回的xml,并将xml转换成dataset,并返回dataset中某个表
        /// </summary>
        /// <param name="url">提交的低至</param>
        /// <param name="param">参数</param>
        /// <param name="tableName">表名</param>
        /// <returns></returns>
        public static DataTable PostData(string url, string param, String tableName)
        {
            string data = PostData(url, param);
            DataSet ds = GetDataSet(data);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[tableName] != null)
            {
                return ds.Tables[tableName].Copy();
            }
            return null;
        }

        /// <summary>
        ///提交数据
        /// </summary>
        /// <param name="vUrl"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static string UploadValues(string vUrl, NameValueCollection values)
        {
            using (WebClient wc = new WebClient())
            {
                Byte[] res = wc.UploadValues(vUrl, "post", values);
                string result = Encoding.UTF8.GetString(res);
                return result;
            }
        }
    }
}