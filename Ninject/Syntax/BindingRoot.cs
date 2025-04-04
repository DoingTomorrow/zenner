// Decompiled with JetBrains decompiler
// Type: Ninject.Syntax.BindingRoot
// Assembly: Ninject, Version=3.0.0.0, Culture=neutral, PublicKeyToken=c7192dc5380945e7
// MVID: C76D661E-417A-4EBA-9151-4717B8101D58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Ninject.dll

using Ninject.Infrastructure;
using Ninject.Infrastructure.Disposal;
using Ninject.Infrastructure.Introspection;
using Ninject.Planning.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Ninject.Syntax
{
  public abstract class BindingRoot : DisposableObject, IBindingRoot, IFluentSyntax
  {
    protected abstract IKernel KernelInstance { get; }

    public IBindingToSyntax<T> Bind<T>()
    {
      Type type = typeof (T);
      Binding binding = new Binding(type);
      this.AddBinding((IBinding) binding);
      return (IBindingToSyntax<T>) new BindingBuilder<T>((IBinding) binding, this.KernelInstance, type.Format());
    }

    public IBindingToSyntax<T1, T2> Bind<T1, T2>()
    {
      Binding binding = new Binding(typeof (T1));
      this.AddBinding((IBinding) binding);
      this.AddBinding((IBinding) new Binding(typeof (T2), binding.BindingConfiguration));
      string[] strArray = new string[2]
      {
        typeof (T1).Format(),
        typeof (T2).Format()
      };
      return (IBindingToSyntax<T1, T2>) new BindingBuilder<T1, T2>(binding.BindingConfiguration, this.KernelInstance, string.Join(", ", strArray));
    }

    public IBindingToSyntax<T1, T2, T3> Bind<T1, T2, T3>()
    {
      Binding binding = new Binding(typeof (T1));
      this.AddBinding((IBinding) binding);
      this.AddBinding((IBinding) new Binding(typeof (T2), binding.BindingConfiguration));
      this.AddBinding((IBinding) new Binding(typeof (T3), binding.BindingConfiguration));
      string[] strArray = new string[3]
      {
        typeof (T1).Format(),
        typeof (T2).Format(),
        typeof (T3).Format()
      };
      return (IBindingToSyntax<T1, T2, T3>) new BindingBuilder<T1, T2, T3>(binding.BindingConfiguration, this.KernelInstance, string.Join(", ", strArray));
    }

    public IBindingToSyntax<T1, T2, T3, T4> Bind<T1, T2, T3, T4>()
    {
      Binding binding = new Binding(typeof (T1));
      this.AddBinding((IBinding) binding);
      this.AddBinding((IBinding) new Binding(typeof (T2), binding.BindingConfiguration));
      this.AddBinding((IBinding) new Binding(typeof (T3), binding.BindingConfiguration));
      this.AddBinding((IBinding) new Binding(typeof (T4), binding.BindingConfiguration));
      string[] strArray = new string[4]
      {
        typeof (T1).Format(),
        typeof (T2).Format(),
        typeof (T3).Format(),
        typeof (T4).Format()
      };
      return (IBindingToSyntax<T1, T2, T3, T4>) new BindingBuilder<T1, T2, T3, T4>(binding.BindingConfiguration, this.KernelInstance, string.Join(", ", strArray));
    }

    public IBindingToSyntax<object> Bind(params Type[] services)
    {
      Ensure.ArgumentNotNull((object) services, "service");
      Binding binding = services.Length != 0 ? new Binding(services[0]) : throw new ArgumentException("The services must contain at least one type", nameof (services));
      this.AddBinding((IBinding) binding);
      foreach (Type service in ((IEnumerable<Type>) services).Skip<Type>(1))
        this.AddBinding((IBinding) new Binding(service, binding.BindingConfiguration));
      return (IBindingToSyntax<object>) new BindingBuilder<object>((IBinding) binding, this.KernelInstance, string.Join(", ", ((IEnumerable<Type>) services).Select<Type, string>((Func<Type, string>) (service => service.Format())).ToArray<string>()));
    }

    public void Unbind<T>() => this.Unbind(typeof (T));

    public abstract void Unbind(Type service);

    public IBindingToSyntax<T1> Rebind<T1>()
    {
      this.Unbind<T1>();
      return this.Bind<T1>();
    }

    public IBindingToSyntax<T1, T2> Rebind<T1, T2>()
    {
      this.Unbind<T1>();
      this.Unbind<T2>();
      return this.Bind<T1, T2>();
    }

    public IBindingToSyntax<T1, T2, T3> Rebind<T1, T2, T3>()
    {
      this.Unbind<T1>();
      this.Unbind<T2>();
      this.Unbind<T3>();
      return this.Bind<T1, T2, T3>();
    }

    public IBindingToSyntax<T1, T2, T3, T4> Rebind<T1, T2, T3, T4>()
    {
      this.Unbind<T1>();
      this.Unbind<T2>();
      this.Unbind<T3>();
      this.Unbind<T4>();
      return this.Bind<T1, T2, T3, T4>();
    }

    public IBindingToSyntax<object> Rebind(params Type[] services)
    {
      foreach (Type service in services)
        this.Unbind(service);
      return this.Bind(services);
    }

    public abstract void AddBinding(IBinding binding);

    public abstract void RemoveBinding(IBinding binding);

    Type IFluentSyntax.GetType() => this.GetType();
  }
}
