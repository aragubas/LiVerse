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
    int selectionFirstIndex = 0;
    int selectionLastIndex = 0;
    int viewportXOffset = 0;
    bool selectionActive = false;
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
        selectionFirstIndex = Math.Clamp(selectionFirstIndex, 0, Text.Length);
        selectionLastIndex = Math.Clamp(selectionLastIndex, 0, Text.Length);

        Vector2 cursorOffset = textLabel.Font.MeasureString(Text.Substring(0, selectionFirstIndex));
        Vector2 cursorEndOffset = textLabel.Font.MeasureString(Text.Substring(0, selectionLastIndex));
        //Vector2 characterUnderCursor = textLabel.Font.MeasureString(Text.Substring(cursorPosition == Text.Length ? Text.Length - 1 : cursorPosition, 1));

        spriteBatch.FillRectangle(new RectangleF(textLabel.TextPosition.X + cursorOffset.X, textLabel.TextPosition.Y, cursorEndOffset.X - cursorOffset.X, textLabel.FontSize), Color.Black);
      }

      // Draw Text
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
      switch (args.Key) {
        case Keys.Back: {
          if (selectionActive && Text.Length >= 1) {
            DeleteSelection();
            return;
          }

          if (Text.Length < 1 || cursorPosition == 0) { return; }          

          Text = Text.Remove(cursorPosition - 1, 1);
          textLabel.Text = Text;

          MoveCursorLeft();
          return;
        }
        
        case Keys.Delete: {
          if (Text.Length < 1 || cursorPosition >= Text.Length) { return; }

          if (selectionActive) {
            DeleteSelection();
            return;
          }

          Text = Text.Remove(cursorPosition, 1);
          textLabel.Text = Text;
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

      // Inserts character
      if (textLabel.Font != null && textLabel.Font.Characters.Contains(args.Character)) {
        // If selection active, replace selection with character
        if (selectionActive) { DeleteSelection(); }
        
        // Place character in current cursor position, advance carret to the right
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

    void ResetSelection() {
      selectionActive = false;
      selectionFirstIndex = 0;
      selectionLastIndex = 0;
    }

    void DeleteSelection() {
      if (!selectionActive) { return; }
      bool deletingLeftFromRight = cursorPosition > selectionFirstIndex;
      int count = Math.Abs(selectionFirstIndex - selectionLastIndex);

      Text = Text.Remove(selectionFirstIndex, count);
      ResetSelection();

      // Move pointer if not deleting from left to right
      if (deletingLeftFromRight) {
        cursorPosition -= count;
      }

      cursorPosition = Math.Clamp(cursorPosition, 0, Text.Length);

      textLabel.Text = Text;
    }

    void SelectLeft() {
      if (!selectionActive) {
        selectionActive = true;
        selectionFirstIndex = cursorPosition;
        selectionLastIndex = cursorPosition;
      }

      if (selectionFirstIndex < cursorPosition) {
        selectionLastIndex = cursorPosition - 1;

      } else {
        selectionFirstIndex = cursorPosition - 1;
      }

      selectionFirstIndex = Math.Clamp(selectionFirstIndex, 0, Text.Length);
      selectionLastIndex = Math.Clamp(selectionLastIndex, 0, Text.Length);
    }

    void SelectRight() {
      if (!selectionActive) {
        selectionActive = true;
        selectionFirstIndex = cursorPosition;
        selectionLastIndex = cursorPosition;
      }

      // Reverse Selection
      if (selectionLastIndex > cursorPosition) {
        selectionFirstIndex = cursorPosition + 1;
      } else {
        selectionLastIndex = cursorPosition + 1;
      }

      selectionFirstIndex = Math.Clamp(selectionFirstIndex, 0, Text.Length);
      selectionLastIndex = Math.Clamp(selectionLastIndex, 0, Text.Length);
    }
    
    void SelectWordOnRight() {
      int count = 0;
      bool firstSpaceSkipped = false;
      for (int i = cursorPosition; i < Text.Length; i++) {
        char currentChar = Text[i];

        if (currentChar != ' ') {
          count++;
        } else {
          if (i + 1 > 0 && Text[i + 1] != ' ' && !firstSpaceSkipped && count == 0) {
            firstSpaceSkipped = true;
            count++;
            continue;
          }
          break;
        }
      }

      if (!selectionActive) {
        selectionActive = true;
        selectionFirstIndex = cursorPosition;
        selectionLastIndex = cursorPosition + count;

      } else {
        selectionLastIndex = cursorPosition + count;
      }

      cursorPosition = selectionLastIndex;
    }

    void SelectWordOnLeft() {
      int count = 0;
      bool firstSpaceSkipped = false;
      for(int i = cursorPosition - 1; i > -1; i--) {
        char currentChar = Text[i];

        if (currentChar != ' ') {
          count++;
        } else {
          if (i - 1 > 0 && Text[i - 1] != ' ' && !firstSpaceSkipped && count == 0) {
            firstSpaceSkipped = true;
            count++;
            continue;
          }          
          break;
        }
      }

      if (!selectionActive) {
        selectionActive = true;
        selectionFirstIndex = cursorPosition - count;
        selectionLastIndex = cursorPosition;

      } else {
        selectionFirstIndex = cursorPosition - count;        
      }

      cursorPosition = selectionFirstIndex;
    }


    public override bool InputUpdate(KeyboardEvent keyboardEvent) {
      if (keyboardEvent.PressedKeys.Length >= 1) {
        shiftModifier = false;
        foreach (var key in keyboardEvent.PressedKeys) {
          // Only set if left shift or right shift keys are pressed and if
          // shift modifier was null; Same for CTRL key
          shiftModifier = (key == Keys.LeftShift || key == Keys.RightShift) && shiftModifier == false;

          // Ignore left and right shift and ctrl to not increase or change keyRepeat key
          if (key == Keys.LeftShift || key == Keys.RightShift 
              || key == Keys.LeftControl || key == Keys.RightControl) {
            continue;
          }

          // Increases keyRepeatCount if current key is the same as repeatKey
          // Otherwise, a new key has been pressed
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

      // Skips every 3 event frame to simulate key repeat
      bool keyRepeatIntervalMet = (keyRepeatCount >= 25 && keyRepeatCount % 3 == 0);

      // Left Arrow Key
      if (keyboardEvent.PressedKeys.Contains(Keys.Left)) {
        if ((keyRepeatKey == Keys.Left && keyRepeatCount == 0) || keyRepeatKey == Keys.Left && keyRepeatIntervalMet) {
          // Check if Shift and Control are pressed at the same time, for both right and left
          if ((keyboardEvent.NewKeyboardState.IsKeyDown(Keys.LeftShift) || keyboardEvent.NewKeyboardState.IsKeyDown(Keys.RightShift)) &&
              (keyboardEvent.NewKeyboardState.IsKeyDown(Keys.LeftControl) || keyboardEvent.NewKeyboardState.IsKeyDown(Keys.RightControl))) {
            SelectWordOnLeft();
            return true;
          }

          if (shiftModifier) { SelectLeft(); } else { 
            // Skips key frame if canceling selection
            if (selectionActive) {
              ResetSelection();
              return true;
            }
            ResetSelection();
          }

          MoveCursorLeft();
        }
      } else if (keyRepeatKey == Keys.Left) {
        ResetKeyRepeat();
      }

      // Right Arrow Key
      if (keyboardEvent.PressedKeys.Contains(Keys.Right)) {
        if ((keyRepeatKey == Keys.Right && keyRepeatCount == 0) || keyRepeatKey == Keys.Right && keyRepeatIntervalMet) {
          // Check if Shift and Control are pressed at the same time, for both right and left
          if ((keyboardEvent.NewKeyboardState.IsKeyDown(Keys.LeftShift) || keyboardEvent.NewKeyboardState.IsKeyDown(Keys.RightShift)) &&
              (keyboardEvent.NewKeyboardState.IsKeyDown(Keys.LeftControl) || keyboardEvent.NewKeyboardState.IsKeyDown(Keys.RightControl))) {
            SelectWordOnRight();
            return true;
          }

          if (shiftModifier) { SelectRight(); } else {
            // Skips key if canceling selection
            if (selectionActive) {
              ResetSelection();
              return true;
            }

            ResetSelection();
          }
          
          MoveCursorRight();          
        }
      } else if (keyRepeatKey == Keys.Right) {
        ResetKeyRepeat();
      }

      // Home Key
      if (keyboardEvent.NewKeyboardState.IsKeyDown(Keys.Home) && keyboardEvent.OldKeyboardState.IsKeyUp(Keys.Home)) {
        ResetKeyRepeat();

        if (shiftModifier) {
          selectionActive = true;

          // If the cursor is at the left end of the text
          // don't move the first and last indexes
          // Avoids canceling the selection if the user presses
          // Shift + Home again
          if (cursorPosition != 0) {
            selectionFirstIndex = 0;
            selectionLastIndex = cursorPosition;
          }

        } else {
          if (selectionActive) {
            ResetSelection();
          }
        }

        MoveCursorBeginning();
      }

      // End Key
      if (keyboardEvent.NewKeyboardState.IsKeyDown(Keys.End) && keyboardEvent.OldKeyboardState.IsKeyUp(Keys.End)) {
        ResetKeyRepeat();

        if (shiftModifier) {
          selectionActive = true;
          
          // If the cursor is at the right end of the text
          // don't move the first and last indexes
          // Avoids canceling the selection if the user presses
          // Shift + End again
          if (cursorPosition != Text.Length) {
            selectionFirstIndex = cursorPosition;
            selectionLastIndex = Text.Length;
          }

        } else {
          if (selectionActive) {
            ResetSelection();
          }
        }

        MoveCursorEnd();
      }

      return true;
    }
  }
}