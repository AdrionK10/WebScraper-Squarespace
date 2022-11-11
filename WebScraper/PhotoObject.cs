using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebScraper
{
    public class PhotoObject
    {
        public string Id { get; set; } = "";
        public string Url { get; set; } = "";
        public string Description { get; set; } = "";
        public byte[]? Image { get; set; }
    }
}
