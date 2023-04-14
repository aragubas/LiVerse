using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace LiVerse.AnaBanUI.Containers {
  public abstract class ContainerBase : ControlBase {
    public override void Draw(SpriteBatch spriteBatch, double deltaTime) {
      if (!Visible) { return; }
      
      spriteBatch.End();
      spriteBatch.Begin(transformMatrix: Matrix.CreateTranslation(AbsolutePosition.X, AbsolutePosition.Y, 0));

      DrawElement(spriteBatch, deltaTime);

      spriteBatch.End();
      spriteBatch.Begin();
    }
  }
}
