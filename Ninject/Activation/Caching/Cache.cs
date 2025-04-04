// Decompiled with JetBrains decompiler
// Type: Ninject.Activation.Caching.Cache
// Assembly: Ninject, Version=3.0.0.0, Culture=neutral, PublicKeyToken=c7192dc5380945e7
// MVID: C76D661E-417A-4EBA-9151-4717B8101D58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Ninject.dll

using Ninject.Components;
using Ninject.Infrastructure;
using Ninject.Infrastructure.Disposal;
using Ninject.Planning.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Ninject.Activation.Caching
{
  public class Cache : NinjectComponent, ICache, INinjectComponent, IDisposable, IPruneable
  {
    private readonly IDictionary<object, Multimap<IBindingConfiguration, Cache.CacheEntry>> entries = (IDictionary<object, Multimap<IBindingConfiguration, Cache.CacheEntry>>) new Dictionary<object, Multimap<IBindingConfiguration, Cache.CacheEntry>>();

    public Cache(IPipeline pipeline, ICachePruner cachePruner)
    {
      Ensure.ArgumentNotNull((object) pipeline, nameof (pipeline));
      Ensure.ArgumentNotNull((object) cachePruner, nameof (cachePruner));
      this.Pipeline = pipeline;
      cachePruner.Start((IPruneable) this);
    }

    public IPipeline Pipeline { get; private set; }

    public int Count => this.GetAllCacheEntries().Count<Cache.CacheEntry>();

    public override void Dispose(bool disposing)
    {
      if (disposing && !this.IsDisposed)
        this.Clear();
      base.Dispose(disposing);
    }

    public void Remember(IContext context, InstanceReference reference)
    {
      Ensure.ArgumentNotNull((object) context, nameof (context));
      object scope = context.GetScope();
      Cache.CacheEntry cacheEntry = new Cache.CacheEntry(context, reference);
      lock (this.entries)
      {
        ReferenceEqualWeakReference weakScopeReference = new ReferenceEqualWeakReference(scope);
        if (!this.entries.ContainsKey((object) weakScopeReference))
        {
          this.entries[(object) weakScopeReference] = new Multimap<IBindingConfiguration, Cache.CacheEntry>();
          if (scope is INotifyWhenDisposed notifyWhenDisposed)
          {
            EventHandler eventHandler = (EventHandler) ((o, e) => this.Clear((object) weakScopeReference));
            notifyWhenDisposed.Disposed += eventHandler;
          }
        }
        this.entries[(object) weakScopeReference].Add(context.Binding.BindingConfiguration, cacheEntry);
      }
    }

    public object TryGet(IContext context)
    {
      Ensure.ArgumentNotNull((object) context, nameof (context));
      object scope = context.GetScope();
      if (scope == null)
        return (object) null;
      lock (this.entries)
      {
        Multimap<IBindingConfiguration, Cache.CacheEntry> multimap;
        if (!this.entries.TryGetValue(scope, out multimap))
          return (object) null;
        foreach (Cache.CacheEntry cacheEntry in (IEnumerable<Cache.CacheEntry>) multimap[context.Binding.BindingConfiguration])
        {
          if (!context.HasInferredGenericArguments || ((IEnumerable<Type>) cacheEntry.Context.GenericArguments).SequenceEqual<Type>((IEnumerable<Type>) context.GenericArguments))
            return cacheEntry.Reference.Instance;
        }
        return (object) null;
      }
    }

    public bool Release(object instance)
    {
      lock (this.entries)
      {
        bool flag = false;
        foreach (ICollection<Cache.CacheEntry> source in this.entries.Values.SelectMany<Multimap<IBindingConfiguration, Cache.CacheEntry>, ICollection<Cache.CacheEntry>>((Func<Multimap<IBindingConfiguration, Cache.CacheEntry>, IEnumerable<ICollection<Cache.CacheEntry>>>) (bindingEntries => (IEnumerable<ICollection<Cache.CacheEntry>>) bindingEntries.Values)))
        {
          foreach (Cache.CacheEntry entry in source.Where<Cache.CacheEntry>((Func<Cache.CacheEntry, bool>) (cacheEntry => object.ReferenceEquals(instance, cacheEntry.Reference.Instance))).ToList<Cache.CacheEntry>())
          {
            this.Forget(entry);
            source.Remove(entry);
            flag = true;
          }
        }
        return flag;
      }
    }

    public void Prune()
    {
      lock (this.entries)
      {
        foreach (KeyValuePair<object, Multimap<IBindingConfiguration, Cache.CacheEntry>> keyValuePair in this.entries.Where<KeyValuePair<object, Multimap<IBindingConfiguration, Cache.CacheEntry>>>((Func<KeyValuePair<object, Multimap<IBindingConfiguration, Cache.CacheEntry>>, bool>) (scope => !((ReferenceEqualWeakReference) scope.Key).IsAlive)).Select<KeyValuePair<object, Multimap<IBindingConfiguration, Cache.CacheEntry>>, KeyValuePair<object, Multimap<IBindingConfiguration, Cache.CacheEntry>>>((Func<KeyValuePair<object, Multimap<IBindingConfiguration, Cache.CacheEntry>>, KeyValuePair<object, Multimap<IBindingConfiguration, Cache.CacheEntry>>>) (scope => scope)).ToList<KeyValuePair<object, Multimap<IBindingConfiguration, Cache.CacheEntry>>>())
        {
          this.Forget(Cache.GetAllBindingEntries((IEnumerable<KeyValuePair<IBindingConfiguration, ICollection<Cache.CacheEntry>>>) keyValuePair.Value));
          this.entries.Remove(keyValuePair.Key);
        }
      }
    }

    public void Clear(object scope)
    {
      lock (this.entries)
      {
        Multimap<IBindingConfiguration, Cache.CacheEntry> bindings;
        if (!this.entries.TryGetValue(scope, out bindings))
          return;
        this.Forget(Cache.GetAllBindingEntries((IEnumerable<KeyValuePair<IBindingConfiguration, ICollection<Cache.CacheEntry>>>) bindings));
        this.entries.Remove(scope);
      }
    }

    public void Clear()
    {
      lock (this.entries)
      {
        this.Forget(this.GetAllCacheEntries());
        this.entries.Clear();
      }
    }

    private static IEnumerable<Cache.CacheEntry> GetAllBindingEntries(
      IEnumerable<KeyValuePair<IBindingConfiguration, ICollection<Cache.CacheEntry>>> bindings)
    {
      return bindings.SelectMany<KeyValuePair<IBindingConfiguration, ICollection<Cache.CacheEntry>>, Cache.CacheEntry>((Func<KeyValuePair<IBindingConfiguration, ICollection<Cache.CacheEntry>>, IEnumerable<Cache.CacheEntry>>) (bindingEntries => (IEnumerable<Cache.CacheEntry>) bindingEntries.Value));
    }

    private IEnumerable<Cache.CacheEntry> GetAllCacheEntries()
    {
      return this.entries.SelectMany<KeyValuePair<object, Multimap<IBindingConfiguration, Cache.CacheEntry>>, Cache.CacheEntry>((Func<KeyValuePair<object, Multimap<IBindingConfiguration, Cache.CacheEntry>>, IEnumerable<Cache.CacheEntry>>) (scopeCache => Cache.GetAllBindingEntries((IEnumerable<KeyValuePair<IBindingConfiguration, ICollection<Cache.CacheEntry>>>) scopeCache.Value)));
    }

    private void Forget(IEnumerable<Cache.CacheEntry> cacheEntries)
    {
      foreach (Cache.CacheEntry entry in cacheEntries.ToList<Cache.CacheEntry>())
        this.Forget(entry);
    }

    private void Forget(Cache.CacheEntry entry)
    {
      this.Clear(entry.Reference.Instance);
      this.Pipeline.Deactivate(entry.Context, entry.Reference);
    }

    private class CacheEntry
    {
      public CacheEntry(IContext context, InstanceReference reference)
      {
        this.Context = context;
        this.Reference = reference;
      }

      public IContext Context { get; private set; }

      public InstanceReference Reference { get; private set; }
    }
  }
}
