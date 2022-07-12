using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Exodus.ViewModels
{
    public class VM_TagPublicWinUserCount
    {
        public VM_Application Application  {  get; set; } = null;
        public long UserCount { get; set; }
        public VM_Tag Tag { get; set; } = null;
    }
}