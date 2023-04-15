using LiVerse.AnaBanUI.Events;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace LiVerse.AnaBanUI.Containers {
  public class ScrollableList : ContainerBase {
    public List<ControlBase> Elements { get; } = new();
    public bool Lines { get; set; } = false;
    public bool StretchElements { get; set; } = true;
    public float Gap { get; set; }

    public ScrollableList() {
      
    }

    void RecalculateUI() {
      if (Elements.Count == 0) { return; }

      float minimumWidth = 0;
      float lastY = 0;

      foreach(var element in Elements) {
        element.Size = new Vector2(element.MinimumSize.X + element.Margin, element.MinimumSize.Y + element.Margin);
        element.RelativePosition = new Vector2(0, lastY);
        element.AbsolutePosition = AbsolutePosition + element.RelativePosition;
        element.ParentControl = this;
        if (element.Size.X > minimumWidth) minimumWidth = element.Size.X + Margin;

        lastY += element.Size.Y + Gap;
      }

      if (StretchElements) {
        foreach (var element in Elements) {
          element.Size = new Vector2(minimumWidth - Margin, element.Size.Y);
        }

      }

      MinimumSize = new Vector2(minimumWidth, 0);
    }

    public override void DrawElement(SpriteBatch spriteBatch, double deltaTime) {
      RecalculateUI();

      // Draw Elements
      for (int i = 0; i < Elements.Count; i++) {
        Elements[i].Draw(spriteBatch, deltaTime);
      }
    }

    public override bool InputUpdate(PointerEvent pointerEvent) {
      // Nothing to Update
      if (Elements.Count == 0) { return false; }

      for (int i = 0; i < Elements.Count; i++) {
        if (Elements[i].InputUpdate(pointerEvent)) return true;
      }

      return false;
    }

    public override void Update(double deltaTime) {
      // Nothing to Update
      if (Elements.Count == 0) { return; }

      for (int i = 0; i < Elements.Count; i++) {
        Elements[i].Update(deltaTime);
      }
    }
  }
}
