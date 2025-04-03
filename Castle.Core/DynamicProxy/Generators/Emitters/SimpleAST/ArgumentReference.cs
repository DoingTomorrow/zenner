// Decompiled with JetBrains decompiler
// Type: Castle.DynamicProxy.Generators.Emitters.SimpleAST.ArgumentReference
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System;
using System.Reflection.Emit;

#nullable disable
namespace Castle.DynamicProxy.Generators.Emitters.SimpleAST
{
  public class ArgumentReference : TypeReference
  {
    public ArgumentReference(Type argumentType)
      : base(argumentType)
    {
      this.Position = -1;
    }

    public ArgumentReference(Type argumentType, int position)
      : base(argumentType)
    {
      this.Position = position;
    }

    internal int Position { get; set; }

    public override void LoadReference(ILGenerator gen)
    {
      if (this.Position == -1)
        throw new ProxyGenerationException("ArgumentReference unitialized");
      switch (this.Position)
      {
        case 0:
          gen.Emit(OpCodes.Ldarg_0);
          break;
        case 1:
          gen.Emit(OpCodes.Ldarg_1);
          break;
        case 2:
          gen.Emit(OpCodes.Ldarg_2);
          break;
        case 3:
          gen.Emit(OpCodes.Ldarg_3);
          break;
        default:
          gen.Emit(OpCodes.Ldarg_S, this.Position);
          break;
      }
    }

    public override void StoreReference(ILGenerator gen)
    {
      if (this.Position == -1)
        throw new ProxyGenerationException("ArgumentReference unitialized");
      gen.Emit(OpCodes.Starg, this.Position);
    }

    public override void LoadAddressOfReference(ILGenerator gen)
    {
      throw new NotSupportedException();
    }
  }
}
