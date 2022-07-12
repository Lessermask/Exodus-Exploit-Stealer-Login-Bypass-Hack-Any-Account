using Exodus.Domain;
using Exodus.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Exodus.Security;
using static Exodus.Security.Security;
using System.Net.Http.Formatting;
using System.Collections.Specialized;
using Newtonsoft.Json;
using Exodus.ViewModels;
using Exodus.Service;
using Exodus.API.Helpers;
using Exodus.Exceptions;
using Exodus.DTO;
using Exodus.API.Models;
using Exodus.Models;
using Exodus.DTO_Api;
using Exodus.Helpers;
using Exodus.Events;
using Exodus.ControllerAttribute;

namespace Exodus.API.Controllers
{
    public class UserController : BaseApiController
    {
        [Compress]
        [HttpPost]
        public API_Response<VM_User> Login(DTO_UserLogin model)
        {
            return InvokeAPI(() => 
            {
                VM_User user = null;
                if (!Global.Cache.CheckEmailExists(model.UserName)) { throw new Exception("User Not Found"); }
                //
                var UserLoginDetails = _DL.User.Account.LoginDetails_ByEmail(model.UserName);
                // Check User Login Details
                if (ModelState.IsValid)
                {
                    // Check Password
                    if (HashHMACSHA1.CheckSaltedHash(model.UserPassword, UserLoginDetails.PasswordHash))
                    {
                        user = _DL.User.Get.ByID(UserLoginDetails.UserID); // Get User
                        user.ApiKey = API_KeyHelper.GenarateKey(user.UserID); // Generate Token
                    }
                    else { throw new Exception("Incorrect password"); }
                }
                else
                {
                    throw new Exception("Login Error");
                }
                return user;
            }, "", false);          
        }

        [Compress]
        [HttpGet]
        public API_Response<UserLoginDetails> GetLoginDetails_ByEmail(string UserEmail, string api_key = null)
        {
            return InvokeAPI(() => 
            {
                if (!Global.Cache.CheckEmailExists(UserEmail)) { throw new Exception("User Not Found"); }
                return _DL.User.Account.LoginDetails_ByEmail(UserEmail);
            }, api_key);
        }

        [Compress]
        [HttpGet]
        public API_Response<VM_User> Get_ByID(long UserID, string api_key = null)
        {
            return InvokeAPI(() => 
            {
                return _DL.User.Get.ByID(UserID);
            }, api_key);
        }

        [Compress]
        [HttpGet]
        public API_Response<VM_User> Get_ByEmail(string UserEmail, string api_key = null)
        {
            return InvokeAPI(() => 
            {
                if (!Global.Cache.CheckEmailExists(UserEmail)) { throw new Exception("User Not Found"); }
                return _DL.User.Get.ByEmail(UserEmail);
            }, api_key);
         }

        [Compress]
        [HttpGet]
        public API_Response<bool> CheckExistence_ByEmail(string UserEmail, string api_key = null)
        {
            return InvokeAPI(() =>
            {
                return _DL.User.Account.CheckExistence_ByEmail(UserEmail);
            }, api_key);
        }

        [Compress]
        [HttpPost]
        public API_Response<int> Update_UserStatus([FromBody]DTO_UserStatusUpdate model, [FromUri] string api_key = null)
        {
            return InvokeAPI(() => 
            {
                if (!Global.Cache.CheckUserExists(model.UserID)) { throw new UserNotFoundException(model.UserID); }
                //
                var rez = _DL.User.Account.Update.UserStatusUpdate(model.UserCurrentStatus, model.ViewModel);
                // Update User
                Global.Cache.SessionSetUser(SessionID, _DL.User.Get.ByID(model.UserID));
                // Create Event
                EventCreator.UserStatusChange(model.UserID, model.UserCurrentStatus);
                //
                return rez;
            }, api_key);
        }

        [Compress]
        [HttpPost]
        public API_Response<int> Delete_ByID([FromBody]long UserID, [FromUri] string api_key = null)
        {
            return InvokeAPI(() => 
            {
                return _DL.User.Account.Delete.ById(UserID);
            }, api_key);
        }

        [Compress]
        [HttpPost]
        public API_Response<int> Update_UserAbout([FromBody]long UserID, [FromBody]string UserAbout, [FromUri] string api_key = "")
        {
            return InvokeAPI(() => 
            {
                return _DL.User.Update.About(UserID, UserAbout);
            }, api_key);
        }

        [Compress]
        [HttpPost]
        public API_Response<VM_UserRegister> RegisterByMyself(DTO_RegisterByMyself model, [FromUri] string api_key = "")
        {
            return InvokeAPI(() => 
            {
                // Get Model
                if (_DL.User.Account.CheckExistence_ByEmail(model.Email))
                { throw new Exception(Global.Localisation.Get("UserAlreadyExist")); }
                else
                {
                    var rez =  _DL.User.Account.Register(model.ViewModel);
                    return rez;
                }
            }, api_key);
        }

        [Compress]
        [HttpPost]
        public API_Response<VM_UserRegister> RegisterByInviteCode(DTO_RegisterByInviteCode model, [FromUri] string api_key = "")
        {
            return InvokeAPI(() =>
            {
                if (_DL.User.Account.CheckExistence_ByEmail(model.Email))
                { throw new UserAlreadyExistException(); }
                else if (_DL.User.Get.ByGuid(model.InviteCode) == null)
                { throw new UserNotFoundException("Invited user not found"); }
                else
                {
                    var rez = _DL.User.Account.Register(model.ViewModel);
                    return rez;
                }
            }, api_key);
        }

        [Compress]
        [HttpPost]
        public API_Response<UserAvatar> UploadAvatar([FromUri]long UserID, [FromUri]string api_key = "")
        {
            return InvokeAPI(() =>
            {
                if (!Global.Cache.CheckUserExists(UserID)) { throw new UserNotFoundException(); }
                //
                var httpRequest = System.Web.HttpContext.Current.Request;
                if (httpRequest.Files.Count == 0) { throw new Exception("Image not found"); }
                else
                {
                    var file = httpRequest.Files[0];
                    switch (System.IO.Path.GetExtension(file.FileName).ToLower())
                    {
                        case ".jpg": case ".png": case ".jpeg": case ".gif":
                            return AvatarGenerator.Generate(file.InputStream, UserID);
                        default: throw new Exception("Incorrect format"); ;
                    }
                }
            }, api_key);
        }

        [Compress]
        [HttpGet]
        public API_Response<VM_User> Current([FromUri]string api_key = "")
        {
            return InvokeAPI(() => 
            {
                VM_User user = String.IsNullOrEmpty(SessionID) ? null : Global.Global.CurrentUser;
                return user;
            }, api_key);
        }

        [Compress]
        [HttpGet]
        public API_Response<bool> IsLogin([FromUri]string api_key = "")
        {
            return InvokeAPI(() =>
            {
                if (String.IsNullOrEmpty(SessionID))
                { return false; }
                else
                {
                    if (Global.Cache.SessionContains(SessionID))
                    { return Global.Cache.SessionGet(SessionID).User == null ? false : true; }
                    else
                    { return false; }
                }
            }, api_key);
        }

        [Compress]
        [HttpPost]
        public API_Response<string> ResetPassword(string email, [FromUri]string api_key = "")
        {
            return InvokeAPI(() =>
            {
                if (!Global.Cache.CheckEmailExists(email)) { throw new UserNotFoundException("User Not found"); }
                var user = _DL.User.Get.ByEmail(email);              
                //
                string token = Guid.NewGuid().ToString();
                string ExodusUrl = System.Configuration.ConfigurationManager.AppSettings.Get("ExodusUrl") + $"Home/TokenAction?token={token}";
                // add to DB
                _DL.GlobalDL.TokenActionAdd(token, DateTime.Now.AddDays(1), Exodus.Models.TokenAction.ChangePass, user.UserID);
                //
                string message = $"To change passworg go to this link {ExodusUrl}";
                // send email
                _SL.Mail.SendMessage(message,email);
                return token;
            }, api_key);
        }

        [Compress]
        [HttpPost]
        public API_Response<bool> ChangePassword(string token, string password, [FromUri]string api_key = "")
        {
            return InvokeAPI(() =>
            {
                // add to DB
                var _token = _DL.GlobalDL.TokenActionGet(token);
                // Errorss
                if (token == null) { throw new TokenActionNotFoundException(); }
                else if (_token.ExpirationDate.Ticks <= DateTime.Now.Ticks)
                { throw new TokenExpirationDateFaledException(); }
                // ChangePassword
                _DL.User.Update.Password(_token.UserID, password);
                //
                return true;
            }, api_key);
        }
    }
}