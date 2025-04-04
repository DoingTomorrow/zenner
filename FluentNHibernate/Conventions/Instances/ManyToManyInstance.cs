// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.Instances.ManyToManyInstance
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

#nullable disable
namespace FluentNHibernate.Conventions.Instances
{
  public class ManyToManyInstance : 
    ManyToManyInspector,
    IManyToManyInstance,
    IManyToManyInspector,
    IRelationshipInstance,
    IRelationshipInspector,
    IInspector
  {
    private readonly ManyToManyMapping mapping;

    public ManyToManyInstance(ManyToManyMapping mapping)
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

    public IEnumerable<IColumnInstance> Columns
    {
      get
      {
        return this.mapping.Columns.Select<ColumnMapping, ColumnInstance>((Func<ColumnMapping, ColumnInstance>) (x => new ColumnInstance(this.mapping.ContainingEntityType, x))).Cast<IColumnInstance>();
      }
    }

    public void CustomClass<T>() => this.CustomClass(typeof (T));

    public void CustomClass(Type type)
    {
      this.mapping.Set<TypeReference>((Expression<Func<ManyToManyMapping, TypeReference>>) (x => x.Class), 1, new TypeReference(type));
    }

    public void ForeignKey(string constraint)
    {
      this.mapping.Set<string>((Expression<Func<ManyToManyMapping, string>>) (x => x.ForeignKey), 1, constraint);
    }

    public void Where(string where)
    {
      this.mapping.Set<string>((Expression<Func<ManyToManyMapping, string>>) (x => x.Where), 1, where);
    }

    public void OrderBy(string orderBy)
    {
      this.mapping.Set<string>((Expression<Func<ManyToManyMapping, string>>) (x => x.OrderBy), 1, orderBy);
    }
  }
}
