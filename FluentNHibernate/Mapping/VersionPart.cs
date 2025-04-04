// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Mapping.VersionPart
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Mapping.Providers;
using FluentNHibernate.MappingModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;

#nullable disable
namespace FluentNHibernate.Mapping
{
  public class VersionPart : IVersionMappingProvider
  {
    private readonly Type entity;
    private readonly Member member;
    private readonly AccessStrategyBuilder<VersionPart> access;
    private readonly VersionGeneratedBuilder<VersionPart> generated;
    private readonly AttributeStore attributes = new AttributeStore();
    private readonly AttributeStore columnAttributes = new AttributeStore();
    private readonly List<string> columns = new List<string>();
    private bool nextBool = true;

    public VersionPart(Type entity, Member member)
    {
      this.entity = entity;
      this.member = member;
      this.access = new AccessStrategyBuilder<VersionPart>(this, (Action<string>) (value => this.attributes.Set(nameof (Access), 2, (object) value)));
      this.generated = new VersionGeneratedBuilder<VersionPart>(this, (Action<string>) (value => this.attributes.Set(nameof (Generated), 2, (object) value)));
      this.SetDefaultAccess();
    }

    private void SetDefaultAccess()
    {
      FluentNHibernate.Mapping.Access access = MemberAccessResolver.Resolve(this.member);
      if (access == FluentNHibernate.Mapping.Access.Property || access == FluentNHibernate.Mapping.Access.Unset)
        return;
      this.attributes.Set("Access", 0, (object) access.ToString());
    }

    public VersionGeneratedBuilder<VersionPart> Generated => this.generated;

    public AccessStrategyBuilder<VersionPart> Access => this.access;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public VersionPart Not
    {
      get
      {
        this.nextBool = !this.nextBool;
        return this;
      }
    }

    public VersionPart Column(string name)
    {
      this.columns.Add(name);
      return this;
    }

    public VersionPart UnsavedValue(string value)
    {
      this.attributes.Set(nameof (UnsavedValue), 2, (object) value);
      return this;
    }

    public VersionPart Length(int length)
    {
      this.columnAttributes.Set(nameof (Length), 2, (object) length);
      return this;
    }

    public VersionPart Precision(int precision)
    {
      this.columnAttributes.Set(nameof (Precision), 2, (object) precision);
      return this;
    }

    public VersionPart Scale(int scale)
    {
      this.columnAttributes.Set(nameof (Scale), 2, (object) scale);
      return this;
    }

    public VersionPart Nullable()
    {
      this.columnAttributes.Set("NotNull", 2, (object) !this.nextBool);
      this.nextBool = true;
      return this;
    }

    public VersionPart Unique()
    {
      this.columnAttributes.Set(nameof (Unique), 2, (object) this.nextBool);
      this.nextBool = true;
      return this;
    }

    public VersionPart UniqueKey(string keyColumns)
    {
      this.columnAttributes.Set(nameof (UniqueKey), 2, (object) keyColumns);
      return this;
    }

    public VersionPart Index(string index)
    {
      this.columnAttributes.Set(nameof (Index), 2, (object) index);
      return this;
    }

    public VersionPart Check(string constraint)
    {
      this.columnAttributes.Set(nameof (Check), 2, (object) constraint);
      return this;
    }

    public VersionPart Default(object value)
    {
      this.columnAttributes.Set(nameof (Default), 2, (object) value.ToString());
      return this;
    }

    public VersionPart CustomType<T>()
    {
      this.attributes.Set("Type", 2, (object) new TypeReference(typeof (T)));
      return this;
    }

    public VersionPart CustomType(Type type)
    {
      this.attributes.Set("Type", 2, (object) new TypeReference(type));
      return this;
    }

    public VersionPart CustomType(string type)
    {
      this.attributes.Set("Type", 2, (object) new TypeReference(type));
      return this;
    }

    public VersionPart CustomSqlType(string sqlType)
    {
      this.columnAttributes.Set("SqlType", 2, (object) sqlType);
      return this;
    }

    VersionMapping IVersionMappingProvider.GetVersionMapping()
    {
      VersionMapping mapping = new VersionMapping(this.attributes.Clone())
      {
        ContainingEntityType = this.entity
      };
      mapping.Set<string>((Expression<Func<VersionMapping, string>>) (x => x.Name), 0, this.member.Name);
      mapping.Set<TypeReference>((Expression<Func<VersionMapping, TypeReference>>) (x => x.Type), 0, this.member.PropertyType == typeof (DateTime) ? new TypeReference("timestamp") : new TypeReference(this.member.PropertyType));
      ColumnMapping mapping1 = new ColumnMapping(this.columnAttributes.Clone());
      mapping1.Set<string>((Expression<Func<ColumnMapping, string>>) (x => x.Name), 0, this.member.Name);
      mapping.AddColumn(0, mapping1);
      this.columns.ForEach((Action<string>) (column =>
      {
        ColumnMapping mapping2 = new ColumnMapping(this.columnAttributes.Clone());
        mapping2.Set<string>((Expression<Func<ColumnMapping, string>>) (x => x.Name), 0, column);
        mapping.AddColumn(2, mapping2);
      }));
      return mapping;
    }
  }
}
