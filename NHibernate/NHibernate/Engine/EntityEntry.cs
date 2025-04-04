// Decompiled with JetBrains decompiler
// Type: NHibernate.Engine.EntityEntry
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Impl;
using NHibernate.Intercept;
using NHibernate.Persister.Entity;
using System;

#nullable disable
namespace NHibernate.Engine
{
  [Serializable]
  public sealed class EntityEntry
  {
    private LockMode lockMode;
    private Status status;
    private Status? previousStatus;
    private readonly object id;
    private object[] loadedState;
    private object[] deletedState;
    private bool existsInDatabase;
    private object version;
    [NonSerialized]
    private IEntityPersister persister;
    private readonly EntityMode entityMode;
    private readonly string entityName;
    private EntityKey cachedEntityKey;
    private readonly bool isBeingReplicated;
    private readonly bool loadedWithLazyPropertiesUnfetched;
    [NonSerialized]
    private readonly object rowId;

    internal EntityEntry(
      Status status,
      object[] loadedState,
      object rowId,
      object id,
      object version,
      LockMode lockMode,
      bool existsInDatabase,
      IEntityPersister persister,
      EntityMode entityMode,
      bool disableVersionIncrement,
      bool lazyPropertiesAreUnfetched)
    {
      this.status = status;
      this.previousStatus = new Status?();
      if (status != Status.ReadOnly)
        this.loadedState = loadedState;
      this.id = id;
      this.rowId = rowId;
      this.existsInDatabase = existsInDatabase;
      this.version = version;
      this.lockMode = lockMode;
      this.isBeingReplicated = disableVersionIncrement;
      this.loadedWithLazyPropertiesUnfetched = lazyPropertiesAreUnfetched;
      this.persister = persister;
      this.entityMode = entityMode;
      this.entityName = persister == null ? (string) null : persister.EntityName;
    }

    public LockMode LockMode
    {
      get => this.lockMode;
      set => this.lockMode = value;
    }

    public Status Status
    {
      get => this.status;
      set
      {
        if (value == Status.ReadOnly)
          this.loadedState = (object[]) null;
        if (this.status == value)
          return;
        this.previousStatus = new Status?(this.status);
        this.status = value;
      }
    }

    public object Id => this.id;

    public object[] LoadedState => this.loadedState;

    public object[] DeletedState
    {
      get => this.deletedState;
      set => this.deletedState = value;
    }

    public bool ExistsInDatabase => this.existsInDatabase;

    public object Version => this.version;

    public IEntityPersister Persister
    {
      get => this.persister;
      internal set => this.persister = value;
    }

    public string EntityName => this.entityName;

    public bool IsBeingReplicated => this.isBeingReplicated;

    public object RowId => this.rowId;

    public bool LoadedWithLazyPropertiesUnfetched => this.loadedWithLazyPropertiesUnfetched;

    public EntityKey EntityKey
    {
      get
      {
        if (this.cachedEntityKey == null)
        {
          if (this.id == null)
            throw new InvalidOperationException("cannot generate an EntityKey when id is null.");
          this.cachedEntityKey = new EntityKey(this.id, this.persister, this.entityMode);
        }
        return this.cachedEntityKey;
      }
    }

    public object GetLoadedValue(string propertyName)
    {
      return this.loadedState[((IUniqueKeyLoadable) this.persister).GetPropertyIndex(propertyName)];
    }

    public void PostInsert() => this.existsInDatabase = true;

    public void PostUpdate(object entity, object[] updatedState, object nextVersion)
    {
      this.loadedState = updatedState;
      this.LockMode = LockMode.Write;
      if (this.Persister.IsVersioned)
      {
        this.version = nextVersion;
        this.Persister.SetPropertyValue(entity, this.Persister.VersionProperty, nextVersion, this.entityMode);
      }
      FieldInterceptionHelper.ClearDirty(entity);
    }

    public void PostDelete()
    {
      this.previousStatus = new Status?(this.status);
      this.status = Status.Gone;
      this.existsInDatabase = false;
    }

    public void ForceLocked(object entity, object nextVersion)
    {
      this.version = nextVersion;
      this.loadedState[this.persister.VersionProperty] = this.version;
      this.LockMode = LockMode.Force;
      this.persister.SetPropertyValue(entity, this.Persister.VersionProperty, nextVersion, this.entityMode);
    }

    public bool IsNullifiable(bool earlyInsert, ISessionImplementor session)
    {
      if (this.Status == Status.Saving)
        return true;
      return !earlyInsert ? session.PersistenceContext.NullifiableEntityKeys.Contains((object) new EntityKey(this.Id, this.Persister, this.entityMode)) : !this.ExistsInDatabase;
    }

    public bool RequiresDirtyCheck(object entity)
    {
      if (!this.IsModifiableEntity())
        return false;
      return this.Persister.HasMutableProperties || !FieldInterceptionHelper.IsInstrumented(entity) || FieldInterceptionHelper.ExtractFieldInterceptor(entity).IsDirty;
    }

    public bool IsModifiableEntity()
    {
      if (this.status != Status.ReadOnly)
      {
        if (this.status == Status.Deleted)
        {
          Status? previousStatus = this.previousStatus;
          if ((previousStatus.GetValueOrDefault() != Status.ReadOnly ? 0 : (previousStatus.HasValue ? 1 : 0)) != 0)
            goto label_4;
        }
        return this.Persister.IsMutable;
      }
label_4:
      return false;
    }

    public bool IsReadOnly
    {
      get
      {
        if (this.status != Status.Loaded && this.status != Status.ReadOnly)
          throw new HibernateException("instance was not in a valid state");
        return this.status == Status.ReadOnly;
      }
    }

    public void SetReadOnly(bool readOnly, object entity)
    {
      if (readOnly == this.IsReadOnly)
        return;
      if (readOnly)
      {
        this.Status = Status.ReadOnly;
        this.loadedState = (object[]) null;
      }
      else
      {
        if (!this.persister.IsMutable)
          throw new InvalidOperationException("Cannot make an immutable entity modifiable.");
        this.Status = Status.Loaded;
        this.loadedState = this.Persister.GetPropertyValues(entity, this.entityMode);
      }
    }

    public override string ToString()
    {
      return string.Format("EntityEntry{0}({1})", (object) MessageHelper.InfoString(this.entityName, this.id), (object) this.status);
    }
  }
}
