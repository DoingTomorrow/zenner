// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.Instances.ElementInstance
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;
using System;
using System.Linq;
using System.Linq.Expressions;

#nullable disable
namespace FluentNHibernate.Conventions.Instances
{
  public class ElementInstance : ElementInspector, IElementInstance, IElementInspector, IInspector
  {
    private readonly ElementMapping mapping;

    public ElementInstance(ElementMapping mapping)
      : base(mapping)
    {
      this.mapping = mapping;
    }

    public void Column(string columnName)
    {
      ColumnMapping columnMapping = this.mapping.Columns.FirstOrDefault<ColumnMapping>();
      ColumnMapping mapping = columnMapping == null ? new ColumnMapping() : columnMapping.Clone();
      mapping.Set<string>((Expression<Func<ColumnMapping, string>>) (x => x.Name), 1, columnName);
      this.mapping.AddColumn(1, mapping);
    }

    public void Type<T>() => this.Type(typeof (T));

    public void Type(string type)
    {
      this.mapping.Set<TypeReference>((Expression<Func<ElementMapping, TypeReference>>) (x => x.Type), 1, new TypeReference(type));
    }

    public void Type(System.Type type)
    {
      this.mapping.Set<TypeReference>((Expression<Func<ElementMapping, TypeReference>>) (x => x.Type), 1, new TypeReference(type));
    }
  }
}
