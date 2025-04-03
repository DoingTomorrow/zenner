// Decompiled with JetBrains decompiler
// Type: Castle.DynamicProxy.Generators.Emitters.MethodEmitter
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using Castle.DynamicProxy.Generators.Emitters.CodeBuilders;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

#nullable disable
namespace Castle.DynamicProxy.Generators.Emitters
{
  [DebuggerDisplay("{builder.Name}")]
  public class MethodEmitter : IMemberEmitter
  {
    private readonly MethodBuilder builder;
    private readonly GenericTypeParameterBuilder[] genericTypeParams;
    private ArgumentReference[] arguments;
    private MethodCodeBuilder codebuilder;

    protected internal MethodEmitter(MethodBuilder builder) => this.builder = builder;

    internal MethodEmitter(AbstractTypeEmitter owner, string name, MethodAttributes attributes)
      : this(owner.TypeBuilder.DefineMethod(name, attributes))
    {
    }

    internal MethodEmitter(
      AbstractTypeEmitter owner,
      string name,
      MethodAttributes attributes,
      Type returnType,
      params Type[] argumentTypes)
      : this(owner, name, attributes)
    {
      this.SetParameters(argumentTypes);
      this.SetReturnType(returnType);
    }

    internal MethodEmitter(
      AbstractTypeEmitter owner,
      string name,
      MethodAttributes attributes,
      MethodInfo methodToUseAsATemplate)
      : this(owner, name, attributes)
    {
      Dictionary<string, GenericTypeParameterBuilder> genericArgumentsMap = GenericUtil.GetGenericArgumentsMap(owner);
      Type correctType = GenericUtil.ExtractCorrectType(methodToUseAsATemplate.ReturnType, genericArgumentsMap);
      ParameterInfo[] parameters = methodToUseAsATemplate.GetParameters();
      Type[] parametersTypes = GenericUtil.ExtractParametersTypes(parameters, genericArgumentsMap);
      this.genericTypeParams = GenericUtil.CopyGenericArguments(methodToUseAsATemplate, this.builder, genericArgumentsMap);
      this.SetParameters(parametersTypes);
      this.SetReturnType(correctType);
      this.SetSignature(correctType, methodToUseAsATemplate.ReturnParameter, parametersTypes, parameters);
      this.DefineParameters(parameters);
    }

    public GenericTypeParameterBuilder[] GenericTypeParams => this.genericTypeParams;

    public virtual MethodCodeBuilder CodeBuilder
    {
      get
      {
        if (this.codebuilder == null)
          this.codebuilder = new MethodCodeBuilder(this.builder.GetILGenerator());
        return this.codebuilder;
      }
    }

    public ArgumentReference[] Arguments => this.arguments;

    public MethodBuilder MethodBuilder => this.builder;

    private bool ImplementedByRuntime
    {
      get
      {
        return (this.builder.GetMethodImplementationFlags() & MethodImplAttributes.CodeTypeMask) != MethodImplAttributes.IL;
      }
    }

    public Type ReturnType => this.builder.ReturnType;

    public MemberInfo Member => (MemberInfo) this.builder;

    public virtual void EnsureValidCodeBlock()
    {
      if (this.ImplementedByRuntime || !this.CodeBuilder.IsEmpty)
        return;
      this.CodeBuilder.AddStatement((Statement) new NopStatement());
      this.CodeBuilder.AddStatement((Statement) new ReturnStatement());
    }

    public virtual void Generate()
    {
      if (this.ImplementedByRuntime)
        return;
      this.codebuilder.Generate((IMemberEmitter) this, this.builder.GetILGenerator());
    }

    private void SetReturnType(Type returnType) => this.builder.SetReturnType(returnType);

    private void SetSignature(
      Type returnType,
      ParameterInfo returnParameter,
      Type[] parameters,
      ParameterInfo[] baseMethodParameters)
    {
      this.builder.SetSignature(returnType, returnParameter.GetRequiredCustomModifiers(), returnParameter.GetOptionalCustomModifiers(), parameters, ((IEnumerable<ParameterInfo>) baseMethodParameters).Select<ParameterInfo, Type[]>((Func<ParameterInfo, Type[]>) (x => x.GetRequiredCustomModifiers())).ToArray<Type[]>(), ((IEnumerable<ParameterInfo>) baseMethodParameters).Select<ParameterInfo, Type[]>((Func<ParameterInfo, Type[]>) (x => x.GetOptionalCustomModifiers())).ToArray<Type[]>());
    }

    public void DefineCustomAttribute(CustomAttributeBuilder attribute)
    {
      this.builder.SetCustomAttribute(attribute);
    }

    public void SetParameters(Type[] paramTypes)
    {
      this.builder.SetParameters(paramTypes);
      this.arguments = ArgumentsUtil.ConvertToArgumentReference(paramTypes);
      ArgumentsUtil.InitializeArgumentsByPosition(this.arguments, this.MethodBuilder.IsStatic);
    }

    private void DefineParameters(ParameterInfo[] parameters)
    {
      foreach (ParameterInfo parameter in parameters)
      {
        ParameterBuilder parameterBuilder = this.builder.DefineParameter(parameter.Position + 1, parameter.Attributes, parameter.Name);
        foreach (CustomAttributeBuilder inheritableAttribute in parameter.GetNonInheritableAttributes())
          parameterBuilder.SetCustomAttribute(inheritableAttribute);
      }
    }
  }
}
