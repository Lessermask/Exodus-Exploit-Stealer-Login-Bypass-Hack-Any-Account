using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Exodus.DTO;
using Exodus.Domain;
using Exodus.ViewModels;
using Exodus.Models.Search;

namespace Exodus.Service
{
    public partial class _SL
    {
        public static class Search
        {
            public static List<UserSearch> UsersByString(string query)
            {
                var list = _DL.Search.UserByString(query);
                return list;
            }

            public static List<TagSearch> TagsByString(string query)
            {
                var list = _DL.Search.TagByString(query);
                return list;       
            }

            public static List<object> Any(string query)
            {
                var list = _DL.Search.Any(query);
                return list;
            }
        }
    }
}