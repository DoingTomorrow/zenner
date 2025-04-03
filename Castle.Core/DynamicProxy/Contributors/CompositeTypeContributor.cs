// Decompiled with JetBrains decompiler
// Type: Castle.DynamicProxy.Contributors.CompositeTypeContributor
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using Castle.Core.Logging;
using Castle.DynamicProxy.Generators;
using Castle.DynamicProxy.Generators.Emitters;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;

#nullable disable
namespace Castle.DynamicProxy.Contributors
{
  public abstract class CompositeTypeContributor : ITypeContributor
  {
    protected readonly INamingScope namingScope;
    protected readonly ICollection<Type> interfaces = (ICollection<Type>) new HashSet<Type>();
    private ILogger logger = (ILogger) NullLogger.Instance;
    private readonly ICollection<MetaProperty> properties = (ICollection<MetaProperty>) new TypeElementCollection<MetaProperty>();
    private readonly ICollection<MetaEvent> events = (ICollection<MetaEvent>) new TypeElementCollection<MetaEvent>();
    private readonly ICollection<MetaMethod> methods = (ICollection<MetaMethod>) new TypeElementCollection<MetaMethod>();

    protected CompositeTypeContributor(INamingScope namingScope) => this.namingScope = namingScope;

    public ILogger Logger
    {
      get => this.logger;
      set => this.logger = value;
    }

    public void CollectElementsToProxy(IProxyGenerationHook hook, MetaType model)
    {
      foreach (MembersCollector membersCollector in this.CollectElementsToProxyInternal(hook))
      {
        foreach (MetaMethod method in membersCollector.Methods)
        {
          model.AddMethod(method);
          this.methods.Add(method);
        }
        foreach (MetaEvent @event in membersCollector.Events)
        {
          model.AddEvent(@event);
          this.events.Add(@event);
        }
        foreach (MetaProperty property in membersCollector.Properties)
        {
          model.AddProperty(property);
          this.properties.Add(property);
        }
      }
    }

    protected abstract IEnumerable<MembersCollector> CollectElementsToProxyInternal(
      IProxyGenerationHook hook);

    public virtual void Generate(ClassEmitter @class, ProxyGenerationOptions options)
    {
      foreach (MetaMethod method in (IEnumerable<MetaMethod>) this.methods)
      {
        if (method.Standalone)
          this.ImplementMethod(method, @class, options, new OverrideMethodDelegate(((AbstractTypeEmitter) @class).CreateMethod));
      }
      foreach (MetaProperty property in (IEnumerable<MetaProperty>) this.properties)
        this.ImplementProperty(@class, property, options);
      foreach (MetaEvent @event in (IEnumerable<MetaEvent>) this.events)
        this.ImplementEvent(@class, @event, options);
    }

    public void AddInterfaceToProxy(Type @interface) => this.interfaces.Add(@interface);

    private void ImplementEvent(
      ClassEmitter emitter,
      MetaEvent @event,
      ProxyGenerationOptions options)
    {
      @event.BuildEventEmitter(emitter);
      this.ImplementMethod(@event.Adder, emitter, options, new OverrideMethodDelegate(@event.Emitter.CreateAddMethod));
      this.ImplementMethod(@event.Remover, emitter, options, new OverrideMethodDelegate(@event.Emitter.CreateRemoveMethod));
    }

    private void ImplementProperty(
      ClassEmitter emitter,
      MetaProperty property,
      ProxyGenerationOptions options)
    {
      property.BuildPropertyEmitter(emitter);
      if (property.CanRead)
        this.ImplementMethod(property.Getter, emitter, options, new OverrideMethodDelegate(property.Emitter.CreateGetMethod));
      if (!property.CanWrite)
        return;
      this.ImplementMethod(property.Setter, emitter, options, new OverrideMethodDelegate(property.Emitter.CreateSetMethod));
    }

    protected abstract MethodGenerator GetMethodGenerator(
      MetaMethod method,
      ClassEmitter @class,
      ProxyGenerationOptions options,
      OverrideMethodDelegate overrideMethod);

    private void ImplementMethod(
      MetaMethod method,
      ClassEmitter @class,
      ProxyGenerationOptions options,
      OverrideMethodDelegate overrideMethod)
    {
      MethodGenerator methodGenerator = this.GetMethodGenerator(method, @class, options, overrideMethod);
      if (methodGenerator == null)
        return;
      MethodEmitter methodEmitter = methodGenerator.Generate(@class, options, this.namingScope);
      foreach (CustomAttributeBuilder inheritableAttribute in method.Method.GetNonInheritableAttributes())
        methodEmitter.DefineCustomAttribute(inheritableAttribute);
    }
  }
}
