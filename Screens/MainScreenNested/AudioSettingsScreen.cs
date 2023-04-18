using LiVerse.AnaBanUI;
using LiVerse.AnaBanUI.Containers;
using LiVerse.AnaBanUI.Controls;
using LiVerse.AnaBanUI.Controls.ComboBox;
using LiVerse.AnaBanUI.Events;
using LiVerse.CaptureDeviceDriver;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LiVerse.Screens.MainScreenNested
{
    public class AudioSettingsScreen : ControlBase {
    ScrollableList ScrollableList { get; }
    DockFillContainer DockFill { get; }

    public AudioSettingsScreen() {
      ScrollableList = new() { ParentControl = this, Gap = 6 };

      DockFill = new() { ParentControl = this, DockType = DockFillContainerDockType.Left, Gap = 6, DrawDebugLines = true };
      Label audioInputDeviceToggleTitle = new("Input Device: ") { Color = Color.Black };
      List<ComboBoxOption> options = new();

      foreach(var captureDevice in CaptureDeviceDriverManager.CaptureDeviceDriver.GetCaptureDevices()) {
        options.Add(new ComboBoxOption(captureDevice.DeviceName, captureDevice));
      }

      ComboBoxOption defaultOption = new() { OptionText = "None" };
      if (CaptureDeviceDriverManager.CaptureDeviceDriver.CurrentCaptureDevice != null) {
        defaultOption.OptionText = CaptureDeviceDriverManager.CaptureDeviceDriver.CurrentCaptureDevice.DeviceName;
        defaultOption.ExtraData = CaptureDeviceDriverManager.CaptureDeviceDriver.CurrentCaptureDevice.DeviceId;
      }

      ComboBoxControl audioDevicesComboBox = new(defaultOption, options);
      audioDevicesComboBox.SelectedOptionChanged += ChangeAudioDevice;

      DockFill.DockElement = audioInputDeviceToggleTitle;
      DockFill.FillElement = audioDevicesComboBox;

      Label audioDriverNameLabel = new($"Current Audio Driver: " +
        $"{(CaptureDeviceDriverManager.CaptureDeviceDriver == null ? "None" : CaptureDeviceDriverManager.CaptureDeviceDriver.DriverName)}") {
        Color = Color.Black,
        TextHorizontalAlignment = LabelTextHorizontalAlignment.Left,
      };

      ScrollableList.Elements.Add(audioDriverNameLabel);
      ScrollableList.Elements.Add(DockFill);
    }

    void ChangeAudioDevice(ComboBoxOption option) {
      if (option.ExtraData == null) {
        Console.WriteLine("[UNDEFINED BEHAVIOUR]: (0x83092) AudioSettingsScreen->ChangeAudioDevice in option->ExtraData is null");
        return;
      }
      CaptureDeviceDriverManager.CaptureDeviceDriver.ChangeDevice((ICaptureDeviceInfo)option.ExtraData);
    }

    public override void DrawElement(SpriteBatch spriteBatch, double deltaTime) {
      ScrollableList.Draw(spriteBatch, deltaTime);
    }

    public override bool InputUpdate(PointerEvent pointerEvent) {
      return ScrollableList.InputUpdate(pointerEvent);
    }

    public override void Update(double deltaTime) {
      ScrollableList.Size = ContentArea;
      ScrollableList.RelativePosition = RelativePosition;
      ScrollableList.AbsolutePosition = AbsolutePosition;

      MinimumSize = ScrollableList.MinimumSize;

      ScrollableList.Update(deltaTime);
    }
  }
}
