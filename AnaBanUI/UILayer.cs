using SFML.Graphics;

namespace LiVerse.AnaBanUI;

public class UILayer : Drawable {
  public Control? RootControl { get; set; }
  public UIRoot UIRoot { get; }

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
    target.Draw(RootControl, states);
  }
}