namespace LiVerse.CharacterRenderer {
  public class Character {
    public string Name { get; set; }
    string _currentExpression = "default";
    public string CurrentExpression  { get => _currentExpression; }
    public SpriteCollection? CurrentSpriteCollection { 
      get {
        if (LoadedSpriteCollection.TryGetValue(_currentExpression, out SpriteCollection collection)) {
          return collection;
        }

        return null;
      } 
    }

    public double BlinkingTrigger { get; set; } = 5;
    public double BlinkingTriggerEnd { get; set; } = 5.15;
    public Dictionary<string, SpriteCollection> LoadedSpriteCollection { get; } = new();

    public Character(string name, List<ExpressionBuilder> expressions) {
      Name = name;
      BuildCharacterExpressions(expressions);
    }

    public void BuildCharacterExpressions(List<ExpressionBuilder> expressions) {
      LoadedSpriteCollection.Clear();
      
      foreach(var expression in expressions) {
        SpriteCollection spriteCollection = new(expression.SpriteCollectionBuilder);

        LoadedSpriteCollection.Add(expression.Name, spriteCollection);
      }
    }

    public bool SetExpression(string expressionName) {
      if (LoadedSpriteCollection.ContainsKey(expressionName)) {
        _currentExpression = expressionName;
        return true;
      }

      return false;
    }

    public override string ToString() {
      return $"Character {Name}";
    }
  }
}
