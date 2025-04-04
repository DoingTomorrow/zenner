// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.IConventionFinder
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Diagnostics;
using System;
using System.Collections.Generic;
using System.Reflection;

#nullable disable
namespace FluentNHibernate.Conventions
{
  public interface IConventionFinder
  {
    ConventionsCollection Conventions { get; }

    void AddSource(ITypeSource source);

    void AddAssembly(Assembly assembly);

    void AddFromAssemblyOf<T>();

    void Add<T>() where T : IConvention;

    void Add(Type type);

    void Add(Type type, object instance);

    void Add<T>(T instance) where T : IConvention;

    IEnumerable<T> Find<T>() where T : IConvention;

    void SetLogger(IDiagnosticLogger logger);

    void Merge(IConventionFinder conventionFinder);
  }
}
