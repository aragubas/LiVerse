using Microsoft.Xna.Framework.Graphics;
using SpriteFontPlus;

namespace LiVerse {
  public static class ResourceManager {
    public static string DefaultContentPath = Path.Combine(Environment.CurrentDirectory, "ApplicationData");
    public static string DefaultContentUserDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                                                                  "LiVerse");
    //public static FontSystem GlobalFontSystem = new FontSystem();
    public static Dictionary<string, SpriteFont> FontCache = new();

    public static SpriteFont BakeFont(string fontName, int size, GraphicsDevice graphicsDevice) {
      string fontPath = Path.Combine(DefaultContentPath, "Fonts", fontName + ".ttf");
      if (fontPath.EndsWith(".ttf.ttf")) fontPath.Replace(".ttf.ttf", ".ttf");

      TtfFontBakerResult fontBakeResult = TtfFontBaker.Bake(File.ReadAllBytes(fontPath),
        size, 1024, 1024, new[] {
          CharacterRange.BasicLatin,
          CharacterRange.Latin1Supplement,
          CharacterRange.LatinExtendedA,
          CharacterRange.Cyrillic
        });

      return fontBakeResult.CreateSpriteFont(graphicsDevice);
    }

    /// <summary>
    /// Gets font from FontCache, bake font if not found in cache
    /// </summary>
    public static SpriteFont GetFont(string fontName, int size, GraphicsDevice graphicsDevice) {
      string fontKey = $"{fontName}:{size}";

      if (FontCache.TryGetValue(fontKey, out SpriteFont foundValue)) {
        return foundValue;
      }

      SpriteFont bakeResult = BakeFont(fontName, size, graphicsDevice);
      FontCache.Add(fontKey, bakeResult);

      return bakeResult;
    }

    public static void LoadBaseResources(GraphicsDevice graphicsDevice) {
      // Check if OpenSans font exists
      string openSansFontPath = Path.Combine(DefaultContentPath, "Fonts", "OpenSans.ttf");
      if (!File.Exists(openSansFontPath)) {
        throw new FileNotFoundException("Could not find default font \"OpenSans.ttf\".");
      }
    }

    // Load Sprite From File
    public static Texture2D LoadTexture2DFromFile(GraphicsDevice graphicsDevice, string FilePath) {
      if (!File.Exists(FilePath)) {
        throw new FileNotFoundException($"Could not find Sprite to load. Path: {FilePath}");
      }

      using (FileStream fileStream = new FileStream(FilePath, FileMode.Open)) {
        Texture2D ValToReturn = Texture2D.FromStream(graphicsDevice, fileStream);
        fileStream.Dispose();

        return ValToReturn;
      }
    }

  }
}