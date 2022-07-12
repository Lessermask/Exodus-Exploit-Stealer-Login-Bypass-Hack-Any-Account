using Exodus.API.Models;
using Exodus.Domain;
using Exodus.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Exodus.Exceptions;
using Exodus.ControllerAttribute;

namespace Exodus.API.Controllers
{
    public class AppH2OController : BaseApiController
    {
        [Compress]
        [HttpPost]
        public API_Response<decimal> Obligations_ByUserID_CurrentMonth(long UserID, En_Currency currency = En_Currency.USD, [FromUri]string api_key = null)
        {
            return InvokeAPI(() =>
            {
                if (!Global.Cache.CheckUserExists(UserID)) { throw new UserNotFoundException(); }
                //
                return _DL.H2O.Get.Obligations_ByUserID_CurrentMonth(UserID, currency);
            }, api_key);
        }

        [Compress]
        [HttpPost]
        public API_Response<decimal> Intentions_ByUserID_CurrentMonth(long UserID, En_Currency currency = En_Currency.USD, [FromUri]string api_key = null)
        {
            return InvokeAPI(() => 
            {
                if (!Global.Cache.CheckUserExists(UserID)) { throw new UserNotFoundException(); }
                //
                return _DL.H2O.Get.Intentions_ByUserID_CurrentMonth(UserID, currency);
            }, api_key);
        }

        [Compress]
        [HttpPost]
        public API_Response<decimal> Intentions_ByUserID_n_Month(long UserID,int year, int month, En_Currency currency = En_Currency.USD, [FromUri]string api_key = null)
        {
            return InvokeAPI(() => 
            {
                if (!Global.Cache.CheckUserExists(UserID)) { throw new UserNotFoundException(); }
                //
                return _DL.H2O.Get.Intentions_ByUserID_CurrentMonth(UserID, new DateTime(year, month, 1), currency);
            }, api_key);
        }
    }
}
