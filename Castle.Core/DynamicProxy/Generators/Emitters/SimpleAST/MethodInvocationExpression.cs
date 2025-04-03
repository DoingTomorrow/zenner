// Decompiled with JetBrains decompiler
// Type: Castle.DynamicProxy.Generators.Emitters.SimpleAST.MethodInvocationExpression
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System.Reflection;
using System.Reflection.Emit;

#nullable disable
namespace Castle.DynamicProxy.Generators.Emitters.SimpleAST
{
  public class MethodInvocationExpression : Expression
  {
    protected readonly MethodInfo method;
    protected readonly Expression[] args;
    protected readonly Reference owner;

    public MethodInvocationExpression(MethodInfo method, params Expression[] args)
      : this((Reference) SelfReference.Self, method, args)
    {
    }

    public MethodInvocationExpression(MethodEmitter method, params Expression[] args)
      : this((Reference) SelfReference.Self, (MethodInfo) method.MethodBuilder, args)
    {
    }

    public MethodInvocationExpression(
      Reference owner,
      MethodEmitter method,
      params Expression[] args)
      : this(owner, (MethodInfo) method.MethodBuilder, args)
    {
    }

    public MethodInvocationExpression(Reference owner, MethodInfo method, params Expression[] args)
    {
      this.owner = owner;
      this.method = method;
      this.args = args;
    }

    public bool VirtualCall { get; set; }

    public override void Emit(IMemberEmitter member, ILGenerator gen)
    {
      ArgumentsUtil.EmitLoadOwnerAndReference(this.owner, gen);
      foreach (Expression expression in this.args)
        expression.Emit(member, gen);
      if (this.VirtualCall)
        gen.Emit(OpCodes.Callvirt, this.method);
      else
        gen.Emit(OpCodes.Call, this.method);
    }
  }
}
