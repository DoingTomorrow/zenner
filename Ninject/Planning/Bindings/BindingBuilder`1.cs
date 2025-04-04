// Decompiled with JetBrains decompiler
// Type: Ninject.Planning.Bindings.BindingBuilder`1
// Assembly: Ninject, Version=3.0.0.0, Culture=neutral, PublicKeyToken=c7192dc5380945e7
// MVID: C76D661E-417A-4EBA-9151-4717B8101D58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Ninject.dll

using Ninject.Activation;
using Ninject.Activation.Providers;
using Ninject.Infrastructure;
using Ninject.Syntax;
using System;
using System.Linq.Expressions;

#nullable disable
namespace Ninject.Planning.Bindings
{
  public class BindingBuilder<T1> : 
    BindingBuilder,
    IBindingToSyntax<T1>,
    IBindingSyntax,
    IHaveBindingConfiguration,
    IHaveKernel,
    IFluentSyntax
  {
    public BindingBuilder(IBinding binding, IKernel kernel, string serviceNames)
      : base(binding.BindingConfiguration, kernel, serviceNames)
    {
      Ensure.ArgumentNotNull((object) binding, nameof (binding));
      Ensure.ArgumentNotNull((object) kernel, nameof (kernel));
      this.Binding = binding;
    }

    public IBinding Binding { get; private set; }

    public IBindingWhenInNamedWithOrOnSyntax<T1> ToSelf()
    {
      this.Binding.ProviderCallback = StandardProvider.GetCreationCallback(this.Binding.Service);
      this.Binding.Target = BindingTarget.Self;
      return (IBindingWhenInNamedWithOrOnSyntax<T1>) new BindingConfigurationBuilder<T1>(this.Binding.BindingConfiguration, this.ServiceNames, this.Kernel);
    }

    public IBindingWhenInNamedWithOrOnSyntax<TImplementation> To<TImplementation>() where TImplementation : T1
    {
      return this.InternalTo<TImplementation>();
    }

    public IBindingWhenInNamedWithOrOnSyntax<T1> To(Type implementation)
    {
      return this.InternalTo<T1>(implementation);
    }

    public IBindingWhenInNamedWithOrOnSyntax<TImplementation> ToConstructor<TImplementation>(
      Expression<Func<IConstructorArgumentSyntax, TImplementation>> newExpression)
      where TImplementation : T1
    {
      return this.InternalToConstructor<TImplementation>(newExpression);
    }

    public IBindingWhenInNamedWithOrOnSyntax<T1> ToProvider<TProvider>() where TProvider : IProvider
    {
      return this.ToProviderInternal<TProvider, T1>();
    }

    public IBindingWhenInNamedWithOrOnSyntax<T1> ToProvider(Type providerType)
    {
      return this.ToProviderInternal<T1>(providerType);
    }

    public IBindingWhenInNamedWithOrOnSyntax<TImplementation> ToProvider<TImplementation>(
      IProvider<TImplementation> provider)
      where TImplementation : T1
    {
      return this.InternalToProvider<TImplementation>(provider);
    }

    public IBindingWhenInNamedWithOrOnSyntax<T1> ToMethod(Func<IContext, T1> method)
    {
      return this.InternalToMethod<T1>(method);
    }

    public IBindingWhenInNamedWithOrOnSyntax<TImplementation> ToMethod<TImplementation>(
      Func<IContext, TImplementation> method)
      where TImplementation : T1
    {
      return this.InternalToMethod<TImplementation>(method);
    }

    public IBindingWhenInNamedWithOrOnSyntax<TImplementation> ToConstant<TImplementation>(
      TImplementation value)
      where TImplementation : T1
    {
      return this.InternalToConfiguration<TImplementation>(value);
    }

    Type IFluentSyntax.GetType() => this.GetType();
  }
}
