using Exodus.Enums;

namespace Exodus.ViewModels
{
    public class VM_Application
    {
        public EN_ApplicationType Type { get { return (EN_ApplicationType)ApplicationID; } }
        public int ApplicationID { get; set; }
        public string ApplicationNameEng { get; set; }
        public string ApplicationNameRus { get; set; }
        public string Name
        {
            get
            {
                return Global.Global.Language == "ru" ? ApplicationNameRus : ApplicationNameEng;
            }
        }
        public string ApplicationDescription { get; set; }
        public bool ApplicationIsActive { get; set; }

    }
}