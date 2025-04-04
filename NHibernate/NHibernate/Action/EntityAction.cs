// Decompiled with JetBrains decompiler
// Type: NHibernate.Action.EntityAction
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.Impl;
using NHibernate.Persister.Entity;
using NHibernate.Util;
using System;
using System.IO;
using System.Runtime.Serialization;

#nullable disable
namespace NHibernate.Action
{
  [Serializable]
  public abstract class EntityAction : 
    IExecutable,
    IComparable<EntityAction>,
    IDeserializationCallback
  {
    private readonly string entityName;
    private readonly object id;
    private readonly object instance;
    private readonly ISessionImplementor session;
    [NonSerialized]
    private IEntityPersister persister;

    protected internal EntityAction(
      ISessionImplementor session,
      object id,
      object instance,
      IEntityPersister persister)
    {
      this.entityName = persister.EntityName;
      this.id = id;
      this.instance = instance;
      this.session = session;
      this.persister = persister;
    }

    public string EntityName => this.entityName;

    public object Id
    {
      get
      {
        return this.id is DelayedPostInsertIdentifier ? this.session.PersistenceContext.GetEntry(this.instance).Id : this.id;
      }
    }

    public object Instance => this.instance;

    public ISessionImplementor Session => this.session;

    public IEntityPersister Persister => this.persister;

    protected internal abstract bool HasPostCommitEventListeners { get; }

    public string[] PropertySpaces => this.persister.PropertySpaces;

    public void BeforeExecutions()
    {
      throw new AssertionFailure("BeforeExecutions() called for non-collection action");
    }

    public abstract void Execute();

    public virtual BeforeTransactionCompletionProcessDelegate BeforeTransactionCompletionProcess
    {
      get
      {
        return new BeforeTransactionCompletionProcessDelegate(this.BeforeTransactionCompletionProcessImpl);
      }
    }

    public virtual AfterTransactionCompletionProcessDelegate AfterTransactionCompletionProcess
    {
      get
      {
        return !this.NeedsAfterTransactionCompletion() ? (AfterTransactionCompletionProcessDelegate) null : new AfterTransactionCompletionProcessDelegate(this.AfterTransactionCompletionProcessImpl);
      }
    }

    private bool NeedsAfterTransactionCompletion()
    {
      return this.persister.HasCache || this.HasPostCommitEventListeners;
    }

    protected virtual void BeforeTransactionCompletionProcessImpl()
    {
    }

    protected virtual void AfterTransactionCompletionProcessImpl(bool success)
    {
    }

    public virtual int CompareTo(EntityAction other)
    {
      int num = this.entityName.CompareTo(other.entityName);
      return num != 0 ? num : this.persister.IdentifierType.Compare(this.id, other.id, new EntityMode?(this.session.EntityMode));
    }

    void IDeserializationCallback.OnDeserialization(object sender)
    {
      try
      {
        this.persister = this.session.Factory.GetEntityPersister(this.entityName);
      }
      catch (MappingException ex)
      {
        throw new IOException("Unable to resolve class persister on deserialization", (Exception) ex);
      }
    }

    public override string ToString()
    {
      return StringHelper.Unqualify(this.GetType().FullName) + MessageHelper.InfoString(this.entityName, this.id);
    }
  }
}
