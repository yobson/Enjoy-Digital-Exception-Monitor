using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enjoy_Digital_Exception_Monitor.GUI
{
    class MenuItem : Menu
    {
        public string ListingText { get; set; }
        public string PropertyName { get; set; }
        public int Type { get; set; }
        public string Menu { get; set; }
        public string DefaltValue = "";
        public const int String = 0;
        public const int Bool = 1;
        public int position;

        public Crawling ChangeValue(Crawling crawl)
        {
            switch (this.Type)
            {
                case (0): //If String
                    Console.Write("\nNew Value: ");
                    Console.ForegroundColor = ConsoleColor.Green;
                    string newValue = Console.ReadLine();
                    crawl.GetType().GetProperty(PropertyName).SetValue(crawl, newValue);
                    break;
                case (1):
                    break;
            }
            return crawl;
        }

        public void Show(Crawling crawl, int selected)
        {
            if (selected == position) { Console.BackgroundColor = ConsoleColor.White; Console.ForegroundColor = ConsoleColor.Black; }
            Console.WriteLine(ListingText + ": " + crawl.GetType().GetProperty(PropertyName).ToString());
            Console.ResetColor();
            Console.ResetColor();
        }
    }
}
