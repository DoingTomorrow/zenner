// Decompiled with JetBrains decompiler
// Type: Castle.DynamicProxy.Generators.Emitters.SimpleAST.SelfReference
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System;
using System.Reflection.Emit;

#nullable disable
namespace Castle.DynamicProxy.Generators.Emitters.SimpleAST
{
  public class SelfReference : Reference
  {
    public static readonly SelfReference Self = new SelfReference();

    protected SelfReference()
      : base((Reference) null)
    {
    }

    public override void LoadReference(ILGenerator gen) => gen.Emit(OpCodes.Ldarg_0);

    public override void StoreReference(ILGenerator gen) => gen.Emit(OpCodes.Ldarg_0);

    public override void LoadAddressOfReference(ILGenerator gen)
    {
      throw new NotSupportedException();
    }
  }
}
