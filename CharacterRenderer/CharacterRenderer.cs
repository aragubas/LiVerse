using LiVerse.AnaBanUI;
using LiVerse.CharacterRenderer.BuiltInAnimators;
using LiVerse.Stores;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace LiVerse.CharacterRenderer; 
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
      if (CharacterStore.CurrentCharacter == null) return null;

      if (!State.IsSpeaking && !State.IsBlinking) {
        Texture2D? sprite = CharacterStore.CurrentCharacter?.CurrentSpriteCollection?.Idle;
        if (sprite == null) {
          Texture2D? defaultSprite = CharacterStore.CurrentCharacter.LoadedSpriteCollections["default"].SpriteCollection.Idle;
          return defaultSprite == null ? null : defaultSprite;
        }
        return sprite;
      } else if (!State.IsSpeaking && State.IsBlinking) {
        Texture2D? sprite = CharacterStore.CurrentCharacter?.CurrentSpriteCollection?.IdleBlink;
        if (sprite == null) {
          Texture2D? defaultSprite = CharacterStore.CurrentCharacter.LoadedSpriteCollections["default"].SpriteCollection.IdleBlink;
          return defaultSprite == null ? null : defaultSprite;
        }
        return sprite;
      } else if (State.IsSpeaking && !State.IsBlinking) {
        Texture2D? sprite = CharacterStore.CurrentCharacter?.CurrentSpriteCollection?.Speaking;
        if (sprite == null) {
          Texture2D? defaultSprite = CharacterStore.CurrentCharacter.LoadedSpriteCollections["default"].SpriteCollection.Speaking;
          return defaultSprite == null ? null : defaultSprite;
        }
        return sprite;
      } else {
        Texture2D? sprite = CharacterStore.CurrentCharacter?.CurrentSpriteCollection?.SpeakingBlink;
        if (sprite == null) {
          Texture2D? defaultSprite = CharacterStore.CurrentCharacter.LoadedSpriteCollections["default"].SpriteCollection.SpeakingBlink;
          return defaultSprite == null ? null : defaultSprite;
        }
        return sprite;
      }
    }
  }

  public override void DrawControl(SpriteBatch spriteBatch, double deltaTime) {
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

      texturePosition.X = (int)ContentArea.X / 2 - textureSize.X / 2;
      texturePosition.Y = (int)ContentArea.Y / 2 - textureSize.Y / 2;

      if (textureSize.Y > Size.Y) {
        textureSize.Y = (int)ContentArea.Y;
        textureSize.X = (int)ContentArea.Y;

        texturePosition.Y = 0;
        texturePosition.X = (int)ContentArea.X / 2 - textureSize.X / 2;
      }

      Vector2 animatorsAccumulator = Vector2.Zero;
      foreach (IAnimator animator in Animators) {
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

      } else if (blinkingPeriod > CharacterStore.CurrentCharacter.BlinkingTrigger) {
        State.IsBlinking = true;
      }

    } else { blinkingPeriod = 0; }

  }

}
