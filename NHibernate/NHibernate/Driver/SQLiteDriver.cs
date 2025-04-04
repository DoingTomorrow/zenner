// Decompiled with JetBrains decompiler
// Type: NHibernate.Driver.SQLiteDriver
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;

#nullable disable
namespace NHibernate.Driver
{
  [Obsolete("Use NHibernate.Driver.SQLite20Driver")]
  public class SQLiteDriver : ReflectionBasedDriver
  {
    public SQLiteDriver()
      : base("System.Data.SQLite", "SQLite.NET", "Finisar.SQLite.SQLiteConnection", "Finisar.SQLite.SQLiteCommand")
    {
    }

    public override bool UseNamedPrefixInSql => true;

    public override bool UseNamedPrefixInParameter => true;

    public override string NamedPrefix => "@";

    public override bool SupportsMultipleOpenReaders => false;
  }
}
