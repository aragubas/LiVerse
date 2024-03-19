using SFML.Graphics;
using SFML.Window;

namespace LiVerse.AnaBanUI;

public class UIRoot : Drawable {
  readonly Window window;
  List<UILayer> layerStack = new();

  public UIRoot(Window window) {
    this.window = window;

  }

  /// <summary>
  /// Adds an UILayer to the top of the stack
  /// </summary>
  public void PushUILayer(UILayer layer) {
    layerStack.Add(layer);
  }


  public void Update(double deltaTime) {
    foreach (UILayer layer in layerStack) {
      layer.Update(deltaTime);
    }
  }

  public void Draw(RenderTarget target, RenderStates states) {
    foreach (UILayer layer in layerStack) {
      target.Draw(layer);
    }
  }
}