// Decompiled with JetBrains decompiler
// Type: Castle.DynamicProxy.Generators.ClassProxyWithTargetGenerator
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
using System.Xml.Serialization;

#nullable disable
namespace Castle.DynamicProxy.Generators
{
  public class ClassProxyWithTargetGenerator : BaseProxyGenerator
  {
    private readonly Type[] additionalInterfacesToProxy;

    public ClassProxyWithTargetGenerator(
      ModuleScope scope,
      Type classToProxy,
      Type[] additionalInterfacesToProxy,
      ProxyGenerationOptions options)
      : base(scope, classToProxy)
    {
      this.CheckNotGenericTypeDefinition(this.targetType, "targetType");
      this.EnsureDoesNotImplementIProxyTargetAccessor(this.targetType, "targetType");
      this.CheckNotGenericTypeDefinitions((IEnumerable<Type>) additionalInterfacesToProxy, nameof (additionalInterfacesToProxy));
      options.Initialize();
      this.ProxyGenerationOptions = options;
      this.additionalInterfacesToProxy = TypeUtil.GetAllInterfaces(additionalInterfacesToProxy).ToArray<Type>();
    }

    private void EnsureDoesNotImplementIProxyTargetAccessor(Type type, string name)
    {
      if (typeof (IProxyTargetAccessor).IsAssignableFrom(type))
        throw new ArgumentException(string.Format("Target type for the proxy implements {0} which is a DynamicProxy infrastructure interface and you should never implement it yourself. Are you trying to proxy an existing proxy?", (object) typeof (IProxyTargetAccessor)), name);
    }

    public Type GetGeneratedType()
    {
      return this.ObtainProxyType(new CacheKey(this.targetType, this.additionalInterfacesToProxy, this.ProxyGenerationOptions), new Func<string, INamingScope, Type>(this.GenerateType));
    }

    private Type GenerateType(string name, INamingScope namingScope)
    {
      IEnumerable<ITypeContributor> contributors;
      IEnumerable<Type> implementerMapping = this.GetTypeImplementerMapping(out contributors, namingScope);
      MetaType model = new MetaType();
      foreach (ITypeContributor typeContributor in contributors)
        typeContributor.CollectElementsToProxy(this.ProxyGenerationOptions.Hook, model);
      this.ProxyGenerationOptions.Hook.MethodsInspected();
      ClassEmitter classEmitter = this.BuildClassEmitter(name, this.targetType, implementerMapping);
      this.CreateFields(classEmitter);
      this.CreateTypeAttributes(classEmitter);
      ConstructorEmitter staticConstructor = this.GenerateStaticConstructor(classEmitter);
      List<FieldReference> fieldReferenceList = new List<FieldReference>()
      {
        this.CreateTargetField(classEmitter)
      };
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
      out IEnumerable<ITypeContributor> contributors,
      INamingScope namingScope)
    {
      List<MethodInfo> methodsToSkip = new List<MethodInfo>();
      ClassProxyInstanceContributor instanceContributor = new ClassProxyInstanceContributor(this.targetType, (IList<MethodInfo>) methodsToSkip, this.additionalInterfacesToProxy, ProxyTypeConstants.ClassWithTarget);
      ClassProxyWithTargetTargetContributor targetContributor1 = new ClassProxyWithTargetTargetContributor(this.targetType, (IList<MethodInfo>) methodsToSkip, namingScope);
      targetContributor1.Logger = this.Logger;
      ClassProxyWithTargetTargetContributor implementer1 = targetContributor1;
      IDictionary<Type, ITypeContributor> dictionary = (IDictionary<Type, ITypeContributor>) new Dictionary<Type, ITypeContributor>();
      ICollection<Type> allInterfaces = this.targetType.GetAllInterfaces();
      MixinContributor mixinContributor = new MixinContributor(namingScope, false);
      mixinContributor.Logger = this.Logger;
      MixinContributor implementer2 = mixinContributor;
      if (this.ProxyGenerationOptions.HasMixins)
      {
        foreach (Type mixinInterface in this.ProxyGenerationOptions.MixinData.MixinInterfaces)
        {
          if (allInterfaces.Contains(mixinInterface))
          {
            if (((IEnumerable<Type>) this.additionalInterfacesToProxy).Contains<Type>(mixinInterface) && !dictionary.ContainsKey(mixinInterface))
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
      foreach (Type type in this.additionalInterfacesToProxy)
      {
        if (allInterfaces.Contains(type))
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
        this.HandleExplicitlyPassedProxyTargetAccessor(allInterfaces, (ICollection<Type>) this.additionalInterfacesToProxy);
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

    private FieldReference CreateTargetField(ClassEmitter emitter)
    {
      FieldReference field = emitter.CreateField("__target", this.targetType);
      emitter.DefineCustomAttributeFor<XmlIgnoreAttribute>(field);
      return field;
    }
  }
}
