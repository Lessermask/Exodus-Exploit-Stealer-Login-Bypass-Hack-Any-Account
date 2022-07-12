using Exodus.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Exodus.ViewModels
{
    public class VM_EventType
    {
        public long ID { get; set; }
        public EN_EventType Type => (EN_EventType)ID;
        public EN_EventCategory Category => EventCategory.EventCategory;
        public VM_EventCategory EventCategory { get; set; }
        public EN_ImportantLevel ImportantLevel { get; set; }
        public string NameEng { get; set; }
        public string NameRus { get; set; }
        public string Comment { get; set; }
    }
}