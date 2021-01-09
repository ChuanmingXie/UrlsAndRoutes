using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UrlsAndRoutes.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            ViewBag.Controller = "Home";
            ViewBag.Action = "Index";
            return View("ActionName");
        }

        /* 定义自定义片段变量 */
        //public ActionResult CustomerVariable()
        //{
        //    ViewBag.Controller = "Home";
        //    ViewBag.Action = "CustomerVariable";
        //    ViewBag.Id = RouteData.Values["id"];
        //    return View();
        //}

        /* 用自定片段变量作为动作方法参数 */
        //public ActionResult CustomerVariable(string id)
        //{
        //    ViewBag.Controller = "Home";
        //    ViewBag.Action = "CustomerVariable";
        //    ViewBag.Id = id;
        //    return View();
        //}

        /* 定义可选URL片段 */
        public ActionResult CustomerVariable( string id)
        {
            ViewBag.Controller = "Home";
            ViewBag.Action = "CustomerVariable";
            ViewBag.Id = id ?? "<no value>";
            return View();
        }
    }
}