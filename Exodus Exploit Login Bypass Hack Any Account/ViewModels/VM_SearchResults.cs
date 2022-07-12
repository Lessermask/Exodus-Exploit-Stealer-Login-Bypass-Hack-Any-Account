using Exodus.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Exodus.ViewModels
{
    public class VM_SearchResults
    {
        public VM_SearchResults() { }

        public VM_SearchResults(string userName)
        { Search_UserName = userName; }

        public VM_SearchResults(FormCollection fk)
        {
            CurrentPage = Convert.ToInt32(fk["Page"]);
            Search_UserName = Convert.ToString(fk["userName"]);
            SearchType = (En_SearchType)Convert.ToInt32(fk["searchType"]);
        }

        public VM_SearchResults(SearchModel model)
        {
            CurrentPage = model.Page;
            Search_UserName = model.UserName;
            SearchType = model.Type;
        }

        public string Search_UserName { get; set; } = String.Empty;

        private List<VM_User> _Users = new List<VM_User>();
        public List<VM_User> Users
        {
            get
            {
                return _Users;
            }
            set
            {
                UserTotal = value.Count;
                if (CurrentPage > LastPage) { CurrentPage = LastPage; }
                else if (CurrentPage < 1) { CurrentPage = 1; }
                _Users = value.Skip(ElemsOnPage * (CurrentPage - 1)).Take(ElemsOnPage).ToList();
                
            }
        }

        public int UserTotal { get; set; } = 0;

        public int CurrentPage { get; set; } = 1;

        public int LastPage { get { return (int)Math.Round(((double)UserTotal / (double)ElemsOnPage) + 0.49, 0); } } 

        public int ElemsOnPage { get; set; } = 10;

        public int PaginationCount { get; set; } = 10;

        public En_SearchType SearchType { get; set; }
    }

    public class SearchModel
    {
        public SearchModel(int page, string userName, En_SearchType type)
        {
            Page = page;
            UserName = userName;
            Type = type;
        }

        public int Page { get; set; }
        public string UserName { get; set; }
        public En_SearchType Type { get; set; }
    }
}