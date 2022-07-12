using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Exodus.ViewModels
{
    public class VM_Period
    {
        public long PeriodID { get; set; }
        public string PeriodNameEng { get; set; }
        public string PeriodNameRus { get; set; }
        public string PeriodComment { get; set; }
    }
}