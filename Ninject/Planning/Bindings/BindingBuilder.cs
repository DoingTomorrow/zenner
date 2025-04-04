// Decompiled with JetBrains decompiler
// Type: Ninject.Planning.Bindings.BindingBuilder
// Assembly: Ninject, Version=3.0.0.0, Culture=neutral, PublicKeyToken=c7192dc5380945e7
// MVID: C76D661E-417A-4EBA-9151-4717B8101D58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Ninject.dll

using Ninject.Activation;
using Ninject.Activation.Providers;
using Ninject.Infrastructure;
using Ninject.Parameters;
using Ninject.Syntax;
using System;
using System.Linq.Expressions;
using System.Reflection;

#nullable disable
namespace Ninject.Planning.Bindings
{
  public class BindingBuilder
  {
    public BindingBuilder(
      IBindingConfiguration bindingConfiguration,
      IKernel kernel,
      string serviceNames)
    {
      Ensure.ArgumentNotNull((object) bindingConfiguration, "binding");
      Ensure.ArgumentNotNull((object) kernel, nameof (kernel));
      this.BindingConfiguration = bindingConfiguration;
      this.Kernel = kernel;
      this.ServiceNames = serviceNames;
      this.BindingConfiguration.ScopeCallback = kernel.Settings.DefaultScopeCallback;
    }

    public IBindingConfiguration BindingConfiguration { get; private set; }

    public IKernel Kernel { get; private set; }

    protected string ServiceNames { get; private set; }

    protected IBindingWhenInNamedWithOrOnSyntax<TImplementation> InternalTo<TImplementation>()
    {
      return this.InternalTo<TImplementation>(typeof (TImplementation));
    }

    protected IBindingWhenInNamedWithOrOnSyntax<T> InternalTo<T>(Type implementation)
    {
      this.BindingConfiguration.ProviderCallback = StandardProvider.GetCreationCallback(implementation);
      this.BindingConfiguration.Target = BindingTarget.Type;
      return (IBindingWhenInNamedWithOrOnSyntax<T>) new BindingConfigurationBuilder<T>(this.BindingConfiguration, this.ServiceNames, this.Kernel);
    }

    protected IBindingWhenInNamedWithOrOnSyntax<TImplementation> InternalToConfiguration<TImplementation>(
      TImplementation value)
    {
      this.BindingConfiguration.ProviderCallback = (Func<IContext, IProvider>) (ctx => (IProvider) new ConstantProvider<TImplementation>(value));
      this.BindingConfiguration.Target = BindingTarget.Constant;
      this.BindingConfiguration.ScopeCallback = StandardScopeCallbacks.Singleton;
      return (IBindingWhenInNamedWithOrOnSyntax<TImplementation>) new BindingConfigurationBuilder<TImplementation>(this.BindingConfiguration, this.ServiceNames, this.Kernel);
    }

    protected IBindingWhenInNamedWithOrOnSyntax<TImplementation> InternalToMethod<TImplementation>(
      Func<IContext, TImplementation> method)
    {
      this.BindingConfiguration.ProviderCallback = (Func<IContext, IProvider>) (ctx => (IProvider) new CallbackProvider<TImplementation>(method));
      this.BindingConfiguration.Target = BindingTarget.Method;
      return (IBindingWhenInNamedWithOrOnSyntax<TImplementation>) new BindingConfigurationBuilder<TImplementation>(this.BindingConfiguration, this.ServiceNames, this.Kernel);
    }

    protected IBindingWhenInNamedWithOrOnSyntax<TImplementation> InternalToProvider<TImplementation>(
      IProvider<TImplementation> provider)
    {
      this.BindingConfiguration.ProviderCallback = (Func<IContext, IProvider>) (ctx => (IProvider) provider);
      this.BindingConfiguration.Target = BindingTarget.Provider;
      return (IBindingWhenInNamedWithOrOnSyntax<TImplementation>) new BindingConfigurationBuilder<TImplementation>(this.BindingConfiguration, this.ServiceNames, this.Kernel);
    }

    protected IBindingWhenInNamedWithOrOnSyntax<TImplementation> ToProviderInternal<TProvider, TImplementation>() where TProvider : IProvider
    {
      this.BindingConfiguration.ProviderCallback = (Func<IContext, IProvider>) (ctx => (IProvider) ctx.Kernel.Get<TProvider>());
      this.BindingConfiguration.Target = BindingTarget.Provider;
      return (IBindingWhenInNamedWithOrOnSyntax<TImplementation>) new BindingConfigurationBuilder<TImplementation>(this.BindingConfiguration, this.ServiceNames, this.Kernel);
    }

    protected IBindingWhenInNamedWithOrOnSyntax<T> ToProviderInternal<T>(Type providerType)
    {
      this.BindingConfiguration.ProviderCallback = (Func<IContext, IProvider>) (ctx => ctx.Kernel.Get(providerType) as IProvider);
      this.BindingConfiguration.Target = BindingTarget.Provider;
      return (IBindingWhenInNamedWithOrOnSyntax<T>) new BindingConfigurationBuilder<T>(this.BindingConfiguration, this.ServiceNames, this.Kernel);
    }

    protected IBindingWhenInNamedWithOrOnSyntax<TImplementation> InternalToConstructor<TImplementation>(
      Expression<Func<IConstructorArgumentSyntax, TImplementation>> newExpression)
    {
      if (!(newExpression.Body is NewExpression body))
        throw new ArgumentException("The expression must be a constructor call.", nameof (newExpression));
      this.BindingConfiguration.ProviderCallback = StandardProvider.GetCreationCallback(body.Type, body.Constructor);
      this.BindingConfiguration.Target = BindingTarget.Type;
      this.AddConstructorArguments(body, newExpression.Parameters[0]);
      return (IBindingWhenInNamedWithOrOnSyntax<TImplementation>) new BindingConfigurationBuilder<TImplementation>(this.BindingConfiguration, this.ServiceNames, this.Kernel);
    }

    private void AddConstructorArguments(
      NewExpression ctorExpression,
      ParameterExpression constructorArgumentSyntaxParameterExpression)
    {
      ParameterInfo[] parameters = ctorExpression.Constructor.GetParameters();
      for (int index = 0; index < ctorExpression.Arguments.Count; ++index)
        this.AddConstructorArgument(ctorExpression.Arguments[index], parameters[index].Name, constructorArgumentSyntaxParameterExpression);
    }

    private void AddConstructorArgument(
      Expression argument,
      string argumentName,
      ParameterExpression constructorArgumentSyntaxParameterExpression)
    {
      if (argument is MethodCallExpression methodCallExpression && !(methodCallExpression.Method.GetGenericMethodDefinition().DeclaringType != typeof (IConstructorArgumentSyntax)))
        return;
      Delegate compiledExpression = Expression.Lambda(argument, constructorArgumentSyntaxParameterExpression).Compile();
      this.BindingConfiguration.Parameters.Add((IParameter) new ConstructorArgument(argumentName, (Func<IContext, object>) (ctx => compiledExpression.DynamicInvoke((object) new BindingBuilder.ConstructorArgumentSyntax(ctx)))));
    }

    private class ConstructorArgumentSyntax : IConstructorArgumentSyntax, IFluentSyntax
    {
      public ConstructorArgumentSyntax(IContext context) => this.Context = context;

      public IContext Context { get; private set; }

      public T1 Inject<T1>()
      {
        throw new InvalidOperationException("This method is for declaration that a parameter shall be injected only! Never call it directly.");
      }

      Type IFluentSyntax.GetType() => this.GetType();
    }
  }
}
