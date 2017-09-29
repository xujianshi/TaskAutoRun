using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace TaskAutoRunManager.DataAccess
{
    /// <summary>
    /// 数据库调用类
    /// </summary>
    public class DBHelper
    {
        /// <summary>
        /// 带参数(占位符)的增删改语句执行
        /// </summary>
        /// <param name="sql">要执行的增删改命令</param>
        /// <param name="pas">占位符们</param>
        /// <returns>得到影响的行数</returns>
        public static int AddUpdateDelete(string connStr, string sql, params SqlParameter[] pas)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);

                    foreach (SqlParameter pa in pas)
                    {
                        cmd.Parameters.Add(pa);
                    }
                   return cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    //throw new Exception(ex.Message);
                }
            }
            return 0;
        }

        /// <summary>
        /// 创建表格，得到表格类型的数据
        /// </summary>
        /// <param name="sql">select语句</param>
        /// <returns>根据select语句得到的表格</returns>
        public static DataTable CreateTable(string connStr, string sql)
        {
            DataTable dt = new DataTable();
            SqlConnection conn = new SqlConnection(connStr);
            using (SqlDataAdapter da = new SqlDataAdapter(sql, conn))
            {
                da.Fill(dt);
            }
            return dt;
        }
        /// <summary>
        /// 创建表格，得到表格类型的数据
        /// </summary>
        /// <param name="sql">select语句</param>
        /// <param name="pars">@占位符和它的值</param>
        /// <returns>根据select语句得到的表格</returns>
        public static DataTable CreateTable(string connStr, string sql, params SqlParameter[] pars)
        {
            using (SqlConnection con = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand(sql, con);

                foreach (SqlParameter pa in pars)
                {
                    cmd.Parameters.Add(pa);
                }
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    try
                    {
                        da.Fill(dt);
                        cmd.Parameters.Clear();
                    }
                    catch //(SqlException ex)
                    {
                        //throw new Exception(ex.Message);
                        return dt;
                    }
                    return dt;
                }
            }
        }
    }
}