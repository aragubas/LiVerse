using LiVerse.Stores;
using SFML.Graphics;
using SFML.System;

namespace LiVerse.Screens;
public class StartupScreen : ScreenBase {
  Texture startupBannerTexture;
  double waitTimer = 0;
  double waitTime = 0.016;
  RectangleShape appLogo = new();

  static readonly Color clearColor = new Color(110, 62, 143, 255);

  public StartupScreen(ScreenManager screenManager) : base(screenManager) {
#if DEBUG
    // Only shows for 1 frame
    waitTime = 0.000000000016;
#endif

    startupBannerTexture = ResourceManager.LoadTextureFromFile(Path.Combine(ResourceManager.DefaultContentPath, "Images", "startup_banner.png"));
    appLogo.Texture = startupBannerTexture;

    appLogo.Size = new Vector2f(startupBannerTexture.Size.X, startupBannerTexture.Size.Y);
    appLogo.Origin = new Vector2f(appLogo.Size.X / 2, appLogo.Size.Y / 2);
  }

  public override void Detach() {
    startupBannerTexture.Dispose();
  }

  public override void Dispose() { }

  public override void Draw(RenderTarget target, RenderStates states) {
    target.Clear(clearColor);

    appLogo.Position = target.GetView().Center;

    target.Draw(appLogo);
  }

  void doStartup() {
    // Load Stores Settings
    SettingsStore.Load();
    CaptureDeviceDriverStore.Load();

    MainScreen mainScreen = new(ScreenManager);
    ScreenManager.AttachScreen(mainScreen);
  }

  public override void Update(double deltaTime) {
    if (waitTimer >= waitTime) {
      doStartup();

    } else {
      waitTimer += 1 * deltaTime;
    }
  }
}
