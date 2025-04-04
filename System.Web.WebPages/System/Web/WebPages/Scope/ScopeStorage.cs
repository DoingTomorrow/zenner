// Decompiled with JetBrains decompiler
// Type: System.Web.WebPages.Scope.ScopeStorage
// Assembly: System.Web.WebPages, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 40C0730F-DB39-4BE9-B184-1864656F2572
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.WebPages.dll

using System.Collections.Generic;

#nullable disable
namespace System.Web.WebPages.Scope
{
  public static class ScopeStorage
  {
    private static readonly IScopeStorageProvider _defaultStorageProvider = (IScopeStorageProvider) new StaticScopeStorageProvider();
    private static IScopeStorageProvider _stateStorageProvider;

    public static IScopeStorageProvider CurrentProvider
    {
      get => ScopeStorage._stateStorageProvider ?? ScopeStorage._defaultStorageProvider;
      set => ScopeStorage._stateStorageProvider = value;
    }

    public static IDictionary<object, object> CurrentScope
    {
      get => ScopeStorage.CurrentProvider.CurrentScope;
    }

    public static IDictionary<object, object> GlobalScope
    {
      get => ScopeStorage.CurrentProvider.GlobalScope;
    }

    public static IDisposable CreateTransientScope(IDictionary<object, object> context)
    {
      IDictionary<object, object> currentContext = ScopeStorage.CurrentScope;
      ScopeStorage.CurrentProvider.CurrentScope = context;
      return (IDisposable) new DisposableAction((Action) (() => ScopeStorage.CurrentProvider.CurrentScope = currentContext));
    }

    public static IDisposable CreateTransientScope()
    {
      return ScopeStorage.CreateTransientScope((IDictionary<object, object>) new ScopeStorageDictionary(ScopeStorage.CurrentScope));
    }
  }
}
