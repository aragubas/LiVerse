using SFML.Graphics;
using SFML.System;

namespace LiVerse.AnaBanUI;

public class UILayer : Drawable {
  public Control? RootControl { get; set; }
  public UIRoot UIRoot { get; }
  static readonly Vector2f zero = new Vector2f(0, 0);

  public UILayer(UIRoot uIRoot) {
    UIRoot = uIRoot;
  }

  public void Update(double deltaTime) {
    if (RootControl == null) return;

    RootControl.Update(deltaTime);
  }

  public void Draw(RenderTarget target, RenderStates states) {
    if (RootControl == null) return;

    RootControl.Size = target.GetView().Size;
    RootControl.RelativePosition = zero;
    RootControl.AbsolutePosition = zero;

    target.Draw(RootControl, states);
  }
}