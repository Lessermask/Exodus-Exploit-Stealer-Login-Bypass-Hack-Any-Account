using Exodus.API.Models;
using Exodus.ControllerAttribute;
using Exodus.Domain;
using Exodus.DTO_Api;
using Exodus.Exceptions;
using Exodus.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Exodus.API.Controllers
{
    public class PublicProfileController : BaseApiController
    {
        [Compress]
        [HttpPost]
        public API_Response<VM_User> GetUser([FromBody]long UserID, [FromUri]string api_key = null)
        {
            return InvokeAPI(() =>
            {
                // Check User
                if (!Global.Cache.CheckUserExists(UserID)) { throw new UserNotFoundException(); }
                //
                return _DL.User.Get.ByID(UserID);
            }, api_key);
        }

        [Compress]
        [HttpPost]
        public API_Response<IEnumerable<DTO_User_LightModel>> GetUserRelations([FromBody]long UserID, [FromUri]string api_key = null)
        {
            return InvokeAPI(() => 
            {
                if (!Global.Cache.CheckUserExists(UserID)) { throw new UserNotFoundException(); }
                return _DL.User.Get.RelationsByID(UserID)
                .Select(a => a.LightModel);
            }, api_key);
        }

        [Compress]
        [HttpPost]
        public API_Response<List<VM_Tag>> GetTagsPublicByUserID([FromBody]long UserID, [FromUri]string api_key = null)
        {
            return InvokeAPI(() => 
            {
                if (!Global.Cache.CheckUserExists(UserID)) { throw new UserNotFoundException(); }
                return _DL.Tag.Get.Public_ByUserID(UserID);
            }, api_key);
        }

        [Compress]
        [HttpPost]
        public API_Response<List<VM_Tag>> GetTagsCommon(long UserID, long CommonUserID, [FromUri]string api_key = null)
        {
            return InvokeAPI(() => 
            {
                if (!Global.Cache.CheckUserExists(UserID)) { throw new UserNotFoundException(); }
                return _DL.Tag.Get.TagsCommon(UserID, CommonUserID);
            }, api_key);
        }
    }
}
