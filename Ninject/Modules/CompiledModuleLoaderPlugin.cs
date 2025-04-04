// Decompiled with JetBrains decompiler
// Type: Ninject.Modules.CompiledModuleLoaderPlugin
// Assembly: Ninject, Version=3.0.0.0, Culture=neutral, PublicKeyToken=c7192dc5380945e7
// MVID: C76D661E-417A-4EBA-9151-4717B8101D58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Ninject.dll

using Ninject.Components;
using Ninject.Infrastructure;
using Ninject.Infrastructure.Language;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

#nullable disable
namespace Ninject.Modules
{
  public class CompiledModuleLoaderPlugin : 
    NinjectComponent,
    IModuleLoaderPlugin,
    INinjectComponent,
    IDisposable
  {
    private readonly IAssemblyNameRetriever assemblyNameRetriever;
    private static readonly string[] Extensions = new string[1]
    {
      ".dll"
    };

    public CompiledModuleLoaderPlugin(IKernel kernel, IAssemblyNameRetriever assemblyNameRetriever)
    {
      Ensure.ArgumentNotNull((object) kernel, nameof (kernel));
      this.Kernel = kernel;
      this.assemblyNameRetriever = assemblyNameRetriever;
    }

    public IKernel Kernel { get; private set; }

    public IEnumerable<string> SupportedExtensions
    {
      get => (IEnumerable<string>) CompiledModuleLoaderPlugin.Extensions;
    }

    public void LoadModules(IEnumerable<string> filenames)
    {
      this.Kernel.Load(this.assemblyNameRetriever.GetAssemblyNames(filenames, (Predicate<Assembly>) (asm => asm.HasNinjectModules())).Select<AssemblyName, Assembly>((Func<AssemblyName, Assembly>) (asm => Assembly.Load(asm))));
    }
  }
}
