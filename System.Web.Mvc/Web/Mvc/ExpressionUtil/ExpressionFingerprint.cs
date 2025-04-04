// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.ExpressionUtil.ExpressionFingerprint
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Linq.Expressions;

#nullable disable
namespace System.Web.Mvc.ExpressionUtil
{
  internal abstract class ExpressionFingerprint
  {
    protected ExpressionFingerprint(ExpressionType nodeType, Type type)
    {
      this.NodeType = nodeType;
      this.Type = type;
    }

    public ExpressionType NodeType { get; private set; }

    public Type Type { get; private set; }

    internal virtual void AddToHashCodeCombiner(HashCodeCombiner combiner)
    {
      combiner.AddInt32((int) this.NodeType);
      combiner.AddObject((object) this.Type);
    }

    protected bool Equals(ExpressionFingerprint other)
    {
      return other != null && this.NodeType == other.NodeType && object.Equals((object) this.Type, (object) other.Type);
    }

    public override bool Equals(object obj) => this.Equals(obj as ExpressionFingerprint);

    public override int GetHashCode()
    {
      HashCodeCombiner combiner = new HashCodeCombiner();
      this.AddToHashCodeCombiner(combiner);
      return combiner.CombinedHash;
    }
  }
}
