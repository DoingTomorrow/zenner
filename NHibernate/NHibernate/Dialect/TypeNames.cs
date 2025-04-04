// Decompiled with JetBrains decompiler
// Type: NHibernate.Dialect.TypeNames
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Util;
using System;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace NHibernate.Dialect
{
  public class TypeNames
  {
    public const string LengthPlaceHolder = "$l";
    public const string PrecisionPlaceHolder = "$p";
    public const string ScalePlaceHolder = "$s";
    private readonly Dictionary<DbType, SortedList<int, string>> weighted = new Dictionary<DbType, SortedList<int, string>>();
    private readonly Dictionary<DbType, string> defaults = new Dictionary<DbType, string>();

    public string Get(DbType typecode)
    {
      string str;
      if (!this.defaults.TryGetValue(typecode, out str))
        throw new ArgumentException("Dialect does not support DbType." + (object) typecode, nameof (typecode));
      return str;
    }

    public string Get(DbType typecode, int size, int precision, int scale)
    {
      SortedList<int, string> sortedList;
      this.weighted.TryGetValue(typecode, out sortedList);
      if (sortedList != null && sortedList.Count > 0)
      {
        foreach (KeyValuePair<int, string> keyValuePair in sortedList)
        {
          if (size <= keyValuePair.Key)
            return TypeNames.Replace(keyValuePair.Value, size, precision, scale);
        }
      }
      return TypeNames.Replace(this.Get(typecode), size, precision, scale);
    }

    public string GetLongest(DbType typecode)
    {
      SortedList<int, string> sortedList;
      this.weighted.TryGetValue(typecode, out sortedList);
      return sortedList != null && sortedList.Count > 0 ? TypeNames.Replace(sortedList.Values[sortedList.Count - 1], sortedList.Keys[sortedList.Count - 1], 0, 0) : this.Get(typecode);
    }

    private static string Replace(string type, int size, int precision, int scale)
    {
      type = StringHelper.ReplaceOnce(type, "$l", size.ToString());
      type = StringHelper.ReplaceOnce(type, "$s", scale.ToString());
      return StringHelper.ReplaceOnce(type, "$p", precision.ToString());
    }

    public void Put(DbType typecode, int capacity, string value)
    {
      SortedList<int, string> sortedList;
      if (!this.weighted.TryGetValue(typecode, out sortedList))
        this.weighted[typecode] = sortedList = new SortedList<int, string>();
      sortedList[capacity] = value;
    }

    public void Put(DbType typecode, string value) => this.defaults[typecode] = value;
  }
}
