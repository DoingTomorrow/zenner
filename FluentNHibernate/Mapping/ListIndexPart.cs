// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Mapping.ListIndexPart
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;
using System;
using System.Linq.Expressions;

#nullable disable
namespace FluentNHibernate.Mapping
{
  public class ListIndexPart
  {
    private readonly IndexMapping mapping;
    private readonly AttributeStore sharedColumnAttributes = new AttributeStore();

    public ListIndexPart(IndexMapping mapping) => this.mapping = mapping;

    public void Offset(int offset)
    {
      if (this.mapping.IsSpecified("Type"))
        throw new NotSupportedException("Offset is mutual exclusive with Type()");
      this.mapping.Set<int>((Expression<Func<IndexMapping, int>>) (x => x.Offset), 2, offset);
    }

    public void Column(string indexColumnName)
    {
      ColumnMapping mapping = new ColumnMapping(this.sharedColumnAttributes);
      mapping.Set<string>((Expression<Func<ColumnMapping, string>>) (x => x.Name), 2, indexColumnName);
      this.mapping.AddColumn(2, mapping);
    }

    public void Type<TIndex>()
    {
      if (this.mapping.IsSpecified("Offset"))
        throw new NotSupportedException("Type() is mutual exclusive with Offset()");
      this.mapping.Set<TypeReference>((Expression<Func<IndexMapping, TypeReference>>) (x => x.Type), 2, new TypeReference(typeof (TIndex)));
    }

    public void Type(System.Type type)
    {
      if (this.mapping.IsSpecified("Offset"))
        throw new NotSupportedException("Type() is mutual exclusive with Offset()");
      this.mapping.Set<TypeReference>((Expression<Func<IndexMapping, TypeReference>>) (x => x.Type), 2, new TypeReference(type));
    }

    public void Type(string type)
    {
      if (this.mapping.IsSpecified("Offset"))
        throw new NotSupportedException("Type() is mutual exclusive with Offset()");
      this.mapping.Set<TypeReference>((Expression<Func<IndexMapping, TypeReference>>) (x => x.Type), 2, new TypeReference(type));
    }
  }
}
