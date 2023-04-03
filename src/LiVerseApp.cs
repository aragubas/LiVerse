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
      
      IsMouseVisible = true;
      IsFixedTimeStep = false;

      InactiveSleepTime = TimeSpan.Zero;
    }

    Label testLabel2;
    Label testLabel3;
    Label testLabel4;
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
      DockFillContainer secondContainer = new DockFillContainer();
      DockFillContainer thirdContainer = new DockFillContainer();
      Label testLabel = new Label("1st Label", 32);
      testLabel2 = new Label("2nd", 32);
      testLabel3 = new Label("3rd label", 32);
      testLabel4 = new Label("4th label", 32);

      thirdContainer.DockType = DockFillContainerDockType.Bottom;
      thirdContainer.Lines = true;

      fillContainer.DockElement = testLabel;
      fillContainer.FillElement = secondContainer;
      
      secondContainer.DockElement = thirdContainer;
      secondContainer.FillElement = testLabel2;

      thirdContainer.DockElement = testLabel3;
      thirdContainer.FillElement = testLabel4;

      windowUIRoot.RootElement = fillContainer;
      
    }

    int size = 18;
    protected override void Update(GameTime gameTime) {
      if (windowUIRoot == null) { return; }
      
      testLabel2.Text = Mouse.GetState().Position.ToString();
      testLabel3.Text = gameTime.ElapsedGameTime.TotalSeconds.ToString().PadRight(7, '0');
      testLabel4.FontSize = size;

      if (Keyboard.GetState().IsKeyDown(Keys.A)) {
        size--;
        if (size < -1) { size = -1; }
      }
      if (Keyboard.GetState().IsKeyDown(Keys.D)) {
        size++;
      }

      windowUIRoot.Update(gameTime.ElapsedGameTime.TotalSeconds);
    }


    protected override void Draw(GameTime gameTime) {
      if (spriteBatch == null || windowUIRoot == null) { Exit(); return; }

      GraphicsDevice.Clear(Color.CornflowerBlue);
 
      windowUIRoot.Draw(spriteBatch);
    }
    
  }
}
