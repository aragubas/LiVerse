using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace LiVerse.AnaBanUI.Drawables {
  public class RectangleDrawable : IDrawable {
    public bool IsFilled { get; set; } = true;
    public float BorderThickness { get; set; } = 1f;
    public Color Color { get; set; }
    public int Opacity { get; set; } = 255;

    public void Draw(SpriteBatch spriteBatch, double deltaTime, Vector2 area, Vector2 position) {
      if (IsFilled) {
        spriteBatch.FillRectangle(new RectangleF(position, area), Color.FromNonPremultiplied(Color.R, Color.G, Color.B, Opacity));
        return;
      }

      spriteBatch.DrawRectangle(new RectangleF(position, area), Color.FromNonPremultiplied(Color.R, Color.G, Color.B, Opacity), BorderThickness);
    }
  }
}
