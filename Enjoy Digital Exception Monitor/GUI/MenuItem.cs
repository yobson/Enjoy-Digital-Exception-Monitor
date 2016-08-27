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
        //public string Value = "";
        public const int String = 0;
        public const int Bool = 1;
        public const int Int = 2;
        public int position;

        public Crawling SelectValue(Crawling crawl)
        {
            //Menu parent = (Parent)this;
            string newValue;
            switch (this.Type)
            {
                case (0): //If String
                    Console.Write("\nNew Value: ");
                    Console.ForegroundColor = ConsoleColor.Green;
                    newValue = Console.ReadLine();
                    crawl.GetType().GetProperty(PropertyName).SetValue(crawl, newValue);
                    break;

                case (1): // If bool
                    
                    break;

                case (2): // If Int
                    Console.Write("\nNew Value: ");
                    Console.ForegroundColor = ConsoleColor.Green;
                    newValue = Console.ReadLine();
                    crawl.GetType().GetProperty(PropertyName).SetValue(crawl, int.Parse(newValue));
                    break;

            }
            return crawl;
        }

        public void Show(Crawling crawl, int selected)
        {
            if (selected == position) { Console.BackgroundColor = ConsoleColor.White; Console.ForegroundColor = ConsoleColor.Black; }
            Console.WriteLine(ListingText + ": " + crawl.GetType().GetProperty(PropertyName).GetValue(crawl));
            Console.ResetColor();
        }
    }
}
