// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Mapping.IndexPart
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.Utils;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

#nullable disable
namespace FluentNHibernate.Mapping
{
  public class IndexPart
  {
    private readonly System.Type entity;
    private readonly List<string> columns = new List<string>();
    private readonly AttributeStore attributes = new AttributeStore();

    public IndexPart(System.Type entity) => this.entity = entity;

    public IndexPart Column(string indexColumnName)
    {
      this.columns.Add(indexColumnName);
      return this;
    }

    public IndexPart Type<TIndex>()
    {
      this.attributes.Set(nameof (Type), 2, (object) new TypeReference(typeof (TIndex)));
      return this;
    }

    public IndexPart Type(System.Type type)
    {
      this.attributes.Set(nameof (Type), 2, (object) new TypeReference(type));
      return this;
    }

    [Obsolete("Do not call this method. Implementation detail mistakenly made public. Will be made private in next version.")]
    public IndexMapping GetIndexMapping()
    {
      IndexMapping mapping = new IndexMapping(this.attributes.Clone());
      mapping.ContainingEntityType = this.entity;
      this.columns.Each<string>((Action<string>) (name =>
      {
        ColumnMapping mapping1 = new ColumnMapping();
        mapping1.Set<string>((Expression<Func<ColumnMapping, string>>) (x => x.Name), 0, name);
        mapping.AddColumn(2, mapping1);
      }));
      return mapping;
    }
  }
}
