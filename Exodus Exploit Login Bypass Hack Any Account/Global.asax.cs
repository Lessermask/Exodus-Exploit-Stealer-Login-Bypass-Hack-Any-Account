using Exodus.Configuration;
using Exodus.Helpers;
using Exodus.Log4Net;
using System;
using System.IO;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.SessionState;

namespace Exodus
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            //-----------------------
            Logger.Logger4Net.Info(String.Format("START --- {0} ---", "Application_Start"));
            //-----------------------

            try { AreaRegistration.RegisterAllAreas(); }
            catch (Exception ex) { Global.Cache.ExceptionSet(Guid.NewGuid().ToString(), ex); }
            //
            try { GlobalConfiguration.Configure(WebApiConfig.Register); }
            catch (Exception ex) { Global.Cache.ExceptionSet(Guid.NewGuid().ToString(), ex); }
            //
            try { RouteConfig.RegisterRoutes(RouteTable.Routes);        }
            catch (Exception ex) { Global.Cache.ExceptionSet(Guid.NewGuid().ToString(), ex); }
            //
            try { FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);  }
            catch (Exception ex) { Global.Cache.ExceptionSet(Guid.NewGuid().ToString(), ex); }
            //
            try { BundleConfig.RegisterBundles(BundleTable.Bundles);}
            catch (Exception ex) { Global.Cache.ExceptionSet(Guid.NewGuid().ToString(), ex); }
            //
            try { Mapper.Mapper.Init(); }
            catch (Exception ex) { Global.Cache.ExceptionSet(Guid.NewGuid().ToString(), ex); }
            // Load Configuration
            try { ConfigFile.CreateOrLoad(Path.Combine(HostingEnvironment.MapPath("~"), "Exodus.config")); }
            catch (Exception ex) { Global.Cache.ExceptionSet(Guid.NewGuid().ToString(), ex); }
            // Load localisation
            try { Global.Localisation.Init(); }
            catch (Exception ex) { Global.Cache.ExceptionSet(Guid.NewGuid().ToString(), ex); }
            //
            try  { Global.Cache.Init(); }
            catch (Exception ex) { Global.Cache.ExceptionSet(Guid.NewGuid().ToString(), ex); }
            //-----------------------
            Logger.Logger4Net.Info(String.Format("END --- {0} ---", "Application_Start"));
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            if (Global.Global.IsApplicationFirstStart && Global.Cache.ExceptionHasAny())
            {
                string message = string.Join(";", Global.Cache.ExceptionGetIdList());
                Global.Global.IsApplicationFirstStart = false;
                throw new Exceptions.ApplicationStartupExceptionList(message);
            }
            else
            {
                Global.Global.IsApplicationFirstStart = false;
            }
        }


        protected void Application_Error(object sender, EventArgs e)
        {
            var ex = Server.GetLastError(); 
            // Startup Errors
            if (ex is Exceptions.ApplicationStartupExceptionList)
            {
                var error = ex as Exceptions.ApplicationStartupExceptionList;
                HttpContext.Current.Response.RedirectToRoute("ApplicationStartupError", new RouteValueDictionary(new { message = error.Message }));
            }
            // Localisation Errors
            else if (ex is Exceptions.LocalisationInitException)
            {
                var error = ex as Exceptions.LocalisationInitException;
                HttpContext.Current.Response.RedirectToRoute("LocalisationError", new RouteValueDictionary(new { message = error.Message }));
            }
            // Path not found
            else if (ex.StackTrace.IndexOf("System.Web.HttpNotFoundHandler") != -1)
            {
                Response.RedirectToRoute("PageNotFound", new RouteValueDictionary(new { aspxerrorpath = ex.Message }));
            }    
            // Known Exception
            else if (ex is Exceptions.ExodusException)
            {
                string guid = Guid.NewGuid().ToString();
                Global.Cache.ExceptionSet(guid, ex);
                Response.RedirectToRoute("ServerError", new RouteValueDictionary(new { id = guid }));
            }
            // Unknown Exception
            else
            {
                string guid = Guid.NewGuid().ToString();
                Global.Cache.ExceptionSet(guid, ex);
                Response.RedirectToRoute("UnknownError", new RouteValueDictionary(new { id = guid }));
            }
            // WRITE LOG
            Logger.Write_Error(ex);
            //
            if (Server != null) { Server?.ClearError(); }
        }

        protected void Application_PostAuthorizeRequest()
        {
            if (HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath.StartsWith("~/api/"))
            {
                //could get current session
                HttpContext.Current.SetSessionStateBehavior(SessionStateBehavior.Required);
            }
        }

        protected void Session_Start(Object sender, EventArgs e)
        {
            //CookieHelper.RemoveAllCookie();
        }

        protected void Session_End(Object sender, EventArgs e)
        {
            Global.Cache.SessionRemove(Session.SessionID);
        }
    }
}