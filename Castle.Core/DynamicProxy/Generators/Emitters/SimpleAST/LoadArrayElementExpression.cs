// Decompiled with JetBrains decompiler
// Type: Castle.DynamicProxy.Generators.Emitters.SimpleAST.LoadArrayElementExpression
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System;
using System.Reflection.Emit;

#nullable disable
namespace Castle.DynamicProxy.Generators.Emitters.SimpleAST
{
  public class LoadArrayElementExpression : Expression
  {
    private readonly ConstReference index;
    private readonly Reference arrayReference;
    private readonly Type returnType;

    public LoadArrayElementExpression(int index, Reference arrayReference, Type returnType)
      : this(new ConstReference((object) index), arrayReference, returnType)
    {
    }

    public LoadArrayElementExpression(
      ConstReference index,
      Reference arrayReference,
      Type returnType)
    {
      this.index = index;
      this.arrayReference = arrayReference;
      this.returnType = returnType;
    }

    public override void Emit(IMemberEmitter member, ILGenerator gen)
    {
      ArgumentsUtil.EmitLoadOwnerAndReference(this.arrayReference, gen);
      ArgumentsUtil.EmitLoadOwnerAndReference((Reference) this.index, gen);
      gen.Emit(OpCodes.Ldelem, this.returnType);
    }
  }
}
