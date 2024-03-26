using LiVerse.AnaBanUI;
using LiVerse.AnaBanUI.Containers;
using LiVerse.AnaBanUI.Controls;
using LiVerse.Stores;
using SFML.Graphics;

namespace LiVerse.Screens;

public class MainScreen : ScreenBase {
  bool characterFullView = false;
  Button button;
  UILayer mainLayer;

  public MainScreen(ScreenManager screenManager) : base(screenManager) {
    // Layout
    // MainFillContainer
    //   dock: HeaderBar
    //   fill: CenterSplit
    //          dock: AudioCaptureDevicePanel
    //          fill: CenterCharacterSplit
    //                  dock: CharacterExpressionsPanel
    //                  fill: CharacterRenderer

    mainLayer = new(screenManager.UIRoot);
    button = new("Bwah!");
    Button button2 = new("Abawa");

    FlexibleContainer flex = new() {
      Gap = 50f,
      Padding = new SFML.System.Vector2f(50, 50)
    };
    flex.Controls.Add(button);
    flex.Controls.Add(button2);

    mainLayer.RootControl = flex;

    screenManager.UIRoot.PushUILayer(mainLayer);

    if (CaptureDeviceDriverStore.CaptureDeviceDriver != null) {
      CaptureDeviceDriverStore.CaptureDeviceDriver.MicrophoneLevelTriggered += MicrophoneLevelMeter_MicrophoneLevelTriggered;
      CaptureDeviceDriverStore.CaptureDeviceDriver.MicrophoneLevelUntriggered += MicrophoneLevelMeter_MicrophoneLevelUntriggered;

      CaptureDeviceDriverStore.CaptureDeviceDriver.Initialize();
      CaptureDeviceDriverStore.CaptureDeviceDriver.SetDefaultDevice();
    }

  }

  private void MicrophoneLevelMeter_MicrophoneLevelUntriggered() {
  }

  private void MicrophoneLevelMeter_MicrophoneLevelTriggered() {
  }


  public override void Detach() { }

  public override void Dispose() {
    if (CaptureDeviceDriverStore.CaptureDeviceDriver != null) {
      CaptureDeviceDriverStore.CaptureDeviceDriver.Dispose();
    }
  }

  public override void Update(double deltaTime) {
    // Update AnaBanUI
    ScreenManager.UIRoot.Update(deltaTime);
  }


  public override void Draw(RenderTarget target, RenderStates states) {
    // Draw AnaBanUI
    target.Draw(ScreenManager.UIRoot, states);
  }
}
