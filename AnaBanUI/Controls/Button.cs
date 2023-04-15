using LiVerse.AnaBanUI;
using LiVerse.AnaBanUI.Controls;
using LiVerse.AnaBanUI.Events;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace LiVerse.AnaBanUI.Controls {
  public enum ButtonStyle {
    Default, Selectable, Flat
  }
  
  public class Button : ControlBase {
    public event Action? Click = null;
    public event Action? BlinkingStarted = null;
    public Label Label;
    public bool IsSelected;
    public bool BlinkWhenPressed { get; set; }
    public ButtonStyle ButtonStyle = ButtonStyle.Default;
    
    // Background Colors
    static readonly Color normalBackground = new Color() { R = 228, G = 227, B = 230, A = 255 };
    static readonly Color hoverBackground = new Color() { R = 220, G = 245, B = 255, A = 255 };
    static readonly Color downBackground = new Color() { R = 200, G = 220, B = 235, A = 255 };
    static readonly Color selectedBackground = new Color() { R = 197, G = 215, B = 230, A = 255 };
    Color currentTargetBackgroundColor = normalBackground;
    Color currentBackgroundColor = normalBackground;

    // Foreground Colors
    static readonly Color normalForeground = new Color() { R = 20, G = 20, B = 40, A = 255 };
    static readonly Color downForeground = new Color() { R = 0, G = 0, B = 0, A = 255 };
    Color currentForegroundColor = normalForeground;

    // Border Colors
    static readonly Color normalBorder = new Color() { R = 10, G = 100, B = 200, A = 255 };
    static readonly Color hoverBorder = new Color() { R = 20, G = 135, B = 225, A = 255 };
    static readonly Color downBorder = new Color() { R = 30, G = 80, B = 160, A = 255 };
    static readonly Color unSelectedBorder = new Color() { R = 0, G = 0, B = 0, A = 0 };
    static readonly Color selectedBorder = new Color() { R = 30, G = 145, B = 235, A = 255 };
    Color currentTargetBorderColor = normalBorder;
    Color currentBorderColor = normalBorder;
    
    bool isMouseHovering = false;
    bool isBlinking = false;
    bool blinkingEnd = false;
    double blinkTimer = 0;

    public Button(string DefaultText, int defaultFontSize = 22, ButtonStyle buttonStyle = ButtonStyle.Default) {
      Label = new Label(DefaultText, defaultFontSize) { ParentControl = this };
      ButtonStyle = buttonStyle;
    }

    public override void DrawElement(SpriteBatch spriteBatch, double deltaTime) {
      spriteBatch.FillRectangle(new RectangleF(Vector2.Zero, Size), currentBackgroundColor);

      Label.Draw(spriteBatch, deltaTime);
      
      // Restore After drawing context switch
      //EndDraw(spriteBatch);
      //BeginDraw(spriteBatch);

      if (ButtonStyle == ButtonStyle.Default) spriteBatch.DrawRectangle(new RectangleF(Vector2.Zero, Size), currentBorderColor);
      if (ButtonStyle == ButtonStyle.Selectable) spriteBatch.FillRectangle(new RectangleF(Vector2.Zero, new Point(2, (int)Size.Y)), currentBorderColor);
      if (ButtonStyle == ButtonStyle.Flat && isMouseHovering) spriteBatch.DrawRectangle(new RectangleF(Vector2.Zero, Size), currentBorderColor);

      //EndDraw(spriteBatch);
    }

    public override bool InputUpdate(PointerEvent pointerEvent) {
      if (Enabled && Visible && !isBlinking) {
        isMouseHovering = pointerEvent.PositionRect.Intersects(AbsoluteArea);

        if (isMouseHovering && !UIRoot.MouseDown) {
          currentTargetBackgroundColor = hoverBackground;
          if (IsSelected) currentTargetBorderColor = hoverBorder;
        }

        if (pointerEvent.DownRect.Intersects(AbsoluteArea)) {
          currentTargetBackgroundColor = downBackground;
          currentTargetBorderColor = downBorder;
          currentForegroundColor = downForeground;
          
          return true;
        }

        if (pointerEvent.UpRect.Intersects(AbsoluteArea)) {
          if (!BlinkWhenPressed) {
            Click?.Invoke();

          } else {
            isBlinking = true;

            currentBackgroundColor = normalBackground;
            currentBorderColor = normalBorder;
            currentForegroundColor = normalForeground;
            isMouseHovering = false;
            IsSelected = false;
            BlinkingStarted?.Invoke();
          }

          return true;
        }
      }

      return false;
    }

    public override void Update(double deltaTime) {
      Label.Update(deltaTime);

      Label.Color = currentForegroundColor;
      Label.Size = ButtonStyle == ButtonStyle.Default ? Size : new Vector2(Size.X - 3, Size.Y);
      Label.AbsolutePosition = ButtonStyle == ButtonStyle.Default ? AbsolutePosition : AbsolutePosition + (Vector2.UnitX * 3);
      MinimumSize = Label.MinimumSize + new Vector2(10, 2);

      if (isBlinking) {
        blinkTimer += 1 * deltaTime;

        if (blinkTimer >= 0.13) {
          blinkTimer = 0;
          blinkingEnd = true;
        
        } else if (blinkTimer >= 0.1) {
          if (!blinkingEnd) {
            currentBackgroundColor = downBackground;
            currentBorderColor = downBorder;
            currentForegroundColor = downForeground;
            isMouseHovering = true;
            IsSelected = true;

          }
          else {
            blinkingEnd = false;
            isBlinking = false;
            isMouseHovering = false;
            IsSelected = false;
            blinkTimer = 0;

            Click?.Invoke();
          }

        }

      }

      // Update Default Style
      if (Enabled && Visible && !isBlinking) {
        // Interpolate
        currentBackgroundColor = Color.Lerp(currentBackgroundColor, currentTargetBackgroundColor, (float)(1 - Math.Pow(0.0025, deltaTime)));
        currentBorderColor = Color.Lerp(currentBorderColor, currentTargetBorderColor, (float)(1 - Math.Pow(0.003, deltaTime)));

        currentForegroundColor = normalForeground;
        currentTargetBackgroundColor = normalBackground;
        currentTargetBorderColor = normalBorder;

        if (ButtonStyle == ButtonStyle.Selectable && IsSelected) {
          currentTargetBackgroundColor = selectedBackground;
          currentTargetBorderColor = selectedBorder;
          currentForegroundColor = downForeground;

        } else if (ButtonStyle == ButtonStyle.Selectable && !IsSelected) {
          currentTargetBorderColor = unSelectedBorder;
        }
      }

    }

  }
}
