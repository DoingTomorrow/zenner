// Decompiled with JetBrains decompiler
// Type: NHibernate.Event.InitializeCollectionEvent
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Collection;
using System;

#nullable disable
namespace NHibernate.Event
{
  [Serializable]
  public class InitializeCollectionEvent(IPersistentCollection collection, IEventSource source) : 
    AbstractCollectionEvent(AbstractCollectionEvent.GetLoadedCollectionPersister(collection, source), collection, source, AbstractCollectionEvent.GetLoadedOwnerOrNull(collection, source), AbstractCollectionEvent.GetLoadedOwnerIdOrNull(collection, source))
  {
  }
}
