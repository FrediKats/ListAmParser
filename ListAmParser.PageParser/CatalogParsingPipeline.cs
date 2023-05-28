using ListAmParser.Abstractions.Models;
using ListAmParser.PageParser.HtmlParsers;
using ListAmParser.PageParser.HtmlLoaders;
using ListAmParser.PageParser.ItemPageCache;

namespace ListAmParser.PageParser;

public class CatalogParsingPipeline : IDisposable
{
    private readonly HtmlSearchPageParser _searchPageParser;
    private readonly HtmlItemPageParser _itemPageParser;
    private readonly CachedItemPageLoader _itemPageLoader;
    private readonly SearchItemBlackList _searchItemBlackList;

    public CatalogParsingPipeline()
    {
        _searchItemBlackList = new SearchItemBlackList();
        _itemPageLoader = new CachedItemPageLoader(new ItemPageLoader(ListAmHttpClient.Instance), new PageCacheConfig("cache", TimeSpan.FromDays(3)));
        _searchPageParser = new HtmlSearchPageParser();
        _itemPageParser = new HtmlItemPageParser();
    }

    public async Task<List<ItemDescription>> Process(SearchPageFilterUrlBuilder buildUrl, int pageForParse)
    {
        var catalogPageLoader = new CatalogPageLoader(buildUrl, ListAmHttpClient.Instance);

        var result = new List<ItemDescription>();

        for (int pageNum = 0; pageNum < pageForParse; pageNum++)
        {
            string pageContent = await catalogPageLoader.LoadPage(pageNum);
            List<int> itemOnPage = _searchPageParser.ParseItemList(pageContent);
            foreach (int itemId in itemOnPage)
            {
                Console.WriteLine($"Scan item {itemId}");
                string content = _itemPageLoader.LoadContent(itemId);
                ItemDescription itemDescription = _itemPageParser.Parse(content);

                if (_searchItemBlackList.IsAcceptable(itemDescription))
                    result.Add(itemDescription);
            }
        }

        return result
            .OrderBy(i => i.Price.Dollar)
            .ToList();
    }

    public void Dispose()
    {
        _itemPageLoader.Dispose();
    }
}