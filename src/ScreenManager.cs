using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiVerse.src {
  public class ScreenManager {
    IScreen? CurrentScreen;

    public ScreenManager() {
    }

    /// <summary>
    /// De-attaches the current screen (if one is attached) and attaches a new one<br></br>This method forcibly calls the Garbage Collector.
    /// </summary>
    /// <param name="screen"></param>
    public void AttachScreen(IScreen screen) {
      if (CurrentScreen == null) {
        CurrentScreen = screen;
        return;
      }

      CurrentScreen.Deattach();
      CurrentScreen = screen;

      GC.Collect();
    }

    public void Update(double deltaTime) {
      CurrentScreen?.Update(deltaTime);
    }

    public void Draw(SpriteBatch spriteBatch, double deltaTime) {
      CurrentScreen?.Draw(spriteBatch, deltaTime);
    }

    
  }
}
