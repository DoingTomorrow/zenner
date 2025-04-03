// Decompiled with JetBrains decompiler
// Type: Castle.DynamicProxy.Generators.Emitters.ConstructorEmitter
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using Castle.DynamicProxy.Generators.Emitters.CodeBuilders;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using System;
using System.Reflection;
using System.Reflection.Emit;

#nullable disable
namespace Castle.DynamicProxy.Generators.Emitters
{
  public class ConstructorEmitter : IMemberEmitter
  {
    private readonly ConstructorBuilder builder;
    private readonly AbstractTypeEmitter maintype;
    private ConstructorCodeBuilder constructorCodeBuilder;

    protected internal ConstructorEmitter(AbstractTypeEmitter maintype, ConstructorBuilder builder)
    {
      this.maintype = maintype;
      this.builder = builder;
    }

    internal ConstructorEmitter(AbstractTypeEmitter maintype, params ArgumentReference[] arguments)
    {
      this.maintype = maintype;
      Type[] parameterTypes = ArgumentsUtil.InitializeAndConvert(arguments);
      this.builder = maintype.TypeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, parameterTypes);
    }

    public virtual ConstructorCodeBuilder CodeBuilder
    {
      get
      {
        if (this.constructorCodeBuilder == null)
          this.constructorCodeBuilder = new ConstructorCodeBuilder(this.maintype.BaseType, this.builder.GetILGenerator());
        return this.constructorCodeBuilder;
      }
    }

    public ConstructorBuilder ConstructorBuilder => this.builder;

    public MemberInfo Member => (MemberInfo) this.builder;

    public Type ReturnType => typeof (void);

    private bool ImplementedByRuntime
    {
      get
      {
        return (this.builder.GetMethodImplementationFlags() & MethodImplAttributes.CodeTypeMask) != MethodImplAttributes.IL;
      }
    }

    public virtual void EnsureValidCodeBlock()
    {
      if (this.ImplementedByRuntime || !this.CodeBuilder.IsEmpty)
        return;
      this.CodeBuilder.InvokeBaseConstructor();
      this.CodeBuilder.AddStatement((Statement) new ReturnStatement());
    }

    public virtual void Generate()
    {
      if (this.ImplementedByRuntime)
        return;
      this.CodeBuilder.Generate((IMemberEmitter) this, this.builder.GetILGenerator());
    }
  }
}
