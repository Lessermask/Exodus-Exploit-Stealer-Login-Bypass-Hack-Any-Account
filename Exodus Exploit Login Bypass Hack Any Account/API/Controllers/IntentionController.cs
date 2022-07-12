using Exodus.API.Helpers;
using Exodus.API.Models;
using Exodus.ControllerAttribute;
using Exodus.Domain;
using Exodus.DTO_Api;
using Exodus.Enums;
using Exodus.Events;
using Exodus.Exceptions;
using Exodus.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;

namespace Exodus.API.Controllers
{
    public class IntentionController : BaseApiController
    {
        [Compress]
        [HttpPost]
        public API_Response<long> Add([FromBody]DTO_Intention model, [FromUri] string api_key = null)
        {
            return InvokeAPI(() =>
            {
                if (!Global.Cache.CheckTagExists(model.TagID)) { throw new TagNotFoundException(); }
                if (!Global.Cache.CheckUserExists(model.HolderUserID, model.IssuerUserID)) { throw new UserNotFoundException(); }
                // Validate Input data
                model.ValidateData(isNew: true);
                //
                var rez = _DL.Intention.Add.Intention(model.ViewModel);
                if (rez > 0 && _DL.Tag.Get.Role(model.TagID, model.IssuerUserID) == EN_TagRole.None)
                {
                    _DL.Tag.Add.AddUser(model.TagID, (int)EN_TagRole.Member, model.IssuerUserID);
                }
                // Total Summ
                var tag = _DL.Tag.Get.ByID(model.TagID);
                decimal totalSum = _DL.Intention.Get.Intention_Sum_ByTagID(tag.TagID) + _DL.Obligation.Get.Obligation_Sum_ByTagID(tag.TagID);
                // If total amount bigger then tag amount
                if (totalSum >= tag.TotalAmount && tag.ApplicationType == EN_ApplicationType.Own_Initiative)
                {
                    Dictionary<string, string> dic = new Dictionary<string, string>();
                    EN_EventType type = EN_EventType.Own_Initiative_Tag_Reached_Target_Amount_Including_Intentions;
                    // Add Keys
                    dic.Add("TagID", tag.TagID.ToString());
                    // Create new event
                    new EventModel(dic, type);
                }
                //
                return rez;
            }, api_key);
        }

        [Compress]
        [HttpGet]
        public API_Response<long> ToObligation(long IntentionID, [FromUri] string api_key = null)
        {
            return InvokeAPI(() =>
            {
                var intention = _DL.Intention.Get.ByID(IntentionID);
                //
                if (intention == null)
                { throw new IntentionNotFoundException(); }
                else
                {
                    var rez = _DL.Intention.To.Obligation(IntentionID);
                    //
                    var obligation = _DL.Obligation.Get.ByID(rez);
                    if ((intention.Period == EN_Period.Undefined || intention.Period == EN_Period.Once) && rez > 0)
                    { _DL.Intention.Delete.ByID(IntentionID); }
                    //
                    var tag = _DL.Tag.Get.ByID(obligation.ObligationTag.TagID);
                    decimal totalSum =  _DL.Obligation.Get.Obligation_Sum_ByTagID(tag.TagID);
                    // If total amount bigger then tag amount
                    if (totalSum >= tag.TotalAmount && tag.ApplicationType == EN_ApplicationType.Own_Initiative)
                    {
                        Dictionary<string, string> dic = new Dictionary<string, string>();
                        EN_EventType type = EN_EventType.Own_Initiative_Tag_Reached_Target_Amount;
                        // Add Keys
                        dic.Add("TagID", tag.TagID.ToString());
                        // Create new event
                        new EventModel(dic, type);
                    }
                    //
                    return rez;
                }
            }, api_key);
        }

        [Compress]
        [HttpGet]
        public API_Response<VM_Intention> Get_ByID(long IntentionID, [FromUri] string api_key = null)
        {
            return InvokeAPI(() => 
            {
                var rez = _DL.Intention.Get.ByID(IntentionID);
                return rez == null ? throw new IntentionNotFoundException() : rez;
            }, api_key);
        }

        [Compress]
        [HttpGet]
        public API_Response<List<VM_Intention>> Get_ByTagID(long TagID, [FromUri] string api_key = null)
        {
            return InvokeAPI(() =>
            {
                if (!Global.Cache.CheckTagExists(TagID)) { throw new TagNotFoundException(); }
                var rez = _DL.Intention.Get.ByTagID(TagID);
                // If Null Intention Not Found
                return rez == null ? throw new IntentionNotFoundException() : rez;
            }, api_key);
        }

        [Compress]
        [HttpGet]
        public API_Response<List<VM_Intention>> ByUserIssuerID_ByTagID(long TagID, long UserID, [FromUri] string api_key = null)
        {
            return InvokeAPI(() =>
            {
                if (!Global.Cache.CheckUserExists(UserID)) { throw new UserNotFoundException(); }
                if (!Global.Cache.CheckTagExists(TagID)) { throw new TagNotFoundException(); }
                //
                var rez = _DL.Intention.Get.ByUserIssuerID_ByTagID(TagID, UserID);
                return rez == null ? throw new IntentionNotFoundException() : rez;
            }, api_key);
        }

        [Compress]
        [HttpGet]
        public API_Response<List<VM_Intention>> Get_ByHolderID(long UserID, [FromUri] string api_key = null)
        {
            return InvokeAPI(() =>
            {
                if (!Global.Cache.CheckUserExists(UserID)) { throw new UserNotFoundException(); }
                var rez = _DL.Intention.Get.ByHolderID(UserID);
                return rez;
            }, api_key);
        }

        [Compress]
        [HttpGet]
        public API_Response<List<VM_Intention>> Get_ByIssuerID(long UserID, [FromUri] string api_key = null)
        {
            return InvokeAPI(() => 
            {
                if (!Global.Cache.CheckUserExists(UserID)) { throw new UserNotFoundException(); }
                var rez =_DL.Intention.Get.ByIssuerID(UserID);
                return rez;
            }, api_key);
        }

        [Compress]
        [HttpGet]
        public API_Response<List<VM_Intention>> Get_ByUserPublicProfile(long HolderUserID, long IssuerUserID, [FromUri] string api_key = null)
        {
            return InvokeAPI(() => 
            {
                if (!Global.Cache.CheckUserExists(HolderUserID, IssuerUserID)) { throw new UserNotFoundException(); }
                //
                return _DL.Intention.Get.ByUserPublicProfile(HolderUserID, IssuerUserID);          
            }, api_key);
        }

        [Compress]
        [HttpPost]
        public API_Response<long> Update([FromBody]DTO_Intention model, [FromUri] string api_key = null)
        {
            return InvokeAPI(() => 
            {
                model.ValidateData();
                //
                return _DL.Intention.Update.Intention(model.ViewModel);
            }, api_key);
        }

        [Compress]
        [HttpPost]
        public API_Response<long> Delete([FromUri]long IntentionID, [FromUri] string api_key = null)
        {
            return InvokeAPI(() =>
            {
                var intention = _DL.Intention.Get.ByID(IntentionID);
                if (intention == null) { throw new IntentionNotFoundException(); }
                var rezInt = _DL.Intention.Delete.ByID(IntentionID);
                //
                if (intention.IntentionTag == null) { return 0; }
                //
                var tag = _DL.Tag.Get.ByID(intention.IntentionTag.TagID);
                var UserID = intention.IntentionIssuer.UserID;
                // If Current User == Tag Owner
                if (tag.Owner_UserID == Global.Global.CurrentUser.UserID) { return 0; }
                // Remove User
                _DL.Tag.Delete.UserRemove(UserID, tag.TagID);
                // Add Event
                EventCreator.UserHasLeftTag(tag.TagID, UserID, tag.ApplicationID);
                //
                return rezInt;
            }, api_key);
        }
    }
}