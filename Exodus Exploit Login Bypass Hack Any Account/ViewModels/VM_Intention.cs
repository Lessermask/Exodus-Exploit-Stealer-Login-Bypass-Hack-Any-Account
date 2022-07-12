using Exodus.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Exodus.ViewModels
{
    public class VM_Intention
    {
        public long IntentionID { get; set; }
        public EN_ObligationKind Kind
        {
            get { return ObligationKind == null ? EN_ObligationKind.H2OUserHelp : ObligationKind.ObligationKindID; }
        }
        public EN_ObligationType Type
        {
            get { return ObligationType == null ? EN_ObligationType.Money : ObligationType.ObligationTypeID; }
        }
        public VM_ObligationType ObligationType { get; set; }
        public VM_ObligationKind ObligationKind { get; set; }
        public EN_Period Period { get; set; }
        public EN_IntentionTerm IntentionTerm { get; set; }
        public VM_User IntentionIssuer { get; set; }
        public VM_User IntentionHolder { get; set; }
        public VM_Tag IntentionTag { get; set; }
        public VM_Application IntentionApplication { get; set; }
        public decimal IntentionAmount { get; set; }
        public En_Currency CurrencyID { get; set; }
        public int IntentionDurationMonths { get; set; }
        public DateTime IntentionStartDate { get; set; }
        public DateTime IntentionEndDate { get; set; }
        public bool IntentionIsActive { get; set; }
        public int IntentionDayOfWeek { get; set; }
        public int IntentionDayOfMonth { get; set; }
        public string IntentionMemo { get; set; }

        public int RemainDays
        {
            get
            {
                if (IntentionEndDate.Ticks < DateTime.Now.Ticks) { return 0; }
                return (int)IntentionEndDate.Subtract(DateTime.Today).TotalDays;
            }
        }
        public int RemainMonths
        {
            get
            {
                if (IntentionEndDate.Ticks < DateTime.Now.Ticks) { return 0; }
                return RemainYears * 12 + (IntentionEndDate.Month - DateTime.Now.Month);
            }
        }
        public int RemainYears
        {
            get
            {
                if (IntentionEndDate.Ticks < DateTime.Now.Ticks) { return 0; }
                return IntentionEndDate.Year - DateTime.Now.Year;
            }
        }
    }
}