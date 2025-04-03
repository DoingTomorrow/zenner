// Decompiled with JetBrains decompiler
// Type: GmmDbLib.PostgreSQL
// Assembly: GmmDbLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: FBABFE79-334C-44CD-A4BC-A66429DECD0D
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\GmmDbLib.dll

using System.Data;

#nullable disable
namespace GmmDbLib
{
  internal class PostgreSQL : DbBasis
  {
    public PostgreSQL(string connectionString)
      : base(connectionString)
    {
    }

    public PostgreSQL(BaseDbConnection newBaseDbConnection)
      : base(newBaseDbConnection)
    {
    }

    public override bool OptimizeTable(string TableName)
    {
      try
      {
        using (IDbConnection dbConnection = this.GetDbConnection())
        {
          dbConnection.Open();
          IDbCommand command = dbConnection.CreateCommand();
          command.CommandText = "VACUUM Full [" + TableName + "];";
          command.ExecuteNonQuery();
          dbConnection.Close();
        }
      }
      catch
      {
        return false;
      }
      return true;
    }
  }
}
