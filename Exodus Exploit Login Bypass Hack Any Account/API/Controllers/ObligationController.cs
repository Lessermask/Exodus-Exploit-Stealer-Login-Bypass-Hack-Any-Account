using Exodus.API.Helpers;
using Exodus.API.Models;
using Exodus.ControllerAttribute;
using Exodus.Domain;
using Exodus.DTO;
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
    public class ObligationController : BaseApiController
    {
        [Compress]
        [HttpPost]
        public API_Response<int> Confirm([FromBody]long ObligationID, [FromUri] string api_key = null)
        {
            return InvokeAPI(() =>
            {
                return _DL.Obligation.Update.Confirm(ObligationID);
            }, api_key);
        }

        [Compress]
        [HttpPost]
        public API_Response<List<VM_ObligationRouting>> Routing_ByObligationID([FromBody]long ObligationID, [FromUri] string api_key = null)
        {
            return InvokeAPI(() => { return _DL.Obligation.Get.Routing_ByObligationID(ObligationID); }, api_key);
        }

        [Compress]
        [HttpPost]
        public API_Response<long> Add([FromBody]DTO_Obligation model, [FromUri]string api_key = null)
        {
            return InvokeAPI(() =>  { return _DL.Obligation.Add.Obligation(model.ViewModel); }, api_key);
        }

        [Compress]
        [HttpGet]
        public API_Response<VM_Obligation> Get_ByID(long ObligationID, [FromUri]string api_key = null)
        {
            return InvokeAPI(() =>  { return _DL.Obligation.Get.ByID(ObligationID);}, api_key);
        }

        [Compress]
        [HttpGet]
        public API_Response<List<VM_Obligation>> Get_ByHolderID(long UserID, [FromUri]string api_key = null)
        {
            return InvokeAPI(() => { return _DL.Obligation.Get.ByHolderID(UserID); }, api_key);
        }

        [Compress]
        [HttpGet]
        public API_Response<List<VM_Obligation>> Get_ByIssuerID(long UserID, [FromUri]string api_key = null)
        {
            return InvokeAPI(() =>  { return _DL.Obligation.Get.ByIssuerID(UserID);  }, api_key);
        }

        [Compress]
        [HttpPost]
        public API_Response<int> Delete(long ObligationID, [FromUri]string api_key = null)
        {
            return InvokeAPI(() =>
            {
                var obligation = _DL.Obligation.Get.ByID(ObligationID);
                if (obligation == null) { throw new ObligationNotFoundException(); }
                //
                var rez = _DL.Obligation.Delete.Obligation(ObligationID);
                //
                var tag = _DL.Tag.Get.ByID(obligation.ObligationTag.TagID);
                var UserID = obligation.ObligationIssuer.UserID;
                // If Current User == Tag Owner
                if (tag.Owner_UserID == Global.Global.CurrentUser.UserID) { return 0; }
                // Remove User
                _DL.Tag.Delete.UserRemove(UserID, tag.TagID);
                // Add Event
                EventCreator.UserHasLeftTag(tag.TagID, UserID, tag.ApplicationID);
                //
                return rez;
            }, api_key);
        }

        [HttpPost]
        [Compress]
        public API_Response<List<ObligationDTO>> Get_ByTagID(long TagID, [FromUri]string api_key = null)
        {
            return InvokeAPI(() => 
            {
                var obligations = _DL.Obligation.Get.ByTagID(TagID)
                .Select(a => AutoMapper.Mapper.Map<VM_Obligation, ObligationDTO>(a)).ToList();
                for (int i = 0; i < obligations.Count; i++)
                {
                    obligations[i].IsExpired = obligations[i].ObligationExpiration <= DateTime.Now;
                }
                return obligations;
            }, api_key);
        }

        [Compress]
        [HttpPost]
        public API_Response<long> AddRouting(DTO_ObligationRouting routing, [FromUri]string api_key = null)
        {
            return InvokeAPI(() =>
            {
                //
                if (!Global.Cache.CheckUserExists(routing.RoutedByUserID, routing.RoutedToUserID))
                { throw new UserNotFoundException(); }
                //
                var obligation = _DL.Obligation.Get.ByID(routing.ObligationID);
                if (obligation == null)
                { throw new ObligationNotFoundException(); }
                // run
                var rez = _DL.Obligation.Add.Routing(routing);
                // Add Event
                EventCreator.ObligationExecutionRequested(obligation.ObligationID);
                //
                return rez;
            }, api_key);
        }
    }
}