// Decompiled with JetBrains decompiler
// Type: MSS.Business.Utils.DataTableHelper
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

#nullable disable
namespace MSS.Business.Utils
{
  public static class DataTableHelper
  {
    public static List<TSource> ToList<TSource>(this DataTable dataTable) where TSource : new()
    {
      List<TSource> list1 = new List<TSource>();
      List<\u003C\u003Ef__AnonymousType2<string, Type>> list2 = Enumerable.Cast<PropertyInfo>(typeof (TSource).GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)).Select(aProp =>
      {
        string name = aProp.Name;
        Type type = Nullable.GetUnderlyingType(aProp.PropertyType);
        if ((object) type == null)
          type = aProp.PropertyType;
        return new{ Name = name, Type = type };
      }).ToList();
      foreach (DataRow dataRow in dataTable.AsEnumerable().ToList<DataRow>())
      {
        TSource source = new TSource();
        foreach (var data in list2)
        {
          PropertyInfo property = source.GetType().GetProperty(data.Name);
          string str = dataRow[data.Name] == DBNull.Value ? (string) null : dataRow[data.Name].ToString();
          property.SetValue((object) source, ConversionHelper.ConvertValue(data.Type, str), (object[]) null);
        }
        list1.Add(source);
      }
      return list1;
    }
  }
}
