// Decompiled with JetBrains decompiler
// Type: Ninject.Planning.Bindings.BindingBuilder`2
// Assembly: Ninject, Version=3.0.0.0, Culture=neutral, PublicKeyToken=c7192dc5380945e7
// MVID: C76D661E-417A-4EBA-9151-4717B8101D58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Ninject.dll

using Ninject.Activation;
using Ninject.Infrastructure;
using Ninject.Syntax;
using System;
using System.Linq.Expressions;

#nullable disable
namespace Ninject.Planning.Bindings
{
  public class BindingBuilder<T1, T2>(
    IBindingConfiguration bindingConfigurationConfiguration,
    IKernel kernel,
    string serviceNames) : 
    BindingBuilder(bindingConfigurationConfiguration, kernel, serviceNames),
    IBindingToSyntax<T1, T2>,
    IBindingSyntax,
    IHaveBindingConfiguration,
    IHaveKernel,
    IFluentSyntax
  {
    public IBindingWhenInNamedWithOrOnSyntax<TImplementation> To<TImplementation>() where TImplementation : T1, T2
    {
      return this.InternalTo<TImplementation>();
    }

    public IBindingWhenInNamedWithOrOnSyntax<object> To(Type implementation)
    {
      return this.InternalTo<object>(implementation);
    }

    public IBindingWhenInNamedWithOrOnSyntax<TImplementation> ToConstructor<TImplementation>(
      Expression<Func<IConstructorArgumentSyntax, TImplementation>> newExpression)
      where TImplementation : T1, T2
    {
      return this.InternalToConstructor<TImplementation>(newExpression);
    }

    public IBindingWhenInNamedWithOrOnSyntax<object> ToProvider<TProvider>() where TProvider : IProvider
    {
      return this.ToProviderInternal<TProvider, object>();
    }

    public IBindingWhenInNamedWithOrOnSyntax<TImplementation> ToProvider<TProvider, TImplementation>()
      where TProvider : IProvider<TImplementation>
      where TImplementation : T1, T2
    {
      return this.ToProviderInternal<TProvider, TImplementation>();
    }

    public IBindingWhenInNamedWithOrOnSyntax<object> ToProvider(Type providerType)
    {
      return this.ToProviderInternal<object>(providerType);
    }

    public IBindingWhenInNamedWithOrOnSyntax<TImplementation> ToProvider<TImplementation>(
      IProvider<TImplementation> provider)
      where TImplementation : T1, T2
    {
      return this.InternalToProvider<TImplementation>(provider);
    }

    public IBindingWhenInNamedWithOrOnSyntax<TImplementation> ToMethod<TImplementation>(
      Func<IContext, TImplementation> method)
      where TImplementation : T1, T2
    {
      return this.InternalToMethod<TImplementation>(method);
    }

    public IBindingWhenInNamedWithOrOnSyntax<TImplementation> ToConstant<TImplementation>(
      TImplementation value)
      where TImplementation : T1, T2
    {
      return this.InternalToConfiguration<TImplementation>(value);
    }

    Type IFluentSyntax.GetType() => this.GetType();
  }
}
