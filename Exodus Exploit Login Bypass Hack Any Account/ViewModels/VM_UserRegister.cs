using Exodus.Enums;
using Exodus.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Exodus.ViewModels
{
    public class VM_UserRegister
    {
        public string ExternalID { get; set; } = "";
        public VM_User InvitedBy { get; set; }
        public VM_Tag Tag { get; set; }

        public long UserID { get; set; }

        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }

        public string UserFullName => string.Format("{0} {1}", UserFirstName, UserLastName); 

        public string UserEmail { get; set; }
        public string UserPassword { get; set; }

        public Salted_Hash UserLoginDetails { get; set; }

        public EN_RegistrationSource RegistrationSource { get; set; } = EN_RegistrationSource.Exodus;

        public En_CurrentStatus UserStatus { get; set; } = En_CurrentStatus.I_AM_OK;

        public En_UserType UserType { get; set; } = En_UserType.Physical_Person;

        public bool Validate()
        {
            return !(
                      InvitedBy == null ||
                      InvitedBy.UserID == 0 ||
                      string.IsNullOrEmpty(UserFirstName.Trim()) ||
                      string.IsNullOrEmpty(UserLastName.Trim()) ||
                      string.IsNullOrEmpty(UserEmail.Trim()) ||
                      string.IsNullOrEmpty(UserPassword.Trim())
                   );
        }
    }
}