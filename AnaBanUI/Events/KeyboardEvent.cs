using Microsoft.Xna.Framework.Input;
using System.Diagnostics.CodeAnalysis;

namespace LiVerse.AnaBanUI.Events {
  public struct KeyboardEvent {
    public KeyboardState NewKeyboardState { get; set; }
    public KeyboardState OldKeyboardState { get; set; }
    public Keys[] PressedKeys { get; set; }
  }
}
