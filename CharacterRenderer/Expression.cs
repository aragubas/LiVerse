using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiVerse.CharacterRenderer; 
public class Expression {
  public string Name { get; }
  public SpriteCollection SpriteCollection { get; }

  public Expression(string name, SpriteCollection spriteCollection) { 
    Name = name;
    SpriteCollection = spriteCollection;
  }
}
