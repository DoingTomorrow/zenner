// Decompiled with JetBrains decompiler
// Type: NHibernate.Collection.AbstractPersistentCollection
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Iesi.Collections.Generic;
using NHibernate.Engine;
using NHibernate.Impl;
using NHibernate.Loader;
using NHibernate.Persister.Collection;
using NHibernate.Type;
using NHibernate.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace NHibernate.Collection
{
  [Serializable]
  public abstract class AbstractPersistentCollection : IPersistentCollection
  {
    protected internal static readonly object Unknown = new object();
    protected internal static readonly object NotFound = new object();
    [NonSerialized]
    private ISessionImplementor session;
    private bool initialized;
    [NonSerialized]
    private List<AbstractPersistentCollection.IDelayedOperation> operationQueue;
    [NonSerialized]
    private bool directlyAccessible;
    [NonSerialized]
    private bool initializing;
    private object owner;
    private int cachedSize = -1;
    private string role;
    private object key;
    private bool dirty;
    private object storedSnapshot;

    protected AbstractPersistentCollection()
    {
    }

    protected AbstractPersistentCollection(ISessionImplementor session) => this.session = session;

    public string Role => this.role;

    public object Key => this.key;

    public bool IsUnreferenced => this.role == null;

    public bool IsDirty => this.dirty;

    public object StoredSnapshot => this.storedSnapshot;

    protected int CachedSize
    {
      get => this.cachedSize;
      set => this.cachedSize = value;
    }

    protected bool IsConnectedToSession
    {
      get
      {
        return this.session != null && this.session.IsOpen && this.session.PersistenceContext.ContainsCollection((IPersistentCollection) this);
      }
    }

    protected bool IsOperationQueueEnabled
    {
      get => !this.initialized && this.IsConnectedToSession && this.IsInverseCollection;
    }

    protected bool PutQueueEnabled
    {
      get
      {
        return !this.initialized && this.IsConnectedToSession && this.InverseOneToManyOrNoOrphanDelete;
      }
    }

    protected bool ClearQueueEnabled
    {
      get => !this.initialized && this.IsConnectedToSession && this.InverseCollectionNoOrphanDelete;
    }

    protected bool IsInverseCollection
    {
      get
      {
        CollectionEntry collectionEntry = this.session.PersistenceContext.GetCollectionEntry((IPersistentCollection) this);
        return collectionEntry != null && collectionEntry.LoadedPersister.IsInverse;
      }
    }

    protected bool InverseCollectionNoOrphanDelete
    {
      get
      {
        CollectionEntry collectionEntry = this.session.PersistenceContext.GetCollectionEntry((IPersistentCollection) this);
        return collectionEntry != null && collectionEntry.LoadedPersister.IsInverse && !collectionEntry.LoadedPersister.HasOrphanDelete;
      }
    }

    protected bool InverseOneToManyOrNoOrphanDelete
    {
      get
      {
        CollectionEntry collectionEntry = this.session.PersistenceContext.GetCollectionEntry((IPersistentCollection) this);
        if (collectionEntry == null || !collectionEntry.LoadedPersister.IsInverse)
          return false;
        return collectionEntry.LoadedPersister.IsOneToMany || !collectionEntry.LoadedPersister.HasOrphanDelete;
      }
    }

    public virtual object GetValue() => (object) this;

    public virtual bool RowUpdatePossible => true;

    protected virtual ISessionImplementor Session => this.session;

    public virtual object Owner
    {
      get => this.owner;
      set => this.owner = value;
    }

    public void ClearDirty() => this.dirty = false;

    public void Dirty() => this.dirty = true;

    public abstract bool Empty { get; }

    public virtual void Read() => this.Initialize(false);

    protected virtual bool ReadSize()
    {
      if (!this.initialized)
      {
        if (this.cachedSize != -1 && !this.HasQueuedOperations)
          return true;
        this.ThrowLazyInitializationExceptionIfNotConnected();
        CollectionEntry collectionEntry = this.session.PersistenceContext.GetCollectionEntry((IPersistentCollection) this);
        ICollectionPersister loadedPersister = collectionEntry.LoadedPersister;
        if (loadedPersister.IsExtraLazy)
        {
          if (this.HasQueuedOperations)
            this.session.Flush();
          this.cachedSize = loadedPersister.GetSize(collectionEntry.LoadedKey, this.session);
          return true;
        }
      }
      this.Read();
      return false;
    }

    protected virtual bool? ReadIndexExistence(object index)
    {
      if (!this.initialized)
      {
        this.ThrowLazyInitializationExceptionIfNotConnected();
        CollectionEntry collectionEntry = this.session.PersistenceContext.GetCollectionEntry((IPersistentCollection) this);
        ICollectionPersister loadedPersister = collectionEntry.LoadedPersister;
        if (loadedPersister.IsExtraLazy)
        {
          if (this.HasQueuedOperations)
            this.session.Flush();
          return new bool?(loadedPersister.IndexExists(collectionEntry.LoadedKey, index, this.session));
        }
      }
      this.Read();
      return new bool?();
    }

    protected virtual bool? ReadElementExistence(object element)
    {
      if (!this.initialized)
      {
        this.ThrowLazyInitializationExceptionIfNotConnected();
        CollectionEntry collectionEntry = this.session.PersistenceContext.GetCollectionEntry((IPersistentCollection) this);
        ICollectionPersister loadedPersister = collectionEntry.LoadedPersister;
        if (loadedPersister.IsExtraLazy)
        {
          if (this.HasQueuedOperations)
            this.session.Flush();
          return new bool?(loadedPersister.ElementExists(collectionEntry.LoadedKey, element, this.session));
        }
      }
      this.Read();
      return new bool?();
    }

    protected virtual object ReadElementByIndex(object index)
    {
      if (!this.initialized)
      {
        this.ThrowLazyInitializationExceptionIfNotConnected();
        CollectionEntry collectionEntry = this.session.PersistenceContext.GetCollectionEntry((IPersistentCollection) this);
        ICollectionPersister loadedPersister = collectionEntry.LoadedPersister;
        if (loadedPersister.IsExtraLazy)
        {
          if (this.HasQueuedOperations)
            this.session.Flush();
          object elementByIndex = loadedPersister.GetElementByIndex(collectionEntry.LoadedKey, index, this.session, this.owner);
          return loadedPersister.NotFoundObject != elementByIndex ? elementByIndex : AbstractPersistentCollection.NotFound;
        }
      }
      this.Read();
      return AbstractPersistentCollection.Unknown;
    }

    protected virtual void Write()
    {
      this.Initialize(true);
      this.Dirty();
    }

    protected virtual void QueueOperation(
      AbstractPersistentCollection.IDelayedOperation element)
    {
      if (this.operationQueue == null)
        this.operationQueue = new List<AbstractPersistentCollection.IDelayedOperation>(10);
      this.operationQueue.Add(element);
      this.dirty = true;
    }

    protected virtual void PerformQueuedOperations()
    {
      for (int index = 0; index < this.operationQueue.Count; ++index)
        this.operationQueue[index].Operate();
    }

    public void SetSnapshot(object key, string role, object snapshot)
    {
      this.key = key;
      this.role = role;
      this.storedSnapshot = snapshot;
    }

    public virtual void PostAction()
    {
      this.operationQueue = (List<AbstractPersistentCollection.IDelayedOperation>) null;
      this.cachedSize = -1;
      this.ClearDirty();
    }

    public virtual void BeginRead() => this.initializing = true;

    public virtual bool EndRead(ICollectionPersister persister) => this.AfterInitialize(persister);

    public virtual bool AfterInitialize(ICollectionPersister persister)
    {
      this.SetInitialized();
      if (this.operationQueue == null)
        return true;
      this.PerformQueuedOperations();
      this.operationQueue = (List<AbstractPersistentCollection.IDelayedOperation>) null;
      this.cachedSize = -1;
      return false;
    }

    protected virtual void Initialize(bool writing)
    {
      if (this.initialized)
        return;
      if (this.initializing)
        throw new LazyInitializationException("illegal access to loading collection");
      this.ThrowLazyInitializationExceptionIfNotConnected();
      this.session.InitializeCollection((IPersistentCollection) this, writing);
    }

    protected void ThrowLazyInitializationExceptionIfNotConnected()
    {
      if (!this.IsConnectedToSession)
        this.ThrowLazyInitializationException("no session or session was closed");
      if (this.session.IsConnected)
        return;
      this.ThrowLazyInitializationException("session is disconnected");
    }

    protected void ThrowLazyInitializationException(string message)
    {
      throw new LazyInitializationException(this.role == null ? "Unavailable" : StringHelper.Qualifier(this.role), this.key, "failed to lazily initialize a collection" + (this.role == null ? "" : " of role: " + this.role) + ", " + message);
    }

    protected virtual void SetInitialized()
    {
      this.initializing = false;
      this.initialized = true;
    }

    public virtual bool IsDirectlyAccessible
    {
      get => this.directlyAccessible;
      protected set => this.directlyAccessible = value;
    }

    public bool UnsetSession(ISessionImplementor currentSession)
    {
      if (currentSession != this.session)
        return false;
      this.session = (ISessionImplementor) null;
      return true;
    }

    public virtual bool SetCurrentSession(ISessionImplementor session)
    {
      if (session == this.session && session.PersistenceContext.ContainsCollection((IPersistentCollection) this))
        return false;
      if (this.IsConnectedToSession)
      {
        CollectionEntry collectionEntry = session.PersistenceContext.GetCollectionEntry((IPersistentCollection) this);
        if (collectionEntry == null)
          throw new HibernateException("Illegal attempt to associate a collection with two open sessions");
        throw new HibernateException("Illegal attempt to associate a collection with two open sessions: " + MessageHelper.InfoString(collectionEntry.LoadedPersister, collectionEntry.LoadedKey, session.Factory));
      }
      this.session = session;
      return true;
    }

    public virtual bool NeedsRecreate(ICollectionPersister persister) => false;

    public virtual void ForceInitialization()
    {
      if (this.initialized)
        return;
      if (this.initializing)
        throw new AssertionFailure("force initialize loading collection");
      if (this.session == null)
        throw new HibernateException("collection is not associated with any session");
      if (!this.session.IsConnected)
        throw new HibernateException("disconnected session");
      this.session.InitializeCollection((IPersistentCollection) this, false);
    }

    protected virtual object GetSnapshot()
    {
      return this.session.PersistenceContext.GetSnapshot((IPersistentCollection) this);
    }

    public bool WasInitialized => this.initialized;

    public bool HasQueuedOperations => this.operationQueue != null && this.operationQueue.Count > 0;

    public IEnumerable QueuedAdditionIterator
    {
      get
      {
        return this.HasQueuedOperations ? (IEnumerable) new AbstractPersistentCollection.AdditionEnumerable(this) : CollectionHelper.EmptyEnumerable;
      }
    }

    public ICollection GetQueuedOrphans(string entityName)
    {
      if (!this.HasQueuedOperations)
        return CollectionHelper.EmptyCollection;
      List<object> currentElements = new List<object>(this.operationQueue.Count);
      List<object> oldElements = new List<object>(this.operationQueue.Count);
      for (int index = 0; index < this.operationQueue.Count; ++index)
      {
        AbstractPersistentCollection.IDelayedOperation operation = this.operationQueue[index];
        if (operation.AddedInstance != null)
          currentElements.Add(operation.AddedInstance);
        if (operation.Orphan != null)
          oldElements.Add(operation.Orphan);
      }
      return this.GetOrphans((ICollection) oldElements, (ICollection) currentElements, entityName, this.session);
    }

    public virtual void PreInsert(ICollectionPersister persister)
    {
    }

    public virtual void AfterRowInsert(
      ICollectionPersister persister,
      object entry,
      int i,
      object id)
    {
    }

    public abstract ICollection GetOrphans(object snapshot, string entityName);

    protected virtual ICollection GetOrphans(
      ICollection oldElements,
      ICollection currentElements,
      string entityName,
      ISessionImplementor session)
    {
      if (currentElements.Count == 0 || oldElements.Count == 0)
        return oldElements;
      IType identifierType = session.Factory.GetEntityPersister(entityName).IdentifierType;
      List<object> orphans = new List<object>();
      HashedSet<TypedValue> hashedSet = new HashedSet<TypedValue>();
      foreach (object currentElement in (IEnumerable) currentElements)
      {
        if (currentElement != null && ForeignKeys.IsNotTransient(entityName, currentElement, new bool?(), session))
        {
          object identifierIfNotUnsaved = ForeignKeys.GetEntityIdentifierIfNotUnsaved(entityName, currentElement, session);
          hashedSet.Add(new TypedValue(identifierType, identifierIfNotUnsaved, session.EntityMode));
        }
      }
      foreach (object oldElement in (IEnumerable) oldElements)
      {
        object identifierIfNotUnsaved = ForeignKeys.GetEntityIdentifierIfNotUnsaved(entityName, oldElement, session);
        if (!hashedSet.Contains(new TypedValue(identifierType, identifierIfNotUnsaved, session.EntityMode)))
          orphans.Add(oldElement);
      }
      return (ICollection) orphans;
    }

    public void IdentityRemove(
      IList list,
      object obj,
      string entityName,
      ISessionImplementor session)
    {
      if (obj == null || !ForeignKeys.IsNotTransient(entityName, obj, new bool?(), session))
        return;
      IType identifierType = session.Factory.GetEntityPersister(entityName).IdentifierType;
      object identifierIfNotUnsaved1 = ForeignKeys.GetEntityIdentifierIfNotUnsaved(entityName, obj, session);
      List<object> objectList = new List<object>(list.Count);
      foreach (object entity in (IEnumerable) list)
      {
        if (entity != null)
        {
          object identifierIfNotUnsaved2 = ForeignKeys.GetEntityIdentifierIfNotUnsaved(entityName, entity, session);
          if (identifierType.IsEqual(identifierIfNotUnsaved1, identifierIfNotUnsaved2, session.EntityMode, session.Factory))
            objectList.Add(entity);
        }
      }
      foreach (object obj1 in objectList)
        list.Remove(obj1);
    }

    public virtual object GetIdentifier(object entry, int i) => throw new NotSupportedException();

    public abstract object Disassemble(ICollectionPersister persister);

    public abstract bool IsWrapper(object collection);

    public abstract bool EntryExists(object entry, int i);

    public abstract IEnumerable GetDeletes(ICollectionPersister persister, bool indexIsFormula);

    public abstract bool IsSnapshotEmpty(object snapshot);

    public abstract IEnumerable Entries(ICollectionPersister persister);

    public abstract ICollection GetSnapshot(ICollectionPersister persister);

    public abstract bool EqualsSnapshot(ICollectionPersister persister);

    public abstract object GetElement(object entry);

    public abstract void InitializeFromCache(
      ICollectionPersister persister,
      object disassembled,
      object owner);

    public abstract bool NeedsUpdating(object entry, int i, IType elemType);

    public abstract object ReadFrom(
      IDataReader reader,
      ICollectionPersister role,
      ICollectionAliases descriptor,
      object owner);

    public abstract object GetSnapshotElement(object entry, int i);

    public abstract bool NeedsInserting(object entry, int i, IType elemType);

    public abstract object GetIndex(object entry, int i, ICollectionPersister persister);

    public abstract void BeforeInitialize(ICollectionPersister persister, int anticipatedSize);

    protected interface IDelayedOperation
    {
      object AddedInstance { get; }

      object Orphan { get; }

      void Operate();
    }

    private class AdditionEnumerable : IEnumerable
    {
      private readonly AbstractPersistentCollection enclosingInstance;

      public AdditionEnumerable(AbstractPersistentCollection enclosingInstance)
      {
        this.enclosingInstance = enclosingInstance;
      }

      public IEnumerator GetEnumerator()
      {
        return (IEnumerator) new AbstractPersistentCollection.AdditionEnumerable.AdditionEnumerator(this.enclosingInstance);
      }

      private class AdditionEnumerator : IEnumerator
      {
        private readonly AbstractPersistentCollection enclosingInstance;
        private int position = -1;

        public AdditionEnumerator(AbstractPersistentCollection enclosingInstance)
        {
          this.enclosingInstance = enclosingInstance;
        }

        public object Current
        {
          get
          {
            try
            {
              return this.enclosingInstance.operationQueue[this.position].AddedInstance;
            }
            catch (IndexOutOfRangeException ex)
            {
              throw new InvalidOperationException();
            }
          }
        }

        public bool MoveNext()
        {
          ++this.position;
          return this.position < this.enclosingInstance.operationQueue.Count;
        }

        public void Reset() => this.position = -1;
      }
    }
  }
}
