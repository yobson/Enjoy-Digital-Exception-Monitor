using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abot.Core;
using Abot.Crawler;
using Abot.Poco;
using System.Net;
using System.Net.Http;
using System.Web.Script.Serialization;

namespace Enjoy_Digital_Exception_Monitor
{
    class Crawling
    {
        private List<Errors> errors;
        private int returnInt = 0;

        public string URL { get; set; }

        public int maxConcurrentThreads { get; set; }
        public int maxPagesToCrawl { get; set; }
        public int crawlTimeoutSeconds { get; set; }
        public int httpRequestTimeoutInSeconds { get; set; }
        public string loginUser { get; set; }
        public string loginPassword { get; set; }
        public bool alwaysLogIn { get; set; }
        public string fileName { get; set; }

        public bool slackBotEnabled { get; set; }
        public string slackBotHookURL { get; set; }
        public string slackIdentifier { get; set; }
        public string slackBotChannel { get; set; }

        public bool pageSpeedEnabled { get; set; }
        public string googleApiKey { get; set; }

        public bool wait = true;

        public void setUp()
        {
            maxConcurrentThreads = 20;
            maxPagesToCrawl = 1000;
            crawlTimeoutSeconds = 0;
            httpRequestTimeoutInSeconds = 15;
            alwaysLogIn = false;
            loginUser = "";
            loginPassword = "";
            fileName = "save.crawl";

            slackBotEnabled = false;
            slackIdentifier = "[Generic Website Name] Bot";
            slackBotChannel = "";
            slackBotHookURL = "";

            pageSpeedEnabled = false;
            googleApiKey = "";
        }

        public int DoCrawl()
        {

            CrawlConfiguration CConfig = AbotConfigurationSectionHandler.LoadFromXml().Convert();
            CConfig.MaxConcurrentThreads = maxConcurrentThreads;
            CConfig.MaxPagesToCrawl = maxPagesToCrawl;
            CConfig.CrawlTimeoutSeconds = crawlTimeoutSeconds;
            CConfig.HttpRequestTimeoutInSeconds = httpRequestTimeoutInSeconds;
            CConfig.LoginUser = loginUser;
            CConfig.LoginPassword = loginPassword;

            Console.WriteLine("Doing Crawl With Slack " + (slackBotEnabled ? "Enabled" : "Disabled"));

            PoliteWebCrawler crawler = new PoliteWebCrawler(CConfig, null, null, null, null, null, null, null, null);
            //PoliteWebCrawler crawler = new PoliteWebCrawler();

            errors = new List<Errors>();


            crawler.PageCrawlStartingAsync += crawler_ProcessPageCrawlStarting;
            crawler.PageCrawlCompletedAsync += crawler_ProcessPageCrawlCompleted;
            crawler.PageCrawlDisallowedAsync += crawler_PageCrawlDisallowed;
            crawler.PageLinksCrawlDisallowedAsync += crawler_PageLinksCrawlDisallowed;

            CrawlResult result = crawler.Crawl(new Uri(URL)); //This is synchronous, it will not go to the next line until the crawl has completed

            if (result.ErrorOccurred)
                Console.WriteLine("Crawl of {0} completed with error: {1}", result.RootUri.AbsoluteUri, result.ErrorException.Message);
            else
                Console.WriteLine("Crawl of {0} completed without error.", result.RootUri.AbsoluteUri);

            string slackbotMessage = "";

            IEnumerable<Errors> EnumList = errors.AsEnumerable();

            for (int i = 0; i < 525; i++)
            {
                if (EnumList.Where(x => x.ErrorCode == i).Count() != 0)
                {
                    returnInt = 1;
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine(i + " (" + getErrorName(i) + ") Errors:");
                    slackbotMessage += i + " (" + getErrorName(i) + ") Errors:\n";
                    Console.ForegroundColor = ConsoleColor.Red;
                    foreach (Errors err in EnumList.Where(x => x.ErrorCode == i))
                    {
                        Console.WriteLine("   " + err.ErrorURL);
                        slackbotMessage += "   " + err.ErrorURL + "\n";
                    }
                }
            }

            Console.ResetColor();

            PageSpeedEntity stats = new PageSpeedEntity();
            if (pageSpeedEnabled)
            {
                Console.Write("\n");
                Console.ForegroundColor = ConsoleColor.Blue; Console.Write("G");
                Console.ForegroundColor = ConsoleColor.Red; Console.Write("o");
                Console.ForegroundColor = ConsoleColor.Yellow; Console.Write("o");
                Console.ForegroundColor = ConsoleColor.Blue; Console.Write("g");
                Console.ForegroundColor = ConsoleColor.Green; Console.Write("l");
                Console.ForegroundColor = ConsoleColor.Red; Console.Write("e");
                Console.ResetColor();
                Console.WriteLine(" Page Benchmark");
                stats = DoPageSpeed().GetAwaiter().GetResult();
                Console.Write("Website Score: ");
                if (stats.score < 50) { Console.ForegroundColor = ConsoleColor.Red; }
                Console.WriteLine(stats.score);
                Console.ResetColor();
                Console.WriteLine("\nFind detailed report at: https://developers.google.com/speed/pagespeed/insights/?url=" + URL);
                slackbotMessage += "Google Page Benchmark = " + stats.score + "See this link for full report:\n" + "https://developers.google.com/speed/pagespeed/insights/?url=" + URL;
            }

            if (slackBotEnabled)
            {
                if (slackbotMessage == "")
                {
                    slackbotMessage = "No Errors In WebPage!";
                }
                string urlWithAccessToken = slackBotHookURL;
                SlackClient client = new SlackClient(urlWithAccessToken);

                client.PostMessage(username: slackIdentifier, text: slackbotMessage, channel: (slackBotChannel != "") ? null : slackBotChannel);
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Done");
            Console.ResetColor();

            if (wait) { Console.Read(); }
            Console.ResetColor();
            return returnInt;
        }

        string getErrorName(int error)
        {
            string errorName;
            switch (error)
            {
                default:
                    errorName = "Unknown";
                    break;
                case (204):
                    errorName = "No Content";
                    break;
                case (400):
                    errorName = "Bad Request";
                    break;
                case (401):
                    errorName = "Unauthorized";
                    break;
                case (402):
                    errorName = "Payment Required";
                    break;
                case (403):
                    errorName = "Forbidden";
                    break;
                case (404):
                    errorName = "Not Found";
                    break;
                case (405):
                    errorName = "Method Not Allowed";
                    break;
                case (406):
                    errorName = "Not Acceptable";
                    break;
                case (407):
                    errorName = "Proxy Authentification Required";
                    break;
                case (408):
                    errorName = "Request Timeout";
                    break;
                case (409):
                    errorName = "Conflict";
                    break;
                case (410):
                    errorName = "Gone";
                    break;
                case (411):
                    errorName = "Length Required";
                    break;
                case (412):
                    errorName = "Precondition Required";
                    break;
                case (413):
                    errorName = "Request Entery Too Large";
                    break;
                case (414):
                    errorName = "Request-URI Too Long";
                    break;
                case (415):
                    errorName = "Unsuported Media Type";
                    break;
                case (416):
                    errorName = "Requested Range Not Satisfiable";
                    break;
                case (417):
                    errorName = "Expectation Failed";
                    break;
                case (418):
                    errorName = "I'm a teapot!";
                    break;
                case (422):
                    errorName = "Unprocessable Entity";
                    break;
                case (428):
                    errorName = "Precondition Required";
                    break;
                case (429):
                    errorName = "Too Many Requests";
                    break;
                case (431):
                    errorName = "Request Header Fields Too Large";
                    break;
                case (451):
                    errorName = "Unavailable For Legal Reasons";
                    break;
                case (500):
                    errorName = "Internal Server Error";
                    break;
                case (501):
                    errorName = "Not Implemented";
                    break;
                case (502):
                    errorName = "Bad Gateway";
                    break;
                case (503):
                    errorName = "Service Unavailable";
                    break;
                case (504):
                    errorName = "Gateway Timeout";
                    break;
                case (505):
                    errorName = "HTTP Version Not Supported";
                    break;
                case (511):
                    errorName = "Network Authentication Required";
                    break;
                case (520):
                    errorName = "Web server is returning an unknown error";
                    break;
                case (522):
                    errorName = "Connection timed out";
                    break;
                case (524):
                    errorName = "A timeout occurred";
                    break;
                    
            }
            return errorName;
        }

        void crawler_ProcessPageCrawlStarting(object sender, PageCrawlStartingArgs e)
        {
            PageToCrawl pageToCrawl = e.PageToCrawl;
            Console.WriteLine("About to crawl link {0} which was found on page {1}", pageToCrawl.Uri.AbsoluteUri, pageToCrawl.ParentUri.AbsoluteUri);
        }

        void crawler_ProcessPageCrawlCompleted(object sender, PageCrawlCompletedArgs e)
        {
            CrawledPage crawledPage = e.CrawledPage;

            
            
            if (crawledPage.WebException != null || crawledPage.HttpWebResponse.StatusCode != HttpStatusCode.OK)
            {
                errors.Add(new Errors { ErrorURL = crawledPage.Uri.AbsoluteUri, ErrorCode = (int)crawledPage.HttpWebResponse.StatusCode });
                Console.WriteLine("Crawl of page failed {0}", crawledPage.Uri.AbsoluteUri);
            }
            else
            {
                Console.WriteLine("Crawl of page succeeded {0}", crawledPage.Uri.AbsoluteUri);
            }

            if (string.IsNullOrEmpty(crawledPage.Content.Text))
            {
                //errors.Add(new Errors { ErrorURL = crawledPage.Uri.AbsoluteUri, ErrorCode = 204 });
            }
            Console.WriteLine("");

        }

        void crawler_PageLinksCrawlDisallowed(object sender, PageLinksCrawlDisallowedArgs e)
        {
            CrawledPage crawledPage = e.CrawledPage;
            Console.WriteLine("Did not crawl the links on page {0} due to {1}", crawledPage.Uri.AbsoluteUri, e.DisallowedReason);
        }

        void crawler_PageCrawlDisallowed(object sender, PageCrawlDisallowedArgs e)
        {
            PageToCrawl pageToCrawl = e.PageToCrawl;
            Console.WriteLine("Did not crawl page {0} due to {1}", pageToCrawl.Uri.AbsoluteUri, e.DisallowedReason);
        }

        async Task<PageSpeedEntity> DoPageSpeed()
        {
            HttpClient client = new HttpClient();
            var homeResponseMessage = await client.GetAsync("https://www.googleapis.com/pagespeedonline/v1/runPagespeed?url="+ URL + (googleApiKey == "" ? "" : ("&key={" + googleApiKey + "}")));
            homeResponseMessage.EnsureSuccessStatusCode();

            var homeValue = await homeResponseMessage.Content.ReadAsStringAsync();

            var javaScriptSerializer = new JavaScriptSerializer();
            var homePageScore = javaScriptSerializer.Deserialize<PageSpeedEntity>(homeValue);

            return homePageScore;
        }
    }
}

