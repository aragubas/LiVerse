using FontStashSharp;
using Microsoft.Xna.Framework.Graphics;

namespace LiVerse {
  public static class ResourceManager {
    public static string DefaultContentPath = Path.Combine(Environment.CurrentDirectory, "ApplicationData");
    public static string DefaultContentUserDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), 
                                                                  "LiVerse");
    public static FontSystem GlobalFontSystem = new FontSystem();

    public static void LoadBaseResources(GraphicsDevice graphicsDevice) {
      // Check if OpenSans font exists
      string openSansFontPath = Path.Combine(DefaultContentPath, "Fonts", "OpenSans.ttf");
      if (!File.Exists(openSansFontPath)) {
        throw new FileNotFoundException("Could not find default font \"OpenSans.ttf\".");
      }

      GlobalFontSystem.AddFont(File.ReadAllBytes(openSansFontPath));      
    }

  }
}