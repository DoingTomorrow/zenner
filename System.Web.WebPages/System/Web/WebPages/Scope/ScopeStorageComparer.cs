// Decompiled with JetBrains decompiler
// Type: System.Web.WebPages.Scope.ScopeStorageComparer
// Assembly: System.Web.WebPages, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 40C0730F-DB39-4BE9-B184-1864656F2572
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.WebPages.dll

using System.Collections.Generic;

#nullable disable
namespace System.Web.WebPages.Scope
{
  internal class ScopeStorageComparer : IEqualityComparer<object>
  {
    private static IEqualityComparer<object> _instance;
    private readonly IEqualityComparer<object> _defaultComparer = (IEqualityComparer<object>) EqualityComparer<object>.Default;
    private readonly IEqualityComparer<string> _stringComparer = (IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase;

    private ScopeStorageComparer()
    {
    }

    public static IEqualityComparer<object> Instance
    {
      get
      {
        if (ScopeStorageComparer._instance == null)
          ScopeStorageComparer._instance = (IEqualityComparer<object>) new ScopeStorageComparer();
        return ScopeStorageComparer._instance;
      }
    }

    public bool Equals(object x, object y)
    {
      string x1 = x as string;
      string y1 = y as string;
      return x1 != null && y1 != null ? this._stringComparer.Equals(x1, y1) : this._defaultComparer.Equals(x, y);
    }

    public int GetHashCode(object obj)
    {
      return obj is string str ? this._stringComparer.GetHashCode(str) : this._defaultComparer.GetHashCode(obj);
    }
  }
}
