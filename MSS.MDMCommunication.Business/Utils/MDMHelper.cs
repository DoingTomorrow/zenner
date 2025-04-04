// Decompiled with JetBrains decompiler
// Type: MSS.MDMCommunication.Business.Utils.MDMHelper
// Assembly: MSS.MDMCommunication.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CBA4B3BD-8D82-4E93-946D-7969F81D07F7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.MDMCommunication.Business.dll

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;

#nullable disable
namespace MSS.MDMCommunication.Business.Utils
{
  public static class MDMHelper
  {
    public static DataTable ToDataTable<T>(this IList<T> data)
    {
      PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof (T));
      DataTable dataTable = new DataTable();
      for (int index = 0; index < properties.Count; ++index)
      {
        PropertyDescriptor propertyDescriptor = properties[index];
        DataColumnCollection columns = dataTable.Columns;
        string name = propertyDescriptor.Name;
        Type type = Nullable.GetUnderlyingType(propertyDescriptor.PropertyType);
        if ((object) type == null)
          type = propertyDescriptor.PropertyType;
        columns.Add(name, type);
      }
      object[] objArray = new object[properties.Count];
      foreach (T component in (IEnumerable<T>) data)
      {
        for (int index = 0; index < objArray.Length; ++index)
          objArray[index] = properties[index].GetValue((object) component);
        dataTable.Rows.Add(objArray);
      }
      return dataTable;
    }
  }
}
