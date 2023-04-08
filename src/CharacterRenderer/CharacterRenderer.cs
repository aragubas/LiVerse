using LiVerse.AnaBanUI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace LiVerse.CharacterRenderer {
  public class CharacterRenderer : ControlBase {
    public Character? CurrentCharacter { get; set; }
    public CharacterSpriteState CurrentSpriteState = CharacterSpriteState.Idle;
    public bool IsSpeaking = false;
    public bool IsBlinking = false;
    Rectangle spriteDestinationRect = Rectangle.Empty;

    public CharacterRenderer() {
      MicrophoneLevelMeter.CharacterStartSpeaking += MicrophoneLevelMeter_CharacterStartSpeaking;
      MicrophoneLevelMeter.CharacterStopSpeaking += MicrophoneLevelMeter_CharacterStopSpeaking;
    }

    private void MicrophoneLevelMeter_CharacterStopSpeaking() {
      IsSpeaking = false;
    }

    private void MicrophoneLevelMeter_CharacterStartSpeaking() {
      IsSpeaking = true;
    }

    Texture2D? CurrentSprite {
      get {
        if (CurrentCharacter == null) { return null; }
        if (!IsSpeaking && !IsBlinking) {
          return CurrentCharacter.characterSprites.Idle;
        }

        else if (!IsSpeaking && IsBlinking) {
          return CurrentCharacter.characterSprites.IdleBlink;
        }

        else if (IsSpeaking && !IsBlinking) {
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

      CalculatePosition();

      // Render current state
      //spriteBatch.Draw(GetStateSprite(), Rectangle.Empty, Color.White);
      spriteBatch.Draw(CurrentSprite, spriteDestinationRect, Color.White);
    }

    void CalculatePosition() {
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



        //if (textureSize.Y > Size.Y) {
        //  textureSize.Y = (int)Size.Y / CurrentSprite.Height;
        //}


        spriteDestinationRect = new Rectangle(texturePosition, textureSize);
      }
    }

    public override void Update(double deltaTime) {

    }

  }
}
