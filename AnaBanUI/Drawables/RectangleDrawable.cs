using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace LiVerse.AnaBanUI.Drawables {
  public class RectangleDrawable : IDrawable {
    public bool FillCenter { get; set; } = true;
    public float BorderThickness { get; set; } = 1f;
    public Color Color { get; set; }

    public void Draw(SpriteBatch spriteBatch, double deltaTime, Vector2 area, Vector2 position) {
      if (FillCenter) {
        spriteBatch.FillRectangle(new RectangleF(position, area), Color);
        return;
      }

      spriteBatch.DrawRectangle(new RectangleF(position, area), Color, BorderThickness);
    }
  }
}
