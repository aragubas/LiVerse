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
    public string Text { get => _text; set {
        if (value == _text) { return; }
        _text = value;
        reMeasureText = true;
      }
    }
    public Color Color { get; set; } = Color.White;
    public LabelTextVerticalAlignment TextVerticalAlignment = LabelTextVerticalAlignment.Center;
    public LabelTextHorizontalAlignment TextHorizontalAlignment = LabelTextHorizontalAlignment.Center;
    SpriteFont? font; 
    bool reBakeFont = true;
    bool reMeasureText = true;
    Vector2 textPosition = Vector2.Zero;
    Vector2 fontArea = Vector2.Zero;

    public Label(string text, int fontSize = 18) {
      Text = text;
      FontSize = fontSize;       
    }  

    public override void Draw(SpriteBatch spriteBatch, double deltaTime) {
      if (font == null || reBakeFont) {
        reBakeFont = false;
        font = ResourceManager.GetFont("OpenSans", FontSize, spriteBatch.GraphicsDevice); 

         RecalculateUI();
      }
      RecalculatePosition();

      spriteBatch.DrawString(font, Text, textPosition, Color);
    }

    void RecalculatePosition() {
      // Calculates X
      switch (TextHorizontalAlignment) {
        case LabelTextHorizontalAlignment.Center: {
          textPosition.X = Size.X / 2 - fontArea.X / 2;
          break;
        }

        case LabelTextHorizontalAlignment.Left: {
          textPosition.X = 0;
          break;
        }

        case LabelTextHorizontalAlignment.Right: {
          textPosition.X = Size.X - fontArea.X;
          break;
        }          
      }

      // Calculates Y
      switch (TextVerticalAlignment) {
        case LabelTextVerticalAlignment.Center: {
            textPosition.Y = Size.Y / 2 - fontArea.Y / 2;
          break;
        }

        case LabelTextVerticalAlignment.Top: {
          textPosition.Y = 0;
          break;
        }

        case LabelTextVerticalAlignment.Bottom: {
          textPosition.Y = Size.Y - fontArea.Y;
          break;
        }          
      }
    }

    void RecalculateUI() {      
      if (reMeasureText && font != null) {
        reMeasureText = false;
        fontArea = font.MeasureString(Text);
        Size = fontArea;
        MinimumSize = fontArea;
      }
    }

    public override void Update(double deltaTime) {
      RecalculateUI();
    }
  }
}