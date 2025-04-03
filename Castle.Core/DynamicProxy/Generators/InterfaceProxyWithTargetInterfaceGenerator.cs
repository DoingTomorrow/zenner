// Decompiled with JetBrains decompiler
// Type: Castle.DynamicProxy.Generators.InterfaceProxyWithTargetInterfaceGenerator
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using Castle.DynamicProxy.Contributors;
using Castle.DynamicProxy.Generators.Emitters;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using Castle.DynamicProxy.Serialization;
using System;
using System.Collections.Generic;
using System.Reflection;

#nullable disable
namespace Castle.DynamicProxy.Generators
{
  public class InterfaceProxyWithTargetInterfaceGenerator(ModuleScope scope, Type @interface) : 
    InterfaceProxyWithTargetGenerator(scope, @interface)
  {
    protected override ITypeContributor AddMappingForTargetType(
      IDictionary<Type, ITypeContributor> typeImplementerMapping,
      Type proxyTargetType,
      ICollection<Type> targetInterfaces,
      ICollection<Type> additionalInterfaces,
      INamingScope namingScope)
    {
      InterfaceProxyWithTargetInterfaceTargetContributor targetContributor = new InterfaceProxyWithTargetInterfaceTargetContributor(proxyTargetType, this.AllowChangeTarget, namingScope);
      targetContributor.Logger = this.Logger;
      InterfaceProxyWithTargetInterfaceTargetContributor implementer = targetContributor;
      foreach (Type allInterface in (IEnumerable<Type>) this.targetType.GetAllInterfaces())
      {
        implementer.AddInterfaceToProxy(allInterface);
        this.AddMappingNoCheck(allInterface, (ITypeContributor) implementer, typeImplementerMapping);
      }
      return (ITypeContributor) implementer;
    }

    protected override InterfaceProxyWithoutTargetContributor GetContributorForAdditionalInterfaces(
      INamingScope namingScope)
    {
      InterfaceProxyWithOptionalTargetContributor additionalInterfaces = new InterfaceProxyWithOptionalTargetContributor(namingScope, new GetTargetExpressionDelegate(this.GetTargetExpression), new GetTargetReferenceDelegate(this.GetTarget));
      additionalInterfaces.Logger = this.Logger;
      return (InterfaceProxyWithoutTargetContributor) additionalInterfaces;
    }

    private Reference GetTarget(ClassEmitter @class, MethodInfo method)
    {
      return (Reference) new AsTypeReference((Reference) @class.GetField("__target"), method.DeclaringType);
    }

    private Expression GetTargetExpression(ClassEmitter @class, MethodInfo method)
    {
      return this.GetTarget(@class, method).ToExpression();
    }

    protected override bool AllowChangeTarget => true;

    protected override string GeneratorType => ProxyTypeConstants.InterfaceWithTargetInterface;
  }
}
