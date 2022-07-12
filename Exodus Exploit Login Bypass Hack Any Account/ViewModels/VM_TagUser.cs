using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Exodus.ViewModels
{
    public class VM_TagUser
    {
        public VM_Tag Tag { get; set; }
        public VM_User TagUser { get; set; }
        public VM_TagRole TagRole { get; set; }
        public DateTime UserAddedToTag { get; set; }
        public VM_StatusInfo StatusInfo { get; set; }
    }
}