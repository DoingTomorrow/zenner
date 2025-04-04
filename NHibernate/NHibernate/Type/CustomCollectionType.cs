// Decompiled with JetBrains decompiler
// Type: NHibernate.Type.CustomCollectionType
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Collection;
using NHibernate.Engine;
using NHibernate.Persister.Collection;
using NHibernate.UserTypes;
using System;
using System.Collections;

#nullable disable
namespace NHibernate.Type
{
  [Serializable]
  public class CustomCollectionType : CollectionType
  {
    private readonly IUserCollectionType userType;

    public CustomCollectionType(
      System.Type userTypeClass,
      string role,
      string foreignKeyPropertyName,
      bool isEmbeddedInXML)
      : base(role, foreignKeyPropertyName, isEmbeddedInXML)
    {
      if (!typeof (IUserCollectionType).IsAssignableFrom(userTypeClass))
        throw new MappingException("Custom type does not implement UserCollectionType: " + userTypeClass.FullName);
      try
      {
        this.userType = (IUserCollectionType) NHibernate.Cfg.Environment.BytecodeProvider.ObjectsFactory.CreateInstance(userTypeClass);
      }
      catch (InstantiationException ex)
      {
        throw new MappingException("Cannot instantiate custom type: " + userTypeClass.FullName, (Exception) ex);
      }
      catch (MemberAccessException ex)
      {
        throw new MappingException("MemberAccessException trying to instantiate custom type: " + userTypeClass.FullName, (Exception) ex);
      }
    }

    public IUserCollectionType UserType => this.userType;

    public override IPersistentCollection Instantiate(
      ISessionImplementor session,
      ICollectionPersister persister,
      object key)
    {
      return this.userType.Instantiate(session, persister);
    }

    public override IPersistentCollection Wrap(ISessionImplementor session, object collection)
    {
      return this.userType.Wrap(session, collection);
    }

    public override System.Type ReturnedClass => this.userType.Instantiate(-1).GetType();

    public override object Instantiate(int anticipatedSize)
    {
      return this.userType.Instantiate(anticipatedSize);
    }

    public override IEnumerable GetElementsIterator(object collection)
    {
      return this.userType.GetElements(collection);
    }

    public override bool Contains(object collection, object entity, ISessionImplementor session)
    {
      return this.userType.Contains(collection, entity);
    }

    public override object IndexOf(object collection, object entity)
    {
      return this.userType.IndexOf(collection, entity);
    }

    public override object ReplaceElements(
      object original,
      object target,
      object owner,
      IDictionary copyCache,
      ISessionImplementor session)
    {
      ICollectionPersister collectionPersister = session.Factory.GetCollectionPersister(this.Role);
      return this.userType.ReplaceElements(original, target, collectionPersister, owner, copyCache, session);
    }
  }
}
