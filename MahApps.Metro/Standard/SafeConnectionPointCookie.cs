// Decompiled with JetBrains decompiler
// Type: Standard.SafeConnectionPointCookie
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using Microsoft.Win32.SafeHandles;
using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices.ComTypes;

#nullable disable
namespace Standard
{
  internal sealed class SafeConnectionPointCookie : SafeHandleZeroOrMinusOneIsInvalid
  {
    private IConnectionPoint _cp;

    public SafeConnectionPointCookie(IConnectionPointContainer target, object sink, Guid eventId)
      : base(true)
    {
      Verify.IsNotNull<IConnectionPointContainer>(target, nameof (target));
      Verify.IsNotNull<object>(sink, nameof (sink));
      Verify.IsNotDefault<Guid>(eventId, nameof (eventId));
      this.handle = IntPtr.Zero;
      IConnectionPoint ppCP = (IConnectionPoint) null;
      try
      {
        target.FindConnectionPoint(ref eventId, out ppCP);
        int pdwCookie;
        ppCP.Advise(sink, out pdwCookie);
        this.handle = pdwCookie != 0 ? new IntPtr(pdwCookie) : throw new InvalidOperationException("IConnectionPoint::Advise returned an invalid cookie.");
        this._cp = ppCP;
        ppCP = (IConnectionPoint) null;
      }
      finally
      {
        Utility.SafeRelease<IConnectionPoint>(ref ppCP);
      }
    }

    public void Disconnect() => this.ReleaseHandle();

    [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
    protected override bool ReleaseHandle()
    {
      try
      {
        if (!this.IsInvalid)
        {
          int int32 = this.handle.ToInt32();
          this.handle = IntPtr.Zero;
          try
          {
            this._cp.Unadvise(int32);
          }
          finally
          {
            Utility.SafeRelease<IConnectionPoint>(ref this._cp);
          }
        }
        return true;
      }
      catch
      {
        return false;
      }
    }
  }
}
