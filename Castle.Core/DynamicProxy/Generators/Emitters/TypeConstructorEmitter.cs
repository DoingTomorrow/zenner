// Decompiled with JetBrains decompiler
// Type: Castle.DynamicProxy.Generators.Emitters.TypeConstructorEmitter
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using Castle.DynamicProxy.Generators.Emitters.SimpleAST;

#nullable disable
namespace Castle.DynamicProxy.Generators.Emitters
{
  public class TypeConstructorEmitter : ConstructorEmitter
  {
    internal TypeConstructorEmitter(AbstractTypeEmitter maintype)
      : base(maintype, maintype.TypeBuilder.DefineTypeInitializer())
    {
    }

    public override void EnsureValidCodeBlock()
    {
      if (!this.CodeBuilder.IsEmpty)
        return;
      this.CodeBuilder.AddStatement((Statement) new ReturnStatement());
    }
  }
}
