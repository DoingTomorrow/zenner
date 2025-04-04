// Decompiled with JetBrains decompiler
// Type: NHibernate.Mapping.ByCode.IColumnMapper
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

#nullable disable
namespace NHibernate.Mapping.ByCode
{
  public interface IColumnMapper
  {
    void Name(string name);

    void Length(int length);

    void Precision(short precision);

    void Scale(short scale);

    void NotNullable(bool notnull);

    void Unique(bool unique);

    void UniqueKey(string uniquekeyName);

    void SqlType(string sqltype);

    void Index(string indexName);

    void Check(string checkConstraint);

    void Default(object defaultValue);
  }
}
