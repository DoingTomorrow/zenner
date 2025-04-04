// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Cfg.FluentMappingsContainer
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Conventions;
using FluentNHibernate.Visitors;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

#nullable disable
namespace FluentNHibernate.Cfg
{
  public class FluentMappingsContainer
  {
    private readonly IList<Assembly> assemblies = (IList<Assembly>) new List<Assembly>();
    private readonly List<Type> types = new List<Type>();
    private readonly IConventionFinder conventionFinder = (IConventionFinder) new DefaultConventionFinder();
    private string exportPath;
    private TextWriter exportTextWriter;
    private PairBiDirectionalManyToManySidesDelegate biDirectionalManyToManyPairer;

    [Obsolete("PersistenceModel is no longer available through FluentMappingsContainer. Use MappingConfiguration.UsePersistenceModel to supply a custom PersistenceModel", true)]
    public PersistenceModel PersistenceModel => (PersistenceModel) null;

    public FluentMappingsContainer OverrideBiDirectionalManyToManyPairing(
      PairBiDirectionalManyToManySidesDelegate userControlledPairing)
    {
      this.biDirectionalManyToManyPairer = userControlledPairing;
      return this;
    }

    public FluentMappingsContainer AddFromAssemblyOf<T>()
    {
      return this.AddFromAssembly(typeof (T).Assembly);
    }

    public FluentMappingsContainer AddFromAssembly(Assembly assembly)
    {
      this.assemblies.Add(assembly);
      this.WasUsed = true;
      return this;
    }

    public FluentMappingsContainer Add<T>() => this.Add(typeof (T));

    public FluentMappingsContainer Add(Type type)
    {
      if (type == null)
        throw new ArgumentNullException(nameof (type));
      this.types.Add(type);
      this.WasUsed = true;
      return this;
    }

    public FluentMappingsContainer ExportTo(string path)
    {
      this.exportPath = path;
      return this;
    }

    public FluentMappingsContainer ExportTo(TextWriter textWriter)
    {
      this.exportTextWriter = textWriter;
      return this;
    }

    public SetupConventionFinder<FluentMappingsContainer> Conventions
    {
      get => new SetupConventionFinder<FluentMappingsContainer>(this, this.conventionFinder);
    }

    internal bool WasUsed { get; set; }

    internal void Apply(PersistenceModel model)
    {
      foreach (Assembly assembly in (IEnumerable<Assembly>) this.assemblies)
        model.AddMappingsFromAssembly(assembly);
      foreach (Type type in this.types)
        model.Add(type);
      model.Conventions.Merge(this.conventionFinder);
      if (!string.IsNullOrEmpty(this.exportPath))
        model.WriteMappingsTo(this.exportPath);
      if (this.exportTextWriter != null)
        model.WriteMappingsTo(this.exportTextWriter);
      if (this.biDirectionalManyToManyPairer == null)
        return;
      model.BiDirectionalManyToManyPairer = this.biDirectionalManyToManyPairer;
    }
  }
}
