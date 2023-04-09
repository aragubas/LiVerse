using LiVerse.AnaBanUI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace LiVerse.AnaBanUI {
  public class UILayer : IUILayer {
    public ControlBase? RootElement { get; set; }    

    public UILayer() {
      RootElement = null;      
    }

    public void Update(double deltaTime) {
      if (RootElement != null) {
        RootElement.Update(deltaTime);
      }
    }

    public void Draw(SpriteBatch spriteBatch, double deltaTime) {
      if (RootElement != null) {
        // Make Sure the RootElement fills the entire viewport
        Vector2 screenSize = new Vector2(spriteBatch.GraphicsDevice.Viewport.Width, spriteBatch.GraphicsDevice.Viewport.Height);
        RootElement.Size = screenSize;
        RootElement.MaximumSize = screenSize;

        spriteBatch.Begin();
        
        RootElement.Draw(spriteBatch, deltaTime);

        spriteBatch.End();
      }
      
    }
  }
}