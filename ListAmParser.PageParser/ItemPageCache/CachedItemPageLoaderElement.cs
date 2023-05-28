namespace ListAmParser.PageParser.ItemPageCache;

public class CachedItemPageLoaderElement
{
    public int ItemId { get; set; }
    public DateTime LastUpdateTime { get; set; }

    public CachedItemPageLoaderElement(int itemId, DateTime lastUpdateTime)
    {
        ItemId = itemId;
        LastUpdateTime = lastUpdateTime;
    }
}