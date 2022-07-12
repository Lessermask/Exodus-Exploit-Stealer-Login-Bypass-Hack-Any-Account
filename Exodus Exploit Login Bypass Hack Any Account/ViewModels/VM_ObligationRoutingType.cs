using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Exodus.ViewModels
{
    public class VM_ObligationRoutingType
    {
        public int RoutingTypeID { get; set; }
        public string RoutingTypeNameEng { get; set; }
        public string RoutingTypeNameRus { get; set; }
        public string RoutingTypeComment { get; set; } // Can be null
    }
}