// Decompiled with JetBrains decompiler
// Type: Ninject.Syntax.IBindingOnSyntax`1
// Assembly: Ninject, Version=3.0.0.0, Culture=neutral, PublicKeyToken=c7192dc5380945e7
// MVID: C76D661E-417A-4EBA-9151-4717B8101D58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Ninject.dll

using Ninject.Activation;
using Ninject.Infrastructure;
using System;

#nullable disable
namespace Ninject.Syntax
{
  public interface IBindingOnSyntax<T> : 
    IBindingSyntax,
    IHaveBindingConfiguration,
    IHaveKernel,
    IFluentSyntax
  {
    IBindingOnSyntax<T> OnActivation(Action<T> action);

    IBindingOnSyntax<T> OnActivation<TImplementation>(Action<TImplementation> action);

    IBindingOnSyntax<T> OnActivation(Action<IContext, T> action);

    IBindingOnSyntax<T> OnActivation<TImplementation>(Action<IContext, TImplementation> action);

    IBindingOnSyntax<T> OnDeactivation(Action<T> action);

    IBindingOnSyntax<T> OnDeactivation<TImplementation>(Action<TImplementation> action);

    IBindingOnSyntax<T> OnDeactivation(Action<IContext, T> action);

    IBindingOnSyntax<T> OnDeactivation<TImplementation>(Action<IContext, TImplementation> action);
  }
}
