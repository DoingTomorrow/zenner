// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Conventions.Instances.IGeneratorInstance
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Mapping;
using NHibernate.Id;
using System;

#nullable disable
namespace FluentNHibernate.Conventions.Instances
{
  public interface IGeneratorInstance : IGeneratorInspector, IInspector
  {
    void Increment();

    void Increment(Action<ParamBuilder> paramValue);

    void Identity();

    void Identity(Action<ParamBuilder> paramValue);

    void Sequence(string sequenceName);

    void Sequence(string sequenceName, Action<ParamBuilder> paramValues);

    void HiLo(string talbe, string column, string maxLo);

    void HiLo(string talbe, string column, string maxLo, Action<ParamBuilder> paramValues);

    void HiLo(string maxLo);

    void HiLo(string maxLo, Action<ParamBuilder> paramValues);

    void SeqHiLo(string sequence, string maxLo);

    void SeqHiLo(string sequence, string maxLo, Action<ParamBuilder> paramValues);

    void UuidHex(string format);

    void UuidHex(string format, Action<ParamBuilder> paramValues);

    void UuidString();

    void UuidString(Action<ParamBuilder> paramValues);

    void Guid();

    void Guid(Action<ParamBuilder> paramValues);

    void GuidComb();

    void GuidComb(Action<ParamBuilder> paramValues);

    void Assigned();

    void Assigned(Action<ParamBuilder> paramValues);

    void Native();

    void Native(Action<ParamBuilder> paramValues);

    void Foreign(string property);

    void Foreign(string property, Action<ParamBuilder> paramValues);

    void Custom<T>() where T : IIdentifierGenerator;

    void Custom(Type generator);

    void Custom(string generator);

    void Custom<T>(Action<ParamBuilder> paramValues) where T : IIdentifierGenerator;

    void Custom(Type generator, Action<ParamBuilder> paramValues);

    void Custom(string generator, Action<ParamBuilder> paramValues);

    void Native(string sequenceName);

    void Native(string sequenceName, Action<ParamBuilder> paramValues);
  }
}
