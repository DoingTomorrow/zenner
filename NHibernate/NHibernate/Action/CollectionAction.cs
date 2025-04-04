// Decompiled with JetBrains decompiler
// Type: NHibernate.Action.CollectionAction
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Cache;
using NHibernate.Cache.Access;
using NHibernate.Collection;
using NHibernate.Engine;
using NHibernate.Impl;
using NHibernate.Persister.Collection;
using NHibernate.Util;
using System;
using System.Runtime.Serialization;

#nullable disable
namespace NHibernate.Action
{
  [Serializable]
  public abstract class CollectionAction : 
    IExecutable,
    IComparable<CollectionAction>,
    IDeserializationCallback
  {
    private readonly object key;
    private object finalKey;
    [NonSerialized]
    private ICollectionPersister persister;
    private readonly ISessionImplementor session;
    private readonly string collectionRole;
    private readonly IPersistentCollection collection;
    private ISoftLock softLock;

    protected CollectionAction(
      ICollectionPersister persister,
      IPersistentCollection collection,
      object key,
      ISessionImplementor session)
    {
      this.persister = persister;
      this.session = session;
      this.key = key;
      this.collectionRole = persister.Role;
      this.collection = collection;
    }

    protected internal IPersistentCollection Collection => this.collection;

    protected internal ICollectionPersister Persister => this.persister;

    protected internal object Key
    {
      get
      {
        this.finalKey = this.key;
        if (this.key is DelayedPostInsertIdentifier)
        {
          this.finalKey = this.session.PersistenceContext.GetEntry(this.collection.Owner).Id;
          object finalKey = this.finalKey;
          object key = this.key;
        }
        return this.finalKey;
      }
    }

    protected internal ISessionImplementor Session => this.session;

    public string[] PropertySpaces => this.Persister.CollectionSpaces;

    public virtual void BeforeExecutions()
    {
      if (!this.persister.HasCache)
        return;
      this.softLock = this.persister.Cache.Lock(new CacheKey(this.key, this.persister.KeyType, this.persister.Role, this.session.EntityMode, this.session.Factory), (object) null);
    }

    public abstract void Execute();

    public virtual BeforeTransactionCompletionProcessDelegate BeforeTransactionCompletionProcess
    {
      get => (BeforeTransactionCompletionProcessDelegate) null;
    }

    public virtual AfterTransactionCompletionProcessDelegate AfterTransactionCompletionProcess
    {
      get
      {
        return (AfterTransactionCompletionProcessDelegate) (success =>
        {
          if (!this.persister.HasCache)
            return;
          this.persister.Cache.Release(new CacheKey(this.key, this.persister.KeyType, this.persister.Role, this.Session.EntityMode, this.Session.Factory), this.softLock);
        });
      }
    }

    public ISoftLock Lock => this.softLock;

    protected internal void Evict()
    {
      if (!this.persister.HasCache)
        return;
      this.persister.Cache.Evict(new CacheKey(this.key, this.persister.KeyType, this.persister.Role, this.session.EntityMode, this.session.Factory));
    }

    public virtual int CompareTo(CollectionAction other)
    {
      int num = string.Compare(this.collectionRole, other.collectionRole);
      return num != 0 ? num : this.persister.KeyType.Compare(this.key, other.key, new EntityMode?(this.session.EntityMode));
    }

    public override string ToString()
    {
      return StringHelper.Unqualify(this.GetType().FullName) + MessageHelper.InfoString(this.collectionRole, this.key);
    }

    void IDeserializationCallback.OnDeserialization(object sender)
    {
      this.persister = this.session.Factory.GetCollectionPersister(this.collectionRole);
    }
  }
}
