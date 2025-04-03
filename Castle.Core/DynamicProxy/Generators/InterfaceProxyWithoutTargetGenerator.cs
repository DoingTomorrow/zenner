// Decompiled with JetBrains decompiler
// Type: Castle.DynamicProxy.Generators.InterfaceProxyWithoutTargetGenerator
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using Castle.DynamicProxy.Contributors;
using Castle.DynamicProxy.Generators.Emitters;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using Castle.DynamicProxy.Serialization;
using System;
using System.Collections.Generic;

#nullable disable
namespace Castle.DynamicProxy.Generators
{
  public class InterfaceProxyWithoutTargetGenerator(ModuleScope scope, Type @interface) : 
    InterfaceProxyWithTargetGenerator(scope, @interface)
  {
    protected override ITypeContributor AddMappingForTargetType(
      IDictionary<Type, ITypeContributor> interfaceTypeImplementerMapping,
      Type proxyTargetType,
      ICollection<Type> targetInterfaces,
      ICollection<Type> additionalInterfaces,
      INamingScope namingScope)
    {
      InterfaceProxyWithoutTargetContributor targetContributor = new InterfaceProxyWithoutTargetContributor(namingScope, (GetTargetExpressionDelegate) ((c, m) => (Expression) NullExpression.Instance));
      targetContributor.Logger = this.Logger;
      InterfaceProxyWithoutTargetContributor implementer = targetContributor;
      foreach (Type allInterface in (IEnumerable<Type>) this.targetType.GetAllInterfaces())
      {
        implementer.AddInterfaceToProxy(allInterface);
        this.AddMappingNoCheck(allInterface, (ITypeContributor) implementer, interfaceTypeImplementerMapping);
      }
      return (ITypeContributor) implementer;
    }

    protected override Type GenerateType(
      string typeName,
      Type proxyTargetType,
      Type[] interfaces,
      INamingScope namingScope)
    {
      IEnumerable<ITypeContributor> contributors;
      IEnumerable<Type> implementerMapping = this.GetTypeImplementerMapping(interfaces, this.targetType, out contributors, namingScope);
      MetaType model = new MetaType();
      foreach (ITypeContributor typeContributor in contributors)
        typeContributor.CollectElementsToProxy(this.ProxyGenerationOptions.Hook, model);
      this.ProxyGenerationOptions.Hook.MethodsInspected();
      ClassEmitter emitter;
      FieldReference interceptorsField;
      Type baseType = this.Init(typeName, out emitter, proxyTargetType, out interceptorsField, implementerMapping);
      ConstructorEmitter staticConstructor = this.GenerateStaticConstructor(emitter);
      List<FieldReference> collection = new List<FieldReference>();
      foreach (ITypeContributor typeContributor in contributors)
      {
        typeContributor.Generate(emitter, this.ProxyGenerationOptions);
        if (typeContributor is MixinContributor)
          collection.AddRange((typeContributor as MixinContributor).Fields);
      }
      List<FieldReference> fieldReferenceList = new List<FieldReference>((IEnumerable<FieldReference>) collection)
      {
        interceptorsField,
        this.targetField
      };
      FieldReference field = emitter.GetField("__selector");
      if (field != null)
        fieldReferenceList.Add(field);
      this.GenerateConstructors(emitter, baseType, fieldReferenceList.ToArray());
      this.CompleteInitCacheMethod(staticConstructor.CodeBuilder);
      Type builtType = emitter.BuildType();
      this.InitializeStaticFields(builtType);
      return builtType;
    }

    protected override string GeneratorType => ProxyTypeConstants.InterfaceWithoutTarget;
  }
}
