// Decompiled with JetBrains decompiler
// Type: Standard.IObjectWithAppUserModelId
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using System.Runtime.InteropServices;

#nullable disable
namespace Standard
{
  [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
  [Guid("36db0196-9665-46d1-9ba7-d3709eecf9ed")]
  [ComImport]
  internal interface IObjectWithAppUserModelId
  {
    void SetAppID([MarshalAs(UnmanagedType.LPWStr)] string pszAppID);

    [return: MarshalAs(UnmanagedType.LPWStr)]
    string GetAppID();
  }
}
