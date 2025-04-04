// Decompiled with JetBrains decompiler
// Type: Ninject.INinjectSettings
// Assembly: Ninject, Version=3.0.0.0, Culture=neutral, PublicKeyToken=c7192dc5380945e7
// MVID: C76D661E-417A-4EBA-9151-4717B8101D58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Ninject.dll

using Ninject.Activation;
using System;

#nullable disable
namespace Ninject
{
  public interface INinjectSettings
  {
    Type InjectAttribute { get; }

    TimeSpan CachePruningInterval { get; }

    Func<IContext, object> DefaultScopeCallback { get; }

    bool LoadExtensions { get; }

    string[] ExtensionSearchPatterns { get; }

    bool UseReflectionBasedInjection { get; }

    bool InjectNonPublic { get; set; }

    bool InjectParentPrivateProperties { get; set; }

    bool ActivationCacheDisabled { get; set; }

    bool AllowNullInjection { get; set; }

    T Get<T>(string key, T defaultValue);

    void Set(string key, object value);
  }
}
