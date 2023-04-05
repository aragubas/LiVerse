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
    public static bool MouseDown = false;
    public static bool WindowFocused = true;
    static MouseState oldMouseState;

    public static void Update(double deltaTime) {
      if (!WindowFocused) { MouseDown = false; return; }
      
      MouseState newMouseState = Mouse.GetState();

      MousePositionRectangle = new Rectangle(newMouseState.Position, new Point(1));

      if (newMouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released) {
        MouseDown = true;

        MouseDownRectangle = MousePositionRectangle;
      }
      else if (newMouseState.LeftButton == ButtonState.Released) {
        MouseDown = false;
        MouseDownRectangle = Rectangle.Empty;
      }

      MouseUpRectangle = (newMouseState.LeftButton == ButtonState.Released && oldMouseState.LeftButton == ButtonState.Pressed) ? MousePositionRectangle : Rectangle.Empty;

      oldMouseState = newMouseState;
    }
  }
}
