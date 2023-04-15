using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiVerse.CaptureDeviceDriver.WasapiCaptureDevice {
  public class WasapiCaptureDeviceInfo : ICaptureDeviceInfo {
    public string DeviceName { get; set; }
    public object DeviceId { get; set; }

    public WasapiCaptureDeviceInfo(string deviceName, object deviceId) {
      DeviceName = deviceName;
      DeviceId = deviceId;
    }
  }
}
