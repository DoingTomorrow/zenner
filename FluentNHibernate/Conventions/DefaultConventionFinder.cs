// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.DefaultConventionFinder
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

#nullable disable
namespace FluentNHibernate.Conventions
{
  public class DefaultConventionFinder : IConventionFinder
  {
    private IDiagnosticLogger log = (IDiagnosticLogger) new NullDiagnosticsLogger();
    private readonly ConventionsCollection conventions = new ConventionsCollection();

    public IEnumerable<T> Find<T>() where T : IConvention
    {
      foreach (Type type in this.conventions.Where<Type>((Func<Type, bool>) (x => typeof (T).IsAssignableFrom(x))))
      {
        foreach (object instance in this.conventions[type])
          yield return (T) instance;
      }
    }

    public void SetLogger(IDiagnosticLogger logger) => this.log = logger;

    public void Merge(IConventionFinder conventionFinder)
    {
      this.conventions.Merge(conventionFinder.Conventions);
    }

    public ConventionsCollection Conventions => this.conventions;

    public void AddSource(ITypeSource source)
    {
      foreach (Type type in source.GetTypes())
      {
        if (!type.IsAbstract && !type.IsGenericType && typeof (IConvention).IsAssignableFrom(type))
          this.Add(type, DefaultConventionFinder.MissingConstructor.Ignore);
      }
      this.log.LoadedConventionsFromSource(source);
    }

    public void AddAssembly(Assembly assembly)
    {
      this.AddSource((ITypeSource) new AssemblyTypeSource(assembly));
    }

    public void AddFromAssemblyOf<T>() => this.AddAssembly(typeof (T).Assembly);

    public void Add<T>() where T : IConvention
    {
      this.Add(typeof (T), DefaultConventionFinder.MissingConstructor.Throw);
    }

    public void Add(Type type) => this.Add(type, DefaultConventionFinder.MissingConstructor.Throw);

    public void Add(Type type, object instance)
    {
      if (this.conventions.Contains(type) && !this.AllowMultiplesOf(type))
        return;
      this.conventions.Add(type, instance);
    }

    public void Add<T>(T instance) where T : IConvention
    {
      if (this.conventions.Contains(typeof (T)) && !this.AllowMultiplesOf(instance.GetType()))
        return;
      this.conventions.Add(typeof (T), (object) instance);
    }

    private void Add(
      Type type,
      DefaultConventionFinder.MissingConstructor missingConstructor)
    {
      if (missingConstructor == DefaultConventionFinder.MissingConstructor.Throw && !this.HasValidConstructor(type))
        throw new MissingConstructorException(type);
      if (missingConstructor == DefaultConventionFinder.MissingConstructor.Ignore && !this.HasValidConstructor(type) || this.conventions.Contains(type) && !this.AllowMultiplesOf(type))
        return;
      this.conventions.Add(type, this.Instantiate(type));
      this.log.ConventionDiscovered(type);
    }

    private bool AllowMultiplesOf(Type type)
    {
      return Attribute.GetCustomAttribute((MemberInfo) type, typeof (MultipleAttribute), true) != null;
    }

    private object Instantiate(Type type)
    {
      object obj = (object) null;
      foreach (ConstructorInfo constructor in type.GetConstructors())
      {
        if (this.IsFinderConstructor(constructor))
          obj = constructor.Invoke((object[]) new DefaultConventionFinder[1]
          {
            this
          });
        else if (this.IsParameterlessConstructor(constructor))
          obj = constructor.Invoke(new object[0]);
      }
      return obj;
    }

    private bool HasValidConstructor(Type type)
    {
      foreach (ConstructorInfo constructor in type.GetConstructors())
      {
        if (this.IsFinderConstructor(constructor) || this.IsParameterlessConstructor(constructor))
          return true;
      }
      return false;
    }

    private bool IsFinderConstructor(ConstructorInfo constructor)
    {
      ParameterInfo[] parameters = constructor.GetParameters();
      return parameters.Length == 1 && parameters[0].ParameterType == typeof (IConventionFinder);
    }

    private bool IsParameterlessConstructor(ConstructorInfo constructor)
    {
      return constructor.GetParameters().Length == 0;
    }

    private enum MissingConstructor
    {
      Throw,
      Ignore,
    }
  }
}
