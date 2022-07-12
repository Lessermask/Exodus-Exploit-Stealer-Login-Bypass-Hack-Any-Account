using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Exodus.Enums;
using Exodus.Extensions;
using Exodus.Interfaces;
using Newtonsoft.Json;

namespace Exodus.ViewModels
{
    public class VM_UserHelpDetail : IDictionarySerialozation
    {
        public long HelpDetailID { get; set; }  =-1;
        [JsonIgnore]
        public long UserID { get; set; } = -1;
        public En_HelpPeriods UserHelpPeriod { get; set; } = En_HelpPeriods.Undefined;
        public En_Currency UserHelpAmountCurrency { get; set; } = En_Currency.USD;
        public decimal UserHelpAmountRequired { get; set; } = 0;
        public string UserHelpDetails { get; set; }
        public DateTime UpdatedDateTime { get; set; } = DateTime.Now;
        public DateTime DealLine { get; set; } = DateTime.Now;
        public Dictionary<string, string> DictionarySerialozation
        {
            get
            {
                Dictionary<string, string> dic = new Dictionary<string, string>();
                //
                dic.Add("HelpDetailID", HelpDetailID.ToString());
                dic.Add("UserID", UserID.ToString());
                dic.Add("UserHelpPeriod", UserHelpPeriod.ToInt().ToString());
                dic.Add("UserHelpAmountCurrency", UserHelpAmountCurrency.ToInt().ToString());
                dic.Add("UserHelpAmountRequired", UserHelpAmountRequired.ToString("F2").Replace(".", ","));
                dic.Add("UserHelpDetails", UserHelpDetails);
                dic.Add("UpdatedDateTime", UpdatedDateTime.ToString());
                dic.Add("DealLine", DealLine.ToString());
                //
                return dic;
            }
        }
    }
}