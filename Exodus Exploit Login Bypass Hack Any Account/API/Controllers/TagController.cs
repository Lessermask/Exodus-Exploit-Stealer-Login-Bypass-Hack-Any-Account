using Exodus.API.Helpers;
using Exodus.API.Models;
using Exodus.ControllerAttribute;
using Exodus.Domain;
using Exodus.DTO;
using Exodus.DTO_Api;
using Exodus.Enums;
using Exodus.Events;
using Exodus.Exceptions;
using Exodus.Models;
using Exodus.Models.Search;
using Exodus.Service;
using Exodus.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Http;
using System.Xml;

namespace Exodus.API.Controllers
{
    public class TagController : BaseApiController
    {
        [Compress]
        [HttpPost]
        public API_Response<long> Join([FromBody]DTO_TagJoinDetails model, [FromUri]string api_key = null)
        {
            return InvokeAPI(() =>
            {
                VM_Tag tag;
                // Get all objects
                if (!Global.Cache.CheckUserExists(model.InvitedUserID, model.InviterUserID)) { throw new UserNotFoundException(); }
                if (!Global.Cache.CheckTagExistsAndGet(model.TagID, out tag)) { throw new TagNotFoundException(); }
                // CHeck
                if (tag.Owner_UserID == model.InvitedUserID)   { throw new   Exception("You can not add your self"); }
                // If Exists
                if (_DL.Tag.Get.ByUserID(model.InvitedUserID).Where(a => a.TagID == model.TagID).Count() != 0)
                { throw new Exception("You have already in tag"); }
                //
                long rezAdd = _DL.Tag.Add.AddUserAsMember(model.TagID, model.InvitedUserID);
                // 
                long rezIntention = _DL.Intention.Add.Intention(model.IntentionModel);
                //
                decimal totalSum = _DL.Intention.Get.Intention_Sum_ByTagID(tag.TagID);
                // EVENTS
                // Tag Reached
                if (totalSum >= tag.TotalAmount && tag.ApplicationType == EN_ApplicationType.Own_Initiative)
                { EventCreator.OwnInitiativeTagReachedTargetAmountIncludingIntentions(tag.TagID); }
                // Join
                if (model.InviterUserID != 0)
                { EventCreator.UserHasJoinedTagUponYourInvitation(tag.TagID, model.InvitedUserID, model.InviterUserID, tag.ApplicationID);  }
                return rezAdd;
            }, api_key);
        }

        [Compress]
        [HttpPost]
        public API_Response<long> Add([FromBody]DTO_Tag model, [FromUri] string api_key = null)
        {
            return InvokeAPI( () => 
            {
                if (model.ApplicationType == EN_ApplicationType.Own_Initiative && model.TotalAmount < model.MinIntentionAmount)
                { throw new TagException("Total Amount Less Then MinIntention"); }
                //
                var rez = _DL.Tag.Add.AddTag(model.ViewModel);
                return rez;
            }, api_key);
        }

        [Compress]
        [HttpGet]
        public API_Response<VM_Tag> Get_ByID(long TagID, [FromUri]string api_key = null)
        {
            return InvokeAPI(() => { return _DL.Tag.Get.ByID(TagID); }, api_key);
        }

        [Compress]
        [HttpGet]
        public API_Response<List<TagSearch>> ByUserID(long UserID, [FromUri]string api_key = null)
        {
            return InvokeAPI(() =>   {  return _DL.Tag.Get.SearchByUserID(UserID);  }, api_key);
        }

        [Compress]
        [HttpGet]
        public API_Response<List<VM_Tag>> GetAll_OwnedByUserID(long UserID, [FromUri]string api_key = null)
        {
            return InvokeAPI(() =>  { return _DL.Tag.Get.OwnedByUserID(UserID); }, api_key);
        }

        [Compress]
        [HttpGet]
        public API_Response<List<VM_TagUser>> GetUsers(long TagID, [FromUri]string api_key = null)
        {
            return InvokeAPI(() => {  return _DL.Tag.Get.UsersByTag(TagID); }, api_key);
        }

        [Compress]
        [HttpGet]
        public API_Response<List<VM_TagUser>> GetUsers_With_Statuses(long TagID, [FromUri]string api_key = null)
        {
            return InvokeAPI(() => { return _DL.Tag.Get.UserByTag_WithStatus(TagID);}, api_key);
        }

        [Compress]
        [HttpGet]
        public API_Response<long> InviteUser(long TagID, long InvitedUserID, long InviterUserID, [FromUri]string api_key = null)
        {
            return InvokeAPI(() => 
            {
                if (!Global.Cache.CheckTagExists(TagID)) { throw new TagNotFoundException(); }
                if (!Global.Cache.CheckUserExists(InvitedUserID, InviterUserID)) { throw new UserNotFoundException(); }
                //
                var tag = _DL.Tag.Get.ByID(TagID);
                //
                return EventCreator.TagJoinInvitation(TagID, InvitedUserID, InviterUserID, tag.ApplicationID);
            }, api_key);
        }

        [Compress]
        [HttpGet]
        public API_Response<EN_TagRole> TagRole(long TagID, long UserID, [FromUri]string api_key = null)
        {
            return InvokeAPI(() =>
            {
                if (!Global.Cache.CheckTagExists(TagID)) { throw new TagNotFoundException(); }
                if (!Global.Cache.CheckUserExists(UserID)) { throw new UserNotFoundException(); }
                //
                return _DL.Tag.Get.Role(TagID, UserID);
            }, api_key);
        }

        [Compress]
        [HttpPost]
        public API_Response<List<VM_TagPublicWinUserCount>> TagRole([FromUri]string api_key = null)
        {
            return InvokeAPI(() => {  return _DL.Tag.Get.TagPublicWinUserCount(); }, api_key);
        }

    }
}