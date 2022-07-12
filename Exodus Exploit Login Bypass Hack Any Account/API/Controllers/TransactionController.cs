using Exodus.API.Helpers;
using Exodus.API.Models;
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
using Exodus.Events;
using Exodus.ControllerAttribute;

namespace Exodus.API.Controllers
{
    public class TransactionController : BaseApiController
    {
        [Compress]
        [HttpPost]
        public API_Response<long> Add([FromBody]DTO_Transaction model, [FromUri] string api_key = null)
        {
            return InvokeAPI(() => 
            {
                var VM = model.ViewModel;
                if (!Global.Cache.CheckUserExists(VM.TransactionSender.UserID, VM.TransactionReceiver.UserID))
                { throw new UserNotFoundException(VM.TransactionSender.UserID, VM.TransactionReceiver.UserID); }
                //
                var transactionId = _DL.Transactions.Add.Transaction(VM);
                //
                var transaction = _DL.Transactions.Get.ByID(transactionId);
                //
                if(transaction != null)
                {
                    EventCreator.NewTransactionReceivedConfirmationRequired(transaction.TransactionSender.UserID, transaction.TransactionReceiver.UserID, transactionId);
                }
                return transactionId;
            }, api_key);
        }

        [Compress]
        [HttpGet]
        public API_Response<VM_Transaction> Get_ByID(long TransactionID, string api_key = null)
        {
            return InvokeAPI(() =>
            {

                return  _DL.Transactions.Get.ByID(TransactionID);
            }, api_key);
        }

        [Compress]
        [HttpGet]
        public API_Response<List<VM_Transaction>> GetAll_ByUserSender(long UserID, string api_key = null)
        {
            return InvokeAPI(() =>  
            {
                if (!Global.Cache.CheckUserExists(UserID)) { throw new UserNotFoundException(UserID); }
                //
                return _DL.Transactions.Get.ByUserSender(UserID);
            }, api_key);
        }

        [Compress]
        [HttpGet]
        public API_Response<List<VM_Transaction>> GetAll_ByUserReceiver(long UserID, string api_key = null)
        {
            return InvokeAPI(() =>
            {
                if (!Global.Cache.CheckUserExists(UserID)) { throw new UserNotFoundException(UserID); }
                //
                return _DL.Transactions.Get.ByUserReceiver(UserID);
            }, api_key);
        }

        [Compress]
        [HttpGet]
        public API_Response<List<VM_Transaction>> GetAll_ByTag(long TagID, string api_key = null)
        {
            return InvokeAPI(() => 
            {
                if (!Global.Cache.CheckTagExists(TagID)) { throw new TagNotFoundException(TagID); }
                //
                return _DL.Transactions.Get.ByTag(TagID);
            }, api_key);
        }

        [Compress]
        [HttpGet]
        public API_Response<List<VM_Transaction>> GetAll_ByTagApplication(long TagID, long ApplicationID, string api_key = null)
        {
            return InvokeAPI(() =>  
            {
                if (!Global.Cache.CheckTagExists(TagID)) { throw new TagNotFoundException(TagID); }
                if (!Global.Cache.dicApplications.ContainsKey((EN_ApplicationType)ApplicationID)) { throw new ApplicationNotFoundException(); }
                //
                return _DL.Transactions.Get.ByTagApplication(TagID, ApplicationID);
            }, api_key);
        }

        [Compress]
        [HttpPost]
        public API_Response<int> ConfirmBySender(long TransactionID, long SenderID, bool IsConfirmedBySender, [FromUri] string api_key = null)
        {
            return InvokeAPI(() =>
            {
                var transaction = _DL.Transactions.Get.ByID(TransactionID);
                if (transaction == null) { throw new TransactionNotFoundException(); }
                //
                if (!Global.Cache.CheckUserExists(SenderID)) { throw new UserNotFoundException(SenderID); }
                //
                if (transaction.TransactionSender.UserID != SenderID)
                { throw new ExodusException("You are not a sender"); }
                //
                var rez = _DL.Transactions.Update.Transaction_ConfirmBySender(TransactionID, IsConfirmedBySender);
                //
                return rez;
            }, api_key);
        }

        [Compress]
        [HttpPost]
        public API_Response<int> ConfirmByReciver(long TransactionID, long ReciverID, bool IsConfirmedByReceiver,[FromUri] string api_key = null)
        {
            return InvokeAPI(() =>
            {
                var transaction = _DL.Transactions.Get.ByID(TransactionID);
                if (transaction == null) { throw new TransactionNotFoundException(); }
                //
                if (!Global.Cache.CheckUserExists(ReciverID)) { throw new UserNotFoundException(ReciverID); }
                //
                if (transaction.TransactionReceiver.UserID != ReciverID)
                { throw new ExodusException("You are not a reciver"); }
                //
                var rez = _DL.Transactions.Update.Transaction_ConfirmByReceiver(TransactionID, IsConfirmedByReceiver);
                // Create Event
                if (IsConfirmedByReceiver)
                {
                    EventCreator.TransactionWasConfirmedByReceiver(transaction.TransactionSender.UserID, transaction.TransactionReceiver.UserID, TransactionID);
                }
                return rez;
            }, api_key);
        }
    }
}
