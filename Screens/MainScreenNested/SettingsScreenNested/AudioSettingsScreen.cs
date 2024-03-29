﻿using LiVerse.AnaBanUI;
using LiVerse.AnaBanUI.Containers;
using LiVerse.AnaBanUI.Controls;
using LiVerse.AnaBanUI.Controls.ComboBox;
using LiVerse.AnaBanUI.Events;
using LiVerse.CaptureDeviceDriver;
using LiVerse.Stores;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LiVerse.Screens.MainScreenNested.SettingsScreenNested;

public class AudioSettingsScreen : ControlBase {
  ScrollableList ScrollableList { get; }
  DockFillContainer DockFill { get; }

  public AudioSettingsScreen() {
    ScrollableList = new() {
      ParentControl = this,
      Gap = 8
    };

    DockFill = new() {
      ParentControl = this,
      DockDirection = DockDirection.Left,
      Gap = 8
    };
    Label audioInputDeviceToggleTitle = new("Input Device: ");
    List<ComboBoxOption> options = new();

    if (CaptureDeviceDriverStore.CaptureDeviceDriver != null) {
      foreach (var captureDevice in CaptureDeviceDriverStore.CaptureDeviceDriver.GetCaptureDevices()) {
        options.Add(new ComboBoxOption(captureDevice.DeviceName, captureDevice));
      }
    }

    ComboBoxOption defaultOption = new() { OptionText = "None/Unavailable" };
    if (CaptureDeviceDriverStore.CaptureDeviceDriver != null) {
      defaultOption.OptionText = CaptureDeviceDriverStore.CaptureDeviceDriver.CurrentCaptureDevice.DeviceName;
      defaultOption.ExtraData = CaptureDeviceDriverStore.CaptureDeviceDriver.CurrentCaptureDevice.DeviceId;
    }

    ComboBoxControl audioDevicesComboBox = new(defaultOption, options);
    audioDevicesComboBox.SelectedOptionChanged += ChangeAudioDevice;

    DockFill.DockElement = audioInputDeviceToggleTitle;
    DockFill.FillElement = audioDevicesComboBox;

    Label audioDriverNameLabel = new($"Audio Driver: " +
      $"{(CaptureDeviceDriverStore.CaptureDeviceDriver == null ? "None" : CaptureDeviceDriverStore.CaptureDeviceDriver.DriverName)}") {
      HorizontalAlignment = LabelHorizontalAlignment.Left,
    };

    ScrollableList.Elements.Add(audioDriverNameLabel);
    ScrollableList.Elements.Add(DockFill);
  }

  void ChangeAudioDevice(ComboBoxOption option) {
    if (option.ExtraData == null) {
      Console.WriteLine("[UNDEFINED BEHAVIOUR]: (0x83092) AudioSettingsScreen::ChangeAudioDevice in option::ExtraData is null");
      return;
    }
    CaptureDeviceDriverStore.CaptureDeviceDriver.ChangeDevice((ICaptureDeviceInfo)option.ExtraData);
  }

  public override void UpdateUI(double deltaTime) {
    FillControl(ScrollableList);
  }
  public override void DrawControl(SpriteBatch spriteBatch, double deltaTime) {
    ScrollableList.Draw(spriteBatch, deltaTime);
  }

  public override bool InputUpdate(PointerEvent pointerEvent) {
    return ScrollableList.InputUpdate(pointerEvent);
  }

  public override void Update(double deltaTime) {
    ScrollableList.Update(deltaTime);
  }
}
