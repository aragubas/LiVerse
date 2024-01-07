using LiVerse.CaptureDeviceDriver;
using LiVerse.CaptureDeviceDriver.WasapiCaptureDevice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiVerse.Stores; 
public static class CaptureDeviceDriverStore {
  public static ICaptureDeviceDriver CaptureDeviceDriver { get; set; }

  static CaptureDeviceDriverStore() {
    CaptureDeviceDriver = new WasapiCaptureDeviceDriver();
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
