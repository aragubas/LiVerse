using LiVerse.AnaBanUI;
using SFML.Graphics;

namespace LiVerse;
public class ScreenManager : Drawable {
  ScreenBase? CurrentScreen;
  public readonly UIRoot UIRoot;

  public ScreenManager(UIRoot uIRoot) {
    UIRoot = uIRoot;
  }

  /// <summary>
  /// De-attaches the current screen (if one is attached) and attaches a new one<br></br>This method forcibly calls the Garbage Collector.
  /// </summary>
  /// <param name="screen"></param>
  public void AttachScreen(ScreenBase screen) {
    if (CurrentScreen == null) {
      CurrentScreen = screen;
      return;
    }

    DetachScreen();
    CurrentScreen = screen;
  }

  public void DetachScreen() {
    if (CurrentScreen != null) {
      CurrentScreen.Detach();
      CurrentScreen.Dispose();

      GC.Collect();
    }
  }

  public void Update(double deltaTime) {
    CurrentScreen?.Update(deltaTime);
  }

  public void Draw(RenderTarget target, RenderStates states) {
    if (CurrentScreen == null) return;

    target.Draw(CurrentScreen);
  }
}
