using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiVerse.CaptureDeviceDriver.WasapiCaptureDevice {
  public class WasapiCaptureDeviceInfo : ICaptureDeviceInfo {
    public string DeviceName { get; set; }
    public string DeviceId { get; set; }

    public WasapiCaptureDeviceInfo(string deviceName, string deviceId) {
      DeviceName = deviceName;
      DeviceId = deviceId;
    }
  }
}
