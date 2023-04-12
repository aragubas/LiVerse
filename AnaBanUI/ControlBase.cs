using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LiVerse.AnaBanUI {
  public abstract class ControlBase {
    /// A disabled control doesn't accept user input, but its still rendered if visible
    public bool Enabled { get; set; } = true;
    /// A invisible control is invisible and doesn't process any events
    public bool Visible { get; set; } = true;
    /// Hints the container to hide Control's overflow by drawing it inside a viewport
    public bool HideOverflow { get; set; } = true;
    /// Backing field for ElementSize
    protected Vector2 _elementSize { get; set; }
    
    /// <summary>
    /// Total Size, including padding/margins.<br></br>Don't change this value manually, this value should be determined by the container
    /// </summary>
    public Vector2 Size { 
      get => _elementSize; 
      set {
        if (value == _elementSize) {
          return;
        }

        _elementSize = value;
        ElementSizeChanged();
      }
    }

    public Vector2 MinimumSize { get; set; }

    public Vector2? MaximumSize { get; set; } = null;
    /// Absolute Position on Screen, Used for collision detection.
    public Vector2 AbsolutePosition { get; set; }
    public Vector2 RelativePosition { get; set; }
 
 
    protected virtual void ElementSizeChanged() { }

    public virtual void Update(double deltaTime) { }
    public virtual void Draw(SpriteBatch spriteBatch, double deltaTime) { }
  }
}