using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Exodus.Enums;

namespace Exodus.ViewModels
{
    public class VM_StatusInfo
    {
        public long HelpDetailID { get; set; }
        public long UserID { get; set; }
        public En_CurrentStatus Status { get; set; }
        public string User_StatusName { get; set; }
        public string User_StatusComment { get; set; }
        public string User_StatusMessage { get; set; }
        public En_Currency Currency { get; set; }
        public En_HelpPeriods HelpPeriod { get; set; }
        public string HelpDetails { get; set; }
        public decimal AmountRequired { get; set; }
        public DateTime UpdateDateTime { get; set; }
    }
}