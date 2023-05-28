using System.Globalization;
using HtmlAgilityPack;
using ListAmParser.Abstractions;
using ListAmParser.Abstractions.Models;

namespace ListAmParser.PageParser.HtmlParsers;

public class HtmlItemPageParser
{
    public ItemDescription Parse(string content)
    {
        // TODO: skip if page does not exist anymore

        var doc = new HtmlDocument();
        doc.LoadHtml(content);

        List<HtmlNode> docAllDivs = doc
            .DocumentNode
            .Descendants("div")
            .ToList();

        HtmlNode? footer = docAllDivs
            .FirstOrDefault(n => n.HasClass("footer"));

        if (footer is null)
            throw new ListAmException("Cannot find footer");


        string itemIdString = footer.ChildNodes[0].InnerText;
        int itemId = int.Parse(itemIdString.Split(" ").Last());

        DateOnly lastUpdateTime = ParseLastUpdateTime(footer);

        List<HtmlNode> userInfos = doc
            .DocumentNode
            .Descendants("a")
            .Where(HasLinkToUserProfile)
            .ToList();

        if (userInfos.Count != 1)
            throw new ListAmException($"Cannot get user info content for item {itemId}. Link count {userInfos.Count}");

        string? authorName = userInfos
            .Single()
            .ChildNodes
            .FindFirst("div")
            .InnerHtml;

        string itemTitle = docAllDivs
            .Single(d => d.HasClass("vih"))
            .ChildNodes
            .FindFirst("h1")
            .InnerHtml;

        string address = docAllDivs
            .Single(d => d.HasClass("loc"))
            .ChildNodes
            .FindFirst("a")
            .InnerHtml;

        ItemPrice? itemPrice = ParsePrice(doc);

        HtmlNode roomInfoNode = docAllDivs
            .Where(d => d.HasClass("c"))
            .Single(d => d.ChildNodes.Any(c => c.InnerHtml == "Количество комнат"));

        HtmlNode roomNumberNode = roomInfoNode
            .ChildNodes
            .First(c => c.HasClass("i"));
        int roomCount = int.Parse(roomNumberNode.InnerHtml);

        bool? hasDishWasher = null;

        string dishWasherDescription = docAllDivs
            .Single(d => d.GetAttributeValue("itemprop", String.Empty) == "description")
            .InnerHtml;

        HtmlNode? dishWasherNode = docAllDivs
            .Where(d => d.HasClass("c"))
            .SingleOrDefault(d => d.ChildNodes.Any(c => c.InnerHtml == "Бытовая техника"));

        string? dishWasherInfo = dishWasherNode
            ?.ChildNodes
            .Single(c => c.HasClass("i"))
            .InnerHtml;

        if (dishWasherDescription.Contains("посудомо"))
            hasDishWasher = true;

        if(dishWasherInfo is not null
           && dishWasherInfo.Contains("посудомо"))
            hasDishWasher = true;

        return new ItemDescription(
            itemId,
            authorName,
            lastUpdateTime,
            itemTitle,
            address,
            roomCount,
            hasDishWasher,
            itemPrice,
            userInfos.Single().GetAttributeValue("href", String.Empty));
    }

    private static bool HasLinkToUserProfile(HtmlNode n)
    {
        if (!n.HasAttributes)
            return false;
        
        string attributeValue = n.GetAttributeValue("href", string.Empty);
        return attributeValue.Contains("/user/")
               || attributeValue.Contains("/u/");
    }

    private static DateOnly ParseLastUpdateTime(HtmlNode footer)
    {
        DateOnly lastUpdateTime;
        if (footer.ChildNodes.Count > 2)
        {
            string updateTime = footer.ChildNodes[2].InnerText.Split(" ").ElementAt(1);
            lastUpdateTime = DateOnly.ParseExact(updateTime, "dd.mm.yyyy", CultureInfo.CurrentCulture);
        }
        else
        {
            string creationTimeString = footer.ChildNodes[1].InnerText.Split(" ").Last();
            lastUpdateTime = DateOnly.ParseExact(creationTimeString, "dd.mm.yyyy", CultureInfo.CurrentCulture);
        }

        return lastUpdateTime;
    }

    private ItemPrice? ParsePrice(HtmlDocument doc)
    {
        HtmlNode? priceNode = doc
            .DocumentNode
            .Descendants("span")
            .SingleOrDefault(s => s.HasClass("xprice"));

        if (priceNode is null)
            return null;

        List<HtmlNode> nodes = priceNode
            .Descendants("span")
            .ToList();

        string dollarString = nodes
            .Single(n => n.InnerHtml.Contains("$"))
            .InnerHtml
            .Split(" ")
            .First()
            .Substring(1)
            .Replace(",", "");

        string rubString = nodes
            .Single(n => n.InnerHtml.Contains("₽"))
            .InnerHtml
            .Split(" ")
            .First()
            .Replace(",", "");

        string other = nodes
            .Single(n => n.InnerHtml.Contains("֏"))
            .InnerHtml
            .Split(" ")
            .First()
            .Replace(",", "");

        return new ItemPrice(
            double.Parse(rubString),
            double.Parse(dollarString),
            double.Parse(other));
    }
}