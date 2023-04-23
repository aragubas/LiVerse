using LiVerse.AnaBanUI.Events;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace LiVerse.AnaBanUI.Controls {
  public class Slider : ControlBase {
    public float MaximumValue { get; set; } = 100;
    public float CurrentValue { get; set; } = 50;
    public bool RaiseOnValueChangedEveryGrabFrame = true;
    public event Action<float>? OnValueChanged;

    float ratio = 0;
    bool sliderGrabbed = false;
    bool sliderPrevGrabbed = false;
    float lastValue = 0;

    // Colors
    static readonly Color backgroundColor = Color.FromNonPremultiplied(228, 227, 230, 255);
    static readonly Color borderColor = Color.FromNonPremultiplied(10, 100, 200, 255);
    static readonly Color levelColor = Color.FromNonPremultiplied(25, 126, 251, 255);
    static readonly Color grabberBackgroundColor = Color.FromNonPremultiplied(228, 227, 230, 255);
    static readonly Color grabberBorderColor = Color.FromNonPremultiplied(10, 100, 200, 255);
    static readonly Color grabberActiveBackgroundColor = Color.FromNonPremultiplied(197, 215, 230, 255);
    static readonly Color grabberActiveBorderColor = Color.FromNonPremultiplied(30, 80, 160, 255);

    public Slider() {
      MinimumSize = new Vector2(20, 18);
    }

    public override void UpdateUI(double deltaTime) {
      // Calculate Level Ratio
      ratio = CurrentValue / MaximumValue;
    }

    public override void DrawElement(SpriteBatch spriteBatch, double deltaTime) {
      spriteBatch.FillRectangle(new RectangleF(0, (Size.Y / 2) / 2, Size.X, Size.Y / 2), backgroundColor);
      spriteBatch.DrawRectangle(new RectangleF(0, (Size.Y / 2) / 2, Size.X, Size.Y / 2), borderColor);

      // Draw Level
      spriteBatch.FillRectangle(new RectangleF(1, (Size.Y / 2) / 2 + 1, Size.X * ratio, Size.Y / 2 - 2), levelColor);

      // Draw Level Detail
      spriteBatch.FillRectangle(new RectangleF(ratio * Size.X - 3, 0, 6, Size.Y), sliderGrabbed ? grabberActiveBackgroundColor : grabberBackgroundColor);
      spriteBatch.DrawRectangle(new RectangleF(ratio * Size.X - 3, 0, 6, Size.Y), sliderGrabbed ? grabberActiveBorderColor : grabberBorderColor);
    }

    void RaiseValueChanged() {
      if (lastValue != CurrentValue) {
        lastValue = CurrentValue;
        OnValueChanged?.Invoke(CurrentValue);
      }
    }

    public override bool InputUpdate(PointerEvent pointerEvent) {
      if (Enabled && Visible) {
        sliderGrabbed = AbsoluteArea.Intersects(pointerEvent.DownRect);

        if (sliderGrabbed) {
          float mouseRelativePos = pointerEvent.PositionRect.X - AbsolutePosition.X;

          CurrentValue = Math.Clamp(mouseRelativePos / Size.X, 0, 1) * MaximumValue;

          if (RaiseOnValueChangedEveryGrabFrame) RaiseValueChanged();
          sliderPrevGrabbed = true;

          return true;
        } else {
          if (sliderPrevGrabbed && !RaiseOnValueChangedEveryGrabFrame) {
            sliderPrevGrabbed = false;
            RaiseValueChanged();
          }
        }
      }

      return false;
    }

    public override void Update(double deltaTime) { }
  }
}
