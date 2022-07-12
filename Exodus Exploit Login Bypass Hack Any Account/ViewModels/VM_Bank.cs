using Exodus.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Exodus.ViewModels
{
    public class VM_Bank
    {
        public static VM_Bank EmptyBank { get { return new VM_Bank { BankID = 0, BankName = "" }; } }

        public long BankID { get; set; }

        public string BankName { get; set; }
    }
}