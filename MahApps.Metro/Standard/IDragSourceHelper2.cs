// Decompiled with JetBrains decompiler
// Type: Standard.IDragSourceHelper2
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

#nullable disable
namespace Standard
{
  [Guid("83E07D0D-0C5F-4163-BF1A-60B274051E40")]
  [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
  [ComImport]
  internal interface IDragSourceHelper2 : IDragSourceHelper
  {
    new void InitializeFromBitmap([In] ref SHDRAGIMAGE pshdi, [In] IDataObject pDataObject);

    new void InitializeFromWindow(IntPtr hwnd, [In] ref POINT ppt, [In] IDataObject pDataObject);

    void SetFlags(DSH dwFlags);
  }
}
