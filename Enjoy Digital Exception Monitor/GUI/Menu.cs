using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enjoy_Digital_Exception_Monitor.GUI
{
    class Menu
    {
        private IEnumerable<MenuItem> Listings;
        private IEnumerable<string> Menus;
        public Crawling crawl { get; set; }
        private int selected = 1;

        public Menu()
        {
            Listings = new List<MenuItem>().AsEnumerable();
            Menus = new List<string>().AsEnumerable();
        }

        public MenuItem newListing(string listingText, int type, string propertyName, string menu)
        {
            return new MenuItem { ListingText = listingText, Type = type, PropertyName = propertyName, Menu = menu, parent = this };
        }

        public void AddListing(MenuItem i)
        {
            List<MenuItem> l = Listings.ToList();
            l.Add(i);
            Listings = l.AsEnumerable<MenuItem>();
        }

        public void AddMenu(string s)
        {
            List<string> l = Menus.ToList();
            l.Add(s);
            Menus = l.AsEnumerable<string>();
        }

        public void RunMenu()
        {
            bool go = true;
            while (go)
            {
                PrintMenu();
                ConsoleKeyInfo keyPressed = Console.ReadKey();
                switch (keyPressed.Key)
                {
                    case (ConsoleKey.UpArrow):
                        if (selected > 1) { selected--; }
                        break;
                    case (ConsoleKey.DownArrow):
                        if (selected < Listings.Count()) { selected++; }
                        break;
                    case (ConsoleKey.Enter):
                        foreach (MenuItem item in Listings.Where(x => x.position == selected))
                        {
                            item.SelectValue(crawl);
                        }
                        break;
                    case (ConsoleKey.Escape):
                        go = false;
                        break;
                }
            }
        }

        public void PrintMenu()
        {
            Console.Clear();

            Console.WriteLine("Use Arrow Keys to select property and 'Esc' to exit\n");

            int position = 1;
            foreach (string menuTitle in Menus)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\n" + menuTitle);
                Console.ResetColor();
                foreach (MenuItem M in Listings.Where(x => x.Menu == menuTitle))
                {
                    M.position = position;
                    M.Show(crawl, selected);
                    position++;
                }
            }
        }

        public void save(string Filename)
        {
            if (File.Exists(Filename))
            {
                File.Delete(Filename);
            }

            StreamWriter sw = new StreamWriter(Filename);
            sw.WriteLine(Directory.GetCurrentDirectory());
            foreach (MenuItem item in Listings)
            {
                sw.WriteLine(item.crawl.GetType().GetProperty(item.PropertyName).GetValue(item.crawl));
            }
            sw.Close();
        }
    }
}
