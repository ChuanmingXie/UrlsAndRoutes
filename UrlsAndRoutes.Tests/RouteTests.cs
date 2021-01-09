using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Reflection;
using System.Web;
using System.Web.Routing;

namespace UrlsAndRoutes.Tests
{
    [TestClass]
    public class RouteTests
    {
        private HttpContextBase CreateHttpContext(string targetUrl=null,string httpMethod = "GET")
        {
            /* 创建模仿请求 */
            Mock<HttpRequestBase> mockRequest = new Mock<HttpRequestBase>();
            mockRequest.Setup(m => m.AppRelativeCurrentExecutionFilePath).Returns(targetUrl);
            mockRequest.Setup(m => m.HttpMethod).Returns(httpMethod);

            /* 创建模仿响应 */
            Mock<HttpResponseBase> mockResponse = new Mock<HttpResponseBase>();
            mockResponse.Setup(m => m.ApplyAppPathModifier(It.IsAny<string>())).Returns<string> (s => s);

            /* 创建使用上述请求和相应的模仿上下文 */
            Mock<HttpContextBase> mockContext = new Mock<HttpContextBase>();
            mockContext.Setup(m => m.Request).Returns(mockRequest.Object);
            mockContext.Setup(m => m.Response).Returns(mockResponse.Object);

            /* 返回模仿的上下文*/
            return mockContext.Object;
        }
        private void TestRouteMatch(string url,string controller,string action,object routeProperties=null,string httpMethod="GET")
        {
            /* 准备 */
            RouteCollection routes = new RouteCollection();
            RouteConfig.RegisterRoutes(routes);

            /* 动作 - 处理路由 */
            RouteData result = routes.GetRouteData(CreateHttpContext(url, httpMethod));

            /* 断言 */
            Assert.IsNotNull(result);
            Assert.IsTrue(TestInComingRouteResult(result, controller, action, routeProperties));
        }

        private bool TestInComingRouteResult(RouteData routeResult, string controller, string action, object routeProperties)
        {
            Func<object, object, bool> valCompare = (v1, v2) =>
                {
                    return StringComparer.InvariantCultureIgnoreCase.Compare(v1, v2) == 0;
                };
            bool result = valCompare(routeResult.Values["controllers"], controller)
                && valCompare(routeResult.Values["action"], action);

            if (routeProperties != null)
            {
                PropertyInfo[] propInfo = routeProperties.GetType().GetProperties();
                foreach (var pi in propInfo)
                {
                    if (!(routeResult.Values.ContainsKey(pi.Name) && valCompare(routeResult.Values[pi.Name], pi.GetValue(routeProperties, null)))){
                        result = false;
                        break;
                    }
                }
            }
            return result;
        }

        private void TestRouteFail(string url)
        {
            /* 准备*/
            RouteCollection routes = new RouteCollection();
            RouteConfig.RegisterRoutes(routes);
            /* 动作 - 处理路由*/
            RouteData result = routes.GetRouteData(CreateHttpContext(url));
            /* 断言 */
            Assert.IsTrue(result == null || result.Route == null);
        }

        [TestMethod]
        public void TestInComingRoutes()
        {
            /* 简单路由 */
            TestRouteMatch("~/Admin/Index", "Admin", "Index");
            TestRouteMatch("~/One/Two", "One", "Two");
            TestRouteMatch("~/Admin/Index", "Admin", "Index");
            TestRouteFail("~/Admin/Index/Segment");
            TestRouteFail("~/Admin");
            /* 测试默认值 */
            TestRouteMatch("~/", "Home", "Index");
            TestRouteMatch("~/Customer", "Customer", "Index");
            TestRouteMatch("~/Customer/List", "Customer", "List");
            TestRouteFail("~/Customer/List/All");
            /* 测试静态片段 */
            TestRouteMatch("~Shop/Index", "Home", "Index");
            /* 定义自定义片段变量 */
            TestRouteMatch("~/", "Home", "Index", new { id = "DefaultId" });
            TestRouteMatch("~/Customer", "Customer", "Index", new { id = "DefaultId" });
            TestRouteMatch("~/Customer/List", "Customer", "List", new { id = "DefaultId" });
            TestRouteMatch("~/Customer/List/All", "Customer", "List", new { id = "All" });
            TestRouteFail("~/Customer/List/All/Delete");
            /* 定义可选URL片段 */
            TestRouteMatch("~/", "Home", "Index");
            TestRouteMatch("~/Customer", "Customer", "Index");
            TestRouteMatch("~/Customer", "Customer", "index");
            TestRouteMatch("~/Customer/List", "Customer", "List");
            TestRouteMatch("~/Customer/List/All", "Customer", "List", new { id = "All" });
            TestRouteFail("~/Customer/List/All/Delete");
            /* 测试catchall片段变量 */
            TestRouteMatch("~/", "Home", "Index");
            TestRouteMatch("~/Customer", "Customer", "Index");
            TestRouteMatch("~/Customer/List", "Customer", "Index");
            TestRouteMatch("~/Customer/List/All", "Customer", "List", new { id = "All" });
            TestRouteMatch("~/Customer/List/ALl/Delete", "Customer", "List", new { id = "All", catchall = "Delete" });
            TestRouteMatch("~/Customer/List/ALl/Delete/Perm", "Customer", "List", new { id = "All", catchall = "Delete/Perm" });

        }
    }
}
