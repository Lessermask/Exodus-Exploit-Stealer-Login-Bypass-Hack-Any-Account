using Exodus.Enums;
using System;

namespace Exodus.ViewModels
{
    public class VM_Obligation
    {
        public long IntentionID { get; set; } // TODO
        public long ObligationID { get; set; }
        public EN_ObligationKind Kind
        {
            get { return ObligationKind == null ? EN_ObligationKind.H2OUserHelp : ObligationKind.ObligationKindID; }
        }
        public EN_ObligationType Type
        {
            get { return ObligationType == null ? EN_ObligationType.Money : ObligationType.ObligationTypeID; }
        }
        public VM_ObligationKind ObligationKind { get; set; }
        public VM_ObligationType ObligationType { get; set; }
        public VM_User ObligationIssuer { get; set; }
        public VM_User ObligationHolder { get; set; }
        public En_Currency ObligationCurrency { get; set; }
        public decimal AmountPerPayment { get; set; }
        public decimal AmountDue { get; set; }
        public decimal AmountTotal { get; set; }
        public bool IsActive { get; set; }
        public EN_Period ObligationPeriod { get; set; }
        public DateTime ObligationDate { get; set; }
        public DateTime ObligationExpiration { get; set; }
        public VM_ObligationClass ObligationClass { get; set; }
        public VM_ObligationStatus ObligationStatus { get; set; }
        public VM_Tag ObligationTag { get; set; }
        public VM_Application ObligationApplication { get; set; }
    }
}