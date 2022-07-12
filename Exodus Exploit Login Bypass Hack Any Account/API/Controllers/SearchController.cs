using Exodus.API.Helpers;
using Exodus.API.Models;
using Exodus.ControllerAttribute;
using Exodus.DTO;
using Exodus.Helpers;
using Exodus.Models.Search;
using Exodus.Service;
using Exodus.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Exodus.API.Controllers
{
    public class SearchController : BaseApiController
    {
        [Compress]
        [HttpGet]
        public API_Response<List<UserSearch>> Users(string query, int count = int.MaxValue, [FromUri] string api_key = "")
        {
            return InvokeAPI(() =>
            {
                count = (count < 0) ? int.MaxValue : count;
                return _SL.Search.UsersByString(query).Take(count).ToList();
            }, api_key);
        }

        [Compress]
        [HttpGet]
        public API_Response<List<TagSearch>> Tags(string query, int count = int.MaxValue, [FromUri] string api_key = "")
        {
            return InvokeAPI(() =>
            {
                count = (count < 0) ? int.MaxValue : count;
                return _SL.Search.TagsByString(query).OrderBy(a => a.Name).Take(count).ToList();
            }, api_key);
        }

        [Compress]
        [HttpGet]
        public API_Response<List<object>> Any(string query, int count = int.MaxValue, [FromUri] string api_key = "")
        {
            return InvokeAPI(() =>
            {
                count = (count < 0) ? int.MaxValue : count;
                return _SL.Search.Any(query).Take(count).ToList();
            }, api_key);
        }

    }
}