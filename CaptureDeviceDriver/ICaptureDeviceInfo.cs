namespace LiVerse.CaptureDeviceDriver {
  public interface ICaptureDeviceInfo {
    /// <summary>
    /// Public Device Name
    /// </summary>
    public string DeviceName { get; set; }
    public object DeviceId { get; set; }
  }
}
