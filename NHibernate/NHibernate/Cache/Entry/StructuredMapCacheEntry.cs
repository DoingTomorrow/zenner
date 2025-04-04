// Decompiled with JetBrains decompiler
// Type: NHibernate.Cache.Entry.StructuredMapCacheEntry
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using System.Collections;

#nullable disable
namespace NHibernate.Cache.Entry
{
  public class StructuredMapCacheEntry : ICacheEntryStructure
  {
    public object Structure(object item)
    {
      object[] state = ((CollectionCacheEntry) item).State;
      IDictionary dictionary1 = (IDictionary) new Hashtable(state.Length);
      int num1 = 0;
      while (num1 < state.Length)
      {
        IDictionary dictionary2 = dictionary1;
        object[] objArray1 = state;
        int index1 = num1;
        int num2 = index1 + 1;
        object key = objArray1[index1];
        object[] objArray2 = state;
        int index2 = num2;
        num1 = index2 + 1;
        object obj = objArray2[index2];
        dictionary2[key] = obj;
      }
      return (object) dictionary1;
    }

    public object Destructure(object item, ISessionFactoryImplementor factory)
    {
      IDictionary dictionary = (IDictionary) item;
      object[] state = new object[dictionary.Count * 2];
      int num1 = 0;
      foreach (DictionaryEntry dictionaryEntry in dictionary)
      {
        object[] objArray1 = state;
        int index1 = num1;
        int num2 = index1 + 1;
        object key = dictionaryEntry.Key;
        objArray1[index1] = key;
        object[] objArray2 = state;
        int index2 = num2;
        num1 = index2 + 1;
        object obj = dictionaryEntry.Value;
        objArray2[index2] = obj;
      }
      return (object) new CollectionCacheEntry((object) state);
    }
  }
}
