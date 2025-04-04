// Decompiled with JetBrains decompiler
// Type: NHibernate.Util.ArrayHelper
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.SqlTypes;
using NHibernate.Type;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

#nullable disable
namespace NHibernate.Util
{
  public static class ArrayHelper
  {
    public static readonly object[] EmptyObjectArray = new object[0];
    public static readonly IType[] EmptyTypeArray = new IType[0];
    public static readonly int[] EmptyIntArray = new int[0];
    public static readonly bool[] EmptyBoolArray = new bool[0];
    public static readonly bool[] True = new bool[1]{ true };
    public static readonly bool[] False = new bool[1];

    public static bool IsAllNegative(int[] array)
    {
      for (int index = 0; index < array.Length; ++index)
      {
        if (array[index] >= 0)
          return false;
      }
      return true;
    }

    public static string[] ToStringArray(object[] objects)
    {
      return (string[]) ArrayHelper.ToArray((ICollection) objects, typeof (string));
    }

    public static string[] FillArray(string str, int length)
    {
      string[] strArray = new string[length];
      for (int index = 0; index < length; ++index)
        strArray[index] = str;
      return strArray;
    }

    public static LockMode[] FillArray(LockMode lockMode, int length)
    {
      LockMode[] lockModeArray = new LockMode[length];
      for (int index = 0; index < length; ++index)
        lockModeArray[index] = lockMode;
      return lockModeArray;
    }

    public static IType[] FillArray(IType type, int length)
    {
      IType[] typeArray = new IType[length];
      for (int index = 0; index < length; ++index)
        typeArray[index] = type;
      return typeArray;
    }

    public static void Fill<T>(T[] array, T value)
    {
      for (int index = 0; index < array.Length; ++index)
        array[index] = value;
    }

    public static int[] ToIntArray(ICollection coll)
    {
      return (int[]) ArrayHelper.ToArray(coll, typeof (int));
    }

    public static bool[] ToBooleanArray(ICollection col)
    {
      return (bool[]) ArrayHelper.ToArray(col, typeof (bool));
    }

    public static string[] Slice(string[] strings, int begin, int length)
    {
      string[] destinationArray = new string[length];
      Array.Copy((Array) strings, begin, (Array) destinationArray, 0, length);
      return destinationArray;
    }

    public static object[] Slice(object[] objects, int begin, int length)
    {
      object[] destinationArray = new object[length];
      Array.Copy((Array) objects, begin, (Array) destinationArray, 0, length);
      return destinationArray;
    }

    public static T[] Join<T>(T[] x, T[] y, bool[] use)
    {
      List<T> objList = new List<T>((IEnumerable<T>) x);
      for (int index = 0; index < y.Length; ++index)
      {
        if (use[index])
          objList.Add(y[index]);
      }
      return objList.ToArray();
    }

    public static string[] Join(string[] x, string[] y)
    {
      string[] destinationArray = new string[x.Length + y.Length];
      Array.Copy((Array) x, 0, (Array) destinationArray, 0, x.Length);
      Array.Copy((Array) y, 0, (Array) destinationArray, x.Length, y.Length);
      return destinationArray;
    }

    public static DbType[] Join(DbType[] x, DbType[] y)
    {
      DbType[] destinationArray = new DbType[x.Length + y.Length];
      Array.Copy((Array) x, 0, (Array) destinationArray, 0, x.Length);
      Array.Copy((Array) y, 0, (Array) destinationArray, x.Length, y.Length);
      return destinationArray;
    }

    public static SqlType[] Join(SqlType[] x, SqlType[] y)
    {
      SqlType[] destinationArray = new SqlType[x.Length + y.Length];
      Array.Copy((Array) x, 0, (Array) destinationArray, 0, x.Length);
      Array.Copy((Array) y, 0, (Array) destinationArray, x.Length, y.Length);
      return destinationArray;
    }

    public static object[] Join(object[] x, object[] y)
    {
      object[] destinationArray = new object[x.Length + y.Length];
      Array.Copy((Array) x, 0, (Array) destinationArray, 0, x.Length);
      Array.Copy((Array) y, 0, (Array) destinationArray, x.Length, y.Length);
      return destinationArray;
    }

    public static bool IsAllFalse(bool[] array) => Array.IndexOf<bool>(array, true) < 0;

    public static string[][] To2DStringArray(ICollection coll)
    {
      string[][] strArray = new string[coll.Count][];
      int index = 0;
      foreach (object obj1 in (IEnumerable) coll)
      {
        if (obj1 is ICollection collection)
        {
          strArray[index] = new string[collection.Count];
          int num = 0;
          foreach (object obj2 in (IEnumerable) collection)
            strArray[index][num++] = obj2 == null ? (string) null : (string) obj2;
        }
        else
        {
          strArray[index] = new string[1];
          strArray[index][0] = obj1 == null ? (string) null : (string) obj1;
        }
        ++index;
      }
      return strArray;
    }

    public static string ToString(object[] array)
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("[");
      for (int index = 0; index < array.Length; ++index)
      {
        stringBuilder.Append(array[index]);
        if (index < array.Length - 1)
          stringBuilder.Append(",");
      }
      stringBuilder.Append("]");
      return stringBuilder.ToString();
    }

    public static void AddAll(IList to, IList from)
    {
      Action action = (Action) null;
      foreach (object obj in (IEnumerable) from)
      {
        if (obj == null)
        {
          if (action == null)
          {
            if (to.GetType().IsGenericType && to.GetType().GetGenericTypeDefinition() == typeof (List<>) && to.GetType().GetGenericArguments()[0].IsGenericType && to.GetType().GetGenericArguments()[0].GetGenericTypeDefinition() == typeof (Nullable<>))
            {
              MethodInfo method = to.GetType().GetMethod("Add");
              action = (Action) Expression.Lambda((Expression) Expression.Call((Expression) Expression.Constant((object) to), method, (Expression) Expression.Constant((object) null, to.GetType().GetGenericArguments()[0]))).Compile();
            }
            else
              action = (Action) (() => to.Add((object) null));
          }
          action();
        }
        else
          to.Add(obj);
      }
    }

    public static void AddAll(IDictionary to, IDictionary from)
    {
      foreach (DictionaryEntry dictionaryEntry in from)
        to[dictionaryEntry.Key] = dictionaryEntry.Value;
    }

    public static void AddAll<TKey, TValue>(
      IDictionary<TKey, TValue> to,
      IDictionary<TKey, TValue> from)
    {
      foreach (KeyValuePair<TKey, TValue> keyValuePair in (IEnumerable<KeyValuePair<TKey, TValue>>) from)
        to[keyValuePair.Key] = keyValuePair.Value;
    }

    public static IDictionary<TKey, TValue> AddOrOverride<TKey, TValue>(
      this IDictionary<TKey, TValue> destination,
      IDictionary<TKey, TValue> sourceOverride)
    {
      foreach (KeyValuePair<TKey, TValue> keyValuePair in (IEnumerable<KeyValuePair<TKey, TValue>>) sourceOverride)
        destination[keyValuePair.Key] = keyValuePair.Value;
      return destination;
    }

    public static int[] GetBatchSizes(int maxBatchSize)
    {
      int batchSize1 = maxBatchSize;
      int length = 1;
      while (batchSize1 > 1)
      {
        batchSize1 = ArrayHelper.GetNextBatchSize(batchSize1);
        ++length;
      }
      int[] batchSizes = new int[length];
      int batchSize2 = maxBatchSize;
      for (int index = 0; index < length; ++index)
      {
        batchSizes[index] = batchSize2;
        batchSize2 = ArrayHelper.GetNextBatchSize(batchSize2);
      }
      return batchSizes;
    }

    private static int GetNextBatchSize(int batchSize)
    {
      if (batchSize <= 10)
        return batchSize - 1;
      return batchSize / 2 < 10 ? 10 : batchSize / 2;
    }

    public static IType[] ToTypeArray(IList list)
    {
      IType[] typeArray = new IType[list.Count];
      list.CopyTo((Array) typeArray, 0);
      return typeArray;
    }

    private static void ExpandWithNulls(IList list, int requiredLength)
    {
      while (list.Count < requiredLength)
        list.Add((object) null);
    }

    public static void SafeSetValue(IList list, int index, object value)
    {
      ArrayHelper.ExpandWithNulls(list, index + 1);
      list[index] = value;
    }

    public static string[] ToStringArray(ICollection coll)
    {
      return (string[]) ArrayHelper.ToArray(coll, typeof (string));
    }

    public static string[] ToStringArray(ICollection<string> coll)
    {
      return new List<string>((IEnumerable<string>) coll).ToArray();
    }

    public static SqlType[] ToSqlTypeArray(ICollection coll)
    {
      return (SqlType[]) ArrayHelper.ToArray(coll, typeof (SqlType));
    }

    public static Array ToArray(ICollection coll, System.Type elementType)
    {
      Array instance = Array.CreateInstance(elementType, coll.Count);
      coll.CopyTo(instance, 0);
      return instance;
    }

    public static int CountTrue(bool[] array)
    {
      int num = 0;
      for (int index = 0; index < array.Length; ++index)
      {
        if (array[index])
          ++num;
      }
      return num;
    }

    public static bool ArrayEquals(SqlType[] a, SqlType[] b)
    {
      if (a.Length != b.Length)
        return false;
      for (int index = 0; index < a.Length; ++index)
      {
        if (!object.Equals((object) a[index], (object) b[index]))
          return false;
      }
      return true;
    }

    public static bool ArrayEquals(byte[] a, byte[] b)
    {
      if (a.Length != b.Length)
        return false;
      int index = 0;
      for (int length = a.Length; index < length; ++index)
      {
        if ((int) a[index] != (int) b[index])
          return false;
      }
      return true;
    }
  }
}
