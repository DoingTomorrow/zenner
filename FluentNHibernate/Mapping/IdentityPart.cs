// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Mapping.IdentityPart
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Mapping.Providers;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Identity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;

#nullable disable
namespace FluentNHibernate.Mapping
{
  public class IdentityPart : IIdentityMappingProvider
  {
    private readonly AttributeStore columnAttributes = new AttributeStore();
    private readonly IList<string> columns = (IList<string>) new List<string>();
    private Member member;
    private readonly Type entityType;
    private readonly AccessStrategyBuilder<IdentityPart> access;
    private readonly AttributeStore attributes = new AttributeStore();
    private Type identityType;
    private bool nextBool = true;
    private string name;

    public IdentityPart(Type entity, Member member)
    {
      this.entityType = entity;
      this.member = member;
      this.identityType = member.PropertyType;
      this.access = new AccessStrategyBuilder<IdentityPart>(this, (Action<string>) (value => this.attributes.Set(nameof (Access), 2, (object) value)));
      this.GeneratedBy = new IdentityGenerationStrategyBuilder<IdentityPart>(this, member.PropertyType, this.entityType);
      this.SetName(member.Name);
      this.SetDefaultGenerator();
      this.SetDefaultAccess();
    }

    public IdentityPart(Type entity, Type identityType)
    {
      this.entityType = entity;
      this.identityType = identityType;
      this.access = new AccessStrategyBuilder<IdentityPart>(this, (Action<string>) (value => this.attributes.Set(nameof (Access), 2, (object) value)));
      this.GeneratedBy = new IdentityGenerationStrategyBuilder<IdentityPart>(this, this.identityType, entity);
      this.SetDefaultGenerator();
    }

    private void SetDefaultAccess()
    {
      FluentNHibernate.Mapping.Access access = MemberAccessResolver.Resolve(this.member);
      if (access == FluentNHibernate.Mapping.Access.Property || access == FluentNHibernate.Mapping.Access.Unset)
        return;
      this.attributes.Set("Access", 0, (object) access.ToString());
    }

    public IdentityGenerationStrategyBuilder<IdentityPart> GeneratedBy { get; private set; }

    public AccessStrategyBuilder<IdentityPart> Access => this.access;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public IdentityPart Not
    {
      get
      {
        this.nextBool = !this.nextBool;
        return this;
      }
    }

    public IdentityPart UnsavedValue(object unsavedValue)
    {
      this.attributes.Set(nameof (UnsavedValue), 2, (object) (unsavedValue ?? (object) "null").ToString());
      return this;
    }

    public IdentityPart Column(string columnName)
    {
      this.columns.Clear();
      this.columns.Add(columnName);
      return this;
    }

    public IdentityPart Length(int length)
    {
      this.columnAttributes.Set(nameof (Length), 2, (object) length);
      return this;
    }

    public IdentityPart Precision(int precision)
    {
      this.columnAttributes.Set(nameof (Precision), 2, (object) precision);
      return this;
    }

    public IdentityPart Scale(int scale)
    {
      this.columnAttributes.Set(nameof (Scale), 2, (object) scale);
      return this;
    }

    public IdentityPart Nullable()
    {
      this.columnAttributes.Set("NotNull", 2, (object) !this.nextBool);
      this.nextBool = true;
      return this;
    }

    public IdentityPart Unique()
    {
      this.columnAttributes.Set(nameof (Unique), 2, (object) this.nextBool);
      this.nextBool = true;
      return this;
    }

    public IdentityPart UniqueKey(string keyColumns)
    {
      this.columnAttributes.Set(nameof (UniqueKey), 2, (object) keyColumns);
      return this;
    }

    public IdentityPart CustomSqlType(string sqlType)
    {
      this.columnAttributes.Set("SqlType", 2, (object) sqlType);
      return this;
    }

    public IdentityPart Index(string key)
    {
      this.columnAttributes.Set(nameof (Index), 2, (object) key);
      return this;
    }

    public IdentityPart Check(string constraint)
    {
      this.columnAttributes.Set(nameof (Check), 2, (object) constraint);
      return this;
    }

    public IdentityPart Default(object value)
    {
      this.columnAttributes.Set(nameof (Default), 2, (object) value.ToString());
      return this;
    }

    public IdentityPart CustomType<T>()
    {
      this.attributes.Set("Type", 2, (object) new TypeReference(typeof (T)));
      return this;
    }

    public IdentityPart CustomType(Type type)
    {
      this.attributes.Set("Type", 2, (object) new TypeReference(type));
      return this;
    }

    public IdentityPart CustomType(string type)
    {
      this.attributes.Set("Type", 2, (object) new TypeReference(type));
      return this;
    }

    internal void SetName(string newName) => this.name = newName;

    private bool HasNameSpecified => !string.IsNullOrEmpty(this.name);

    private void SetDefaultGenerator()
    {
      GeneratorMapping mapping = new GeneratorMapping();
      GeneratorBuilder generatorBuilder = new GeneratorBuilder(mapping, this.identityType, 2);
      if (this.identityType == typeof (Guid))
        generatorBuilder.GuidComb();
      else if (this.identityType == typeof (int) || this.identityType == typeof (long))
        generatorBuilder.Identity();
      else
        generatorBuilder.Assigned();
      this.attributes.Set("Generator", 0, (object) mapping);
    }

    IdMapping IIdentityMappingProvider.GetIdentityMapping()
    {
      IdMapping identityMapping = new IdMapping(this.attributes.Clone())
      {
        ContainingEntityType = this.entityType
      };
      if (this.columns.Count > 0)
      {
        foreach (string column in (IEnumerable<string>) this.columns)
        {
          ColumnMapping mapping = new ColumnMapping(this.columnAttributes.Clone());
          mapping.Set<string>((Expression<Func<ColumnMapping, string>>) (x => x.Name), 0, column);
          identityMapping.AddColumn(2, mapping);
        }
      }
      else if (this.HasNameSpecified)
      {
        ColumnMapping mapping = new ColumnMapping(this.columnAttributes.Clone());
        mapping.Set<string>((Expression<Func<ColumnMapping, string>>) (x => x.Name), 0, this.name);
        identityMapping.AddColumn(0, mapping);
      }
      if (this.member != (Member) null)
        identityMapping.Set((Expression<Func<IdMapping, object>>) (x => x.Name), 0, (object) this.name);
      identityMapping.Set((Expression<Func<IdMapping, object>>) (x => x.Type), 0, (object) new TypeReference(this.identityType));
      if (this.GeneratedBy.IsDirty)
        identityMapping.Set((Expression<Func<IdMapping, object>>) (x => x.Generator), 2, (object) this.GeneratedBy.GetGeneratorMapping());
      return identityMapping;
    }
  }
}
