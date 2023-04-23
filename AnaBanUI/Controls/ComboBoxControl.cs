using LiVerse.AnaBanUI.Controls.ComboBox;
using LiVerse.AnaBanUI.Events;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Sprites;

namespace LiVerse.AnaBanUI.Controls {

  public class ComboBoxControl : ControlBase {
    public List<ComboBoxOption> Options { get; }
    public Button ToggleButton { get; }

    ComboBoxOption _selectedOption;
    public ComboBoxOption SelectedOption {
      get => _selectedOption;
      set {
        _selectedOption = value;
        ToggleButton.Label.Text = value.OptionText;
      }
    }
    UILayer? OptionsUILayer { get; set; }
    public event Action<ComboBoxOption>? SelectedOptionChanged;
    ComboBoxOverlayContainer? boxOverlayContainer;

    public ComboBoxControl(ComboBoxOption selectedOption, List<ComboBoxOption> options) {
      ToggleButton = new(selectedOption.OptionText);
      ToggleButton.Click += ToggleOptionsContainer;

      Options = options;
    }

    private void ToggleOptionsContainer() {
      if (OptionsUILayer != null && UIRoot.UILayers.Contains(OptionsUILayer)) {
        UIRoot.UILayers.Remove(OptionsUILayer);
        return;
      }

      OptionsUILayer = new();
      boxOverlayContainer = new(Options, (ComboBoxOption option) => {
        SelectedOption = option;
        SelectedOptionChanged?.Invoke(option);
        ToggleOptionsContainer();
      });
      boxOverlayContainer.ComboBoxRectangle = new(AbsolutePosition, ContentArea);

      OptionsUILayer.RootElement = boxOverlayContainer;
      OptionsUILayer.PointerInputUpdateEvent += OptionsUILayer_PointerInputUpdateEvent;

      UIRoot.UILayers.Add(OptionsUILayer);
    }

    private void OptionsUILayer_PointerInputUpdateEvent(PointerEvent obj) {
      if (boxOverlayContainer == null) { return; }
      if (obj.UpRect != RectangleF.Empty && !boxOverlayContainer.OptionSelected) {
        ToggleOptionsContainer();
      }
    }

    public override void UpdateUI(double deltaTime) {
      FillElement(ToggleButton);
      if (boxOverlayContainer != null) boxOverlayContainer.ComboBoxRectangle = new(AbsolutePosition, ContentArea);
    }

    public override void DrawElement(SpriteBatch spriteBatch, double deltaTime) {
      ToggleButton.Draw(spriteBatch, deltaTime);
    }

    public override bool InputUpdate(PointerEvent pointerEvent) {
      return ToggleButton.InputUpdate(pointerEvent);
    }

    public override void Update(double deltaTime) {
      ToggleButton.Update(deltaTime);
    }

  }
}
