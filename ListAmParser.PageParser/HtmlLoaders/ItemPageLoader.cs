namespace ListAmParser.PageParser.HtmlLoaders;

public class ItemPageLoader : IItemPageLoader
{
    private readonly ListAmHttpClient _httpClient;

    public ItemPageLoader(ListAmHttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public string LoadContent(int itemId)
    {
        HttpResponseMessage httpResponseMessage = _httpClient.SendRequest("https://www.list.am/ru/item/" + itemId).Result;
        return httpResponseMessage.Content.ReadAsStringAsync().Result;
    }
}