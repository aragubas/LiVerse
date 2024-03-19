using LiVerse.CaptureDeviceDriver;

namespace LiVerse.Stores;
public static class CaptureDeviceDriverStore {
  public static ICaptureDeviceDriver? CaptureDeviceDriver { get; set; }

  static CaptureDeviceDriverStore() {
#if WINDOWS
    CaptureDeviceDriver = new WasapiCaptureDeviceDriver();
#else
    CaptureDeviceDriver = null;
#endif
  }

  public static void Update(double deltaTime) {
    CaptureDeviceDriver?.Update(deltaTime);
  }

  // Load Settings
  public static void Load() {

  }

  // Save Settings
  public static void Save() {

  }
}
