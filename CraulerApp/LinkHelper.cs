using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;

namespace CraulerApp
{
    public class LinkHelper
    {
        public List<string> links = new List<string>();

        public void Start(string url)
        {
            using (WebClient client = new WebClient())
            {
                var web = new HtmlWeb();
                Thread.Sleep(1000);
                var doc = web.Load(url);
                HtmlNode s = doc.DocumentNode.SelectSingleNode("//body");
                Guid guid = Guid.NewGuid();
                string path = $@"C:\Users\Искандер\source\repos\CraulerApp\CraulerApp\Files\{guid.ToString()}.txt";
                Regex trimmer = new Regex(@"\s\s+");
                string bodyText = trimmer.Replace(s.InnerText, " ");
                File.WriteAllText(path, bodyText);
                foreach (var item in s.SelectNodes("//a[@href]"))
                {
                    string link = item.Attributes["href"].Value;
                    if (!string.IsNullOrWhiteSpace(link) && link.Contains("https://vc.ru/") && !link.Contains("?comments") && link.Length > 23)
                    {
                        if (!links.Contains(link) && links.Count < 101)
                        {
                            links.Add(link);
                            Start(link);
                        }                       
                    }
                }
            }
        }
    }
}
