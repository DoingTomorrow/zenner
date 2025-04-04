// Decompiled with JetBrains decompiler
// Type: Ninject.Components.ComponentContainer
// Assembly: Ninject, Version=3.0.0.0, Culture=neutral, PublicKeyToken=c7192dc5380945e7
// MVID: C76D661E-417A-4EBA-9151-4717B8101D58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Ninject.dll

using Ninject.Infrastructure;
using Ninject.Infrastructure.Disposal;
using Ninject.Infrastructure.Introspection;
using Ninject.Infrastructure.Language;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

#nullable disable
namespace Ninject.Components
{
  public class ComponentContainer : DisposableObject, IComponentContainer, IDisposable
  {
    private readonly Multimap<Type, Type> _mappings = new Multimap<Type, Type>();
    private readonly Dictionary<Type, INinjectComponent> _instances = new Dictionary<Type, INinjectComponent>();
    private readonly HashSet<KeyValuePair<Type, Type>> transients = new HashSet<KeyValuePair<Type, Type>>();

    public IKernel Kernel { get; set; }

    public override void Dispose(bool disposing)
    {
      if (disposing && !this.IsDisposed)
      {
        foreach (IDisposable disposable in this._instances.Values)
          disposable.Dispose();
        this._mappings.Clear();
        this._instances.Clear();
      }
      base.Dispose(disposing);
    }

    public void Add<TComponent, TImplementation>()
      where TComponent : INinjectComponent
      where TImplementation : TComponent, INinjectComponent
    {
      this._mappings.Add(typeof (TComponent), typeof (TImplementation));
    }

    public void AddTransient<TComponent, TImplementation>()
      where TComponent : INinjectComponent
      where TImplementation : TComponent, INinjectComponent
    {
      this.Add<TComponent, TImplementation>();
      this.transients.Add(new KeyValuePair<Type, Type>(typeof (TComponent), typeof (TImplementation)));
    }

    public void RemoveAll<T>() where T : INinjectComponent => this.RemoveAll(typeof (T));

    public void RemoveAll(Type component)
    {
      Ensure.ArgumentNotNull((object) component, nameof (component));
      foreach (Type key in (IEnumerable<Type>) this._mappings[component])
      {
        if (this._instances.ContainsKey(key))
          this._instances[key].Dispose();
        this._instances.Remove(key);
      }
      this._mappings.RemoveAll(component);
    }

    public T Get<T>() where T : INinjectComponent => (T) this.Get(typeof (T));

    public IEnumerable<T> GetAll<T>() where T : INinjectComponent
    {
      return this.GetAll(typeof (T)).Cast<T>();
    }

    public object Get(Type component)
    {
      Ensure.ArgumentNotNull((object) component, nameof (component));
      if (component == typeof (IKernel))
        return (object) this.Kernel;
      if (component.IsGenericType)
      {
        Type genericTypeDefinition = component.GetGenericTypeDefinition();
        Type genericArgument = component.GetGenericArguments()[0];
        if (genericTypeDefinition.IsInterface && typeof (IEnumerable<>).IsAssignableFrom(genericTypeDefinition))
          return (object) this.GetAll(genericArgument).CastSlow(genericArgument);
      }
      Type implementation = this._mappings[component].FirstOrDefault<Type>();
      return !(implementation == (Type) null) ? this.ResolveInstance(component, implementation) : throw new InvalidOperationException(ExceptionFormatter.NoSuchComponentRegistered(component));
    }

    public IEnumerable<object> GetAll(Type component)
    {
      Ensure.ArgumentNotNull((object) component, nameof (component));
      return this._mappings[component].Select<Type, object>((Func<Type, object>) (implementation => this.ResolveInstance(component, implementation)));
    }

    private object ResolveInstance(Type component, Type implementation)
    {
      lock (this._instances)
        return this._instances.ContainsKey(implementation) ? (object) this._instances[implementation] : this.CreateNewInstance(component, implementation);
    }

    private object CreateNewInstance(Type component, Type implementation)
    {
      ConstructorInfo constructorInfo = ComponentContainer.SelectConstructor(component, implementation);
      object[] array = ((IEnumerable<ParameterInfo>) constructorInfo.GetParameters()).Select<ParameterInfo, object>((Func<ParameterInfo, object>) (parameter => this.Get(parameter.ParameterType))).ToArray<object>();
      try
      {
        INinjectComponent newInstance = constructorInfo.Invoke(array) as INinjectComponent;
        newInstance.Settings = this.Kernel.Settings;
        if (!this.transients.Contains(new KeyValuePair<Type, Type>(component, implementation)))
          this._instances.Add(implementation, newInstance);
        return (object) newInstance;
      }
      catch (TargetInvocationException ex)
      {
        ex.RethrowInnerException();
        return (object) null;
      }
    }

    private static ConstructorInfo SelectConstructor(Type component, Type implementation)
    {
      ConstructorInfo constructorInfo = ((IEnumerable<ConstructorInfo>) implementation.GetConstructors()).OrderByDescending<ConstructorInfo, int>((Func<ConstructorInfo, int>) (c => c.GetParameters().Length)).FirstOrDefault<ConstructorInfo>();
      return !(constructorInfo == (ConstructorInfo) null) ? constructorInfo : throw new InvalidOperationException(ExceptionFormatter.NoConstructorsAvailableForComponent(component, implementation));
    }
  }
}
