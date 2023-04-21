using LiVerse.SerializeableModels;
using Newtonsoft.Json;
using Color = Microsoft.Xna.Framework.Color;

namespace LiVerse.Stores {
  public static class SettingsStore {
    private struct SerializeableSettingsStore {
      public SerializeableColor WindowTransparencyColor { get; set; }
    }
    private static string storeFilePath = Path.Combine(ResourceManager.DefaultStoresPath, "settings_store.json");

    public static Color WindowTransparencyColor { get; set; } = Color.Transparent;

    public static void LoadDefaultValues() {
      WindowTransparencyColor = Color.Transparent;
    }

    public static void Load() {
      if (!File.Exists(storeFilePath)) {
        // Create settings file with default values
        Save();
        return;
      }

      string jsonFile = File.ReadAllText(storeFilePath, System.Text.Encoding.UTF8);
      try {
        SerializeableSettingsStore serialized = JsonConvert.DeserializeObject<SerializeableSettingsStore>(jsonFile);

        WindowTransparencyColor = serialized.WindowTransparencyColor.ToColor();

      } catch (Exception exception) {
        if (exception is JsonSerializationException || exception is JsonReaderException) {
          // File is corrupted, create a new one with default values
          Console.WriteLine("[SettingsStore->Load] Serialization error while loading, replacing file with default values.");
        } else {
          Console.WriteLine("[SettingsStore->Load] Unknown error while loading, replacing file with default values.");
        }

        LoadDefaultValues();
        Save();
      }
    }

    public static void Save() {
      SerializeableSettingsStore serializeable = new();
      serializeable.WindowTransparencyColor = new(WindowTransparencyColor);

      string jsonText = JsonConvert.SerializeObject(serializeable);

      File.WriteAllText(storeFilePath, jsonText, System.Text.Encoding.UTF8);
    }
  }
}
