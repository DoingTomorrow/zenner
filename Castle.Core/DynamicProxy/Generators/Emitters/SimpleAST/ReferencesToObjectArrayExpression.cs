// Decompiled with JetBrains decompiler
// Type: Castle.DynamicProxy.Generators.Emitters.SimpleAST.ReferencesToObjectArrayExpression
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System;
using System.Reflection.Emit;

#nullable disable
namespace Castle.DynamicProxy.Generators.Emitters.SimpleAST
{
  public class ReferencesToObjectArrayExpression : Expression
  {
    private readonly TypeReference[] args;

    public ReferencesToObjectArrayExpression(params TypeReference[] args) => this.args = args;

    public override void Emit(IMemberEmitter member, ILGenerator gen)
    {
      LocalBuilder local = gen.DeclareLocal(typeof (object[]));
      gen.Emit(OpCodes.Ldc_I4, this.args.Length);
      gen.Emit(OpCodes.Newarr, typeof (object));
      gen.Emit(OpCodes.Stloc, local);
      for (int index = 0; index < this.args.Length; ++index)
      {
        gen.Emit(OpCodes.Ldloc, local);
        gen.Emit(OpCodes.Ldc_I4, index);
        TypeReference typeReference = this.args[index];
        ArgumentsUtil.EmitLoadOwnerAndReference((Reference) typeReference, gen);
        if (typeReference.Type.IsByRef)
          throw new NotSupportedException();
        if (typeReference.Type.IsValueType)
          gen.Emit(OpCodes.Box, typeReference.Type.UnderlyingSystemType);
        if (typeReference.Type.IsGenericParameter)
          gen.Emit(OpCodes.Box, typeReference.Type);
        gen.Emit(OpCodes.Stelem_Ref);
      }
      gen.Emit(OpCodes.Ldloc, local);
    }
  }
}
