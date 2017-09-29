using System.Web.Mvc;
using TaskAutoRunManager.DataAccess;
using TaskAutoRunManager.Models;


namespace TaskAutoRunManager.Controllers
{
    public class MainController : Controller
    {
        //
        // GET: /Main/

        /// <summary>
        /// 主页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
           ViewBag.Manager = 1;
            return View();
        }

        /// <summary>
        /// 通过状态，起始页，每页条数获取任务列表
        /// 用于分页
        /// </summary>
        /// <param name="state"></param>
        /// <param name="start"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult TuanListAjax(int state,int start,int page,int show)
        {
            ViewBag.Manager = show;
            var tuanTaskList = new TuanTaskList();
            tuanTaskList.tuanTaskList = DataAccess.TuanTaskDal.GetTaskList(config.connstrRead, state,start,page);
            return View("~/Views/Main/TaskList.cshtml", tuanTaskList);
        }

        /// <summary>
        /// 通过状态获取数据总条数
        /// 用于分页统计总条数
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public ActionResult TuanListAjaxAll(int state)
        {
            int Count = DataAccess.TuanTaskDal.GetCountByState(config.connstrRead, state);
            return Content(Count.ToString());
        }

        public ActionResult RunState(string id, int isOpen)
        {
            if (DataAccess.TuanTaskDal.UpdateRunState(config.connstrWrite, id, isOpen) > 0)
            {
                if (isOpen == 1)
                    return Content("已停用");
                else
                    return Content("已启用");
            }
            return Content("0");
        }

    }
}
