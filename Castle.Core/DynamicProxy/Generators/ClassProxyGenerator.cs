// Decompiled with JetBrains decompiler
// Type: Castle.DynamicProxy.Generators.ClassProxyGenerator
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using Castle.DynamicProxy.Contributors;
using Castle.DynamicProxy.Generators.Emitters;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using Castle.DynamicProxy.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

#nullable disable
namespace Castle.DynamicProxy.Generators
{
  public class ClassProxyGenerator : BaseProxyGenerator
  {
    public ClassProxyGenerator(ModuleScope scope, Type targetType)
      : base(scope, targetType)
    {
      this.CheckNotGenericTypeDefinition(targetType, nameof (targetType));
      this.EnsureDoesNotImplementIProxyTargetAccessor(targetType, nameof (targetType));
    }

    private void EnsureDoesNotImplementIProxyTargetAccessor(Type type, string name)
    {
      if (typeof (IProxyTargetAccessor).IsAssignableFrom(type))
        throw new ArgumentException(string.Format("Target type for the proxy implements {0} which is a DynamicProxy infrastructure interface and you should never implement it yourself. Are you trying to proxy an existing proxy?", (object) typeof (IProxyTargetAccessor)), name);
    }

    public Type GenerateCode(Type[] interfaces, ProxyGenerationOptions options)
    {
      options.Initialize();
      interfaces = TypeUtil.GetAllInterfaces(interfaces).ToArray<Type>();
      this.CheckNotGenericTypeDefinitions((IEnumerable<Type>) interfaces, nameof (interfaces));
      this.ProxyGenerationOptions = options;
      return this.ObtainProxyType(new CacheKey(this.targetType, interfaces, options), (Func<string, INamingScope, Type>) ((n, s) => this.GenerateType(n, interfaces, s)));
    }

    protected virtual Type GenerateType(string name, Type[] interfaces, INamingScope namingScope)
    {
      IEnumerable<ITypeContributor> contributors;
      IEnumerable<Type> implementerMapping = this.GetTypeImplementerMapping(interfaces, out contributors, namingScope);
      MetaType model = new MetaType();
      foreach (ITypeContributor typeContributor in contributors)
        typeContributor.CollectElementsToProxy(this.ProxyGenerationOptions.Hook, model);
      this.ProxyGenerationOptions.Hook.MethodsInspected();
      ClassEmitter classEmitter = this.BuildClassEmitter(name, this.targetType, implementerMapping);
      this.CreateFields(classEmitter);
      this.CreateTypeAttributes(classEmitter);
      ConstructorEmitter staticConstructor = this.GenerateStaticConstructor(classEmitter);
      List<FieldReference> fieldReferenceList = new List<FieldReference>();
      foreach (ITypeContributor typeContributor in contributors)
      {
        typeContributor.Generate(classEmitter, this.ProxyGenerationOptions);
        if (typeContributor is MixinContributor)
          fieldReferenceList.AddRange((typeContributor as MixinContributor).Fields);
      }
      FieldReference field1 = classEmitter.GetField("__interceptors");
      fieldReferenceList.Add(field1);
      FieldReference field2 = classEmitter.GetField("__selector");
      if (field2 != null)
        fieldReferenceList.Add(field2);
      this.GenerateConstructors(classEmitter, this.targetType, fieldReferenceList.ToArray());
      this.GenerateParameterlessConstructor(classEmitter, this.targetType, field1);
      this.CompleteInitCacheMethod(staticConstructor.CodeBuilder);
      Type builtType = classEmitter.BuildType();
      this.InitializeStaticFields(builtType);
      return builtType;
    }

    protected virtual IEnumerable<Type> GetTypeImplementerMapping(
      Type[] interfaces,
      out IEnumerable<ITypeContributor> contributors,
      INamingScope namingScope)
    {
      List<MethodInfo> methodsToSkip = new List<MethodInfo>();
      ClassProxyInstanceContributor instanceContributor = new ClassProxyInstanceContributor(this.targetType, (IList<MethodInfo>) methodsToSkip, interfaces, ProxyTypeConstants.Class);
      ClassProxyTargetContributor targetContributor1 = new ClassProxyTargetContributor(this.targetType, (IList<MethodInfo>) methodsToSkip, namingScope);
      targetContributor1.Logger = this.Logger;
      ClassProxyTargetContributor implementer1 = targetContributor1;
      IDictionary<Type, ITypeContributor> dictionary = (IDictionary<Type, ITypeContributor>) new Dictionary<Type, ITypeContributor>();
      ICollection<Type> allInterfaces1 = this.targetType.GetAllInterfaces();
      ICollection<Type> allInterfaces2 = TypeUtil.GetAllInterfaces(interfaces);
      MixinContributor mixinContributor = new MixinContributor(namingScope, false);
      mixinContributor.Logger = this.Logger;
      MixinContributor implementer2 = mixinContributor;
      if (this.ProxyGenerationOptions.HasMixins)
      {
        foreach (Type mixinInterface in this.ProxyGenerationOptions.MixinData.MixinInterfaces)
        {
          if (allInterfaces1.Contains(mixinInterface))
          {
            if (allInterfaces2.Contains(mixinInterface) && !dictionary.ContainsKey(mixinInterface))
            {
              this.AddMappingNoCheck(mixinInterface, (ITypeContributor) implementer1, dictionary);
              implementer1.AddInterfaceToProxy(mixinInterface);
            }
            implementer2.AddEmptyInterface(mixinInterface);
          }
          else if (!dictionary.ContainsKey(mixinInterface))
          {
            implementer2.AddInterfaceToProxy(mixinInterface);
            this.AddMappingNoCheck(mixinInterface, (ITypeContributor) implementer2, dictionary);
          }
        }
      }
      InterfaceProxyWithoutTargetContributor targetContributor2 = new InterfaceProxyWithoutTargetContributor(namingScope, (GetTargetExpressionDelegate) ((c, m) => (Expression) NullExpression.Instance));
      targetContributor2.Logger = this.Logger;
      InterfaceProxyWithoutTargetContributor implementer3 = targetContributor2;
      foreach (Type type in (IEnumerable<Type>) allInterfaces2)
      {
        if (allInterfaces1.Contains(type))
        {
          if (!dictionary.ContainsKey(type))
          {
            this.AddMappingNoCheck(type, (ITypeContributor) implementer1, dictionary);
            implementer1.AddInterfaceToProxy(type);
          }
        }
        else if (!this.ProxyGenerationOptions.MixinData.ContainsMixin(type))
        {
          implementer3.AddInterfaceToProxy(type);
          this.AddMapping(type, (ITypeContributor) implementer3, dictionary);
        }
      }
      if (this.targetType.IsSerializable)
        this.AddMappingForISerializable(dictionary, (ITypeContributor) instanceContributor);
      try
      {
        this.AddMappingNoCheck(typeof (IProxyTargetAccessor), (ITypeContributor) instanceContributor, dictionary);
      }
      catch (ArgumentException ex)
      {
        this.HandleExplicitlyPassedProxyTargetAccessor(allInterfaces1, allInterfaces2);
      }
      contributors = (IEnumerable<ITypeContributor>) new List<ITypeContributor>()
      {
        (ITypeContributor) implementer1,
        (ITypeContributor) implementer2,
        (ITypeContributor) implementer3,
        (ITypeContributor) instanceContributor
      };
      return (IEnumerable<Type>) dictionary.Keys;
    }
  }
}
