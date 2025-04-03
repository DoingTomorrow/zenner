// Decompiled with JetBrains decompiler
// Type: Castle.DynamicProxy.Contributors.MembersCollector
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using Castle.Core.Logging;
using Castle.DynamicProxy.Generators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

#nullable disable
namespace Castle.DynamicProxy.Contributors
{
  public abstract class MembersCollector
  {
    private const BindingFlags Flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
    private ILogger logger = (ILogger) NullLogger.Instance;
    private ICollection<MethodInfo> checkedMethods = (ICollection<MethodInfo>) new HashSet<MethodInfo>();
    private readonly IDictionary<PropertyInfo, MetaProperty> properties = (IDictionary<PropertyInfo, MetaProperty>) new Dictionary<PropertyInfo, MetaProperty>();
    private readonly IDictionary<EventInfo, MetaEvent> events = (IDictionary<EventInfo, MetaEvent>) new Dictionary<EventInfo, MetaEvent>();
    private readonly IDictionary<MethodInfo, MetaMethod> methods = (IDictionary<MethodInfo, MetaMethod>) new Dictionary<MethodInfo, MetaMethod>();
    protected readonly Type type;

    protected MembersCollector(Type type) => this.type = type;

    public ILogger Logger
    {
      get => this.logger;
      set => this.logger = value;
    }

    public IEnumerable<MetaMethod> Methods => (IEnumerable<MetaMethod>) this.methods.Values;

    public IEnumerable<MetaProperty> Properties
    {
      get => (IEnumerable<MetaProperty>) this.properties.Values;
    }

    public IEnumerable<MetaEvent> Events => (IEnumerable<MetaEvent>) this.events.Values;

    public virtual void CollectMembersToProxy(IProxyGenerationHook hook)
    {
      if (this.checkedMethods == null)
        throw new InvalidOperationException(string.Format("Can't call 'CollectMembersToProxy' method twice. This usually signifies a bug in custom {0}.", (object) typeof (ITypeContributor)));
      this.CollectProperties(hook);
      this.CollectEvents(hook);
      this.CollectMethods(hook);
      this.checkedMethods = (ICollection<MethodInfo>) null;
    }

    private void CollectProperties(IProxyGenerationHook hook)
    {
      foreach (PropertyInfo property in this.type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
        this.AddProperty(property, hook);
    }

    private void CollectEvents(IProxyGenerationHook hook)
    {
      foreach (EventInfo @event in this.type.GetEvents(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
        this.AddEvent(@event, hook);
    }

    private void CollectMethods(IProxyGenerationHook hook)
    {
      foreach (MethodInfo allInstanceMethod in MethodFinder.GetAllInstanceMethods(this.type, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
        this.AddMethod(allInstanceMethod, hook, true);
    }

    private void AddProperty(PropertyInfo property, IProxyGenerationHook hook)
    {
      MetaMethod getter = (MetaMethod) null;
      MetaMethod setter = (MetaMethod) null;
      if (property.CanRead)
        getter = this.AddMethod(property.GetGetMethod(true), hook, false);
      if (property.CanWrite)
        setter = this.AddMethod(property.GetSetMethod(true), hook, false);
      if (setter == null && getter == null)
        return;
      IEnumerable<CustomAttributeBuilder> inheritableAttributes = property.GetNonInheritableAttributes();
      ParameterInfo[] indexParameters = property.GetIndexParameters();
      this.properties[property] = new MetaProperty(property.Name, property.PropertyType, property.DeclaringType, getter, setter, inheritableAttributes, ((IEnumerable<ParameterInfo>) indexParameters).Select<ParameterInfo, Type>((Func<ParameterInfo, Type>) (a => a.ParameterType)).ToArray<Type>());
    }

    private void AddEvent(EventInfo @event, IProxyGenerationHook hook)
    {
      MethodInfo addMethod = @event.GetAddMethod(true);
      MethodInfo removeMethod = @event.GetRemoveMethod(true);
      MetaMethod adder = (MetaMethod) null;
      MetaMethod remover = (MetaMethod) null;
      if (addMethod != null)
        adder = this.AddMethod(addMethod, hook, false);
      if (removeMethod != null)
        remover = this.AddMethod(removeMethod, hook, false);
      if (adder == null && remover == null)
        return;
      this.events[@event] = new MetaEvent(@event.Name, @event.DeclaringType, @event.EventHandlerType, adder, remover, EventAttributes.None);
    }

    private MetaMethod AddMethod(MethodInfo method, IProxyGenerationHook hook, bool isStandalone)
    {
      if (this.checkedMethods.Contains(method))
        return (MetaMethod) null;
      this.checkedMethods.Add(method);
      if (this.methods.ContainsKey(method))
        return (MetaMethod) null;
      MetaMethod methodToGenerate = this.GetMethodToGenerate(method, hook, isStandalone);
      if (methodToGenerate != null)
        this.methods[method] = methodToGenerate;
      return methodToGenerate;
    }

    protected abstract MetaMethod GetMethodToGenerate(
      MethodInfo method,
      IProxyGenerationHook hook,
      bool isStandalone);

    protected bool IsAccessible(MethodBase method)
    {
      return method.IsPublic || method.IsFamily || method.IsFamilyOrAssembly || method.IsFamilyAndAssembly || InternalsHelper.IsInternalToDynamicProxy(method.DeclaringType.Assembly) && method.IsAssembly;
    }

    protected bool AcceptMethod(MethodInfo method, bool onlyVirtuals, IProxyGenerationHook hook)
    {
      if (method.IsFinal)
      {
        this.Logger.Debug("Excluded sealed method {0} on {1} because it cannot be intercepted.", (object) method.Name, (object) method.DeclaringType.FullName);
        return false;
      }
      bool flag = InternalsHelper.IsInternal(method);
      if (flag)
        flag = !InternalsHelper.IsInternalToDynamicProxy(method.DeclaringType.Assembly);
      if (flag)
        return false;
      if (onlyVirtuals && !method.IsVirtual)
      {
        if (method.DeclaringType != typeof (MarshalByRefObject))
        {
          this.Logger.Debug("Excluded non-virtual method {0} on {1} because it cannot be intercepted.", (object) method.Name, (object) method.DeclaringType.FullName);
          hook.NonProxyableMemberNotification(this.type, (MemberInfo) method);
        }
        return false;
      }
      return (method.IsPublic || method.IsFamily || method.IsAssembly || method.IsFamilyOrAssembly) && method.DeclaringType != typeof (MarshalByRefObject) && (method.DeclaringType != typeof (object) || !method.Name.Equals("Finalize", StringComparison.OrdinalIgnoreCase)) && hook.ShouldInterceptMethod(this.type, method);
    }
  }
}
