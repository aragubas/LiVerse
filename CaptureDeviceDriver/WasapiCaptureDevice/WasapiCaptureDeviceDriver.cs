﻿using NAudio.CoreAudioApi;
using NAudio.Wave;

namespace LiVerse.CaptureDeviceDriver.WasapiCaptureDevice {
  public class WasapiCaptureDeviceDriver : ICaptureDeviceDriver {
    public float TriggerLevel { get; set; } = 0.45f;
    public float MaximumLevel { get; set; }
    public double ActivationDelay { get; set; }
    public float ActivationDelayTrigger { get; set; } = 0.65f;

    public event Action<double>? MicrophoneVolumeLevelUpdated;
    public event Action? MicrophoneTriggerLevelTriggered;
    public event Action? MicrophoneLevelTriggered;
    public event Action? MicrophoneLevelUntriggered;
    public WasapiCapture? CurrentWasapiCaptureDevice;

    // Private Fields
    bool isMicrophoneLevelTriggered = false;

    public void Initialize() {

    }

    public void ChangeDevice(ICaptureDeviceInfo device) {
      
    }

    public ICaptureDeviceInfo[] GetCaptureDevices() {
      List<ICaptureDeviceInfo> returnDevices = new();
      MMDeviceCollection devices = new MMDeviceEnumerator().EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.Active);
      
      foreach( MMDevice device in devices ) {
        var info = new WasapiCaptureDeviceInfo(device.DeviceFriendlyName, device.DeviceTopology.DeviceId);
        returnDevices.Add(info);
      }

      return returnDevices.ToArray();
    }
    
    void SetDevice(MMDevice device) {
      // Stop capturing if capturing
      if (CurrentWasapiCaptureDevice != null && CurrentWasapiCaptureDevice.CaptureState == CaptureState.Capturing) {
        // Remove listener and stop recording
        CurrentWasapiCaptureDevice.DataAvailable -= CurrentWasapiCaptureDevice_DataAvailable;
        CurrentWasapiCaptureDevice.StopRecording();
      }

      CurrentWasapiCaptureDevice = new WasapiCapture(device, false, 100);

      CurrentWasapiCaptureDevice.DataAvailable += CurrentWasapiCaptureDevice_DataAvailable; ;
      CurrentWasapiCaptureDevice.StartRecording();
    }

    private void CurrentWasapiCaptureDevice_DataAvailable(object? sender, WaveInEventArgs e) {
      double rmsSum = 0;
      var buffer = new WaveBuffer(e.Buffer);
      for (int i = 0; i < e.BytesRecorded / 4; i++) {
        float sample = buffer.FloatBuffer[i];

        rmsSum += sample * sample;
      }

      double rms = Math.Sqrt(rmsSum);
      double levelDB = 20.0 * Math.Log10(rmsSum) + 20; // 20 is the silence point, converting to semi-linear scale xP
      MaximumLevel = 70;

      MicrophoneVolumeLevelUpdated?.Invoke(levelDB);

      if ((levelDB / MaximumLevel) >= TriggerLevel) {
        MicrophoneTriggerLevelTriggered?.Invoke();
        ActivationDelay = 1;
      }
    }

    public void SetDefaultDevice() => SetDevice(new MMDeviceEnumerator().GetDefaultAudioEndpoint(DataFlow.Capture, Role.Communications));    

    public void Update(double deltaTime) {
      if (ActivationDelay > 0) {
        ActivationDelay -= 1 * deltaTime;
      }

      if (ActivationDelay >= ActivationDelayTrigger) {
        if (!isMicrophoneLevelTriggered) {
          isMicrophoneLevelTriggered = true;

          MicrophoneLevelTriggered?.Invoke();
        }

      }
      else if (isMicrophoneLevelTriggered) {
        isMicrophoneLevelTriggered = false;

        MicrophoneLevelUntriggered?.Invoke();
      }
    }

  }
}