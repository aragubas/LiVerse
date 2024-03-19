using SFML.Graphics;
using SFML.System;

namespace LiVerse.AnaBanUI.Controls;

public class Button : Control {
  RectangleShape buttonBackground;
  Text text;

  public Button(string text) {
    buttonBackground = new(new Vector2f(24, 16));
    buttonBackground.FillColor = Color.Blue;

    this.text = new(text, ResourceManager.GetFont("NotoSans.ttf"), 18);

    this.text.FillColor = Color.Red;
  }

  public override void Update(double deltaTime) {
    buttonBackground.Size = ContentArea;
    text.Position = ContentArea / 2;

    FloatRect textLocalBounds = text.GetLocalBounds();
    text.Origin = new Vector2f(textLocalBounds.Width / 2, textLocalBounds.Height / 2);

    MinimumSize = new Vector2f(textLocalBounds.Width, textLocalBounds.Height);
  }

  protected override void DoDraw(RenderTarget target, RenderStates states) {
    target.Draw(buttonBackground, states);
    target.Draw(text, states);
  }
}