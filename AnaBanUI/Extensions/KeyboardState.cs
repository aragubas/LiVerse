using Microsoft.Xna.Framework.Input;

namespace LiVerse.AnaBanUI.Extensions {
  public static class KeyboardStateExtensions {
    /// <summary>
    /// Check if key has been released based on current keyboard state and a new one
    /// </summary>
    /// <param name="newState">This KeyboardState.</param>
    /// <param name="key">Key to compare</param>
    /// <returns></returns>
    public static bool IsKeyUpExtended(this KeyboardState keyboardState, Keys key) {
      KeyboardState state = Keyboard.GetState();
      return state.IsKeyDown(key) && keyboardState.IsKeyUp(key);
    }
  }
}
