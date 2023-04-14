using LiVerse.AnaBanUI;
using LiVerse.AnaBanUI.Drawables;
using LiVerse.AnaBanUI.Events;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace LiVerse.AnaBanUI {
  public class UILayer {
    public ControlBase? RootElement { get; set; }    
    public RectangleDrawable? BackgroundRectDrawable { get; set; }
    public event Action? InputUpdateEvent;
    PointerEvent lastMouseEvent;

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

      bool mouseEventConsumed = false;
      if (RootElement != null) {
        if (latestMouseEvent != lastMouseEvent) { mouseEventConsumed = RootElement.InputUpdate(latestMouseEvent); }
      }

      lastMouseEvent = latestMouseEvent;

      InputUpdateEvent?.Invoke();
    }

    public void Draw(SpriteBatch spriteBatch, double deltaTime) {
      if (RootElement != null) {
        // Make Sure the RootElement fills the entire viewport
        Vector2 screenSize = new Vector2(spriteBatch.GraphicsDevice.Viewport.Width, spriteBatch.GraphicsDevice.Viewport.Height);
        RootElement.Size = screenSize;
        RootElement.MaximumSize = screenSize;
        if (RootElement.Margin > 1) {
           RootElement.AbsolutePosition = Vector2.Zero;
        }

        spriteBatch.Begin();

        if (BackgroundRectDrawable != null) BackgroundRectDrawable.Draw(spriteBatch, deltaTime, screenSize, Vector2.Zero);

        RootElement.Draw(spriteBatch, deltaTime);

        spriteBatch.End();
      }
      
    }
  }
}