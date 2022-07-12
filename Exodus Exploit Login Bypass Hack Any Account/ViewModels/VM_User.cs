using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Exodus.Enums;
using Exodus.Domain;
using System.Web.Mvc;
using static Exodus.Service._SL;
using Exodus.Service;
using System.IO;
using Exodus.Controllers;
using Exodus.Security;
using Exodus.Helpers;
using Newtonsoft.Json;
using Exodus.Models;
using Exodus.DTO_Api;

namespace Exodus.ViewModels
{
    public class VM_User
    {
        [JsonIgnore]
        public static VM_User EmptyUser
        {
            get { return new VM_User(); }
        }

        public VM_UserHelpDetail HelpDetail { get; set; } = new VM_UserHelpDetail();
        [JsonProperty("InvitedBy")]
        public VM_User UserInvitedBy { get; set; }
        public long UserID { get; set; } = -1;
        public string UserExternalID { get; set; } = "";
        public string UserGuid { get; set; } = null;
        public string UserFirstName { get; set; } = "";
        public string UserLastName { get; set; } = "";
        public string UserEmail { get; set; } = null;
        public En_UserType UserType { get; set; }
        public En_CurrentStatus UserStatus { get; set; }
        public string UserStatusMessage { get; set; } = "";
        public string UserAbout { get; set; } = "";
        [JsonIgnore]
        public DateTime UserRegistered { get; set; }
        public EN_RegistrationSource UserRegistrationSource { get; set; }
        public string ApiKey { get; set; } = null;
        [JsonIgnore]
        public UserAvatar Avatar { get; set; } = null;
        public string UserFullName => $"{UserFirstName} {UserLastName}";
        public string AvatarBig => Avatar?.AvatarBig;
        public string AvatarSmall => Avatar?.AvatarSmall;
        [JsonIgnore]
        public List<VM_PaymentAccount> Accounts { get; set; }
        [JsonIgnore]
        public List<Contact> Contacts { get; set; }
        [JsonIgnore]
        public DTO_User_LightModel LightModel
        {
            get
            {
                return new DTO_User_LightModel()
                {
                    Avatar = Avatar,
                    HelpDetails = HelpDetail,
                    UserFirstName = UserFirstName,
                    UserID = UserID,
                    UserLastName = UserLastName,
                    UserStatus = UserStatus
                };
            }
        }
    }
}