using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using TaskAutoRunManager.Models;


namespace TaskAutoRunManager.DataAccess
{
    public class ConfigSettingDal
    {

        public static List<ConfigSetting> GetConfigSettingList(string connstr)
        {
            const string sql = @"select * from TuanConfigSetting with(nolock)";

            var dt = DBHelper.CreateTable(connstr, sql);

            return dt.Rows.Count > 0 ? GetList(dt) : null;
        }

        /// <summary>
        /// 根据id获取任务详情
        /// </summary>
        /// <param name="connstr"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Models.ConfigSetting GetSetting(string connstr, string id)
        {
            const string sql = @"select top 1* from TuanConfigSetting with(nolock) where id=@id";
            var parameters = new SqlParameter[] { new SqlParameter() { ParameterName = "@id", Value = id } };
            var dt = DBHelper.CreateTable(connstr, sql, parameters);
            return dt.Rows.Count > 0 ? GetOne(dt.Rows[0]) : null;
        }

        public static bool DelSetting(string connstr, string id)
        {
            var parameters = new List<SqlParameter>()
            {
                new SqlParameter() { ParameterName = "@Id", Value = id },
            };

            const string sql = @"
                        delete [TuanConfigSetting]
                            WHERE 
                                [id] = @Id
                    ";

            var i = DBHelper.AddUpdateDelete(connstr, sql, parameters.ToArray());
            return i > 0;
        }


        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="connstr"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static bool UpDataSetting(string connstr, ConfigSetting entity)
        {
            var parameters = new List<SqlParameter>()
            {
                new SqlParameter() { ParameterName = "@Id", Value = entity.Id },
                new SqlParameter() { ParameterName = "@Value", Value = entity.Value },
                new SqlParameter() { ParameterName = "@Fenzu", Value = entity.Fenzu },
                new SqlParameter(){ParameterName = "@title",Value = entity.Title}
            };

            const string sql = @"
                        UPDATE [TuanConfigSetting]
                            SET
                                [id] = @Id
                                ,[value] = @Value
                                ,[fenzu] = @Fenzu
                                ,[title]=@title
                            WHERE 
                                [id] = @Id
                    ";

            var i = DBHelper.AddUpdateDelete(connstr, sql, parameters.ToArray());
            return i > 0;
        }

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="connstr"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static bool InsertSetting(string connstr, ConfigSetting entity)
        {
            var parameters = new List<SqlParameter>()
            {
                new SqlParameter() { ParameterName = "@Id", Value = entity.Id },
                new SqlParameter() { ParameterName = "@Value", Value = entity.Value },
                 new SqlParameter() { ParameterName = "@Fenzu", Value = entity.Fenzu },
                 new SqlParameter(){ParameterName = "@title",Value = entity.Title}
            };

            const string sql = @"INSERT INTO TuanConfigSetting (
                                        [id]
                                        ,[value]
                                        ,[fenzu]
                                        ,[title]
                                        ) VALUES (
                                        @Id,@Value,@Fenzu,@title) ";


            var i = DBHelper.AddUpdateDelete(connstr, sql, parameters.ToArray());
            return i > 0;
        }

        /// <summary>
        ///批量转换
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public static List<Models.ConfigSetting> GetList(DataTable dataTable)
        {
            if (dataTable != null)
            {
                return dataTable.AsEnumerable().Select(GetOne).ToList();
            }
            return null;
        }

        /// <summary>
        /// 一对一转换
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public static Models.ConfigSetting GetOne(DataRow row)
        {
            return new Models.ConfigSetting()
            {
                Id = row["id"].ToString(),
                Value = row["value"].ToString(),
                Fenzu = row["fenzu"].ToString(),
                Title = row["title"].ToString()
            };
        }
    }
}