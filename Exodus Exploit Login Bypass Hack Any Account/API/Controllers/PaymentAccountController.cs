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
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;

namespace Exodus.API.Controllers
{
    public class PaymentAccountController : BaseApiController
    {
        [Compress]
        [HttpGet]
        public API_Response<List<VM_AccountType>> Get_AccountTypes(string api_key = null)
        {
            return InvokeAPI(() => { return _DL.PaymentAccount.Get.Types(); }, api_key);
        }

        [Compress]
        [HttpGet]
        public API_Response<VM_UserPaymentAccounts> Get_List(long UserID, string api_key = null)
        {
            return InvokeAPI(() => { return _DL.PaymentAccount.Get.UserPaymentAccounts(UserID); }, api_key);
        }

        [Compress]
        [HttpPost]
        public API_Response<int> Update([FromBody]DTO_PaymentAccount model, [FromUri] string api_key = null)
        {
            return InvokeAPI(() =>
            {
                var rez = _DL.PaymentAccount.Update.UpdateAccount(model.ViewModel);
                return rez;
            }, api_key);
        }

        [Compress]
        [HttpPost]
        public API_Response<int> Delete(long AccountID, string api_key = null)
        {
            return InvokeAPI(() =>
            {
                var rez = _DL.PaymentAccount.Delete.DeleteAccount(AccountID);
                return rez;
            }, api_key);         
        }

        [Compress]
        [HttpPost]
        public API_Response<long> Add([FromBody]DTO_PaymentAccount model, [FromUri] string api_key = null)
        {
            return InvokeAPI(() =>
            {
                switch (model.AccountType)
                {
                    case En_AccountType.Bitcoin:  case En_AccountType.PayPal:  case En_AccountType.WebMoney:
                        return _DL.PaymentAccount.Add.New(model.ViewModel);
                    default: throw new Exception("Incorrect PaymentAccountType");
                }
            }, api_key );
        }

        [Compress]
        [HttpGet]
        public API_Response<VM_PaymentAccount> Get_ByID(int AccountID, string api_key = null)
        {
            return InvokeAPI(() =>
            {
                var rez =  _DL.PaymentAccount.Get.ByID(AccountID);
                return rez;
            }, api_key);
        }
    }
}