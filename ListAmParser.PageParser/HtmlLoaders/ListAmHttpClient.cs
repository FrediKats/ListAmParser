using System.Net;

namespace ListAmParser.PageParser.HtmlLoaders;

public class ListAmHttpClient
{
    private static readonly TimeSpan Delay = TimeSpan.FromMilliseconds(600);

    public static ListAmHttpClient Instance { get; private set; } = new ListAmHttpClient();

    public async Task<HttpResponseMessage> SendRequest(string link)
    {
        Thread.Sleep(Delay);

        var cookieContainer = new CookieContainer();
        using var handler = new HttpClientHandler { CookieContainer = cookieContainer };
        using var client = new HttpClient(handler) { BaseAddress = ListAmConstants.BaseAddress };

        var request = new HttpRequestMessage(HttpMethod.Get, link);
        client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (compatible; AcmeInc/1.0)");
        //request.Headers.UserAgent.Add(new ProductInfoHeaderValue("Mozilla/5.0 (X11; Ubuntu; Linux x86_64; rv:97.0) Gecko/20100101 Firefox/97.0"));
        cookieContainer.Add(ListAmConstants.BaseAddress, new Cookie("CookieName", "cookie_value"));

        return await client.SendAsync(request);
    }
}