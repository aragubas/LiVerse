using LiVerse.AnaBanUI;
using LiVerse.AnaBanUI.Containers;
using LiVerse.AnaBanUI.Controls;
using LiVerse.src.AnaBanUI;
using LiVerse.src.AnaBanUI.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;

namespace LiVerse {
  public class LiVerseApp : Game {
    GraphicsDeviceManager graphics { get; }
    SpriteBatch? spriteBatch;
    IWindowRoot? windowUIRoot;

    // MainUI Members
    DockFillContainer mainFillContainer = new();


    public LiVerseApp() {
      graphics = new GraphicsDeviceManager(this);

      // Enables VSync
      graphics.SynchronizeWithVerticalRetrace = true;
      graphics.ApplyChanges();
      
      IsMouseVisible = true;
      IsFixedTimeStep = false;

      InactiveSleepTime = TimeSpan.Zero;
    }

    protected override void Initialize() {
      Window.Title = "LiVerse";
      Window.AllowUserResizing = true;
      

      // Creates the sprite batch
      spriteBatch = new SpriteBatch(graphics.GraphicsDevice);

      // Load base resources
      ResourceManager.LoadBaseResources(GraphicsDevice); 

      // Creates UIRoot
      windowUIRoot = new WindowRoot();

      DockFillContainer HeaderBar = new DockFillContainer();
      DockFillContainer centerSplit = new DockFillContainer();


      Label characterNameLabel = new Label("{character_name}", 22);
      Label placeholderLabel = new Label("Placeholder", 42);
      Button charactersButton = new Button("Characters");
      VerticalLevelTrigger levelTrigger = new VerticalLevelTrigger();

      HeaderBar.DockType = DockFillContainerDockType.Left;
      HeaderBar.DockElement = charactersButton;
      HeaderBar.FillElement = characterNameLabel;

      centerSplit.DockType = DockFillContainerDockType.Left;
      centerSplit.DockElement = levelTrigger;
      centerSplit.FillElement = placeholderLabel;


      //HeaderBar.Lines = true;
      mainFillContainer.DockElement = HeaderBar;
      mainFillContainer.FillElement = centerSplit;

      windowUIRoot.RootElement = mainFillContainer;      
    }

    protected override void Update(GameTime gameTime) {
      if (windowUIRoot == null) { return; }

      // Update UIRoot
      UIRoot.Update(gameTime.GetElapsedSeconds());

      windowUIRoot.Update(gameTime.ElapsedGameTime.TotalSeconds);
    }


    protected override void Draw(GameTime gameTime) {
      if (spriteBatch == null || windowUIRoot == null) { Exit(); return; }

      GraphicsDevice.Clear(Color.CornflowerBlue);
 
      windowUIRoot.Draw(spriteBatch, gameTime.ElapsedGameTime.TotalSeconds);
    }
    
  }
}
