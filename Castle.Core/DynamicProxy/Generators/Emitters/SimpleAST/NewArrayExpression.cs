// Decompiled with JetBrains decompiler
// Type: Castle.DynamicProxy.Generators.Emitters.SimpleAST.NewArrayExpression
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System;
using System.Reflection.Emit;

#nullable disable
namespace Castle.DynamicProxy.Generators.Emitters.SimpleAST
{
  public class NewArrayExpression : Expression
  {
    private readonly int size;
    private readonly Type arrayType;

    public NewArrayExpression(int size, Type arrayType)
    {
      this.size = size;
      this.arrayType = arrayType;
    }

    public override void Emit(IMemberEmitter member, ILGenerator gen)
    {
      gen.Emit(OpCodes.Ldc_I4, this.size);
      gen.Emit(OpCodes.Newarr, this.arrayType);
    }
  }
}
