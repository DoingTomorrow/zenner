// Decompiled with JetBrains decompiler
// Type: GmmDbLib.SQLiteDB
// Assembly: GmmDbLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: FBABFE79-334C-44CD-A4BC-A66429DECD0D
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\GmmDbLib.dll

using System.Data.SQLite;
using System.IO;

#nullable disable
namespace GmmDbLib
{
  public class SQLiteDB : DbBasis
  {
    public SQLiteDB(string connectionString)
      : base(connectionString)
    {
    }

    public SQLiteDB(BaseDbConnection newBaseDbConnection)
      : base(newBaseDbConnection)
    {
    }

    public override long GetDatabaseSize()
    {
      string dataSource = new SQLiteConnectionStringBuilder(this.ConnectionString).DataSource;
      if (string.IsNullOrEmpty(dataSource))
        return 0;
      try
      {
        return new FileInfo(dataSource).Length;
      }
      catch
      {
      }
      return 0;
    }
  }
}
