using Exodus.API.Helpers;
using Exodus.API.Models;
using Exodus.Domain;
using Exodus.Enums;
using Exodus.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using Exodus.Exceptions;
using Exodus.ControllerAttribute;

namespace Exodus.API.Controllers
{
    public class ExodusController : BaseApiController
    {
        [Compress]
        [HttpGet]
        public API_Response<string> Info()
        {
            return InvokeAPI(() => { return "Exodus API v1.0 Provide in 2018"; }, "", false); 
        }
    }
}