using Microsoft.Xna.Framework;
using MonoGame.Extended;
using System.Diagnostics.CodeAnalysis;

namespace LiVerse.AnaBanUI.Events {
  public struct PointerEvent {
    public RectangleF PositionRect { get; set; }
    public RectangleF DownRect { get; set; }
    public RectangleF UpRect { get; set; }
    public bool Down { get; set; }

    public override bool Equals([NotNullWhen(true)] object? obj) {
      if (obj?.GetType() != typeof(PointerEvent)) { return false; }
      if (obj == null) return false;
      PointerEvent mouseEvent = (PointerEvent)obj;

      return mouseEvent.DownRect == DownRect && mouseEvent.PositionRect == PositionRect && mouseEvent.UpRect == UpRect && mouseEvent.Down == Down;      
    }

    public static bool operator ==(PointerEvent left, PointerEvent right) {
      return left.Equals(right);
    }

    public static bool operator !=(PointerEvent left, PointerEvent right) {
      return !(left == right);
    }

    public override int GetHashCode() {
      return PositionRect.GetHashCode() + DownRect.GetHashCode() + UpRect.GetHashCode() + Down.GetHashCode();
    }
  }
}
