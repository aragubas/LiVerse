using LiVerse.AnaBanUI;
using LiVerse.Screens;
using LiVerse.Stores;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System.Reflection;

namespace LiVerse; 
public class LiVerseApp : Game {
  public static GraphicsDeviceManager? Graphics { get; set; }
  SpriteBatch spriteBatch;
  readonly ScreenManager screenManager;


#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
  public LiVerseApp() {
    Graphics = new GraphicsDeviceManager(this);
    screenManager = new ScreenManager();

    // Enables VSync
    Graphics.SynchronizeWithVerticalRetrace = true;
    Graphics.ApplyChanges();

    IsMouseVisible = true;
    IsFixedTimeStep = false;

    InactiveSleepTime = TimeSpan.Zero;
  }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

  protected override void OnExiting(object sender, EventArgs args) {
    Console.WriteLine("Goodbye!");
    screenManager.DetachScreen();

    SettingsStore.Save();
  }

  protected override void Initialize() {
    Window.Title = $"{ResourceManager.AppInfo.Name} v{ResourceManager.AppInfo.Version}";
#if DEBUG
    Window.Title += " {Debug}";
#endif

    Window.AllowUserResizing = true;
    Window.TextInput += OnWindowTextInput;

    // Creates the sprite batch
    if (Graphics == null) {
      throw new NullReferenceException("Could not create SpriteBatch, GraphicsDeviceManager is null");
    }
    spriteBatch = new SpriteBatch(Graphics.GraphicsDevice);

    // Load base resources
    ResourceManager.LoadBaseResources();

    // Attach MainScreen to ScreenManager
    screenManager.AttachScreen(new StartupScreen(screenManager));

  }

  void OnWindowTextInput(object? sender, TextInputEventArgs args) {
    UIRoot.UpdateWindowTextInput(args);
  }

  protected override void Update(GameTime gameTime) {
    UIRoot.WindowFocused = IsActive;

    // Update Microphone
    CaptureDeviceDriverStore.Update(gameTime.GetElapsedSeconds());

    // Update UIRoot
    UIRoot.Update(gameTime.GetElapsedSeconds());

    // Update Screen
    screenManager.Update(gameTime.GetElapsedSeconds());
  }


  protected override void Draw(GameTime gameTime) {
    screenManager.Draw(spriteBatch, gameTime.GetElapsedSeconds());
  }

}
