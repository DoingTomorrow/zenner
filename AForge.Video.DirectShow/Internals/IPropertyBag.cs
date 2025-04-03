// Decompiled with JetBrains decompiler
// Type: AForge.Video.DirectShow.Internals.IPropertyBag
// Assembly: AForge.Video.DirectShow, Version=2.2.5.0, Culture=neutral, PublicKeyToken=61ea4348d43881b7
// MVID: 40B45F39-FACC-42DB-95D1-CED109AC01F1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AForge.Video.DirectShow.dll

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace AForge.Video.DirectShow.Internals
{
  [Guid("55272A00-42CB-11CE-8135-00AA004BB851")]
  [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
  [ComImport]
  internal interface IPropertyBag
  {
    [MethodImpl(MethodImplOptions.PreserveSig)]
    int Read([MarshalAs(UnmanagedType.LPWStr), In] string propertyName, [MarshalAs(UnmanagedType.Struct), In, Out] ref object pVar, [In] IntPtr pErrorLog);

    [MethodImpl(MethodImplOptions.PreserveSig)]
    int Write([MarshalAs(UnmanagedType.LPWStr), In] string propertyName, [MarshalAs(UnmanagedType.Struct), In] ref object pVar);
  }
}
