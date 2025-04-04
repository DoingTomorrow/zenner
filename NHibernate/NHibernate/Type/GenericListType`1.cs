// Decompiled with JetBrains decompiler
// Type: NHibernate.Type.GenericListType`1
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Collection;
using NHibernate.Collection.Generic;
using NHibernate.Engine;
using NHibernate.Persister.Collection;
using System;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Type
{
  [Serializable]
  public class GenericListType<T>(string role, string propertyRef) : ListType(role, propertyRef, false)
  {
    public override IPersistentCollection Instantiate(
      ISessionImplementor session,
      ICollectionPersister persister,
      object key)
    {
      return (IPersistentCollection) new PersistentGenericList<T>(session);
    }

    public override System.Type ReturnedClass => typeof (IList<T>);

    public override IPersistentCollection Wrap(ISessionImplementor session, object collection)
    {
      return (IPersistentCollection) new PersistentGenericList<T>(session, (IList<T>) collection);
    }

    public override object Instantiate(int anticipatedSize)
    {
      return anticipatedSize > 0 ? (object) new List<T>(anticipatedSize + 1) : (object) new List<T>();
    }
  }
}
