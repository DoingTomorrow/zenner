// Decompiled with JetBrains decompiler
// Type: NHibernate.Loader.Entity.BatchingEntityLoader
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.Persister.Entity;
using NHibernate.Type;
using NHibernate.Util;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Loader.Entity
{
  public class BatchingEntityLoader : IUniqueEntityLoader
  {
    private readonly NHibernate.Loader.Loader[] loaders;
    private readonly int[] batchSizes;
    private readonly IEntityPersister persister;
    private readonly IType idType;

    public BatchingEntityLoader(IEntityPersister persister, int[] batchSizes, NHibernate.Loader.Loader[] loaders)
    {
      this.batchSizes = batchSizes;
      this.loaders = loaders;
      this.persister = persister;
      this.idType = persister.IdentifierType;
    }

    private object GetObjectFromList(IList results, object id, ISessionImplementor session)
    {
      foreach (object result in (IEnumerable) results)
      {
        if (this.idType.IsEqual(id, session.GetContextEntityIdentifier(result), session.EntityMode, session.Factory))
          return result;
      }
      return (object) null;
    }

    public object Load(object id, object optionalObject, ISessionImplementor session)
    {
      object[] entityBatch = session.PersistenceContext.BatchFetchQueue.GetEntityBatch(this.persister, id, this.batchSizes[0], session.EntityMode);
      for (int index = 0; index < this.batchSizes.Length - 1; ++index)
      {
        int batchSiz = this.batchSizes[index];
        if (entityBatch[batchSiz - 1] != null)
        {
          object[] objArray = new object[batchSiz];
          Array.Copy((Array) entityBatch, 0, (Array) objArray, 0, batchSiz);
          return this.GetObjectFromList(this.loaders[index].LoadEntityBatch(session, objArray, this.idType, optionalObject, this.persister.EntityName, id, this.persister), id, session);
        }
      }
      return ((IUniqueEntityLoader) this.loaders[this.batchSizes.Length - 1]).Load(id, optionalObject, session);
    }

    public static IUniqueEntityLoader CreateBatchingEntityLoader(
      IOuterJoinLoadable persister,
      int maxBatchSize,
      LockMode lockMode,
      ISessionFactoryImplementor factory,
      IDictionary<string, IFilter> enabledFilters)
    {
      if (maxBatchSize <= 1)
        return (IUniqueEntityLoader) new EntityLoader(persister, lockMode, factory, enabledFilters);
      int[] batchSizes = ArrayHelper.GetBatchSizes(maxBatchSize);
      NHibernate.Loader.Loader[] loaders = new NHibernate.Loader.Loader[batchSizes.Length];
      for (int index = 0; index < batchSizes.Length; ++index)
        loaders[index] = (NHibernate.Loader.Loader) new EntityLoader(persister, batchSizes[index], lockMode, factory, enabledFilters);
      return (IUniqueEntityLoader) new BatchingEntityLoader((IEntityPersister) persister, batchSizes, loaders);
    }
  }
}
