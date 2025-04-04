// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Automapping.AutoMap
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace FluentNHibernate.Automapping
{
  public static class AutoMap
  {
    public static AutoPersistenceModel AssemblyOf<T>() => AutoMap.Assembly(typeof (T).Assembly);

    public static AutoPersistenceModel AssemblyOf<T>(IAutomappingConfiguration cfg)
    {
      return AutoMap.Assembly(typeof (T).Assembly, cfg);
    }

    public static AutoPersistenceModel Assembly(System.Reflection.Assembly assembly)
    {
      return AutoMap.Source((ITypeSource) new AssemblyTypeSource(assembly));
    }

    public static AutoPersistenceModel Assembly(System.Reflection.Assembly assembly, IAutomappingConfiguration cfg)
    {
      return AutoMap.Source((ITypeSource) new AssemblyTypeSource(assembly), cfg);
    }

    public static AutoPersistenceModel Assemblies(params System.Reflection.Assembly[] assemblies)
    {
      return AutoMap.Source((ITypeSource) new CombinedAssemblyTypeSource(((IEnumerable<System.Reflection.Assembly>) assemblies).Select<System.Reflection.Assembly, AssemblyTypeSource>((Func<System.Reflection.Assembly, AssemblyTypeSource>) (x => new AssemblyTypeSource(x)))));
    }

    public static AutoPersistenceModel Assemblies(
      IAutomappingConfiguration cfg,
      params System.Reflection.Assembly[] assemblies)
    {
      return AutoMap.Source((ITypeSource) new CombinedAssemblyTypeSource(((IEnumerable<System.Reflection.Assembly>) assemblies).Select<System.Reflection.Assembly, AssemblyTypeSource>((Func<System.Reflection.Assembly, AssemblyTypeSource>) (x => new AssemblyTypeSource(x)))), cfg);
    }

    public static AutoPersistenceModel Assemblies(
      IAutomappingConfiguration cfg,
      IEnumerable<System.Reflection.Assembly> assemblies)
    {
      return AutoMap.Source((ITypeSource) new CombinedAssemblyTypeSource(assemblies.Select<System.Reflection.Assembly, AssemblyTypeSource>((Func<System.Reflection.Assembly, AssemblyTypeSource>) (x => new AssemblyTypeSource(x)))), cfg);
    }

    public static AutoPersistenceModel Source(ITypeSource source)
    {
      return new AutoPersistenceModel().AddTypeSource(source);
    }

    public static AutoPersistenceModel Source(ITypeSource source, IAutomappingConfiguration cfg)
    {
      return new AutoPersistenceModel(cfg).AddTypeSource(source);
    }

    [Obsolete("Depreciated overload. Use either chained Where method or ShouldMap(Type) in IAutomappingConfiguration.")]
    public static AutoPersistenceModel Source(ITypeSource source, Func<Type, bool> where)
    {
      return AutoMap.Source(source).Where(where);
    }

    [Obsolete("Depreciated overload. Use either chained Where method or ShouldMap(Type) in IAutomappingConfiguration.")]
    public static AutoPersistenceModel Assembly(System.Reflection.Assembly assembly, Func<Type, bool> where)
    {
      return AutoMap.Source((ITypeSource) new AssemblyTypeSource(assembly)).Where(where);
    }

    [Obsolete("Depreciated overload. Use either chained Where method or ShouldMap(Type) in IAutomappingConfiguration.")]
    public static AutoPersistenceModel AssemblyOf<T>(Func<Type, bool> where)
    {
      return AutoMap.Assembly(typeof (T).Assembly).Where(where);
    }
  }
}
