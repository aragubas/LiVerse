using LiVerse.CharacterRenderer;
  
namespace LiVerse.Stores {
  public static class CharacterStore {
    public static Character? CurrentCharacter { get; set; }
    public static List<Character> Characters { get; set; } = new();

    static CharacterStore() {

    }

    public static void Load() {

    }

    public static void Save() {

    }
  }
}
