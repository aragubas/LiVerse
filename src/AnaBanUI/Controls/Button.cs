using LiVerse.AnaBanUI;
using LiVerse.AnaBanUI.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiVerse.src.AnaBanUI.Controls {
  public class Button : ControlBase {
    public event Action? Clicked = null;
    public Label Label;
    
    // Background Colors
    static readonly Color normalBackground = new Color() { R = 225, G = 225, B = 230, A = 255 };
    static readonly Color hoverBackground = new Color() { R = 220, G = 245, B = 255, A = 255 };
    static readonly Color downBackground = new Color() { R = 200, G = 220, B = 235, A = 255 };
    Color currentTargetBackgroundColor = normalBackground;
    Color currentBackgroundColor = normalBackground;

    // Foreground Colors
    static readonly Color normalForeground = new Color() { R = 50, G = 50, B = 60, A = 255 };
    static readonly Color downForeground = new Color() { R = 0, G = 0, B = 0, A = 255 };
    Color currentForegroundColor = normalForeground;

    // Border Colors
    static readonly Color normalBorder = new Color() { R = 10, G = 100, B = 200, A = 255 };
    static readonly Color hoverBorder = new Color() { R = 20, G = 135, B = 225, A = 255 };
    static readonly Color downBorder = new Color() { R = 30, G = 80, B = 160, A = 255 };
    Color currentBorderColor = normalBorder;
    
    bool isMouseHovering = false;
    float borderThickness = 2;
    float borderThicknessTarget = 2;

    public Button(string DefaultText, int defaultFontSize = 22) {
      Label = new Label(DefaultText, defaultFontSize);
    }

    public override void Draw(SpriteBatch spriteBatch, double deltaTime) {
      spriteBatch.FillRectangle(new RectangleF(Vector2.Zero, Size), currentBackgroundColor);

      Label.Draw(spriteBatch, deltaTime);

      spriteBatch.DrawRectangle(new RectangleF(Vector2.Zero, Size), currentBorderColor, thickness: borderThickness);
    }

    public override void Update(double deltaTime) {
      Label.Update(deltaTime);

      Label.Color = currentForegroundColor;
      Label.Size = Size;
      MinimumSize = Label.MinimumSize + new Vector2(10, 2);


      if (Enabled && Visible) {
        // Interpolate
        float interpolationFactor = (float)(1 - Math.Pow(0.00025, deltaTime));
        borderThickness = MathHelper.LerpPrecise(borderThickness, borderThicknessTarget, interpolationFactor);
        currentBackgroundColor = Color.Lerp(currentBackgroundColor, currentTargetBackgroundColor, (float)(1 - Math.Pow(0.003, deltaTime)));

        Rectangle absoluteRectangle = new Rectangle(AbsolutePosition.ToPoint(), Size.ToPoint());
        isMouseHovering = UIRoot.MousePositionRectangle.Intersects(absoluteRectangle);

        currentForegroundColor = normalForeground;
        currentTargetBackgroundColor = normalBackground;
        currentBorderColor = normalBorder;


        if (isMouseHovering) {
          currentTargetBackgroundColor = hoverBackground;
          currentBorderColor = hoverBorder;
          borderThicknessTarget = 2;
        } else { borderThicknessTarget = 1; }

        if (UIRoot.MouseDownRectangle.Intersects(absoluteRectangle)) {
          currentTargetBackgroundColor = downBackground;
          currentForegroundColor = downForeground;
          currentBorderColor = downBorder;
        }
        
        if (UIRoot.MouseUpRectangle.Intersects(absoluteRectangle)) {
          Clicked?.Invoke();
        }
      }
    }

  }
}
