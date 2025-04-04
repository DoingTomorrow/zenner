// Decompiled with JetBrains decompiler
// Type: NLog.Internal.Win32FileNativeMethods
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using Microsoft.Win32.SafeHandles;
using NLog.Targets;
using System;
using System.Runtime.InteropServices;

#nullable disable
namespace NLog.Internal
{
  internal static class Win32FileNativeMethods
  {
    public const int FILE_SHARE_READ = 1;
    public const int FILE_SHARE_WRITE = 2;
    public const int FILE_SHARE_DELETE = 4;

    [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    public static extern SafeFileHandle CreateFile(
      string lpFileName,
      Win32FileNativeMethods.FileAccess dwDesiredAccess,
      int dwShareMode,
      IntPtr lpSecurityAttributes,
      Win32FileNativeMethods.CreationDisposition dwCreationDisposition,
      Win32FileAttributes dwFlagsAndAttributes,
      IntPtr hTemplateFile);

    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool GetFileInformationByHandle(
      IntPtr hFile,
      out Win32FileNativeMethods.BY_HANDLE_FILE_INFORMATION lpFileInformation);

    [Flags]
    public enum FileAccess : uint
    {
      GenericRead = 2147483648, // 0x80000000
      GenericWrite = 1073741824, // 0x40000000
      GenericExecute = 536870912, // 0x20000000
      GenericAll = 268435456, // 0x10000000
    }

    public enum CreationDisposition : uint
    {
      New = 1,
      CreateAlways = 2,
      OpenExisting = 3,
      OpenAlways = 4,
      TruncateExisting = 5,
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct BY_HANDLE_FILE_INFORMATION
    {
      public uint dwFileAttributes;
      public long ftCreationTime;
      public long ftLastAccessTime;
      public long ftLastWriteTime;
      public uint dwVolumeSerialNumber;
      public uint nFileSizeHigh;
      public uint nFileSizeLow;
      public uint nNumberOfLinks;
      public uint nFileIndexHigh;
      public uint nFileIndexLow;
    }
  }
}
