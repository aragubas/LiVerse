using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiVerse.AnaBanUI; 
/// <summary>
/// A Drawing Primitive
/// </summary>
public interface IDrawable {
  /// <summary>
  /// Draws in specified area and position
  /// </summary>
  /// <param name="spriteBatch">SpriteBatch</param>
  /// <param name="deltaTime">Delta Time</param>
  /// <param name="area">Area to draw (usually ContentArea)</param>
  /// <param name="position">Position</param>
  void Draw(SpriteBatch spriteBatch, double deltaTime, Vector2 area, Vector2 position);
  
  /// <summary>
  /// Draws in specified area at 0, 0
  /// </summary>
  /// <param name="spriteBatch">SpriteBatch</param>
  /// <param name="deltaTime">Delta Time</param>
  void Draw(SpriteBatch spriteBatch, double deltaTime, Vector2 area);
}
