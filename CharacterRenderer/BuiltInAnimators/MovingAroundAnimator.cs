using Microsoft.Xna.Framework;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiVerse.CharacterRenderer.BuiltInAnimators {
  public class MovingAroundAnimator : IAnimator {
    public bool AnimationEnded { get; set; }
    double ceira = 0;
    Vector2 nextPos = Vector2.Zero;
    Vector2 returnPos = Vector2.Zero;
    Vector2 returnPosTarget = Vector2.Zero;

    public Vector2 Update(CharacterState state, double deltaTime) {
      ceira += 1 * deltaTime;

      if (ceira > 1.5) {
        Random.Shared.NextUnitVector(out Vector2 nextPosition);
        
        returnPosTarget = Vector2.Zero;
        returnPosTarget += nextPosition * 8;

        ceira = 0;
      }

      returnPos = Vector2.LerpPrecise(returnPos, returnPosTarget, (float)(1 - Math.Pow(0.1, deltaTime)));

      return returnPos;
    }
  }
}
