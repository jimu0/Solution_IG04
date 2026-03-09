using System.Reflection;
using IGC.CardCore_IG04.cfg;
using IGC.CardCore_IG04.Luban;

namespace IGC.CardCore_IG04;

public static class DeckFactory
{
    public static readonly Tables tables = new(LoadByteBuf);

    private static ByteBuf LoadByteBuf(string file)
    {
        string resourceName = $"CardCore_IG04.GameConfig.Bin.{file}.bytes";
        Assembly assembly = typeof(DeckFactory).Assembly;

        using Stream? stream = assembly.GetManifestResourceStream(resourceName);
        if (stream != null)
        {
            using var ms = new MemoryStream();
            stream.CopyTo(ms);
            return ByteBuf.Wrap(ms.ToArray());
        }

        string[] fallbackPaths =
        {
            Path.Combine(AppContext.BaseDirectory, "GameConfig", "Bin", $"{file}.bytes"),
            Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "GameConfig", "Bin", $"{file}.bytes"),
            Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "CardCore_IG04", "GameConfig", "Bin", $"{file}.bytes")
        };

        foreach (string path in fallbackPaths)
        {
            string fullPath = Path.GetFullPath(path);
            if (File.Exists(fullPath))
            {
                return ByteBuf.Wrap(File.ReadAllBytes(fullPath));
            }
        }

        throw new FileNotFoundException($"Cannot find config bytes for '{file}'. Resource: {resourceName}");
    }
    
}
