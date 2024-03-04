using LiVerse.AnaBanUI;

namespace LiVerse.Screens.MainScreenNested; 

public abstract class NestedScreen {
  public UILayer RootLayer = new();
  
  public virtual void ToggleUILayer() {
    if (UIRoot.UILayers.Contains(RootLayer)) {
      UIRoot.UILayers.Remove(RootLayer);
      return;
    }

    UIRoot.UILayers.Add(RootLayer);
  }
}
