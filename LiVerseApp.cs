using LiVerse.AnaBanUI;
using LiVerse.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System.Reflection;

namespace LiVerse {
  public class LiVerseApp : Game {
    GraphicsDeviceManager graphics { get; }
    SpriteBatch spriteBatch;
    readonly ScreenManager screenManager;


#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
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
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    protected override void OnExiting(object sender, EventArgs args) {
      Console.WriteLine("Exit!");
      screenManager.DetachScreen();
    }

    protected override void Initialize() {
      Window.Title = $"LiVerse Alpha";
#if DEBUG
      Window.Title += " (Debug)";
#endif

      Window.AllowUserResizing = true;

      // Creates the sprite batch
      spriteBatch = new SpriteBatch(graphics.GraphicsDevice);

      // Load base resources
      ResourceManager.LoadBaseResources(GraphicsDevice);

      // Attach MainScreen to ScreenManager
#if DEBUG
      screenManager.AttachScreen(new MainScreen(screenManager));
#else
      screenManager.AttachScreen(new StartupScreen(screenManager));
#endif

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
