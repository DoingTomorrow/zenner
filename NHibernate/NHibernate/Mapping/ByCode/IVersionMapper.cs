// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.ByCode.IVersionMapper
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Type;
using NHibernate.UserTypes;

#nullable disable
namespace NHibernate.Mapping.ByCode
{
  public interface IVersionMapper : IAccessorPropertyMapper, IColumnsMapper
  {
    void Type(IVersionType persistentType);

    void Type<TPersistentType>() where TPersistentType : IUserVersionType;

    void Type(System.Type persistentType);

    void UnsavedValue(object value);

    void Insert(bool useInInsert);

    void Generated(VersionGeneration generatedByDb);
  }
}
