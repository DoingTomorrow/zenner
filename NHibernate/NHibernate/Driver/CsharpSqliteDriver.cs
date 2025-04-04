// Decompiled with JetBrains decompiler
// Type: NHibernate.Driver.CsharpSqliteDriver
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

#nullable disable
namespace NHibernate.Driver
{
  public class CsharpSqliteDriver : ReflectionBasedDriver
  {
    public CsharpSqliteDriver()
      : base("Community.CsharpSqlite.SQLiteClient", "Community.CsharpSqlite.SQLiteClient.SqliteConnection", "Community.CsharpSqlite.SQLiteClient.SqliteCommand")
    {
    }

    public override bool UseNamedPrefixInSql => true;

    public override bool UseNamedPrefixInParameter => true;

    public override string NamedPrefix => "@";

    public override bool SupportsMultipleOpenReaders => false;
  }
}
