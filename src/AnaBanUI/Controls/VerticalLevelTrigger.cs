using LiVerse.AnaBanUI;
using LiVerse.AnaBanUI.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;

namespace LiVerse.src.AnaBanUI.Controls {
  public class VerticalLevelTrigger : ControlBase {
    public float MaxValue = 100;
    public float CurrentValue = 50;
    public bool ShowPeaks = true;
    Label debugLabel;

    public float peakLevel { get; set; }
    float peakLevelTarget = 0;
    float peakReset = 0;

    // Colors
    // Color.FromNonPremultiplied(66, 100, 234, 255);
    static readonly Color backgroundColor = Color.FromNonPremultiplied(66, 100, 234, 100);
    static readonly Color borderColor = Color.FromNonPremultiplied(50, 118, 234, 255);
    static readonly Color levelMeterColor = Color.FromNonPremultiplied(50, 130, 238, 255);
    static readonly Color levelMeterDetailColor = Color.FromNonPremultiplied(50, 118, 234, 255);
    static readonly Color peakMeterColor = Color.FromNonPremultiplied(100, 219, 255, 150);

    public VerticalLevelTrigger() {
      debugLabel = new Label("e");
      debugLabel.Color = Color.Blue;

    }

    public override void Draw(SpriteBatch spriteBatch, double deltaTime) {
      float ratio = CurrentValue / MaxValue;

      if (ratio > peakLevelTarget) { peakReset = 0; peakLevelTarget = ratio; peakLevel = ratio; }

      spriteBatch.FillRectangle(new RectangleF(Vector2.Zero, Size), backgroundColor);

      // Draw Level
      spriteBatch.FillRectangle(new RectangleF(0, Size.Y - (Size.Y * ratio), Size.X, Size.Y * ratio), levelMeterColor);
      // Draw Level Detail
      spriteBatch.FillRectangle(new RectangleF(0, Size.Y - (Size.Y * ratio), Size.X, 1), levelMeterDetailColor);

      // Draw Peak
      spriteBatch.FillRectangle(new RectangleF(0, Size.Y - (Size.Y * peakLevel) - 1, Size.X, 2), peakMeterColor);

      // Draw Border
      spriteBatch.DrawRectangle(new RectangleF(Vector2.Zero, Size), borderColor);

      debugLabel.Draw(spriteBatch, deltaTime);
    }

    public override void Update(double deltaTime) {
      MinimumSize = new Vector2(32, 64);

      if (Keyboard.GetState().IsKeyDown(Keys.A)) CurrentValue++;
      if (Keyboard.GetState().IsKeyDown(Keys.D)) CurrentValue--;

      peakReset += (float)deltaTime * 1;

      if (peakReset >= 3) {
        peakReset = 0;
        peakLevelTarget = CurrentValue / MaxValue;
      }

      CurrentValue = Math.Clamp(CurrentValue, 0, MaxValue);

      peakLevel = MathHelper.LerpPrecise(peakLevel, peakLevelTarget, (float)(1 - Math.Pow(0.000005, deltaTime)));
      
      debugLabel.Update(deltaTime);
    }

  }
}
