using LiVerse.AnaBanUI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiVerse.Screens {
  public class StartupScreen : ScreenBase {
    public WindowRoot? WindowRoot => null;
    Texture2D? startupBannerTexture;
    double waitTimer = 0;

    static readonly Color clearColor = Color.FromNonPremultiplied(149, 148, 204, 255);

    public StartupScreen(ScreenManager screenManager) : base(screenManager) {

    }

    public override void Deattach() {
      if (startupBannerTexture != null) {
        startupBannerTexture.Dispose();
      }
    }

    public override void Draw(SpriteBatch spriteBatch, double deltaTime) {
      if (startupBannerTexture == null) {
        startupBannerTexture = ResourceManager.LoadTexture2DFromFile(spriteBatch.GraphicsDevice, Path.Combine(ResourceManager.DefaultContentPath, "Images", "startup_banner.png"), DefaultColorProcessors.PremultiplyAlpha);
      }

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
      MainScreen mainScreen = new(ScreenManager);
      ScreenManager.AttachScreen(mainScreen);
    }

    public override void Update(double deltaTime) {
      if (waitTimer > 0.25) {
        doStartup();

      } else {
        waitTimer += 1 * deltaTime;
      }
    }
  }
}
