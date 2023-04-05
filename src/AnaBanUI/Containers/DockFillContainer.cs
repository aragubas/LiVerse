using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace LiVerse.AnaBanUI.Containers {
  public enum DockFillContainerDockType {
    Top, Right, Bottom, Left
  }
  
  public class DockFillContainer : ControlBase
  {
    public ControlBase? DockElement { get; set; }
    public ControlBase? FillElement { get; set; }
    
    /// <summary>
    /// The location that the dock element will be placed
    /// </summary>
    public DockFillContainerDockType DockType { get; set; }
    public float Gap = 0;
    public bool Lines = false;

    public DockFillContainer(ControlBase? dockElement = null, ControlBase? fillElement = null) {
      DockType = DockFillContainerDockType.Top;
      
      DockElement = dockElement;
      FillElement = fillElement;
    }
    
    void FixMinimunSize(ControlBase control)
    {
      if (control.Size.X < control.MinimumSize.X) { control.Size = new Vector2(control.MinimumSize.X, control.Size.Y); }
      if (control.Size.Y < control.MinimumSize.Y) { control.Size = new Vector2(control.Size.X, control.MinimumSize.Y); }
    }
    
    void FillControl(ControlBase element) {
      element.Size = Size; // Set element height to minimum size
      element.AbsolutePosition = AbsolutePosition;
      element.RelativePosition = Vector2.Zero;

      MinimumSize = element.MinimumSize;
    }

    void RecalculateUI() {
      // Fill Dock Element if its the only one set
      if (FillElement == null && DockElement != null) { FillControl(DockElement); return; }

      // Fill Fill Element if its the only one set
      if (FillElement != null && DockElement == null) { FillControl(FillElement); return; }


      if (DockType == DockFillContainerDockType.Top) {
        DockElement.Size = new Vector2(Size.X, DockElement.MinimumSize.Y); // Set element height to minimum size
        DockElement.AbsolutePosition = AbsolutePosition;
        DockElement.RelativePosition = Vector2.Zero;

        FillElement.Size = new Vector2(Size.X, Size.Y - DockElement.Size.Y);
        FixMinimunSize(FillElement);

        FillElement.RelativePosition = new Vector2(0, DockElement.Size.Y);
        FillElement.AbsolutePosition = new Vector2(AbsolutePosition.X, DockElement.Size.Y + AbsolutePosition.Y);

        // Calculate MinimiumSize
        float minimumWidth = DockElement.MinimumSize.X;
        if (FillElement.MinimumSize.X > minimumWidth) { minimumWidth = FillElement.MinimumSize.X; }

        MinimumSize = new Vector2(minimumWidth, DockElement.MinimumSize.Y + FillElement.MinimumSize.Y);
      }

      if (DockType == DockFillContainerDockType.Bottom) {
        FillElement.Size = new Vector2(Size.X, Size.Y - DockElement.Size.Y); // Set element height to minimum size
        FixMinimunSize(FillElement);
        FillElement.AbsolutePosition = AbsolutePosition;
        FillElement.RelativePosition = Vector2.Zero;

        DockElement.Size = new Vector2(Size.X, DockElement.MinimumSize.Y);
        DockElement.AbsolutePosition = new Vector2(AbsolutePosition.X, FillElement.Size.Y + AbsolutePosition.Y);
        DockElement.RelativePosition = new Vector2(0, FillElement.Size.Y);

        // Calculate MinimiumSize
        float minimumWidth = DockElement.MinimumSize.X;
        if (FillElement.MinimumSize.X > minimumWidth) { minimumWidth = FillElement.MinimumSize.X; }

        MinimumSize = new Vector2(minimumWidth, DockElement.MinimumSize.Y + FillElement.MinimumSize.Y);
      }

      if (DockType == DockFillContainerDockType.Left) {
        DockElement.Size = new Vector2(DockElement.MinimumSize.X, Size.Y);
        DockElement.AbsolutePosition = AbsolutePosition;
        DockElement.RelativePosition = Vector2.Zero;

        FillElement.Size = new Vector2(Size.X - DockElement.Size.X, Size.Y);
        FillElement.RelativePosition = new Vector2(DockElement.Size.X, 0);
        FillElement.AbsolutePosition = new Vector2(AbsolutePosition.X + DockElement.Size.X, AbsolutePosition.Y);

        // Calculate MinimiumSize
        float minimumHeight = DockElement.MinimumSize.Y;
        if (FillElement.MinimumSize.Y > minimumHeight) { minimumHeight = FillElement.MinimumSize.Y; }

        MinimumSize = new Vector2(DockElement.MinimumSize.X + FillElement.MinimumSize.X, minimumHeight);
      }

      if (DockType == DockFillContainerDockType.Right) {
        FillElement.Size = new Vector2(Size.X - DockElement.Size.X, Size.Y);
        FillElement.RelativePosition = Vector2.Zero;
        FillElement.AbsolutePosition = AbsolutePosition;

        DockElement.Size = new Vector2(DockElement.MinimumSize.X, Size.Y);
        DockElement.AbsolutePosition = new Vector2(AbsolutePosition.X + FillElement.Size.X, AbsolutePosition.Y);
        DockElement.RelativePosition = new Vector2(FillElement.Size.X, 0);


        // Calculate MinimiumSize
        float minimumHeight = DockElement.MinimumSize.Y;
        if (FillElement.MinimumSize.Y > minimumHeight) { minimumHeight = FillElement.MinimumSize.Y; }

        MinimumSize = new Vector2(DockElement.MinimumSize.X + FillElement.MinimumSize.X, minimumHeight);
      }

      //MinimumSize = Vector2.Zero;
      //if (DockElement.MinimumSize.X > MinimumSize.X) { MinimumSize = new Vector2(DockElement.MinimumSize.X, MinimumSize.Y); }
      //if (DockElement.MinimumSize.Y > MinimumSize.Y) { MinimumSize = new Vector2(MinimumSize.X, DockElement.MinimumSize.Y); }

      //if (FillElement.MinimumSize.X > MinimumSize.X) { MinimumSize = new Vector2(FillElement.MinimumSize.X, MinimumSize.Y); }
      //if (FillElement.MinimumSize.Y > MinimumSize.Y) { MinimumSize = new Vector2(MinimumSize.X, FillElement.MinimumSize.Y); }


    }

    void DrawElement(SpriteBatch spriteBatch, ControlBase element) {
  
      spriteBatch.End();
      spriteBatch.Begin(transformMatrix: Matrix.CreateTranslation(element.RelativePosition.X, element.RelativePosition.Y, 0));

      //spriteBatch.GraphicsDevice.Viewport = elementViewport; 

      element.Draw(spriteBatch);
      if (Lines) spriteBatch.DrawRectangle(new RectangleF(0, 0, element.Size.X, element.Size.Y), Color.Red);

      spriteBatch.End();

      // Restore SpriteBatch
      //spriteBatch.GraphicsDevice.Viewport = oldViewport; 

      spriteBatch.Begin();
    }

    public override void Draw(SpriteBatch spriteBatch) {
      RecalculateUI();

      Viewport elementViewport = new Viewport((int)AbsolutePosition.X, (int)AbsolutePosition.Y, (int)Size.X, (int)Size.Y);
      Viewport oldViewport = spriteBatch.GraphicsDevice.Viewport;

      spriteBatch.End();
      spriteBatch.Begin();

      spriteBatch.GraphicsDevice.Viewport = elementViewport;

      if (Lines) spriteBatch.DrawRectangle(new RectangleF(0, 0, MinimumSize.X, MinimumSize.Y), Color.Blue);

      if (DockElement != null) DrawElement(spriteBatch, DockElement);
      if (FillElement != null) DrawElement(spriteBatch, FillElement);

      if (Lines) spriteBatch.DrawRectangle(new RectangleF(0, 0, Size.X, Size.Y), Color.Magenta);


      spriteBatch.End();

      //Restore SpriteBatch
      spriteBatch.GraphicsDevice.Viewport = oldViewport;


      spriteBatch.Begin();


    }

    public override void Update(double deltaTime) {
      if (DockElement != null) DockElement.Update(deltaTime);
      if (FillElement != null) FillElement.Update(deltaTime);
    }
  }
}