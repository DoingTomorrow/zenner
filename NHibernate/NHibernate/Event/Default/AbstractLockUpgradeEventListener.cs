// Decompiled with JetBrains decompiler
// Type: NHibernate.Event.Default.AbstractLockUpgradeEventListener
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Cache;
using NHibernate.Cache.Access;
using NHibernate.Engine;
using NHibernate.Impl;
using NHibernate.Persister.Entity;
using System;

#nullable disable
namespace NHibernate.Event.Default
{
  [Serializable]
  public class AbstractLockUpgradeEventListener : AbstractReassociateEventListener
  {
    private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof (AbstractLockUpgradeEventListener));

    protected virtual void UpgradeLock(
      object entity,
      EntityEntry entry,
      LockMode requestedLockMode,
      ISessionImplementor source)
    {
      if (!requestedLockMode.GreaterThan(entry.LockMode))
        return;
      IEntityPersister persister = entry.Status == Status.Loaded ? entry.Persister : throw new ObjectDeletedException("attempted to lock a deleted instance", entry.Id, entry.EntityName);
      if (AbstractLockUpgradeEventListener.log.IsDebugEnabled)
        AbstractLockUpgradeEventListener.log.Debug((object) string.Format("locking {0} in mode: {1}", (object) MessageHelper.InfoString(persister, entry.Id, source.Factory), (object) requestedLockMode));
      CacheKey key;
      ISoftLock @lock;
      if (persister.HasCache)
      {
        key = new CacheKey(entry.Id, persister.IdentifierType, persister.RootEntityName, source.EntityMode, source.Factory);
        @lock = persister.Cache.Lock(key, entry.Version);
      }
      else
      {
        key = (CacheKey) null;
        @lock = (ISoftLock) null;
      }
      try
      {
        if (persister.IsVersioned && requestedLockMode == LockMode.Force)
        {
          object nextVersion = persister.ForceVersionIncrement(entry.Id, entry.Version, source);
          entry.ForceLocked(entity, nextVersion);
        }
        else
          persister.Lock(entry.Id, entry.Version, entity, requestedLockMode, source);
        entry.LockMode = requestedLockMode;
      }
      finally
      {
        if (persister.HasCache)
          persister.Cache.Release(key, @lock);
      }
    }
  }
}
