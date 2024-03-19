using System.Runtime.CompilerServices;
using LiVerse.AnaBanUI;
using LiVerse.Screens;
using LiVerse.Stores;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace LiVerse;
public class LiVerseApp : IDisposable {
  readonly ScreenManager screenManager;
  readonly UIRoot uIRoot;
  RenderWindow window;
  View mainView;

  public LiVerseApp() {
    VideoMode videoMode = new() {
      Width = 1024,
      Height = 600
    };

    ContextSettings contextSettings = new() {
      AntialiasingLevel = 8
    };

    window = new(videoMode, "LiVerse", Styles.Resize | Styles.Close, contextSettings);

    // Wire up Events
    window.Closed += OnClosed;
    window.Resized += OnResized;

    // Set Window properties
    window.SetVerticalSyncEnabled(true);

    mainView = window.GetView();

    // Set up Screens and UIRoot
    uIRoot = new(window);
    screenManager = new(uIRoot);
  }

  void OnClosed(object? sender, EventArgs args) {
    Console.WriteLine("Goodbye!");
    screenManager.DetachScreen();

    SettingsStore.Save();

    window.Close();
  }

  void OnResized(object? sender, SizeEventArgs args) {
    mainView.Reset(new FloatRect(0, 0, args.Width, args.Height));
    window.SetView(mainView);
  }

  public void Run() {
    Initialize();

    Clock deltaClock = new();

    while (window.IsOpen) {
      double deltaTime = deltaClock.Restart().AsSeconds();

      // Handle OS Stuff
      window.DispatchEvents();

      // First update, then draw
      Update(deltaTime);
      Draw(deltaTime);
    }
  }

  void Initialize() {
    // Load base resources
    ResourceManager.LoadBaseResources();

    // Attach MainScreen to ScreenManager
    screenManager.AttachScreen(new StartupScreen(screenManager));
  }


  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  void Update(double delta) {
    // Update Microphone
    CaptureDeviceDriverStore.Update(delta);

    // Update Screen
    screenManager.Update(delta);
  }

  void Draw(double delta) {
    window.Clear(Color.Transparent);

    window.Draw(screenManager);

    window.Display();
  }

  public void Dispose() {
    window.Dispose();
  }
}
