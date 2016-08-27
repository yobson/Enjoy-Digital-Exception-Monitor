using System;
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

            menu.AddListing(new MenuItem { ListingText = "Maximum Threads", DefaltValue = "10", Type = MenuItem.Int, PropertyName = "maxConcurrentThreads", Menu = "Crawler Settings"});
            menu.AddListing(new MenuItem { ListingText = "Maximum Threads", DefaltValue = "10", Type = MenuItem.Int, PropertyName = "maxConcurrentThreads", Menu = "Crawler Settings" });

            menu.PrintMenu();

            Console.ReadLine();

            return 0;
        }

    }
}
