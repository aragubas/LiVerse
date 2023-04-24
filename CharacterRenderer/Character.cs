namespace LiVerse.CharacterRenderer {
  public class Character {
    public string Name { get; set; }
    string _currentExpression = "default";
    public string CurrentExpression  { get => _currentExpression; }
    public SpriteCollection? CurrentSpriteCollection { 
      get {
        return LoadedSpriteCollections.TryGetValue(_currentExpression, out Expression expression) ? expression.SpriteCollection : null;
      }
    }

    public double BlinkingTrigger { get; set; } = 5;
    public double BlinkingTriggerEnd { get; set; } = 5.15;
    public Dictionary<string, Expression> LoadedSpriteCollections { get; } = new();

    public Character(string name, List<ExpressionBuilder> expressions) {
      Name = name;
      BuildCharacterExpressions(expressions);
    }

    public void BuildCharacterExpressions(List<ExpressionBuilder> expressions) {
      LoadedSpriteCollections.Clear();

      foreach(var expression in expressions) {
        string expressionName = expression.Name.Trim();
        Expression newExpression = new(expressionName, new SpriteCollection(expression.SpriteCollectionBuilder));

        LoadedSpriteCollections.Add(expression.Name, newExpression);
      }
    }

    public bool SetExpression(string expressionName) {
      if (LoadedSpriteCollections.ContainsKey(expressionName)) {
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
