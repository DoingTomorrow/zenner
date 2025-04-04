// Decompiled with JetBrains decompiler
// Type: Ninject.IKernel
// Assembly: Ninject, Version=3.0.0.0, Culture=neutral, PublicKeyToken=c7192dc5380945e7
// MVID: C76D661E-417A-4EBA-9151-4717B8101D58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Ninject.dll

using Ninject.Activation.Blocks;
using Ninject.Components;
using Ninject.Infrastructure.Disposal;
using Ninject.Modules;
using Ninject.Parameters;
using Ninject.Planning.Bindings;
using Ninject.Syntax;
using System;
using System.Collections.Generic;
using System.Reflection;

#nullable disable
namespace Ninject
{
  public interface IKernel : 
    IBindingRoot,
    IResolutionRoot,
    IFluentSyntax,
    IServiceProvider,
    IDisposableObject,
    IDisposable
  {
    INinjectSettings Settings { get; }

    IComponentContainer Components { get; }

    IEnumerable<INinjectModule> GetModules();

    bool HasModule(string name);

    void Load(IEnumerable<INinjectModule> m);

    void Load(IEnumerable<string> filePatterns);

    void Load(IEnumerable<Assembly> assemblies);

    void Unload(string name);

    void Inject(object instance, params IParameter[] parameters);

    bool Release(object instance);

    IEnumerable<IBinding> GetBindings(Type service);

    IActivationBlock BeginBlock();
  }
}
