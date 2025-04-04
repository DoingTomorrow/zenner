// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Mapping.PropertyPart
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Mapping.Providers;
using FluentNHibernate.MappingModel;
using FluentNHibernate.Utils;
using NHibernate.UserTypes;
using System;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;

#nullable disable
namespace FluentNHibernate.Mapping
{
  public class PropertyPart : IPropertyMappingProvider
  {
    private readonly Member member;
    private readonly Type parentType;
    private readonly AccessStrategyBuilder<PropertyPart> access;
    private readonly PropertyGeneratedBuilder generated;
    private readonly ColumnMappingCollection<PropertyPart> columns;
    private readonly AttributeStore attributes = new AttributeStore();
    private readonly AttributeStore columnAttributes = new AttributeStore();
    private bool nextBool = true;

    public PropertyPart(Member member, Type parentType)
    {
      this.columns = new ColumnMappingCollection<PropertyPart>(this);
      this.access = new AccessStrategyBuilder<PropertyPart>(this, (Action<string>) (value => this.attributes.Set(nameof (Access), 2, (object) value)));
      this.generated = new PropertyGeneratedBuilder(this, (Action<string>) (value => this.attributes.Set(nameof (Generated), 2, (object) value)));
      this.member = member;
      this.parentType = parentType;
      this.SetDefaultAccess();
    }

    private void SetDefaultAccess()
    {
      FluentNHibernate.Mapping.Access access = MemberAccessResolver.Resolve(this.member);
      if (access == FluentNHibernate.Mapping.Access.Property || access == FluentNHibernate.Mapping.Access.Unset)
        return;
      this.attributes.Set("Access", 0, (object) access.ToString());
    }

    public PropertyGeneratedBuilder Generated => this.generated;

    public PropertyPart Column(string columnName)
    {
      this.Columns.Clear();
      this.Columns.Add(columnName);
      return this;
    }

    public ColumnMappingCollection<PropertyPart> Columns => this.columns;

    public AccessStrategyBuilder<PropertyPart> Access => this.access;

    public PropertyPart Insert()
    {
      this.attributes.Set(nameof (Insert), 2, (object) this.nextBool);
      this.nextBool = true;
      return this;
    }

    public PropertyPart Update()
    {
      this.attributes.Set(nameof (Update), 2, (object) this.nextBool);
      this.nextBool = true;
      return this;
    }

    public PropertyPart Length(int length)
    {
      this.columnAttributes.Set(nameof (Length), 2, (object) length);
      return this;
    }

    public PropertyPart Nullable()
    {
      this.columnAttributes.Set("NotNull", 2, (object) !this.nextBool);
      this.nextBool = true;
      return this;
    }

    public PropertyPart ReadOnly()
    {
      this.attributes.Set("Insert", 2, (object) !this.nextBool);
      this.attributes.Set("Update", 2, (object) !this.nextBool);
      this.nextBool = true;
      return this;
    }

    public PropertyPart Formula(string formula)
    {
      this.attributes.Set(nameof (Formula), 2, (object) formula);
      return this;
    }

    public PropertyPart LazyLoad()
    {
      this.attributes.Set("Lazy", 2, (object) this.nextBool);
      this.nextBool = true;
      return this;
    }

    public PropertyPart Index(string index)
    {
      this.columnAttributes.Set(nameof (Index), 2, (object) index);
      return this;
    }

    public PropertyPart CustomType<TCustomtype>() => this.CustomType(typeof (TCustomtype));

    public PropertyPart CustomType(Type type)
    {
      if (typeof (ICompositeUserType).IsAssignableFrom(type))
        this.AddColumnsFromCompositeUserType(type);
      return this.CustomType(TypeMapping.GetTypeString(type));
    }

    public PropertyPart CustomType(string type)
    {
      this.attributes.Set("Type", 2, (object) new TypeReference(type));
      return this;
    }

    public PropertyPart CustomType(Func<Type, Type> typeFunc)
    {
      Type type = typeFunc(this.member.PropertyType);
      if (typeof (ICompositeUserType).IsAssignableFrom(type))
        this.AddColumnsFromCompositeUserType(type);
      return this.CustomType(TypeMapping.GetTypeString(type));
    }

    private void AddColumnsFromCompositeUserType(Type compositeUserType)
    {
      foreach (string propertyName in ((ICompositeUserType) Activator.CreateInstance(compositeUserType)).PropertyNames)
        this.Columns.Add(propertyName);
    }

    public PropertyPart CustomSqlType(string sqlType)
    {
      this.columnAttributes.Set("SqlType", 2, (object) sqlType);
      return this;
    }

    public PropertyPart Unique()
    {
      this.columnAttributes.Set(nameof (Unique), 2, (object) this.nextBool);
      this.nextBool = true;
      return this;
    }

    public PropertyPart Precision(int precision)
    {
      this.columnAttributes.Set(nameof (Precision), 2, (object) precision);
      return this;
    }

    public PropertyPart Scale(int scale)
    {
      this.columnAttributes.Set(nameof (Scale), 2, (object) scale);
      return this;
    }

    public PropertyPart Default(string value)
    {
      this.columnAttributes.Set(nameof (Default), 2, (object) value);
      return this;
    }

    public PropertyPart UniqueKey(string keyName)
    {
      this.columnAttributes.Set(nameof (UniqueKey), 2, (object) keyName);
      return this;
    }

    public PropertyPart OptimisticLock()
    {
      this.attributes.Set(nameof (OptimisticLock), 2, (object) this.nextBool);
      this.nextBool = true;
      return this;
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public PropertyPart Not
    {
      get
      {
        this.nextBool = !this.nextBool;
        return this;
      }
    }

    public PropertyPart Check(string constraint)
    {
      this.columnAttributes.Set(nameof (Check), 2, (object) constraint);
      return this;
    }

    PropertyMapping IPropertyMappingProvider.GetPropertyMapping()
    {
      PropertyMapping propertyMapping = new PropertyMapping(this.attributes.Clone())
      {
        ContainingEntityType = this.parentType,
        Member = this.member
      };
      if (this.columns.Count<ColumnMapping>() == 0 && !propertyMapping.IsSpecified("Formula"))
      {
        ColumnMapping mapping = new ColumnMapping(this.columnAttributes.Clone());
        mapping.Set<string>((Expression<Func<ColumnMapping, string>>) (x => x.Name), 0, this.member.Name);
        propertyMapping.AddColumn(0, mapping);
      }
      foreach (ColumnMapping column in this.columns)
        propertyMapping.AddColumn(2, column);
      foreach (ColumnMapping column in propertyMapping.Columns)
      {
        if (this.member.PropertyType.IsNullable() && this.member.PropertyType.IsEnum())
          column.Set<bool>((Expression<Func<ColumnMapping, bool>>) (x => x.NotNull), 0, false);
        column.MergeAttributes(this.columnAttributes);
      }
      propertyMapping.Set<string>((Expression<Func<PropertyMapping, string>>) (x => x.Name), 0, propertyMapping.Member.Name);
      propertyMapping.Set<TypeReference>((Expression<Func<PropertyMapping, TypeReference>>) (x => x.Type), 0, this.GetDefaultType());
      return propertyMapping;
    }

    private TypeReference GetDefaultType()
    {
      TypeReference defaultType = new TypeReference(this.member.PropertyType);
      if (this.member.PropertyType.IsEnum())
        defaultType = new TypeReference(typeof (GenericEnumMapper<>).MakeGenericType(this.member.PropertyType));
      if (this.member.PropertyType.IsNullable() && this.member.PropertyType.IsEnum())
        defaultType = new TypeReference(typeof (GenericEnumMapper<>).MakeGenericType(this.member.PropertyType.GetGenericArguments()[0]));
      return defaultType;
    }
  }
}
