using Exodus.API.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Exodus.API.Models
{
    public class API_Session
    {
        public API_Session(long UserID)
        {
            this.UserID = UserID;
        }

        public DateTime EndDate { get; set; } = DateTime.Now.AddHours(API_KeyHelper.LifeTimeDefault);
        public bool IsValid { get { return DateTime.Now.Ticks < EndDate.Ticks; } }
        public long UserID { get; set; }
    }
}