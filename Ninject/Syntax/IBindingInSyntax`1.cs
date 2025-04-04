// Decompiled with JetBrains decompiler
// Type: Ninject.Syntax.IBindingInSyntax`1
// Assembly: Ninject, Version=3.0.0.0, Culture=neutral, PublicKeyToken=c7192dc5380945e7
// MVID: C76D661E-417A-4EBA-9151-4717B8101D58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Ninject.dll

using Ninject.Activation;
using Ninject.Infrastructure;
using System;

#nullable disable
namespace Ninject.Syntax
{
  public interface IBindingInSyntax<T> : 
    IBindingSyntax,
    IHaveBindingConfiguration,
    IHaveKernel,
    IFluentSyntax
  {
    IBindingNamedWithOrOnSyntax<T> InSingletonScope();

    IBindingNamedWithOrOnSyntax<T> InTransientScope();

    IBindingNamedWithOrOnSyntax<T> InThreadScope();

    IBindingNamedWithOrOnSyntax<T> InScope(Func<IContext, object> scope);
  }
}
