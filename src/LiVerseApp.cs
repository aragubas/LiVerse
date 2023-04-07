using LiVerse.AnaBanUI;
using LiVerse.AnaBanUI.Containers;
using LiVerse.AnaBanUI.Controls;
using LiVerse.src;
using LiVerse.src.AnaBanUI;
using LiVerse.src.AnaBanUI.Controls;
using LiVerse.src.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;

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
      graphics.ApplyChanges();

      IsMouseVisible = true;
      IsFixedTimeStep = false;

      InactiveSleepTime = TimeSpan.Zero;
    }

    protected override void Initialize() {
      Window.Title = "LiVerse Alpha";
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
      GraphicsDevice.Clear(Color.CornflowerBlue);

      screenManager.Draw(spriteBatch, gameTime.GetElapsedSeconds());
    }

  }
}
