// Decompiled with JetBrains decompiler
// Type: NHibernate.Type.IdentifierBagType
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
  public class IdentifierBagType(string role, string propertyRef, bool isEmbeddedInXML) : 
    CollectionType(role, propertyRef, isEmbeddedInXML)
  {
    public override IPersistentCollection Instantiate(
      ISessionImplementor session,
      ICollectionPersister persister,
      object key)
    {
      return (IPersistentCollection) new PersistentIdentifierBag(session);
    }

    public override System.Type ReturnedClass => typeof (ICollection);

    public override IPersistentCollection Wrap(ISessionImplementor session, object collection)
    {
      return (IPersistentCollection) new PersistentIdentifierBag(session, (ICollection) collection);
    }

    protected override void Clear(object collection) => ((IList) collection).Clear();

    protected override void Add(object collection, object element)
    {
      ((IList) collection).Add(element);
    }

    public override object Instantiate(int anticipatedSize)
    {
      return anticipatedSize > 0 ? (object) new ArrayList(anticipatedSize + 1) : (object) new ArrayList();
    }
  }
}
