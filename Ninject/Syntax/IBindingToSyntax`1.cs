// Decompiled with JetBrains decompiler
// Type: Ninject.Syntax.IBindingToSyntax`1
// Assembly: Ninject, Version=3.0.0.0, Culture=neutral, PublicKeyToken=c7192dc5380945e7
// MVID: C76D661E-417A-4EBA-9151-4717B8101D58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Ninject.dll

using Ninject.Activation;
using Ninject.Infrastructure;
using System;
using System.Linq.Expressions;

#nullable disable
namespace Ninject.Syntax
{
  public interface IBindingToSyntax<T1> : 
    IBindingSyntax,
    IHaveBindingConfiguration,
    IHaveKernel,
    IFluentSyntax
  {
    IBindingWhenInNamedWithOrOnSyntax<T1> ToSelf();

    IBindingWhenInNamedWithOrOnSyntax<TImplementation> To<TImplementation>() where TImplementation : T1;

    IBindingWhenInNamedWithOrOnSyntax<T1> To(Type implementation);

    IBindingWhenInNamedWithOrOnSyntax<T1> ToProvider<TProvider>() where TProvider : IProvider;

    IBindingWhenInNamedWithOrOnSyntax<T1> ToProvider(Type providerType);

    IBindingWhenInNamedWithOrOnSyntax<TImplementation> ToProvider<TImplementation>(
      IProvider<TImplementation> provider)
      where TImplementation : T1;

    IBindingWhenInNamedWithOrOnSyntax<T1> ToMethod(Func<IContext, T1> method);

    IBindingWhenInNamedWithOrOnSyntax<TImplementation> ToMethod<TImplementation>(
      Func<IContext, TImplementation> method)
      where TImplementation : T1;

    IBindingWhenInNamedWithOrOnSyntax<TImplementation> ToConstant<TImplementation>(
      TImplementation value)
      where TImplementation : T1;

    IBindingWhenInNamedWithOrOnSyntax<TImplementation> ToConstructor<TImplementation>(
      Expression<Func<IConstructorArgumentSyntax, TImplementation>> newExpression)
      where TImplementation : T1;
  }
}
