using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebScraper
{
    public class NavObject
    {
        public List<string> Directories { get; set; } = new();
        public List<string> SubLinks { get; set; } = new List<string>();
    }
}
