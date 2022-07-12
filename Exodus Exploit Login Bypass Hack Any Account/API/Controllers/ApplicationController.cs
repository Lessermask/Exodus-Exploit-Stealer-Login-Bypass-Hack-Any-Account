using Exodus.API.Helpers;
using Exodus.API.Models;
using Exodus.Domain;
using Exodus.Exceptions;
using Exodus.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;
using Exodus.Enums;
using Exodus.DTO_Api;
using Exodus.Extensions;
using Exodus.ControllerAttribute;

namespace Exodus.API.Controllers
{
    public class ApplicationController : BaseApiController
    {
        /// <summary>
        /// For getting Application
        /// </summary>
        /// <param name="ApplicationID">Applicatiion ID</param>
        /// <param name="api_key">api key</param>
        /// <returns></returns>
        [HttpPost]
        [Compress]
        public API_Response<VM_Application> Get_ByID(EN_ApplicationType ApplicationID, [FromUri]string api_key = null)
        {
            return InvokeAPI(() => 
            {
                return Global.Cache.dicApplications[ApplicationID];
            }, api_key);
        }

        /// <summary>
        /// Get all applications
        /// </summary>
        /// <param name="api_key">api key</param>
        /// <returns></returns>
        [HttpPost]
        [Compress]
        public API_Response<List<VM_Application>> Get_All([FromUri]string api_key = null)
        {
            return InvokeAPI(() => 
            {
                return Global.Cache.dicApplications.Values.ToList();
            }, api_key);
        }

        [HttpPost]
        [Compress]
        public API_Response<long> Settings_Write(DTO_ApplicationSettings appSettings, [FromUri]string api_key = null)
        {
            return InvokeAPI(() => 
            {
                if (!Global.Cache.CheckTagExists(appSettings.TagID)) { throw new TagNotFoundException(); }
                if (!Global.Cache.dicApplications.ContainsKey((EN_ApplicationType)appSettings.ApplicationID)) { throw new ApplicationNotFoundException(); }
                //
                return  _DL.Application.Add.Settings(appSettings);
            }, api_key);
        }

        [HttpPost]
        [Compress]
        public API_Response<DTO_ApplicationSettings> Settings_Read(long TagID, EN_ApplicationType ApplicationID, [FromUri]string api_key = null)
        {
            return InvokeAPI(() => 
            {
                if (!Global.Cache.CheckTagExists(TagID)) { throw new TagNotFoundException(); }
                if (!Global.Cache.dicApplications.ContainsKey(ApplicationID)) { throw new ApplicationNotFoundException(); }
                //
                return _DL.Application.Get.Settings(TagID, ApplicationID.ToInt());
            }, api_key);
        }
    }
}