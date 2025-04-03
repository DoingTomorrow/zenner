// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.TypeNameStringComparer
// Assembly: System.Data.SQLite, Version=1.0.103.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 386C6C7E-4AF4-46DD-83BA-B8B7485E47C2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Data.SQLite.dll

using System.Collections.Generic;

#nullable disable
namespace System.Data.SQLite
{
  internal sealed class TypeNameStringComparer : IEqualityComparer<string>, IComparer<string>
  {
    public bool Equals(string left, string right)
    {
      return string.Equals(left, right, StringComparison.OrdinalIgnoreCase);
    }

    public int GetHashCode(string value)
    {
      return value != null ? value.ToLowerInvariant().GetHashCode() : throw new ArgumentNullException(nameof (value));
    }

    public int Compare(string x, string y)
    {
      if (x == null && y == null)
        return 0;
      if (x == null)
        return -1;
      return y == null ? 1 : x.CompareTo(y);
    }
  }
}
