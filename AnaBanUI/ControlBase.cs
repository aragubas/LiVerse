using LiVerse.AnaBanUI.Events;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace LiVerse.AnaBanUI {
  public abstract class ControlBase {
    public ControlBase? ParentControl { get; set; }

    /// <summary>
    /// A disabled control doesn't accept user input, but its still rendered if visible <br>Returns false if control is not visible</br>
    /// </summary>
    bool _enabled = true;
    public bool Enabled {
      get => Visible ? _enabled : false;
      set { _enabled = value; }
    }
    public bool DrawDebugLines { get; set; } = false;
    
    /// If invisible, no event processing will take place and control will be disabled
    public bool Visible { get; set; } = true;
    #region Container Properties
    /// Backing field for Size
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

    /// <summary>
    /// Total content area, <b>not</b> including margins
    /// </summary>
    public Vector2 ContentArea { get => new Vector2(Size.X - Margin.X * 2, Size.Y - Margin.Y * 2); }
    public Vector2 Margin { get; set; } = Vector2.Zero;

    Vector2 _minimumSize = Vector2.Zero;
    /// <summary>
    /// Minimum element size (including margins)
    /// </summary>
    public Vector2 MinimumSize {
      get => _minimumSize;
      set {
        //if (value == _minimumSize) { return; }

        _minimumSize = new Vector2(value.X + Margin.X, value.Y + Margin.Y);
      }
    }

    public Vector2? MaximumSize { get; set; } = null;
    /// Absolute Position on Screen
    Vector2 _absolutePosition = Vector2.Zero;
    public Vector2 AbsolutePosition {
      get => _absolutePosition;
      set {
        //if (value == _absolutePosition) { return; }

        _absolutePosition = value + Margin;
        _abosoluteArea = new RectangleF(_absolutePosition, _size);
      }
    }

    public Vector2 RelativePosition { get; set; }

    RectangleF _abosoluteArea = RectangleF.Empty;
    public RectangleF AbsoluteArea { get => _abosoluteArea; }
    #endregion

    Vector2 _renderRelativePosition = Vector2.Zero;
    public Vector2 RenderRelativePosition { get => _renderRelativePosition; }
    public Vector2 RenderOffset { get; set; } = Vector2.Zero;
    public SamplerState SamplerState { get; set; } = SamplerState.PointWrap;

    Viewport oldViewport;

    protected virtual void ElementSizeChanged() { }

    /// <summary>
    /// Updates internal logic
    /// </summary>
    /// <param name="deltaTime"></param>
    public abstract void Update(double deltaTime);

    /// <summary>
    /// Called before draw element, updates elements positioning, animation steps etc
    /// </summary>
    /// <param name="deltaTime"></param>
    public virtual void UpdateUI(double deltaTime) { }

    /// <summary>
    /// Method called when the UIRoot decides this element should process pointer events
    /// </summary>
    /// <returns>True if the event should be blocked, False if the event should bubble up the tree</returns>
    public virtual bool InputUpdate(PointerEvent pointerEvent) { return false; }

    /// <summary>
    /// Method called when the UIRoot decides this element should process keyboard events
    /// </summary>
    /// <returns>True if the event should be blocked, False if the event should bubble up the tree</returns>
    public virtual bool InputUpdate(KeyboardEvent keyboardEvent) { return false; }

    /// <summary>
    /// Creates a virtual viewport, calls <seealso cref="DrawControl"/> and restores old viewport after finishing drawing
    /// </summary>
    public virtual void Draw(SpriteBatch spriteBatch, double deltaTime) {
      if (!Visible) { return; }

      BeginDraw(spriteBatch);
      UpdateUI(deltaTime);
      DrawControl(spriteBatch, deltaTime);

      if (DrawDebugLines) {
        if (Margin != Vector2.Zero) {
          spriteBatch.DrawRectangle(new RectangleF(Vector2.Zero, ContentArea), Color.Magenta);
        }

        spriteBatch.End();
        spriteBatch.GraphicsDevice.Viewport = new Viewport((int)(AbsolutePosition.X - Margin.X), (int)(AbsolutePosition.Y - Margin.Y), (int)Size.X, (int)Size.Y);
        spriteBatch.Begin();

        spriteBatch.DrawRectangle(new RectangleF(Vector2.Zero, Size), Color.Blue);
      }

      EndDraw(spriteBatch);
    }

    public void BeginDraw(SpriteBatch spriteBatch) {
      oldViewport = spriteBatch.GraphicsDevice.Viewport;

      spriteBatch.End();
      
      // Adds ParentControl's renderRelativePosition
      Vector2 renderOffset = RenderOffset;
      if (ParentControl != null) {
        renderOffset = RenderOffset + ParentControl._renderRelativePosition;
      }
      _renderRelativePosition = renderOffset;
      spriteBatch.GraphicsDevice.Viewport = new Viewport((int)AbsolutePosition.X, (int)AbsolutePosition.Y, (int)Size.X, (int)Size.Y);

      spriteBatch.Begin(samplerState: SamplerState, transformMatrix: Matrix.CreateTranslation(new Vector3(renderOffset.X, renderOffset.Y, 0)));
    }

    public void EndDraw(SpriteBatch spriteBatch) {
      spriteBatch.End();
      spriteBatch.GraphicsDevice.Viewport = oldViewport;
      spriteBatch.Begin(samplerState: SamplerState);
    }

    /// <summary>
    /// Make another control fill all of this control's space <br>also changes the target control's parent to this</br>
    /// </summary>
    internal void FillControl(ControlBase element, bool overrideMinimumSize = true) {
      element.Size = ContentArea;
      element.RelativePosition = RelativePosition;
      element.AbsolutePosition = AbsolutePosition;
      element.ParentControl = this;

      if (overrideMinimumSize) MinimumSize = element.MinimumSize;
    }

    /// <summary>
    /// Draws the control without transformation
    /// </summary>
    public abstract void DrawControl(SpriteBatch spriteBatch, double deltaTime);
  }
}