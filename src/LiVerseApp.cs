using LiVerse.AnaBanUI;
using LiVerse.AnaBanUI.Containers;
using LiVerse.AnaBanUI.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;

namespace LiVerse {
  public class LiVerseApp : Game {
    GraphicsDeviceManager graphics { get; }
    SpriteBatch? spriteBatch;
    IWindowRoot? windowUIRoot;

    public LiVerseApp() {
      graphics = new GraphicsDeviceManager(this);

      // Enables VSync
      graphics.SynchronizeWithVerticalRetrace = true;
      graphics.ApplyChanges();
      
    }

    Label testLabel2;
    protected override void Initialize() {
      Window.Title = "LiVerse";
      Window.AllowUserResizing = true;
      

      // Creates the sprite batch
      spriteBatch = new SpriteBatch(graphics.GraphicsDevice);

      // Load base resources
      ResourceManager.LoadBaseResources(GraphicsDevice); 

      // Creates UIRoot
      windowUIRoot = new WindowRoot();

      DockFillContainer fillContainer = new DockFillContainer();
      Label testLabel = new Label("DockElement Label", 24);
      testLabel2 = new Label("FillElement Label", 24);
       
      fillContainer.DockElement = testLabel;
      fillContainer.FillElement = testLabel2;
        
      windowUIRoot.RootElement = fillContainer;
      
    }

    protected override void Update(GameTime gameTime) {
      if (windowUIRoot == null) { return; }
      
      testLabel2.Text = Mouse.GetState().Position.ToString();

      windowUIRoot.Update(gameTime.ElapsedGameTime.TotalSeconds);
    }


    protected override void Draw(GameTime gameTime) {
      if (spriteBatch == null || windowUIRoot == null) { Exit(); return; }

      GraphicsDevice.Clear(Color.CornflowerBlue);
 
      windowUIRoot.Draw(spriteBatch);
    }
    
  }
}
