using Exodus.Extensions;
using Exodus.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;

namespace Exodus.ViewModels
{
    public class VM_Event
    {
        [JsonIgnore]
        public long ID { get; set; }
        [JsonIgnore]
        public VM_EventCategory Category { get; set; }
        [JsonIgnore]
        public VM_EventType Type { get; set; }
        [JsonIgnore]
        public VM_Tag Tag { get; set; }
        [JsonIgnore]
        public VM_Application Application { get; set; }
        [JsonIgnore]
        public string Title { get; set; }
        [JsonIgnore]
        public XmlDocument Content { get; set; } = new XmlDocument();
        [JsonIgnore]
        public XmlDocument Thumbnail { get; set; } = new XmlDocument();
        [JsonIgnore]
        public DateTime Added { get; set; }
        [JsonIgnore]
        public VM_ImportanLevel ImportanLevel { get; set; }
        public Dictionary<string, object> LightJson
        {
            get
            {
                var listNames = EventTemplate.GetKeyItemsThumbnail(Type.Type)
                    .Where(a => a.ModelType == Enums.EN_ModelType.Class)
                    .Select(a => a.Name).ToList();
                //
                Dictionary<string, object> dic = Thumbnail.SelectNodes("//key").Cast<XmlElement>()
                    .Concat(Content.SelectNodes("//key").Cast<XmlElement>())
                    .Select(a => new
                    {
                        name = a.GetAttribute("name").Trim(),
                        value = a.GetAttribute("value").Trim()
                    }).ToList().Distinct().ToDictionary(x => x.name, x => (object)x.value);
                //
                foreach (var name in listNames)
                {
                    string value = dic[name] as string;
                    var list = value.Split(new string[] { "\",\"" }, StringSplitOptions.RemoveEmptyEntries)
                        .ToDictionary(
                        a => a.Split(new string[] { "\":\"" }, StringSplitOptions.RemoveEmptyEntries).First().Trim(new char[] {'\"'}),
                        b => b.Split(new string[] { "\":\"" }, StringSplitOptions.RemoveEmptyEntries).Last().Trim(new char[] { '\"' }));
                    //
                    dic[name] = list;
                }
                // Additional
                dic.Add("EventID", ID);
                if (!dic.ContainsKey("ApplicationID")) { dic.Add("ApplicationID", Application == null ? 0 : Application.ApplicationID); }
                if (!dic.ContainsKey("TagDescription")) { dic.Add("TagDescription", Tag == null ? "" : Tag.Description); }
                if (!dic.ContainsKey("TagID")) { dic.Add("TagID", Tag == null ? 0 : Tag.TagID); }
                if (!dic.ContainsKey("Category")) { dic.Add("Category", Category == null ? 0 : Category.EventCategory.ToInt()); }
                if (!dic.ContainsKey("ImportanLevel")) { dic.Add("ImportanLevel", ImportanLevel == null ? 0 : ImportanLevel.ImportantLevel.ToInt());}
                if (!dic.ContainsKey("Type")) { dic.Add("Type", Type == null ? 0 : Type.Type.ToInt());}
                if (!dic.ContainsKey("Title")) { dic.Add("Title", Title);}
                if (!dic.ContainsKey("Added")) { dic.Add("Added", Added);}
                //
                UpdateAvatar(ref dic);
                //
                return dic;
            }
        }
        private void UpdateAvatar(ref Dictionary<string, object> dic)
        {
            foreach (string key in dic.Keys.ToList())
            {
                if (key.ToLower().IndexOf("avatarsmall") != -1)
                {
                    long UserID = long.Parse(dic[key].ToString());
                    dic[key] = Global.Cache.dicUserAvatars[UserID].AvatarSmall;
                }
                else if (key.ToLower().IndexOf("avatarbig") != -1)
                {
                    long UserID = long.Parse(dic[key].ToString());
                    dic[key] = Global.Cache.dicUserAvatars[UserID].AvatarBig;
                }
            }
        }
    }
}