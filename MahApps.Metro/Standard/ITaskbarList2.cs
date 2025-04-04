// Decompiled with JetBrains decompiler
// Type: Standard.ITaskbarList2
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Standard
{
  [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
  [Guid("602D4995-B13A-429b-A66E-1935E44F4317")]
  [ComImport]
  internal interface ITaskbarList2 : ITaskbarList
  {
    new void HrInit();

    new void AddTab(IntPtr hwnd);

    new void DeleteTab(IntPtr hwnd);

    new void ActivateTab(IntPtr hwnd);

    new void SetActiveAlt(IntPtr hwnd);

    void MarkFullscreenWindow(IntPtr hwnd, [MarshalAs(UnmanagedType.Bool)] bool fFullscreen);
  }
}
