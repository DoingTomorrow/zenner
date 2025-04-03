// Decompiled with JetBrains decompiler
// Type: Castle.DynamicProxy.Generators.Emitters.SimpleAST.Reference
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System.Reflection.Emit;

#nullable disable
namespace Castle.DynamicProxy.Generators.Emitters.SimpleAST
{
  public abstract class Reference
  {
    protected Reference owner = (Reference) SelfReference.Self;

    protected Reference()
    {
    }

    protected Reference(Reference owner) => this.owner = owner;

    public Reference OwnerReference
    {
      get => this.owner;
      set => this.owner = value;
    }

    public virtual Expression ToExpression() => (Expression) new ReferenceExpression(this);

    public virtual Expression ToAddressOfExpression()
    {
      return (Expression) new AddressOfReferenceExpression(this);
    }

    public virtual void Generate(ILGenerator gen)
    {
    }

    public abstract void LoadAddressOfReference(ILGenerator gen);

    public abstract void LoadReference(ILGenerator gen);

    public abstract void StoreReference(ILGenerator gen);
  }
}
