namespace LiVerse.CaptureDeviceDriver {
  public interface ICaptureDeviceDriver {
    public event Action<double>? MicrophoneVolumeLevelUpdated;
    public event Action? MicrophoneTriggerLevelTriggered;
    public event Action? MicrophoneLevelTriggered;
    public event Action? MicrophoneLevelUntriggered;
    public float TriggerLevel { get; set; }
    public float MaximumLevel { get; set; }
    public double ActivationDelay { get; set; }
    public float ActivationDelayTrigger { get; set; }

    ICaptureDeviceInfo[] GetCaptureDevices();
    void Initialize();
    void SetDefaultDevice();
    void ChangeDevice(ICaptureDeviceInfo device);
    void Update(double deltaTime);
  }
}
