using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiVerse.src.AnaBanUI
{
  public static class UIRoot {
    public static Rectangle MousePositionRectangle;
    public static Rectangle MouseDownRectangle;
    public static Rectangle MouseUpRectangle;
    static MouseState oldMouseState;

    public static void Update(double deltaTime) {
      MouseState newMouseState = Mouse.GetState();

      MousePositionRectangle = new Rectangle(newMouseState.Position, new Point(1));

      MouseDownRectangle = (newMouseState.LeftButton == ButtonState.Pressed) ? MousePositionRectangle : Rectangle.Empty;

      MouseUpRectangle = (newMouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released) ? MousePositionRectangle : Rectangle.Empty;

      oldMouseState = newMouseState;
    }
  }
}
