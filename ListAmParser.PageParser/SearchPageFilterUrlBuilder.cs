using System.Text;
using ListAmParser.PageParser.HtmlLoaders;
using ListAmParser.PageParser.SearchPageItemFilters;

namespace ListAmParser.PageParser;

public class SearchPageFilterUrlBuilder
{
    private readonly List<ISearchPageItemFilter> _filters;
    private int _page;

    public SearchPageFilterUrlBuilder()
    {
        _filters = new List<ISearchPageItemFilter>();
        _page = 1;
    }

    public SearchPageFilterUrlBuilder SetPage(int page)
    {
        _page = page;
        return this;
    }

    public SearchPageFilterUrlBuilder Add(ISearchPageItemFilter filter)
    {
        _filters.Add(filter);
        return this;
    }

    public string BuildUrl()
    {
        var builder = new StringBuilder();
        builder.Append(ListAmConstants.UrlPrefix);
        builder.Append($"/{_page}");
        builder.Append("?");
        builder.AppendJoin("&", _filters.Select(f => f.PrepareArgument()));
        return builder.ToString();
    }
}