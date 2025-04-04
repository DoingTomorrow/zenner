// Decompiled with JetBrains decompiler
// Type: Standard.ITaskbarList
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Standard
{
  [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
  [Guid("56FDF342-FD6D-11d0-958A-006097C9A090")]
  [ComImport]
  internal interface ITaskbarList
  {
    void HrInit();

    void AddTab(IntPtr hwnd);

    void DeleteTab(IntPtr hwnd);

    void ActivateTab(IntPtr hwnd);

    void SetActiveAlt(IntPtr hwnd);
  }
}
