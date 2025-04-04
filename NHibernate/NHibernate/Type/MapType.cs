// Decompiled with JetBrains decompiler
// Type: NHibernate.Type.MapType
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Collection;
using NHibernate.Engine;
using NHibernate.Persister.Collection;
using System;
using System.Collections;

#nullable disable
namespace NHibernate.Type
{
  [Serializable]
  public class MapType(string role, string propertyRef, bool isEmbeddedInXML) : CollectionType(role, propertyRef, isEmbeddedInXML)
  {
    public override IPersistentCollection Instantiate(
      ISessionImplementor session,
      ICollectionPersister persister,
      object key)
    {
      return (IPersistentCollection) new PersistentMap(session);
    }

    public override System.Type ReturnedClass => typeof (IDictionary);

    public override IEnumerable GetElementsIterator(object collection)
    {
      return (IEnumerable) ((IDictionary) collection).Values;
    }

    public override IPersistentCollection Wrap(ISessionImplementor session, object collection)
    {
      return (IPersistentCollection) new PersistentMap(session, (IDictionary) collection);
    }

    protected override void Add(object collection, object element)
    {
      DictionaryEntry dictionaryEntry = (DictionaryEntry) element;
      ((IDictionary) collection).Add(dictionaryEntry.Key, dictionaryEntry.Value);
    }

    protected override void Clear(object collection) => ((IDictionary) collection).Clear();

    public override object ReplaceElements(
      object original,
      object target,
      object owner,
      IDictionary copyCache,
      ISessionImplementor session)
    {
      ICollectionPersister collectionPersister = session.Factory.GetCollectionPersister(this.Role);
      IDictionary dictionary = (IDictionary) target;
      dictionary.Clear();
      foreach (DictionaryEntry dictionaryEntry in (IEnumerable) original)
      {
        object key = collectionPersister.IndexType.Replace(dictionaryEntry.Key, (object) null, session, owner, copyCache);
        object obj = collectionPersister.ElementType.Replace(dictionaryEntry.Value, (object) null, session, owner, copyCache);
        dictionary[key] = obj;
      }
      IPersistentCollection persistentCollection1 = original as IPersistentCollection;
      IPersistentCollection persistentCollection2 = dictionary as IPersistentCollection;
      if (persistentCollection1 != null && persistentCollection2 != null && !persistentCollection1.IsDirty)
        persistentCollection2.ClearDirty();
      return (object) dictionary;
    }

    public override object Instantiate(int anticipatedSize)
    {
      return anticipatedSize > 0 ? (object) new Hashtable(anticipatedSize + 1) : (object) new Hashtable();
    }

    public override object IndexOf(object collection, object element)
    {
      IDictionaryEnumerator enumerator = ((IDictionary) collection).GetEnumerator();
      while (enumerator.MoveNext())
      {
        if (enumerator.Value == element)
          return enumerator.Key;
      }
      return (object) null;
    }
  }
}
