// Decompiled with JetBrains decompiler
// Type: Ninject.Modules.ModuleLoader
// Assembly: Ninject, Version=3.0.0.0, Culture=neutral, PublicKeyToken=c7192dc5380945e7
// MVID: C76D661E-417A-4EBA-9151-4717B8101D58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Ninject.dll

using Ninject.Components;
using Ninject.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

#nullable disable
namespace Ninject.Modules
{
  public class ModuleLoader : NinjectComponent, IModuleLoader, INinjectComponent, IDisposable
  {
    public IKernel Kernel { get; private set; }

    public ModuleLoader(IKernel kernel)
    {
      Ensure.ArgumentNotNull((object) kernel, nameof (kernel));
      this.Kernel = kernel;
    }

    public void LoadModules(IEnumerable<string> patterns)
    {
      IEnumerable<IModuleLoaderPlugin> all = this.Kernel.Components.GetAll<IModuleLoaderPlugin>();
      foreach (IGrouping<string, string> filenames in patterns.SelectMany<string, string>((Func<string, IEnumerable<string>>) (pattern => ModuleLoader.GetFilesMatchingPattern(pattern))).GroupBy<string, string>((Func<string, string>) (filename => Path.GetExtension(filename).ToLowerInvariant())))
      {
        string extension = filenames.Key;
        all.Where<IModuleLoaderPlugin>((Func<IModuleLoaderPlugin, bool>) (p => p.SupportedExtensions.Contains<string>(extension))).FirstOrDefault<IModuleLoaderPlugin>()?.LoadModules((IEnumerable<string>) filenames);
      }
    }

    private static IEnumerable<string> GetFilesMatchingPattern(string pattern)
    {
      return ModuleLoader.NormalizePaths(Path.GetDirectoryName(pattern)).SelectMany<string, string>((Func<string, IEnumerable<string>>) (path => (IEnumerable<string>) Directory.GetFiles(path, Path.GetFileName(pattern))));
    }

    private static IEnumerable<string> NormalizePaths(string path)
    {
      if (!Path.IsPathRooted(path))
        return ModuleLoader.GetBaseDirectories().Select<string, string>((Func<string, string>) (baseDirectory => Path.Combine(baseDirectory, path)));
      return (IEnumerable<string>) new string[1]
      {
        Path.GetFullPath(path)
      };
    }

    private static IEnumerable<string> GetBaseDirectories()
    {
      string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
      string relativeSearchPath = AppDomain.CurrentDomain.RelativeSearchPath;
      return !string.IsNullOrEmpty(relativeSearchPath) ? ((IEnumerable<string>) relativeSearchPath.Split(new char[1]
      {
        Path.PathSeparator
      }, StringSplitOptions.RemoveEmptyEntries)).Select<string, string>((Func<string, string>) (path => Path.Combine(baseDirectory, path))) : (IEnumerable<string>) new string[1]
      {
        baseDirectory
      };
    }
  }
}
