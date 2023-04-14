using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;

namespace LiVerse.AnaBanUI
{
  public static class UIRoot {
    public static RectangleF MousePositionRectangle;
    public static RectangleF MouseDownRectangle;
    public static RectangleF MouseUpRectangle;
    public static bool MouseDown = false;
    public static bool WindowFocused = true;

    static MouseState oldMouseState;

    public static void Update(double deltaTime) {
      if (!WindowFocused) { MouseDown = false; return; }
      
      MouseState newMouseState = Mouse.GetState();

      MousePositionRectangle = new Rectangle(newMouseState.Position, new Point(1));

      // MouseDown
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
