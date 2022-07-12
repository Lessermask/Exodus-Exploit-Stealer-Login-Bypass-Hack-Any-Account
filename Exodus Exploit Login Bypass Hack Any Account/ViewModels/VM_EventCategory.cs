using Exodus.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Exodus.ViewModels
{
    public class VM_EventCategory
    {
        public EN_EventCategory EventCategory => (EN_EventCategory)ID;
        public int ID { get; set; }
        public string NameEng { get; set; }
        public string NameRus { get; set; }
        public string Comment { get; set; }
    }
}