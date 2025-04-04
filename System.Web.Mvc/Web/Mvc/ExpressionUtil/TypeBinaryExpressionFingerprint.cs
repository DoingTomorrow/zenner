// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.ExpressionUtil.TypeBinaryExpressionFingerprint
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Linq.Expressions;

#nullable disable
namespace System.Web.Mvc.ExpressionUtil
{
  internal sealed class TypeBinaryExpressionFingerprint : ExpressionFingerprint
  {
    public TypeBinaryExpressionFingerprint(ExpressionType nodeType, Type type, Type typeOperand)
      : base(nodeType, type)
    {
      this.TypeOperand = typeOperand;
    }

    public Type TypeOperand { get; private set; }

    public override bool Equals(object obj)
    {
      return obj is TypeBinaryExpressionFingerprint other && object.Equals((object) this.TypeOperand, (object) other.TypeOperand) && this.Equals((ExpressionFingerprint) other);
    }

    internal override void AddToHashCodeCombiner(HashCodeCombiner combiner)
    {
      combiner.AddObject((object) this.TypeOperand);
      base.AddToHashCodeCombiner(combiner);
    }
  }
}
