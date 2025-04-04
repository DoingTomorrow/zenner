// Decompiled with JetBrains decompiler
// Type: Standard.IDropTargetHelper
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

#nullable disable
namespace Standard
{
  [Guid("4657278B-411B-11D2-839A-00C04FD918D0")]
  [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
  [ComImport]
  internal interface IDropTargetHelper
  {
    void DragEnter(IntPtr hwndTarget, IDataObject pDataObject, ref POINT ppt, int effect);

    void DragLeave();

    void DragOver(ref POINT ppt, int effect);

    void Drop(IDataObject dataObject, ref POINT ppt, int effect);

    void Show([MarshalAs(UnmanagedType.Bool)] bool fShow);
  }
}
