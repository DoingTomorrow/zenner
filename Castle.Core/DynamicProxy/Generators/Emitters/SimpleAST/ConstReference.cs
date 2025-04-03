// Decompiled with JetBrains decompiler
// Type: Castle.DynamicProxy.Generators.Emitters.SimpleAST.ConstReference
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System;
using System.Reflection.Emit;

#nullable disable
namespace Castle.DynamicProxy.Generators.Emitters.SimpleAST
{
  public class ConstReference : TypeReference
  {
    private readonly object value;

    public ConstReference(object value)
      : base(value.GetType())
    {
      this.value = value.GetType().IsPrimitive || value is string ? value : throw new ProxyGenerationException("Invalid type to ConstReference");
    }

    public override void Generate(ILGenerator gen)
    {
    }

    public override void LoadReference(ILGenerator gen)
    {
      OpCodeUtil.EmitLoadOpCodeForConstantValue(gen, this.value);
    }

    public override void StoreReference(ILGenerator gen)
    {
      throw new NotImplementedException("ConstReference.StoreReference");
    }

    public override void LoadAddressOfReference(ILGenerator gen)
    {
      throw new NotSupportedException();
    }
  }
}
