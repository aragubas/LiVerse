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
    int selectionFirstIndex = 2;
    int selectionLastIndex = 6;
    int viewportXOffset = 0;
    bool selectionActive = true;
    bool shiftModifier = false;
    float keyRepeatCount = 0;
    Keys? keyRepeatKey = null;

    public LineEdit(string text = "") {
      background = new() {
        FillCenter = true,
        Color = Color.Blue
      };
      Text = text;
      textLabel = new(text) {
        HorizontalAlignment = LabelHorizontalAlignment.Left,
        FontName = "Inter"
      };

      UIRoot.OnTextInputEvent += OnTextInputEventUpdate;
    }


    public override void UpdateUI(double deltaTime) {
      FillElement(textLabel);
    }

    public override void DrawElement(SpriteBatch spriteBatch, double deltaTime) {
      background.Draw(spriteBatch, deltaTime, ContentArea, Vector2.Zero);

      // Render text selection
      if (Text.Length > 0 && selectionActive && textLabel.Font != null) {
        Vector2 cursorOffset = textLabel.Font.MeasureString(Text.Substring(0, selectionFirstIndex));
        Vector2 cursorEndOffset = textLabel.Font.MeasureString(Text.Substring(0, selectionLastIndex));
        //Vector2 characterUnderCursor = textLabel.Font.MeasureString(Text.Substring(cursorPosition == Text.Length ? Text.Length - 1 : cursorPosition, 1));

        spriteBatch.FillRectangle(new RectangleF(textLabel.TextPosition.X + cursorOffset.X, textLabel.TextPosition.Y, cursorEndOffset.X - cursorOffset.X, textLabel.FontSize), Color.Red);
      }


      textLabel.Draw(spriteBatch, deltaTime);

      // Only draw text cursor if text size is greater than 0
      if (Text.Length > 0 && textLabel.Font != null) {
        Vector2 cursorOffset = textLabel.Font.MeasureString(Text.Substring(0, cursorPosition));
        Vector2 characterUnderCursor = textLabel.Font.MeasureString(Text.Substring(cursorPosition == Text.Length ? Text.Length - 1 : cursorPosition, 1));
        spriteBatch.FillRectangle(new RectangleF(textLabel.TextPosition.X + cursorOffset.X, textLabel.TextPosition.Y, 1, characterUnderCursor.Y), Color.Red);
      } else {
        spriteBatch.FillRectangle(new RectangleF(textLabel.TextPosition.X, textLabel.TextPosition.Y, 1, textLabel.FontSize), Color.Red);
      }

    }

    public override void Update(double deltaTime) { }

    void OnTextInputEventUpdate(TextInputEventArgs args) {
      Console.WriteLine(args.Key);
      switch (args.Key) {
        case Keys.Back: {
          if (Text.Length < 1 || cursorPosition == 0) { return; }
          Text = Text.Remove(cursorPosition - 1, 1);
          textLabel.Text = Text;

          MoveCursorLeft();
          return;
        }
        
        case Keys.Delete: {
          if (Text.Length < 1 || cursorPosition >= Text.Length) { return; }
          Text = Text.Remove(cursorPosition, 1);
          textLabel.Text = Text;

          //MoveCursorRight();
          return;
        }

        case Keys.Home: {
          cursorPosition = 0;
          return;
        }

        case Keys.End: {
          cursorPosition = Text.Length;
          return;
        }

      }

      if (textLabel.Font != null && textLabel.Font.Characters.Contains(args.Character)) {
        // Place text in current cursor position, advance carret to the right
        int position = cursorPosition > Text.Length ? Text.Length : cursorPosition;
        Text = Text.Insert(position, args.Character.ToString());

        MoveCursorRight();

        textLabel.Text = Text;
      }
    }

    void MoveCursorLeft() {
      cursorPosition--;
      if (cursorPosition < 0) { cursorPosition = 0; }
    }

    void MoveCursorRight() {
      cursorPosition++;
      if (cursorPosition > Text.Length) { cursorPosition = Text.Length; }
    }

    void ResetKeyRepeat() {
      keyRepeatCount = 0;
      keyRepeatKey = null;
    }

    void MoveCursorBeginning() {
      cursorPosition = 0;
    }

    void MoveCursorEnd() {
      cursorPosition = Text.Length;
    }

    public override bool InputUpdate(KeyboardEvent keyboardEvent) {
      return false;
      if (keyboardEvent.PressedKeys.Length >= 1) {
        shiftModifier = false;
        foreach (var key in keyboardEvent.PressedKeys) {
          shiftModifier = (key == Keys.LeftShift || key == Keys.RightShift) && shiftModifier == false;
          
          if (key == Keys.LeftShift || key == Keys.RightShift) {
            continue;
          }

          if (keyRepeatKey == key) {
            keyRepeatCount++;
          } else {
            keyRepeatCount = 0;
            keyRepeatKey = key;
          }

        }

      } else {
        ResetKeyRepeat();
        shiftModifier = false;
      }

      Console.Clear();
      Console.WriteLine($"shift: {shiftModifier}");
      Console.WriteLine($"key: {keyRepeatKey}");


      if ((keyRepeatKey == Keys.Left && keyRepeatCount == 0) || keyRepeatKey == Keys.Left && (keyRepeatCount >= 25 && keyRepeatCount % 3 == 0)) {
        if (shiftModifier) {
          Console.WriteLine("Shift + Left");
        }
        
        MoveCursorLeft();
      }

      else if ((keyRepeatKey == Keys.Right && keyRepeatCount == 0) || keyRepeatKey == Keys.Right && (keyRepeatCount >= 25 && keyRepeatCount % 3 == 0)) {
        MoveCursorRight();
      }

      if (keyboardEvent.NewKeyboardState.IsKeyDown(Keys.Home) && keyboardEvent.OldKeyboardState.IsKeyUp(Keys.Home)) {
        MoveCursorBeginning();
      }

      if (keyboardEvent.NewKeyboardState.IsKeyDown(Keys.End) && keyboardEvent.OldKeyboardState.IsKeyUp(Keys.End)) {
        MoveCursorEnd();
      }

      return true;
    }
  }
}