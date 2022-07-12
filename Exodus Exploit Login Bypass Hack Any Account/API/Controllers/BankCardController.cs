using Exodus.API.Helpers;
using Exodus.API.Models;
using Exodus.ControllerAttribute;
using Exodus.Domain;
using Exodus.DTO_Api;
using Exodus.Enums;
using Exodus.Exceptions;
using Exodus.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web.Http;

namespace Exodus.API.Controllers
{
    public class BankCardController : BaseApiController
    {
        [HttpGet]
        [Compress]
        public API_Response<List<VM_BankCard>> GetAll_ByUserID(long UserID, [FromUri]string api_key = null)
        {
            return InvokeAPI(() => 
            {
                if (!Global.Cache.CheckUserExists(UserID)) { throw new UserNotFoundException(); }
                //
                var rez = _DL.Cards.Get.ByUserID(UserID);
                //
                return rez ?? throw new CardNotFoundException(); 
            }, api_key);
        }

        [HttpGet]
        [Compress]
        public API_Response<VM_BankCard> Get_ByID(int CardID, [FromUri]string api_key = null)
        {
            return InvokeAPI(() => { return _DL.Cards.Get.ByID(CardID) ?? throw new CardNotFoundException(); }, api_key);
        }

        [HttpPost]
        [Compress]
        public API_Response<long> Add([FromBody]DTO_CreditCard model, [FromUri]string api_key = null)
        {
            return InvokeAPI(() => 
            {              
                model.ValidateData(); // validation      
                return _DL.Cards.Add.AddCard(model.ViewModel);
            }, api_key);          
        }

        [HttpPost]
        [Compress]
        public API_Response<int> Update([FromBody]DTO_CreditCard model, [FromUri]string api_key = null)
        {
            return InvokeAPI(() => 
            {
                model.ValidateData();  // validation
                return _DL.Cards.Update.UpdateCard(model.ViewModel);
            }, api_key);
        }

        [HttpPost]
        [Compress]
        public API_Response<int> Delete([FromBody]long CardID, [FromUri]string api_key = null)
        {
            return InvokeAPI(() => 
            {
                return _DL.Cards.Delete.DeleteCard(CardID);
            }, api_key);
        }

        [HttpGet]
        [Compress]
        public API_Response<List<VM_Bank>> Get_BankNames([FromUri]string api_key = null)
        {
            return InvokeAPI(() =>  { return Global.Cache.dicBanks.Values.ToList(); }, api_key);
        }

        [HttpGet]
        [Compress]
        public API_Response<VM_Bank> Get_BankNameByCardNumber(string cardNumber, [FromUri]string api_key = null)
        {
            return InvokeAPI(() =>
            {
                cardNumber = (cardNumber.Length > 6) ? cardNumber.Substring(0, 6) + "%" : cardNumber + "%";
                return _DL.Banks.Get.BankNamesByCardNumber(cardNumber).FirstOrDefault();
            }, api_key);
        }

        [HttpGet]
        [Compress]
        public API_Response<List<VM_BankCardType>> Get_CardTypes([FromUri]string api_key = null)
        {
            return InvokeAPI(() => { return _DL.Cards.Get.CreditCardType(); }, api_key);
        }
    }
}