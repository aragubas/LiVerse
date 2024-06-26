﻿using LiVerse.AnaBanUI.Containers;
using LiVerse.AnaBanUI.Drawables;
using LiVerse.AnaBanUI.Events;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace LiVerse.AnaBanUI.Controls.ComboBox; 
public class ComboBoxOverlayContainer : ControlBase {
  public List<ComboBoxOption> Options { get; }
  public ScrollableList ScrollableList { get; }
  public RectangleF ComboBoxRectangle { get; set; }
  public event Action<ComboBoxOption>? Callback;
  public bool OptionSelected = false;

  public ComboBoxOverlayContainer(ControlBase? parent, List<ComboBoxOption> options, Action<ComboBoxOption> callback) : base(parent) {
    Options = options;
    ScrollableList = new(this) { BackgroundRectDrawable = new() { Color = Color.Magenta } };
    Callback += callback;

    foreach (var option in Options) {
      Button optionButton = new(this, option.OptionText) { 
        ButtonStyle = ButtonStyle.Flat, 
        BlinkWhenPressed = true 
      };
      
      optionButton.Click += () => {
        Callback?.Invoke(option);
      };

      optionButton.BlinkingStarted += () => {
        OptionSelected = true;
      };

      ScrollableList.Elements.Add(optionButton);
    }
  }

  public override void UpdateUI(double deltaTime) {
    ScrollableList.Size = new Vector2(ComboBoxRectangle.Size.Width, ScrollableList.MinimumSize.Y);
    ScrollableList.AbsolutePosition = ComboBoxRectangle.Position;
  }

  public override void DrawControl(SpriteBatch spriteBatch, double deltaTime) {
    ScrollableList.Draw(spriteBatch, deltaTime);
  }

  public override bool InputUpdate(PointerEvent pointerEvent) {
    return !OptionSelected ? ScrollableList.InputUpdate(pointerEvent) : true;
  }

  public override void Update(double deltaTime) {
    ScrollableList.Update(deltaTime);
  }
}
