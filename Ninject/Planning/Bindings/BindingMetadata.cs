// Decompiled with JetBrains decompiler
// Type: Ninject.Planning.Bindings.BindingMetadata
// Assembly: Ninject, Version=3.0.0.0, Culture=neutral, PublicKeyToken=c7192dc5380945e7
// MVID: C76D661E-417A-4EBA-9151-4717B8101D58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Ninject.dll

using Ninject.Infrastructure;
using System.Collections.Generic;

#nullable disable
namespace Ninject.Planning.Bindings
{
  public class BindingMetadata : IBindingMetadata
  {
    private readonly Dictionary<string, object> _values = new Dictionary<string, object>();

    public string Name { get; set; }

    public bool Has(string key)
    {
      Ensure.ArgumentNotNullOrEmpty(key, nameof (key));
      return this._values.ContainsKey(key);
    }

    public T Get<T>(string key)
    {
      Ensure.ArgumentNotNullOrEmpty(key, nameof (key));
      return this.Get<T>(key, default (T));
    }

    public T Get<T>(string key, T defaultValue)
    {
      Ensure.ArgumentNotNullOrEmpty(key, nameof (key));
      return !this._values.ContainsKey(key) ? defaultValue : (T) this._values[key];
    }

    public void Set(string key, object value)
    {
      Ensure.ArgumentNotNullOrEmpty(key, nameof (key));
      this._values[key] = value;
    }
  }
}
