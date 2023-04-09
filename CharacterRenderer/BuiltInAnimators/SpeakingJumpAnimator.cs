using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiVerse.CharacterRenderer.BuiltInAnimators {
  public class SpeakingJumpAnimator : IAnimator {
    public bool AnimationEnded { get; set; }
    Vector2 returnVector { get; set; }
    Vector2 returnVectorTarget { get; set; }
    bool oldState = false;
    KeyboardState oldKeyboardState;
    bool speakingReset = false;
    double speakingResetTimer = 0;

    void doSpeaking() {
      returnVectorTarget = Vector2.Zero;
      returnVectorTarget += Vector2.UnitY * -30;

      speakingReset = true;
      Console.WriteLine(returnVectorTarget);
    }

    public Vector2 Update(CharacterState state, double deltaTime) {
      if (Keyboard.GetState().IsKeyDown(Keys.E) && oldKeyboardState.IsKeyUp(Keys.E)) {
        doSpeaking();
      }

      oldKeyboardState = Keyboard.GetState();

      if (state.IsSpeaking != oldState) {
        oldState = state.IsSpeaking;

        if (oldState) {
          doSpeaking();
          Console.WriteLine($"State changed: {oldState}");
        }
      }

      if (speakingReset) {
        speakingResetTimer += 1 * deltaTime;

        if (speakingResetTimer > 0.15) {
          speakingReset = false;
          speakingResetTimer = 0;

          Console.WriteLine("Speak Reset");

          returnVectorTarget = Vector2.Zero;
          
        }

      }

      returnVector = Vector2.LerpPrecise(returnVector, returnVectorTarget, (float)(1 - Math.Pow(0.00025, deltaTime)));
      return returnVector;
    }
  }
}
