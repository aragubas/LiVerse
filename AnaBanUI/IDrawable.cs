using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiVerse.AnaBanUI {
  /// <summary>
  /// A Drawing Primitive
  /// </summary>
  public interface IDrawable {
    void Draw(SpriteBatch spriteBatch, double deltaTime, Vector2 area, Vector2 position);
  }
}
