// Decompiled with JetBrains decompiler
// Type: Ninject.NinjectSettings
// Assembly: Ninject, Version=3.0.0.0, Culture=neutral, PublicKeyToken=c7192dc5380945e7
// MVID: C76D661E-417A-4EBA-9151-4717B8101D58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Ninject.dll

using Ninject.Activation;
using Ninject.Infrastructure;
using System;
using System.Collections.Generic;

#nullable disable
namespace Ninject
{
  public class NinjectSettings : INinjectSettings
  {
    private readonly Dictionary<string, object> _values = new Dictionary<string, object>();

    public Type InjectAttribute
    {
      get => this.Get<Type>(nameof (InjectAttribute), typeof (Ninject.InjectAttribute));
      set => this.Set(nameof (InjectAttribute), (object) value);
    }

    public TimeSpan CachePruningInterval
    {
      get => this.Get<TimeSpan>(nameof (CachePruningInterval), TimeSpan.FromSeconds(30.0));
      set => this.Set(nameof (CachePruningInterval), (object) value);
    }

    public Func<IContext, object> DefaultScopeCallback
    {
      get
      {
        return this.Get<Func<IContext, object>>(nameof (DefaultScopeCallback), StandardScopeCallbacks.Transient);
      }
      set => this.Set(nameof (DefaultScopeCallback), (object) value);
    }

    public bool LoadExtensions
    {
      get => this.Get<bool>(nameof (LoadExtensions), true);
      set => this.Set(nameof (LoadExtensions), (object) value);
    }

    public string[] ExtensionSearchPatterns
    {
      get
      {
        return this.Get<string[]>(nameof (ExtensionSearchPatterns), new string[2]
        {
          "Ninject.Extensions.*.dll",
          "Ninject.Web*.dll"
        });
      }
      set => this.Set(nameof (ExtensionSearchPatterns), (object) value);
    }

    public bool UseReflectionBasedInjection
    {
      get => this.Get<bool>(nameof (UseReflectionBasedInjection), false);
      set => this.Set(nameof (UseReflectionBasedInjection), (object) value);
    }

    public bool InjectNonPublic
    {
      get => this.Get<bool>(nameof (InjectNonPublic), false);
      set => this.Set(nameof (InjectNonPublic), (object) value);
    }

    public bool InjectParentPrivateProperties
    {
      get => this.Get<bool>(nameof (InjectParentPrivateProperties), false);
      set => this.Set(nameof (InjectParentPrivateProperties), (object) value);
    }

    public bool ActivationCacheDisabled
    {
      get => this.Get<bool>(nameof (ActivationCacheDisabled), false);
      set => this.Set(nameof (ActivationCacheDisabled), (object) value);
    }

    public bool AllowNullInjection
    {
      get => this.Get<bool>(nameof (AllowNullInjection), false);
      set => this.Set(nameof (AllowNullInjection), (object) value);
    }

    public T Get<T>(string key, T defaultValue)
    {
      object obj;
      return !this._values.TryGetValue(key, out obj) ? defaultValue : (T) obj;
    }

    public void Set(string key, object value) => this._values[key] = value;
  }
}
