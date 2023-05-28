using HtmlAgilityPack;

namespace ListAmParser.PageParser.HtmlParsers;

public class HtmlSearchPageParser
{
    public List<int> ParseItemList(string content)
    {
        var doc = new HtmlDocument();
        doc.LoadHtml(content);

        HtmlNode itemList = doc
            .DocumentNode
            .Descendants("div")
            .Single(n => n.Id == "contentr")
            .ChildNodes
            .Single(d => d.HasClass("dl"));

        List<int> items = itemList
            .Descendants("a")
            .Select(a => a.GetAttributeValue("href", String.Empty))
            .Where(hrefLink => hrefLink.Contains("/item/"))
            .Select(a => a.Split("/").Last())
            .Select(int.Parse)
            .ToList();

        return items;
    }
}