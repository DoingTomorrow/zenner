// Decompiled with JetBrains decompiler
// Type: MahApps.Metro.Controls.MetroThumb
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using System.Windows.Controls.Primitives;
using System.Windows.Input;

#nullable disable
namespace MahApps.Metro.Controls
{
  public class MetroThumb : Thumb
  {
    private TouchDevice _currentDevice;

    protected override void OnPreviewTouchDown(TouchEventArgs e)
    {
      this.ReleaseCurrentDevice();
      this.CaptureCurrentDevice(e);
    }

    protected override void OnPreviewTouchUp(TouchEventArgs e) => this.ReleaseCurrentDevice();

    protected override void OnLostTouchCapture(TouchEventArgs e)
    {
      if (this._currentDevice == null)
        return;
      this.CaptureCurrentDevice(e);
    }

    private void ReleaseCurrentDevice()
    {
      if (this._currentDevice == null)
        return;
      TouchDevice currentDevice = this._currentDevice;
      this._currentDevice = (TouchDevice) null;
      this.ReleaseTouchCapture(currentDevice);
    }

    private void CaptureCurrentDevice(TouchEventArgs e)
    {
      if (!this.CaptureTouch(e.TouchDevice))
        return;
      this._currentDevice = e.TouchDevice;
    }
  }
}
