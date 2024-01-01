using NAudio.CoreAudioApi;
using NAudio.Wave;

namespace LiVerse.CaptureDeviceDriver.WasapiCaptureDevice {
  public class WasapiCaptureDeviceDriver : ICaptureDeviceDriver {
    // Maximum value observed from tests (playing sinewave at maximum volume)
    static readonly float s_MaximumObservedValue = 0.99998856f;

    public float TriggerLevel { get; set; } = 0.45f;
    public float MaximumLevel { get; set; } = 100f;
    public double ActivationDelay { get; set; }
    public float ActivationDelayTrigger { get; set; } = 0.65f;

    ICaptureDeviceInfo? _currentCaptureDevice;
    public ICaptureDeviceInfo? CurrentCaptureDevice { get => _currentCaptureDevice; }
    public string DriverName => "NAudio WasapiCaptureDeviceDriver for Windows";
    public event Action<double>? MicrophoneVolumeLevelUpdated;
    public event Action? MicrophoneTriggerLevelTriggered;
    public event Action? MicrophoneLevelTriggered;
    public event Action? MicrophoneLevelUntriggered;
    public WasapiCapture? CurrentWasapiCaptureDevice;

    // Private Fields
    bool isMicrophoneLevelTriggered = false;

    public WasapiCaptureDeviceDriver() {
    }

    public void Initialize() {

    }

    public void ChangeDevice(ICaptureDeviceInfo device) {
      MMDeviceCollection devices = new MMDeviceEnumerator().EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.Active);

      foreach (MMDevice mmDevice in devices) {
        var info = new WasapiCaptureDeviceInfo(mmDevice.DeviceFriendlyName, mmDevice.DeviceTopology.DeviceId);
        
        // Device Found
        if (device.DeviceName == info.DeviceName && (string)device.DeviceId == mmDevice.DeviceTopology.DeviceId) {
          SetDevice(mmDevice);
          return;
        }
      }

      Console.WriteLine($"[WasapiCaptureDeviceDriver::ChangeDeviceDevice] {device.DeviceName} not found.");
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
      if (CurrentWasapiCaptureDevice != null) {
        // Remove listener and stop recording
        CurrentWasapiCaptureDevice.DataAvailable -= CurrentWasapiCaptureDevice_DataAvailable;
        CurrentWasapiCaptureDevice.StopRecording();
        CurrentWasapiCaptureDevice.Dispose();
      }

      CurrentWasapiCaptureDevice = new WasapiCapture(device, false, 50);
      
      CurrentWasapiCaptureDevice.DataAvailable += CurrentWasapiCaptureDevice_DataAvailable; ;
      CurrentWasapiCaptureDevice.StartRecording();

      _currentCaptureDevice = new WasapiCaptureDeviceInfo(device.DeviceFriendlyName, device.DeviceTopology.DeviceId);
      Console.WriteLine($"[WasapiCaptureDeviceDriver::SetDevice] {device.DeviceFriendlyName}");
    }

    private void CurrentWasapiCaptureDevice_DataAvailable(object? sender, WaveInEventArgs e) {
      var buffer = new WaveBuffer(e.Buffer);

      double sum = 0;
      for (int i = 0; i < e.BytesRecorded / 4; i++) {
        double sample = buffer.FloatBuffer[i];

        sum += sample * sample;
      }
      double rms = Math.Sqrt(sum / buffer.FloatBuffer.Length);
      double level = 92.8 + 20 * Math.Log10(rms);
      
      // Converts to 0 - 100 scale
      level = Math.Clamp(level, 0, 100) / 81 * 100;

      MicrophoneVolumeLevelUpdated?.Invoke(level);

      // Converts level to 0 - 1 scale, since TriggerLevel
      // works in a 0.0 - 1.0 scale
      if ((level / MaximumLevel) >= TriggerLevel) {
        MicrophoneTriggerLevelTriggered?.Invoke();
        ActivationDelay = 1.1; // HACK: Fixes level swinging when active
      }
    }

    public void SetDefaultDevice() => SetDevice(new MMDeviceEnumerator().GetDefaultAudioEndpoint(DataFlow.Capture, Role.Multimedia));    

    public void Update(double deltaTime) {
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

      if (ActivationDelay >= 0) {
        ActivationDelay -= 1 * deltaTime;
      }
    }

    public void Dispose() {
      _currentCaptureDevice = null;

      if (CurrentWasapiCaptureDevice != null) {
        CurrentWasapiCaptureDevice.StopRecording();
        CurrentWasapiCaptureDevice.Dispose();
      }
    }

  }
}
