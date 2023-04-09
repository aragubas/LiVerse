using LiVerse.AnaBanUI;
using LiVerse.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System.Reflection;

namespace LiVerse {
  public class LiVerseApp : Game {
    GraphicsDeviceManager graphics { get; }
    SpriteBatch? spriteBatch;
    ScreenManager screenManager { get; }


    public LiVerseApp() {
      graphics = new GraphicsDeviceManager(this);
      screenManager = new ScreenManager();

      // Enables VSync
      graphics.SynchronizeWithVerticalRetrace = true;
      graphics.PreferHalfPixelOffset = true;
      graphics.ApplyChanges();

      IsMouseVisible = true;
      IsFixedTimeStep = false;

      InactiveSleepTime = TimeSpan.Zero;
    }

    protected override void Initialize() {
      Window.Title = $"LiVerse Alpha";
      Window.AllowUserResizing = true;

      // Creates the sprite batch
      spriteBatch = new SpriteBatch(graphics.GraphicsDevice);

      // Load base resources
      ResourceManager.LoadBaseResources(GraphicsDevice);

      // Attach MainScreen to ScreenManager
      screenManager.AttachScreen(new MainScreen());
    }

    protected override void Update(GameTime gameTime) {
      UIRoot.WindowFocused = IsActive;

      // Update UIRoot
      UIRoot.Update(gameTime.GetElapsedSeconds());

      screenManager.Update(gameTime.GetElapsedSeconds());
    }


    protected override void Draw(GameTime gameTime) {
      screenManager.Draw(spriteBatch, gameTime.GetElapsedSeconds());
    }

  }
}
