// Decompiled with JetBrains decompiler
// Type: NHibernate.Util.CollectionPrinter
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Iesi.Collections;
using System.Collections;
using System.Collections.Generic;
using System.Text;

#nullable disable
namespace NHibernate.Util
{
  public sealed class CollectionPrinter
  {
    private static void AppendNullOrValue(StringBuilder builder, object value)
    {
      if (value == null)
        builder.Append("null");
      else
        builder.Append("'").Append(value).Append("'");
    }

    public static string ToString(IDictionary dictionary)
    {
      StringBuilder builder = new StringBuilder();
      builder.Append("{");
      bool flag = true;
      foreach (DictionaryEntry dictionaryEntry in dictionary)
      {
        if (!flag)
          builder.Append(", ");
        CollectionPrinter.AppendNullOrValue(builder, dictionaryEntry.Key);
        builder.Append("=");
        CollectionPrinter.AppendNullOrValue(builder, dictionaryEntry.Value);
        flag = false;
      }
      builder.Append("}");
      return builder.ToString();
    }

    public static string ToString(IDictionary<string, string> dictionary)
    {
      StringBuilder builder = new StringBuilder();
      builder.Append("{");
      bool flag = true;
      foreach (KeyValuePair<string, string> keyValuePair in (IEnumerable<KeyValuePair<string, string>>) dictionary)
      {
        if (!flag)
          builder.Append(", ");
        CollectionPrinter.AppendNullOrValue(builder, (object) keyValuePair.Key);
        builder.Append("=");
        CollectionPrinter.AppendNullOrValue(builder, (object) keyValuePair.Value);
        flag = false;
      }
      builder.Append("}");
      return builder.ToString();
    }

    private static string ICollectionToString(ICollection collection)
    {
      StringBuilder builder = new StringBuilder();
      builder.Append("[");
      bool flag = true;
      foreach (object obj in (IEnumerable) collection)
      {
        if (!flag)
          builder.Append(", ");
        CollectionPrinter.AppendNullOrValue(builder, obj);
        flag = false;
      }
      builder.Append("]");
      return builder.ToString();
    }

    public static string ToString(IList list)
    {
      return CollectionPrinter.ICollectionToString((ICollection) list);
    }

    public static string ToString(ISet set)
    {
      return CollectionPrinter.ICollectionToString((ICollection) set);
    }
  }
}
