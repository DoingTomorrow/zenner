// Decompiled with JetBrains decompiler
// Type: Ninject.Modules.AssemblyNameRetriever
// Assembly: Ninject, Version=3.0.0.0, Culture=neutral, PublicKeyToken=c7192dc5380945e7
// MVID: C76D661E-417A-4EBA-9151-4717B8101D58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Ninject.dll

using Ninject.Components;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

#nullable disable
namespace Ninject.Modules
{
  public class AssemblyNameRetriever : 
    NinjectComponent,
    IAssemblyNameRetriever,
    INinjectComponent,
    IDisposable
  {
    public IEnumerable<AssemblyName> GetAssemblyNames(
      IEnumerable<string> filenames,
      Predicate<Assembly> filter)
    {
      Type type = typeof (AssemblyNameRetriever.AssemblyChecker);
      AppDomain temporaryAppDomain = AssemblyNameRetriever.CreateTemporaryAppDomain();
      try
      {
        return ((AssemblyNameRetriever.AssemblyChecker) temporaryAppDomain.CreateInstanceAndUnwrap(type.Assembly.FullName, type.FullName ?? string.Empty)).GetAssemblyNames((IEnumerable<string>) filenames.ToArray<string>(), filter);
      }
      finally
      {
        AppDomain.Unload(temporaryAppDomain);
      }
    }

    private static AppDomain CreateTemporaryAppDomain()
    {
      return AppDomain.CreateDomain("NinjectModuleLoader", AppDomain.CurrentDomain.Evidence, AppDomain.CurrentDomain.SetupInformation);
    }

    private class AssemblyChecker : MarshalByRefObject
    {
      public IEnumerable<AssemblyName> GetAssemblyNames(
        IEnumerable<string> filenames,
        Predicate<Assembly> filter)
      {
        List<AssemblyName> assemblyNames = new List<AssemblyName>();
        foreach (string filename in filenames)
        {
          Assembly assembly;
          if (File.Exists(filename))
          {
            try
            {
              assembly = Assembly.LoadFrom(filename);
            }
            catch (BadImageFormatException ex)
            {
              continue;
            }
          }
          else
          {
            try
            {
              assembly = Assembly.Load(filename);
            }
            catch (FileNotFoundException ex)
            {
              continue;
            }
          }
          if (filter(assembly))
            assemblyNames.Add(assembly.GetName(false));
        }
        return (IEnumerable<AssemblyName>) assemblyNames;
      }
    }
  }
}
