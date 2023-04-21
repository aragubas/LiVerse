using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace LiVerse.AnaBanUI.Controls {
  public enum LabelHorizontalAlignment {
    Left, Center, Right
  }
  public enum LabelVerticalAlignment {
    Top, Center, Bottom
  }
  
  /// <summary>
  /// Draws Text into Screen
  /// </summary>
  public class Label : ControlBase {
    int _fontSize = 18;
    public int FontSize { get => _fontSize; set {
        if (value == _fontSize) { return; }
        _fontSize = value;

        if (_fontSize < -1) { _fontSize = -1; }

        reBakeFont = true;
        reMeasureText = true;
      }
    }
    public string _text = "";
    public string Text {
      get => _text; set {
        if (value == _text) { return; }
        _text = value;
        reMeasureText = true;
      }
    }
    public string _fontName = "OpenSans";
    public string FontName {
      get => _fontName; set {
        if (value == _fontName) { return; }
        _fontName = value;
        reMeasureText = true;
      }
    }

    public Color Color { get; set; } = Color.Black;
    public LabelVerticalAlignment VerticalAlignment = LabelVerticalAlignment.Center;
    public LabelHorizontalAlignment HorizontalAlignment = LabelHorizontalAlignment.Center;
    public Vector2 FontArea = Vector2.Zero;
    SpriteFont? font; 
    bool reBakeFont = true;
    bool reMeasureText = true;
    Vector2 textPosition = Vector2.Zero;

    public Label(string text, int fontSize = 18, string fontName = "OpenSans") {
      Text = text;
      FontSize = fontSize;
      FontName = fontName;
    }

    public override void UpdateUI(double deltaTime) {
      if (Text == null) { Text = ""; }

      if (font == null || reBakeFont) {
        reBakeFont = false;
        font = ResourceManager.GetFont(FontName, FontSize);
      }

      if (reMeasureText && font != null && Text != null) {
        reMeasureText = false;
        FontArea = font.MeasureString(Text);
        MinimumSize = FontArea;
      }

      RecalculatePosition();
    }

    public override void DrawElement(SpriteBatch spriteBatch, double deltaTime) {
      if (font == null) { return; }
      spriteBatch.DrawString(font, Text, textPosition, Color);
    }

    void RecalculatePosition() {
      // Calculates X
      switch (HorizontalAlignment) {
        case LabelHorizontalAlignment.Center: {
          textPosition.X = ContentArea.X / 2 - FontArea.X / 2;
          break;
        }

        case LabelHorizontalAlignment.Left: {
          textPosition.X = 0;
          break;
        }

        case LabelHorizontalAlignment.Right: {
          textPosition.X = ContentArea.X - FontArea.X;
          break;
        }          
      }

      // Calculates Y
      switch (VerticalAlignment) {
        case LabelVerticalAlignment.Center: {
            textPosition.Y = ContentArea.Y / 2 - FontArea.Y / 2;
          break;
        }

        case LabelVerticalAlignment.Top: {
          textPosition.Y = 0;
          break;
        }

        case LabelVerticalAlignment.Bottom: {
          textPosition.Y = ContentArea.Y - FontArea.Y;
          break;
        }          
      }
    }

    public override void Update(double deltaTime) {
    }

  }
}