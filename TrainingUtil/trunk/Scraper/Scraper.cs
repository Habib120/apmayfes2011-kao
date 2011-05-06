using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sgml;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Net;
using System.Threading;

namespace Scraper
{
    public class Scraper
    {
        private string base_url = "http://www.weblio.jp/category/people/tltdb/";
        private string base_dir = @"C:\Users\t2ladmin\Documents\ml_data\scraping";
        private string[] indexes = 
        {
            "aa", "ii", "uu", "ee", "oo",
            "ka", "ki", "ku", "ke", "ko",
            "sa", "si", "su", "se", "so",
            "ta", "ti", "tu", "te", "to",
            "na", "ni", "nu", "ne", "no",
            "ha", "hi", "hu", "he", "ho",
            "ma", "mi", "mu", "me", "mo",
            "ra", "ri", "ru", "re", "ro",
            "wa",
            "ga", "gi", "gu", "ge", "go",
            "da",             "de", "do",
            "ba", "bi", "bu", "be", "bo",
        };

        public Scraper()
        {
        }

        public static XDocument ParseHtml(string url)
        {
            using (var stream = new WebClient().OpenRead(url))
            using (var sr = new StreamReader(stream, Encoding.UTF8))
            using (var sgmlReader = new SgmlReader { DocType = "HTML", CaseFolding = CaseFolding.ToLower })
            {
                sgmlReader.InputStream = sr; // ↑の初期化子にくっつけても構いません
                return XDocument.Load(sgmlReader);
            }
        }

        public void Start(int count, Action<string> msgCallback = null, Action<double> progressCallback = null, Action completeCallback = null)
        {
            var targets = this.GetUrlList(count);
            foreach (var item in targets.Select((target, i) => new {target, i}))
            {

                var loader = new ImageLoader(item.target, String.Format(@"{0}\face_{1:D4}.jpg", base_dir, item.i));
                ThreadPool.QueueUserWorkItem(new WaitCallback((obj) => loader.Start(() => OnProgressChanged())));
            }
        }

        public void OnProgressChanged()
        {
        }

        protected List<string> GetUrlList(int nimages)
        {
            var current_page = ParseHtml(base_url + indexes[0]);
            var current_page_idx = 1;
            var iindex = 0;
            var cnt = 0;
            var urls = new List<string>();
            while (cnt < nimages)
            {
                var ns = current_page.Root.Name.Namespace;
                var targets = from div in current_page.Descendants(ns + "div")
                              where div.Attribute("class") != null && div.Attribute("class").Value == "CtgryImgListL"
                              select div.Descendants(ns + "a").First().Attribute("href").Value;
                targets = targets.Where((url, i) => i % 2 == 0);
                foreach (var target in targets)
                {
                    urls.Add(target);
                    cnt++;
                    if (cnt >= nimages)
                    {
                        break;
                    }
                }

                string npage_url;
                while (true)
                {
                    try
                    {
                        var cpage = current_page.Descendants(ns + "span")
                                    .Where((span) => span.Attribute("class") != null && span.Attribute("class").Value == "TargetPage")
                                    .First();
                        npage_url = cpage.ElementsAfterSelf(ns + "a").First().Attribute("href").Value;
                    }
                    catch (Exception ex)
                    {
                        iindex ++;
                        if (iindex >= indexes.Count())
                        {
                            break;
                        }
                        npage_url = base_url + indexes[iindex];
                        current_page_idx = 1;
                    }
                    try
                    {
                        current_page = ParseHtml(npage_url);
                        break;
                    }
                    catch
                    {
                        continue;
                    }
                }
            }


            return urls;
        }

        protected string GetNextPage(XDocument xml, int current_page_idx)
        {
            return "http://www.weblio.jp/content/%E7%9B%B8%E6%BE%A4%E6%99%AF%E5%AD%90?dictCode=TLTDB";
        }

    }

    public class ImageLoader
    {
        private string _url;
        private string _filename;

        public ImageLoader(string url, string _filename)
        {
            this._url = url;
            this._filename = _filename;
        }

        public void Start(Action completeCallback)
        {
            try
            {
                var xml = Scraper.ParseHtml(_url); // これだけでHtml to Xml完了。あとはLinq to Xmlで操作。

                XNamespace ns = xml.Root.Name.Namespace;
                var target = xml.Descendants(ns + "div").Where((e) => e.Attribute("class") != null && e.Attribute("class").Value == "TltdbLeft").First();
                var img_url = target.Element(ns + "img").Attribute("src").Value;

                new WebClient().DownloadFile(img_url, _filename);
            }
            catch (Exception ex)
            {
                
            }
        }
    }
}
