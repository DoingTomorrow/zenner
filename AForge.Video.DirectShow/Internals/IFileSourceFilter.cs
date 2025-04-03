// Decompiled with JetBrains decompiler
// Type: AForge.Video.DirectShow.Internals.IFileSourceFilter
// Assembly: AForge.Video.DirectShow, Version=2.2.5.0, Culture=neutral, PublicKeyToken=61ea4348d43881b7
// MVID: 40B45F39-FACC-42DB-95D1-CED109AC01F1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AForge.Video.DirectShow.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace AForge.Video.DirectShow.Internals
{
  [Guid("56A868A6-0Ad4-11CE-B03A-0020AF0BA770")]
  [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
  [ComImport]
  internal interface IFileSourceFilter
  {
    [MethodImpl(MethodImplOptions.PreserveSig)]
    int Load([MarshalAs(UnmanagedType.LPWStr), In] string fileName, [MarshalAs(UnmanagedType.LPStruct), In] AMMediaType mediaType);

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int GetCurFile([MarshalAs(UnmanagedType.LPWStr)] out string fileName, [MarshalAs(UnmanagedType.LPStruct), Out] AMMediaType mediaType);
  }
}
