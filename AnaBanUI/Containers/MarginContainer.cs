using LiVerse.AnaBanUI.Drawables;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LiVerse.AnaBanUI.Containers {
  public class MarginContainer : ContainerBase {
    public ControlBase? FillElement { get; set; }
    public RectangleDrawable? BackgroundRectDrawable { get; set; }
    public float Margin { get; set; }

    public MarginContainer(float margin = 0,ControlBase? dockElement = null, RectangleDrawable? backgroundRectDrable = null) {
      Margin = margin;
      FillElement = dockElement;
      BackgroundRectDrawable = backgroundRectDrable;
    }
    
    void RecalculateUI() {
      if (FillElement == null) { return; }

      FillElement.Size = new Vector2(Size.X - Margin * 2, Size.Y - Margin * 2);
      FillElement.RelativePosition = new Vector2(Margin, Margin);
      FillElement.AbsolutePosition = AbsolutePosition + FillElement.RelativePosition;
    }

    public override void Draw(SpriteBatch spriteBatch, double deltaTime) {
      if (FillElement == null) return;
      RecalculateUI();

      Viewport elementViewport = new Viewport((int)AbsolutePosition.X, (int)AbsolutePosition.Y, (int)Size.X, (int)Size.Y);
      Viewport oldViewport = spriteBatch.GraphicsDevice.Viewport;

      spriteBatch.End();
      spriteBatch.Begin();
      spriteBatch.GraphicsDevice.Viewport = elementViewport;

      if (BackgroundRectDrawable != null) BackgroundRectDrawable.Draw(spriteBatch, deltaTime, Size, Vector2.Zero);
      DrawElement(spriteBatch, deltaTime, FillElement);

      spriteBatch.End();

      //Restore SpriteBatch
      spriteBatch.GraphicsDevice.Viewport = oldViewport;
      spriteBatch.Begin();
    }

    public override void Update(double deltaTime) {
      if (FillElement != null) FillElement.Update(deltaTime);

    }

  }
}
