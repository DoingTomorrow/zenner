// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.Utilities.RegistryBase`3
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Remotion.Linq.Utilities
{
  public abstract class RegistryBase<TRegistry, TKey, TItem> where TRegistry : RegistryBase<TRegistry, TKey, TItem>, new()
  {
    private readonly Dictionary<TKey, TItem> _items = new Dictionary<TKey, TItem>();

    public static TRegistry CreateDefault()
    {
      IEnumerable<Type> itemTypes = ((IEnumerable<Type>) typeof (TRegistry).Assembly.GetTypes()).Where<Type>((Func<Type, bool>) (t => typeof (TItem).IsAssignableFrom(t) && !t.IsAbstract));
      TRegistry registry = new TRegistry();
      registry.RegisterForTypes(itemTypes);
      return registry;
    }

    public virtual void Register(TKey key, TItem item)
    {
      ArgumentUtility.CheckNotNull<TKey>(nameof (key), key);
      ArgumentUtility.CheckNotNull<TItem>(nameof (item), item);
      this._items[key] = item;
    }

    public void Register(IEnumerable<TKey> keys, TItem item)
    {
      ArgumentUtility.CheckNotNull<IEnumerable<TKey>>(nameof (keys), keys);
      ArgumentUtility.CheckNotNull<TItem>(nameof (item), item);
      foreach (TKey key in keys)
        this.Register(key, item);
    }

    public abstract TItem GetItem(TKey key);

    public virtual bool IsRegistered(TKey key) => this._items.ContainsKey(key);

    protected virtual TItem GetItemExact(TKey key)
    {
      TItem itemExact;
      this._items.TryGetValue(key, out itemExact);
      return itemExact;
    }

    protected abstract void RegisterForTypes(IEnumerable<Type> itemTypes);
  }
}
