using LiVerse.AppInfo;
using SFML.Graphics;

namespace LiVerse;
public static class ResourceManager {
  public static string DefaultContentPath = Path.Combine(Environment.CurrentDirectory, "ApplicationData");
  public static string DefaultContentUserDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                                                                "LiVerse");
  public static string DefaultStoresPath = Path.Combine(DefaultContentUserDataPath, "Stores");
  public static Info AppInfo;

  //public static FontSystem GlobalFontSystem = new FontSystem();
  public static Dictionary<string, Font> FontCache = new();

  public static Font BakeFont(string fontName) {
    // Avoids common mistakes with Font file names
    string fontPath = Path.Combine(DefaultContentPath, "Fonts", fontName + ".ttf");
    if (fontPath.EndsWith(".ttf.ttf")) fontPath = fontPath.Replace(".ttf.ttf", ".ttf");

    Font newFont = new(fontPath);

    return newFont;
  }

  /// <summary>
  /// Gets font from FontCache, bake font if not found in cache
  /// </summary>
  public static Font GetFont(string fontName) {
    string fontKey = fontName.ToLower();

    if (FontCache.TryGetValue(fontKey, out Font? foundValue)) {
      return foundValue;
    }

    Font newFont = BakeFont(fontName);
    FontCache.Add(fontKey, newFont);

    return newFont;
  }

  public static void LoadBaseResources() {
    // Create Directories
    Directory.CreateDirectory(DefaultContentUserDataPath);
    Directory.CreateDirectory(DefaultStoresPath);

    // Check if OpenSans font exists
    string openSansFontPath = Path.Combine(DefaultContentPath, "Fonts", "NotoSans.ttf");
    if (!File.Exists(openSansFontPath)) {
      throw new FileNotFoundException("Could not find default font \"NotoSans.ttf\".");
    }
  }

  // Load Texture From File
  public static Texture LoadTextureFromFile(string filePath) {
    if (!File.Exists(filePath)) {
      throw new FileNotFoundException($"Could not find Texture. Path: {filePath}");
    }

    Texture newTexture = new(filePath);

    return newTexture;
  }

}