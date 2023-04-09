using Microsoft.Xna.Framework.Graphics;

namespace LiVerse.CharacterRenderer {
  public struct CharacterSpritesCollection {
    public Texture2D Idle { get; set; }
    public Texture2D IdleBlink { get; set; }
    public Texture2D Speaking { get; set; }
    public Texture2D SpeakingBlink { get; set; }
  }
}
