// Decompiled with JetBrains decompiler
// Type: NHibernate.Persister.Entity.IPropertyMapping
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Type;

#nullable disable
namespace NHibernate.Persister.Entity
{
  public interface IPropertyMapping
  {
    IType Type { get; }

    IType ToType(string propertyName);

    bool TryToType(string propertyName, out IType type);

    string[] ToColumns(string alias, string propertyName);

    string[] ToColumns(string propertyName);
  }
}
