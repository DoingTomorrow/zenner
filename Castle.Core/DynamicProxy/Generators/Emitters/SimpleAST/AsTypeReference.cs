// Decompiled with JetBrains decompiler
// Type: Castle.DynamicProxy.Generators.Emitters.SimpleAST.AsTypeReference
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System;
using System.Reflection.Emit;

#nullable disable
namespace Castle.DynamicProxy.Generators.Emitters.SimpleAST
{
  public class AsTypeReference : Reference
  {
    private readonly Reference reference;
    private readonly Type type;

    public AsTypeReference(Reference reference, Type type)
    {
      if (reference == null)
        throw new ArgumentNullException(nameof (reference));
      if (type == null)
        throw new ArgumentNullException(nameof (type));
      this.reference = reference;
      this.type = type;
      if (reference != this.OwnerReference)
        return;
      this.OwnerReference = (Reference) null;
    }

    public override void LoadAddressOfReference(ILGenerator gen)
    {
      this.reference.LoadAddressOfReference(gen);
    }

    public override void LoadReference(ILGenerator gen)
    {
      this.reference.LoadReference(gen);
      gen.Emit(OpCodes.Isinst, this.type);
    }

    public override void StoreReference(ILGenerator gen) => this.reference.StoreReference(gen);
  }
}
