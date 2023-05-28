namespace ListAmParser.PageParser.ItemPageCache;

public record PageCacheConfig(string DirectoryPath, TimeSpan InvalidateAfter);