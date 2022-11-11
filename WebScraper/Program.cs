using HtmlAgilityPack;
using WebScraper;
using Downloader;

internal class Program
{
    private static async Task Main(string[] args)
    {
        string url = "https://sesterfarmsphotos.squarespace.com";
        var categories = AllSubLinks(url);
        await ScrapeImageObjects(url, categories);

        static NavObject AllSubLinks(string url)
        {
            var doc = GetDocument(url);
            var subNodes = doc.DocumentNode.SelectNodes("/html/body/div/div/div/header/div/nav/ul/li/div/ul/li/a");
            var nav = new NavObject();

            foreach (var subNode in subNodes)
                nav.SubLinks.Add(subNode.InnerText);
            
            return nav;
        }

        async Task ScrapeImageObjects(string url, NavObject categories)
        {
            var downloadOpt = new DownloadConfiguration()
            {
                ChunkCount = 1, // file parts to download, default value is 1
                ParallelDownload = true // download parts of file as parallel or not. Default value is false
            };
            var downloader = new DownloadService(downloadOpt);

            foreach (var subLink in categories.SubLinks) {

                var path = @$"C:\Users\adrion\Desktop\Scrape\{subLink}\";
                Directory.CreateDirectory(path);
                var fullUrl = url + $"/{subLink.ToLower()}";
                var doc = GetDocument(fullUrl);

                var imageNodes = doc.DocumentNode.SelectNodes("//img");
                if (imageNodes == null) continue;
                foreach (var imageNode in imageNodes)
                {
                    var photoObject = new PhotoObject();
                    photoObject.Id = imageNode.GetAttributeValue("data-image-id", "");
                    photoObject.Url = imageNode.GetAttributeValue("data-src", "");

                    if (string.IsNullOrEmpty(photoObject.Url))
                        photoObject.Url = imageNode.GetAttributeValue("data-img", "");

                    if (string.IsNullOrEmpty(photoObject.Url))
                        photoObject.Url = imageNode.GetAttributeValue("src", "");

                    photoObject.Description = imageNode.GetAttributeValue("alt", "");
                    if (string.IsNullOrEmpty(photoObject.Description)) photoObject.Description = imageNodes.IndexOf(imageNode).ToString();

                    var s2 = photoObject.Description.Split('#')[0];

                    await downloader.DownloadFileTaskAsync(photoObject.Url, path + s2.Replace(" ", "") + ".jpg");

                    Console.WriteLine($"{photoObject.Description}");
                }               
            }
        }

        static HtmlDocument GetDocument(string url)
        {
            HtmlWeb web = new HtmlWeb();
            return web.Load(url);
        }
    }
}