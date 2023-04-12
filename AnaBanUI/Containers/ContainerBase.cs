using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace LiVerse.AnaBanUI.Containers {
  public abstract class ContainerBase : ControlBase {

    protected void DrawElement(SpriteBatch spriteBatch, double deltaTime, ControlBase element, bool drawLines = false) {
      if (!element.Visible) { return; }

      Viewport oldViewport = spriteBatch.GraphicsDevice.Viewport;

      spriteBatch.End();
      if (element.HideOverflow) spriteBatch.GraphicsDevice.Viewport = new Viewport((int)element.AbsolutePosition.X, (int)element.AbsolutePosition.Y, (int)element.Size.X, (int)element.Size.Y);
      if (!element.HideOverflow) spriteBatch.Begin(transformMatrix: Matrix.CreateTranslation(element.RelativePosition.X, element.RelativePosition.Y, 0));
      if (element.HideOverflow) spriteBatch.Begin();

      element.Draw(spriteBatch, deltaTime);
      if (drawLines) spriteBatch.DrawRectangle(new RectangleF(0, 0, element.Size.X, element.Size.Y), Color.Red);

      spriteBatch.End();

      if (element.HideOverflow) spriteBatch.GraphicsDevice.Viewport = oldViewport;

      spriteBatch.Begin();
    }
  }
}
