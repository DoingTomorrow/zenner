// Decompiled with JetBrains decompiler
// Type: Standard.PROPVARIANT
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Standard
{
  [StructLayout(LayoutKind.Explicit)]
  internal class PROPVARIANT : IDisposable
  {
    [FieldOffset(0)]
    private ushort vt;
    [FieldOffset(8)]
    private IntPtr pointerVal;
    [FieldOffset(8)]
    private byte byteVal;
    [FieldOffset(8)]
    private long longVal;
    [FieldOffset(8)]
    private short boolVal;

    public VarEnum VarType => (VarEnum) this.vt;

    public string GetValue()
    {
      return this.vt == (ushort) 31 ? Marshal.PtrToStringUni(this.pointerVal) : (string) null;
    }

    public void SetValue(bool f)
    {
      this.Clear();
      this.vt = (ushort) 11;
      this.boolVal = f ? (short) -1 : (short) 0;
    }

    public void SetValue(string val)
    {
      this.Clear();
      this.vt = (ushort) 31;
      this.pointerVal = Marshal.StringToCoTaskMemUni(val);
    }

    public void Clear() => PROPVARIANT.NativeMethods.PropVariantClear(this);

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    ~PROPVARIANT() => this.Dispose(false);

    private void Dispose(bool disposing) => this.Clear();

    private static class NativeMethods
    {
      [DllImport("ole32.dll")]
      internal static extern HRESULT PropVariantClear(PROPVARIANT pvar);
    }
  }
}
