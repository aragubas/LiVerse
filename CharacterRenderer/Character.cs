namespace LiVerse.CharacterRenderer {
  public class Character {
    public string Name { get; set; }
    public double BlinkingTrigger { get; set; } = 5;
    public double BlinkingTriggerEnd { get; set; } = 5.15;
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
