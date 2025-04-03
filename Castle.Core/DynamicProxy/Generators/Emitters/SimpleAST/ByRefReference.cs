// Decompiled with JetBrains decompiler
// Type: Castle.DynamicProxy.Generators.Emitters.SimpleAST.ByRefReference
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System;
using System.Reflection.Emit;

#nullable disable
namespace Castle.DynamicProxy.Generators.Emitters.SimpleAST
{
  public class ByRefReference : TypeReference
  {
    private readonly LocalReference localReference;

    public ByRefReference(LocalReference localReference)
      : base(localReference.Type)
    {
      this.localReference = localReference;
    }

    public override void LoadAddressOfReference(ILGenerator gen)
    {
      this.localReference.LoadAddressOfReference(gen);
    }

    public override void LoadReference(ILGenerator gen)
    {
      this.localReference.LoadAddressOfReference(gen);
    }

    public override void StoreReference(ILGenerator gen) => throw new NotImplementedException();
  }
}
