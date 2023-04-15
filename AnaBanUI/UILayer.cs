using LiVerse.AnaBanUI;
using LiVerse.AnaBanUI.Drawables;
using LiVerse.AnaBanUI.Events;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;

namespace LiVerse.AnaBanUI {
  public class UILayer {
    public ControlBase? RootElement { get; set; }    
    public RectangleDrawable? BackgroundRectDrawable { get; set; }
    public event Action<KeyboardEvent> InputUpdateEvent;

    // States
    KeyboardState oldKeyboardState;

    public UILayer() {
      RootElement = null;      
    }

    public void Update(double deltaTime) {
      if (RootElement != null) {
        RootElement.Update(deltaTime);
      }
    }

    public void InputUpdate() {
      PointerEvent latestMouseEvent = new PointerEvent() { 
        PositionRect = UIRoot.MousePositionRectangle, 
        DownRect = UIRoot.MouseDownRectangle, 
        UpRect = UIRoot.MouseUpRectangle,
        Down = UIRoot.MouseDown
      };
      KeyboardState newState = Keyboard.GetState();
      KeyboardEvent latestKeyboardEvent = new KeyboardEvent() {
        NewKeyboardState = newState,
        OldKeyboardState = oldKeyboardState
      };

      bool mouseEventConsumed = false;
      bool keyboardEventConsumed = false;
      if (RootElement != null) {
        mouseEventConsumed = RootElement.InputUpdate(latestMouseEvent);
        keyboardEventConsumed = RootElement.InputUpdate(latestKeyboardEvent);
      }

      if (!keyboardEventConsumed) InputUpdateEvent?.Invoke(latestKeyboardEvent);

      oldKeyboardState = Keyboard.GetState();
    }

    public void Draw(SpriteBatch spriteBatch, double deltaTime) {
      if (RootElement != null) {
        // Make Sure the RootElement fills the entire viewport
        Vector2 screenSize = new Vector2(spriteBatch.GraphicsDevice.Viewport.Width, spriteBatch.GraphicsDevice.Viewport.Height);
        RootElement.Size = screenSize;
        RootElement.MaximumSize = screenSize;
        RootElement.AbsolutePosition = Vector2.Zero;

        spriteBatch.Begin();

        if (BackgroundRectDrawable != null) BackgroundRectDrawable.Draw(spriteBatch, deltaTime, screenSize, Vector2.Zero);

        RootElement.Draw(spriteBatch, deltaTime);

        spriteBatch.End();
      }
      
    }
  }
}