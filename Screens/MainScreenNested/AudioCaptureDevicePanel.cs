using LiVerse.AnaBanUI;
using LiVerse.AnaBanUI.Containers;
using LiVerse.AnaBanUI.Controls;
using LiVerse.AnaBanUI.Events;
using LiVerse.Stores;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json.Linq;

namespace LiVerse.Screens.MainScreenNested;
public class AudioCaptureDevicePanel : ControlBase {
  // Static ReadOnly Fields
  static readonly Color speakingIndicatorColor = ColorScheme.ControlBackgroundDisabled;
  static readonly Color speakingIndicatorActiveColor = ColorScheme.RedAccent;
  static readonly Color speakingIndicatorLabelColor = ColorScheme.TextDisabled;
  static readonly Color speakingIndicatorActiveLabelColor = ColorScheme.TextActive;

  VerticalLevelTrigger micLevelTrigger;
  VerticalLevelTrigger levelDelayTrigger;
  Label speakingIndicatorLabel;
  DockFillContainer sideFillContainer;
  SolidColorRectangle speakingIndicatorSolidColorRect;

  public AudioCaptureDevicePanel() {
    micLevelTrigger = new() { ShowPeaks = true, MaximumValue = 84 };
    levelDelayTrigger = new() { MaximumValue = 1 };

    SideBySideContainer sideBySide = new(this) { Gap = 8f };
    sideFillContainer = new(this) { DockDirection = DockDirection.Bottom, Margin = new(8), Gap = 8, FillElement = sideBySide };

    sideBySide.Elements.Add(micLevelTrigger);
    sideBySide.Elements.Add(levelDelayTrigger);

    // HACK: Label alignment set to bottom to fix weird issue with text's base line
    speakingIndicatorLabel = new("Active", 18) { Color = speakingIndicatorLabelColor, VerticalAlignment = LabelVerticalAlignment.Bottom };
    speakingIndicatorSolidColorRect = new(speakingIndicatorLabel) { BackgroundColor = speakingIndicatorColor };

    sideFillContainer.DockElement = speakingIndicatorSolidColorRect;

    if (CaptureDeviceDriverStore.CaptureDeviceDriver != null) {
      CaptureDeviceDriverStore.CaptureDeviceDriver.MicrophoneLevelTriggered += CaptureDeviceDriver_MicrophoneLevelTriggered;
      CaptureDeviceDriverStore.CaptureDeviceDriver.MicrophoneLevelUntriggered += CaptureDeviceDriver_MicrophoneLevelUntriggered;
      CaptureDeviceDriverStore.CaptureDeviceDriver.MicrophoneVolumeLevelUpdated += CaptureDeviceDriver_MicrophoneVolumeLevelUpdated;

      // Sincronize Values
      micLevelTrigger.TriggerLevel = CaptureDeviceDriverStore.CaptureDeviceDriver.TriggerLevel;
      micLevelTrigger.MaximumValue = CaptureDeviceDriverStore.CaptureDeviceDriver.MaximumLevel;
      levelDelayTrigger.TriggerLevel = CaptureDeviceDriverStore.CaptureDeviceDriver.ActivationDelayTrigger;
    }

  }

  private void CaptureDeviceDriver_MicrophoneVolumeLevelUpdated(double obj) {
    micLevelTrigger.CurrentValue = (float)obj;
  }

  private void CaptureDeviceDriver_MicrophoneLevelUntriggered() {
    speakingIndicatorSolidColorRect.BackgroundColor = speakingIndicatorColor;
    speakingIndicatorLabel.Color = speakingIndicatorLabelColor;
  }

  private void CaptureDeviceDriver_MicrophoneLevelTriggered() {
    speakingIndicatorSolidColorRect.BackgroundColor = speakingIndicatorActiveColor;
    speakingIndicatorLabel.Color = speakingIndicatorActiveLabelColor;
  }

  public override void UpdateUI(double deltaTime) {
    FillControl(sideFillContainer);
  }

  public override void DrawControl(SpriteBatch spriteBatch, double deltaTime) {
    sideFillContainer.Draw(spriteBatch, deltaTime);
  }

  public override bool InputUpdate(KeyboardEvent keyboardEvent) {
    return sideFillContainer.InputUpdate(keyboardEvent);
  }

  public override bool InputUpdate(PointerEvent pointerEvent) {
    return sideFillContainer.InputUpdate(pointerEvent);
  }

  public override void Update(double deltaTime) {

    if (CaptureDeviceDriverStore.CaptureDeviceDriver != null) {
      // Sincronize Changes
      CaptureDeviceDriverStore.CaptureDeviceDriver.TriggerLevel = micLevelTrigger.TriggerLevel;
      CaptureDeviceDriverStore.CaptureDeviceDriver.ActivationDelayTrigger = levelDelayTrigger.TriggerLevel;
    } else {
      micLevelTrigger.CurrentValue = 0;
    }

    sideFillContainer.Update(deltaTime);

    if (CaptureDeviceDriverStore.CaptureDeviceDriver != null) {
      // Set Values
      levelDelayTrigger.CurrentValue = (float)CaptureDeviceDriverStore.CaptureDeviceDriver.ActivationDelay;

      // Sincronize Values
      micLevelTrigger.TriggerLevel = CaptureDeviceDriverStore.CaptureDeviceDriver.TriggerLevel;
      micLevelTrigger.MaximumValue = CaptureDeviceDriverStore.CaptureDeviceDriver.MaximumLevel;
      levelDelayTrigger.TriggerLevel = CaptureDeviceDriverStore.CaptureDeviceDriver.ActivationDelayTrigger;
    }
  }
}
