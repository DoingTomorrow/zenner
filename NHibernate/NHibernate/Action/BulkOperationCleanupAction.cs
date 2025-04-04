// Decompiled with JetBrains decompiler
// Type: NHibernate.Action.BulkOperationCleanupAction
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Iesi.Collections.Generic;
using NHibernate.Engine;
using NHibernate.Metadata;
using NHibernate.Persister.Entity;
using System;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Action
{
  [Serializable]
  public class BulkOperationCleanupAction : IExecutable
  {
    private readonly ISessionImplementor session;
    private readonly HashedSet<string> affectedEntityNames = new HashedSet<string>();
    private readonly HashedSet<string> affectedCollectionRoles = new HashedSet<string>();
    private readonly List<string> spaces;

    public BulkOperationCleanupAction(ISessionImplementor session, IQueryable[] affectedQueryables)
    {
      this.session = session;
      List<string> collection = new List<string>();
      for (int index1 = 0; index1 < affectedQueryables.Length; ++index1)
      {
        if (affectedQueryables[index1].HasCache)
          this.affectedEntityNames.Add(affectedQueryables[index1].EntityName);
        ISet<string> entityParticipant = session.Factory.GetCollectionRolesByEntityParticipant(affectedQueryables[index1].EntityName);
        if (entityParticipant != null)
          this.affectedCollectionRoles.AddAll((ICollection<string>) entityParticipant);
        for (int index2 = 0; index2 < affectedQueryables[index1].QuerySpaces.Length; ++index2)
          collection.Add(affectedQueryables[index1].QuerySpaces[index2]);
      }
      this.spaces = new List<string>((IEnumerable<string>) collection);
    }

    public BulkOperationCleanupAction(ISessionImplementor session, ISet<string> querySpaces)
    {
      this.session = session;
      ISet<string> collection = (ISet<string>) new HashedSet<string>((ICollection<string>) querySpaces);
      ISessionFactoryImplementor factory = session.Factory;
      foreach (KeyValuePair<string, IClassMetadata> keyValuePair in (IEnumerable<KeyValuePair<string, IClassMetadata>>) factory.GetAllClassMetadata())
      {
        string key = keyValuePair.Key;
        IEntityPersister entityPersister = factory.GetEntityPersister(key);
        string[] querySpaces1 = entityPersister.QuerySpaces;
        if (this.AffectedEntity(querySpaces, querySpaces1))
        {
          if (entityPersister.HasCache)
            this.affectedEntityNames.Add(entityPersister.EntityName);
          ISet<string> entityParticipant = session.Factory.GetCollectionRolesByEntityParticipant(entityPersister.EntityName);
          if (entityParticipant != null)
            this.affectedCollectionRoles.AddAll((ICollection<string>) entityParticipant);
          for (int index = 0; index < querySpaces1.Length; ++index)
            collection.Add(querySpaces1[index]);
        }
      }
      this.spaces = new List<string>((IEnumerable<string>) collection);
    }

    private bool AffectedEntity(ISet<string> querySpaces, string[] entitySpaces)
    {
      if (querySpaces == null || querySpaces.Count == 0)
        return true;
      for (int index = 0; index < entitySpaces.Length; ++index)
      {
        if (querySpaces.Contains(entitySpaces[index]))
          return true;
      }
      return false;
    }

    public string[] PropertySpaces => this.spaces.ToArray();

    public void BeforeExecutions()
    {
    }

    public void Execute()
    {
    }

    public BeforeTransactionCompletionProcessDelegate BeforeTransactionCompletionProcess
    {
      get => (BeforeTransactionCompletionProcessDelegate) null;
    }

    public AfterTransactionCompletionProcessDelegate AfterTransactionCompletionProcess
    {
      get
      {
        return (AfterTransactionCompletionProcessDelegate) (success =>
        {
          this.EvictEntityRegions();
          this.EvictCollectionRegions();
        });
      }
    }

    private void EvictCollectionRegions()
    {
      if (this.affectedCollectionRoles == null)
        return;
      foreach (string affectedCollectionRole in (Set<string>) this.affectedCollectionRoles)
        this.session.Factory.EvictCollection(affectedCollectionRole);
    }

    private void EvictEntityRegions()
    {
      if (this.affectedEntityNames == null)
        return;
      foreach (string affectedEntityName in (Set<string>) this.affectedEntityNames)
        this.session.Factory.EvictEntity(affectedEntityName);
    }

    public virtual void Init()
    {
      this.EvictEntityRegions();
      this.EvictCollectionRegions();
    }
  }
}
