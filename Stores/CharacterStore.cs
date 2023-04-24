using LiVerse.CharacterRenderer;

namespace LiVerse.Stores {
  public static class CharacterStore {
    static Character? _currentCharacter = null;
    public static Character? CurrentCharacter {
      get => _currentCharacter;
      set {
        if (_currentCharacter == value) return;
        
        _currentCharacter = value;
        OnCurrentCharacterChanged?.Invoke(value);
      }
    }
    public static event Action<Character?>? OnCurrentCharacterChanged;
    public static List<Character> Characters { get; set; } = new();

    static CharacterStore() {

    }

    public static void Load() {

    }

    public static void Save() {

    }
  }
}
