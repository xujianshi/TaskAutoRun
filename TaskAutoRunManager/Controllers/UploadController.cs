using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TaskAutoRunManager.Controllers
{

    public class UploadController : Controller
    {
        //http://www.uploadify.com/demos/
        // GET: /Upload/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult UpLoad()
        {
            return Content("sucess");
        }

    }
}
