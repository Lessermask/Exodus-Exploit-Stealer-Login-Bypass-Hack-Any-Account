using Exodus.API.Models;
using Exodus.Domain;
using Exodus.Enums;
using Exodus.Helpers;
using Exodus.Events;
using Exodus.Exceptions;
using Exodus.Extensions;
using Exodus.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Web.Http;
using System.Xml;
using Exodus.ControllerAttribute;

namespace Exodus.API.Controllers
{
    public class EventsController : BaseApiController
    {
        [HttpPost]
        [Compress]
        public API_Response<List<Dictionary<string, object>>> GetEventList(long UserID, [FromUri]string api_key = null)
        {
            return InvokeAPI(() =>
            {
                if (!Global.Cache.CheckUserExists(UserID)) { throw new UserNotFoundException(); }
                // Sort
                var events = _DL.Events.Get.EventList_ByUserID(UserID);
                // Only Light Json
                return events.Select(a => a.LightJson).ToList();
            }, api_key);
        }

        [HttpPost]
        [Compress]
        public API_Response<List<VM_Event>> EventList_Read_ByUserID(long UserID, [FromUri]string api_key = null)
        {
            return InvokeAPI(() =>
            {
                var _events = _DL.Events.Get.EventList_Read_ByUserID(UserID);
                //
                return _events;
            }, api_key);
        }

        [Compress]
        [HttpPost]
        public API_Response<long> Add(Dictionary<string,string> dic, [FromUri]EN_EventType type, [FromUri]string api_key = null)
        {
            return InvokeAPI(() => 
            {
                var _event = new EventModel(dic, type);
                //
                return _event.EventID;
            }, api_key);
        }

        [Compress]
        [HttpPost]
        public API_Response<string> UpdateEventsData([FromUri]string api_key = null)
        {
            return InvokeAPI(() => 
            {
                var list = _DL.Events.Get.ListID();
                long updated = 0, currentEventID = 0, totalCount = list.Count;    //
                foreach (var eventID in list)
                {
                    try
                    { 
                        currentEventID = eventID;
                        var _eventModel = EventModel.LoadEventByID(EventID: eventID);
                        //
                        var rez = _DL.Events.Update.TagID_AppID_XML(eventID, 
                            _eventModel.Thumbnail.OuterXml,
                            _eventModel.Content.OuterXml,
                            _eventModel.Event.Tag?.TagID,
                            _eventModel.Event.Application?.ApplicationID);
                        //
                        if (rez > 0) { updated++; }
                    }
                    catch (Exception ex)
                    {
                        Log4Net.Logger.Logger4Net.Error($"EVENT UPDATE ERROR EVENT: {currentEventID}\r\n{ex.Message}", ex);
                    }
                }
                return $"UPDATE {updated} FROM {totalCount}";
            }, api_key);
        }

        [Compress]
        [HttpPost]
        public API_Response<VM_Event> GetByID(long EventID, [FromUri]string api_key = null)
        {
            return InvokeAPI(() =>
            {
                var _event = _DL.Events.Get.ByID(EventID);
                return _event;
            }, api_key);
        }

        [Compress]
        [HttpPost]
        public API_Response<long> DeleteByID(long EventID, [FromUri]string api_key = null)
        {
            return InvokeAPI(() =>
            {
                var rez = _DL.Events.Delete.ByID(EventID);
                return rez;
            }, api_key);
        }

        [HttpPost]
        [Compress]
        public API_Response<int> MarkETU_Processed(long UserID, long EventID, [FromUri]string api_key = null)
        {
            return InvokeAPI(() =>
            {
                var rez = _DL.Events.Update.MarkETU_Processed(UserID, EventID);
                return rez;
            }, api_key);
        }
    }
}