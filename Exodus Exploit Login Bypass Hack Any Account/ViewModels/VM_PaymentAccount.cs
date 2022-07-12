using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Exodus.Enums;

namespace Exodus.ViewModels
{
    public class VM_PaymentAccount
    {
        public long AccountID { get; set; }
        public En_AccountType AccountType { get; set; }
        public string AccountDetails { get; set; }
        public string AccountTypeName { get; set; }
        public VM_User User { get; set; }
        public VM_BankCard Card { get; set; }
    }
}