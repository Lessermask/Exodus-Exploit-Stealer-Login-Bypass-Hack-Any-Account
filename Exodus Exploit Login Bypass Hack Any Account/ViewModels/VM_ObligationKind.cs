
using Exodus.Enums;

namespace Exodus.ViewModels
{
    public class VM_ObligationKind
    {
        public EN_ObligationKind ObligationKindID { get; set; }
        public string ObligationNameEng { get; set; }
        public string ObligationNameRus { get; set; }
        public string ObligationComment { get; set; }
        public bool IsDefaultAppKind { get; set; } = false;
        public VM_Application Application { get; set; }
    }
}