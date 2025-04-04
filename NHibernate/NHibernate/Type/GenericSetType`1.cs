// Decompiled with JetBrains decompiler
// Type: NHibernate.Type.GenericSetType`1
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Iesi.Collections.Generic;
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
  public class GenericSetType<T>(string role, string propertyRef) : SetType(role, propertyRef, false)
  {
    public override IPersistentCollection Instantiate(
      ISessionImplementor session,
      ICollectionPersister persister,
      object key)
    {
      return (IPersistentCollection) new PersistentGenericSet<T>(session);
    }

    public override System.Type ReturnedClass => typeof (ISet<T>);

    public override IPersistentCollection Wrap(ISessionImplementor session, object collection)
    {
      switch (collection)
      {
        case ISet<T> original:
label_3:
          return (IPersistentCollection) new PersistentGenericSet<T>(session, original);
        case ICollection<T> initialValues:
          original = (ISet<T>) new HashedSet<T>(initialValues);
          goto label_3;
        default:
          throw new HibernateException(this.Role + " must be an implementation of ISet<T> or ICollection<T>");
      }
    }

    public override object Instantiate(int anticipatedSize) => (object) new HashedSet<T>();
  }
}
