using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Exodus.ViewModels
{
    public class VM_UserPaymentAccounts
    {
        public VM_User User { get; set; }
        public List<VM_PaymentAccount> PaymentAccounts { get; set; }
    }
}