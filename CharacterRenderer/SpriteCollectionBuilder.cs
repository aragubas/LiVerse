namespace LiVerse.CharacterRenderer; 
public struct SpriteCollectionBuilder {
  /// <summary>
  /// Idle Frame Path
  /// </summary>
  public string? Idle { get; set; }
  
  /// <summary>
  /// Idle Blinking Frame Path
  /// </summary>
  public string? IdleBlink { get; set; }
  
  /// <summary>
  /// Speaking Frame Path
  /// </summary>
  public string? Speaking { get; set; }
  
  /// <summary>
  /// Speaking Blinking Frame Path
  /// </summary>
  public string? SpeakingBlink { get; set; }
}
