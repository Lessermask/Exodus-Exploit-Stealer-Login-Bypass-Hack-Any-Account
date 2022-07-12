using Exodus.Controllers;
using Exodus.Domain;
using Exodus.Enums;
using Exodus.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Exodus.ViewModels
{
    public class VM_Tag
    {
        public long TagID { get; set; }
        public string NameSystem { get; set; }
        public string NameEng { get; set; } = String.Empty;
        public string NameRus { get; set; } = String.Empty;
        public string Name
        {
            get
            {
                if(Global.Global.Language.ToLower() == "ru")
                { return String.IsNullOrEmpty(NameRus) ? NameEng : NameRus; }
                else
                { return String.IsNullOrEmpty(NameEng) ? NameRus : NameEng; }
            }
        }

        public string Description { get; set; } = String.Empty;
        public DateTime Created { get; set; } = DateTime.Now;
        public long Owner_UserID { get; set; }
        public string OwnerFirstName { get; set; }
        public string OwnerLastName { get; set; }
        public EN_TagType Type { get; set; } = EN_TagType.UserDefined;
        public string TypeNameEng { get; set; }
        public string TypeNameRus { get; set; }
        public string TypeCommentEng { get; set; }
        public string TypeCommentRus { get; set; }
        public bool IsTypePredefined { get; set; }
        public EN_TagAccessType AccessType { get; set; } = EN_TagAccessType.None;
        public string AccessTypeNameEng { get; set; }
        public string AccessTypeNameRus { get; set; }
        public int UsersCount { get; set; }
        public DateTime Added { get; set; } = DateTime.Now;
        public VM_TagRole Role { get; set; }
        public EN_TagPeriod Period { get; set; }
        public string PeriodNameRus { get; set; }
        public string PeriodNameEng { get; set; }
        public int ApplicationID { get; set; }
        public EN_ApplicationType ApplicationType => (EN_ApplicationType)ApplicationID;
        public string ApplicationNameRus { get; set; }
        public string ApplicationNameEng { get; set; }
        public string ApplicationName
        {
            get
            {
                if (Global.Global.Language.ToLower() == "ru")
                { return String.IsNullOrEmpty(NameRus) ? ApplicationNameEng : ApplicationNameRus; }
                else
                { return String.IsNullOrEmpty(NameEng) ? ApplicationNameRus : ApplicationNameEng; }
            }
        }
        public DateTime EndDate { get; set; } = DateTime.MinValue;
        public int DayOfMonth { get; set; }
        public int DayOfWeek { get; set; }
        public decimal TotalAmount { get; set; }
        public int TotalAmountCurrencyID { get; set; }
        public string TotalAmountCurrencyCode { get; set; }
        public decimal MinIntentionAmount { get; set; }
        public int MinIntentionCurrencyID { get; set; }
        public string MinIntentionCurrencyCode { get; set; }
        public VM_User DefaultIntentionOwner { get; set; }
        public En_Currency TagCurrency { get { return (En_Currency)TotalAmountCurrencyID; } }

        public string LinkToJoin
        {
            get
            {
                var Request = HttpContext.Current.Request.Url;
                var port = (Request.Host == "localhost") ? ":" +Request.Port.ToString() : "";
                var location = new Uri($"{Request.Scheme}://{Request.Host}{port}").ToString().TrimEnd('/');
                return $"{location}/Tag/JoinToTag?TagID={TagID}&UserID={Global.Global.CurrentUser.UserID}";
            }
        } 
    }
}