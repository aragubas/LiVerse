using LiVerse.AnaBanUI.Drawables;
using LiVerse.AnaBanUI.Events;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LiVerse.AnaBanUI.Containers; 
public enum DockFillContainerDockDirection {
  Top, Right, Bottom, Left
}
public enum DockFillContainerFillElementScalingStyle {
  Fill, KeepMinimunSize
}

// TODO: Implement Gap property on all DockType modes
public class DockFillContainer : ContainerBase {
  public ControlBase? DockElement { get; set; }
  public ControlBase? FillElement { get; set; }
  public RectangleDrawable? BackgroundRectDrawable { get; set; }
  public float Gap { get; set; } = 0;

  /// <summary>
  /// The location that the dock element will be placed
  /// </summary>
  public DockFillContainerDockDirection DockType { get; set; }

  public DockFillContainer(ControlBase? dockElement = null, ControlBase? fillElement = null) {
    DockType = DockFillContainerDockDirection.Top;

    DockElement = dockElement;
    FillElement = fillElement;
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

    if (DockType == DockFillContainerDockDirection.Top) {
      DockElement.Size = new Vector2(ContentArea.X, DockElement.MinimumSize.Y); // Set element height to minimum size
      DockElement.RelativePosition = Vector2.Zero;
      DockElement.AbsolutePosition = AbsolutePosition;

      FillElement.Size = new Vector2(ContentArea.X, ContentArea.Y - DockElement.Size.Y - Gap);

      FillElement.RelativePosition = new Vector2(0, DockElement.Size.Y + Gap);
      FillElement.AbsolutePosition = AbsolutePosition + FillElement.RelativePosition;


      // Calculate MinimiumSize
      float minimumWidth = DockElement.MinimumSize.X;
      if (FillElement.MinimumSize.X > minimumWidth) { minimumWidth = FillElement.MinimumSize.X; }

      MinimumSize = new Vector2(minimumWidth, DockElement.MinimumSize.Y + FillElement.MinimumSize.Y + Gap);
    }

    if (DockType == DockFillContainerDockDirection.Bottom) {
      FillElement.Size = new Vector2(ContentArea.X, ContentArea.Y - DockElement.Size.Y - Gap);
      FillElement.RelativePosition = Vector2.Zero;
      FillElement.AbsolutePosition = AbsolutePosition + FillElement.RelativePosition;

      DockElement.Size = new Vector2(ContentArea.X, DockElement.MinimumSize.Y);
      DockElement.RelativePosition = new Vector2(0, FillElement.Size.Y + Gap);
      DockElement.AbsolutePosition = AbsolutePosition + DockElement.RelativePosition;

      // Calculate MinimiumSize
      float minimumWidth = DockElement.MinimumSize.X + Margin.X;
      if (FillElement.MinimumSize.X > minimumWidth) { minimumWidth = FillElement.MinimumSize.X + Margin.X; }

      MinimumSize = new Vector2(minimumWidth, FillElement.MinimumSize.Y + Margin.Y * 2 + DockElement.MinimumSize.Y + Margin.Y * 2 + Gap);
    }

    if (DockType == DockFillContainerDockDirection.Left) {
      DockElement.Size = new Vector2(DockElement.MinimumSize.X, ContentArea.Y);
      DockElement.RelativePosition = Vector2.Zero;
      DockElement.AbsolutePosition = AbsolutePosition + DockElement.RelativePosition;

      FillElement.Size = new Vector2(ContentArea.X - DockElement.Size.X - Gap, ContentArea.Y);
      FillElement.RelativePosition = new Vector2(DockElement.Size.X + Gap, 0);
      FillElement.AbsolutePosition = AbsolutePosition + FillElement.RelativePosition;

      // Calculate MinimiumSize
      float minimumHeight = DockElement.MinimumSize.Y;
      if (FillElement.MinimumSize.Y > minimumHeight) { minimumHeight = FillElement.MinimumSize.Y; }

      MinimumSize = new Vector2(DockElement.MinimumSize.X + FillElement.MinimumSize.X, minimumHeight + Margin.Y);
    }

    if (DockType == DockFillContainerDockDirection.Right) {
      FillElement.Size = new Vector2(ContentArea.X - DockElement.Size.X - Gap, ContentArea.Y);
      FillElement.RelativePosition = Vector2.Zero;
      FillElement.AbsolutePosition = AbsolutePosition + FillElement.RelativePosition;

      DockElement.Size = new Vector2(DockElement.MinimumSize.X, ContentArea.Y);
      DockElement.RelativePosition = new Vector2(FillElement.Size.X + Gap, 0);
      DockElement.AbsolutePosition = AbsolutePosition + DockElement.RelativePosition;

      // Calculate MinimiumSize
      float minimumHeight = DockElement.MinimumSize.Y;
      if (FillElement.MinimumSize.Y > minimumHeight) { minimumHeight = FillElement.MinimumSize.Y; }

      MinimumSize = new Vector2(DockElement.MinimumSize.X + FillElement.MinimumSize.X, minimumHeight);
    }

    DockElement.ParentControl = this;
    FillElement.ParentControl = this;
  }

  public override void UpdateUI(double deltaTime) {
    RecalculateUI();
  }

  public override void DrawControl(SpriteBatch spriteBatch, double deltaTime) {
    BackgroundRectDrawable?.Draw(spriteBatch, deltaTime, ContentArea, Vector2.Zero);

    DockElement?.Draw(spriteBatch, deltaTime);
    FillElement?.Draw(spriteBatch, deltaTime);
  }

  public override bool InputUpdate(PointerEvent pointerEvent) {
    if (DockElement != null && DockElement.Visible) if (DockElement.InputUpdate(pointerEvent)) return true;
    if (FillElement != null && FillElement.Visible) if (FillElement.InputUpdate(pointerEvent)) return true;

    return false;
  }

  public override bool InputUpdate(KeyboardEvent keyboardEvent) {
    if (DockElement != null && DockElement.Visible) if (DockElement.InputUpdate(keyboardEvent)) return true;
    if (FillElement != null && FillElement.Visible) if (FillElement.InputUpdate(keyboardEvent)) return true;

    return false;
  }

  public override void Update(double deltaTime) {
    if (DockElement != null && DockElement.Visible) DockElement.Update(deltaTime);
    if (FillElement != null && FillElement.Visible) FillElement.Update(deltaTime);
  }
}