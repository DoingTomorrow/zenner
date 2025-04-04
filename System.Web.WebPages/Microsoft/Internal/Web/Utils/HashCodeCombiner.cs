// Decompiled with JetBrains decompiler
// Type: Microsoft.Internal.Web.Utils.HashCodeCombiner
// Assembly: System.Web.WebPages, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 40C0730F-DB39-4BE9-B184-1864656F2572
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.WebPages.dll

using System.Collections;

#nullable disable
namespace Microsoft.Internal.Web.Utils
{
  internal class HashCodeCombiner
  {
    private long _combinedHash64 = 5381;

    public int CombinedHash => this._combinedHash64.GetHashCode();

    public HashCodeCombiner Add(IEnumerable e)
    {
      if (e == null)
      {
        this.Add(0);
      }
      else
      {
        int i = 0;
        foreach (object o in e)
        {
          this.Add(o);
          ++i;
        }
        this.Add(i);
      }
      return this;
    }

    public HashCodeCombiner Add(int i)
    {
      this._combinedHash64 = (this._combinedHash64 << 5) + this._combinedHash64 ^ (long) i;
      return this;
    }

    public HashCodeCombiner Add(object o)
    {
      this.Add(o != null ? o.GetHashCode() : 0);
      return this;
    }

    public static HashCodeCombiner Start() => new HashCodeCombiner();
  }
}
