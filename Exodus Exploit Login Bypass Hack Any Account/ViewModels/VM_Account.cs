using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Exodus.ViewModels
{
    public class VM_Account
    {
        public long AccountID { get; set; }
        public int fk_AccountTypeID { get; set; }
        public long fk_UserID { get; set; }
        public string AccountDetails { get; set; }
    }
}