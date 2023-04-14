using LiVerse.AnaBanUI;
using Microsoft.Xna.Framework.Graphics;

namespace LiVerse {
  public abstract class ScreenBase : IDisposable {
    protected ScreenManager ScreenManager { get; }

    public ScreenBase(ScreenManager screenManagerRef) {
      ScreenManager = screenManagerRef;
    }     

    public abstract void Deattach();
    public abstract void Update(double deltaTime);
    public abstract void Draw(SpriteBatch spriteBatch, double deltaTime);
    public abstract void Dispose();
    
  }
}
