using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Exodus.ViewModels;
using System.IO;
using Exodus.Domain;
using System.Text;
using System.Xml;

namespace Exodus.Service
{
    public partial class _SL
    {
        public static class EventService
        {
            public class XML_Dictionary
            {
                private XML_Dictionary(string content)
                {
                    // load
                    Document = new XmlDocument();
                    Document.LoadXml(content);
                    // parse
                    Parse();
                }

                private XML_Dictionary(Dictionary<string, string> dic)
                {
                    // Set dictionary
                    Dictionary = dic;
                    // Create Document
                    Document = new XmlDocument();
                    var keys = Document.CreateElement("keys");

                    foreach (var item in dic)
                    {
                        var key = Document.CreateElement("key");
                        key.SetAttribute("name", item.Key);
                        key.SetAttribute("value", item.Value);
                        keys.AppendChild(key);
                    }
                    Document.AppendChild(keys);
                }

                private void Parse()
                {
                    var dicXml = Document.DocumentElement.SelectNodes("key")
                        .OfType<XmlNode>().Select(a => new
                        {
                            key = a.Attributes["name"].Value,
                            value = a.Attributes["value"].Value
                        });

                    Dictionary = new Dictionary<string, string>();
                    foreach (var item in dicXml)
                    {
                        Dictionary.Add(item.key, item.value);
                    }

                }

                public Dictionary<string, string> Dictionary { get; private set; }

                public XmlDocument Document { get; private set; }

                public string XML { get { return Document.OuterXml; } }

                public List<string> Keys { get { return Dictionary.Keys.ToList(); } }

                public List<string> Values { get { return Dictionary.Values.ToList(); } }

                public static Dictionary<string, string> XMLToDictionary(string xml)
                {
                    XmlDocument Document = new XmlDocument();
                    Document.LoadXml(xml);
                    Dictionary<string, string> dic = new Dictionary<string, string>();

                    foreach (var item in Document.DocumentElement.SelectNodes("key").OfType<XmlNode>().Select(a => new { key = a.Attributes["name"].Value, value = a.Attributes["value"].Value }))
                    {
                        dic.Add(item.key, item.value);
                    }

                    return dic;
                }

                public static string DictionaryToXml(Dictionary<string, string> dic)
                {
                    XmlDocument Document = new XmlDocument();
                    XmlNode docNode = Document.CreateXmlDeclaration("1.0", "UTF-8", "yes");
                    Document.AppendChild(docNode);
                    var keys = Document.CreateElement("keys");

                    foreach (var item in dic)
                    {
                        var key = Document.CreateElement("key");
                        key.SetAttribute("name", item.Key);
                        key.SetAttribute("value", item.Value);
                        keys.AppendChild(key);
                    }
                    Document.AppendChild(keys);
                    return Document.OuterXml;
                }

                /// <summary>
                /// Load from dictionary
                /// </summary>
                /// <param name="dic">Dictionary content</param>
                /// <returns></returns>
                public static XML_Dictionary LoadDictionary(Dictionary<string, string> dic)
                {
                    var xmlDic = new XML_Dictionary(dic);
                    return xmlDic;
                }

                /// <summary>
                /// String content
                /// </summary>
                /// <param name="xml">Xml content</param>
                /// <returns></returns>
                public static XML_Dictionary LoadXML(string xml)
                {
                    var xmlDic = new XML_Dictionary(xml);
                    return xmlDic;
                }

                /// <summary>
                /// Path to file
                /// </summary>
                /// <param name="filepath">path to file</param>
                /// <returns></returns>
                public static XML_Dictionary Load(string filepath)
                {
                    var xml = File.ReadAllText(filepath);
                    var xmlDic = new XML_Dictionary(xml);
                    return xmlDic;
                }
            }
        }   
    }
}