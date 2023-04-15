using LiVerse.AnaBanUI.Containers;
using LiVerse.AnaBanUI.Events;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Sprites;

namespace LiVerse.AnaBanUI.Controls {
  public struct ComboBoxOption {
    public string OptionText { get; set; }
    public object? ExtraData { get; set; }

    public ComboBoxOption(string optionText, object? extraData = null) {
      OptionText = optionText;
      ExtraData = extraData;
    }
  }
  
  public class ComboBoxOverlayContainer : ControlBase {
    public List<ComboBoxOption> Options { get; }
    public ScrollableList ScrollableList { get; }
    public RectangleF ComboBoxPosition { get; set; }
    public event Action<ComboBoxOption>? Callback;
    bool blockInput = false;

    public ComboBoxOverlayContainer(List<ComboBoxOption> options, Action<ComboBoxOption> callback) {
      Options = options;
      ScrollableList = new() { ParentControl = this };
      Callback += callback;

      foreach(var option in Options) {
        Button optionButton = new(option.OptionText) { ButtonStyle = ButtonStyle.Flat, BlinkWhenPressed = true };
        optionButton.Label.TextHorizontalAlignment = LabelTextHorizontalAlignment.Left;
        optionButton.Click += () => {
          Callback?.Invoke(option);
        };

        optionButton.BlinkingStarted += () => {
          blockInput = true;
        };

        ScrollableList.Elements.Add(optionButton);
      }
    }

    public override void DrawElement(SpriteBatch spriteBatch, double deltaTime) {
      ScrollableList.Draw(spriteBatch, deltaTime);
    }

    public override bool InputUpdate(PointerEvent pointerEvent) {
      return !blockInput ? ScrollableList.InputUpdate(pointerEvent) : true;
    }

    public override void Update(double deltaTime) {
      ScrollableList.Size = new Vector2(ComboBoxPosition.Size.Width, ScrollableList.MinimumSize.Y);
      ScrollableList.AbsolutePosition = ComboBoxPosition.Position;

      ScrollableList.Update(deltaTime);
    }
  }

  public class ComboBox : ControlBase {
    public List<ComboBoxOption> Options { get; }
    public Button ToggleButton { get; }
    
    ComboBoxOption _selectedOption;
    public ComboBoxOption SelectedOption { get => _selectedOption; 
      set {
        _selectedOption = value;
        ToggleButton.Label.Text = value.OptionText;
      }
    }
    UILayer? optionsUILayer { get; set; }
    public event Action<ComboBoxOption>? SelectedOptionChanged;
    bool IsOpened = false;


    public ComboBox(ComboBoxOption selectedOption, List<ComboBoxOption> options) {
      ToggleButton = new(selectedOption.OptionText);
      ToggleButton.Click += ToggleOptionsContainer;

      Options = options;
    }

    private void ToggleOptionsContainer() {
      if (optionsUILayer != null && UIRoot.UILayers.Contains(optionsUILayer)) {
        UIRoot.UILayers.Remove(optionsUILayer);
        IsOpened = false;
        return;
      }

      optionsUILayer = new();
      ComboBoxOverlayContainer boxOverlayContainer = new(Options, (ComboBoxOption option) => {
        SelectedOption = option;
        SelectedOptionChanged?.Invoke(option);
        ToggleOptionsContainer();
      });
      boxOverlayContainer.ComboBoxPosition = new RectangleF(AbsolutePosition, ContentArea);

      optionsUILayer.RootElement = boxOverlayContainer;
      UIRoot.UILayers.Add(optionsUILayer);
      IsOpened = true;
    }

    public override void DrawElement(SpriteBatch spriteBatch, double deltaTime) {
      ToggleButton.Draw(spriteBatch, deltaTime);
    }

    public override bool InputUpdate(PointerEvent pointerEvent) {      
      return ToggleButton.InputUpdate(pointerEvent);
    }

    public override void Update(double deltaTime) {
      ToggleButton.Size = ContentArea;
      ToggleButton.RelativePosition = RelativePosition;
      ToggleButton.AbsolutePosition = AbsolutePosition;

      MinimumSize = ToggleButton.MinimumSize;

      ToggleButton.Update(deltaTime);
    }

  }
}
