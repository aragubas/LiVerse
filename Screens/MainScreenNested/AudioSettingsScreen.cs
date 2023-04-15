using LiVerse.AnaBanUI;
using LiVerse.AnaBanUI.Containers;
using LiVerse.AnaBanUI.Controls;
using LiVerse.AnaBanUI.Events;
using LiVerse.CaptureDeviceDriver;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace LiVerse.Screens.MainScreenNested {
  public class AudioSettingsScreen : ControlBase {
    ScrollableList scrollableList;
    DockFillContainer dockFill;

    public AudioSettingsScreen() {
      scrollableList = new() { ParentControl = this, Gap = 6 };

      dockFill = new() { ParentControl = this };
      Label audioInputDeviceToggleTitle = new("Input Device: ") { Color = Color.Black };
      List<ComboBoxOption> options = new();

      foreach(var captureDevice in CaptureDeviceDriverManager.CaptureDeviceDriver.GetCaptureDevices()) {
        options.Add(new ComboBoxOption(captureDevice.DeviceName, captureDevice));
      }

      ComboBox audioDevicesComboBox = new(options[0], options);
      audioDevicesComboBox.SelectedOptionChanged += ChangeAudioDevice;

      dockFill.DockType = DockFillContainerDockType.Left;
      dockFill.DockElement = audioInputDeviceToggleTitle;
      dockFill.FillElement = audioDevicesComboBox;

      Label audioDriverNameLabel = new($"Current Audio Driver: " +
        $"{(CaptureDeviceDriverManager.CaptureDeviceDriver == null ? "None" : CaptureDeviceDriverManager.CaptureDeviceDriver.DriverName)}") {
        Color = Color.Black
      };

      scrollableList.Elements.Add(audioDriverNameLabel);
      scrollableList.Elements.Add(dockFill);
    }

    void ChangeAudioDevice(ComboBoxOption option) {
      if (option.ExtraData == null) {
        Console.WriteLine("[UNDEFINED BEHAVIOUR]: (0x83092) AudioSettingsScreen->ChangeAudioDevice in option->ExtraData is null");
        return;
      }
      CaptureDeviceDriverManager.CaptureDeviceDriver.ChangeDevice((ICaptureDeviceInfo)option.ExtraData);
    }

    public override void DrawElement(SpriteBatch spriteBatch, double deltaTime) {
      scrollableList.Draw(spriteBatch, deltaTime);
    }

    public override bool InputUpdate(PointerEvent pointerEvent) {
      return scrollableList.InputUpdate(pointerEvent);
    }

    public override void Update(double deltaTime) {
      scrollableList.Size = ContentArea;
      scrollableList.RelativePosition = RelativePosition;
      scrollableList.AbsolutePosition = AbsolutePosition;

      MinimumSize = scrollableList.MinimumSize;

      scrollableList.Update(deltaTime);
    }
  }
}
