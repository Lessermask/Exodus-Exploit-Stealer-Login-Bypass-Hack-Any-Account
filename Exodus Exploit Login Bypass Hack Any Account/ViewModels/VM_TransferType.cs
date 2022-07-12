using Exodus.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Exodus.ViewModels
{
    public class VM_TransferType
    {
        public string NameEng { get; set; }

        public string NameRus { get; set; }

        public EN_TransferType Type { get; set; }
    }
}