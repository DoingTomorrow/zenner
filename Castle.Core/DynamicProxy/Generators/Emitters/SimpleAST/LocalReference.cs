// Decompiled with JetBrains decompiler
// Type: Castle.DynamicProxy.Generators.Emitters.SimpleAST.LocalReference
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System;
using System.Reflection.Emit;

#nullable disable
namespace Castle.DynamicProxy.Generators.Emitters.SimpleAST
{
  public class LocalReference(Type type) : TypeReference(type)
  {
    private LocalBuilder localbuilder;

    public override void Generate(ILGenerator gen)
    {
      this.localbuilder = gen.DeclareLocal(this.Type);
    }

    public override void LoadReference(ILGenerator gen)
    {
      gen.Emit(OpCodes.Ldloc, this.localbuilder);
    }

    public override void StoreReference(ILGenerator gen)
    {
      gen.Emit(OpCodes.Stloc, this.localbuilder);
    }

    public override void LoadAddressOfReference(ILGenerator gen)
    {
      gen.Emit(OpCodes.Ldloca, this.localbuilder);
    }
  }
}
