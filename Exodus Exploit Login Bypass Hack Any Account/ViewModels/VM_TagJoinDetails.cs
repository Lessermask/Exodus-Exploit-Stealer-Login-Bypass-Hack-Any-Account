using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Exodus.Enums;

namespace Exodus.ViewModels
{
    public class VM_TagJoinDetails
    {
        public long EventID { get; set; }
        public long UserID { get; set; }
        public long TagID { get; set; }
        public decimal IntentionAmount { get; set; }
        public En_Currency IntentionCurrencyID { get; set; }
        public EN_IntentionTerm IntentionTerm { get; set; } = EN_IntentionTerm.Indefinitely;
        public EN_Period Period { get; set; } = EN_Period.Once;
    }
}