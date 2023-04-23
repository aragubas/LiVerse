using LiVerse.AnaBanUI.Events;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace LiVerse.AnaBanUI.Controls {
  public enum ButtonStyle {
    Default, Selectable, Flat
  }

  public class Button : ControlBase {
    public event Action? Click = null;
    public event Action? BlinkingStarted = null;
    public Label Label;
    public bool IsSelected;
    public bool BlinkWhenPressed { get; set; } = false;
    public ButtonStyle ButtonStyle = ButtonStyle.Default;
    public Vector2 LabelPadding { get; set; } = new(10, 2);

    // Background Colors
    static readonly Color normalBackground = Color.FromNonPremultiplied(228, 227, 230, 255);
    static readonly Color hoverBackground = Color.FromNonPremultiplied(220, 245, 255, 255);
    static readonly Color downBackground = Color.FromNonPremultiplied(200, 220, 235, 255);
    static readonly Color selectedBackground = Color.FromNonPremultiplied(197, 215, 230, 255);
    static readonly Color flatHoverBackground = Color.FromNonPremultiplied(25, 126, 251, 255);
    Color currentTargetBackgroundColor = normalBackground;
    Color currentBackgroundColor = normalBackground;

    // Foreground Colors
    static readonly Color normalForeground = Color.FromNonPremultiplied(20, 20, 40, 255);
    static readonly Color flatHoverForeground = Color.FromNonPremultiplied(255, 255, 255, 255);
    static readonly Color downForeground = Color.FromNonPremultiplied(0, 0, 0, 255);
    Color currentForegroundColor = normalForeground;

    // Border Colors
    static readonly Color normalBorder = Color.FromNonPremultiplied(10, 100, 200, 255);
    static readonly Color hoverBorder = Color.FromNonPremultiplied(20, 135, 225, 255);
    static readonly Color downBorder = Color.FromNonPremultiplied(30, 80, 160, 255);
    static readonly Color unSelectedBorder = Color.FromNonPremultiplied(0, 0, 0, 0);
    static readonly Color selectedBorder = Color.FromNonPremultiplied(30, 145, 235, 255);
    Color currentTargetBorderColor = normalBorder;
    Color currentBorderColor = normalBorder;

    bool isMouseHovering = false;
    bool isBlinking = false;
    bool blinkingEnd = false;
    bool wasHolding = false;
    double blinkTimer = 0;

    public Button(string DefaultText, int defaultFontSize = 21, ButtonStyle buttonStyle = ButtonStyle.Default) {
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

      //EndDraw(spriteBatch);
    }

    public override bool InputUpdate(PointerEvent pointerEvent) {
      if (Enabled && Visible && !isBlinking) {
        isMouseHovering = pointerEvent.PositionRect.Intersects(AbsoluteArea);

        if (isMouseHovering && !pointerEvent.Down) {
          currentTargetBackgroundColor = ButtonStyle != ButtonStyle.Flat ? hoverBackground : flatHoverBackground;
          currentForegroundColor = ButtonStyle != ButtonStyle.Flat ? normalForeground : flatHoverForeground;
          if (ButtonStyle != ButtonStyle.Selectable) currentTargetBorderColor = hoverBorder;
        } else { wasHolding = false; }

        if (pointerEvent.DownRect.Intersects(AbsoluteArea)) {
          currentTargetBackgroundColor = ButtonStyle != ButtonStyle.Flat ? downBackground : normalBackground;
          currentTargetBorderColor = downBorder;
          currentForegroundColor = ButtonStyle != ButtonStyle.Flat ? downForeground : normalForeground;
          wasHolding = true;

          return true;
        }

        if (pointerEvent.UpRect.Intersects(AbsoluteArea) && wasHolding) {
          wasHolding = false;

          if (!BlinkWhenPressed) {
            Click?.Invoke();

        }
        else {
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

    public override void UpdateUI(double deltaTime) {
      if (isBlinking) return;

      // Interpolate
      if (ButtonStyle != ButtonStyle.Flat) {
        currentBackgroundColor = Color.Lerp(currentBackgroundColor, currentTargetBackgroundColor, (float)(1 - Math.Pow(0.00025, deltaTime)));
        currentBorderColor = Color.Lerp(currentBorderColor, currentTargetBorderColor, (float)(1 - Math.Pow(0.000025, deltaTime)));

      } else {
        currentBackgroundColor = currentTargetBackgroundColor;
        currentBorderColor = currentTargetBorderColor;
      }      
    }

    public override void Update(double deltaTime) {
      Label.Update(deltaTime);

      Label.Color = currentForegroundColor;
      Label.Size = ButtonStyle == ButtonStyle.Default ? ContentArea : ButtonStyle == ButtonStyle.Selectable ? new(ContentArea.X - 3, ContentArea.Y) : ContentArea;
      Label.AbsolutePosition = ButtonStyle == ButtonStyle.Default ? AbsolutePosition : AbsolutePosition + (Vector2.UnitX * 3);
      Label.RenderOffset = RenderOffset;
      if (ParentControl != null) Label.RenderOffset += ParentControl.RenderOffset;

      MinimumSize = Label.MinimumSize + LabelPadding;

      if (isBlinking) {
        blinkTimer += 1 * deltaTime;

        if (blinkTimer >= 0.1) {
          blinkTimer = 0;
          blinkingEnd = true;

        }
        else if (blinkTimer >= 0.05) {
          if (!blinkingEnd) {
            currentBackgroundColor = flatHoverBackground;
            currentBorderColor = downBorder;
            currentForegroundColor = flatHoverForeground;
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
        currentForegroundColor = normalForeground;
        currentTargetBackgroundColor = normalBackground;
        currentTargetBorderColor = normalBorder;

        if (ButtonStyle == ButtonStyle.Selectable && IsSelected) {
          currentTargetBackgroundColor = selectedBackground;
          currentTargetBorderColor = selectedBorder;
          currentForegroundColor = downForeground;

        }
        else if (ButtonStyle == ButtonStyle.Selectable && !IsSelected) {
          currentTargetBorderColor = unSelectedBorder;
        }
      }

    }

  }
}
