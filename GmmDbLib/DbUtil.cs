// Decompiled with JetBrains decompiler
// Type: GmmDbLib.DbUtil
// Assembly: GmmDbLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: FBABFE79-334C-44CD-A4BC-A66429DECD0D
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\GmmDbLib.dll

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using System.Text;

#nullable disable
namespace GmmDbLib
{
  public static class DbUtil
  {
    public static int GetInt32(this DbDataReader reader, string columnName)
    {
      int ordinal = reader.GetOrdinal(columnName);
      return !reader.IsDBNull(ordinal) ? reader.GetInt32(ordinal) : throw new Exception("Database has invalid data! Column: " + columnName + " can not be empty (DBNull).");
    }

    public static byte[] GetByteArray(this DbDataReader reader, string columnName)
    {
      int ordinal = reader.GetOrdinal(columnName);
      return !reader.IsDBNull(ordinal) ? (byte[]) reader.GetValue(ordinal) : throw new Exception("Database has invalid data! Column: " + columnName + " can not be empty (DBNull).");
    }

    public static uint GetUInt32(this DbDataReader reader, string columnName)
    {
      int ordinal = reader.GetOrdinal(columnName);
      return !reader.IsDBNull(ordinal) ? (uint) reader.GetInt32(ordinal) : throw new Exception("Database has invalid data! Column: " + columnName + " can not be empty (DBNull).");
    }

    public static string GetString(this DbDataReader reader, string columnName)
    {
      int ordinal = reader.GetOrdinal(columnName);
      return !reader.IsDBNull(ordinal) ? reader.GetString(ordinal) : throw new Exception("Database has invalid data! Column: " + columnName + " can not be empty (DBNull).");
    }

    public static DateTime? SafeGetDateTime(this DbDataReader reader, string columnName)
    {
      int ordinal = reader.GetOrdinal(columnName);
      return !reader.IsDBNull(ordinal) ? new DateTime?(reader.GetDateTime(ordinal)) : new DateTime?();
    }

    public static string SafeGetString(this DbDataReader reader, string columnName)
    {
      int ordinal = reader.GetOrdinal(columnName);
      return !reader.IsDBNull(ordinal) ? reader.GetString(ordinal) : string.Empty;
    }

    public static double SafeGetDouble(this DbDataReader reader, string columnName)
    {
      int ordinal = reader.GetOrdinal(columnName);
      return !reader.IsDBNull(ordinal) ? reader.GetDouble(ordinal) : 0.0;
    }

    public static void AddParameter(IDbCommand cmd, string parameterName, string value)
    {
      IDbDataParameter parameter = cmd.CreateParameter();
      parameter.DbType = DbType.String;
      parameter.ParameterName = parameterName;
      parameter.Value = (object) value;
      cmd.Parameters.Add((object) parameter);
    }

    public static void AddParameter(IDbCommand cmd, string parameterName, Guid value)
    {
      IDbDataParameter parameter = cmd.CreateParameter();
      parameter.DbType = DbType.Guid;
      parameter.ParameterName = parameterName;
      parameter.Value = (object) value;
      cmd.Parameters.Add((object) parameter);
    }

    public static void AddParameter(IDbCommand cmd, string parameterName, int value)
    {
      IDbDataParameter parameter = cmd.CreateParameter();
      parameter.ParameterName = parameterName;
      parameter.DbType = DbType.Int32;
      parameter.Value = (object) value;
      cmd.Parameters.Add((object) parameter);
    }

    public static void AddParameter(IDbCommand cmd, string parameterName, bool value)
    {
      IDbDataParameter parameter = cmd.CreateParameter();
      parameter.ParameterName = parameterName;
      parameter.DbType = DbType.Boolean;
      parameter.Value = (object) value;
      cmd.Parameters.Add((object) parameter);
    }

    public static void AddParameter(IDbCommand cmd, string parameterName, byte[] value)
    {
      IDbDataParameter parameter = cmd.CreateParameter();
      parameter.ParameterName = parameterName;
      parameter.DbType = DbType.Binary;
      if (value == null)
        parameter.Value = (object) DBNull.Value;
      else
        parameter.Value = (object) value;
      cmd.Parameters.Add((object) parameter);
    }

    public static void AddParameter(IDbCommand cmd, string parameterName, uint value)
    {
      IDbDataParameter parameter = cmd.CreateParameter();
      parameter.ParameterName = parameterName;
      parameter.DbType = DbType.Int32;
      parameter.Value = (object) value;
      cmd.Parameters.Add((object) parameter);
    }

    public static void AddParameter(IDbCommand cmd, string parameterName, DateTime? value)
    {
      if (value.HasValue)
      {
        DbUtil.AddParameter(cmd, parameterName, value.Value);
      }
      else
      {
        IDbDataParameter parameter = cmd.CreateParameter();
        parameter.ParameterName = parameterName;
        parameter.DbType = DbType.DateTime;
        parameter.Value = (object) DBNull.Value;
        cmd.Parameters.Add((object) parameter);
      }
    }

    public static void AddParameter(IDbCommand cmd, string parameterName, DateTime value)
    {
      IDbDataParameter parameter = cmd.CreateParameter();
      parameter.ParameterName = parameterName;
      parameter.DbType = DbType.DateTime;
      parameter.Value = (object) DbUtil.RemoveMilliseconds(value);
      cmd.Parameters.Add((object) parameter);
    }

    public static DateTime RemoveMilliseconds(DateTime dateTime)
    {
      return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second);
    }

    public static string ByteArrayToHexString(byte[] buffer)
    {
      return buffer == null ? string.Empty : DbUtil.ByteArrayToHexString(buffer, 0, buffer.Length);
    }

    public static string ByteArrayToHexString(byte[] buffer, int startIndex)
    {
      return buffer == null ? string.Empty : DbUtil.ByteArrayToHexString(buffer, startIndex, buffer.Length - startIndex);
    }

    public static string ByteArrayToHexString(byte[] buffer, int startIndex, int length)
    {
      if (buffer == null)
        return string.Empty;
      char[] chArray = new char[length * 2];
      int num1 = 0;
      int index = 0;
      while (num1 < length)
      {
        byte num2 = (byte) ((uint) buffer[startIndex + num1] >> 4);
        chArray[index] = num2 > (byte) 9 ? (char) ((int) num2 + 55) : (char) ((int) num2 + 48);
        byte num3 = (byte) ((uint) buffer[startIndex + num1] & 15U);
        int num4;
        chArray[num4 = index + 1] = num3 > (byte) 9 ? (char) ((int) num3 + 55) : (char) ((int) num3 + 48);
        ++num1;
        index = num4 + 1;
      }
      return new string(chArray, 0, chArray.Length);
    }

    public static SortedList<string, string> KeyValueStringListToSortedList(
      string keyValueStringList)
    {
      SortedList<string, string> sortedList = new SortedList<string, string>();
      string[] strArray1 = keyValueStringList.Split(new char[1]
      {
        ';'
      }, StringSplitOptions.RemoveEmptyEntries);
      if (strArray1 != null)
      {
        foreach (string str in strArray1)
        {
          if (!string.IsNullOrEmpty(str))
          {
            string[] strArray2 = str.Split(new char[1]
            {
              '='
            }, StringSplitOptions.RemoveEmptyEntries);
            if (strArray2 != null)
            {
              if (strArray2.Length == 1)
                sortedList.Add(strArray2[0], (string) null);
              else if (strArray2.Length > 1)
                sortedList.Add(strArray2[0], strArray2[1]);
            }
          }
        }
      }
      return sortedList;
    }

    public static string SortedListToKeyValueStringList(SortedList<string, string> keyValueList)
    {
      StringBuilder stringBuilder = new StringBuilder();
      if (keyValueList != null && keyValueList.Count > 0)
      {
        foreach (KeyValuePair<string, string> keyValue in keyValueList)
        {
          string str1 = keyValue.Key.Trim();
          if (str1.Length < 1)
            throw new Exception("Illegal empty key");
          stringBuilder.Append(";" + str1);
          string str2 = (string) null;
          if (keyValue.Value != null)
            str2 = keyValue.Value.Trim();
          if (!string.IsNullOrEmpty(str2))
            stringBuilder.Append("=" + str2);
        }
        stringBuilder.Append(';');
      }
      return stringBuilder.ToString();
    }

    public static List<KeyValuePair<string, string>> KeyValueStringListToKeyValuePairList(
      string keyValueStringList)
    {
      List<KeyValuePair<string, string>> keyValuePairList = new List<KeyValuePair<string, string>>();
      string[] strArray1 = keyValueStringList.Split(new char[1]
      {
        ';'
      }, StringSplitOptions.RemoveEmptyEntries);
      if (strArray1 != null)
      {
        foreach (string str in strArray1)
        {
          if (!string.IsNullOrEmpty(str))
          {
            string[] strArray2 = str.Split(new char[1]
            {
              '='
            }, StringSplitOptions.RemoveEmptyEntries);
            if (strArray2 != null)
            {
              if (strArray2.Length == 1)
                keyValuePairList.Add(new KeyValuePair<string, string>(strArray2[0], (string) null));
              else if (strArray2.Length > 1)
                keyValuePairList.Add(new KeyValuePair<string, string>(strArray2[0], strArray2[1]));
            }
          }
        }
      }
      return keyValuePairList;
    }

    public static string KeyValuePairListToKeyValueStringList(
      List<KeyValuePair<string, string>> keyValueList)
    {
      List<KeyValuePair<string, string>> list = keyValueList.OrderBy<KeyValuePair<string, string>, string>((System.Func<KeyValuePair<string, string>, string>) (x => x.Key)).ToList<KeyValuePair<string, string>>();
      StringBuilder stringBuilder = new StringBuilder();
      if (list != null && list.Count > 0)
      {
        foreach (KeyValuePair<string, string> keyValuePair in list)
        {
          string str1 = keyValuePair.Key.Trim();
          if (str1.Length < 1)
            throw new Exception("Illegal empty key");
          stringBuilder.Append(";" + str1);
          string str2 = (string) null;
          if (keyValuePair.Value != null)
            str2 = keyValuePair.Value.Trim();
          if (!string.IsNullOrEmpty(str2))
            stringBuilder.Append("=" + str2);
        }
        stringBuilder.Append(';');
      }
      return stringBuilder.ToString();
    }

    public static string GetValueForKey(string key, SortedList<string, string> keyValueList)
    {
      int index = keyValueList.IndexOfKey(key);
      return index < 0 ? (string) null : keyValueList.Values[index];
    }

    public static string GetValueForKey(
      string key,
      List<KeyValuePair<string, string>> keyValuePairList)
    {
      List<KeyValuePair<string, string>> all = keyValuePairList.FindAll((Predicate<KeyValuePair<string, string>>) (x => x.Key == key));
      if (all.Count == 0)
        return (string) null;
      StringBuilder stringBuilder = new StringBuilder();
      foreach (KeyValuePair<string, string> keyValuePair in all)
        stringBuilder.Append(";" + keyValuePair.Value);
      stringBuilder.Append(";");
      return stringBuilder.ToString();
    }

    public static string GetOneValueForKey(
      string key,
      List<KeyValuePair<string, string>> keyValuePairList)
    {
      List<KeyValuePair<string, string>> all = keyValuePairList.FindAll((Predicate<KeyValuePair<string, string>>) (x => x.Key == key));
      if (all.Count == 0)
        throw new Exception("Value for key: '" + key + "' not available");
      return all.Count <= 1 ? all[0].Value : throw new Exception("Value for key: '" + key + "' not unique");
    }

    public static bool IsKeyAvailable(
      string key,
      List<KeyValuePair<string, string>> keyValuePairList)
    {
      return keyValuePairList.FindAll((Predicate<KeyValuePair<string, string>>) (x => x.Key == key)).Count > 0;
    }

    public static double? GetDoubleForKey(
      string key,
      List<KeyValuePair<string, string>> keyValuePairList)
    {
      List<KeyValuePair<string, string>> all = keyValuePairList.FindAll((Predicate<KeyValuePair<string, string>>) (x => x.Key == key));
      if (all.Count == 0)
        return new double?();
      if (all.Count > 1)
        throw new Exception("Value for key: '" + key + "' not unique");
      return new double?(double.Parse(all[0].Value, (IFormatProvider) CultureInfo.InvariantCulture));
    }

    public static ushort? GetUshortForKey(
      string key,
      List<KeyValuePair<string, string>> keyValuePairList)
    {
      List<KeyValuePair<string, string>> all = keyValuePairList.FindAll((Predicate<KeyValuePair<string, string>>) (x => x.Key == key));
      if (all.Count == 0)
        return new ushort?();
      return all.Count <= 1 ? new ushort?(ushort.Parse(all[0].Value)) : throw new Exception("Value for key: '" + key + "' not unique");
    }

    public static ushort? GetHexUshortForKey(
      string key,
      List<KeyValuePair<string, string>> keyValuePairList)
    {
      List<KeyValuePair<string, string>> all = keyValuePairList.FindAll((Predicate<KeyValuePair<string, string>>) (x => x.Key == key));
      if (all.Count == 0)
        return new ushort?();
      return all.Count <= 1 ? new ushort?(ushort.Parse(all[0].Value, NumberStyles.HexNumber)) : throw new Exception("Value for key: '" + key + "' not unique");
    }
  }
}
