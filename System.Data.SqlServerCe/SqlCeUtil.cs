// Decompiled with JetBrains decompiler
// Type: System.Data.SqlServerCe.SqlCeUtil
// Assembly: System.Data.SqlServerCe, Version=3.5.1.50, Culture=neutral, PublicKeyToken=89845dcd8080cc91
// MVID: 5CF67607-9835-4428-8475-9E80A4482327
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Data.SqlServerCe.dll

using Microsoft.Win32;
using System.IO;

#nullable disable
namespace System.Data.SqlServerCe
{
  internal static class SqlCeUtil
  {
    internal const string ProductName = "Microsoft SQL Server Compact";
    internal const string ProductRootRegKey = "Software\\Microsoft\\Microsoft SQL Server Compact Edition\\v3.5";
    internal const string Product35RootRegKey = "SOFTWARE\\Microsoft\\Microsoft SQL Server Compact Edition\\v3.5";
    internal const string ProductProxyPortsRegKey = "Software\\Microsoft\\Windows CE Services\\ProxyPorts";
    internal const string NetCFKey = "Software\\Microsoft\\.NetCompactFramework\\Installer\\Assemblies\\Global";
    internal const string ProviderName = "Sql Server Compact ADO.NET Data Provider";
    internal const string ProductServicingFile = "Microsoft.SqlServer.Compact.351.{0}.bc";
    internal const string ModuleStorageEngine = "sqlcese35.dll";
    internal const string ModuleStorageEngineSys = "sqlcese35.sys.dll";
    internal const string ModuleQueryProcessor = "sqlceqp35.dll";
    internal const string ModuleClientAgent = "sqlceca35.dll";
    internal const string ModuleOleDbProvider = "sqlceoledb35.dll";
    internal const string ModuleManagedExtentions = "sqlceme35.dll";
    internal const string ModuleDbCompact = "sqlcecompact35.dll";
    internal const string ModuleErrRes = "sqlceer35{0}.dll";
    internal const string ModuleServerAgent = "sqlcesa35.dll";
    internal const string ModuleReplicationProvider = "sqlcerp35.dll";
    internal const string ModuleTdsServer = "tdsserver35.exe";

    internal static RegistryKey RegistryOpenSubKey(
      RegistryKey rootKey,
      string subKeyName,
      bool writable)
    {
      return rootKey.OpenSubKey(subKeyName, writable);
    }

    internal static RegistryKey RegistryOpenProductRootKey(RegistryKey sysRootKey, bool writable)
    {
      return SqlCeUtil.RegistryOpenSubKey(sysRootKey, "Software\\Microsoft\\Microsoft SQL Server Compact Edition\\v3.5", writable);
    }

    internal static RegistryKey RegistryOpenProductRootSubKey(
      RegistryKey sysRootKey,
      string subKeyName,
      bool writable)
    {
      RegistryKey rootKey = SqlCeUtil.RegistryOpenSubKey(sysRootKey, "Software\\Microsoft\\Microsoft SQL Server Compact Edition\\v3.5", writable);
      RegistryKey registryKey = (RegistryKey) null;
      if (rootKey != null)
      {
        registryKey = SqlCeUtil.RegistryOpenSubKey(rootKey, subKeyName, writable);
        rootKey.Close();
      }
      return registryKey;
    }

    internal static RegistryKey RegistryCreateProductRootKey(RegistryKey sysRootKey)
    {
      return sysRootKey.CreateSubKey("Software\\Microsoft\\Microsoft SQL Server Compact Edition\\v3.5");
    }

    internal static RegistryKey RegistryCreateProductRootSubKey(
      RegistryKey sysRootKey,
      string subKeyName)
    {
      RegistryKey productRootKey = SqlCeUtil.RegistryCreateProductRootKey(sysRootKey);
      RegistryKey productRootSubKey = (RegistryKey) null;
      if (productRootKey != null)
      {
        productRootSubKey = productRootKey.CreateSubKey(subKeyName);
        productRootKey.Close();
      }
      return productRootSubKey;
    }

    internal static string GetModuleInstallPath(string moduleName)
    {
      string empty = string.Empty;
      RegistryKey registryKey = SqlCeUtil.RegistryOpenProductRootKey(Registry.LocalMachine, false);
      if (registryKey == null)
        return string.Empty;
      string path1 = (string) registryKey.GetValue("NativeDir");
      registryKey.Close();
      return string.IsNullOrEmpty(path1) ? string.Empty : Path.Combine(path1, moduleName);
    }
  }
}
