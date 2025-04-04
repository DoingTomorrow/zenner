// Decompiled with JetBrains decompiler
// Type: Ninject.Planning.Bindings.BindingConfigurationBuilder`1
// Assembly: Ninject, Version=3.0.0.0, Culture=neutral, PublicKeyToken=c7192dc5380945e7
// MVID: C76D661E-417A-4EBA-9151-4717B8101D58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Ninject.dll

using Ninject.Activation;
using Ninject.Infrastructure;
using Ninject.Infrastructure.Introspection;
using Ninject.Infrastructure.Language;
using Ninject.Parameters;
using Ninject.Planning.Targets;
using Ninject.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Ninject.Planning.Bindings
{
  public class BindingConfigurationBuilder<T> : 
    IBindingConfigurationSyntax<T>,
    IBindingWhenInNamedWithOrOnSyntax<T>,
    IBindingWhenSyntax<T>,
    IBindingInNamedWithOrOnSyntax<T>,
    IBindingInSyntax<T>,
    IBindingNamedWithOrOnSyntax<T>,
    IBindingNamedSyntax<T>,
    IBindingWithOrOnSyntax<T>,
    IBindingWithSyntax<T>,
    IBindingOnSyntax<T>,
    IBindingSyntax,
    IHaveBindingConfiguration,
    IHaveKernel,
    IFluentSyntax
  {
    private readonly string serviceNames;

    public IBindingConfiguration BindingConfiguration { get; private set; }

    public IKernel Kernel { get; private set; }

    public BindingConfigurationBuilder(
      IBindingConfiguration bindingConfiguration,
      string serviceNames,
      IKernel kernel)
    {
      Ensure.ArgumentNotNull((object) bindingConfiguration, nameof (bindingConfiguration));
      Ensure.ArgumentNotNull((object) kernel, nameof (kernel));
      this.BindingConfiguration = bindingConfiguration;
      this.Kernel = kernel;
      this.serviceNames = serviceNames;
    }

    public IBindingInNamedWithOrOnSyntax<T> When(Func<IRequest, bool> condition)
    {
      this.BindingConfiguration.Condition = condition;
      return (IBindingInNamedWithOrOnSyntax<T>) this;
    }

    public IBindingInNamedWithOrOnSyntax<T> WhenInjectedInto<TParent>()
    {
      return this.WhenInjectedInto(typeof (TParent));
    }

    public IBindingInNamedWithOrOnSyntax<T> WhenInjectedInto(Type parent)
    {
      this.BindingConfiguration.Condition = !parent.IsGenericTypeDefinition ? (Func<IRequest, bool>) (r => r.Target != null && parent.IsAssignableFrom(r.Target.Member.ReflectedType)) : (!parent.IsInterface ? (Func<IRequest, bool>) (r => r.Target != null && r.Target.Member.ReflectedType.GetAllBaseTypes().Any<Type>((Func<Type, bool>) (i => i.IsGenericType && i.GetGenericTypeDefinition() == parent))) : (Func<IRequest, bool>) (r => r.Target != null && ((IEnumerable<Type>) r.Target.Member.ReflectedType.GetInterfaces()).Any<Type>((Func<Type, bool>) (i => i.IsGenericType && i.GetGenericTypeDefinition() == parent))));
      return (IBindingInNamedWithOrOnSyntax<T>) this;
    }

    public IBindingInNamedWithOrOnSyntax<T> WhenInjectedExactlyInto<TParent>()
    {
      return this.WhenInjectedExactlyInto(typeof (TParent));
    }

    public IBindingInNamedWithOrOnSyntax<T> WhenInjectedExactlyInto(Type parent)
    {
      this.BindingConfiguration.Condition = !parent.IsGenericTypeDefinition ? (Func<IRequest, bool>) (r => r.Target != null && r.Target.Member.ReflectedType == parent) : (Func<IRequest, bool>) (r => r.Target != null && r.Target.Member.ReflectedType.IsGenericType && parent == r.Target.Member.ReflectedType.GetGenericTypeDefinition());
      return (IBindingInNamedWithOrOnSyntax<T>) this;
    }

    public IBindingInNamedWithOrOnSyntax<T> WhenClassHas<TAttribute>() where TAttribute : Attribute
    {
      return this.WhenClassHas(typeof (TAttribute));
    }

    public IBindingInNamedWithOrOnSyntax<T> WhenMemberHas<TAttribute>() where TAttribute : Attribute
    {
      return this.WhenMemberHas(typeof (TAttribute));
    }

    public IBindingInNamedWithOrOnSyntax<T> WhenTargetHas<TAttribute>() where TAttribute : Attribute
    {
      return this.WhenTargetHas(typeof (TAttribute));
    }

    public IBindingInNamedWithOrOnSyntax<T> WhenClassHas(Type attributeType)
    {
      if (!typeof (Attribute).IsAssignableFrom(attributeType))
        throw new InvalidOperationException(ExceptionFormatter.InvalidAttributeTypeUsedInBindingCondition(this.serviceNames, nameof (WhenClassHas), attributeType));
      this.BindingConfiguration.Condition = (Func<IRequest, bool>) (r => r.Target != null && ExtensionsForMemberInfo.HasAttribute(r.Target.Member.ReflectedType, attributeType));
      return (IBindingInNamedWithOrOnSyntax<T>) this;
    }

    public IBindingInNamedWithOrOnSyntax<T> WhenMemberHas(Type attributeType)
    {
      if (!typeof (Attribute).IsAssignableFrom(attributeType))
        throw new InvalidOperationException(ExceptionFormatter.InvalidAttributeTypeUsedInBindingCondition(this.serviceNames, nameof (WhenMemberHas), attributeType));
      this.BindingConfiguration.Condition = (Func<IRequest, bool>) (r => r.Target != null && ExtensionsForMemberInfo.HasAttribute(r.Target.Member, attributeType));
      return (IBindingInNamedWithOrOnSyntax<T>) this;
    }

    public IBindingInNamedWithOrOnSyntax<T> WhenTargetHas(Type attributeType)
    {
      if (!typeof (Attribute).IsAssignableFrom(attributeType))
        throw new InvalidOperationException(ExceptionFormatter.InvalidAttributeTypeUsedInBindingCondition(this.serviceNames, nameof (WhenTargetHas), attributeType));
      this.BindingConfiguration.Condition = (Func<IRequest, bool>) (r => r.Target != null && r.Target.HasAttribute(attributeType));
      return (IBindingInNamedWithOrOnSyntax<T>) this;
    }

    public IBindingInNamedWithOrOnSyntax<T> WhenParentNamed(string name)
    {
      string.Intern(name);
      this.BindingConfiguration.Condition = (Func<IRequest, bool>) (r => r.ParentContext != null && string.Equals(r.ParentContext.Binding.Metadata.Name, name, StringComparison.Ordinal));
      return (IBindingInNamedWithOrOnSyntax<T>) this;
    }

    public IBindingInNamedWithOrOnSyntax<T> WhenAnyAnchestorNamed(string name)
    {
      this.BindingConfiguration.Condition = (Func<IRequest, bool>) (r => BindingConfigurationBuilder<T>.IsAnyAnchestorNamed(r, name));
      return (IBindingInNamedWithOrOnSyntax<T>) this;
    }

    public IBindingWithOrOnSyntax<T> Named(string name)
    {
      string.Intern(name);
      this.BindingConfiguration.Metadata.Name = name;
      return (IBindingWithOrOnSyntax<T>) this;
    }

    public IBindingNamedWithOrOnSyntax<T> InSingletonScope()
    {
      this.BindingConfiguration.ScopeCallback = StandardScopeCallbacks.Singleton;
      return (IBindingNamedWithOrOnSyntax<T>) this;
    }

    public IBindingNamedWithOrOnSyntax<T> InTransientScope()
    {
      this.BindingConfiguration.ScopeCallback = StandardScopeCallbacks.Transient;
      return (IBindingNamedWithOrOnSyntax<T>) this;
    }

    public IBindingNamedWithOrOnSyntax<T> InThreadScope()
    {
      this.BindingConfiguration.ScopeCallback = StandardScopeCallbacks.Thread;
      return (IBindingNamedWithOrOnSyntax<T>) this;
    }

    public IBindingNamedWithOrOnSyntax<T> InScope(Func<IContext, object> scope)
    {
      this.BindingConfiguration.ScopeCallback = scope;
      return (IBindingNamedWithOrOnSyntax<T>) this;
    }

    public IBindingWithOrOnSyntax<T> WithConstructorArgument(string name, object value)
    {
      this.BindingConfiguration.Parameters.Add((IParameter) new ConstructorArgument(name, value));
      return (IBindingWithOrOnSyntax<T>) this;
    }

    public IBindingWithOrOnSyntax<T> WithConstructorArgument(
      string name,
      Func<IContext, object> callback)
    {
      this.BindingConfiguration.Parameters.Add((IParameter) new ConstructorArgument(name, callback));
      return (IBindingWithOrOnSyntax<T>) this;
    }

    public IBindingWithOrOnSyntax<T> WithConstructorArgument(
      string name,
      Func<IContext, ITarget, object> callback)
    {
      this.BindingConfiguration.Parameters.Add((IParameter) new ConstructorArgument(name, callback));
      return (IBindingWithOrOnSyntax<T>) this;
    }

    public IBindingWithOrOnSyntax<T> WithPropertyValue(string name, object value)
    {
      this.BindingConfiguration.Parameters.Add((IParameter) new PropertyValue(name, value));
      return (IBindingWithOrOnSyntax<T>) this;
    }

    public IBindingWithOrOnSyntax<T> WithPropertyValue(string name, Func<IContext, object> callback)
    {
      this.BindingConfiguration.Parameters.Add((IParameter) new PropertyValue(name, callback));
      return (IBindingWithOrOnSyntax<T>) this;
    }

    public IBindingWithOrOnSyntax<T> WithPropertyValue(
      string name,
      Func<IContext, ITarget, object> callback)
    {
      this.BindingConfiguration.Parameters.Add((IParameter) new PropertyValue(name, callback));
      return (IBindingWithOrOnSyntax<T>) this;
    }

    public IBindingWithOrOnSyntax<T> WithParameter(IParameter parameter)
    {
      this.BindingConfiguration.Parameters.Add(parameter);
      return (IBindingWithOrOnSyntax<T>) this;
    }

    public IBindingWithOrOnSyntax<T> WithMetadata(string key, object value)
    {
      this.BindingConfiguration.Metadata.Set(key, value);
      return (IBindingWithOrOnSyntax<T>) this;
    }

    public IBindingOnSyntax<T> OnActivation(Action<T> action) => this.OnActivation<T>(action);

    public IBindingOnSyntax<T> OnActivation<TImplementation>(Action<TImplementation> action)
    {
      this.BindingConfiguration.ActivationActions.Add((Action<IContext, object>) ((context, instance) => action((TImplementation) instance)));
      return (IBindingOnSyntax<T>) this;
    }

    public IBindingOnSyntax<T> OnActivation(Action<IContext, T> action)
    {
      return this.OnActivation<T>(action);
    }

    public IBindingOnSyntax<T> OnActivation<TImplementation>(
      Action<IContext, TImplementation> action)
    {
      this.BindingConfiguration.ActivationActions.Add((Action<IContext, object>) ((context, instance) => action(context, (TImplementation) instance)));
      return (IBindingOnSyntax<T>) this;
    }

    public IBindingOnSyntax<T> OnDeactivation(Action<T> action) => this.OnDeactivation<T>(action);

    public IBindingOnSyntax<T> OnDeactivation<TImplementation>(Action<TImplementation> action)
    {
      this.BindingConfiguration.DeactivationActions.Add((Action<IContext, object>) ((context, instance) => action((TImplementation) instance)));
      return (IBindingOnSyntax<T>) this;
    }

    public IBindingOnSyntax<T> OnDeactivation(Action<IContext, T> action)
    {
      return this.OnDeactivation<T>(action);
    }

    public IBindingOnSyntax<T> OnDeactivation<TImplementation>(
      Action<IContext, TImplementation> action)
    {
      this.BindingConfiguration.DeactivationActions.Add((Action<IContext, object>) ((context, instance) => action(context, (TImplementation) instance)));
      return (IBindingOnSyntax<T>) this;
    }

    private static bool IsAnyAnchestorNamed(IRequest request, string name)
    {
      IContext parentContext = request.ParentContext;
      if (parentContext == null)
        return false;
      return string.Equals(parentContext.Binding.Metadata.Name, name, StringComparison.Ordinal) || BindingConfigurationBuilder<T>.IsAnyAnchestorNamed(parentContext.Request, name);
    }

    Type IFluentSyntax.GetType() => this.GetType();
  }
}
