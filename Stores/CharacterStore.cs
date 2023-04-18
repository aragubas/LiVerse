using LiVerse.CharacterRenderer;
  
namespace LiVerse.Stores {
  public static class CharacterStore {
    public static Character? CurrentCharacter { get; set; }

    static CharacterStore() {

    }

    public static void LoadCharacter(Character character, SpriteCollectionBuilder? spritesBuilder = null) {
      if (spritesBuilder != null) {
        SpriteCollection sprites = new SpriteCollection();
        sprites.Idle = ResourceManager.LoadTexture2DFromFile(spritesBuilder.Value.Idle);
        sprites.IdleBlink = ResourceManager.LoadTexture2DFromFile(spritesBuilder.Value.IdleBlink);
        sprites.Speaking = ResourceManager.LoadTexture2DFromFile(spritesBuilder.Value.Speaking);
        sprites.SpeakingBlink = ResourceManager.LoadTexture2DFromFile(spritesBuilder.Value.SpeakingBlink);

        character.CurrentSpriteCollection = sprites;
      }

      CurrentCharacter = character;
    }

  }
}
