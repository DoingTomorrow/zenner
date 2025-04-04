// Decompiled with JetBrains decompiler
// Type: NHibernate.Event.Default.WrapVisitor
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Collection;
using NHibernate.Engine;
using NHibernate.Persister.Collection;
using NHibernate.Persister.Entity;
using NHibernate.Type;

#nullable disable
namespace NHibernate.Event.Default
{
  public class WrapVisitor(IEventSource session) : ProxyVisitor(session)
  {
    private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof (WrapVisitor));
    private bool substitute;

    internal bool SubstitutionRequired => this.substitute;

    internal override void Process(object obj, IEntityPersister persister)
    {
      EntityMode entityMode = this.Session.EntityMode;
      object[] propertyValues = persister.GetPropertyValues(obj, entityMode);
      IType[] propertyTypes = persister.PropertyTypes;
      this.ProcessEntityPropertyValues(propertyValues, propertyTypes);
      if (!this.SubstitutionRequired)
        return;
      persister.SetPropertyValues(obj, propertyValues, entityMode);
    }

    internal override object ProcessCollection(object collection, CollectionType collectionType)
    {
      if (!(collection is IPersistentCollection collection1))
        return this.ProcessArrayOrNewCollection(collection, collectionType);
      ISessionImplementor session = (ISessionImplementor) this.Session;
      if (collection1.SetCurrentSession(session))
        this.ReattachCollection(collection1, collectionType);
      return (object) null;
    }

    private object ProcessArrayOrNewCollection(object collection, CollectionType collectionType)
    {
      if (collection == null)
        return (object) null;
      ISessionImplementor session = (ISessionImplementor) this.Session;
      ICollectionPersister collectionPersister = session.Factory.GetCollectionPersister(collectionType.Role);
      IPersistenceContext persistenceContext = session.PersistenceContext;
      if (collectionType.HasHolder(session.EntityMode))
      {
        if (collection == CollectionType.UnfetchedCollection)
          return (object) null;
        if (persistenceContext.GetCollectionHolder(collection) == null)
        {
          IPersistentCollection persistentCollection = collectionType.Wrap(session, collection);
          persistenceContext.AddNewCollection(collectionPersister, persistentCollection);
          persistenceContext.AddCollectionHolder(persistentCollection);
        }
        return (object) null;
      }
      IPersistentCollection collection1 = collectionType.Wrap(session, collection);
      persistenceContext.AddNewCollection(collectionPersister, collection1);
      if (WrapVisitor.log.IsDebugEnabled)
        WrapVisitor.log.Debug((object) ("Wrapped collection in role: " + collectionType.Role));
      return (object) collection1;
    }

    internal override void ProcessValue(int i, object[] values, IType[] types)
    {
      object obj = this.ProcessValue(values[i], types[i]);
      if (obj == null)
        return;
      this.substitute = true;
      values[i] = obj;
    }

    internal override object ProcessComponent(
      object component,
      IAbstractComponentType componentType)
    {
      if (component != null)
      {
        object[] propertyValues = componentType.GetPropertyValues(component, (ISessionImplementor) this.Session);
        IType[] subtypes = componentType.Subtypes;
        bool flag = false;
        for (int index = 0; index < subtypes.Length; ++index)
        {
          object obj = this.ProcessValue(propertyValues[index], subtypes[index]);
          if (obj != null)
          {
            propertyValues[index] = obj;
            flag = true;
          }
        }
        if (flag)
          componentType.SetPropertyValues(component, propertyValues, this.Session.EntityMode);
      }
      return (object) null;
    }
  }
}
