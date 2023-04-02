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
    public DockFillContainerDockType DockType { get; set; }
    public float Gap = 0;
    public bool Lines = true;

    public DockFillContainer(ControlBase? dockElement = null, ControlBase? fillElement = null) {
      DockType = DockFillContainerDockType.Top;
      
      DockElement = dockElement;
      FillElement = fillElement;
    }

    void RecalculateUI() {
      if (DockType == DockFillContainerDockType.Top) {
        DockElement.AbsolutePosition = AbsolutePosition;
        DockElement.Size = new Vector2(Size.X, DockElement.MinimumSize.Y); // Set element height to minimum size
   
        FillElement.Size = new Vector2(Size.X, Size.Y - DockElement.Size.Y);
         
        if (FillElement.Size.X < FillElement.MinimumSize.X) { FillElement.Size = new Vector2(FillElement.MinimumSize.X, FillElement.Size.Y); }
        if (FillElement.Size.Y < FillElement.MinimumSize.Y) { FillElement.Size = new Vector2(FillElement.Size.X, FillElement.MinimumSize.Y); }

        FillElement.RelativePosition = new Vector2(0, DockElement.Size.Y); 
        FillElement.AbsolutePosition = new Vector2(AbsolutePosition.X, DockElement.Size.Y + AbsolutePosition.Y);
      } 
    }

    void DrawElement(SpriteBatch spriteBatch, ControlBase element) {
      Viewport elementViewport = new Viewport((int)element.AbsolutePosition.X, (int)element.AbsolutePosition.Y, (int)element.Size.X, (int)element.Size.Y);
      Viewport oldViewport = spriteBatch.GraphicsDevice.Viewport;
  
      spriteBatch.End();
      spriteBatch.Begin();

      spriteBatch.GraphicsDevice.Viewport = elementViewport; 

      element.Draw(spriteBatch);
      
 
      spriteBatch.End();

      // Restore SpriteBatch
      spriteBatch.GraphicsDevice.Viewport = oldViewport; 

      spriteBatch.Begin();
    }

    public override void Draw(SpriteBatch spriteBatch) {
      RecalculateUI(); 

      spriteBatch.End();
      spriteBatch.Begin();

      if (DockElement != null) DrawElement(spriteBatch, DockElement);
      if (FillElement != null) DrawElement(spriteBatch, FillElement);
      
      if (Lines) spriteBatch.DrawRectangle(new RectangleF(0, 0, Size.X, Size.Y), Color.Magenta);

      spriteBatch.End();
      spriteBatch.Begin();

    }

    public override void Update(double deltaTime) {
      if (DockElement != null) DockElement.Update(deltaTime);
      if (FillElement != null) FillElement.Update(deltaTime);
    }
  }
}