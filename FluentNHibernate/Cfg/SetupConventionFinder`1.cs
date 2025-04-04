// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Cfg.SetupConventionFinder`1
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Conventions;
using FluentNHibernate.Diagnostics;
using System;
using System.Collections.Generic;
using System.Reflection;

#nullable disable
namespace FluentNHibernate.Cfg
{
  public class SetupConventionFinder<TReturn> : IConventionFinder
  {
    private readonly TReturn parent;
    private readonly IConventionFinder conventionFinder;

    public SetupConventionFinder(TReturn container, IConventionFinder conventionFinder)
    {
      this.parent = container;
      this.conventionFinder = conventionFinder;
    }

    ConventionsCollection IConventionFinder.Conventions => this.conventionFinder.Conventions;

    public TReturn AddSource(ITypeSource source)
    {
      this.conventionFinder.AddSource(source);
      return this.parent;
    }

    void IConventionFinder.AddSource(ITypeSource source) => this.AddSource(source);

    public TReturn AddAssembly(Assembly assembly)
    {
      this.conventionFinder.AddAssembly(assembly);
      return this.parent;
    }

    public TReturn AddFromAssemblyOf<T>()
    {
      this.conventionFinder.AddFromAssemblyOf<T>();
      return this.parent;
    }

    void IConventionFinder.AddFromAssemblyOf<T>() => this.AddFromAssemblyOf<T>();

    void IConventionFinder.AddAssembly(Assembly assembly) => this.AddAssembly(assembly);

    public TReturn Add<T>() where T : IConvention
    {
      this.conventionFinder.Add<T>();
      return this.parent;
    }

    void IConventionFinder.Add<T>() => this.Add<T>();

    public void Add(Type type, object instance) => this.conventionFinder.Add(type, instance);

    public TReturn Add<T>(T instance) where T : IConvention
    {
      this.conventionFinder.Add<T>(instance);
      return this.parent;
    }

    void IConventionFinder.Add(Type type) => this.Add(type);

    public TReturn Add(Type type)
    {
      this.conventionFinder.Add(type);
      return this.parent;
    }

    void IConventionFinder.Add<T>(T instance) => this.Add<T>(instance);

    public TReturn Add(params IConvention[] instances)
    {
      foreach (IConvention instance in instances)
        this.conventionFinder.Add(instance.GetType(), (object) instance);
      return this.parent;
    }

    public TReturn Setup(Action<IConventionFinder> setupAction)
    {
      setupAction((IConventionFinder) this);
      return this.parent;
    }

    public IEnumerable<T> Find<T>() where T : IConvention => this.conventionFinder.Find<T>();

    void IConventionFinder.SetLogger(IDiagnosticLogger logger)
    {
      this.conventionFinder.SetLogger(logger);
    }

    void IConventionFinder.Merge(IConventionFinder other) => this.conventionFinder.Merge(other);
  }
}
