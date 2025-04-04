// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Mapping.NaturalIdPart`1
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Mapping.Providers;
using FluentNHibernate.MappingModel;
using FluentNHibernate.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;

#nullable disable
namespace FluentNHibernate.Mapping
{
  public class NaturalIdPart<T> : INaturalIdMappingProvider
  {
    private readonly AttributeStore attributes = new AttributeStore();
    private readonly IList<PropertyMapping> properties = (IList<PropertyMapping>) new List<PropertyMapping>();
    private readonly IList<ManyToOneMapping> manyToOnes = (IList<ManyToOneMapping>) new List<ManyToOneMapping>();
    private bool nextBool = true;

    public NaturalIdPart<T> Property(Expression<Func<T, object>> expression)
    {
      Member member = expression.ToMember<T, object>();
      return this.Property(expression, member.Name);
    }

    public NaturalIdPart<T> Property(Expression<Func<T, object>> expression, string columnName)
    {
      return this.Property(expression.ToMember<T, object>(), columnName);
    }

    protected virtual NaturalIdPart<T> Property(Member member, string columnName)
    {
      PropertyMapping propertyMapping = new PropertyMapping();
      propertyMapping.Set<string>((Expression<Func<PropertyMapping, string>>) (x => x.Name), 0, member.Name);
      propertyMapping.Set<TypeReference>((Expression<Func<PropertyMapping, TypeReference>>) (x => x.Type), 0, new TypeReference(member.PropertyType));
      ColumnMapping mapping = new ColumnMapping();
      mapping.Set<string>((Expression<Func<ColumnMapping, string>>) (x => x.Name), 0, columnName);
      propertyMapping.AddColumn(2, mapping);
      this.properties.Add(propertyMapping);
      return this;
    }

    public NaturalIdPart<T> Reference(Expression<Func<T, object>> expression)
    {
      Member member = expression.ToMember<T, object>();
      return this.Reference(expression, member.Name);
    }

    public NaturalIdPart<T> Reference(Expression<Func<T, object>> expression, string columnName)
    {
      return this.Reference(expression.ToMember<T, object>(), columnName);
    }

    protected virtual NaturalIdPart<T> Reference(Member member, string columnName)
    {
      ManyToOneMapping manyToOneMapping = new ManyToOneMapping()
      {
        ContainingEntityType = typeof (T)
      };
      manyToOneMapping.Set<string>((Expression<Func<ManyToOneMapping, string>>) (x => x.Name), 0, member.Name);
      manyToOneMapping.Set<TypeReference>((Expression<Func<ManyToOneMapping, TypeReference>>) (x => x.Class), 0, new TypeReference(member.PropertyType));
      ColumnMapping mapping = new ColumnMapping();
      mapping.Set<string>((Expression<Func<ColumnMapping, string>>) (x => x.Name), 0, columnName);
      manyToOneMapping.AddColumn(0, mapping);
      this.manyToOnes.Add(manyToOneMapping);
      return this;
    }

    public NaturalIdPart<T> ReadOnly()
    {
      this.attributes.Set("Mutable", 2, (object) !this.nextBool);
      this.nextBool = true;
      return this;
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public NaturalIdPart<T> Not
    {
      get
      {
        this.nextBool = false;
        return this;
      }
    }

    NaturalIdMapping INaturalIdMappingProvider.GetNaturalIdMapping()
    {
      NaturalIdMapping naturalIdMapping = new NaturalIdMapping(this.attributes.Clone());
      this.properties.Each<PropertyMapping>(new Action<PropertyMapping>(naturalIdMapping.AddProperty));
      this.manyToOnes.Each<ManyToOneMapping>(new Action<ManyToOneMapping>(naturalIdMapping.AddReference));
      return naturalIdMapping;
    }
  }
}
