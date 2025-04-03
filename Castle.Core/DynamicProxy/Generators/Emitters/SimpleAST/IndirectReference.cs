// Decompiled with JetBrains decompiler
// Type: Castle.DynamicProxy.Generators.Emitters.SimpleAST.IndirectReference
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System;
using System.Reflection.Emit;

#nullable disable
namespace Castle.DynamicProxy.Generators.Emitters.SimpleAST
{
  public class IndirectReference : TypeReference
  {
    public IndirectReference(TypeReference byRefReference)
      : base((Reference) byRefReference, byRefReference.Type.GetElementType())
    {
      if (!byRefReference.Type.IsByRef)
        throw new ArgumentException("Expected an IsByRef reference", nameof (byRefReference));
    }

    public static TypeReference WrapIfByRef(TypeReference reference)
    {
      return !reference.Type.IsByRef ? reference : (TypeReference) new IndirectReference(reference);
    }

    public static TypeReference[] WrapIfByRef(TypeReference[] references)
    {
      TypeReference[] typeReferenceArray = new TypeReference[references.Length];
      for (int index = 0; index < references.Length; ++index)
        typeReferenceArray[index] = IndirectReference.WrapIfByRef(references[index]);
      return typeReferenceArray;
    }

    public override void LoadReference(ILGenerator gen)
    {
      OpCodeUtil.EmitLoadIndirectOpCodeForType(gen, this.Type);
    }

    public override void StoreReference(ILGenerator gen)
    {
      OpCodeUtil.EmitStoreIndirectOpCodeForType(gen, this.Type);
    }

    public override void LoadAddressOfReference(ILGenerator gen)
    {
    }
  }
}
