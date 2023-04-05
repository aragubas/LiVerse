using LiVerse.AnaBanUI;
using LiVerse.AnaBanUI.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;

namespace LiVerse.src.AnaBanUI.Controls {
  public class VerticalLevelTrigger : ControlBase {
    public float MaximumValue = 100;
    public float CurrentValue = 0;
    public bool ShowPeaks = false;
    public event Action? LevelTriggered;
    public float TriggerLevel = 0;

    float peakLevel = 0;
    float peakLevelTarget = 0;
    float peakReset = 0;

    // Colors
    // Color.FromNonPremultiplied(66, 100, 234, 255);
    static readonly Color backgroundColor = Color.FromNonPremultiplied(66, 100, 234, 100);
    static readonly Color borderColor = Color.FromNonPremultiplied(50, 118, 234, 255);
    static readonly Color levelMeterColor = Color.FromNonPremultiplied(50, 150, 248, 255);
    static readonly Color levelMeterDetailColor = Color.FromNonPremultiplied(50, 118, 234, 255);
    static readonly Color peakMeterColor = Color.FromNonPremultiplied(100, 219, 255, 75);
    static readonly Color triggerMeterColor = Color.FromNonPremultiplied(5, 96, 150, 255);
    static readonly Color triggerActiveMeterColor = Color.FromNonPremultiplied(96, 15, 160, 255);
    static readonly Color triggerGrabbedMeterColor = Color.FromNonPremultiplied(196, 115, 260, 255);

    bool triggerGrabbed = false;
    bool triggerActive = false;

    public VerticalLevelTrigger() {
    }

    public override void Draw(SpriteBatch spriteBatch, double deltaTime) {
      float ratio = CurrentValue / MaximumValue;

      if (ratio > peakLevelTarget) { peakReset = 0; peakLevelTarget = ratio; peakLevel = ratio; }      

      spriteBatch.FillRectangle(new RectangleF(Vector2.Zero, Size), backgroundColor);

      // Draw Level
      spriteBatch.FillRectangle(new RectangleF(0, Size.Y - (Size.Y * ratio), Size.X, Size.Y * ratio), levelMeterColor);
      // Draw Level Detail
      spriteBatch.FillRectangle(new RectangleF(0, Size.Y - (Size.Y * ratio), Size.X, 1), levelMeterDetailColor);

      // Draw Peak
      spriteBatch.FillRectangle(new RectangleF(0, Size.Y - (Size.Y * peakLevel) - 1, Size.X, 2), peakMeterColor);

      // Draw Trigger Level
      spriteBatch.FillRectangle(new RectangleF(0, Size.Y - (Size.Y * TriggerLevel) - 1, Size.X, 2), 
        triggerGrabbed ? triggerGrabbedMeterColor : (triggerActive ? triggerActiveMeterColor : triggerMeterColor));

      // Draw Border
      spriteBatch.DrawRectangle(new RectangleF(Vector2.Zero, Size), borderColor);
    }

    public override void Update(double deltaTime) {
      MinimumSize = new Vector2(32, 64);

      if (Keyboard.GetState().IsKeyDown(Keys.A)) CurrentValue++;
      if (Keyboard.GetState().IsKeyDown(Keys.D)) CurrentValue--;

      peakReset += (float)deltaTime * 1;

      if (peakReset >= 3) {
        peakReset = 0;
        peakLevelTarget = CurrentValue / MaximumValue;
      }

      RectangleF rect = new RectangleF(AbsolutePosition, Size);

      if (rect.Intersects(UIRoot.MouseDownRectangle)) {
        triggerGrabbed = true;
      }
      else {
        triggerGrabbed = false;
      }

      if (triggerGrabbed) {
        float mouseRelativePos = (Size.Y + AbsolutePosition.Y) - UIRoot.MousePositionRectangle.Y;

        TriggerLevel = Math.Clamp(mouseRelativePos / Size.Y, 0, 1);
      }

      peakLevel = MathHelper.Lerp(peakLevel, peakLevelTarget, (float)(1 - Math.Pow(0.00005, deltaTime)));
      
      triggerActive = (CurrentValue / MaximumValue) >= TriggerLevel;
    }

  }
}
