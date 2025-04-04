// Decompiled with JetBrains decompiler
// Type: System.Web.WebPages.Scope.StaticScopeStorageProvider
// Assembly: System.Web.WebPages, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 40C0730F-DB39-4BE9-B184-1864656F2572
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.WebPages.dll

using System.Collections.Concurrent;
using System.Collections.Generic;

#nullable disable
namespace System.Web.WebPages.Scope
{
  public class StaticScopeStorageProvider : IScopeStorageProvider
  {
    private static readonly IDictionary<object, object> _defaultContext = (IDictionary<object, object>) new ScopeStorageDictionary((IDictionary<object, object>) null, (IDictionary<object, object>) new ConcurrentDictionary<object, object>(ScopeStorageComparer.Instance));
    private IDictionary<object, object> _currentContext;

    public IDictionary<object, object> CurrentScope
    {
      get => this._currentContext ?? StaticScopeStorageProvider._defaultContext;
      set => this._currentContext = value;
    }

    public IDictionary<object, object> GlobalScope => StaticScopeStorageProvider._defaultContext;
  }
}
