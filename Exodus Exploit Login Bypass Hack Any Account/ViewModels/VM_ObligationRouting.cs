using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Exodus.Enums;
using Newtonsoft.Json;

namespace Exodus.ViewModels
{
    public class VM_ObligationRouting
    {
        public long RoutingID { get; set; }
        public long ObligationID { get; set; }
        public VM_ObligationRoutingType RoutingType { get; set; }
        public VM_User RoutedByUser { get; set; }
        public VM_User RoutedToUser { get; set; }
        public VM_User TransferUser { get; set; }
        public VM_TransferType TransferType { get; set; }
        public VM_AccountType AccountType { get; set; }
        public VM_Account Account { get; set; }
        public string TransferUserCustomDetails { get; set; }
        public string AccountCustomDetails { get; set; }
        public decimal TransferAmount { get; set; }
        public EN_TransferType TransferAmountCurrency { get; set; }
        public DateTime DesiredEndDate { get; set; }
        public DateTime RoutingDT { get; set; }
        public bool isExecuted { get; set; }
        public VM_BankCard BankCard { get; set; } = new VM_BankCard();
    }
}