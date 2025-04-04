// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Mapping.IdentityGenerationStrategyBuilder`1
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.MappingModel.Identity;
using NHibernate.Id;
using System;

#nullable disable
namespace FluentNHibernate.Mapping
{
  public class IdentityGenerationStrategyBuilder<TParent>
  {
    private readonly TParent parent;
    private readonly Type entity;
    private readonly GeneratorMapping mapping = new GeneratorMapping();
    private readonly GeneratorBuilder builder;

    public IdentityGenerationStrategyBuilder(TParent parent, Type identityType, Type entity)
    {
      this.parent = parent;
      this.entity = entity;
      this.builder = new GeneratorBuilder(this.mapping, identityType, 2);
    }

    internal bool IsDirty { get; private set; }

    public GeneratorMapping GetGeneratorMapping()
    {
      this.mapping.ContainingEntityType = this.entity;
      return this.mapping;
    }

    public TParent Increment()
    {
      this.builder.Increment();
      this.IsDirty = true;
      return this.parent;
    }

    public TParent Increment(Action<ParamBuilder> paramValues)
    {
      this.builder.Increment(paramValues);
      this.IsDirty = true;
      return this.parent;
    }

    public TParent Identity()
    {
      this.builder.Identity();
      this.IsDirty = true;
      return this.parent;
    }

    public TParent Identity(Action<ParamBuilder> paramValues)
    {
      this.builder.Identity(paramValues);
      this.IsDirty = true;
      return this.parent;
    }

    public TParent Sequence(string sequenceName)
    {
      this.builder.Sequence(sequenceName);
      this.IsDirty = true;
      return this.parent;
    }

    public TParent Sequence(string sequenceName, Action<ParamBuilder> paramValues)
    {
      this.builder.Sequence(sequenceName, paramValues);
      this.IsDirty = true;
      return this.parent;
    }

    public TParent HiLo(string table, string column, string maxLo, string where)
    {
      this.builder.HiLo(table, column, maxLo, where);
      this.IsDirty = true;
      return this.parent;
    }

    public TParent HiLo(string table, string column, string maxLo)
    {
      this.builder.HiLo(table, column, maxLo);
      this.IsDirty = true;
      return this.parent;
    }

    public TParent HiLo(
      string table,
      string column,
      string maxLo,
      Action<ParamBuilder> paramValues)
    {
      this.builder.HiLo(table, column, maxLo, paramValues);
      this.IsDirty = true;
      return this.parent;
    }

    public TParent HiLo(string maxLo)
    {
      this.builder.HiLo(maxLo);
      this.IsDirty = true;
      return this.parent;
    }

    public TParent HiLo(string maxLo, Action<ParamBuilder> paramValues)
    {
      this.builder.HiLo(maxLo, paramValues);
      this.IsDirty = true;
      return this.parent;
    }

    public TParent SeqHiLo(string sequence, string maxLo)
    {
      this.builder.SeqHiLo(sequence, maxLo);
      this.IsDirty = true;
      return this.parent;
    }

    public TParent SeqHiLo(string sequence, string maxLo, Action<ParamBuilder> paramValues)
    {
      this.builder.SeqHiLo(sequence, maxLo, paramValues);
      this.IsDirty = true;
      return this.parent;
    }

    public TParent UuidHex(string format)
    {
      this.builder.UuidHex(format);
      this.IsDirty = true;
      return this.parent;
    }

    public TParent UuidHex(string format, Action<ParamBuilder> paramValues)
    {
      this.builder.UuidHex(format, paramValues);
      this.IsDirty = true;
      return this.parent;
    }

    public TParent UuidString()
    {
      this.builder.UuidString();
      this.IsDirty = true;
      return this.parent;
    }

    public TParent UuidString(Action<ParamBuilder> paramValues)
    {
      this.builder.UuidString(paramValues);
      this.IsDirty = true;
      return this.parent;
    }

    public TParent Guid()
    {
      this.builder.Guid();
      this.IsDirty = true;
      return this.parent;
    }

    public TParent Guid(Action<ParamBuilder> paramValues)
    {
      this.builder.Guid(paramValues);
      this.IsDirty = true;
      return this.parent;
    }

    public TParent GuidComb()
    {
      this.builder.GuidComb();
      this.IsDirty = true;
      return this.parent;
    }

    public TParent GuidComb(Action<ParamBuilder> paramValues)
    {
      this.builder.GuidComb(paramValues);
      this.IsDirty = true;
      return this.parent;
    }

    public TParent Assigned()
    {
      this.builder.Assigned();
      this.IsDirty = true;
      return this.parent;
    }

    public TParent Assigned(Action<ParamBuilder> paramValues)
    {
      this.builder.Assigned(paramValues);
      this.IsDirty = true;
      return this.parent;
    }

    public TParent Native()
    {
      this.builder.Native();
      this.IsDirty = true;
      return this.parent;
    }

    public TParent Native(Action<ParamBuilder> paramValues)
    {
      this.builder.Native(paramValues);
      this.IsDirty = true;
      return this.parent;
    }

    public TParent Native(string sequenceName)
    {
      this.builder.Native(sequenceName);
      this.IsDirty = true;
      return this.parent;
    }

    public TParent Native(string sequenceName, Action<ParamBuilder> paramValues)
    {
      this.builder.Native(sequenceName, paramValues);
      this.IsDirty = true;
      return this.parent;
    }

    public TParent Foreign(string property)
    {
      this.builder.Foreign(property);
      this.IsDirty = true;
      return this.parent;
    }

    public TParent Foreign(string property, Action<ParamBuilder> paramValues)
    {
      this.builder.Foreign(property, paramValues);
      this.IsDirty = true;
      return this.parent;
    }

    public TParent Custom<T>() where T : IIdentifierGenerator => this.Custom(typeof (T));

    public TParent Custom(Type generator) => this.Custom(generator.AssemblyQualifiedName);

    public TParent Custom(string generator)
    {
      this.builder.Custom(generator);
      this.IsDirty = true;
      return this.parent;
    }

    public TParent Custom<T>(Action<ParamBuilder> paramValues) where T : IIdentifierGenerator
    {
      return this.Custom(typeof (T), paramValues);
    }

    public TParent Custom(Type generator, Action<ParamBuilder> paramValues)
    {
      return this.Custom(generator.AssemblyQualifiedName, paramValues);
    }

    public TParent Custom(string generator, Action<ParamBuilder> paramValues)
    {
      this.builder.Custom(generator, paramValues);
      this.IsDirty = true;
      return this.parent;
    }

    public TParent GuidNative()
    {
      this.builder.GuidNative();
      this.IsDirty = true;
      return this.parent;
    }

    public TParent GuidNative(Action<ParamBuilder> paramValues)
    {
      this.builder.GuidNative(paramValues);
      this.IsDirty = true;
      return this.parent;
    }

    public TParent Select()
    {
      this.builder.Select();
      this.IsDirty = true;
      return this.parent;
    }

    public TParent Select(Action<ParamBuilder> paramValues)
    {
      this.builder.Select(paramValues);
      this.IsDirty = true;
      return this.parent;
    }

    public TParent SequenceIdentity()
    {
      this.builder.SequenceIdentity();
      this.IsDirty = true;
      return this.parent;
    }

    public TParent SequenceIdentity(string sequence)
    {
      this.builder.SequenceIdentity(sequence);
      this.IsDirty = true;
      return this.parent;
    }

    public TParent SequenceIdentity(Action<ParamBuilder> paramValues)
    {
      this.builder.SequenceIdentity(paramValues);
      this.IsDirty = true;
      return this.parent;
    }

    public TParent SequenceIdentity(string sequence, Action<ParamBuilder> paramValues)
    {
      this.builder.SequenceIdentity(sequence, paramValues);
      this.IsDirty = true;
      return this.parent;
    }

    public TParent TriggerIdentity()
    {
      this.builder.TriggerIdentity();
      this.IsDirty = true;
      return this.parent;
    }

    public TParent TriggerIdentity(Action<ParamBuilder> paramValues)
    {
      this.builder.TriggerIdentity(paramValues);
      this.IsDirty = true;
      return this.parent;
    }
  }
}
