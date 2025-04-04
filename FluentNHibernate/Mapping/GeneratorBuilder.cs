// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Mapping.GeneratorBuilder
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.MappingModel.Identity;
using NHibernate.Id;
using System;
using System.Linq.Expressions;

#nullable disable
namespace FluentNHibernate.Mapping
{
  internal class GeneratorBuilder
  {
    private readonly Type identityType;
    private readonly int layer;
    private readonly GeneratorMapping mapping;

    public GeneratorBuilder(GeneratorMapping mapping, Type identityType, int layer)
    {
      this.mapping = mapping;
      this.identityType = identityType;
      this.layer = layer;
    }

    private void SetGenerator(string generator)
    {
      this.mapping.Set<string>((Expression<Func<GeneratorMapping, string>>) (x => x.Class), this.layer, generator);
    }

    private void AddGeneratorParam(string name, string value)
    {
      this.mapping.Params.Add(name, value);
    }

    private void EnsureIntegralIdenityType()
    {
      if (!GeneratorBuilder.IsIntegralType(this.identityType))
        throw new InvalidOperationException("Identity type must be integral (int, long, uint, ulong)");
    }

    private void EnsureGuidIdentityType()
    {
      if (this.identityType != typeof (System.Guid) && this.identityType != typeof (System.Guid?))
        throw new InvalidOperationException("Identity type must be Guid");
    }

    private void EnsureStringIdentityType()
    {
      if (this.identityType != typeof (string))
        throw new InvalidOperationException("Identity type must be string");
    }

    private static bool IsIntegralType(Type t)
    {
      return t == typeof (int) || t == typeof (int?) || t == typeof (long) || t == typeof (long?) || t == typeof (uint) || t == typeof (uint?) || t == typeof (ulong) || t == typeof (ulong?) || t == typeof (byte) || t == typeof (byte?) || t == typeof (sbyte) || t == typeof (sbyte?) || t == typeof (short) || t == typeof (short?) || t == typeof (ushort) || t == typeof (ushort?);
    }

    public void Increment()
    {
      this.EnsureIntegralIdenityType();
      this.SetGenerator("increment");
    }

    public void Increment(Action<ParamBuilder> paramValues)
    {
      paramValues(new ParamBuilder(this.mapping.Params));
      this.Increment();
    }

    public void Identity()
    {
      this.EnsureIntegralIdenityType();
      this.SetGenerator("identity");
    }

    public void Identity(Action<ParamBuilder> paramValues)
    {
      paramValues(new ParamBuilder(this.mapping.Params));
      this.Identity();
    }

    public void Sequence(string sequenceName)
    {
      this.EnsureIntegralIdenityType();
      this.SetGenerator("sequence");
      this.AddGeneratorParam("sequence", sequenceName);
    }

    public void Sequence(string sequenceName, Action<ParamBuilder> paramValues)
    {
      paramValues(new ParamBuilder(this.mapping.Params));
      this.Sequence(sequenceName);
    }

    public void HiLo(string table, string column, string maxLo, string where)
    {
      this.AddGeneratorParam(nameof (table), table);
      this.AddGeneratorParam(nameof (column), column);
      this.AddGeneratorParam(nameof (where), where);
      this.HiLo(maxLo);
    }

    public void HiLo(string table, string column, string maxLo)
    {
      this.AddGeneratorParam(nameof (table), table);
      this.AddGeneratorParam(nameof (column), column);
      this.HiLo(maxLo);
    }

    public void HiLo(string table, string column, string maxLo, Action<ParamBuilder> paramValues)
    {
      paramValues(new ParamBuilder(this.mapping.Params));
      this.HiLo(table, column, maxLo);
    }

    public void HiLo(string maxLo)
    {
      this.EnsureIntegralIdenityType();
      this.SetGenerator("hilo");
      this.AddGeneratorParam("max_lo", maxLo);
    }

    public void HiLo(string maxLo, Action<ParamBuilder> paramValues)
    {
      paramValues(new ParamBuilder(this.mapping.Params));
      this.HiLo(maxLo);
    }

    public void SeqHiLo(string sequence, string maxLo)
    {
      this.EnsureIntegralIdenityType();
      this.SetGenerator("seqhilo");
      this.AddGeneratorParam(nameof (sequence), sequence);
      this.AddGeneratorParam("max_lo", maxLo);
    }

    public void SeqHiLo(string sequence, string maxLo, Action<ParamBuilder> paramValues)
    {
      paramValues(new ParamBuilder(this.mapping.Params));
      this.SeqHiLo(sequence, maxLo);
    }

    public void UuidHex(string format)
    {
      this.EnsureStringIdentityType();
      this.SetGenerator("uuid.hex");
      this.AddGeneratorParam(nameof (format), format);
    }

    public void UuidHex(string format, Action<ParamBuilder> paramValues)
    {
      paramValues(new ParamBuilder(this.mapping.Params));
      this.UuidHex(format);
    }

    public void UuidString()
    {
      this.EnsureStringIdentityType();
      this.SetGenerator("uuid.string");
    }

    public void UuidString(Action<ParamBuilder> paramValues)
    {
      paramValues(new ParamBuilder(this.mapping.Params));
      this.UuidString();
    }

    public void Guid()
    {
      this.EnsureGuidIdentityType();
      this.SetGenerator("guid");
    }

    public void Guid(Action<ParamBuilder> paramValues)
    {
      paramValues(new ParamBuilder(this.mapping.Params));
      this.Guid();
    }

    public void GuidComb()
    {
      this.EnsureGuidIdentityType();
      this.SetGenerator("guid.comb");
    }

    public void GuidComb(Action<ParamBuilder> paramValues)
    {
      paramValues(new ParamBuilder(this.mapping.Params));
      this.GuidComb();
    }

    public void GuidNative()
    {
      this.EnsureGuidIdentityType();
      this.SetGenerator("guid.native");
    }

    public void GuidNative(Action<ParamBuilder> paramValues)
    {
      paramValues(new ParamBuilder(this.mapping.Params));
    }

    public void Select() => this.SetGenerator("select");

    public void Select(Action<ParamBuilder> paramValues)
    {
      paramValues(new ParamBuilder(this.mapping.Params));
      this.Select();
    }

    public void SequenceIdentity()
    {
      this.EnsureIntegralIdenityType();
      this.SetGenerator("sequence-identity");
    }

    public void SequenceIdentity(string sequence)
    {
      this.AddGeneratorParam(nameof (sequence), sequence);
      this.SequenceIdentity();
    }

    public void SequenceIdentity(Action<ParamBuilder> paramValues)
    {
      paramValues(new ParamBuilder(this.mapping.Params));
      this.SequenceIdentity();
    }

    public void SequenceIdentity(string sequence, Action<ParamBuilder> paramValues)
    {
      paramValues(new ParamBuilder(this.mapping.Params));
      this.SequenceIdentity(sequence);
    }

    public void TriggerIdentity() => this.SetGenerator("trigger-identity");

    public void TriggerIdentity(Action<ParamBuilder> paramValues)
    {
      paramValues(new ParamBuilder(this.mapping.Params));
      this.TriggerIdentity();
    }

    public void Assigned() => this.SetGenerator("assigned");

    public void Assigned(Action<ParamBuilder> paramValues)
    {
      paramValues(new ParamBuilder(this.mapping.Params));
      this.Assigned();
    }

    public void Native()
    {
      this.EnsureIntegralIdenityType();
      this.SetGenerator("native");
    }

    public void Native(Action<ParamBuilder> paramValues)
    {
      paramValues(new ParamBuilder(this.mapping.Params));
      this.Native();
    }

    public void Native(string sequenceName)
    {
      this.EnsureIntegralIdenityType();
      this.SetGenerator("native");
      this.AddGeneratorParam("sequence", sequenceName);
    }

    public void Native(string sequenceName, Action<ParamBuilder> paramValues)
    {
      paramValues(new ParamBuilder(this.mapping.Params));
      this.Native(sequenceName);
    }

    public void Foreign(string property)
    {
      this.SetGenerator("foreign");
      this.AddGeneratorParam(nameof (property), property);
    }

    public void Foreign(string property, Action<ParamBuilder> paramValues)
    {
      paramValues(new ParamBuilder(this.mapping.Params));
      this.Foreign(property);
    }

    public void Custom<T>() where T : IIdentifierGenerator => this.Custom(typeof (T));

    public void Custom(Type generator) => this.Custom(generator.AssemblyQualifiedName);

    public void Custom(string generator) => this.SetGenerator(generator);

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
      paramValues(new ParamBuilder(this.mapping.Params));
      this.SetGenerator(generator);
    }
  }
}
