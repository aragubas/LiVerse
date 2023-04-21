using Microsoft.Xna.Framework.Graphics;

namespace LiVerse.CharacterRenderer {
  public struct SpriteCollection {
    public Texture2D Idle { get; set; }
    public Texture2D IdleBlink { get; set; }
    public Texture2D Speaking { get; set; }
    public Texture2D SpeakingBlink { get; set; }

    public SpriteCollection(SpriteCollectionBuilder builder) {
      Idle = ResourceManager.LoadTexture2DFromFile(builder.Idle);
      IdleBlink = ResourceManager.LoadTexture2DFromFile(builder.IdleBlink);
      Speaking = ResourceManager.LoadTexture2DFromFile(builder.Speaking);
      SpeakingBlink = ResourceManager.LoadTexture2DFromFile(builder.SpeakingBlink);
    }
  }
}
