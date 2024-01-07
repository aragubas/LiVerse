using LiVerse.AnaBanUI;
using LiVerse.Stores;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiVerse.Screens; 
public class StartupScreen : ScreenBase {
  public UILayer? WindowRoot => null;
  Texture2D startupBannerTexture;
  double waitTimer = 0;
  double waitTime = 0.2;

  static readonly Color clearColor = Color.FromNonPremultiplied(110, 110, 200, 255);

  public StartupScreen(ScreenManager screenManager) : base(screenManager) {
#if DEBUG
    waitTime = 0.00016;
#endif

    startupBannerTexture = ResourceManager.LoadTexture2DFromFile(Path.Combine(ResourceManager.DefaultContentPath, "Images", "startup_banner.png"), DefaultColorProcessors.PremultiplyAlpha);
  }

  public override void Detach() {
    startupBannerTexture.Dispose();
  }

  public override void Dispose() { }

  public override void Draw(SpriteBatch spriteBatch, double deltaTime) {
    spriteBatch.GraphicsDevice.Clear(clearColor);

    Rectangle viewRect = new Rectangle(
      spriteBatch.GraphicsDevice.Viewport.Width / 2 - startupBannerTexture.Width / 2,
      spriteBatch.GraphicsDevice.Viewport.Height / 2 - startupBannerTexture.Height / 2,
      startupBannerTexture.Width,
      startupBannerTexture.Height);

    spriteBatch.Begin();

    spriteBatch.Draw(startupBannerTexture, viewRect, Color.White);

    spriteBatch.End();
  }

  void doStartup() {
    // Load Stores Settings
    SettingsStore.Load();
    CharacterStore.Load();
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
