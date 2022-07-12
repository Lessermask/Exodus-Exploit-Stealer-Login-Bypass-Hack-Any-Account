using Exodus.Enums;

namespace Exodus.ViewModels
{
    public class VM_ObligationType
    {
        public EN_ObligationType ObligationTypeID { get; set; }
        public string ObligationTypeNameEng { get; set; }
        public string ObligationTypeNameRus { get; set; }
        public string ObligationTypeComment { get; set; }
    }
}