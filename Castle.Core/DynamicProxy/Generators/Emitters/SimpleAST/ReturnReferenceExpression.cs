// Decompiled with JetBrains decompiler
// Type: Castle.DynamicProxy.Generators.Emitters.SimpleAST.ReturnReferenceExpression
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System;
using System.Reflection.Emit;

#nullable disable
namespace Castle.DynamicProxy.Generators.Emitters.SimpleAST
{
  public class ReturnReferenceExpression(Type argumentType) : TypeReference(argumentType)
  {
    public override void LoadReference(ILGenerator gen)
    {
    }

    public override void StoreReference(ILGenerator gen)
    {
    }

    public override void LoadAddressOfReference(ILGenerator gen)
    {
      throw new NotSupportedException();
    }
  }
}
