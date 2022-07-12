using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Exodus.Enums;
using Exodus.Domain;

namespace Exodus.ViewModels
{
    public class VM_BankCardType
    {
        public EN_CardType CardTypeID { get; set; }
        public string CardTypeName { get; set; }
    }
}