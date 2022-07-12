using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Exodus.Enums;

namespace Exodus.ViewModels
{
    public class VM_ImportanLevel
    {
        public int ID { get; set; }
        public EN_ImportantLevel ImportantLevel => (EN_ImportantLevel)ID; 
        public string NameRus { get; set; }
        public string NameEng { get; set; }
        public string Comment { get; set; }
    }
}