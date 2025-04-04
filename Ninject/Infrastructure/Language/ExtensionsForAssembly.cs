// Decompiled with JetBrains decompiler
// Type: Ninject.Infrastructure.Language.ExtensionsForAssembly
// Assembly: Ninject, Version=3.0.0.0, Culture=neutral, PublicKeyToken=c7192dc5380945e7
// MVID: C76D661E-417A-4EBA-9151-4717B8101D58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Ninject.dll

using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

#nullable disable
namespace Ninject.Infrastructure.Language
{
  internal static class ExtensionsForAssembly
  {
    public static bool HasNinjectModules(this Assembly assembly)
    {
      return ((IEnumerable<Type>) assembly.GetExportedTypes()).Any<Type>(new Func<Type, bool>(ExtensionsForAssembly.IsLoadableModule));
    }

    public static IEnumerable<INinjectModule> GetNinjectModules(this Assembly assembly)
    {
      return ((IEnumerable<Type>) assembly.GetExportedTypes()).Where<Type>(new Func<Type, bool>(ExtensionsForAssembly.IsLoadableModule)).Select<Type, INinjectModule>((Func<Type, INinjectModule>) (type => Activator.CreateInstance(type) as INinjectModule));
    }

    private static bool IsLoadableModule(Type type)
    {
      return typeof (INinjectModule).IsAssignableFrom(type) && !type.IsAbstract && !type.IsInterface && type.GetConstructor(Type.EmptyTypes) != (ConstructorInfo) null;
    }
  }
}
