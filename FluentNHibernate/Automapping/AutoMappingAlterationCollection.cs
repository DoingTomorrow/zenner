// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Automapping.AutoMappingAlterationCollection
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Automapping.Alterations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

#nullable disable
namespace FluentNHibernate.Automapping
{
  public class AutoMappingAlterationCollection : IEnumerable<IAutoMappingAlteration>, IEnumerable
  {
    private readonly List<IAutoMappingAlteration> alterations = new List<IAutoMappingAlteration>();

    private void Add(Type type)
    {
      this.Add((IAutoMappingAlteration) Activator.CreateInstance(type));
    }

    public AutoMappingAlterationCollection Add<T>() where T : IAutoMappingAlteration
    {
      this.Add(typeof (T));
      return this;
    }

    public AutoMappingAlterationCollection Add(IAutoMappingAlteration alteration)
    {
      if (!this.alterations.Exists((Predicate<IAutoMappingAlteration>) (a => a.GetType() == alteration.GetType() && alteration.GetType() != typeof (AutoMappingOverrideAlteration))))
        this.alterations.Add(alteration);
      return this;
    }

    public AutoMappingAlterationCollection AddFromAssembly(Assembly assembly)
    {
      foreach (Type exportedType in assembly.GetExportedTypes())
      {
        if (typeof (IAutoMappingAlteration).IsAssignableFrom(exportedType))
          this.Add(exportedType);
      }
      return this;
    }

    public AutoMappingAlterationCollection AddFromAssemblyOf<T>()
    {
      return this.AddFromAssembly(typeof (T).Assembly);
    }

    protected internal void Apply(AutoPersistenceModel model)
    {
      foreach (IAutoMappingAlteration alteration in this.alterations)
        alteration.Alter(model);
    }

    public IEnumerator<IAutoMappingAlteration> GetEnumerator()
    {
      return (IEnumerator<IAutoMappingAlteration>) this.alterations.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();
  }
}
