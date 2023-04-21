using LiVerse.AnaBanUI;
using LiVerse.AnaBanUI.Events;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace LiVerse.AnaBanUI.Controls {
  public class SolidColorRectangle : ControlBase {
    public ControlBase Element { get; set; }
    public Color BackgroundColor { get; set; } = Color.Transparent;
    public float Padding { get; set; } = 4;

    public SolidColorRectangle(ControlBase element) {
      Element = element;
    }

    public override void UpdateUI(double deltaTime) {
      MinimumSize = Element.MinimumSize;
      Element.Size = ContentArea;
      Element.AbsolutePosition = AbsolutePosition;
      Element.RelativePosition = RelativePosition;
    }

    public override void DrawElement(SpriteBatch spriteBatch, double deltaTime) {
      spriteBatch.FillRectangle(new RectangleF(Vector2.Zero, ContentArea), BackgroundColor);

      Element.Draw(spriteBatch, deltaTime);
    }

    public override bool InputUpdate(PointerEvent pointerEvent) {
      return Element.InputUpdate(pointerEvent);
    }

    public override bool InputUpdate(KeyboardEvent keyboardEvent) {
      return Element.InputUpdate(keyboardEvent);
    }

    public override void Update(double deltaTime) {
      Element.Update(deltaTime);
    }

  }
}
