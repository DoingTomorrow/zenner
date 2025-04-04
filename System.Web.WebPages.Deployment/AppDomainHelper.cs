// Decompiled with JetBrains decompiler
// Type: System.Web.WebPages.Deployment.AppDomainHelper
// Assembly: System.Web.WebPages.Deployment, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 9BAA0F73-1735-489C-B5F8-24E366CE85FF
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.WebPages.Deployment.dll

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

#nullable disable
namespace System.Web.WebPages.Deployment
{
  internal static class AppDomainHelper
  {
    public static IDictionary<string, IEnumerable<string>> GetBinAssemblyReferences(
      string appPath,
      string configPath)
    {
      string path = Path.Combine(appPath, "bin");
      if (!Directory.Exists(path))
        return (IDictionary<string, IEnumerable<string>>) null;
      AppDomain domain = (AppDomain) null;
      try
      {
        domain = AppDomain.CreateDomain(typeof (AppDomainHelper).Namespace, AppDomain.CurrentDomain.Evidence, new AppDomainSetup()
        {
          ApplicationBase = appPath,
          ConfigurationFile = configPath,
          PrivateBinPath = path
        });
        Type type = typeof (AppDomainHelper.RemoteAssemblyLoader);
        AppDomainHelper.RemoteAssemblyLoader instance = (AppDomainHelper.RemoteAssemblyLoader) domain.CreateInstanceAndUnwrap(type.Assembly.FullName, type.FullName);
        return (IDictionary<string, IEnumerable<string>>) Directory.EnumerateFiles(path, "*.dll").ToDictionary<string, string, IEnumerable<string>>((Func<string, string>) (assemblyPath => assemblyPath), (Func<string, IEnumerable<string>>) (assemblyPath => instance.GetReferences(assemblyPath)));
      }
      finally
      {
        if (domain != null)
          AppDomain.Unload(domain);
      }
    }

    private sealed class RemoteAssemblyLoader : MarshalByRefObject
    {
      public IEnumerable<string> GetReferences(string assemblyPath)
      {
        Assembly assembly = Assembly.LoadFrom(assemblyPath);
        return (IEnumerable<string>) ((IEnumerable<AssemblyName>) assembly.GetReferencedAssemblies()).Select<AssemblyName, string>((Func<AssemblyName, string>) (asmName => Assembly.Load(asmName.FullName).FullName)).Concat<string>((IEnumerable<string>) new string[1]
        {
          assembly.FullName
        }).ToArray<string>();
      }
    }
  }
}
