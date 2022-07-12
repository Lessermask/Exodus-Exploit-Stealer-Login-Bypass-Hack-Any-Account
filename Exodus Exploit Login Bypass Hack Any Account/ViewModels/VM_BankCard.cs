using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Exodus.Enums;

namespace Exodus.ViewModels
{
    public class VM_BankCard
    {
        public long CardID { get; set; } 
        public long UserID { get; set; }
        public EN_CardType TypeID { get; set; }
        public long BankID { get; set; }
        public string BankName { get; set; }
        public string CardNumber { get; set; }
        public DateTime CardValidTill { get; set; }
        public string AdditionalInfo { get; set; } // XML
    }
}