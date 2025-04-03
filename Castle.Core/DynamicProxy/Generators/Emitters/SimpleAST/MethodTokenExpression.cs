// Decompiled with JetBrains decompiler
// Type: Castle.DynamicProxy.Generators.Emitters.SimpleAST.MethodTokenExpression
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using Castle.DynamicProxy.Tokens;
using System;
using System.Reflection;
using System.Reflection.Emit;

#nullable disable
namespace Castle.DynamicProxy.Generators.Emitters.SimpleAST
{
  public class MethodTokenExpression : Expression
  {
    private readonly MethodInfo method;
    private readonly Type declaringType;

    public MethodTokenExpression(MethodInfo method)
    {
      this.method = method;
      this.declaringType = method.DeclaringType;
    }

    public override void Emit(IMemberEmitter member, ILGenerator gen)
    {
      gen.Emit(OpCodes.Ldtoken, this.method);
      if (this.declaringType == null)
        throw new GeneratorException("declaringType can't be null for this situation");
      gen.Emit(OpCodes.Ldtoken, this.declaringType);
      MethodInfo methodFromHandle1 = MethodBaseMethods.GetMethodFromHandle1;
      MethodInfo methodFromHandle2 = MethodBaseMethods.GetMethodFromHandle2;
      gen.Emit(OpCodes.Call, methodFromHandle2);
      gen.Emit(OpCodes.Castclass, typeof (MethodInfo));
    }
  }
}
