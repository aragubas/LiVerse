using SFML.Graphics;
using SFML.System;

namespace LiVerse.AnaBanUI;

public abstract class Control : Drawable {
  /// <summary>
  /// Total size, including padding. Don not change this manually, this value should be set by the container
  /// </summary>
  public Vector2f Size { get; set; } = new Vector2f(0, 0);

  /// <summary>
  /// Area inside the control. (Size - Padding * 2)
  /// </summary>
  public Vector2f ContentArea {
    //get => new Vector2f(Size.X - Padding.X * 2, Size.Y - Padding.Y * 2);
    get => Size - (Padding * 2);
  }

  /// <summary>
  /// Space from outside size border to the content area
  /// </summary>
  public Vector2f Padding { get; set; } = new Vector2f(0, 0);

  /// <summary>
  /// Minimum size. (-1, -1) = unset
  /// </summary>
  public Vector2f MinimumSize { get; set; } = new Vector2f(-1, -1);

  /// <summary>
  /// Maximum size. (-1, -1) = unset
  /// </summary>
  public Vector2f MaximumSize { get; set; } = new Vector2f(-1, -1);

  /// <summary>
  /// Relative position inside container space. Don not change this manually, this value should be set by the container
  /// </summary>
  public Vector2f RelativePosition { get; set; }

  /// <summary>
  /// Absolute position relative to Window's Top Left corner
  /// </summary>
  public Vector2f AbsolutePosition { get; set; }

  public abstract void Update(double deltaTime);
  public void Draw(RenderTarget target, RenderStates states) {

    states.Transform.Translate(RelativePosition);

    DoDraw(target, states);

    // Restore transform
    states.Transform.Translate(-RelativePosition);
  }
  protected abstract void DoDraw(RenderTarget target, RenderStates states);
}