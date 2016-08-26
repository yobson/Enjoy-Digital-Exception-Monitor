using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;

namespace Enjoy_Digital_Exception_Monitor
{
    class Program
    {

        static int Main(string[] args)
        {
            Crawling crawl = new Crawling();
            bool repeat = true;
            int exitCode = 0;
            while (repeat)
            {
                string Url = "";
                
                crawl.setUp();
                if (File.Exists("save.crawl"))
                {
                    StreamReader sr = new StreamReader("save.crawl");
                    if (sr.ReadLine() == Directory.GetCurrentDirectory())
                    {
                        crawl.maxConcurrentThreads = int.Parse(sr.ReadLine());
                        crawl.maxPagesToCrawl = int.Parse(sr.ReadLine());
                        crawl.crawlTimeoutSeconds = int.Parse(sr.ReadLine());
                        crawl.httpRequestTimeoutInSeconds = int.Parse(sr.ReadLine());
                        crawl.loginUser = sr.ReadLine();
                        crawl.loginPassword = sr.ReadLine();
                        crawl.slackBotEnabled = bool.Parse(sr.ReadLine());
                        crawl.slackIdentifier = sr.ReadLine();
                        crawl.slackBotChannel = sr.ReadLine();
                        crawl.slackBotHookURL = sr.ReadLine();
                        sr.Close();
                    } else
                    {
                        sr.Close();
                        File.Delete("save.crawl");
                    }
                    
                }

                if (args.Count() == 0)
                {
                    while (Url == "")
                    {
                        Console.Clear();
                        Console.WriteLine("Enter URL to crawl or hit enter to change settings");
                        Console.Write("URL: ");
                        Console.ForegroundColor = ConsoleColor.Green;
                        Url = Console.ReadLine();
                        Console.ResetColor();

                        if (Url == "")
                        {
                            crawl = setUp(crawl);
                        }
                    }
                }
                else
                {
                    Url = args[0];
                    crawl.wait = false;
                    repeat = false;
                }



                crawl.URL = Url;
                exitCode = crawl.DoCrawl();
            }

            return exitCode;
        }

        static Crawling setUp(Crawling crawl)
        {
            int selected = 1;
            while (true)
            {
                printSettingsPage(crawl, selected);
                ConsoleKeyInfo keyInfo = Console.ReadKey();
                if (keyInfo.Key == ConsoleKey.UpArrow) { if (selected > 1) { selected--;  } }
                if (keyInfo.Key == ConsoleKey.DownArrow) {  if (selected < 10) { selected++; } }
                if (keyInfo.Key == ConsoleKey.Escape) { Console.ResetColor(); break; }
                if (keyInfo.Key == ConsoleKey.Enter && selected == 7)
                {
                    crawl.slackBotEnabled = selectTrueFalse(crawl, selected, crawl.slackBotEnabled);
                }
                if (keyInfo.Key == ConsoleKey.Enter && selected != 7)
                {
                    Console.Write("\nNew Value: ");
                    Console.ForegroundColor = ConsoleColor.Green;
                    string newProp = Console.ReadLine();
                    switch (selected)
                    {
                        case (1):
                            crawl.maxConcurrentThreads = int.Parse(newProp);
                            break;
                        case (2):
                            crawl.maxPagesToCrawl = int.Parse(newProp);
                            break;
                        case (3):
                            crawl.crawlTimeoutSeconds = int.Parse(newProp);
                            break;
                        case (4):
                            crawl.httpRequestTimeoutInSeconds = int.Parse(newProp);
                            break;
                        case (5):
                            crawl.loginUser = newProp;
                            break;
                        case (6):
                            crawl.loginPassword = newProp;
                            break;
                        case (8):
                            crawl.slackIdentifier = newProp;
                            break;
                        case (9):
                            crawl.slackBotChannel = newProp;
                            break;
                        case (10):
                            crawl.slackBotHookURL = newProp;
                            break;                        
                    }

                    if (File.Exists("save.crawl"))
                    {
                        File.Delete("save.crawl");
                    }

                    StreamWriter sw = new StreamWriter("save.crawl");
                    sw.WriteLine(Directory.GetCurrentDirectory());
                    sw.WriteLine(crawl.maxConcurrentThreads);
                    sw.WriteLine(crawl.maxPagesToCrawl);
                    sw.WriteLine(crawl.crawlTimeoutSeconds);
                    sw.WriteLine(crawl.httpRequestTimeoutInSeconds);
                    sw.WriteLine(crawl.loginUser);
                    sw.WriteLine(crawl.loginPassword);
                    sw.WriteLine(crawl.slackBotEnabled);
                    sw.WriteLine(crawl.slackIdentifier);
                    sw.WriteLine(crawl.slackBotChannel);
                    sw.WriteLine(crawl.slackBotHookURL);

                    sw.Close();
                    Console.ResetColor();
                }
            }
            return crawl;
        }

        static void printSettingsPage(Crawling crawl, int selected)
        {
            Console.Clear();

            Console.WriteLine("Use Arrow Keys to select property and 'Esc' to exit\n");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n[Crawler Configuration]");
            Console.ResetColor();
            if (selected == 1) { Console.BackgroundColor = ConsoleColor.White; Console.ForegroundColor = ConsoleColor.Black; }
            Console.WriteLine("Maximum Concurrent Threads: " + crawl.maxConcurrentThreads);
            Console.ResetColor();

            if (selected == 2) { Console.BackgroundColor = ConsoleColor.White; Console.ForegroundColor = ConsoleColor.Black; }
            Console.WriteLine("Maximum Pages To Crawl: " + crawl.maxPagesToCrawl);
            Console.ResetColor();

            if (selected == 3) { Console.BackgroundColor = ConsoleColor.White; Console.ForegroundColor = ConsoleColor.Black; }
            Console.WriteLine("Crawl Timeout in Seconds: " + crawl.crawlTimeoutSeconds);
            Console.ResetColor();

            if (selected == 4) { Console.BackgroundColor = ConsoleColor.White; Console.ForegroundColor = ConsoleColor.Black; }
            Console.WriteLine("Http Request Timeout in Seconds: " + crawl.httpRequestTimeoutInSeconds);
            Console.ResetColor();

            if (selected == 5) { Console.BackgroundColor = ConsoleColor.White; Console.ForegroundColor = ConsoleColor.Black; }
            Console.WriteLine("Username: " + crawl.loginUser);
            Console.ResetColor();

            if (selected == 6) { Console.BackgroundColor = ConsoleColor.White; Console.ForegroundColor = ConsoleColor.Black; }
            Console.WriteLine("Password: " + crawl.loginPassword);
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n[SlackBot Configuration]");
            Console.ResetColor();

            if (selected == 7) { Console.BackgroundColor = ConsoleColor.White; Console.ForegroundColor = ConsoleColor.Black; }
            Console.WriteLine("Slackbot Enabled: " + crawl.slackBotEnabled);
            Console.ResetColor();

            if (selected == 8) { Console.BackgroundColor = ConsoleColor.White; Console.ForegroundColor = ConsoleColor.Black; }
            Console.WriteLine("Slackbot Identifier: " + crawl.slackIdentifier);
            Console.ResetColor();

            if (selected == 9) { Console.BackgroundColor = ConsoleColor.White; Console.ForegroundColor = ConsoleColor.Black; }
            Console.WriteLine("Slackbot Channel: " + crawl.slackBotChannel);
            Console.ResetColor();

            if (selected == 10) { Console.BackgroundColor = ConsoleColor.White; Console.ForegroundColor = ConsoleColor.Black; }
            Console.WriteLine("Slackbot Hook URL: " + crawl.slackBotHookURL);
            Console.ResetColor();

        }

        static bool selectTrueFalse(Crawling crawl, int selected, bool def)
        {
            bool ret = def;
            while (true)
            {
                printSettingsPage(crawl, selected);
                Console.WriteLine("\nSelect New Value:\n");
                if (ret) { Console.BackgroundColor = ConsoleColor.White; Console.ForegroundColor = ConsoleColor.Black; }
                Console.Write("TRUE");
                Console.ResetColor();
                Console.Write("   ");
                if (!ret) { Console.BackgroundColor = ConsoleColor.White; Console.ForegroundColor = ConsoleColor.Black; }
                Console.WriteLine("FALSE");
                Console.ResetColor();

                ConsoleKeyInfo keyInfo = Console.ReadKey();
                if (keyInfo.Key == ConsoleKey.LeftArrow) { if (!ret) { ret = true; } }
                if (keyInfo.Key == ConsoleKey.RightArrow) { if (ret) { ret = false; } }
                if (keyInfo.Key == ConsoleKey.Enter) { break; }
            }
            return ret;
           
        }

    }
}
