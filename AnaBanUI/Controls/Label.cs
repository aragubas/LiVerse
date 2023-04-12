using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace LiVerse.AnaBanUI.Controls {
  public enum LabelTextHorizontalAlignment {
    Left, Center, Right
  }
  public enum LabelTextVerticalAlignment {
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

    public Color Color { get; set; } = Color.White;
    public LabelTextVerticalAlignment TextVerticalAlignment = LabelTextVerticalAlignment.Center;
    public LabelTextHorizontalAlignment TextHorizontalAlignment = LabelTextHorizontalAlignment.Center;
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

    public override void Draw(SpriteBatch spriteBatch, double deltaTime) {
      if (font == null || reBakeFont) {
        reBakeFont = false;
        font = ResourceManager.GetFont(FontName, FontSize, spriteBatch.GraphicsDevice); 

         RecalculateUI();
      }
      RecalculatePosition();

      spriteBatch.DrawString(font, Text, textPosition, Color);
    }

    void RecalculatePosition() {
      // Calculates X
      switch (TextHorizontalAlignment) {
        case LabelTextHorizontalAlignment.Center: {
          textPosition.X = Size.X / 2 - FontArea.X / 2;
          break;
        }

        case LabelTextHorizontalAlignment.Left: {
          textPosition.X = 0;
          break;
        }

        case LabelTextHorizontalAlignment.Right: {
          textPosition.X = Size.X - FontArea.X;
          break;
        }          
      }

      // Calculates Y
      switch (TextVerticalAlignment) {
        case LabelTextVerticalAlignment.Center: {
            textPosition.Y = Size.Y / 2 - FontArea.Y / 2;
          break;
        }

        case LabelTextVerticalAlignment.Top: {
          textPosition.Y = 0;
          break;
        }

        case LabelTextVerticalAlignment.Bottom: {
          textPosition.Y = Size.Y - FontArea.Y;
          break;
        }          
      }
    }

    void RecalculateUI() {      
      if (reMeasureText && font != null) {
        reMeasureText = false;
        FontArea = font.MeasureString(Text);
        Size = FontArea;
        MinimumSize = FontArea;
      }
    }

    public override void Update(double deltaTime) {
      RecalculateUI();
    }
  }
}