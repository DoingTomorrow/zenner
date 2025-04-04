// Decompiled with JetBrains decompiler
// Type: NHibernate.Type.SetType
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Iesi.Collections;
using NHibernate.Collection;
using NHibernate.Engine;
using NHibernate.Persister.Collection;
using System;

#nullable disable
namespace NHibernate.Type
{
  [Serializable]
  public class SetType(string role, string propertyRef, bool isEmbeddedInXML) : CollectionType(role, propertyRef, isEmbeddedInXML)
  {
    public override IPersistentCollection Instantiate(
      ISessionImplementor session,
      ICollectionPersister persister,
      object key)
    {
      return (IPersistentCollection) new PersistentSet(session);
    }

    public override System.Type ReturnedClass => typeof (ISet);

    public override IPersistentCollection Wrap(ISessionImplementor session, object collection)
    {
      return (IPersistentCollection) new PersistentSet(session, (ISet) collection);
    }

    protected override void Add(object collection, object element)
    {
      ((ISet) collection).Add(element);
    }

    protected override void Clear(object collection) => ((ISet) collection).Clear();

    public override object Instantiate(int anticipatedSize) => (object) new HashedSet();
  }
}
