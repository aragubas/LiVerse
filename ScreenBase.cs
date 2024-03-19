using SFML.Graphics;

namespace LiVerse;
public abstract class ScreenBase : IDisposable, Drawable {
  protected ScreenManager ScreenManager { get; }

  public ScreenBase(ScreenManager screenManagerRef) {
    ScreenManager = screenManagerRef;
  }

  public abstract void Detach();
  public abstract void Update(double deltaTime);
  public abstract void Draw(RenderTarget target, RenderStates states);
  public abstract void Dispose();

}
