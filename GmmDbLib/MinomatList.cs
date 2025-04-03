// Decompiled with JetBrains decompiler
// Type: GmmDbLib.MinomatList
// Assembly: GmmDbLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: FBABFE79-334C-44CD-A4BC-A66429DECD0D
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\GmmDbLib.dll

using GmmDbLib.DataSets;
using NLog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Linq;

#nullable disable
namespace GmmDbLib
{
  public static class MinomatList
  {
    private static Logger logger = LogManager.GetLogger(nameof (MinomatList));
    private static readonly object databaseLock = new object();

    public static List<DriverTables.MinomatListRow> LoadMinomatList(BaseDbConnection db)
    {
      if (db == null)
        throw new ArgumentNullException(nameof (db));
      using (DbConnection newConnection = db.GetNewConnection())
      {
        DbDataAdapter dataAdapter = db.GetDataAdapter("SELECT * FROM MinomatList;", newConnection);
        DriverTables.MinomatListDataTable source = new DriverTables.MinomatListDataTable();
        return dataAdapter.Fill((DataTable) source) == 0 ? (List<DriverTables.MinomatListRow>) null : source.ToList<DriverTables.MinomatListRow>();
      }
    }

    public static List<DriverTables.MinomatListRow> GetMinomatListByChallengeEncoded(
      BaseDbConnection db,
      uint challengeKeyEncoded)
    {
      if (db == null)
        throw new ArgumentNullException(nameof (db));
      using (DbConnection newConnection = db.GetNewConnection())
      {
        DbDataAdapter dataAdapter = db.GetDataAdapter("SELECT * FROM MinomatList WHERE ChallengeKeyEncoded=@ChallengeKeyEncoded;", newConnection);
        DbUtil.AddParameter((IDbCommand) dataAdapter.SelectCommand, "@ChallengeKeyEncoded", challengeKeyEncoded.ToString("X8"));
        DriverTables.MinomatListDataTable source = new DriverTables.MinomatListDataTable();
        return dataAdapter.Fill((DataTable) source) == 0 ? (List<DriverTables.MinomatListRow>) null : source.ToList<DriverTables.MinomatListRow>();
      }
    }

    public static SortedList<ulong, ulong> LoadHandshakeKeys(BaseDbConnection db)
    {
      if (db == null)
        throw new ArgumentNullException(nameof (db));
      using (DbConnection newConnection = db.GetNewConnection())
      {
        newConnection.Open();
        DbCommand command = newConnection.CreateCommand();
        command.CommandText = "SELECT SessionKey, SessionKeyOld, GsmIDEncoded, ChallengeKeyEncoded, GsmIDEncodedOld, ChallengeKeyEncodedOld FROM MinomatList;";
        SortedList<ulong, ulong> sortedList = new SortedList<ulong, ulong>();
        using (DbDataReader dbDataReader = command.ExecuteReader())
        {
          while (dbDataReader.Read())
          {
            string s1 = dbDataReader["SessionKey"].ToString();
            string s2 = dbDataReader["SessionKeyOld"].ToString();
            string s3 = dbDataReader["GsmIDEncoded"].ToString();
            string s4 = dbDataReader["ChallengeKeyEncoded"].ToString();
            string s5 = dbDataReader["GsmIDEncodedOld"].ToString();
            string s6 = dbDataReader["ChallengeKeyEncodedOld"].ToString();
            ulong result1;
            uint result2;
            uint result3;
            if (ulong.TryParse(s1, NumberStyles.AllowHexSpecifier, (IFormatProvider) null, out result1) && uint.TryParse(s3, NumberStyles.AllowHexSpecifier, (IFormatProvider) null, out result2) && uint.TryParse(s4, NumberStyles.AllowHexSpecifier, (IFormatProvider) null, out result3))
            {
              ulong key = (ulong) result3 << 32 | (ulong) result2;
              sortedList.Add(key, result1);
            }
            ulong result4;
            uint result5;
            uint result6;
            if (ulong.TryParse(s2, NumberStyles.AllowHexSpecifier, (IFormatProvider) null, out result4) && uint.TryParse(s5, NumberStyles.AllowHexSpecifier, (IFormatProvider) null, out result5) && uint.TryParse(s6, NumberStyles.AllowHexSpecifier, (IFormatProvider) null, out result6))
            {
              ulong key = (ulong) result6 << 32 | (ulong) result5;
              if (!sortedList.ContainsKey(key))
                sortedList.Add(key, result4);
            }
          }
          return sortedList;
        }
      }
    }

    public static ulong? GetSessionKey(
      BaseDbConnection db,
      uint gsmIDEncoded,
      uint challengeKeyEncoded)
    {
      if (db == null)
        throw new ArgumentNullException(nameof (db));
      using (DbConnection newConnection = db.GetNewConnection())
      {
        newConnection.Open();
        DbCommand command = newConnection.CreateCommand();
        command.CommandText = "SELECT SessionKey FROM MinomatList WHERE GsmIDEncoded=@GsmIDEncoded AND ChallengeKeyEncoded=@ChallengeKeyEncoded;";
        DbUtil.AddParameter((IDbCommand) command, "@GsmIDEncoded", gsmIDEncoded.ToString("X8"));
        DbUtil.AddParameter((IDbCommand) command, "@ChallengeKeyEncoded", challengeKeyEncoded.ToString("X8"));
        object obj = command.ExecuteScalar();
        if (obj == null)
        {
          command.CommandText = "SELECT SessionKeyOld FROM MinomatList WHERE GsmIDEncodedOld=@GsmIDEncodedOld AND ChallengeKeyEncodedOld=@ChallengeKeyEncodedOld;";
          DbUtil.AddParameter((IDbCommand) command, "@GsmIDEncodedOld", gsmIDEncoded.ToString("X8"));
          DbUtil.AddParameter((IDbCommand) command, "@ChallengeKeyEncodedOld", challengeKeyEncoded.ToString("X8"));
          obj = command.ExecuteScalar();
          if (obj == null)
            return new ulong?();
        }
        ulong result;
        return !ulong.TryParse(obj.ToString(), NumberStyles.AllowHexSpecifier, (IFormatProvider) null, out result) ? new ulong?() : new ulong?(result);
      }
    }

    public static bool DeleteMinomatList(BaseDbConnection db)
    {
      if (db == null)
        throw new ArgumentNullException(nameof (db));
      using (DbConnection newConnection = db.GetNewConnection())
      {
        newConnection.Open();
        DbCommand command = newConnection.CreateCommand();
        command.CommandText = "DELETE FROM MinomatList;";
        return command.ExecuteNonQuery() > 0;
      }
    }

    public static bool DeleteMinomatList(BaseDbConnection db, uint gsmID)
    {
      if (db == null)
        throw new ArgumentNullException(nameof (db));
      using (DbConnection newConnection = db.GetNewConnection())
      {
        newConnection.Open();
        DbCommand command = newConnection.CreateCommand();
        command.CommandText = "DELETE FROM MinomatList WHERE GsmID=@GsmID";
        DbUtil.AddParameter((IDbCommand) command, "@GsmID", gsmID.ToString("X8"));
        return command.ExecuteNonQuery() == 1;
      }
    }

    public static DriverTables.MinomatListRow AddMinomatList(
      BaseDbConnection db,
      uint gsmID,
      uint challengeKey,
      ulong sessionKey)
    {
      if (db == null)
        throw new ArgumentNullException(nameof (db));
      uint num1 = gsmID ^ (uint) (sessionKey & (ulong) uint.MaxValue);
      uint num2 = challengeKey ^ (uint) (sessionKey >> 32);
      using (DbConnection newConnection = db.GetNewConnection())
      {
        DbDataAdapter dataAdapter = db.GetDataAdapter("SELECT * FROM MinomatList;", newConnection, out DbCommandBuilder _);
        DriverTables.MinomatListDataTable minomatListDataTable = new DriverTables.MinomatListDataTable();
        DriverTables.MinomatListRow row = minomatListDataTable.NewMinomatListRow();
        row.GsmID = gsmID.ToString("X8");
        row.ChallengeKey = challengeKey.ToString("X8");
        row.SessionKey = sessionKey.ToString("X8");
        row.GsmIDEncoded = num1.ToString("X8");
        row.ChallengeKeyEncoded = num2.ToString("X8");
        minomatListDataTable.AddMinomatListRow(row);
        if (dataAdapter.Update((DataTable) minomatListDataTable) != 1)
          throw new Exception("Can not add new item to MinomatList table!");
        return row;
      }
    }

    public static DriverTables.MinomatListRow GetMinomatList(BaseDbConnection db, uint gsmID)
    {
      if (db == null)
        throw new ArgumentNullException(nameof (db));
      if (gsmID <= 0U)
        throw new ArgumentException(nameof (gsmID));
      using (DbConnection newConnection = db.GetNewConnection())
      {
        DbDataAdapter dataAdapter = db.GetDataAdapter("SELECT * FROM MinomatList WHERE GsmID=@GsmID;", newConnection);
        DriverTables.MinomatListDataTable source = new DriverTables.MinomatListDataTable();
        DbUtil.AddParameter((IDbCommand) dataAdapter.SelectCommand, "@GsmID", gsmID.ToString("X8"));
        return dataAdapter.Fill((DataTable) source) != 1 ? (DriverTables.MinomatListRow) null : source.ToList<DriverTables.MinomatListRow>()[0];
      }
    }

    public static void SaveMinomatList(
      BaseDbConnection db,
      uint? gsmID,
      uint? minolID,
      uint? challengeKey,
      ulong? sessionKey,
      uint? challengeKeyOld,
      ulong? sessionKeyOld,
      uint? gsmIDEncoded,
      uint? challengeKeyEncoded,
      uint? gsmIDEncodedOld,
      uint? challengeKeyEncodedOld)
    {
      if (db == null)
        throw new ArgumentNullException(nameof (db));
      if (!gsmID.HasValue)
        throw new ArgumentException(nameof (gsmID));
      if (!challengeKey.HasValue)
        throw new ArgumentException(nameof (challengeKey));
      if (!sessionKey.HasValue)
        throw new ArgumentNullException(nameof (sessionKey));
      if (!gsmIDEncoded.HasValue)
        throw new ArgumentNullException(nameof (gsmIDEncoded));
      if (!challengeKeyEncoded.HasValue)
        throw new ArgumentNullException(nameof (challengeKeyEncoded));
      lock (MinomatList.databaseLock)
      {
        using (DbConnection newConnection = db.GetNewConnection())
        {
          newConnection.Open();
          DbCommand command = newConnection.CreateCommand();
          command.CommandText = "SELECT GsmID FROM MinomatList WHERE GsmID = @GsmID;";
          DbCommand cmd1 = command;
          uint num = gsmID.Value;
          string str1 = num.ToString("X8");
          DbUtil.AddParameter((IDbCommand) cmd1, "@GsmID", str1);
          object obj = command.ExecuteScalar();
          command.Parameters.Clear();
          if (obj == null)
          {
            Logger logger = MinomatList.logger;
            num = gsmID.Value;
            string message = "INSERT INTO MinomatList... GSM ID: " + num.ToString("X8");
            logger.Debug(message);
            command.CommandText = "INSERT INTO MinomatList(GsmID,MinolID,ChallengeKey,SessionKey,ChallengeKeyOld,SessionKeyOld,GsmIDEncoded,ChallengeKeyEncoded,GsmIDEncodedOld,ChallengeKeyEncodedOld) VALUES (@GsmID,@MinolID,@ChallengeKey,@SessionKey,@ChallengeKeyOld,@SessionKeyOld,@GsmIDEncoded,@ChallengeKeyEncoded,@GsmIDEncodedOld,@ChallengeKeyEncodedOld)";
            DbCommand cmd2 = command;
            num = gsmID.Value;
            string str2 = num.ToString("X8");
            DbUtil.AddParameter((IDbCommand) cmd2, "@GsmID", str2);
          }
          else
          {
            Logger logger = MinomatList.logger;
            num = gsmID.Value;
            string message = "UPDATE MinomatList... GSM ID: " + num.ToString("X8");
            logger.Debug(message);
            command.CommandText = "UPDATE MinomatList SET MinolID=@MinolID,ChallengeKey=@ChallengeKey,SessionKey=@SessionKey,ChallengeKeyOld=@ChallengeKeyOld,SessionKeyOld=@SessionKeyOld,GsmIDEncoded=@GsmIDEncoded,ChallengeKeyEncoded=@ChallengeKeyEncoded,GsmIDEncodedOld=@GsmIDEncodedOld,ChallengeKeyEncodedOld=@ChallengeKeyEncodedOld WHERE GsmID=@GsmID";
          }
          DbCommand cmd3 = command;
          string empty1;
          if (!minolID.HasValue)
          {
            empty1 = string.Empty;
          }
          else
          {
            num = minolID.Value;
            empty1 = num.ToString("X8");
          }
          DbUtil.AddParameter((IDbCommand) cmd3, "@MinolID", empty1);
          DbCommand cmd4 = command;
          string empty2;
          if (!challengeKey.HasValue)
          {
            empty2 = string.Empty;
          }
          else
          {
            num = challengeKey.Value;
            empty2 = num.ToString("X8");
          }
          DbUtil.AddParameter((IDbCommand) cmd4, "@ChallengeKey", empty2);
          DbUtil.AddParameter((IDbCommand) command, "@SessionKey", sessionKey.HasValue ? sessionKey.Value.ToString("X16") : string.Empty);
          DbCommand cmd5 = command;
          string empty3;
          if (!challengeKeyOld.HasValue)
          {
            empty3 = string.Empty;
          }
          else
          {
            num = challengeKeyOld.Value;
            empty3 = num.ToString("X8");
          }
          DbUtil.AddParameter((IDbCommand) cmd5, "@ChallengeKeyOld", empty3);
          DbUtil.AddParameter((IDbCommand) command, "@SessionKeyOld", sessionKeyOld.HasValue ? sessionKeyOld.Value.ToString("X16") : string.Empty);
          DbCommand cmd6 = command;
          string empty4;
          if (!gsmIDEncoded.HasValue)
          {
            empty4 = string.Empty;
          }
          else
          {
            num = gsmIDEncoded.Value;
            empty4 = num.ToString("X8");
          }
          DbUtil.AddParameter((IDbCommand) cmd6, "@GsmIDEncoded", empty4);
          DbCommand cmd7 = command;
          string empty5;
          if (!challengeKeyEncoded.HasValue)
          {
            empty5 = string.Empty;
          }
          else
          {
            num = challengeKeyEncoded.Value;
            empty5 = num.ToString("X8");
          }
          DbUtil.AddParameter((IDbCommand) cmd7, "@ChallengeKeyEncoded", empty5);
          DbCommand cmd8 = command;
          string empty6;
          if (!gsmIDEncodedOld.HasValue)
          {
            empty6 = string.Empty;
          }
          else
          {
            num = gsmIDEncodedOld.Value;
            empty6 = num.ToString("X8");
          }
          DbUtil.AddParameter((IDbCommand) cmd8, "@GsmIDEncodedOld", empty6);
          DbCommand cmd9 = command;
          string empty7;
          if (!challengeKeyEncodedOld.HasValue)
          {
            empty7 = string.Empty;
          }
          else
          {
            num = challengeKeyEncodedOld.Value;
            empty7 = num.ToString("X8");
          }
          DbUtil.AddParameter((IDbCommand) cmd9, "@ChallengeKeyEncodedOld", empty7);
          if (obj != null)
          {
            DbCommand cmd10 = command;
            num = gsmID.Value;
            string str3 = num.ToString("X8");
            DbUtil.AddParameter((IDbCommand) cmd10, "@GsmID", str3);
          }
          if (command.ExecuteNonQuery() != 1)
            throw new Exception("Failed to save data to 'MinomatList' table! GSM ID: " + gsmID.ToString());
        }
      }
    }
  }
}
