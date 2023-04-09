using LiVerse.AnaBanUI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiVerse.AnaBanUI.Controls {
  public class SolidColorRectangle : ControlBase {
    public ControlBase Element { get; set; }
    public float Margin { get; set; } = 0f;
    public Color BackgroundColor { get; set; } = Color.Transparent;


    public SolidColorRectangle(ControlBase element) {
      Element = element;
    }

    public override void Draw(SpriteBatch spriteBatch, double deltaTime) {
      spriteBatch.FillRectangle(new RectangleF(Vector2.Zero, Size), BackgroundColor);

      Element.Draw(spriteBatch, deltaTime);

      //spriteBatch.DrawRectangle(new RectangleF(Vector2.Zero, Element.Size), BorderColor, BorderThickness);
    }

    public override void Update(double deltaTime) {
      Margin = 8f; 
      MinimumSize = Element.MinimumSize + new Vector2(Margin);
      Element.Size = Size;

      Element.Update(deltaTime);
    }

  }
}
