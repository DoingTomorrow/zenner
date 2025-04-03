// Decompiled with JetBrains decompiler
// Type: Castle.DynamicProxy.Generators.InterfaceProxyWithTargetGenerator
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
  public class InterfaceProxyWithTargetGenerator : BaseProxyGenerator
  {
    protected FieldReference targetField;

    public InterfaceProxyWithTargetGenerator(ModuleScope scope, Type @interface)
      : base(scope, @interface)
    {
      this.CheckNotGenericTypeDefinition(@interface, "@interface");
    }

    public Type GenerateCode(
      Type proxyTargetType,
      Type[] interfaces,
      ProxyGenerationOptions options)
    {
      options.Initialize();
      this.CheckNotGenericTypeDefinition(proxyTargetType, nameof (proxyTargetType));
      this.CheckNotGenericTypeDefinitions((IEnumerable<Type>) interfaces, nameof (interfaces));
      this.EnsureValidBaseType(options.BaseTypeForInterfaceProxy);
      this.ProxyGenerationOptions = options;
      interfaces = TypeUtil.GetAllInterfaces(interfaces).ToArray<Type>();
      return this.ObtainProxyType(new CacheKey((MemberInfo) proxyTargetType, this.targetType, interfaces, options), (Func<string, INamingScope, Type>) ((n, s) => this.GenerateType(n, proxyTargetType, interfaces, s)));
    }

    private void EnsureValidBaseType(Type type)
    {
      if (type == null)
        throw new ArgumentException("Base type for proxy is null reference. Please set it to System.Object or some other valid type.");
      if (!type.IsClass)
        this.ThrowInvalidBaseType(type, "it is not a class type");
      if (type.IsSealed)
        this.ThrowInvalidBaseType(type, "it is sealed");
      ConstructorInfo constructor = type.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, Type.EmptyTypes, (ParameterModifier[]) null);
      if (constructor != null && !constructor.IsPrivate)
        return;
      this.ThrowInvalidBaseType(type, "it does not have accessible parameterless constructor");
    }

    private void ThrowInvalidBaseType(
      Type type,
      string doesNotHaveAccessibleParameterlessConstructor)
    {
      throw new ArgumentException(string.Format("Type {0} is not valid base type for interface proxy, because {1}. Only a non-sealed class with non-private default constructor can be used as base type for interface proxy. Please use some other valid type.", (object) type, (object) doesNotHaveAccessibleParameterlessConstructor));
    }

    protected virtual Type GenerateType(
      string typeName,
      Type proxyTargetType,
      Type[] interfaces,
      INamingScope namingScope)
    {
      IEnumerable<ITypeContributor> contributors;
      IEnumerable<Type> implementerMapping = this.GetTypeImplementerMapping(interfaces, proxyTargetType, out contributors, namingScope);
      ClassEmitter emitter;
      FieldReference interceptorsField;
      Type baseType = this.Init(typeName, out emitter, proxyTargetType, out interceptorsField, implementerMapping);
      MetaType model = new MetaType();
      foreach (ITypeContributor typeContributor in contributors)
        typeContributor.CollectElementsToProxy(this.ProxyGenerationOptions.Hook, model);
      this.ProxyGenerationOptions.Hook.MethodsInspected();
      ConstructorEmitter staticConstructor = this.GenerateStaticConstructor(emitter);
      List<FieldReference> fieldReferenceList = new List<FieldReference>();
      foreach (ITypeContributor typeContributor in contributors)
      {
        typeContributor.Generate(emitter, this.ProxyGenerationOptions);
        if (typeContributor is MixinContributor)
          fieldReferenceList.AddRange((typeContributor as MixinContributor).Fields);
      }
      fieldReferenceList.Add(interceptorsField);
      fieldReferenceList.Add(this.targetField);
      FieldReference field = emitter.GetField("__selector");
      if (field != null)
        fieldReferenceList.Add(field);
      this.GenerateConstructors(emitter, baseType, fieldReferenceList.ToArray());
      this.CompleteInitCacheMethod(staticConstructor.CodeBuilder);
      Type builtType = emitter.BuildType();
      this.InitializeStaticFields(builtType);
      return builtType;
    }

    protected virtual Type Init(
      string typeName,
      out ClassEmitter emitter,
      Type proxyTargetType,
      out FieldReference interceptorsField,
      IEnumerable<Type> interfaces)
    {
      Type forInterfaceProxy = this.ProxyGenerationOptions.BaseTypeForInterfaceProxy;
      emitter = this.BuildClassEmitter(typeName, forInterfaceProxy, interfaces);
      this.CreateFields(emitter, proxyTargetType);
      this.CreateTypeAttributes(emitter);
      interceptorsField = emitter.GetField("__interceptors");
      return forInterfaceProxy;
    }

    private void CreateFields(ClassEmitter emitter, Type proxyTargetType)
    {
      this.CreateFields(emitter);
      this.targetField = emitter.CreateField("__target", proxyTargetType);
      emitter.DefineCustomAttributeFor<XmlIgnoreAttribute>(this.targetField);
    }

    protected override void CreateTypeAttributes(ClassEmitter emitter)
    {
      base.CreateTypeAttributes(emitter);
      emitter.DefineCustomAttribute<SerializableAttribute>();
    }

    protected virtual string GeneratorType => ProxyTypeConstants.InterfaceWithTarget;

    protected virtual bool AllowChangeTarget => false;

    protected virtual IEnumerable<Type> GetTypeImplementerMapping(
      Type[] interfaces,
      Type proxyTargetType,
      out IEnumerable<ITypeContributor> contributors,
      INamingScope namingScope)
    {
      IDictionary<Type, ITypeContributor> dictionary = (IDictionary<Type, ITypeContributor>) new Dictionary<Type, ITypeContributor>();
      MixinContributor mixinContributor1 = new MixinContributor(namingScope, this.AllowChangeTarget);
      mixinContributor1.Logger = this.Logger;
      MixinContributor mixinContributor2 = mixinContributor1;
      ICollection<Type> allInterfaces1 = proxyTargetType.GetAllInterfaces();
      ICollection<Type> allInterfaces2 = TypeUtil.GetAllInterfaces(interfaces);
      ITypeContributor implementer = this.AddMappingForTargetType(dictionary, proxyTargetType, allInterfaces1, allInterfaces2, namingScope);
      if (this.ProxyGenerationOptions.HasMixins)
      {
        foreach (Type mixinInterface in this.ProxyGenerationOptions.MixinData.MixinInterfaces)
        {
          if (allInterfaces1.Contains(mixinInterface))
          {
            if (allInterfaces2.Contains(mixinInterface))
              this.AddMapping(mixinInterface, implementer, dictionary);
            mixinContributor2.AddEmptyInterface(mixinInterface);
          }
          else if (!dictionary.ContainsKey(mixinInterface))
          {
            mixinContributor2.AddInterfaceToProxy(mixinInterface);
            dictionary.Add(mixinInterface, (ITypeContributor) mixinContributor2);
          }
        }
      }
      InterfaceProxyWithoutTargetContributor additionalInterfaces = this.GetContributorForAdditionalInterfaces(namingScope);
      foreach (Type type in (IEnumerable<Type>) allInterfaces2)
      {
        if (!dictionary.ContainsKey(type) && !this.ProxyGenerationOptions.MixinData.ContainsMixin(type))
        {
          additionalInterfaces.AddInterfaceToProxy(type);
          this.AddMappingNoCheck(type, (ITypeContributor) additionalInterfaces, dictionary);
        }
      }
      InterfaceProxyInstanceContributor instanceContributor = new InterfaceProxyInstanceContributor(this.targetType, this.GeneratorType, interfaces);
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
        implementer,
        (ITypeContributor) additionalInterfaces,
        (ITypeContributor) mixinContributor2,
        (ITypeContributor) instanceContributor
      };
      return (IEnumerable<Type>) dictionary.Keys;
    }

    protected virtual InterfaceProxyWithoutTargetContributor GetContributorForAdditionalInterfaces(
      INamingScope namingScope)
    {
      InterfaceProxyWithoutTargetContributor additionalInterfaces = new InterfaceProxyWithoutTargetContributor(namingScope, (GetTargetExpressionDelegate) ((c, m) => (Expression) NullExpression.Instance));
      additionalInterfaces.Logger = this.Logger;
      return additionalInterfaces;
    }

    protected virtual ITypeContributor AddMappingForTargetType(
      IDictionary<Type, ITypeContributor> typeImplementerMapping,
      Type proxyTargetType,
      ICollection<Type> targetInterfaces,
      ICollection<Type> additionalInterfaces,
      INamingScope namingScope)
    {
      InterfaceProxyTargetContributor targetContributor = new InterfaceProxyTargetContributor(proxyTargetType, this.AllowChangeTarget, namingScope);
      targetContributor.Logger = this.Logger;
      InterfaceProxyTargetContributor implementer = targetContributor;
      ICollection<Type> allInterfaces = this.targetType.GetAllInterfaces();
      foreach (Type @interface in (IEnumerable<Type>) allInterfaces)
      {
        implementer.AddInterfaceToProxy(@interface);
        this.AddMappingNoCheck(@interface, (ITypeContributor) implementer, typeImplementerMapping);
      }
      foreach (Type additionalInterface in (IEnumerable<Type>) additionalInterfaces)
      {
        if (this.ImplementedByTarget(targetInterfaces, additionalInterface) && !allInterfaces.Contains(additionalInterface))
        {
          implementer.AddInterfaceToProxy(additionalInterface);
          this.AddMappingNoCheck(additionalInterface, (ITypeContributor) implementer, typeImplementerMapping);
        }
      }
      return (ITypeContributor) implementer;
    }

    private bool ImplementedByTarget(ICollection<Type> targetInterfaces, Type @interface)
    {
      return targetInterfaces.Contains(@interface);
    }
  }
}
