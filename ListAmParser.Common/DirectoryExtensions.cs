namespace ListAmParser.Common;

public static class DirectoryExtensions
{
    public static void EnsureFileExists(string path)
    {
        if (path is null)
            throw new ArgumentNullException(nameof(path));

        if (Directory.Exists(path))
            return;

        Directory.CreateDirectory(path);
    }
}