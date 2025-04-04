// Decompiled with JetBrains decompiler
// Type: Ninject.Modules.IAssemblyNameRetriever
// Assembly: Ninject, Version=3.0.0.0, Culture=neutral, PublicKeyToken=c7192dc5380945e7
// MVID: C76D661E-417A-4EBA-9151-4717B8101D58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Ninject.dll

using Ninject.Components;
using System;
using System.Collections.Generic;
using System.Reflection;

#nullable disable
namespace Ninject.Modules
{
  public interface IAssemblyNameRetriever : INinjectComponent, IDisposable
  {
    IEnumerable<AssemblyName> GetAssemblyNames(
      IEnumerable<string> filenames,
      Predicate<Assembly> filter);
  }
}
