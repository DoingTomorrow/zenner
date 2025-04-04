// Decompiled with JetBrains decompiler
// Type: NHibernate.Engine.ActionQueue
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Iesi.Collections.Generic;
using NHibernate.Action;
using NHibernate.Cache;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#nullable disable
namespace NHibernate.Engine
{
  [Serializable]
  public class ActionQueue
  {
    private const int InitQueueListSize = 5;
    private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof (ActionQueue));
    private ISessionImplementor session;
    private readonly List<IExecutable> insertions;
    private readonly List<EntityDeleteAction> deletions;
    private readonly List<EntityUpdateAction> updates;
    private readonly List<CollectionRecreateAction> collectionCreations;
    private readonly List<CollectionUpdateAction> collectionUpdates;
    private readonly List<CollectionRemoveAction> collectionRemovals;
    private readonly ActionQueue.AfterTransactionCompletionProcessQueue afterTransactionProcesses;
    private readonly ActionQueue.BeforeTransactionCompletionProcessQueue beforeTransactionProcesses;

    public ActionQueue(ISessionImplementor session)
    {
      this.session = session;
      this.insertions = new List<IExecutable>(5);
      this.deletions = new List<EntityDeleteAction>(5);
      this.updates = new List<EntityUpdateAction>(5);
      this.collectionCreations = new List<CollectionRecreateAction>(5);
      this.collectionUpdates = new List<CollectionUpdateAction>(5);
      this.collectionRemovals = new List<CollectionRemoveAction>(5);
      this.afterTransactionProcesses = new ActionQueue.AfterTransactionCompletionProcessQueue(session);
      this.beforeTransactionProcesses = new ActionQueue.BeforeTransactionCompletionProcessQueue(session);
    }

    public virtual void Clear()
    {
      this.updates.Clear();
      this.insertions.Clear();
      this.deletions.Clear();
      this.collectionCreations.Clear();
      this.collectionRemovals.Clear();
      this.collectionUpdates.Clear();
    }

    public void AddAction(EntityInsertAction action) => this.insertions.Add((IExecutable) action);

    public void AddAction(EntityDeleteAction action) => this.deletions.Add(action);

    public void AddAction(EntityUpdateAction action) => this.updates.Add(action);

    public void AddAction(CollectionRecreateAction action) => this.collectionCreations.Add(action);

    public void AddAction(CollectionRemoveAction action) => this.collectionRemovals.Add(action);

    public void AddAction(CollectionUpdateAction action) => this.collectionUpdates.Add(action);

    public void AddAction(EntityIdentityInsertAction insert)
    {
      this.insertions.Add((IExecutable) insert);
    }

    public void AddAction(BulkOperationCleanupAction cleanupAction)
    {
      this.RegisterCleanupActions((IExecutable) cleanupAction);
    }

    public void RegisterProcess(AfterTransactionCompletionProcessDelegate process)
    {
      this.afterTransactionProcesses.Register(process);
    }

    public void RegisterProcess(BeforeTransactionCompletionProcessDelegate process)
    {
      this.beforeTransactionProcesses.Register(process);
    }

    private void ExecuteActions(IList list)
    {
      int count = list.Count;
      for (int index = 0; index < count; ++index)
        this.Execute((IExecutable) list[index]);
      list.Clear();
      this.session.Batcher.ExecuteBatch();
    }

    public void Execute(IExecutable executable)
    {
      try
      {
        executable.Execute();
      }
      finally
      {
        this.RegisterCleanupActions(executable);
      }
    }

    private void RegisterCleanupActions(IExecutable executable)
    {
      this.beforeTransactionProcesses.Register(executable.BeforeTransactionCompletionProcess);
      if (this.session.Factory.Settings.IsQueryCacheEnabled)
      {
        string[] propertySpaces = executable.PropertySpaces;
        this.afterTransactionProcesses.AddSpacesToInvalidate(propertySpaces);
        this.session.Factory.UpdateTimestampsCache.PreInvalidate((object[]) propertySpaces);
      }
      this.afterTransactionProcesses.Register(executable.AfterTransactionCompletionProcess);
    }

    public void ExecuteInserts() => this.ExecuteActions((IList) this.insertions);

    public void ExecuteActions()
    {
      this.ExecuteActions((IList) this.insertions);
      this.ExecuteActions((IList) this.updates);
      this.ExecuteActions((IList) this.collectionRemovals);
      this.ExecuteActions((IList) this.collectionUpdates);
      this.ExecuteActions((IList) this.collectionCreations);
      this.ExecuteActions((IList) this.deletions);
    }

    private void PrepareActions(IList queue)
    {
      foreach (IExecutable executable in (IEnumerable) queue)
        executable.BeforeExecutions();
    }

    public void PrepareActions()
    {
      this.PrepareActions((IList) this.collectionRemovals);
      this.PrepareActions((IList) this.collectionUpdates);
      this.PrepareActions((IList) this.collectionCreations);
    }

    public void BeforeTransactionCompletion()
    {
      this.beforeTransactionProcesses.BeforeTransactionCompletion();
    }

    public void AfterTransactionCompletion(bool success)
    {
      this.afterTransactionProcesses.AfterTransactionCompletion(success);
    }

    public virtual bool AreTablesToBeUpdated(ISet<string> tables)
    {
      return ActionQueue.AreTablesToUpdated((IList) this.updates, (ICollection<string>) tables) || ActionQueue.AreTablesToUpdated((IList) this.insertions, (ICollection<string>) tables) || ActionQueue.AreTablesToUpdated((IList) this.deletions, (ICollection<string>) tables) || ActionQueue.AreTablesToUpdated((IList) this.collectionUpdates, (ICollection<string>) tables) || ActionQueue.AreTablesToUpdated((IList) this.collectionCreations, (ICollection<string>) tables) || ActionQueue.AreTablesToUpdated((IList) this.collectionRemovals, (ICollection<string>) tables);
    }

    public bool AreInsertionsOrDeletionsQueued
    {
      get => this.insertions.Count > 0 || this.deletions.Count > 0;
    }

    private static bool AreTablesToUpdated(IList executables, ICollection<string> tablespaces)
    {
      foreach (IExecutable executable in (IEnumerable) executables)
      {
        foreach (string propertySpace in executable.PropertySpaces)
        {
          if (tablespaces.Contains(propertySpace))
          {
            if (ActionQueue.log.IsDebugEnabled)
              ActionQueue.log.Debug((object) ("changes must be flushed to space: " + propertySpace));
            return true;
          }
        }
      }
      return false;
    }

    public int CollectionRemovalsCount => this.collectionRemovals.Count;

    public int CollectionUpdatesCount => this.collectionUpdates.Count;

    public int CollectionCreationsCount => this.collectionCreations.Count;

    public int DeletionsCount => this.deletions.Count;

    public int UpdatesCount => this.updates.Count;

    public int InsertionsCount => this.insertions.Count;

    public void SortCollectionActions()
    {
      if (!this.session.Factory.Settings.IsOrderUpdatesEnabled)
        return;
      this.collectionCreations.Sort();
      this.collectionUpdates.Sort();
      this.collectionRemovals.Sort();
    }

    public void SortActions()
    {
      if (this.session.Factory.Settings.IsOrderUpdatesEnabled)
        this.updates.Sort();
      if (!this.session.Factory.Settings.IsOrderInsertsEnabled)
        return;
      this.SortInsertActions();
    }

    private void SortInsertActions()
    {
      Dictionary<int, List<EntityInsertAction>> dictionary = new Dictionary<int, List<EntityInsertAction>>();
      List<string> stringList = new List<string>();
label_15:
      while (this.insertions.Count != 0)
      {
        object insertion = (object) this.insertions[0];
        this.insertions.RemoveAt(0);
        EntityInsertAction entityInsertAction = (EntityInsertAction) insertion;
        string entityName = entityInsertAction.EntityName;
        if (!stringList.Contains(entityName))
        {
          List<EntityInsertAction> entityInsertActionList = new List<EntityInsertAction>();
          entityInsertActionList.Add(entityInsertAction);
          stringList.Add(entityName);
          dictionary[stringList.IndexOf(entityName)] = entityInsertActionList;
        }
        else
        {
          int key1 = stringList.LastIndexOf(entityName);
          foreach (object obj in entityInsertAction.State)
          {
            for (int key2 = 0; key2 < stringList.Count; ++key2)
            {
              List<EntityInsertAction> entityInsertActionList1 = dictionary[key2];
              for (int index = 0; index < entityInsertActionList1.Count; ++index)
              {
                if (entityInsertActionList1[index].Instance == obj && key2 > key1)
                {
                  List<EntityInsertAction> entityInsertActionList2 = new List<EntityInsertAction>();
                  entityInsertActionList2.Add(entityInsertAction);
                  stringList.Add(entityName);
                  dictionary[stringList.LastIndexOf(entityName)] = entityInsertActionList2;
                  goto label_15;
                }
              }
            }
          }
          dictionary[key1].Add(entityInsertAction);
        }
      }
      for (int key = 0; key < stringList.Count; ++key)
      {
        foreach (IExecutable executable in dictionary[key])
          this.insertions.Add(executable);
      }
    }

    public IList<EntityDeleteAction> CloneDeletions()
    {
      return (IList<EntityDeleteAction>) new List<EntityDeleteAction>((IEnumerable<EntityDeleteAction>) this.deletions);
    }

    public void ClearFromFlushNeededCheck(int previousCollectionRemovalSize)
    {
      this.collectionCreations.Clear();
      this.collectionUpdates.Clear();
      this.updates.Clear();
      for (int index = this.collectionRemovals.Count - 1; index >= previousCollectionRemovalSize; --index)
        this.collectionRemovals.RemoveAt(index);
    }

    public bool HasBeforeTransactionActions() => this.beforeTransactionProcesses.HasActions;

    public bool HasAfterTransactionActions() => this.afterTransactionProcesses.HasActions;

    public bool HasAnyQueuedActions
    {
      get
      {
        return this.updates.Count > 0 || this.insertions.Count > 0 || this.deletions.Count > 0 || this.collectionUpdates.Count > 0 || this.collectionRemovals.Count > 0 || this.collectionCreations.Count > 0;
      }
    }

    public override string ToString()
    {
      return new StringBuilder().Append("ActionQueue[insertions=").Append((object) this.insertions).Append(" updates=").Append((object) this.updates).Append(" deletions=").Append((object) this.deletions).Append(" collectionCreations=").Append((object) this.collectionCreations).Append(" collectionRemovals=").Append((object) this.collectionRemovals).Append(" collectionUpdates=").Append((object) this.collectionUpdates).Append("]").ToString();
    }

    [Serializable]
    private class BeforeTransactionCompletionProcessQueue
    {
      private ISessionImplementor session;
      private IList<BeforeTransactionCompletionProcessDelegate> processes = (IList<BeforeTransactionCompletionProcessDelegate>) new List<BeforeTransactionCompletionProcessDelegate>();

      public bool HasActions => this.processes.Count > 0;

      public BeforeTransactionCompletionProcessQueue(ISessionImplementor session)
      {
        this.session = session;
      }

      public void Register(BeforeTransactionCompletionProcessDelegate process)
      {
        if (process == null)
          return;
        this.processes.Add(process);
      }

      public void BeforeTransactionCompletion()
      {
        int count = this.processes.Count;
        for (int index = 0; index < count; ++index)
        {
          try
          {
            this.processes[index]();
          }
          catch (HibernateException ex)
          {
            throw ex;
          }
          catch (Exception ex)
          {
            throw new AssertionFailure("Unable to perform BeforeTransactionCompletion callback", ex);
          }
        }
        this.processes.Clear();
      }
    }

    [Serializable]
    private class AfterTransactionCompletionProcessQueue
    {
      private ISessionImplementor session;
      private ISet<string> querySpacesToInvalidate = (ISet<string>) new HashedSet<string>();
      private IList<AfterTransactionCompletionProcessDelegate> processes = (IList<AfterTransactionCompletionProcessDelegate>) new List<AfterTransactionCompletionProcessDelegate>(15);

      public bool HasActions => this.processes.Count > 0;

      public AfterTransactionCompletionProcessQueue(ISessionImplementor session)
      {
        this.session = session;
      }

      public void AddSpacesToInvalidate(string[] spaces)
      {
        if (spaces == null)
          return;
        int index = 0;
        for (int length = spaces.Length; index < length; ++index)
          this.AddSpaceToInvalidate(spaces[index]);
      }

      public void AddSpaceToInvalidate(string space) => this.querySpacesToInvalidate.Add(space);

      public void Register(AfterTransactionCompletionProcessDelegate process)
      {
        if (process == null)
          return;
        this.processes.Add(process);
      }

      public void AfterTransactionCompletion(bool success)
      {
        int count = this.processes.Count;
        for (int index = 0; index < count; ++index)
        {
          try
          {
            this.processes[index](success);
          }
          catch (CacheException ex)
          {
            ActionQueue.log.Error((object) "could not release a cache lock", (Exception) ex);
          }
          catch (Exception ex)
          {
            throw new AssertionFailure("Exception releasing cache locks", ex);
          }
        }
        this.processes.Clear();
        if (this.session.Factory.Settings.IsQueryCacheEnabled)
          this.session.Factory.UpdateTimestampsCache.Invalidate((object[]) this.querySpacesToInvalidate.ToArray<string>());
        this.querySpacesToInvalidate.Clear();
      }
    }
  }
}
