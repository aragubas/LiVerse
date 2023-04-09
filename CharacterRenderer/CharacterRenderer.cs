using LiVerse.AnaBanUI;
using LiVerse.CharacterRenderer.BuiltInAnimators;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace LiVerse.CharacterRenderer
{
    public class CharacterRenderer : ControlBase {
    public Character? CurrentCharacter { get; set; }
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
        if (CurrentCharacter == null) { return null; }
        if (!State.IsSpeaking && !State.IsBlinking) {
          return CurrentCharacter.characterSprites.Idle;
        }

        else if (!State.IsSpeaking && State.IsBlinking) {
          return CurrentCharacter.characterSprites.IdleBlink;
        }

        else if (State.IsSpeaking && !State.IsBlinking) {
          return CurrentCharacter.characterSprites.Speaking;
        }

        else {
          return CurrentCharacter.characterSprites.SpeakingBlink;
        }
      }
    }

    public override void Draw(SpriteBatch spriteBatch, double deltaTime) {
      if (CurrentCharacter == null) {
        CharacterSpritesCollection spritesCollection = new();
        // Hardcoded path, will be removed on the future
        // TODO: Remove hardcoded path
        spritesCollection.Idle = ResourceManager.LoadTexture2DFromFile(spriteBatch.GraphicsDevice, @"C:\Users\Ceira\Downloads\Telegram Desktop\Aragubas PNGTuber\Aragubas Boca Fechada.png");
        spritesCollection.Speaking = ResourceManager.LoadTexture2DFromFile(spriteBatch.GraphicsDevice, @"C:\Users\Ceira\Downloads\Telegram Desktop\Aragubas PNGTuber\Aragubas Boca Aberta.png");
        spritesCollection.IdleBlink = ResourceManager.LoadTexture2DFromFile(spriteBatch.GraphicsDevice, @"C:\Users\Ceira\Downloads\Telegram Desktop\Aragubas PNGTuber\Aragubas Piscando Boca Fechada.png");
        spritesCollection.SpeakingBlink = ResourceManager.LoadTexture2DFromFile(spriteBatch.GraphicsDevice, @"C:\Users\Ceira\Downloads\Telegram Desktop\Aragubas PNGTuber\Aragubas Piscando Boca Aberta.png");

        CurrentCharacter = new Character("Aragubas", spritesCollection);
      }

      CalculatePosition(deltaTime);

      // Render current state
      spriteBatch.Draw(CurrentSprite, spriteDestinationRect, Color.White);
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
      if (CurrentCharacter != null) {
        blinkingPeriod += 1 * deltaTime;

        if (blinkingPeriod > CurrentCharacter.BlinkingTriggerEnd) {
          State.IsBlinking = false;
          blinkingPeriod = 0;

        }
        else if (blinkingPeriod > CurrentCharacter.BlinkingTrigger) {
          State.IsBlinking = true;
        }

      } else { blinkingPeriod = 0; }

    }

  }
}
