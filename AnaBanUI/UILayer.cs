using LiVerse.AnaBanUI.Drawables;
using LiVerse.AnaBanUI.Events;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LiVerse.AnaBanUI {
  public class UILayer {
    public ControlBase? RootElement { get; set; }
    public bool HasInputProcessing { get; set; } = true;
    public RectangleDrawable? BackgroundRectDrawable { get; set; }
    public event Action<KeyboardEvent>? KeyboardInputUpdateEvent;
    public event Action<PointerEvent>? PointerInputUpdateEvent;

    // States
    KeyboardState oldKeyboardState;

    public UILayer() {
      RootElement = null;      
    }

    public void Update(double deltaTime) {
      RootElement?.Update(deltaTime);
    }

    public void InputUpdate() {
      PointerEvent latestMouseEvent = new() { 
        PositionRect = UIRoot.MousePositionRectangle, 
        DownRect = UIRoot.MouseDownRectangle, 
        UpRect = UIRoot.MouseUpRectangle,
        Down = UIRoot.MouseDown
      };
      KeyboardState newState = Keyboard.GetState();
      KeyboardEvent latestKeyboardEvent = new() {
        NewKeyboardState = newState,
        OldKeyboardState = oldKeyboardState
      };

      bool mouseEventConsumed = false;
      bool keyboardEventConsumed = false;
      if (RootElement != null) {
        mouseEventConsumed = RootElement.InputUpdate(latestMouseEvent);
        keyboardEventConsumed = RootElement.InputUpdate(latestKeyboardEvent);
      }

      if (!keyboardEventConsumed) KeyboardInputUpdateEvent?.Invoke(latestKeyboardEvent);
      if (!mouseEventConsumed) PointerInputUpdateEvent?.Invoke(latestMouseEvent);

      oldKeyboardState = Keyboard.GetState();
    }

    public void Draw(SpriteBatch spriteBatch, double deltaTime) {
      if (RootElement != null) {
        // Make Sure the RootElement fills the entire viewport
        Vector2 screenSize = new(spriteBatch.GraphicsDevice.Viewport.Width, spriteBatch.GraphicsDevice.Viewport.Height);
        RootElement.Size = screenSize;
        RootElement.MaximumSize = screenSize;
        RootElement.AbsolutePosition = Vector2.Zero;

        spriteBatch.Begin();

        BackgroundRectDrawable?.Draw(spriteBatch, deltaTime, screenSize, Vector2.Zero);

        RootElement.Draw(spriteBatch, deltaTime);

        spriteBatch.End();
      }
      
    }
  }
}