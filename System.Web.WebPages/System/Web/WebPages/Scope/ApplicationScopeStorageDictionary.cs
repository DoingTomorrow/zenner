// Decompiled with JetBrains decompiler
// Type: System.Web.WebPages.Scope.ApplicationScopeStorageDictionary
// Assembly: System.Web.WebPages, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 40C0730F-DB39-4BE9-B184-1864656F2572
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.WebPages.dll

using System.Collections.Concurrent;
using System.Collections.Generic;

#nullable disable
namespace System.Web.WebPages.Scope
{
  internal class ApplicationScopeStorageDictionary(WebConfigScopeDictionary webConfigState) : 
    ScopeStorageDictionary((IDictionary<object, object>) webConfigState, ApplicationScopeStorageDictionary._innerDictionary)
  {
    private static readonly IDictionary<object, object> _innerDictionary = (IDictionary<object, object>) new ConcurrentDictionary<object, object>(ScopeStorageComparer.Instance);

    public ApplicationScopeStorageDictionary()
      : this(new WebConfigScopeDictionary())
    {
    }
  }
}
