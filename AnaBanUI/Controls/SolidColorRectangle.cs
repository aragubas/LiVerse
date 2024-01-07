using LiVerse.AnaBanUI;
using LiVerse.AnaBanUI.Events;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace LiVerse.AnaBanUI.Controls; 
public class SolidColorRectangle : ControlBase {
  public ControlBase? Element { get; set; }
  public Color BackgroundColor { get; set; } = Color.Transparent;
  //public float Padding { get; set; } = 4;

  public SolidColorRectangle(ControlBase element) {
    Element = element;
  }

  public SolidColorRectangle() {
    Element = null;
  }

  public override void UpdateUI(double deltaTime) {
    if (Element != null) FillControl(Element);
  }

  public override void DrawControl(SpriteBatch spriteBatch, double deltaTime) {
    spriteBatch.FillRectangle(new RectangleF(Vector2.Zero, ContentArea), BackgroundColor);

    if (Element != null) Element.Draw(spriteBatch, deltaTime);
  }

  public override bool InputUpdate(PointerEvent pointerEvent) {
    return Element == null ? false : Element.InputUpdate(pointerEvent);
  }

  public override bool InputUpdate(KeyboardEvent keyboardEvent) {
    return Element == null ? false : Element.InputUpdate(keyboardEvent);
  }

  public override void Update(double deltaTime) {
    Element?.Update(deltaTime);
  }

}
