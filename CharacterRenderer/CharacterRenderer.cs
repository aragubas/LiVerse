using LiVerse.AnaBanUI;
using LiVerse.CharacterRenderer.BuiltInAnimators;
using LiVerse.Stores;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace LiVerse.CharacterRenderer
{
    public class CharacterRenderer : ControlBase {
    public CharacterSpriteState CurrentSpriteState = CharacterSpriteState.Idle;
    public CharacterState State = new();
    public List<IAnimator> Animators { get; set; } = new();
    double blinkingPeriod = 0;
    Rectangle spriteDestinationRect = Rectangle.Empty;

    public CharacterRenderer() {
      Animators.Add(new MovingAroundAnimator());
      Animators.Add(new SpeakingJumpAnimator());
    }

    /// <summary>
    /// Controls if the character is "speaking"
    /// </summary>
    /// <param name="value">new value</param>
    public void SetSpeaking(bool value) => State.IsSpeaking = value;

    Texture2D? CurrentSprite {
      get {
        if (!State.IsSpeaking && !State.IsBlinking) {
          return CharacterStore.CurrentCharacter?.CurrentSpriteCollection?.Idle;
        }

        else if (!State.IsSpeaking && State.IsBlinking) {
          return CharacterStore.CurrentCharacter?.CurrentSpriteCollection?.IdleBlink;
        }

        else if (State.IsSpeaking && !State.IsBlinking) {
          return CharacterStore.CurrentCharacter?.CurrentSpriteCollection?.Speaking;
        }

        else {
          return CharacterStore.CurrentCharacter?.CurrentSpriteCollection?.SpeakingBlink;
        }
      }
    }

    public override void DrawElement(SpriteBatch spriteBatch, double deltaTime) {
      CalculatePosition(deltaTime);

      // Render current state
      if (CurrentSprite != null) {
        spriteBatch.Draw(CurrentSprite, spriteDestinationRect, Color.White);
      }
    }

    void CalculatePosition(double deltaTime) {
      if (CurrentSprite != null) {
        Point textureSize = new Point(CurrentSprite.Width, CurrentSprite.Height);
        Point texturePosition = Point.Zero;

        //textureSize.X = (int)Size.X;
        //textureSize.Y = (int)Size.Y;

        //if (textureSize.X > Size.X) {
        //  textureSize.X = (int)Size.X;
        //}

        texturePosition.X = (int)Size.X / 2 - textureSize.X / 2;
        texturePosition.Y = (int)Size.Y / 2 - textureSize.Y / 2;

        if (textureSize.Y > Size.Y) {
          textureSize.Y = (int)Size.Y;
          textureSize.X = (int)Size.Y;

          texturePosition.Y = 0;
          texturePosition.X = (int)Size.X / 2 - textureSize.X / 2;
        }

        Vector2 animatorsAccumulator = Vector2.Zero;
        foreach(IAnimator animator in Animators) {
          animatorsAccumulator += animator.Update(State, deltaTime);
        }

        //if (textureSize.Y > Size.Y) {
        //  textureSize.Y = (int)Size.Y / CurrentSprite.Height;
        //}


        spriteDestinationRect = new Rectangle(texturePosition + animatorsAccumulator.ToPoint(), textureSize);
      }
    }

    public override void Update(double deltaTime) {
      // Update Blinking
      if (CharacterStore.CurrentCharacter != null) {
        blinkingPeriod += 1 * deltaTime;

        if (blinkingPeriod > CharacterStore.CurrentCharacter.BlinkingTriggerEnd) {
          State.IsBlinking = false;
          blinkingPeriod = 0;

        }
        else if (blinkingPeriod > CharacterStore.CurrentCharacter.BlinkingTrigger) {
          State.IsBlinking = true;
        }

      } else { blinkingPeriod = 0; }

    }

  }
}
