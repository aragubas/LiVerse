namespace LiVerse.CharacterRenderer {
  public class Character {
    public string Name { get; set; }
    public CharacterSpritesCollection characterSprites { get; set; }

    public Character(string name, CharacterSpritesCollection sprites) {
      Name = name;
      characterSprites = sprites;
    }

    public override string ToString() {
      return $"Character {Name}";
    }
  }
}
