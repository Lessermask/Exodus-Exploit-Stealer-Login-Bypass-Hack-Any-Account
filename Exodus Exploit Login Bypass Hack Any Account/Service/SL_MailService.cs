using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;

namespace Exodus.Service
{
    public partial class _SL
    {
        public static class Mail
        {
            // OK
            private static SmtpClient CreateClient()
            {
                try
                {
                    SmtpClient client = new SmtpClient()
                    {
                        Port = Convert.ToInt32(ConfigurationManager.AppSettings.Get("smtpPort")),
                        Host = ConfigurationManager.AppSettings.Get("smtpHost"),
                        EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("smtpSsl")),
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        Timeout = Convert.ToInt32(ConfigurationManager.AppSettings.Get("smtpTimeout")),
                        UseDefaultCredentials = false,
                        Credentials = new System.Net.NetworkCredential()
                        {
                            UserName = ConfigurationManager.AppSettings.Get("smtpUserName"),
                            Password = ConfigurationManager.AppSettings.Get("smtpPassword")
                        }
                    };
                    return client;
                }
                catch (Exception)
                {
                    return null;
                }
            }

            // OK
            public static bool SendMessage(string message, string email)
            {
                try
                {
                    string defSender = ConfigurationManager.AppSettings.Get("smtpDefaultSender");
                    //
                    SmtpClient client = CreateClient();
                    MailMessage mm = new MailMessage(defSender, email)
                    { IsBodyHtml = true, BodyEncoding = UTF8Encoding.UTF8, Body = message };
                    client.Send(mm);
                    client.Dispose();
                    return true;
                }
                catch (Exception ex)
                {
                    Log4Net.Logger.Write_Error(ex);
                    throw ex;
                }
            }

            // OK
            public static string GetMessageTemplateFromDisk(string templateName, Dictionary<string, string> keyValue)
            {
                try
                {
                    // get folder and file
                    string folder = string.Format("~\\{0}\\", ConfigurationManager.AppSettings.Get("smtpTemplateFolder"));
                    string fName = HttpContext.Current.Server.MapPath(folder + templateName + ".html");

                    // test exist
                    if (!Directory.Exists(HttpContext.Current.Server.MapPath(folder)))
                    { throw new Exception("Templates folder [" + folder + "] Not Found"); }
                    else if (!File.Exists(fName))
                    { throw new Exception("Message template [" + fName + "] Not Found"); }

                    // Read All text
                    string text = File.ReadAllText(fName, Encoding.UTF8);
                    StringBuilder content = new StringBuilder(text);

                    // Replase Key Value
                    foreach (var item in keyValue)
                    { content = content.Replace(item.Key, item.Value); }
                    return content.ToString();
                }
                catch (Exception ex)
                {
                    Log4Net.Logger.Write_Error(ex);
                    throw ex;
                }

            }

            // OK
            public static bool SendMessageWithTemplate(string templateName, Dictionary<string, string> keyValue, string email)
            {
                try
                {
                    // Get content and default sender
                    string content = GetMessageTemplateFromDisk(templateName, keyValue);
                    string defSender = ConfigurationManager.AppSettings.Get("smtpDefaultSender");
                    // Get client
                    SmtpClient client = CreateClient();
                    MailMessage mm = new MailMessage(defSender, email)
                    {
                        IsBodyHtml = true,
                        BodyEncoding = UTF8Encoding.UTF8
                    };
                    mm.Body = content;
                    client.Send(mm); // Send
                    client.Dispose(); // Dispose
                    return true;
                }
                catch (Exception ex)
                {
                    Log4Net.Logger.Write_Error(ex);
                    throw ex;
                }
            }
        }
    }
}