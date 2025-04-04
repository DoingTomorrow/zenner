// Decompiled with JetBrains decompiler
// Type: NHibernate.Dialect.Schema.IForeignKeyMetadata
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

#nullable disable
namespace NHibernate.Dialect.Schema
{
  public interface IForeignKeyMetadata
  {
    string Name { get; }

    void AddColumn(IColumnMetadata column);

    IColumnMetadata[] Columns { get; }
  }
}
