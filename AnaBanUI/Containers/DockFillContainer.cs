using LiVerse.AnaBanUI.Drawables;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System.Reflection.PortableExecutable;

namespace LiVerse.AnaBanUI.Containers {
  public enum DockFillContainerDockType {
    Top, Right, Bottom, Left
  }
  public enum DockFillContainerFillElementScalingStyle {
    Fill, KeepMinimunSize
  }

  public class DockFillContainer : ContainerBase {
    public ControlBase? DockElement { get; set; }
    public ControlBase? FillElement { get; set; }
    public RectangleDrawable? BackgroundRectDrawble { get; set; }
    public RectangleDrawable? ForegroundRectDrawble { get; set; }

    /// <summary>
    /// The location that the dock element will be placed
    /// </summary>
    public DockFillContainerDockType DockType { get; set; }
    public bool Lines = false;

    public DockFillContainer(ControlBase? dockElement = null, ControlBase? fillElement = null) {
      DockType = DockFillContainerDockType.Top;

      DockElement = dockElement;
      FillElement = fillElement;
    }

    void FixMinimunSize(ControlBase control) {
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

      // Check if there's nothing to calculate
      if (FillElement == null || DockElement == null) { return; }

      // Fill Fill Element if the Dock Element is invisible
      if (!DockElement.Visible) {
        FillControl(FillElement);
        return;
      }

      // Fill Dock Element if the Fill Element is invisible
      if (!FillElement.Visible) {
        FillControl(DockElement);
        return;
      }

      if (DockType == DockFillContainerDockType.Top) {
        DockElement.Size = new Vector2(ContentArea.X, DockElement.MinimumSize.Y); // Set element height to minimum size
        DockElement.RelativePosition = Vector2.Zero;
        DockElement.AbsolutePosition = AbsolutePosition;

        FillElement.Size = new Vector2(ContentArea.X, ContentArea.Y - DockElement.Size.Y);
        FixMinimunSize(FillElement);

        FillElement.RelativePosition = new Vector2(0, DockElement.Size.Y);
        FillElement.AbsolutePosition = AbsolutePosition + FillElement.RelativePosition;

        // Calculate MinimiumSize
        float minimumWidth = DockElement.MinimumSize.X;
        if (FillElement.MinimumSize.X > minimumWidth) { minimumWidth = FillElement.MinimumSize.X; }

        MinimumSize = new Vector2(minimumWidth, DockElement.MinimumSize.Y + FillElement.MinimumSize.Y);
      }

      if (DockType == DockFillContainerDockType.Bottom) {
        FillElement.Size = new Vector2(Size.X - Margin * 2, (Size.Y - Margin) - DockElement.Size.Y - Margin * 2);
        FixMinimunSize(FillElement);
        FillElement.RelativePosition = new Vector2(Margin);
        FillElement.AbsolutePosition = AbsolutePosition + FillElement.RelativePosition;

        DockElement.Size = new Vector2(DockElement.MinimumSize.X, DockElement.MinimumSize.Y);
        DockElement.RelativePosition = new Vector2(Margin, FillElement.Size.Y + Margin * 2);
        DockElement.AbsolutePosition = AbsolutePosition + DockElement.RelativePosition;

        // Calculate MinimiumSize
        float minimumWidth = DockElement.MinimumSize.X + Margin * 2;
        if (FillElement.MinimumSize.X > minimumWidth) { minimumWidth = FillElement.MinimumSize.X + Margin * 2; }

        MinimumSize = new Vector2(minimumWidth, FillElement.MinimumSize.Y + Margin * 2 + DockElement.MinimumSize.Y + Margin * 2);
      }

      if (DockType == DockFillContainerDockType.Left) {
        DockElement.Size = new Vector2(DockElement.MinimumSize.X, ContentArea.Y);
        DockElement.RelativePosition = Vector2.Zero;
        DockElement.AbsolutePosition = AbsolutePosition + DockElement.RelativePosition;

        FillElement.Size = new Vector2(ContentArea.X - DockElement.Size.X, ContentArea.Y);
        FillElement.RelativePosition = new Vector2(DockElement.Size.X, 0);
        FillElement.AbsolutePosition = AbsolutePosition + FillElement.RelativePosition;

        // Calculate MinimiumSize
        float minimumHeight = DockElement.MinimumSize.Y;
        if (FillElement.MinimumSize.Y > minimumHeight) { minimumHeight = FillElement.MinimumSize.Y; }

        MinimumSize = new Vector2(DockElement.MinimumSize.X + FillElement.MinimumSize.X, minimumHeight + Margin);
      }

      if (DockType == DockFillContainerDockType.Right) {
        FillElement.Size = new Vector2(Size.X - DockElement.Size.X, Size.Y);
        FillElement.RelativePosition = Vector2.Zero;
        FillElement.AbsolutePosition = AbsolutePosition;

        DockElement.Size = new Vector2(DockElement.MinimumSize.X, Size.Y);
        DockElement.RelativePosition = new Vector2(FillElement.Size.X, 0);
        DockElement.AbsolutePosition = new Vector2(AbsolutePosition.X + FillElement.Size.X, AbsolutePosition.Y);


        // Calculate MinimiumSize
        float minimumHeight = DockElement.MinimumSize.Y;
        if (FillElement.MinimumSize.Y > minimumHeight) { minimumHeight = FillElement.MinimumSize.Y; }

        MinimumSize = new Vector2(DockElement.MinimumSize.X + FillElement.MinimumSize.X, minimumHeight);
      }

    }
    public override void DrawElement(SpriteBatch spriteBatch, double deltaTime) {
      RecalculateUI();

      if (BackgroundRectDrawble != null) BackgroundRectDrawble.Draw(spriteBatch, deltaTime, ContentArea, Vector2.Zero);

      if (DockElement != null) DockElement.Draw(spriteBatch, deltaTime);
      if (FillElement != null) FillElement.Draw(spriteBatch, deltaTime);
    }

    public override void Update(double deltaTime) {
      if (DockElement != null && DockElement.Visible) DockElement.Update(deltaTime);
      if (FillElement != null && FillElement.Visible) FillElement.Update(deltaTime);
    }
  }
}