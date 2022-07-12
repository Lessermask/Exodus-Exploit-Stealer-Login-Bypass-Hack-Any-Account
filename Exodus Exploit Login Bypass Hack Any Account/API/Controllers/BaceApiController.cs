using Exodus.API.Helpers;
using Exodus.API.Models;
using Exodus.ControllerAttribute;
using Exodus.Enums;
using Exodus.Exceptions;
using Exodus.Log4Net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Exodus.API.Controllers
{
    public class BaseApiController : ApiController
    {
        protected string IPArddress
        {
            get
            {
                if (HttpContext.Current != null && HttpContext.Current.Session != null)
                {
                    return HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] ??
                  HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].Split(',')[0].Trim();
                }
                else
                {
                    return "";
                }
            }
        }

        protected string SessionID
        {
            get
            {
                return HttpContext.Current?.Session?.SessionID ?? "";
            }
        }

        [Compress]
        protected API_Response<T> InvokeAPI<T>(Func<T> action, string api_key, bool checkKey = true)
        {
            Logger.Write_Message($"Start InvokeAPI: {new StackTrace().GetFrame(1).GetMethod().Name}");
            API_Response<T> responce = null;
            Stopwatch clock = Stopwatch.StartNew();
            try
            {
                responce = (!String.IsNullOrEmpty(SessionID) || !checkKey || API_KeyHelper.CheckKey(api_key)) ?
                    new API_Response<T>(action(), $"{clock.ElapsedMilliseconds} ms") :
                    new API_Response<T>(default(T), $"{clock.ElapsedMilliseconds} ms", EN_ErrorCodes.Forbidden, HttpStatusCode.Forbidden);
            }
            catch(ExodusException ex)
            {
                string message = Logger.Write_Error(ex);
                return new API_Response<T>(default(T), $"{clock.ElapsedMilliseconds} ms", ex.ErrorCode, HttpStatusCode.InternalServerError, message);
            }
            catch (Exception ex)
            {
                string message = Logger.Write_Error(ex);
                return new API_Response<T>(default(T), $"{clock.ElapsedMilliseconds} ms", EN_ErrorCodes.SystemExeption, HttpStatusCode.InternalServerError, message);
            }
            finally
            {
                Logger.Write_Message($"End InvokeAPI: {new StackTrace().GetFrame(1).GetMethod().Name}");
            }
            return responce;
        }
    }
}