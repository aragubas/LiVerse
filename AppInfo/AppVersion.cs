using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiVerse.AppInfo {
  public struct AppVersion {
    public float Major { get; set; }
    public float Revision { get; set; }
    public string ReleaseChannel { get; set; }

    public override string ToString() {
      return $"{Major}.{Revision}({ReleaseChannel})";
    }
  }
}
