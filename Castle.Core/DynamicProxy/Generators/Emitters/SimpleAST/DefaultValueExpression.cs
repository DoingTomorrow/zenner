// Decompiled with JetBrains decompiler
// Type: Castle.DynamicProxy.Generators.Emitters.SimpleAST.DefaultValueExpression
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System;
using System.Reflection.Emit;

#nullable disable
namespace Castle.DynamicProxy.Generators.Emitters.SimpleAST
{
  public class DefaultValueExpression : Expression
  {
    private readonly Type type;

    public DefaultValueExpression(Type type) => this.type = type;

    public override void Emit(IMemberEmitter member, ILGenerator gen)
    {
      if (this.IsPrimitiveOrClass(this.type))
        OpCodeUtil.EmitLoadOpCodeForDefaultValueOfType(gen, this.type);
      else if (this.type.IsValueType || this.type.IsGenericParameter)
      {
        LocalBuilder local = gen.DeclareLocal(this.type);
        gen.Emit(OpCodes.Ldloca_S, local);
        gen.Emit(OpCodes.Initobj, this.type);
        gen.Emit(OpCodes.Ldloc, local);
      }
      else
      {
        if (!this.type.IsByRef)
          throw new ProxyGenerationException("Can't emit default value for type " + (object) this.type);
        this.EmitByRef(gen);
      }
    }

    private bool IsPrimitiveOrClass(Type type)
    {
      if (type.IsPrimitive && type != typeof (IntPtr))
        return true;
      return (type.IsClass || type.IsInterface) && !type.IsGenericParameter && !type.IsByRef;
    }

    private void EmitByRef(ILGenerator gen)
    {
      Type elementType = this.type.GetElementType();
      if (this.IsPrimitiveOrClass(elementType))
      {
        OpCodeUtil.EmitLoadOpCodeForDefaultValueOfType(gen, elementType);
        OpCodeUtil.EmitStoreIndirectOpCodeForType(gen, elementType);
      }
      else
      {
        if (!elementType.IsGenericParameter && !elementType.IsValueType)
          throw new ProxyGenerationException("Can't emit default value for reference of type " + (object) elementType);
        gen.Emit(OpCodes.Initobj, elementType);
      }
    }
  }
}
