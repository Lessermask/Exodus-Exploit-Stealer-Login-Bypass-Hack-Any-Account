using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Exodus.Domain;
using Exodus.Enums;

namespace Exodus.ViewModels
{
    public class VM_Transaction
    {
        public long TransactionID { get; set; }
        public decimal TransactionAmount { get; set; }
        public En_Currency TransactionCurrency { get; set; }
        public EN_TransferType TransferType { get; set; }
        public VM_User TransactionSender { get; set; }
        public VM_User TransactionReceiver { get; set; }
        public VM_Tag Tag { get; set; }
        public VM_Application Application { get; set; }
        public VM_Obligation Obligation { get; set; }
        public VM_Account TransactionAccount { get; set; }
        public bool IsConfirmed { get; set; }
        public bool IsConfirmedBySender { get; set; }
        public bool IsConfirmedByReceiver { get; set; }
        public DateTime TransactionDateTime { get; set; }
        public string TransactionMemo { get; set; }
        public VM_PaymentAccount PaymentAccount { get; set; }

    }
}