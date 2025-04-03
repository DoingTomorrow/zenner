// Decompiled with JetBrains decompiler
// Type: Castle.DynamicProxy.Generators.Emitters.SimpleAST.AssignArrayStatement
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System.Reflection.Emit;

#nullable disable
namespace Castle.DynamicProxy.Generators.Emitters.SimpleAST
{
  public class AssignArrayStatement : Statement
  {
    private readonly Reference targetArray;
    private readonly int targetPosition;
    private readonly Expression value;

    public AssignArrayStatement(Reference targetArray, int targetPosition, Expression value)
    {
      this.targetArray = targetArray;
      this.targetPosition = targetPosition;
      this.value = value;
    }

    public override void Emit(IMemberEmitter member, ILGenerator il)
    {
      ArgumentsUtil.EmitLoadOwnerAndReference(this.targetArray, il);
      il.Emit(OpCodes.Ldc_I4, this.targetPosition);
      this.value.Emit(member, il);
      il.Emit(OpCodes.Stelem_Ref);
    }
  }
}
