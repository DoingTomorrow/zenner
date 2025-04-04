// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.ByCode.IIdMapper
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Type;
using System;

#nullable disable
namespace NHibernate.Mapping.ByCode
{
  public interface IIdMapper : IAccessorPropertyMapper
  {
    void Generator(IGeneratorDef generator);

    void Generator(IGeneratorDef generator, Action<IGeneratorMapper> generatorMapping);

    void Type(IIdentifierType persistentType);

    void UnsavedValue(object value);

    void Column(string name);

    void Length(int length);
  }
}
