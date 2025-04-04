// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.ExpressionUtil.IndexExpressionFingerprint
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Linq.Expressions;
using System.Reflection;

#nullable disable
namespace System.Web.Mvc.ExpressionUtil
{
  internal sealed class IndexExpressionFingerprint : ExpressionFingerprint
  {
    public IndexExpressionFingerprint(ExpressionType nodeType, Type type, PropertyInfo indexer)
      : base(nodeType, type)
    {
      this.Indexer = indexer;
    }

    public PropertyInfo Indexer { get; private set; }

    public override bool Equals(object obj)
    {
      return obj is IndexExpressionFingerprint other && object.Equals((object) this.Indexer, (object) other.Indexer) && this.Equals((ExpressionFingerprint) other);
    }

    internal override void AddToHashCodeCombiner(HashCodeCombiner combiner)
    {
      combiner.AddObject((object) this.Indexer);
      base.AddToHashCodeCombiner(combiner);
    }
  }
}
