// Decompiled with JetBrains decompiler
// Type: NHibernate.Loader.Collection.BatchingCollectionInitializer
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.Persister.Collection;
using NHibernate.Util;
using System;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Loader.Collection
{
  public class BatchingCollectionInitializer : ICollectionInitializer
  {
    private readonly NHibernate.Loader.Loader[] loaders;
    private readonly int[] batchSizes;
    private readonly ICollectionPersister collectionPersister;

    public BatchingCollectionInitializer(
      ICollectionPersister collectionPersister,
      int[] batchSizes,
      NHibernate.Loader.Loader[] loaders)
    {
      this.loaders = loaders;
      this.batchSizes = batchSizes;
      this.collectionPersister = collectionPersister;
    }

    public void Initialize(object id, ISessionImplementor session)
    {
      object[] collectionBatch = session.PersistenceContext.BatchFetchQueue.GetCollectionBatch(this.collectionPersister, id, this.batchSizes[0], session.EntityMode);
      for (int index = 0; index < this.batchSizes.Length; ++index)
      {
        int batchSiz = this.batchSizes[index];
        if (collectionBatch[batchSiz - 1] != null)
        {
          object[] objArray = new object[batchSiz];
          Array.Copy((Array) collectionBatch, 0, (Array) objArray, 0, batchSiz);
          this.loaders[index].LoadCollectionBatch(session, objArray, this.collectionPersister.KeyType);
          return;
        }
      }
      this.loaders[this.batchSizes.Length - 1].LoadCollection(session, id, this.collectionPersister.KeyType);
    }

    public static ICollectionInitializer CreateBatchingOneToManyInitializer(
      OneToManyPersister persister,
      int maxBatchSize,
      ISessionFactoryImplementor factory,
      IDictionary<string, IFilter> enabledFilters)
    {
      if (maxBatchSize <= 1)
        return (ICollectionInitializer) new OneToManyLoader((IQueryableCollection) persister, factory, enabledFilters);
      int[] batchSizes = ArrayHelper.GetBatchSizes(maxBatchSize);
      NHibernate.Loader.Loader[] loaders = new NHibernate.Loader.Loader[batchSizes.Length];
      for (int index = 0; index < batchSizes.Length; ++index)
        loaders[index] = (NHibernate.Loader.Loader) new OneToManyLoader((IQueryableCollection) persister, batchSizes[index], factory, enabledFilters);
      return (ICollectionInitializer) new BatchingCollectionInitializer((ICollectionPersister) persister, batchSizes, loaders);
    }

    public static ICollectionInitializer CreateBatchingCollectionInitializer(
      IQueryableCollection persister,
      int maxBatchSize,
      ISessionFactoryImplementor factory,
      IDictionary<string, IFilter> enabledFilters)
    {
      if (maxBatchSize <= 1)
        return (ICollectionInitializer) new BasicCollectionLoader(persister, factory, enabledFilters);
      int[] batchSizes = ArrayHelper.GetBatchSizes(maxBatchSize);
      NHibernate.Loader.Loader[] loaders = new NHibernate.Loader.Loader[batchSizes.Length];
      for (int index = 0; index < batchSizes.Length; ++index)
        loaders[index] = (NHibernate.Loader.Loader) new BasicCollectionLoader(persister, batchSizes[index], factory, enabledFilters);
      return (ICollectionInitializer) new BatchingCollectionInitializer((ICollectionPersister) persister, batchSizes, loaders);
    }
  }
}
