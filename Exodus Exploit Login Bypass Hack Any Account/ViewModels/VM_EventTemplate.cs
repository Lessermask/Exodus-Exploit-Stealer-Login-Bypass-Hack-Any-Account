using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

namespace Exodus.ViewModels
{
    public class VM_EventTemplate
    {
        public long ID { get; set; }
        public VM_EventType eventType { get; set; }
        public string TitleRus { get; set; }
        public string TitleEng { get; set; }
        public XmlDocument Content { get; set; } = new XmlDocument();
        public XmlDocument Thumbnail { get; set; } = new XmlDocument();
    }
}