using Microsoft.Xna.Framework;

namespace LiVerse.AnaBanUI; 

// TODO: Make a proper theming system for AnaBan
internal static class ColorScheme {

  // Backgrounds
  public static readonly Color BackgroundLevel0 = Color.FromNonPremultiplied(43, 33, 54, 255);
  public static readonly Color ForegroundLevel0 = Color.FromNonPremultiplied(61, 46, 77, 255);
  public static readonly Color ForegroundLevel1 = Color.FromNonPremultiplied(91, 68, 115, 255);

  // Controls
  public static readonly Color ControlBackgroundDisabled = Color.FromNonPremultiplied(70, 39, 92, 255);
  public static readonly Color ControlBackground = Color.FromNonPremultiplied(110, 62, 148, 255);
  public static readonly Color ControlForegroundHover = Color.FromNonPremultiplied(135, 100, 184, 255);
  public static readonly Color ControlForegroundActive = Color.FromNonPremultiplied(163, 120, 200, 255);
  public static readonly Color ControlHightlight = Color.FromNonPremultiplied(194, 142, 238, 255);

  // Accent Colors
  public static readonly Color RedAccent = Color.FromNonPremultiplied(237, 62, 65, 255);

  // Text
  public static readonly Color TextDisabled = Color.FromNonPremultiplied(200, 200, 200, 127);
  public static readonly Color TextNormal = Color.FromNonPremultiplied(235, 235, 235, 255);
  public static readonly Color TextTitle = Color.FromNonPremultiplied(240, 240, 240, 255);
  public static readonly Color TextActive = Color.FromNonPremultiplied(250, 250, 250, 255);
}
