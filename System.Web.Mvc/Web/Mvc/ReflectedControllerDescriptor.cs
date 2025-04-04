// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.ReflectedControllerDescriptor
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
  public class ReflectedControllerDescriptor : ControllerDescriptor
  {
    private readonly Type _controllerType;
    private readonly ActionMethodSelector _selector;
    private ActionDescriptor[] _canonicalActionsCache;

    public ReflectedControllerDescriptor(Type controllerType)
    {
      this._controllerType = !(controllerType == (Type) null) ? controllerType : throw new ArgumentNullException(nameof (controllerType));
      this._selector = new ActionMethodSelector(this._controllerType);
    }

    public override sealed Type ControllerType => this._controllerType;

    public override ActionDescriptor FindAction(
      ControllerContext controllerContext,
      string actionName)
    {
      if (controllerContext == null)
        throw new ArgumentNullException(nameof (controllerContext));
      if (string.IsNullOrEmpty(actionName))
        throw new ArgumentException(MvcResources.Common_NullOrEmpty, nameof (actionName));
      MethodInfo actionMethod = this._selector.FindActionMethod(controllerContext, actionName);
      return actionMethod == (MethodInfo) null ? (ActionDescriptor) null : (ActionDescriptor) new ReflectedActionDescriptor(actionMethod, actionName, (ControllerDescriptor) this);
    }

    private MethodInfo[] GetAllActionMethodsFromSelector()
    {
      List<MethodInfo> methodInfoList = new List<MethodInfo>();
      methodInfoList.AddRange((IEnumerable<MethodInfo>) this._selector.AliasedMethods);
      methodInfoList.AddRange(this._selector.NonAliasedMethods.SelectMany<IGrouping<string, MethodInfo>, MethodInfo>((Func<IGrouping<string, MethodInfo>, IEnumerable<MethodInfo>>) (g => (IEnumerable<MethodInfo>) g)));
      return methodInfoList.ToArray();
    }

    public override ActionDescriptor[] GetCanonicalActions()
    {
      return (ActionDescriptor[]) this.LazilyFetchCanonicalActionsCollection().Clone();
    }

    public override object[] GetCustomAttributes(bool inherit)
    {
      return this.ControllerType.GetCustomAttributes(inherit);
    }

    public override object[] GetCustomAttributes(Type attributeType, bool inherit)
    {
      return this.ControllerType.GetCustomAttributes(attributeType, inherit);
    }

    public override IEnumerable<FilterAttribute> GetFilterAttributes(bool useCache)
    {
      return useCache && this.GetType() == typeof (ReflectedControllerDescriptor) ? (IEnumerable<FilterAttribute>) ReflectedAttributeCache.GetTypeFilterAttributes(this.ControllerType) : base.GetFilterAttributes(useCache);
    }

    public override bool IsDefined(Type attributeType, bool inherit)
    {
      return this.ControllerType.IsDefined(attributeType, inherit);
    }

    private ActionDescriptor[] LazilyFetchCanonicalActionsCollection()
    {
      return DescriptorUtil.LazilyFetchOrCreateDescriptors<MethodInfo, ActionDescriptor>(ref this._canonicalActionsCache, new Func<MethodInfo[]>(this.GetAllActionMethodsFromSelector), (Func<MethodInfo, ActionDescriptor>) (methodInfo => (ActionDescriptor) ReflectedActionDescriptor.TryCreateDescriptor(methodInfo, methodInfo.Name, (ControllerDescriptor) this)));
    }
  }
}
