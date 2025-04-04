// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.ExpressionUtil.ExpressionFingerprintChain
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Collections.Generic;

#nullable disable
namespace System.Web.Mvc.ExpressionUtil
{
  internal sealed class ExpressionFingerprintChain : IEquatable<ExpressionFingerprintChain>
  {
    public readonly List<ExpressionFingerprint> Elements = new List<ExpressionFingerprint>();

    public bool Equals(ExpressionFingerprintChain other)
    {
      if (other == null || this.Elements.Count != other.Elements.Count)
        return false;
      for (int index = 0; index < this.Elements.Count; ++index)
      {
        if (!object.Equals((object) this.Elements[index], (object) other.Elements[index]))
          return false;
      }
      return true;
    }

    public override bool Equals(object obj) => this.Equals(obj as ExpressionFingerprintChain);

    public override int GetHashCode()
    {
      HashCodeCombiner hashCodeCombiner = new HashCodeCombiner();
      this.Elements.ForEach(new Action<ExpressionFingerprint>(hashCodeCombiner.AddFingerprint));
      return hashCodeCombiner.CombinedHash;
    }
  }
}
