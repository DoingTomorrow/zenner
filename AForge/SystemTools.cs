// Decompiled with JetBrains decompiler
// Type: AForge.SystemTools
// Assembly: AForge, Version=2.2.5.0, Culture=neutral, PublicKeyToken=c1db6ff4eaa06aeb
// MVID: D4933F01-4742-407D-982E-D47DDB340621
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\AForge.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace AForge
{
  public static class SystemTools
  {
    public static unsafe IntPtr CopyUnmanagedMemory(IntPtr dst, IntPtr src, int count)
    {
      SystemTools.CopyUnmanagedMemory((byte*) dst.ToPointer(), (byte*) src.ToPointer(), count);
      return dst;
    }

    public static unsafe byte* CopyUnmanagedMemory(byte* dst, byte* src, int count)
    {
      return SystemTools.memcpy(dst, src, count);
    }

    public static unsafe IntPtr SetUnmanagedMemory(IntPtr dst, int filler, int count)
    {
      SystemTools.SetUnmanagedMemory((byte*) dst.ToPointer(), filler, count);
      return dst;
    }

    public static unsafe byte* SetUnmanagedMemory(byte* dst, int filler, int count)
    {
      return SystemTools.memset(dst, filler, count);
    }

    [DllImport("ntdll.dll", CallingConvention = CallingConvention.Cdecl)]
    private static extern unsafe byte* memcpy(byte* dst, byte* src, int count);

    [DllImport("ntdll.dll", CallingConvention = CallingConvention.Cdecl)]
    private static extern unsafe byte* memset(byte* dst, int filler, int count);
  }
}
