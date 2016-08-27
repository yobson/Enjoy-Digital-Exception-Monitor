﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using Enjoy_Digital_Exception_Monitor.GUI;

namespace Enjoy_Digital_Exception_Monitor
{
    class Program
    {

        static int Main(string[] args)
        {
            

            Crawling crawl = new Crawling();
            crawl.setUp();

            Menu menu = new Menu{ crawl = crawl };
            menu.AddMenu("Crawler Settings");
            menu.AddMenu("Slack Configuration");

            menu.AddListing(menu.newListing("Maximum Threads", MenuItem.Int, "maxConcurrentThreads", "Crawler Settings"));
            menu.AddListing(menu.newListing("Maximum Pages To Crawl", MenuItem.Int, "maxPagesToCrawl", "Crawler Settings"));
            menu.AddListing(menu.newListing("Crawler Timeout", MenuItem.Int, "crawlTimeoutSeconds", "Crawler Settings"));
            menu.AddListing(menu.newListing("HTTP Request Timeout", MenuItem.Int, "httpRequestTimeoutInSeconds", "Crawler Settings"));
            menu.AddListing(menu.newListing("Always Log In?", MenuItem.Bool, "alwaysLogIn", "Crawler Settings"));
            menu.AddListing(menu.newListing("Username", MenuItem.String, "loginUser", "Crawler Settings"));
            menu.AddListing(menu.newListing("Password", MenuItem.String, "loginPassword", "Crawler Settings"));
            menu.AddListing(menu.newListing("Post to Slack Enabled", MenuItem.Bool, "slackBotEnabled", "Slack Configuration"));
            menu.AddListing(menu.newListing("Slack Bot Identifier", MenuItem.String, "slackIdentifier", "Slack Configuration"));
            menu.AddListing(menu.newListing("Slack Channel Hook URL", MenuItem.String, "slackBotHookURL", "Slack Configuration"));

            menu.load("save.crawl");
            menu.RunMenu();
            menu.save("save.crawl");

            Console.ReadLine();

            return 0;
        }

    }
}
