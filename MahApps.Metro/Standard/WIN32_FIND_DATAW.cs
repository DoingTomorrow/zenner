// Decompiled with JetBrains decompiler
// Type: Standard.WIN32_FIND_DATAW
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using System.IO;
using System.Runtime.InteropServices;

#nullable disable
namespace Standard
{
  [BestFitMapping(false)]
  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
  internal class WIN32_FIND_DATAW
  {
    public FileAttributes dwFileAttributes;
    public System.Runtime.InteropServices.ComTypes.FILETIME ftCreationTime;
    public System.Runtime.InteropServices.ComTypes.FILETIME ftLastAccessTime;
    public System.Runtime.InteropServices.ComTypes.FILETIME ftLastWriteTime;
    public int nFileSizeHigh;
    public int nFileSizeLow;
    public int dwReserved0;
    public int dwReserved1;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
    public string cFileName;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 14)]
    public string cAlternateFileName;
  }
}
