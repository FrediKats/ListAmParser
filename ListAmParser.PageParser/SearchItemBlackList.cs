using ListAmParser.Abstractions.Models;

namespace ListAmParser.PageParser;

public class SearchItemBlackList
{
    public bool IsAcceptable(ItemDescription item)
    {
        if (item.Price is null)
            return false;

        if (item.Price.Dollar > 2000)
            return false;

        if (item.Title.Contains("Хоренаци") || item.Title.Contains("Khorenatsi"))
            return false;

        if (item.Title.Contains("Цитернакаберд") || item.Address.Contains("Цитернакаберд"))
            return false;

        if (item.Title.Contains("Антарайин") || item.Address.Contains("Антарайин"))
            return false;

        if (item.Title.Contains("Антараин") || item.Address.Contains("Антарайин"))
            return false;

        if (item.Title.Contains("Лесная"))
            return false;

        if (item.AuthorName == "Օգտատեր")
            return false;

        return true;
    }
}