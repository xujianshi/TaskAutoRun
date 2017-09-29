using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using TaskAutoRunManager.Models;

namespace TaskAutoRunManager.DataAccess
{
    public class TuanTaskDal
    {
        public const string columns = @"
                                         [id]
                                        ,[name]
                                        ,[path]
                                        ,[runType]
                                        ,[runMiniteOf]
                                        ,[runTimeAt]
                                        ,[Author]
                                        ,[LastRunTime]
                                        ,[isOpen]";

        public const string selectColumns = @"select " + columns + @" FROM [TaskAutoRun] with(nolock) ";
        public const string selectTop1Columns = @"select top 1 " + columns + @" FROM [TaskAutoRun] with(nolock) ";

        /// <summary>
        /// 一对一转换
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public static TuanTask GetOne(DataRow row)
        {
            return new TuanTask()
            {
                Id = row["ID"].ToString(),
                Name = row["name"].ToString(),
                Path = row["path"].ToString(),
                RunType = XConvert.ToInt32(row["runType"]),
                RunMiniteOf = XConvert.ToInt32(row["runMiniteOf"]),
                RunTimeAt = row["runTimeAt"].ToString(),
                Author = row["Author"].ToString(),
                LastRunTime = XConvert.ToDateTime(row["LastRunTime"]),
                IsOpen = XConvert.ToInt16(row["isOpen"]),
            };
        }

        /// <summary>
        ///批量转换
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public static List<TuanTask> GetList(DataTable dataTable)
        {
            if (dataTable != null)
            {
                return dataTable.AsEnumerable().Select(GetOne).ToList();
            }
            return null;
        }

        /// <summary>
        /// 获取任务列表
        /// </summary>
        /// <param name="connstr"></param>
        /// <returns></returns>
        public static List<TuanTask> GetTaskList(string connstr)
        {
            string sql = selectColumns;
            var dt = DBHelper.CreateTable(connstr, sql);
            if (dt.Rows.Count > 0)
            {
                return GetList(dt);
            }
            return null;
        }

        /// <summary>
        /// 通过状态，起始页，每页条数获取任务列表
        /// state 0:全部 1：启用 2：停用
        /// </summary>
        /// <param name="connstr"></param>
        /// <param name="state"></param>
        /// <param name="start">起始页</param>
        /// <param name="page">每页条数</param>
        /// <returns></returns>
        public static List<TuanTask> GetTaskList(string connstr, int state, int start,int page)
        {
            var dt = new DataTable();
            string sql = string.Empty;
            if (state == 0)
            {
                sql = selectColumns + @" order by id ";
                dt = DBHelper.CreateTable(connstr, sql);
            }
            else
            {
                sql = selectColumns + " where isOpen=@isOpen  order by id ";
                SqlParameter p_IsOpen = new SqlParameter("@isOpen", state);
                dt = DBHelper.CreateTable(connstr, sql, p_IsOpen);
            }
            if (dt.Rows.Count > 0)
            {
                return GetList(dt);
            }
            return null;
        }

        /// <summary>
        /// 通过状态获取数据数据总条数
        /// </summary>
        /// <param name="connstr"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public static int GetCountByState(string connstr, int state)
        {
            var dt = new DataTable();
            string sql = string.Empty;
            if (state == 0)
            {
                sql = @"select count(*) from TaskAutoRun with(nolock) ";
                dt = DBHelper.CreateTable(connstr, sql);
            }
            else
            {
                sql = @"select count(*) from TaskAutoRun with(nolock) where isOpen=@isOpen";
                SqlParameter p_IsOpen = new SqlParameter("@isOpen", state);
                dt = DBHelper.CreateTable(connstr, sql, p_IsOpen);
            }
            if (dt.Rows.Count > 0)
            {
                return Convert.ToInt32(dt.Rows[0][0]);
            }
            return 0;
        }

        /// <summary>
        /// 更新启用或停用状态
        /// </summary>
        /// <param name="connstr"></param>
        /// <param name="id"></param>
        /// <param name="isOpen"></param>
        /// <returns></returns>
        public static int UpdateRunState(string connstr, string id, int isOpen)
        {
            string sql = @"update TaskAutoRun set isOpen=@isOpen where id=@id";
            isOpen = isOpen == 1 ? 2 : 1;
            SqlParameter p_IsOpen = new SqlParameter("@isOpen", isOpen);
            SqlParameter p_Id = new SqlParameter("@id", id);
            return DBHelper.AddUpdateDelete(connstr, sql, p_IsOpen, p_Id);
        }

        /// <summary>
        /// 根据id获取任务详情
        /// </summary>
        /// <param name="connstr"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static TuanTask GetTask(string connstr, long id)
        {
            string sql = selectColumns + " WHERE id=@id";
            var parameters = new SqlParameter[] { new SqlParameter() { ParameterName = "@id", Value = id } };
            var dt = DBHelper.CreateTable(connstr, sql, parameters);
            if (dt.Rows.Count > 0)
            {
                return GetOne(dt.Rows[0]);
            }
            return null;
        }


        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="connstr"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static bool InsertTask(string connstr, TuanTask entity)
        {
            string sql = "";
              List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter() { ParameterName = "@id", Value = entity.Id },
                new SqlParameter() { ParameterName = "@name", Value = entity.Name },
                new SqlParameter() { ParameterName = "@path", Value = entity.Path },
                new SqlParameter() { ParameterName = "@runType", Value = entity.RunType },
                new SqlParameter() { ParameterName = "@Author", Value = entity.Author },
                new SqlParameter() { ParameterName = "@isOpen", Value = entity.IsOpen }
            };
              if (entity.RunTimeAt != "" && entity.RunType != 1)
            {
                sql = @"INSERT INTO TaskAutoRun (
                                        [id]
                                        ,[name]
                                        ,[path]
                                        ,[runType]
                                        ,[runTimeAt]
                                        ,[Author]
                                        ,[isOpen]  ) VALUES (
                                        @id,@name,@path,@runType,@runTimeAt,@Author,@isOpen) ";
                parameters.Add(new SqlParameter()
                {
                    ParameterName = "@runTimeAt",
                    Value = entity.RunTimeAt
                });
            }
              else if (entity.RunMiniteOf.ToString() != "" && entity.RunType == 1)
            {
                sql = @"INSERT INTO TaskAutoRun (
                                        [id]
                                        ,[name]
                                        ,[path]
                                        ,[runType]
                                        ,[runMiniteOf]
                                        ,[Author]
                                        ,[isOpen]  ) VALUES (
                                        @id,@name,@path,@runType,@runMiniteOf,@Author,@isOpen) ";
                parameters.Add(new SqlParameter()
                {
                    ParameterName = "@runMiniteOf",
                    Value = (entity.RunMiniteOf.ToString() == "") ? 0 : entity.RunMiniteOf
                });
            }
            else
            {
                return false;
            }



            int i = DBHelper.AddUpdateDelete(connstr, sql, parameters.ToArray());
            if (i>0)
            {
                return true;
            }
            return false;
        }



        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="connstr"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static bool UpdataTask(string connstr, TuanTask entity)
        {
            string sql = string.Empty;
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter() { ParameterName = "@id", Value = entity.Id },
                new SqlParameter() { ParameterName = "@name", Value = entity.Name },
                 new SqlParameter() { ParameterName = "@path", Value = entity.Path },
                new SqlParameter() { ParameterName = "@runType", Value = entity.RunType },
                new SqlParameter() { ParameterName = "@Author", Value = entity.Author },
                new SqlParameter() { ParameterName = "@isOpen", Value = entity.IsOpen }
            };
            if (entity.RunTimeAt != ""&&entity.RunType!=1)
            {
                sql = @"
                        UPDATE [TaskAutoRun]
                            SET
                                [name] = @name
                                ,[path] = @path
                                ,[runType] = @runType
                                ,[runTimeAt] = @runTimeAt
                                ,[Author] = @Author
                                ,[isOpen] = @isOpen
                            WHERE 
                                [id] = @id
                    ";
                parameters.Add(new SqlParameter()
                {
                    ParameterName = "@runTimeAt",
                    Value = entity.RunTimeAt
                });
            }
            else if (entity.RunMiniteOf.ToString() != "" && entity.RunType == 1)
             {
                 sql = @"
                        UPDATE [TaskAutoRun]
                            SET
                                [name] = @name
                                ,[path] = @path
                                ,[runType] = @runType
                                ,[runMiniteOf] = @runMiniteOf
                                ,[Author] = @Author
                                ,[isOpen] = @isOpen
                            WHERE 
                                [id] = @id
                    ";
                 parameters.Add(new SqlParameter()
                 {
                     ParameterName = "@runMiniteOf",
                     Value = (entity.RunMiniteOf.ToString() == "") ? 0 : entity.RunMiniteOf
                 });
             }
            else
            {
                return false;
            }

          
            int i = DBHelper.AddUpdateDelete(connstr, sql, parameters.ToArray());
            if (i > 0)
            {
                return true;
            }
            return false;
        }
    }
}