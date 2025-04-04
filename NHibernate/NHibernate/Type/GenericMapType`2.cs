// Decompiled with JetBrains decompiler
// Type: NHibernate.Type.GenericMapType`2
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Collection;
using NHibernate.Collection.Generic;
using NHibernate.Engine;
using NHibernate.Persister.Collection;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Type
{
  [Serializable]
  public class GenericMapType<TKey, TValue>(string role, string propertyRef) : MapType(role, propertyRef, false)
  {
    public override IPersistentCollection Instantiate(
      ISessionImplementor session,
      ICollectionPersister persister,
      object key)
    {
      return (IPersistentCollection) new PersistentGenericMap<TKey, TValue>(session);
    }

    public override System.Type ReturnedClass => typeof (IDictionary<TKey, TValue>);

    public override IPersistentCollection Wrap(ISessionImplementor session, object collection)
    {
      return (IPersistentCollection) new PersistentGenericMap<TKey, TValue>(session, (IDictionary<TKey, TValue>) collection);
    }

    protected override void Add(object collection, object element)
    {
      ((ICollection<KeyValuePair<TKey, TValue>>) collection).Add((KeyValuePair<TKey, TValue>) element);
    }

    public override object ReplaceElements(
      object original,
      object target,
      object owner,
      IDictionary copyCache,
      ISessionImplementor session)
    {
      ICollectionPersister collectionPersister = session.Factory.GetCollectionPersister(this.Role);
      IDictionary<TKey, TValue> dictionary = (IDictionary<TKey, TValue>) target;
      dictionary.Clear();
      foreach (KeyValuePair<TKey, TValue> keyValuePair in (IEnumerable<KeyValuePair<TKey, TValue>>) original)
      {
        TKey key = (TKey) collectionPersister.IndexType.Replace((object) keyValuePair.Key, (object) null, session, owner, copyCache);
        TValue obj = (TValue) collectionPersister.ElementType.Replace((object) keyValuePair.Value, (object) null, session, owner, copyCache);
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
      return anticipatedSize > 0 ? (object) new Dictionary<TKey, TValue>(anticipatedSize + 1) : (object) new Dictionary<TKey, TValue>();
    }
  }
}
