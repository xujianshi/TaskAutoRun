using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TaskAutoRunManager.DataAccess;
using TaskAutoRunManager.Models;

namespace TaskAutoRunManager.Controllers
{
    public class EditSettingController : Controller
    {
        public ActionResult Home(string configId)
        {
            ViewBag.IfExist = false;

            ConfigSetting settingEntity = ConfigSettingDal.GetSetting(config.connstrRead, configId);

            if (settingEntity != null)
            {
                ViewBag.IfExist = true;
            }
            else
            {
                settingEntity = new ConfigSetting();
            }
            return View("~/Views/Edit/ConfigEdit.cshtml", settingEntity);
        }

        public JsonResult DelSetting(string id)
        {
            var success = false;
            string msg;

            if (ConfigSettingDal.DelSetting(config.connstrRead, id))
            {
                success = true;
                msg = "删除成功！";
            }
            else
            {
                success = false;
                msg = "删除失败！";
            }

            var m = new JsonResult()
            {
                Data = new
                {
                    Success = success,
                    Message = msg,
                }
            };
            return Json(m, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SettingSave()
        {
            var success = false;
            string msg;
            var model = new ConfigSetting { Id = Request["Id"], Value = Request["Value"], Fenzu = Request["Fenzu"], Title = Request["title"] };


            if (Convert.ToBoolean(Request["IfExist"]))
            {
                if (ConfigSettingDal.UpDataSetting(config.connstrRead, model))
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
                var hasEntity = ConfigSettingDal.GetSetting(config.connstrRead, XConvert.ToString(model.Id));
                if (hasEntity != null)
                {
                    success = false;
                    msg = "id已经存在！";
                }
                else if (ConfigSettingDal.InsertSetting(config.connstrRead, model))
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
            return Json(m, JsonRequestBehavior.AllowGet);
        }

    }
}
