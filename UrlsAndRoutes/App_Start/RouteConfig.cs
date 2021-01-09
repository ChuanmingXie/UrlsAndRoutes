using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace UrlsAndRoutes
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            //    routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //    routes.MapRoute(
            //        name: "Default",
            //        url: "{controller}/{action}/{id}",
            //        defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            //    );
            /* 简单路由 */
            //Route myRoute = new Route("{controller}/{action}", new MvcRouteHandler());
            //routes.Add("MyRoutes", myRoute);
            //routes.MapRoute("MyRoutes", "{controller}/{action}");

            /* 默认路由 */
            //routes.MapRoute("MyRoutes", "{controller}/{action}", new { action = "Index" });
            //routes.MapRoute("MyRoutes", "{controller}/{action}", new { controller = "Home", action = "Index" });

            /* 静态URL片段 */
            //routes.MapRoute("ShopSchema2", "Shop/OldAction", new { cntroller = "Home", action = "Index" });
            //routes.MapRoute("ShopSchema", "Shop/{action}", new { controller = "Home" });
            //routes.MapRoute("", "X{controller}/{action}");
            //routes.MapRoute("MyRoutes", "{controller}/{action}", new { controller = "Home", action = "Index" });
            //routes.MapRoute("", "public/{controller}/{action}", new { controller = "Home", action = "Index" });

            /* 定义自定义片段变量 */
            //routes.MapRoute("MyRoute", "{controller}/{action}/{id}", new { controller = "Home", action = "action", id = "DefaultId" });
            /* 定义可选URL片段 */
            routes.MapRoute("MyRoute", "{controller}/{action}/{id}"
                , new { controller = "Home", action = "Index", id = UrlParameter.Optional });
            /* 定义可边长路由 */
            routes.MapRoute("MyRoute", "{controller}/{action}/{id}/{*catchall}"
                , new { controller = "Home", action = "Index", id = UrlParameter.Optional });
            /* 按命名空间区分控制器路由顺序 */
            routes.MapRoute("MyRoute", "{controller}/{action}/{id}/{*catachall}"
                , new { controller = "Home", action = "Index" , id = UrlParameter.Optional }
                , new[] { "UrlsAndRoutes.AddControllers" });
            /* 使用多路由控制命名空间解析 */
            routes.MapRoute("AddControllerRoute", "Home/{action}/{id}/{*catchall}"
                , new { controller = "Home", action = "Index", id = UrlParameter.Optional }
                , new[] { "UrlsAndRoutes.AddControllers" });
            routes.MapRoute("AddControllerRoute", "Home/{action}/{id}/{*catchall}"
                , new { controller = "Home", action = "Index", id = UrlParameter.Optional }
                , new[] { "UrlsAndRoutes.Controllers" });
            /* 禁用备用命名空间 */
             Route myRoute= routes.MapRoute("AddControllerRoute", "Home/{action}/{id}/{*catchall}"
                , new { controller = "Home", action = "Index", id = UrlParameter.Optional }
                , new[] { "UrlsAndRoutes.AddControllers" });
            myRoute.DataTokens["UseNamespaceFallback"] = false;

        }
    }
}
