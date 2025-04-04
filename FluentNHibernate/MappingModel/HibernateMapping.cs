// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.MappingModel.HibernateMapping
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.Utils;
using FluentNHibernate.Visitors;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

#nullable disable
namespace FluentNHibernate.MappingModel
{
  [Serializable]
  public class HibernateMapping : MappingBase
  {
    private readonly IList<ClassMapping> classes;
    private readonly IList<FilterDefinitionMapping> filters;
    private readonly IList<ImportMapping> imports;
    private readonly AttributeStore attributes;

    public HibernateMapping()
      : this(new AttributeStore())
    {
    }

    public HibernateMapping(AttributeStore attributes)
    {
      this.attributes = attributes;
      this.classes = (IList<ClassMapping>) new List<ClassMapping>();
      this.filters = (IList<FilterDefinitionMapping>) new List<FilterDefinitionMapping>();
      this.imports = (IList<ImportMapping>) new List<ImportMapping>();
    }

    public override void AcceptVisitor(IMappingModelVisitor visitor)
    {
      visitor.ProcessHibernateMapping(this);
      foreach (ImportMapping import in this.Imports)
        visitor.Visit(import);
      foreach (ClassMapping classMapping in this.Classes)
        visitor.Visit(classMapping);
      foreach (FilterDefinitionMapping filter in this.Filters)
        visitor.Visit(filter);
    }

    public IEnumerable<ClassMapping> Classes => (IEnumerable<ClassMapping>) this.classes;

    public IEnumerable<FilterDefinitionMapping> Filters
    {
      get => (IEnumerable<FilterDefinitionMapping>) this.filters;
    }

    public IEnumerable<ImportMapping> Imports => (IEnumerable<ImportMapping>) this.imports;

    public void AddClass(ClassMapping classMapping) => this.classes.Add(classMapping);

    public void AddFilter(FilterDefinitionMapping filterMapping) => this.filters.Add(filterMapping);

    public void AddImport(ImportMapping importMapping) => this.imports.Add(importMapping);

    public string Catalog => this.attributes.GetOrDefault<string>(nameof (Catalog));

    public string DefaultAccess => this.attributes.GetOrDefault<string>(nameof (DefaultAccess));

    public string DefaultCascade => this.attributes.GetOrDefault<string>(nameof (DefaultCascade));

    public bool AutoImport => this.attributes.GetOrDefault<bool>(nameof (AutoImport));

    public string Schema => this.attributes.GetOrDefault<string>(nameof (Schema));

    public bool DefaultLazy => this.attributes.GetOrDefault<bool>(nameof (DefaultLazy));

    public string Namespace => this.attributes.GetOrDefault<string>(nameof (Namespace));

    public string Assembly => this.attributes.GetOrDefault<string>(nameof (Assembly));

    public bool Equals(HibernateMapping other)
    {
      if (object.ReferenceEquals((object) null, (object) other))
        return false;
      if (object.ReferenceEquals((object) this, (object) other))
        return true;
      return other.classes.ContentEquals<ClassMapping>((IEnumerable<ClassMapping>) this.classes) && other.filters.ContentEquals<FilterDefinitionMapping>((IEnumerable<FilterDefinitionMapping>) this.filters) && other.imports.ContentEquals<ImportMapping>((IEnumerable<ImportMapping>) this.imports) && object.Equals((object) other.attributes, (object) this.attributes);
    }

    public override bool Equals(object obj)
    {
      if (object.ReferenceEquals((object) null, obj))
        return false;
      if (object.ReferenceEquals((object) this, obj))
        return true;
      return obj.GetType() == typeof (HibernateMapping) && this.Equals((HibernateMapping) obj);
    }

    public override int GetHashCode()
    {
      return (((this.classes != null ? this.classes.GetHashCode() : 0) * 397 ^ (this.filters != null ? this.filters.GetHashCode() : 0)) * 397 ^ (this.imports != null ? this.imports.GetHashCode() : 0)) * 397 ^ (this.attributes != null ? this.attributes.GetHashCode() : 0);
    }

    public void Set<T>(Expression<Func<HibernateMapping, T>> expression, int layer, T value)
    {
      this.Set(expression.ToMember<HibernateMapping, T>().Name, layer, (object) value);
    }

    protected override void Set(string attribute, int layer, object value)
    {
      this.attributes.Set(attribute, layer, value);
    }

    public override bool IsSpecified(string attribute) => this.attributes.IsSpecified(attribute);
  }
}
