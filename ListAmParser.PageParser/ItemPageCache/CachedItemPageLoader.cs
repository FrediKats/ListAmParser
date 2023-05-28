using System.Text.Json;
using ListAmParser.Abstractions;
using ListAmParser.Common;

namespace ListAmParser.PageParser.ItemPageCache;

public class CachedItemPageLoader : IItemPageLoader, IDisposable
{
    private const string ConfigFileName = "cache.json";

    private readonly IItemPageLoader _loader;
    private readonly PageCacheConfig _cacheConfig;
    private List<CachedItemPageLoaderElement>? _loadedCache;

    public CachedItemPageLoader(IItemPageLoader loader, PageCacheConfig cacheConfig)
    {
        _loader = loader;
        _cacheConfig = cacheConfig;
        _loadedCache = null;

        DirectoryExtensions.EnsureFileExists(cacheConfig.DirectoryPath);
        EnsureConfigFileCreated();
    }

    public string LoadContent(int itemId)
    {
        CachedItemPageLoaderElement? element = FindElement(itemId);
        if (element is not null && IsActual(element))
            return GetItemContentFromCache(element);

        string actualContent = _loader.LoadContent(itemId);

        element = new CachedItemPageLoaderElement(itemId, DateTime.Now);
        File.WriteAllText(GetItemPath(element.ItemId), actualContent);
        SaveElement(element);

        return actualContent;
    }

    private void EnsureConfigFileCreated()
    {
        if (File.Exists(GetConfigFullPath()))
            return;

        var cacheConfig = new List<CachedItemPageLoaderElement>();
        WriteCacheList(cacheConfig);
    }

    private CachedItemPageLoaderElement? FindElement(int itemId)
    {
        List<CachedItemPageLoaderElement> elements = ReadCacheList();
        CachedItemPageLoaderElement? element = elements.Find(e => e.ItemId == itemId);
        return element;
    }

    private void SaveElement(CachedItemPageLoaderElement element)
    {
        List<CachedItemPageLoaderElement> elements = ReadCacheList();
        elements.RemoveAll(e => e.ItemId == element.ItemId);
        elements.Add(element);
    }

    private List<CachedItemPageLoaderElement> ReadCacheList()
    {
        if (_loadedCache is not null)
            return _loadedCache;

        string configFileFullPath = GetConfigFullPath();
        var elements = JsonSerializer.Deserialize<List<CachedItemPageLoaderElement>>(File.ReadAllText(configFileFullPath));
        _loadedCache = elements ?? throw new ListAmException($"Cannot read config from {configFileFullPath}. Model was not serialized.");
        return elements;
    }

    private void WriteCacheList(List<CachedItemPageLoaderElement> elements)
    {
        string configFileFullPath = GetConfigFullPath();
        FileExtensions.EnsureFileExists(configFileFullPath);
        File.WriteAllText(configFileFullPath, JsonSerializer.Serialize(elements));
    }

    private string GetConfigFullPath()
    {
        return Path.Combine(_cacheConfig.DirectoryPath, ConfigFileName);
    }

    private string GetItemPath(int itemId)
    {
        return Path.Combine(_cacheConfig.DirectoryPath, $"{itemId}.html");
    }

    private bool IsActual(CachedItemPageLoaderElement element)
    {
        return element.LastUpdateTime.Add(_cacheConfig.InvalidateAfter) >= DateTime.Now;
    }

    private string GetItemContentFromCache(CachedItemPageLoaderElement element)
    {
        return File.ReadAllText(GetItemPath(element.ItemId));
    }

    public void Dispose()
    {
        if (_loadedCache is not null)
            WriteCacheList(_loadedCache);
    }
}