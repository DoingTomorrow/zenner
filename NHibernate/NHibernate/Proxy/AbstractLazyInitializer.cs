// Decompiled with JetBrains decompiler
// Type: NHibernate.Proxy.AbstractLazyInitializer
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.Persister.Entity;
using System;

#nullable disable
namespace NHibernate.Proxy
{
  [Serializable]
  public abstract class AbstractLazyInitializer : ILazyInitializer
  {
    protected static readonly object InvokeImplementation = new object();
    private object _target;
    private bool initialized;
    private object _id;
    [NonSerialized]
    private ISessionImplementor _session;
    private bool unwrap;
    private readonly string _entityName;
    private bool readOnly;
    private bool? readOnlyBeforeAttachedToSession;

    protected internal AbstractLazyInitializer(
      string entityName,
      object id,
      ISessionImplementor session)
    {
      this._id = id;
      this._entityName = entityName;
      if (session == null)
        this.UnsetSession();
      else
        this.SetSession(session);
    }

    public void SetSession(ISessionImplementor s)
    {
      if (s == this._session)
        return;
      if (s == null)
      {
        this.UnsetSession();
      }
      else
      {
        if (this.IsConnectedToSession)
          throw new HibernateException("illegally attempted to associate a proxy with two open Sessions");
        this._session = s;
        if (!this.readOnlyBeforeAttachedToSession.HasValue)
        {
          IEntityPersister entityPersister = s.Factory.GetEntityPersister(this._entityName);
          this.SetReadOnly(s.PersistenceContext.DefaultReadOnly || !entityPersister.IsMutable);
        }
        else
        {
          this.SetReadOnly(this.readOnlyBeforeAttachedToSession.Value);
          this.readOnlyBeforeAttachedToSession = new bool?();
        }
      }
    }

    public void UnsetSession()
    {
      this._session = (ISessionImplementor) null;
      this.readOnly = false;
      this.readOnlyBeforeAttachedToSession = new bool?();
    }

    protected internal bool IsConnectedToSession => this.GetProxyOrNull() != null;

    public virtual void Initialize()
    {
      if (!this.initialized)
      {
        if (this._session == null)
          throw new LazyInitializationException(this._entityName, this._id, "Could not initialize proxy - no Session.");
        if (!this._session.IsOpen)
          throw new LazyInitializationException(this._entityName, this._id, "Could not initialize proxy - the owning Session was closed.");
        if (!this._session.IsConnected)
          throw new LazyInitializationException(this._entityName, this._id, "Could not initialize proxy - the owning Session is disconnected.");
        this._target = this._session.ImmediateLoad(this._entityName, this._id);
        this.initialized = true;
        this.CheckTargetState();
      }
      else
        this.CheckTargetState();
    }

    public object Identifier
    {
      get => this._id;
      set => this._id = value;
    }

    public abstract Type PersistentClass { get; }

    public bool IsUninitialized => !this.initialized;

    public bool Unwrap
    {
      get => this.unwrap;
      set => this.unwrap = value;
    }

    public ISessionImplementor Session
    {
      get => this._session;
      set
      {
        if (value == this._session)
          return;
        this._session = value == null || !this.IsConnectedToSession ? value : throw new LazyInitializationException(this._entityName, this._id, "Illegally attempted to associate a proxy with two open Sessions");
      }
    }

    public string EntityName => this._entityName;

    protected internal object Target => this._target;

    public object GetImplementation()
    {
      this.Initialize();
      return this._target;
    }

    public object GetImplementation(ISessionImplementor s)
    {
      EntityKey key = new EntityKey(this.Identifier, s.Factory.GetEntityPersister(this.EntityName), s.EntityMode);
      return s.PersistenceContext.GetEntity(key);
    }

    public void SetImplementation(object target)
    {
      this._target = target;
      this.initialized = true;
    }

    public bool IsReadOnlySettingAvailable => this._session != null && !this._session.IsClosed;

    public bool ReadOnly
    {
      get
      {
        this.ErrorIfReadOnlySettingNotAvailable();
        return this.readOnly;
      }
      set
      {
        this.ErrorIfReadOnlySettingNotAvailable();
        if (this.readOnly == value)
          return;
        this.SetReadOnly(value);
      }
    }

    private void ErrorIfReadOnlySettingNotAvailable()
    {
      if (this._session == null)
        throw new TransientObjectException("Proxy is detached (i.e, session is null). The read-only/modifiable setting is only accessible when the proxy is associated with an open session.");
      if (this._session.IsClosed)
        throw new SessionException("Session is closed. The read-only/modifiable setting is only accessible when the proxy is associated with an open session.");
    }

    private static EntityKey GenerateEntityKeyOrNull(
      object id,
      ISessionImplementor s,
      string entityName)
    {
      return id == null || s == null || entityName == null ? (EntityKey) null : new EntityKey(id, s.Factory.GetEntityPersister(entityName), s.EntityMode);
    }

    private void CheckTargetState()
    {
      if (this.unwrap || this._target != null)
        return;
      this.Session.Factory.EntityNotFoundDelegate.HandleEntityNotFound(this._entityName, this._id);
    }

    private object GetProxyOrNull()
    {
      EntityKey entityKeyOrNull = AbstractLazyInitializer.GenerateEntityKeyOrNull(this._id, this._session, this._entityName);
      return entityKeyOrNull != null && this._session != null && this._session.IsOpen ? this._session.PersistenceContext.GetProxy(entityKeyOrNull) : (object) null;
    }

    private void SetReadOnly(bool readOnly)
    {
      if (!this._session.Factory.GetEntityPersister(this._entityName).IsMutable && !readOnly)
        throw new InvalidOperationException("cannot make proxies for immutable entities modifiable");
      this.readOnly = readOnly;
      if (!this.initialized)
        return;
      EntityKey entityKeyOrNull = AbstractLazyInitializer.GenerateEntityKeyOrNull(this._id, this._session, this._entityName);
      if (entityKeyOrNull == null || !this._session.PersistenceContext.ContainsEntity(entityKeyOrNull))
        return;
      this._session.PersistenceContext.SetReadOnly(this._target, readOnly);
    }
  }
}
