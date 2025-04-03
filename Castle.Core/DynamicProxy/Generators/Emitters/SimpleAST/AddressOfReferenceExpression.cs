// Decompiled with JetBrains decompiler
// Type: Castle.DynamicProxy.Generators.Emitters.SimpleAST.AddressOfReferenceExpression
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System.Reflection.Emit;

#nullable disable
namespace Castle.DynamicProxy.Generators.Emitters.SimpleAST
{
  public class AddressOfReferenceExpression : Expression
  {
    private readonly Reference reference;

    public AddressOfReferenceExpression(Reference reference) => this.reference = reference;

    public override void Emit(IMemberEmitter member, ILGenerator gen)
    {
      ArgumentsUtil.EmitLoadOwnerAndReference(this.reference.OwnerReference, gen);
      this.reference.LoadAddressOfReference(gen);
    }
  }
}
