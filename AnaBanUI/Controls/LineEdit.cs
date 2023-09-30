using LiVerse.AnaBanUI;
using LiVerse.AnaBanUI.Controls;
using LiVerse.AnaBanUI.Drawables;
using LiVerse.AnaBanUI.Events;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;

namespace LiVerse.AnabanUI.Controls {
  public class LineEdit : ControlBase {
    RectangleDrawable background;
    public string Text;
    Label textLabel;
    int cursorPosition = 0;
    float keyRepeat = 0;

    public LineEdit(string text = "") {
      background = new() {
        FillCenter = true,
        Color = Color.Blue
      };
      Text = text;
      textLabel = new(text);
      UIRoot.OnTextInputEvent += OnTextInputEventUpdate;
    }


    public override void UpdateUI(double deltaTime) {
      FillElement(textLabel);
    }

    public override void DrawElement(SpriteBatch spriteBatch, double deltaTime) {
      background.Draw(spriteBatch, deltaTime, ContentArea, Vector2.Zero);
      textLabel.Draw(spriteBatch, deltaTime);

      // Only draw text cursor is text size is greater than 0
      if (Text.Length > 0) {
        Vector2 cursorOffset = textLabel.Font.MeasureString(Text.Substring(0, cursorPosition));
        Vector2 characterUnderCursor = textLabel.Font.MeasureString(Text.Substring(cursorPosition == Text.Length ? Text.Length - 1 : cursorPosition, 1));
        spriteBatch.FillRectangle(new RectangleF(textLabel.TextPosition.X + cursorOffset.X + characterUnderCursor.X, textLabel.TextPosition.Y, 1, characterUnderCursor.Y), Color.Red);
      }
    }

    public override void Update(double deltaTime) {
    }

    void OnTextInputEventUpdate(TextInputEventArgs args) {
      if (textLabel.Font.Characters.Contains(args.Character)) {
        int position = cursorPosition + 1 > Text.Length ? 0 : cursorPosition + 1;
        Text = Text.Insert(position, args.Character.ToString());
        if (Text.Length > 1) { cursorPosition++; }
        textLabel.Text = Text;
      }
    }

    void MoveCarretLeft() {
      cursorPosition--;
      if (cursorPosition < 0) { cursorPosition = 0; }
    }

    void MoveCarretRight() {
      cursorPosition++;
      if (cursorPosition > Text.Length - 1) { cursorPosition = Text.Length - 1; }
    }

    public override bool InputUpdate(KeyboardEvent keyboardEvent) {


      if (keyboardEvent.NewKeyboardState.IsKeyDown(Keys.Left) && keyboardEvent.OldKeyboardState.IsKeyUp(Keys.Left)) {
        MoveCarretLeft();

      } else if (keyboardEvent.NewKeyboardState.IsKeyDown(Keys.Right) && keyboardEvent.OldKeyboardState.IsKeyUp(Keys.Right)) {
        MoveCarretRight();

      } else if (keyboardEvent.NewKeyboardState.IsKeyDown(Keys.Back) && keyboardEvent.OldKeyboardState.IsKeyUp(Keys.Back) && Text.Length > 0) {
        Text = Text.Remove(cursorPosition, 1);
        textLabel.Text = Text;

        cursorPosition--;
        if (cursorPosition < 0) { cursorPosition = 0; }

      } else if (keyboardEvent.NewKeyboardState.IsKeyDown(Keys.Delete) && keyboardEvent.OldKeyboardState.IsKeyUp(Keys.Delete) && Text.Length > 1) {
        Text = Text.Remove(cursorPosition + 1, 1);
        textLabel.Text = Text;

        cursorPosition--;
        if (cursorPosition < 0) { cursorPosition = 0; }

      }

      return true;
    }
  }
}