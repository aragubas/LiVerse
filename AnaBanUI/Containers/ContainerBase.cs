using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace LiVerse.AnaBanUI.Containers {
  public abstract class ContainerBase : ControlBase {    
    //public override void Draw(SpriteBatch spriteBatch, double deltaTime) {
    //  if (!Visible) { return; }
    //  spriteBatch.End();
    //  spriteBatch.Begin(transformMatrix: Matrix.CreateTranslation(AbsolutePosition.X, AbsolutePosition.Y, 0));

    //  DrawElement(spriteBatch, deltaTime);

    //  if (DrawDebugLines) {
    //    spriteBatch.End();
    //    spriteBatch.Begin(transformMatrix: Matrix.CreateTranslation(AbsolutePosition.X, AbsolutePosition.Y, 0));

    //    if (Margin != Vector2.Zero) {
    //      spriteBatch.DrawRectangle(new RectangleF(Vector2.Zero, ContentArea), Color.Magenta);
    //    }

    //    spriteBatch.End();
    //    spriteBatch.Begin(transformMatrix: Matrix.CreateTranslation(AbsolutePosition.X - Margin.X, AbsolutePosition.Y - Margin.Y, 0));

    //    spriteBatch.DrawRectangle(new RectangleF(Vector2.Zero, Size), Color.Blue);
    //  }

    //  spriteBatch.End();
    //  spriteBatch.Begin();
    //}

  }
}
