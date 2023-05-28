namespace ListAmParser.PageParser.HtmlLoaders;

public class CatalogPageLoader
{
    private readonly ListAmHttpClient _httpClient;
    private readonly SearchPageFilterUrlBuilder _filterUrlBuilder;

    public CatalogPageLoader(SearchPageFilterUrlBuilder filterUrlBuilder, ListAmHttpClient httpClient)
    {
        _filterUrlBuilder = filterUrlBuilder;
        _httpClient = httpClient;
    }

    public async Task<string> LoadPage(int page)
    {
        _filterUrlBuilder.SetPage(page);
        string url = _filterUrlBuilder.BuildUrl();
        HttpResponseMessage httpResponseMessage = await _httpClient.SendRequest(url);
        string responseContent = await httpResponseMessage.Content.ReadAsStringAsync();
        return responseContent;
    }
}