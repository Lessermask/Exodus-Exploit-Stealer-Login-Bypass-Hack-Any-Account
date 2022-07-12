using Exodus.Domain;
using Exodus.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Exodus.ViewModels
{
    public class VM_TagRole
    {
        public long TagRoleID { get; set; }
        public EN_TagRole Role => (EN_TagRole)TagRoleID; 
        public string NameEng { get; set; }
        public string NameRus { get; set; }
    }
}