using LiVerse.AnaBanUI;
using LiVerse.AnaBanUI.Controls;
using LiVerse.AnaBanUI.Events;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;

namespace LiVerse.AnaBanUI.Controls {
  public class VerticalLevelTrigger : ControlBase {
    public float MaximumValue = 100;
    public float CurrentValue = 0;
    public bool ShowPeaks = false;
    public float TriggerLevel = 0;

    float peakLevel = 0;
    float peakLevelTarget = 0;
    float peakReset = 0;
    float ratio = 0;

    // Colors
    // Color.FromNonPremultiplied(66, 100, 234, 255);
    static readonly Color backgroundColor = Color.FromNonPremultiplied(66, 100, 234, 100);
    static readonly Color borderColor = Color.FromNonPremultiplied(50, 118, 234, 255);
    static readonly Color levelMeterColor = Color.FromNonPremultiplied(50, 150, 248, 255);
    static readonly Color levelMeterDetailColor = Color.FromNonPremultiplied(50, 118, 234, 255);
    static readonly Color peakMeterColor = Color.FromNonPremultiplied(255, 219, 230, 150);
    static readonly Color triggerMeterColor = Color.FromNonPremultiplied(5, 96, 150, 255);
    static readonly Color triggerActiveMeterColor = Color.FromNonPremultiplied(96, 15, 160, 255);
    static readonly Color triggerGrabbedMeterColor = Color.FromNonPremultiplied(196, 115, 260, 255);

    bool triggerGrabbed = false;
    bool triggerActive = false;

    public VerticalLevelTrigger() {
      MinimumSize = new Vector2(24, 64);
    }

    public override void UpdateUI(double deltaTime) {
      #region Update Peak Meter
      if (ShowPeaks) {
        peakReset += (float)deltaTime * 1;

        if (peakReset >= 3) {
          peakReset = 0;
          peakLevelTarget = CurrentValue / MaximumValue;
        }

        peakLevel = MathHelper.Lerp(peakLevel, peakLevelTarget, (float)(1 - Math.Pow(0.00005, deltaTime)));
      }
      #endregion

      // Calculate Level Ratio
      ratio = CurrentValue / MaximumValue;
    }

    public override void DrawElement(SpriteBatch spriteBatch, double deltaTime) {
      spriteBatch.FillRectangle(new RectangleF(Vector2.Zero, Size), backgroundColor);

      // Draw Level
      spriteBatch.FillRectangle(new RectangleF(0, Size.Y - (Size.Y * ratio), Size.X, Size.Y * ratio), levelMeterColor);
      // Draw Level Detail
      spriteBatch.FillRectangle(new RectangleF(0, Size.Y - (Size.Y * ratio), Size.X, 1), levelMeterDetailColor);

      // Draw Peak
      if (ShowPeaks) {
        if (ratio > peakLevelTarget) { peakReset = 0; peakLevelTarget = ratio; peakLevel = ratio; }

        spriteBatch.FillRectangle(new RectangleF(0, Size.Y - (Size.Y * peakLevel) - 1, Size.X, 2), peakMeterColor);
      }

      // Draw Trigger Level
      spriteBatch.FillRectangle(new RectangleF(0, Size.Y - (Size.Y * TriggerLevel) - 1, Size.X, 2), 
        triggerGrabbed ? triggerGrabbedMeterColor : (triggerActive ? triggerActiveMeterColor : triggerMeterColor));

      // Draw Border
      spriteBatch.DrawRectangle(new RectangleF(Vector2.Zero, Size), borderColor);
    }

    public override bool InputUpdate(PointerEvent pointerEvent) {
      if (Enabled && Visible) {
        triggerGrabbed = AbsoluteArea.Intersects(pointerEvent.DownRect);

        if (triggerGrabbed) {
          float mouseRelativePos = (Size.Y + AbsolutePosition.Y) - pointerEvent.PositionRect.Y;

          TriggerLevel = Math.Clamp(mouseRelativePos / Size.Y, 0, 1);
          return true;
        }
      }

      return false;
    }

    public override void Update(double deltaTime) {
      if (!Visible) { return; }

      triggerActive = (CurrentValue / MaximumValue) >= TriggerLevel;
    }

  }
}
