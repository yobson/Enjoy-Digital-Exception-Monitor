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

        public void PrintMenu()
        {
            Console.Clear();
            int position = 1;
            foreach (string menuTitle in Menus)
            {
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
                sw.WriteLine(item.DefaltValue);
            }
            sw.Close();
        }
    }
}
