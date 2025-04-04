// Decompiled with JetBrains decompiler
// Type: Ninject.ModuleLoadExtensions
// Assembly: Ninject, Version=3.0.0.0, Culture=neutral, PublicKeyToken=c7192dc5380945e7
// MVID: C76D661E-417A-4EBA-9151-4717B8101D58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Ninject.dll

using Ninject.Infrastructure;
using Ninject.Modules;
using System.Collections.Generic;
using System.Reflection;

#nullable disable
namespace Ninject
{
  public static class ModuleLoadExtensions
  {
    public static void Load<TModule>(this IKernel kernel) where TModule : INinjectModule, new()
    {
      Ensure.ArgumentNotNull((object) kernel, nameof (kernel));
      kernel.Load((INinjectModule) new TModule());
    }

    public static void Load(this IKernel kernel, params INinjectModule[] modules)
    {
      kernel.Load((IEnumerable<INinjectModule>) modules);
    }

    public static void Load(this IKernel kernel, params string[] filePatterns)
    {
      kernel.Load((IEnumerable<string>) filePatterns);
    }

    public static void Load(this IKernel kernel, params Assembly[] assemblies)
    {
      kernel.Load((IEnumerable<Assembly>) assemblies);
    }
  }
}
