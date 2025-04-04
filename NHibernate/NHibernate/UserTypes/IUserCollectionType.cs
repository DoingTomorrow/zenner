// Decompiled with JetBrains decompiler
// Type: NHibernate.UserTypes.IUserCollectionType
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Collection;
using NHibernate.Engine;
using NHibernate.Persister.Collection;
using System.Collections;

#nullable disable
namespace NHibernate.UserTypes
{
  public interface IUserCollectionType
  {
    IPersistentCollection Instantiate(ISessionImplementor session, ICollectionPersister persister);

    IPersistentCollection Wrap(ISessionImplementor session, object collection);

    IEnumerable GetElements(object collection);

    bool Contains(object collection, object entity);

    object IndexOf(object collection, object entity);

    object ReplaceElements(
      object original,
      object target,
      ICollectionPersister persister,
      object owner,
      IDictionary copyCache,
      ISessionImplementor session);

    object Instantiate(int anticipatedSize);
  }
}
