using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enjoy_Digital_Exception_Monitor
{

    public class PageSpeedEntity
    {
        public string kind { get; set; }
        public string id { get; set; }
        public int responseCode { get; set; }
        public string title { get; set; }
        public int score { get; set; }
        public PageStats pageStats { get; set; }
    }

    public class PageStats
    {
        public int numberResources { get; set; }
        public int numberHosts { get; set; }
        public double totalRequestBytes { get; set; }
        public int numberStaticResources { get; set; }
        public double htmlResponseBytes { get; set; }
        public double cssResponseBytes { get; set; }
        public double imageResponseBytes { get; set; }
        public double javascriptResponseBytes { get; set; }
        public int numberJsResources { get; set; }
        public int numberCssResources { get; set; }
    }
}
