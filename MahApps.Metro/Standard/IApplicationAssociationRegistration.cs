// Decompiled with JetBrains decompiler
// Type: Standard.IApplicationAssociationRegistration
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using System.Runtime.InteropServices;

#nullable disable
namespace Standard
{
  [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
  [Guid("4e530b0a-e611-4c77-a3ac-9031d022281b")]
  [ComImport]
  internal interface IApplicationAssociationRegistration
  {
    [return: MarshalAs(UnmanagedType.LPWStr)]
    string QueryCurrentDefault([MarshalAs(UnmanagedType.LPWStr)] string pszQuery, AT atQueryType, AL alQueryLevel);

    [return: MarshalAs(UnmanagedType.Bool)]
    bool QueryAppIsDefault(
      [MarshalAs(UnmanagedType.LPWStr)] string pszQuery,
      AT atQueryType,
      AL alQueryLevel,
      [MarshalAs(UnmanagedType.LPWStr)] string pszAppRegistryName);

    [return: MarshalAs(UnmanagedType.Bool)]
    bool QueryAppIsDefaultAll(AL alQueryLevel, [MarshalAs(UnmanagedType.LPWStr)] string pszAppRegistryName);

    void SetAppAsDefault([MarshalAs(UnmanagedType.LPWStr)] string pszAppRegistryName, [MarshalAs(UnmanagedType.LPWStr)] string pszSet, AT atSetType);

    void SetAppAsDefaultAll([MarshalAs(UnmanagedType.LPWStr)] string pszAppRegistryName);

    void ClearUserAssociations();
  }
}
