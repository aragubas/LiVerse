using Microsoft.Xna.Framework.Graphics;

namespace LiVerse {
  public class ScreenManager {
    ScreenBase? CurrentScreen;

    public ScreenManager() {
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

    public void Draw(SpriteBatch spriteBatch, double deltaTime) {
      CurrentScreen?.Draw(spriteBatch, deltaTime);
    }


  }
}
