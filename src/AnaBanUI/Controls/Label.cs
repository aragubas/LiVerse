using FontStashSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LiVerse.AnaBanUI.Controls {
  public class Label : ControlBase {
    int _fontSize = 18;
    public int FontSize { get => _fontSize; set {
        if (value == _fontSize) { return; }
        _fontSize = value;
        reBakeFont = true;
      }
    }
    public string _text;
    public string Text { get => _text; set {
        if (value == _text) { return; }
        _text = value;
        reMeasureText = true;
      }
    }
    public Color Color { get; set; } = Color.White;
    DynamicSpriteFont? font; 
    bool reBakeFont = true;
    bool reMeasureText = true;


    public Label(string text, int fontSize = 18) {
      Text = text;
      FontSize = fontSize; 
      
    }  

    public override void Draw(SpriteBatch spriteBatch) {
      if (font == null || reBakeFont) {
        reBakeFont = false;
        font = ResourceManager.GlobalFontSystem.GetFont(FontSize); 
      }
 
      spriteBatch.DrawString(font, Text, new Vector2(Size.X / 2 - MinimumSize.X / 2, Size.Y / 2 - MinimumSize.Y / 2), this.Color, Vector2.One);
    }

    public override void Update(double deltaTime)
    {
      if (reMeasureText && font != null) {
        reMeasureText = false;
        Vector2 FontArea = font.MeasureString(Text);
        Size = FontArea;
        MinimumSize = FontArea;
      }

    }
  }
}