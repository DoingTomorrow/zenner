// Decompiled with JetBrains decompiler
// Type: NHibernate.Cache.Entry.CacheEntry
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.Event;
using NHibernate.Persister.Entity;
using NHibernate.Type;
using System;

#nullable disable
namespace NHibernate.Cache.Entry
{
  [Serializable]
  public sealed class CacheEntry
  {
    private readonly object[] disassembledState;
    private readonly string subclass;
    private readonly bool lazyPropertiesAreUnfetched;
    private readonly object version;

    public CacheEntry(
      object[] state,
      IEntityPersister persister,
      bool unfetched,
      object version,
      ISessionImplementor session,
      object owner)
    {
      this.disassembledState = TypeHelper.Disassemble(state, (ICacheAssembler[]) persister.PropertyTypes, (bool[]) null, session, owner);
      this.subclass = persister.EntityName;
      this.lazyPropertiesAreUnfetched = unfetched || !persister.IsLazyPropertiesCacheable;
      this.version = version;
    }

    internal CacheEntry(object[] state, string subclass, bool unfetched, object version)
    {
      this.disassembledState = state;
      this.subclass = subclass;
      this.lazyPropertiesAreUnfetched = unfetched;
      this.version = version;
    }

    public object Version => this.version;

    public string Subclass => this.subclass;

    public bool AreLazyPropertiesUnfetched => this.lazyPropertiesAreUnfetched;

    public object[] DisassembledState => this.disassembledState;

    public object[] Assemble(
      object instance,
      object id,
      IEntityPersister persister,
      IInterceptor interceptor,
      ISessionImplementor session)
    {
      if (!persister.EntityName.Equals(this.subclass))
        throw new AssertionFailure("Tried to assemble a different subclass instance");
      return CacheEntry.Assemble(this.disassembledState, instance, id, persister, interceptor, session);
    }

    private static object[] Assemble(
      object[] values,
      object result,
      object id,
      IEntityPersister persister,
      IInterceptor interceptor,
      ISessionImplementor session)
    {
      object[] values1 = TypeHelper.Assemble(values, (ICacheAssembler[]) persister.PropertyTypes, session, result);
      PreLoadEvent @event = new PreLoadEvent((IEventSource) session);
      @event.Entity = result;
      @event.State = values1;
      @event.Id = id;
      @event.Persister = persister;
      foreach (IPreLoadEventListener loadEventListener in session.Listeners.PreLoadEventListeners)
        loadEventListener.OnPreLoad(@event);
      persister.SetPropertyValues(result, values1, session.EntityMode);
      return values1;
    }
  }
}
