using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TaskAutoRunManager.Common;
using TaskAutoRunManager.DataAccess;
using TaskAutoRunManager.Models;

namespace TaskAutoRunManager.Controllers
{
    public class EditController : Controller
    {
        /// <summary>
        /// 编辑页面
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public ActionResult Index(long taskId)
        {
            TuanTask taskEntity;
            ViewBag.IfExist = false;
            taskEntity = TuanTaskDal.GetTask(config.connstrRead, taskId);
            if (taskEntity != null)
            {
                ViewBag.IfExist = true;
            }
            else
            {
                taskEntity = new TuanTask();
            }
            return View(taskEntity);
        }

        public JsonResult save()
        {
            bool success = false;
            string msg = string.Empty;
            TuanTask taskEntity = new TuanTask();
            taskEntity.Id = Request["id"];
            taskEntity.Name =Request["name"];
            taskEntity.Path =Request["path"] ;
            taskEntity.RunType = XConvert.ToInt32(Request["runType"]);
            taskEntity.RunMiniteOf =XConvert.ToInt32( Request["runMiniteOf"]);
            taskEntity.RunTimeAt = Request["runTimeAt"];
            taskEntity.Author = Request["author"];
            taskEntity.IsOpen =XConvert.ToInt32( Request["isOpen"]);

            
            if (Convert.ToBoolean( Request["IfExist"]))
            {
                if (TuanTaskDal.UpdataTask(config.connstrRead, taskEntity))
                {
                    success = true;
                    msg = "更新成功！";
                }
                else
                {
                    success = false;
                    msg = "更新失败！";
                }
            }
            else
            {
                var hasEntity = TuanTaskDal.GetTask(config.connstrRead, XConvert.ToInt64(taskEntity.Id));
                if (hasEntity != null)
                {
                    success = false;
                    msg = "id已经存在！";
                }
                else if (TuanTaskDal.InsertTask(config.connstrRead, taskEntity))
                {
                    success = true;
                    msg = "添加成功！";
                }
                else
                {
                    success = false;
                    msg = "添加失败！";
                }
            }
            var m = new JsonResult()
            {
                Data = new 
                {
                    Success = success,
                    Message = msg,
                }
            };
            return Json(m,JsonRequestBehavior.AllowGet);
        }

        public ActionResult upload()
        {
            string filePath = @"E:\xjs123456.txt";
            FtpHelper ftp = new FtpHelper("218.30.110.109", "message", "9s3s3a8ge");
            bool reslut = ftp.Upload(filePath, "/tuan/ElectronicContract/test/", Guid.NewGuid().ToString());
            return Content(reslut.ToString());
        }
    }

   
}
