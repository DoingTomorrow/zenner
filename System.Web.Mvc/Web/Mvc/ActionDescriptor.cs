// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.ActionDescriptor
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web.Mvc.Properties;

#nullable disable
namespace System.Web.Mvc
{
  public abstract class ActionDescriptor : ICustomAttributeProvider, IUniquelyIdentifiable
  {
    private static readonly ActionMethodDispatcherCache _staticDispatcherCache = new ActionMethodDispatcherCache();
    private static readonly ActionSelector[] _emptySelectors = new ActionSelector[0];
    private readonly Lazy<string> _uniqueId;
    private ActionMethodDispatcherCache _instanceDispatcherCache;

    protected ActionDescriptor()
    {
      this._uniqueId = new Lazy<string>(new Func<string>(this.CreateUniqueId));
    }

    public abstract string ActionName { get; }

    public abstract ControllerDescriptor ControllerDescriptor { get; }

    internal ActionMethodDispatcherCache DispatcherCache
    {
      get
      {
        if (this._instanceDispatcherCache == null)
          this._instanceDispatcherCache = ActionDescriptor._staticDispatcherCache;
        return this._instanceDispatcherCache;
      }
      set => this._instanceDispatcherCache = value;
    }

    public virtual string UniqueId => this._uniqueId.Value;

    private string CreateUniqueId()
    {
      return DescriptorUtil.CreateUniqueId((object) this.GetType(), (object) this.ControllerDescriptor, (object) this.ActionName);
    }

    public abstract object Execute(
      ControllerContext controllerContext,
      IDictionary<string, object> parameters);

    internal static object ExtractParameterFromDictionary(
      ParameterInfo parameterInfo,
      IDictionary<string, object> parameters,
      MethodInfo methodInfo)
    {
      object o;
      if (!parameters.TryGetValue(parameterInfo.Name, out o))
        throw new ArgumentException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, MvcResources.ReflectedActionDescriptor_ParameterNotInDictionary, (object) parameterInfo.Name, (object) parameterInfo.ParameterType, (object) methodInfo, (object) methodInfo.DeclaringType), nameof (parameters));
      if (o == null && !TypeHelpers.TypeAllowsNullValue(parameterInfo.ParameterType))
        throw new ArgumentException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, MvcResources.ReflectedActionDescriptor_ParameterCannotBeNull, (object) parameterInfo.Name, (object) parameterInfo.ParameterType, (object) methodInfo, (object) methodInfo.DeclaringType), nameof (parameters));
      if (o != null && !parameterInfo.ParameterType.IsInstanceOfType(o))
        throw new ArgumentException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, MvcResources.ReflectedActionDescriptor_ParameterValueHasWrongType, (object) parameterInfo.Name, (object) methodInfo, (object) methodInfo.DeclaringType, (object) o.GetType(), (object) parameterInfo.ParameterType), nameof (parameters));
      return o;
    }

    internal static object ExtractParameterOrDefaultFromDictionary(
      ParameterInfo parameterInfo,
      IDictionary<string, object> parameters)
    {
      Type parameterType = parameterInfo.ParameterType;
      object o;
      parameters.TryGetValue(parameterInfo.Name, out o);
      if (parameterType.IsInstanceOfType(o))
        return o;
      object obj;
      return ParameterInfoUtil.TryGetDefaultValue(parameterInfo, out obj) ? obj : TypeHelpers.GetDefaultValue(parameterType);
    }

    public virtual object[] GetCustomAttributes(bool inherit)
    {
      return this.GetCustomAttributes(typeof (object), inherit);
    }

    public virtual object[] GetCustomAttributes(Type attributeType, bool inherit)
    {
      return !(attributeType == (Type) null) ? (object[]) Array.CreateInstance(attributeType, 0) : throw new ArgumentNullException(nameof (attributeType));
    }

    public virtual IEnumerable<FilterAttribute> GetFilterAttributes(bool useCache)
    {
      return this.GetCustomAttributes(typeof (FilterAttribute), true).Cast<FilterAttribute>();
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("Please call System.Web.Mvc.FilterProviders.Providers.GetFilters() now.", true)]
    public virtual FilterInfo GetFilters() => new FilterInfo();

    public abstract ParameterDescriptor[] GetParameters();

    public virtual ICollection<ActionSelector> GetSelectors()
    {
      return (ICollection<ActionSelector>) ActionDescriptor._emptySelectors;
    }

    public virtual bool IsDefined(Type attributeType, bool inherit)
    {
      if (attributeType == (Type) null)
        throw new ArgumentNullException(nameof (attributeType));
      return false;
    }

    internal static string VerifyActionMethodIsCallable(MethodInfo methodInfo)
    {
      if (methodInfo.IsStatic)
        return string.Format((IFormatProvider) CultureInfo.CurrentCulture, MvcResources.ReflectedActionDescriptor_CannotCallStaticMethod, new object[2]
        {
          (object) methodInfo,
          (object) methodInfo.ReflectedType.FullName
        });
      if (!typeof (ControllerBase).IsAssignableFrom(methodInfo.ReflectedType))
        return string.Format((IFormatProvider) CultureInfo.CurrentCulture, MvcResources.ReflectedActionDescriptor_CannotCallInstanceMethodOnNonControllerType, new object[2]
        {
          (object) methodInfo,
          (object) methodInfo.ReflectedType.FullName
        });
      if (methodInfo.ContainsGenericParameters)
        return string.Format((IFormatProvider) CultureInfo.CurrentCulture, MvcResources.ReflectedActionDescriptor_CannotCallOpenGenericMethods, new object[2]
        {
          (object) methodInfo,
          (object) methodInfo.ReflectedType.FullName
        });
      foreach (ParameterInfo parameter in methodInfo.GetParameters())
      {
        if (parameter.IsOut || parameter.ParameterType.IsByRef)
          return string.Format((IFormatProvider) CultureInfo.CurrentCulture, MvcResources.ReflectedActionDescriptor_CannotCallMethodsWithOutOrRefParameters, new object[3]
          {
            (object) methodInfo,
            (object) methodInfo.ReflectedType.FullName,
            (object) parameter
          });
      }
      return (string) null;
    }
  }
}
