namespace LiVerse.CharacterRenderer {
  public class Character {
    public string Name { get; set; }
    public double BlinkingTrigger { get; set; } = 5;
    public double BlinkingTriggerEnd { get; set; } = 5.15;
    public SpriteCollection CurrentSpriteCollection { get; set; }

    public Character(string name, SpriteCollection sprites) {
      Name = name;
      CurrentSpriteCollection = sprites;
    }

    public override string ToString() {
      return $"Character {Name}";
    }
  }
}
