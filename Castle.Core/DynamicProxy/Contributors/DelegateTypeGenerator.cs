// Decompiled with JetBrains decompiler
// Type: Castle.DynamicProxy.Contributors.DelegateTypeGenerator
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using Castle.DynamicProxy.Generators;
using Castle.DynamicProxy.Generators.Emitters;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using System;
using System.Collections.Generic;
using System.Reflection;

#nullable disable
namespace Castle.DynamicProxy.Contributors
{
  public class DelegateTypeGenerator : IGenerator<AbstractTypeEmitter>
  {
    private const TypeAttributes DelegateFlags = TypeAttributes.Public | TypeAttributes.Sealed | TypeAttributes.AutoClass;
    private readonly MetaMethod method;
    private readonly Type targetType;

    public DelegateTypeGenerator(MetaMethod method, Type targetType)
    {
      this.method = method;
      this.targetType = targetType;
    }

    public AbstractTypeEmitter Generate(
      ClassEmitter @class,
      ProxyGenerationOptions options,
      INamingScope namingScope)
    {
      AbstractTypeEmitter emitter = this.GetEmitter(@class, namingScope);
      this.BuildConstructor(emitter);
      this.BuildInvokeMethod(emitter);
      return emitter;
    }

    private void BuildInvokeMethod(AbstractTypeEmitter @delegate)
    {
      Type[] paramTypes = this.GetParamTypes(@delegate);
      @delegate.CreateMethod("Invoke", MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.HideBySig | MethodAttributes.VtableLayoutMask, @delegate.GetClosedParameterType(this.method.MethodOnTarget.ReturnType), paramTypes).MethodBuilder.SetImplementationFlags(MethodImplAttributes.CodeTypeMask);
    }

    private Type[] GetParamTypes(AbstractTypeEmitter @delegate)
    {
      ParameterInfo[] parameters = this.method.MethodOnTarget.GetParameters();
      if (@delegate.TypeBuilder.IsGenericType)
      {
        Type[] paramTypes = new Type[parameters.Length];
        for (int index = 0; index < parameters.Length; ++index)
          paramTypes[index] = @delegate.GetClosedParameterType(parameters[index].ParameterType);
        return paramTypes;
      }
      Type[] paramTypes1 = new Type[parameters.Length + 1];
      paramTypes1[0] = this.targetType;
      for (int index = 0; index < parameters.Length; ++index)
        paramTypes1[index + 1] = @delegate.GetClosedParameterType(parameters[index].ParameterType);
      return paramTypes1;
    }

    private void BuildConstructor(AbstractTypeEmitter emitter)
    {
      emitter.CreateConstructor(new ArgumentReference(typeof (object)), new ArgumentReference(typeof (IntPtr))).ConstructorBuilder.SetImplementationFlags(MethodImplAttributes.CodeTypeMask);
    }

    private AbstractTypeEmitter GetEmitter(ClassEmitter @class, INamingScope namingScope)
    {
      string suggestedName = string.Format("Castle.Proxies.Delegates.{0}_{1}", (object) this.method.MethodOnTarget.DeclaringType.Name, (object) this.method.Method.Name);
      string uniqueName = namingScope.ParentScope.GetUniqueName(suggestedName);
      ClassEmitter emitter = new ClassEmitter(@class.ModuleScope, uniqueName, typeof (MulticastDelegate), (IEnumerable<Type>) Type.EmptyTypes, TypeAttributes.Public | TypeAttributes.Sealed | TypeAttributes.AutoClass);
      emitter.CopyGenericParametersFromMethod(this.method.Method);
      return (AbstractTypeEmitter) emitter;
    }
  }
}
