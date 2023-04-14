using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace LiVerse.AnaBanUI {
  public abstract class ControlBase {
    /// A disabled control doesn't accept user input, but its still rendered if visible
    public bool Enabled { get; set; } = true;
    /// A disabled control doesn't accept user input, but its still rendered if visible
    public bool DrawDebugLines { get; set; } = false;
    /// A invisible control is invisible and doesn't process any events
    public bool Visible { get; set; } = true;
    /// Backing field for ElementSize
    protected Vector2 _size { get; set; }
    
    /// <summary>
    /// Total Size, including padding/margins.<br></br>Don't change this value manually, this value should be determined by the container
    /// </summary>
    public Vector2 Size { 
      get => _size; 
      set {
        if (value == _size) {
          return;
        }

        _size = value;
        _abosoluteArea = new RectangleF(_absolutePosition, _size);
        ElementSizeChanged();
      }
    }

    public Vector2 ContentArea { get => new Vector2(Size.X - Margin * 2, Size.Y - Margin * 2); }

    public float Margin { get; set; } = 0;

    Vector2 _minimumSize = Vector2.Zero;
    public Vector2 MinimumSize { get => _minimumSize;
      set {
        //if (value == _minimumSize) { return; }

        _minimumSize = new Vector2(value.X + Margin, value.Y + Margin);
      }
    }

    public Vector2? MaximumSize { get; set; } = null;
    /// Absolute Position on Screen, Used for collision detection.
    Vector2 _absolutePosition = Vector2.Zero;
    public Vector2 AbsolutePosition {
      get => _absolutePosition;
      set {
        //if (value == _absolutePosition) { return; }

        _absolutePosition = value + new Vector2(Margin);
        _abosoluteArea = new RectangleF(_absolutePosition, _size);
      }
    }
    public Vector2 RelativePosition { get; set; }
    RectangleF _abosoluteArea = RectangleF.Empty;
    public RectangleF AbsoluteArea { get => _abosoluteArea; }

    protected virtual void ElementSizeChanged() { }

    public abstract void Update(double deltaTime);
    
    /// <summary>
    /// Applies a translation matrix and calls <seealso cref="DrawElement"/>
    /// </summary>
    public virtual void Draw(SpriteBatch spriteBatch, double deltaTime) {
      if (!Visible) { return; }

      Viewport oldViewport = spriteBatch.GraphicsDevice.Viewport;

      spriteBatch.End();
      spriteBatch.GraphicsDevice.Viewport = new Viewport((int)(AbsolutePosition.X), (int)(AbsolutePosition.Y), (int)Size.X, (int)Size.Y);
      spriteBatch.Begin();
      DrawElement(spriteBatch, deltaTime);

      if (DrawDebugLines) {
        if (Margin != 0) {
          spriteBatch.DrawRectangle(new RectangleF(Vector2.Zero, ContentArea), Color.Magenta);
        }

        spriteBatch.End();
        spriteBatch.GraphicsDevice.Viewport = new Viewport((int)(AbsolutePosition.X - Margin), (int)(AbsolutePosition.Y - Margin), (int)Size.X, (int)Size.Y);
        spriteBatch.Begin();

        spriteBatch.DrawRectangle(new RectangleF(Vector2.Zero, Size), Color.Blue);
      }

      spriteBatch.End();
      spriteBatch.GraphicsDevice.Viewport = oldViewport;
      spriteBatch.Begin();

    }

    /// <summary>
    /// Draws the element. Keep in mind that <u>the origin point is always (0, 0)</u>
    /// </summary>
    public abstract void DrawElement(SpriteBatch spriteBatch, double deltaTime);
  }
}