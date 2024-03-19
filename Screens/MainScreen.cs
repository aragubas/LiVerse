using LiVerse.Stores;
using SFML.Graphics;

namespace LiVerse.Screens;

public class MainScreen : ScreenBase {
  bool characterFullView = false;

  public MainScreen(ScreenManager screenManager) : base(screenManager) {
    // Layout
    // MainFillContainer
    //   dock: HeaderBar
    //   fill: CenterSplit
    //          dock: AudioCaptureDevicePanel
    //          fill: CenterCharacterSplit
    //                  dock: CharacterExpressionsPanel
    //                  fill: CharacterRenderer

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
  }


  public override void Draw(RenderTarget target, RenderStates states) {

  }
}
