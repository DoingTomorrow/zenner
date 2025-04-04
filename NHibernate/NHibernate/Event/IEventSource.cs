// Decompiled with JetBrains decompiler
// Type: NHibernate.Event.IEventSource
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Iesi.Collections;
using NHibernate.Engine;
using NHibernate.Persister.Entity;
using System;
using System.Collections;

#nullable disable
namespace NHibernate.Event
{
  public interface IEventSource : ISessionImplementor, ISession, IDisposable
  {
    ActionQueue ActionQueue { get; }

    object Instantiate(IEntityPersister persister, object id);

    void ForceFlush(EntityEntry e);

    void Merge(string entityName, object obj, IDictionary copiedAlready);

    void Persist(string entityName, object obj, IDictionary createdAlready);

    void PersistOnFlush(string entityName, object obj, IDictionary copiedAlready);

    void Refresh(object obj, IDictionary refreshedAlready);

    [Obsolete("Use Merge(string, object, IDictionary) instead")]
    void SaveOrUpdateCopy(string entityName, object obj, IDictionary copiedAlready);

    void Delete(
      string entityName,
      object child,
      bool isCascadeDeleteEnabled,
      ISet transientEntities);
  }
}
