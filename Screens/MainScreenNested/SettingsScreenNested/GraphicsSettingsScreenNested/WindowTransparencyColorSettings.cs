using LiVerse.AnaBanUI;
using LiVerse.AnaBanUI.Containers;
using LiVerse.AnaBanUI.Controls;
using LiVerse.AnaBanUI.Controls.ComboBox;
using LiVerse.AnaBanUI.Events;
using LiVerse.Stores;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiVerse.Screens.MainScreenNested.SettingsScreenNested.GraphicsSettingsScreenNested; 
public class WindowTransparencyColorSettings : ControlBase {
  ScrollableList optionsList { get; }
  Label transparentBackgroudWarning { get; }
  SolidColorRectangle colorPreview;
  DockFillContainer dockFillCustomColor;
  Slider rSlider;
  Slider gSlider;
  Slider bSlider;

  public WindowTransparencyColorSettings() {
    optionsList = new() { 
      ParentControl = this, 
      Gap = 8 
    };

    DockFillContainer DockFillTransparencyType = new() { 
      ParentControl = this, 
      DockDirection = DockDirection.Left, 
      Gap = 8
    };
    Label backgroundTransparencyTypeTitleLabel = new("Background Transparency Type:");
    List<ComboBoxOption> options = new();
    ComboBoxOption defaultOption = new();
    
    if (SettingsStore.WindowTransparencyColor == Color.Transparent) {
      defaultOption.OptionText = "Transparent";
      defaultOption.ExtraData = 0;

    } else if (SettingsStore.WindowTransparencyColor == new Color(0, 255, 0, 255)) {
      defaultOption.OptionText = "Green";
      defaultOption.ExtraData = 1;

    } else if (SettingsStore.WindowTransparencyColor == Color.Blue) {
      defaultOption.OptionText = "Blue";
      defaultOption.ExtraData = 2;

    } else if (SettingsStore.WindowTransparencyColor == Color.Magenta) {
      defaultOption.OptionText = "Magenta";
      defaultOption.ExtraData = 3;

    } else {
      defaultOption.OptionText = "Custom Color";
      defaultOption.ExtraData = -1;
    }

    options.Add(new ComboBoxOption("Transparent", 0));
    options.Add(new ComboBoxOption("Green", 1));
    options.Add(new ComboBoxOption("Blue", 2));
    options.Add(new ComboBoxOption("Magenta", 3));
    options.Add(new ComboBoxOption("Custom Color", -1));

    ComboBoxControl backgroundTransparencyOptionsComboBox = new(defaultOption, options);
    backgroundTransparencyOptionsComboBox.SelectedOptionChanged += BackgroundTransparencyOptionsComboBox_SelectedOptionChanged; ;

    DockFillTransparencyType.DockElement = backgroundTransparencyTypeTitleLabel;
    DockFillTransparencyType.FillElement = backgroundTransparencyOptionsComboBox;

    optionsList.Elements.Add(DockFillTransparencyType);

    dockFillCustomColor = new() { 
      Gap = 8f, 
      DockDirection = DockDirection.Right 
    };
    ScrollableList rgbSlidersList = new() { Gap = 8f };

    DockFillContainer rSliderDockFill = new() { DockDirection = DockDirection.Left, Gap = 8 };
    rSlider = new() { RaiseOnValueChangedEveryGrabFrame = true, MaximumValue = 255 };
    rSlider.OnValueChanged += new Action<float>((value) => { RGBSlidersChanged(); });
    rSliderDockFill.DockElement = new Label("R:");
    rSliderDockFill.FillElement = rSlider;

    rgbSlidersList.Elements.Add(rSliderDockFill);

    DockFillContainer gSliderDockFill = new() { DockDirection = DockDirection.Left, Gap = 8 };
    gSlider = new() { RaiseOnValueChangedEveryGrabFrame = true, MaximumValue = 255 };
    gSlider.OnValueChanged += new Action<float>((value) => { RGBSlidersChanged(); });
    gSliderDockFill.DockElement = new Label("G:");
    gSliderDockFill.FillElement = gSlider;

    rgbSlidersList.Elements.Add(gSliderDockFill);

    DockFillContainer bSliderDockFill = new() { DockDirection = DockDirection.Left, Gap = 8 };
    bSlider = new() { RaiseOnValueChangedEveryGrabFrame = true, MaximumValue = 255 };
    bSlider.OnValueChanged += new Action<float>((value) => { RGBSlidersChanged(); });
    bSliderDockFill.DockElement = new Label("B:");
    bSliderDockFill.FillElement = bSlider;

    rgbSlidersList.Elements.Add(bSliderDockFill);

    DockFillContainer colorPreviewDockFill = new() { Gap = 8f };

    colorPreview = new(
      new Label("Preview", 24) { 
        Margin = new Vector2(8), 
        Color = Color.White, 
        DrawShadow = true, 
        ShadowColor = Color.Black 
      }) { BackgroundColor = Color.Magenta };

    dockFillCustomColor.DockElement = colorPreview;
    dockFillCustomColor.FillElement = rgbSlidersList;

    optionsList.Elements.Add(dockFillCustomColor);

    transparentBackgroudWarning = new Label("To use transparent background you will need to capture the window with alpha.\nin OBS you can use GameCapture for that, by enabling the alpha channel") { HorizontalAlignment = LabelHorizontalAlignment.Left };
    optionsList.Elements.Add(transparentBackgroudWarning);

    // Load default values for the RGB sliders
    rSlider.CurrentValue = SettingsStore.WindowTransparencyColor.R;
    gSlider.CurrentValue = SettingsStore.WindowTransparencyColor.G;
    bSlider.CurrentValue = SettingsStore.WindowTransparencyColor.B;
  }

  private void BackgroundTransparencyOptionsComboBox_SelectedOptionChanged(ComboBoxOption obj) {
    if (obj.ExtraData is int selectionNumber) {
      dockFillCustomColor.Visible = false;
      SettingsStore.WindowTransparencyColorIsCustom = false;

      switch (selectionNumber) {
        case 0: { // Transparent
            SettingsStore.WindowTransparencyColor = Color.Transparent;
            break;
          }

        case 1: { // Green
            SettingsStore.WindowTransparencyColor = Color.FromNonPremultiplied(0, 255, 0, 255);
            break;
          }

        case 2: { // Blue
            SettingsStore.WindowTransparencyColor = Color.FromNonPremultiplied(0, 0, 255, 255);
            break;
          }

        case 3: { // Magenta
            SettingsStore.WindowTransparencyColor = Color.FromNonPremultiplied(255, 0, 255, 255);
            break;
          }

        default: { // Invalid/Custom Color
            SettingsStore.WindowTransparencyColorIsCustom = true;
            SettingsStore.WindowTransparencyColor = Color.FromNonPremultiplied(0, 255, 0, 255);
            rSlider.CurrentValue = SettingsStore.WindowTransparencyColor.R;
            gSlider.CurrentValue = SettingsStore.WindowTransparencyColor.G;
            bSlider.CurrentValue = SettingsStore.WindowTransparencyColor.B;
            break;
          }
      }
    }
  }

  void RGBSlidersChanged() {
    SettingsStore.WindowTransparencyColor = Color.FromNonPremultiplied((int)rSlider.CurrentValue, (int)gSlider.CurrentValue, (int)bSlider.CurrentValue, 255);
  }

  public override void UpdateUI(double deltaTime) {
    FillControl(optionsList);

    transparentBackgroudWarning.Visible = SettingsStore.WindowTransparencyColor == Color.Transparent && SettingsStore.WindowTransparencyColorIsCustom == false;

    dockFillCustomColor.Visible = SettingsStore.WindowTransparencyColorIsCustom;

    if (dockFillCustomColor.Visible) {
      colorPreview.BackgroundColor = SettingsStore.WindowTransparencyColor;
    }
  }

  public override void DrawControl(SpriteBatch spriteBatch, double deltaTime) {
    optionsList.Draw(spriteBatch, deltaTime);
  }

  public override bool InputUpdate(PointerEvent pointerEvent) {
    return optionsList.InputUpdate(pointerEvent);
  }

  public override bool InputUpdate(KeyboardEvent keyboardEvent) {
    return optionsList.InputUpdate(keyboardEvent);
  }

  public override void Update(double deltaTime) {
    optionsList.Update(deltaTime);
  }
}
