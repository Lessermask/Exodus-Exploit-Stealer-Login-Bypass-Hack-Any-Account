using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Exodus.API.Models;
using Exodus.ControllerAttribute;
using Exodus.Events;
using Exodus.Exceptions;

namespace Exodus.API.Controllers
{
    public class AppOwnInitiativeController : BaseApiController
    {
        [HttpGet]
        [Compress]
        public API_Response<int> ReportEvent(long TagID, long reporterUserID, [FromUri]string api_key = null)
        {
            return InvokeAPI(() => 
            {
                if (!Global.Cache.CheckTagExists(TagID)) { throw new TagNotFoundException(TagID); }
                if (!Global.Cache.CheckUserExists(reporterUserID)) { throw new UserNotFoundException(reporterUserID); }
                //
                EventCreator.UserReportedKeyTagRelatedEvent(reporterUserID, TagID);
                //
                return 1;
            }, api_key);
        }
    }
}
