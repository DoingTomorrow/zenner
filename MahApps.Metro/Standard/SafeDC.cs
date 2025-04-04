﻿// Decompiled with JetBrains decompiler
// Type: Standard.SafeDC
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using Microsoft.Win32.SafeHandles;
using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;

#nullable disable
namespace Standard
{
  internal sealed class SafeDC : SafeHandleZeroOrMinusOneIsInvalid
  {
    private IntPtr? _hwnd;
    private bool _created;

    public IntPtr Hwnd
    {
      set => this._hwnd = new IntPtr?(value);
    }

    private SafeDC()
      : base(true)
    {
    }

    [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
    protected override bool ReleaseHandle()
    {
      if (this._created)
        return SafeDC.NativeMethods.DeleteDC(this.handle);
      return !this._hwnd.HasValue || this._hwnd.Value == IntPtr.Zero || SafeDC.NativeMethods.ReleaseDC(this._hwnd.Value, this.handle) == 1;
    }

    public static SafeDC CreateDC(string deviceName)
    {
      SafeDC dc = (SafeDC) null;
      try
      {
        dc = SafeDC.NativeMethods.CreateDC(deviceName, (string) null, IntPtr.Zero, IntPtr.Zero);
      }
      finally
      {
        if (dc != null)
          dc._created = true;
      }
      if (dc.IsInvalid)
      {
        dc.Dispose();
        throw new SystemException("Unable to create a device context from the specified device information.");
      }
      return dc;
    }

    public static SafeDC CreateCompatibleDC(SafeDC hdc)
    {
      SafeDC compatibleDc = (SafeDC) null;
      try
      {
        IntPtr hdc1 = IntPtr.Zero;
        if (hdc != null)
          hdc1 = hdc.handle;
        compatibleDc = SafeDC.NativeMethods.CreateCompatibleDC(hdc1);
        if (compatibleDc == null)
          HRESULT.ThrowLastError();
      }
      finally
      {
        if (compatibleDc != null)
          compatibleDc._created = true;
      }
      if (compatibleDc.IsInvalid)
      {
        compatibleDc.Dispose();
        throw new SystemException("Unable to create a device context from the specified device information.");
      }
      return compatibleDc;
    }

    public static SafeDC GetDC(IntPtr hwnd)
    {
      SafeDC dc = (SafeDC) null;
      try
      {
        dc = SafeDC.NativeMethods.GetDC(hwnd);
      }
      finally
      {
        if (dc != null)
          dc.Hwnd = hwnd;
      }
      if (dc.IsInvalid)
        HRESULT.E_FAIL.ThrowIfFailed();
      return dc;
    }

    public static SafeDC GetDesktop() => SafeDC.GetDC(IntPtr.Zero);

    public static SafeDC WrapDC(IntPtr hdc)
    {
      SafeDC safeDc = new SafeDC();
      safeDc.handle = hdc;
      safeDc._created = false;
      safeDc._hwnd = new IntPtr?(IntPtr.Zero);
      return safeDc;
    }

    private static class NativeMethods
    {
      [DllImport("user32.dll")]
      public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

      [DllImport("user32.dll")]
      public static extern SafeDC GetDC(IntPtr hwnd);

      [DllImport("gdi32.dll", CharSet = CharSet.Unicode)]
      public static extern SafeDC CreateDC(
        [MarshalAs(UnmanagedType.LPWStr)] string lpszDriver,
        [MarshalAs(UnmanagedType.LPWStr)] string lpszDevice,
        IntPtr lpszOutput,
        IntPtr lpInitData);

      [DllImport("gdi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
      public static extern SafeDC CreateCompatibleDC(IntPtr hdc);

      [DllImport("gdi32.dll")]
      [return: MarshalAs(UnmanagedType.Bool)]
      public static extern bool DeleteDC(IntPtr hdc);
    }
  }
}
