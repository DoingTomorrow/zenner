// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Mapping.AnyPart`1
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Mapping.Providers;
using FluentNHibernate.MappingModel;
using FluentNHibernate.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;

#nullable disable
namespace FluentNHibernate.Mapping
{
  public class AnyPart<T> : IAnyMappingProvider
  {
    private readonly AttributeStore attributes = new AttributeStore();
    private readonly Type entity;
    private readonly Member member;
    private readonly AccessStrategyBuilder<AnyPart<T>> access;
    private readonly CascadeExpression<AnyPart<T>> cascade;
    private readonly IList<string> typeColumns = (IList<string>) new List<string>();
    private readonly IList<string> identifierColumns = (IList<string>) new List<string>();
    private readonly IList<MetaValueMapping> metaValues = (IList<MetaValueMapping>) new List<MetaValueMapping>();
    private bool nextBool = true;
    private bool idTypeSet;

    public AnyPart(Type entity, Member member)
    {
      this.entity = entity;
      this.member = member;
      this.access = new AccessStrategyBuilder<AnyPart<T>>(this, (Action<string>) (value => this.attributes.Set(nameof (Access), 2, (object) value)));
      this.cascade = new CascadeExpression<AnyPart<T>>(this, (Action<string>) (value => this.attributes.Set(nameof (Cascade), 2, (object) value)));
      this.SetDefaultAccess();
    }

    private void SetDefaultAccess()
    {
      FluentNHibernate.Mapping.Access access = MemberAccessResolver.Resolve(this.member);
      if (access == FluentNHibernate.Mapping.Access.Property || access == FluentNHibernate.Mapping.Access.Unset)
        return;
      this.attributes.Set("Access", 0, (object) access.ToString());
    }

    public AccessStrategyBuilder<AnyPart<T>> Access => this.access;

    public CascadeExpression<AnyPart<T>> Cascade => this.cascade;

    public AnyPart<T> IdentityType(Expression<Func<T, object>> expression)
    {
      return this.IdentityType(expression.ToMember<T, object>().PropertyType);
    }

    public AnyPart<T> IdentityType<TIdentity>() => this.IdentityType(typeof (TIdentity));

    public AnyPart<T> IdentityType(Type type)
    {
      this.attributes.Set("IdType", 2, (object) type.AssemblyQualifiedName);
      this.idTypeSet = true;
      return this;
    }

    public AnyPart<T> EntityTypeColumn(string columnName)
    {
      this.typeColumns.Add(columnName);
      return this;
    }

    public AnyPart<T> EntityIdentifierColumn(string columnName)
    {
      this.identifierColumns.Add(columnName);
      return this;
    }

    public AnyPart<T> AddMetaValue<TModel>(string valueMap)
    {
      return this.AddMetaValue(typeof (TModel), valueMap);
    }

    public AnyPart<T> AddMetaValue(Type @class, string valueMap)
    {
      MetaValueMapping metaValueMapping = new MetaValueMapping()
      {
        ContainingEntityType = this.entity
      };
      metaValueMapping.Set<TypeReference>((Expression<Func<MetaValueMapping, TypeReference>>) (x => x.Class), 0, new TypeReference(@class));
      metaValueMapping.Set<string>((Expression<Func<MetaValueMapping, string>>) (x => x.Value), 0, valueMap);
      this.metaValues.Add(metaValueMapping);
      return this;
    }

    public AnyPart<T> Insert()
    {
      this.attributes.Set(nameof (Insert), 2, (object) this.nextBool);
      this.nextBool = true;
      return this;
    }

    public AnyPart<T> Update()
    {
      this.attributes.Set(nameof (Update), 2, (object) this.nextBool);
      this.nextBool = true;
      return this;
    }

    public AnyPart<T> ReadOnly()
    {
      this.attributes.Set("Insert", 2, (object) !this.nextBool);
      this.attributes.Set("Update", 2, (object) !this.nextBool);
      this.nextBool = true;
      return this;
    }

    public AnyPart<T> LazyLoad()
    {
      this.attributes.Set("Lazy", 2, (object) this.nextBool);
      this.nextBool = true;
      return this;
    }

    public AnyPart<T> OptimisticLock()
    {
      this.attributes.Set(nameof (OptimisticLock), 2, (object) this.nextBool);
      this.nextBool = true;
      return this;
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public AnyPart<T> Not
    {
      get
      {
        this.nextBool = !this.nextBool;
        return this;
      }
    }

    AnyMapping IAnyMappingProvider.GetAnyMapping()
    {
      AnyMapping anyMapping = new AnyMapping(this.attributes.Clone());
      if (this.typeColumns.Count<string>() == 0)
        throw new InvalidOperationException("<any> mapping is not valid without specifying an Entity Type Column");
      if (this.identifierColumns.Count<string>() == 0)
        throw new InvalidOperationException("<any> mapping is not valid without specifying an Entity Identifier Column");
      if (!this.idTypeSet)
        throw new InvalidOperationException("<any> mapping is not valid without specifying an IdType");
      anyMapping.ContainingEntityType = this.entity;
      anyMapping.Set<string>((Expression<Func<AnyMapping, string>>) (x => x.Name), 0, this.member.Name);
      if (!anyMapping.IsSpecified("MetaType"))
        anyMapping.Set<TypeReference>((Expression<Func<AnyMapping, TypeReference>>) (x => x.MetaType), 0, new TypeReference(this.member.PropertyType));
      if (this.metaValues.Count<MetaValueMapping>() > 0)
      {
        this.metaValues.Each<MetaValueMapping>(new Action<MetaValueMapping>(anyMapping.AddMetaValue));
        anyMapping.Set<TypeReference>((Expression<Func<AnyMapping, TypeReference>>) (x => x.MetaType), 0, new TypeReference(typeof (string)));
      }
      foreach (string typeColumn in (IEnumerable<string>) this.typeColumns)
      {
        ColumnMapping column = new ColumnMapping();
        column.Set<string>((Expression<Func<ColumnMapping, string>>) (x => x.Name), 0, typeColumn);
        anyMapping.AddTypeColumn(2, column);
      }
      foreach (string identifierColumn in (IEnumerable<string>) this.identifierColumns)
      {
        ColumnMapping column = new ColumnMapping();
        column.Set<string>((Expression<Func<ColumnMapping, string>>) (x => x.Name), 0, identifierColumn);
        anyMapping.AddIdentifierColumn(2, column);
      }
      return anyMapping;
    }

    public AnyPart<T> MetaType<TMetaType>()
    {
      this.attributes.Set(nameof (MetaType), 2, (object) new TypeReference(typeof (TMetaType)));
      return this;
    }

    public AnyPart<T> MetaType(string metaType)
    {
      this.attributes.Set(nameof (MetaType), 2, (object) new TypeReference(metaType));
      return this;
    }

    public AnyPart<T> MetaType(Type metaType)
    {
      this.attributes.Set(nameof (MetaType), 2, (object) new TypeReference(metaType));
      return this;
    }
  }
}
