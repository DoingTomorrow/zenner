// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Mapping.DiscriminatorPart
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Mapping.Providers;
using FluentNHibernate.MappingModel;
using System;
using System.Diagnostics;
using System.Linq.Expressions;

#nullable disable
namespace FluentNHibernate.Mapping
{
  public class DiscriminatorPart : IDiscriminatorMappingProvider
  {
    private readonly string columnName;
    private readonly Type entity;
    private readonly Action<Type, ISubclassMappingProvider> setter;
    private readonly TypeReference discriminatorValueType;
    private readonly AttributeStore attributes = new AttributeStore();
    private readonly AttributeStore columnAttributes = new AttributeStore();
    private bool nextBool = true;

    public DiscriminatorPart(
      string columnName,
      Type entity,
      Action<Type, ISubclassMappingProvider> setter,
      TypeReference discriminatorValueType)
    {
      this.columnName = columnName;
      this.entity = entity;
      this.setter = setter;
      this.discriminatorValueType = discriminatorValueType;
    }

    [Obsolete("Inline definitions of subclasses are depreciated. Please create a derived class from SubclassMap in the same way you do with ClassMap.")]
    public DiscriminatorPart SubClass<TSubClass>(
      object discriminatorValue,
      Action<SubClassPart<TSubClass>> action)
    {
      SubClassPart<TSubClass> subClassPart = new SubClassPart<TSubClass>(this, discriminatorValue);
      action(subClassPart);
      this.setter(typeof (TSubClass), (ISubclassMappingProvider) subClassPart);
      return this;
    }

    [Obsolete("Inline definitions of subclasses are depreciated. Please create a derived class from SubclassMap in the same way you do with ClassMap.")]
    public DiscriminatorPart SubClass<TSubClass>(Action<SubClassPart<TSubClass>> action)
    {
      return this.SubClass<TSubClass>((object) null, action);
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public DiscriminatorPart Not
    {
      get
      {
        this.nextBool = !this.nextBool;
        return this;
      }
    }

    public DiscriminatorPart AlwaysSelectWithValue()
    {
      this.attributes.Set("Force", 2, (object) this.nextBool);
      this.nextBool = true;
      return this;
    }

    public DiscriminatorPart ReadOnly()
    {
      this.attributes.Set("Insert", 2, (object) !this.nextBool);
      this.nextBool = true;
      return this;
    }

    public DiscriminatorPart Formula(string sql)
    {
      this.attributes.Set(nameof (Formula), 2, (object) sql);
      return this;
    }

    public DiscriminatorPart Precision(int precision)
    {
      this.columnAttributes.Set(nameof (Precision), 2, (object) precision);
      return this;
    }

    public DiscriminatorPart Scale(int scale)
    {
      this.columnAttributes.Set(nameof (Scale), 2, (object) scale);
      return this;
    }

    public DiscriminatorPart Length(int length)
    {
      this.columnAttributes.Set(nameof (Length), 2, (object) length);
      return this;
    }

    public DiscriminatorPart Nullable()
    {
      this.columnAttributes.Set("NotNull", 2, (object) !this.nextBool);
      this.nextBool = true;
      return this;
    }

    public DiscriminatorPart Unique()
    {
      this.columnAttributes.Set(nameof (Unique), 2, (object) this.nextBool);
      this.nextBool = true;
      return this;
    }

    public DiscriminatorPart UniqueKey(string keyColumns)
    {
      this.columnAttributes.Set(nameof (UniqueKey), 2, (object) keyColumns);
      return this;
    }

    public DiscriminatorPart Index(string index)
    {
      this.columnAttributes.Set(nameof (Index), 2, (object) index);
      return this;
    }

    public DiscriminatorPart Check(string constraint)
    {
      this.columnAttributes.Set(nameof (Check), 2, (object) constraint);
      return this;
    }

    public DiscriminatorPart Default(object value)
    {
      this.columnAttributes.Set(nameof (Default), 2, (object) value.ToString());
      return this;
    }

    public DiscriminatorPart CustomType<T>()
    {
      this.attributes.Set("Type", 2, (object) new TypeReference(typeof (T)));
      return this;
    }

    public DiscriminatorPart CustomType(Type type)
    {
      this.attributes.Set("Type", 2, (object) new TypeReference(type));
      return this;
    }

    public DiscriminatorPart CustomType(string type)
    {
      this.attributes.Set("Type", 2, (object) new TypeReference(type));
      return this;
    }

    public DiscriminatorPart SqlType(string type)
    {
      this.columnAttributes.Set(nameof (SqlType), 2, (object) type);
      return this;
    }

    DiscriminatorMapping IDiscriminatorMappingProvider.GetDiscriminatorMapping()
    {
      DiscriminatorMapping discriminatorMapping = new DiscriminatorMapping(this.attributes.Clone())
      {
        ContainingEntityType = this.entity
      };
      discriminatorMapping.Set((Expression<Func<DiscriminatorMapping, object>>) (x => x.Type), 0, (object) this.discriminatorValueType);
      ColumnMapping mapping = new ColumnMapping(this.columnAttributes.Clone());
      mapping.Set<string>((Expression<Func<ColumnMapping, string>>) (x => x.Name), 0, this.columnName);
      discriminatorMapping.AddColumn(0, mapping);
      return discriminatorMapping;
    }
  }
}
