using Microsoft.Xna.Framework.Graphics;

namespace LiVerse.CharacterRenderer {
  public class SpriteCollection {
    public Texture2D? Idle { get; set; } = null;
    public Texture2D? IdleBlink { get; set; } = null;
    public Texture2D? Speaking { get; set; } = null;
    public Texture2D? SpeakingBlink { get; set; } = null;

    public SpriteCollection(SpriteCollectionBuilder builder) {
      if (builder.Idle != null) Idle = ResourceManager.LoadTexture2DFromFile(builder.Idle);
      if (builder.IdleBlink != null) IdleBlink = ResourceManager.LoadTexture2DFromFile(builder.IdleBlink);
      if (builder.Speaking != null) Speaking = ResourceManager.LoadTexture2DFromFile(builder.Speaking);
      if (builder.SpeakingBlink != null) SpeakingBlink = ResourceManager.LoadTexture2DFromFile(builder.SpeakingBlink);
    }
  }
}
