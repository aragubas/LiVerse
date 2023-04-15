using LiVerse.CaptureDeviceDriver.WasapiCaptureDevice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiVerse.CaptureDeviceDriver {
  public static class CaptureDeviceDriverManager {
    public static ICaptureDeviceDriver CaptureDeviceDriver { get; set; }

    static CaptureDeviceDriverManager() {
      CaptureDeviceDriver = new WasapiCaptureDeviceDriver();
    }
  }
}
