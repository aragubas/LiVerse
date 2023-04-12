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
        element.Size = element.MinimumSize;
        element.RelativePosition = new Vector2(0, lastY);
        element.AbsolutePosition = AbsolutePosition + element.RelativePosition;
        
        if (element.Size.X > minimumWidth) minimumWidth = element.Size.X;
        lastY += element.Size.Y + Gap;
      }

      if (StretchElements) {
        foreach (var element in Elements) {
          element.Size = new Vector2(minimumWidth, element.Size.Y);
        }

      }

        MinimumSize = new Vector2(minimumWidth, 0);
    }

    public override void Draw(SpriteBatch spriteBatch, double deltaTime) {
      RecalculateUI();

      Viewport elementViewport = new Viewport((int)AbsolutePosition.X, (int)AbsolutePosition.Y, (int)Size.X, (int)Size.Y);
      Viewport oldViewport = spriteBatch.GraphicsDevice.Viewport;

      spriteBatch.End();
      spriteBatch.Begin();
      spriteBatch.GraphicsDevice.Viewport = elementViewport;

      //if (Lines) spriteBatch.DrawRectangle(new RectangleF(0, 0, MinimumSize.X, MinimumSize.Y), Color.Blue);

      // Draw Elements
      for (int i = 0; i < Elements.Count; i++) {
        DrawElement(spriteBatch, deltaTime, Elements[i]);
      }

      if (Lines) spriteBatch.DrawRectangle(new RectangleF(0, 0, Size.X, Size.Y), Color.Magenta);

      spriteBatch.End();

      //Restore SpriteBatch
      spriteBatch.GraphicsDevice.Viewport = oldViewport;
      spriteBatch.Begin();
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
