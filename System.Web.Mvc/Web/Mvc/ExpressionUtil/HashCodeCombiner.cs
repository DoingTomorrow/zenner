// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.ExpressionUtil.HashCodeCombiner
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Collections;

#nullable disable
namespace System.Web.Mvc.ExpressionUtil
{
  internal class HashCodeCombiner
  {
    private long _combinedHash64 = 5381;

    public int CombinedHash => this._combinedHash64.GetHashCode();

    public void AddFingerprint(ExpressionFingerprint fingerprint)
    {
      if (fingerprint != null)
        fingerprint.AddToHashCodeCombiner(this);
      else
        this.AddInt32(0);
    }

    public void AddEnumerable(IEnumerable e)
    {
      if (e == null)
      {
        this.AddInt32(0);
      }
      else
      {
        int i = 0;
        foreach (object o in e)
        {
          this.AddObject(o);
          ++i;
        }
        this.AddInt32(i);
      }
    }

    public void AddInt32(int i)
    {
      this._combinedHash64 = (this._combinedHash64 << 5) + this._combinedHash64 ^ (long) i;
    }

    public void AddObject(object o) => this.AddInt32(o != null ? o.GetHashCode() : 0);
  }
}
