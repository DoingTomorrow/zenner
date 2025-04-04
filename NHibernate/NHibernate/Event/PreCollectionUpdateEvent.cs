// Decompiled with JetBrains decompiler
// Type: NHibernate.Event.PreCollectionUpdateEvent
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Collection;
using NHibernate.Persister.Collection;
using System;

#nullable disable
namespace NHibernate.Event
{
  [Serializable]
  public class PreCollectionUpdateEvent(
    ICollectionPersister collectionPersister,
    IPersistentCollection collection,
    IEventSource source) : AbstractCollectionEvent(collectionPersister, collection, source, AbstractCollectionEvent.GetLoadedOwnerOrNull(collection, source), AbstractCollectionEvent.GetLoadedOwnerIdOrNull(collection, source))
  {
  }
}
