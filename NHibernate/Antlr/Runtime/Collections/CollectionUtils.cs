// Decompiled with JetBrains decompiler
// Type: Antlr.Runtime.Collections.CollectionUtils
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System.Collections;
using System.Text;

#nullable disable
namespace Antlr.Runtime.Collections
{
  internal class CollectionUtils
  {
    public static string ListToString(IList coll)
    {
      StringBuilder stringBuilder = new StringBuilder();
      if (coll != null)
      {
        stringBuilder.Append("[");
        for (int index = 0; index < coll.Count; ++index)
        {
          if (index > 0)
            stringBuilder.Append(", ");
          object obj = coll[index];
          switch (obj)
          {
            case null:
              stringBuilder.Append("null");
              break;
            case IDictionary _:
              stringBuilder.Append(CollectionUtils.DictionaryToString((IDictionary) obj));
              break;
            case IList _:
              stringBuilder.Append(CollectionUtils.ListToString((IList) obj));
              break;
            default:
              stringBuilder.Append(obj.ToString());
              break;
          }
        }
        stringBuilder.Append("]");
      }
      else
        stringBuilder.Insert(0, "null");
      return stringBuilder.ToString();
    }

    public static string DictionaryToString(IDictionary dict)
    {
      StringBuilder stringBuilder = new StringBuilder();
      if (dict != null)
      {
        stringBuilder.Append("{");
        int num = 0;
        foreach (DictionaryEntry dictionaryEntry in dict)
        {
          if (num > 0)
            stringBuilder.Append(", ");
          if (dictionaryEntry.Value is IDictionary)
            stringBuilder.AppendFormat("{0}={1}", (object) dictionaryEntry.Key.ToString(), (object) CollectionUtils.DictionaryToString((IDictionary) dictionaryEntry.Value));
          else if (dictionaryEntry.Value is IList)
            stringBuilder.AppendFormat("{0}={1}", (object) dictionaryEntry.Key.ToString(), (object) CollectionUtils.ListToString((IList) dictionaryEntry.Value));
          else
            stringBuilder.AppendFormat("{0}={1}", (object) dictionaryEntry.Key.ToString(), (object) dictionaryEntry.Value.ToString());
          ++num;
        }
        stringBuilder.Append("}");
      }
      else
        stringBuilder.Insert(0, "null");
      return stringBuilder.ToString();
    }
  }
}
