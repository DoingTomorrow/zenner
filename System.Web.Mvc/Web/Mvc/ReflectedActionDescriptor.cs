// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.ReflectedActionDescriptor
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc.Properties;

#nullable disable
namespace System.Web.Mvc
{
  public class ReflectedActionDescriptor : ActionDescriptor
  {
    private readonly string _actionName;
    private readonly ControllerDescriptor _controllerDescriptor;
    private readonly Lazy<string> _uniqueId;
    private ParameterDescriptor[] _parametersCache;

    public ReflectedActionDescriptor(
      MethodInfo methodInfo,
      string actionName,
      ControllerDescriptor controllerDescriptor)
      : this(methodInfo, actionName, controllerDescriptor, true)
    {
    }

    internal ReflectedActionDescriptor(
      MethodInfo methodInfo,
      string actionName,
      ControllerDescriptor controllerDescriptor,
      bool validateMethod)
    {
      if (methodInfo == (MethodInfo) null)
        throw new ArgumentNullException(nameof (methodInfo));
      if (string.IsNullOrEmpty(actionName))
        throw new ArgumentException(MvcResources.Common_NullOrEmpty, nameof (actionName));
      if (controllerDescriptor == null)
        throw new ArgumentNullException(nameof (controllerDescriptor));
      if (validateMethod)
      {
        string message = ActionDescriptor.VerifyActionMethodIsCallable(methodInfo);
        if (message != null)
          throw new ArgumentException(message, nameof (methodInfo));
      }
      this.MethodInfo = methodInfo;
      this._actionName = actionName;
      this._controllerDescriptor = controllerDescriptor;
      this._uniqueId = new Lazy<string>(new Func<string>(this.CreateUniqueId));
    }

    public override string ActionName => this._actionName;

    public override ControllerDescriptor ControllerDescriptor => this._controllerDescriptor;

    public MethodInfo MethodInfo { get; private set; }

    public override string UniqueId => this._uniqueId.Value;

    private string CreateUniqueId()
    {
      return base.UniqueId + DescriptorUtil.CreateUniqueId((object) this.MethodInfo);
    }

    public override object Execute(
      ControllerContext controllerContext,
      IDictionary<string, object> parameters)
    {
      if (controllerContext == null)
        throw new ArgumentNullException(nameof (controllerContext));
      if (parameters == null)
        throw new ArgumentNullException(nameof (parameters));
      object[] array = ((IEnumerable<ParameterInfo>) this.MethodInfo.GetParameters()).Select<ParameterInfo, object>((Func<ParameterInfo, object>) (parameterInfo => ActionDescriptor.ExtractParameterFromDictionary(parameterInfo, parameters, this.MethodInfo))).ToArray<object>();
      return this.DispatcherCache.GetDispatcher(this.MethodInfo).Execute(controllerContext.Controller, array);
    }

    public override object[] GetCustomAttributes(bool inherit)
    {
      return ActionDescriptorHelper.GetCustomAttributes((MemberInfo) this.MethodInfo, inherit);
    }

    public override object[] GetCustomAttributes(Type attributeType, bool inherit)
    {
      return ActionDescriptorHelper.GetCustomAttributes((MemberInfo) this.MethodInfo, attributeType, inherit);
    }

    public override IEnumerable<FilterAttribute> GetFilterAttributes(bool useCache)
    {
      return useCache && this.GetType() == typeof (ReflectedActionDescriptor) ? (IEnumerable<FilterAttribute>) ReflectedAttributeCache.GetMethodFilterAttributes(this.MethodInfo) : base.GetFilterAttributes(useCache);
    }

    public override ParameterDescriptor[] GetParameters()
    {
      return ActionDescriptorHelper.GetParameters((ActionDescriptor) this, this.MethodInfo, ref this._parametersCache);
    }

    public override ICollection<ActionSelector> GetSelectors()
    {
      return ActionDescriptorHelper.GetSelectors(this.MethodInfo);
    }

    public override bool IsDefined(Type attributeType, bool inherit)
    {
      return ActionDescriptorHelper.IsDefined((MemberInfo) this.MethodInfo, attributeType, inherit);
    }

    internal static ReflectedActionDescriptor TryCreateDescriptor(
      MethodInfo methodInfo,
      string name,
      ControllerDescriptor controllerDescriptor)
    {
      ReflectedActionDescriptor actionDescriptor = new ReflectedActionDescriptor(methodInfo, name, controllerDescriptor, false);
      return ActionDescriptor.VerifyActionMethodIsCallable(methodInfo) != null ? (ReflectedActionDescriptor) null : actionDescriptor;
    }
  }
}
