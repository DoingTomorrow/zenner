// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.ExpressionUtil.MethodCallExpressionFingerprint
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Linq.Expressions;
using System.Reflection;

#nullable disable
namespace System.Web.Mvc.ExpressionUtil
{
  internal sealed class MethodCallExpressionFingerprint : ExpressionFingerprint
  {
    public MethodCallExpressionFingerprint(ExpressionType nodeType, Type type, MethodInfo method)
      : base(nodeType, type)
    {
      this.Method = method;
    }

    public MethodInfo Method { get; private set; }

    public override bool Equals(object obj)
    {
      return obj is MethodCallExpressionFingerprint other && object.Equals((object) this.Method, (object) other.Method) && this.Equals((ExpressionFingerprint) other);
    }

    internal override void AddToHashCodeCombiner(HashCodeCombiner combiner)
    {
      combiner.AddObject((object) this.Method);
      base.AddToHashCodeCombiner(combiner);
    }
  }
}
