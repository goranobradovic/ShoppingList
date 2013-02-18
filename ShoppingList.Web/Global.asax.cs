using System;
using System.Net;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Microsoft.Web.WebPages.OAuth;
using ShoppingList.Web.Helpers;
using log4net;

namespace ShoppingList.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode,
    // visit http://go.microsoft.com/?LinkId=9394801

    public class ShoppingListApp : System.Web.HttpApplication
    {

        private readonly ILog _logger = LogManager.GetLogger(typeof(ShoppingListApp));
        private static string _host = null;

        static ShoppingListApp()
        {
            log4net.Config.XmlConfigurator.Configure();
        }



        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();

            //BootstrapSupport.BootstrapBundleConfig.RegisterBundles(System.Web.Optimization.BundleTable.Bundles);
            BootstrapMvcSample.ExampleLayoutsRouteConfig.RegisterRoutes(RouteTable.Routes);

            this.Error += ShoppingListApp_Error;

            var t = new Timer(delegate
                {
                    try
                    {
                        if (string.IsNullOrEmpty(_host)) return;

                        _logger.InfoFormat("Performing keep-alive request to {0}", _host);
                        var response = WebRequest.Create(_host).GetResponse();
                        _logger.InfoFormat("Got keep-alive response. Length:{0}", response.ContentLength);
                    }
                    catch (Exception ex)
                    {
                        _logger.Error("Error while trying to perform keep-alive request", ex);
                    }
                });
            t.Change(TimeSpan.FromSeconds(300), TimeSpan.FromSeconds(300));
        }

        void ShoppingListApp_Error(object sender, EventArgs e)
        {
            var ex = Server.GetLastError();
            _logger.Fatal(ex);
        }

        void Application_BeginRequest(Object source, EventArgs e)
        {
            if (string.IsNullOrEmpty(_host))
            {
                var url = HttpContext.Current.Request.Url;
                _host = string.Format("{0}{1}{2}:{3}{4}",
                    url.Scheme,
                    Uri.SchemeDelimiter,
                    url.Host,
                    url.Port,
                    UrlHelper.GenerateContentUrl("~", HttpContext.Current.Request.RequestContext.HttpContext));
            }

        }

    }
}