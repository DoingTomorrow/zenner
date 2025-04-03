// Decompiled with JetBrains decompiler
// Type: Castle.DynamicProxy.Generators.DelegateProxyGenerator
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
using System.Xml.Serialization;

#nullable disable
namespace Castle.DynamicProxy.Generators
{
  public class DelegateProxyGenerator : BaseProxyGenerator
  {
    public DelegateProxyGenerator(ModuleScope scope, Type delegateType)
      : base(scope, delegateType)
    {
      this.ProxyGenerationOptions = new ProxyGenerationOptions((IProxyGenerationHook) new DelegateProxyGenerationHook());
      this.ProxyGenerationOptions.Initialize();
    }

    public Type GetProxyType()
    {
      return this.ObtainProxyType(new CacheKey(this.targetType, (Type[]) null, (ProxyGenerationOptions) null), new Func<string, INamingScope, Type>(this.GenerateType));
    }

    private Type GenerateType(string name, INamingScope namingScope)
    {
      IEnumerable<ITypeContributor> contributors;
      IEnumerable<Type> implementerMapping = this.GetTypeImplementerMapping(out contributors, namingScope);
      MetaType model = new MetaType();
      foreach (ITypeContributor typeContributor in contributors)
        typeContributor.CollectElementsToProxy(this.ProxyGenerationOptions.Hook, model);
      this.ProxyGenerationOptions.Hook.MethodsInspected();
      ClassEmitter classEmitter = this.BuildClassEmitter(name, typeof (object), implementerMapping);
      this.CreateFields(classEmitter);
      this.CreateTypeAttributes(classEmitter);
      ConstructorEmitter staticConstructor = this.GenerateStaticConstructor(classEmitter);
      List<FieldReference> fieldReferenceList = new List<FieldReference>()
      {
        this.CreateTargetField(classEmitter)
      };
      foreach (ITypeContributor typeContributor in contributors)
        typeContributor.Generate(classEmitter, this.ProxyGenerationOptions);
      FieldReference field1 = classEmitter.GetField("__interceptors");
      fieldReferenceList.Add(field1);
      FieldReference field2 = classEmitter.GetField("__selector");
      if (field2 != null)
        fieldReferenceList.Add(field2);
      this.GenerateConstructor(classEmitter, (ConstructorInfo) null, fieldReferenceList.ToArray());
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
      ClassProxyInstanceContributor instanceContributor = new ClassProxyInstanceContributor(this.targetType, (IList<MethodInfo>) new List<MethodInfo>(), Type.EmptyTypes, ProxyTypeConstants.ClassWithTarget);
      DelegateProxyTargetContributor targetContributor1 = new DelegateProxyTargetContributor(this.targetType, namingScope);
      targetContributor1.Logger = this.Logger;
      DelegateProxyTargetContributor targetContributor2 = targetContributor1;
      IDictionary<Type, ITypeContributor> dictionary = (IDictionary<Type, ITypeContributor>) new Dictionary<Type, ITypeContributor>();
      if (this.targetType.IsSerializable)
        this.AddMappingForISerializable(dictionary, (ITypeContributor) instanceContributor);
      this.AddMappingNoCheck(typeof (IProxyTargetAccessor), (ITypeContributor) instanceContributor, dictionary);
      contributors = (IEnumerable<ITypeContributor>) new List<ITypeContributor>()
      {
        (ITypeContributor) targetContributor2,
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
