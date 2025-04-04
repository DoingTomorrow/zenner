// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Cfg.HbmMappingsContainer
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using NHibernate.Cfg;
using System;
using System.Collections.Generic;
using System.Reflection;

#nullable disable
namespace FluentNHibernate.Cfg
{
  public class HbmMappingsContainer
  {
    private readonly IList<Type> classes = (IList<Type>) new List<Type>();
    private readonly IList<Assembly> assemblies = (IList<Assembly>) new List<Assembly>();

    internal HbmMappingsContainer()
    {
    }

    public HbmMappingsContainer AddClasses(params Type[] types)
    {
      foreach (Type type in types)
        this.classes.Add(type);
      this.WasUsed = types.Length > 0;
      return this;
    }

    public HbmMappingsContainer AddFromAssemblyOf<T>() => this.AddFromAssembly(typeof (T).Assembly);

    public HbmMappingsContainer AddFromAssembly(Assembly assembly)
    {
      this.assemblies.Add(assembly);
      this.WasUsed = true;
      return this;
    }

    internal bool WasUsed { get; set; }

    internal void Apply(Configuration cfg)
    {
      foreach (Type persistentClass in (IEnumerable<Type>) this.classes)
        cfg.AddClass(persistentClass);
      foreach (Assembly assembly in (IEnumerable<Assembly>) this.assemblies)
        cfg.AddAssembly(assembly);
    }
  }
}
