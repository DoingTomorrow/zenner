// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.Instances.GeneratorInstance
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel.Identity;
using NHibernate.Id;
using System;

#nullable disable
namespace FluentNHibernate.Conventions.Instances
{
  public class GeneratorInstance : 
    GeneratorInspector,
    IGeneratorInstance,
    IGeneratorInspector,
    IInspector
  {
    private readonly GeneratorBuilder builder;

    public GeneratorInstance(GeneratorMapping mapping, Type type)
      : base(mapping)
    {
      this.builder = new GeneratorBuilder(mapping, type, 1);
    }

    public void Increment() => this.builder.Increment();

    public void Increment(Action<ParamBuilder> paramValues) => this.builder.Increment(paramValues);

    public void Identity() => this.builder.Identity();

    public void Identity(Action<ParamBuilder> paramValues) => this.builder.Identity(paramValues);

    public void Sequence(string sequenceName) => this.builder.Sequence(sequenceName);

    public void Sequence(string sequenceName, Action<ParamBuilder> paramValues)
    {
      this.builder.Sequence(sequenceName, paramValues);
    }

    public void HiLo(string table, string column, string maxLo, string where)
    {
      this.builder.HiLo(table, column, maxLo, where);
    }

    public void HiLo(string table, string column, string maxLo)
    {
      this.builder.HiLo(table, column, maxLo);
    }

    public void HiLo(string table, string column, string maxLo, Action<ParamBuilder> paramValues)
    {
      this.builder.HiLo(table, column, maxLo, paramValues);
    }

    public void HiLo(string maxLo) => this.builder.HiLo(maxLo);

    public void HiLo(string maxLo, Action<ParamBuilder> paramValues)
    {
      this.builder.HiLo(maxLo, paramValues);
    }

    public void SeqHiLo(string sequence, string maxLo) => this.builder.SeqHiLo(sequence, maxLo);

    public void SeqHiLo(string sequence, string maxLo, Action<ParamBuilder> paramValues)
    {
      this.builder.SeqHiLo(sequence, maxLo, paramValues);
    }

    public void UuidHex(string format) => this.builder.UuidHex(format);

    public void UuidHex(string format, Action<ParamBuilder> paramValues)
    {
      this.builder.UuidHex(format, paramValues);
    }

    public void UuidString() => this.builder.UuidString();

    public void UuidString(Action<ParamBuilder> paramValues)
    {
      this.builder.UuidString(paramValues);
    }

    public void Guid() => this.builder.Guid();

    public void Guid(Action<ParamBuilder> paramValues) => this.builder.Guid(paramValues);

    public void GuidComb() => this.builder.GuidComb();

    public void GuidComb(Action<ParamBuilder> paramValues) => this.builder.GuidComb(paramValues);

    public void GuidNative() => this.builder.GuidNative();

    public void GuidNative(Action<ParamBuilder> paramValues)
    {
      this.builder.GuidNative(paramValues);
    }

    public void Select() => this.builder.Select();

    public void Select(Action<ParamBuilder> paramValues) => this.builder.Select(paramValues);

    public void SequenceIdentity() => this.builder.SequenceIdentity();

    public void SequenceIdentity(string sequence) => this.builder.SequenceIdentity(sequence);

    public void SequenceIdentity(Action<ParamBuilder> paramValues)
    {
      this.builder.SequenceIdentity(paramValues);
    }

    public void SequenceIdentity(string sequence, Action<ParamBuilder> paramValues)
    {
      this.builder.SequenceIdentity(sequence, paramValues);
    }

    public void TriggerIdentity() => this.builder.TriggerIdentity();

    public void TriggerIdentity(Action<ParamBuilder> paramValues)
    {
      this.builder.TriggerIdentity(paramValues);
    }

    public void Assigned() => this.builder.Assigned();

    public void Assigned(Action<ParamBuilder> paramValues) => this.builder.Assigned(paramValues);

    public void Native() => this.builder.Native();

    public void Native(Action<ParamBuilder> paramValues) => this.builder.Native(paramValues);

    public void Native(string sequenceName) => this.builder.Native(sequenceName);

    public void Native(string sequenceName, Action<ParamBuilder> paramValues)
    {
      this.builder.Native(sequenceName, paramValues);
    }

    public void Foreign(string property) => this.builder.Foreign(property);

    public void Foreign(string property, Action<ParamBuilder> paramValues)
    {
      this.builder.Foreign(property, paramValues);
    }

    public void Custom<T>() where T : IIdentifierGenerator => this.Custom(typeof (T));

    public void Custom(Type generator) => this.Custom(generator.AssemblyQualifiedName);

    public void Custom(string generator) => this.builder.Custom(generator);

    public void Custom<T>(Action<ParamBuilder> paramValues) where T : IIdentifierGenerator
    {
      this.Custom(typeof (T), paramValues);
    }

    public void Custom(Type generator, Action<ParamBuilder> paramValues)
    {
      this.Custom(generator.AssemblyQualifiedName, paramValues);
    }

    public void Custom(string generator, Action<ParamBuilder> paramValues)
    {
      this.builder.Custom(generator, paramValues);
    }
  }
}
