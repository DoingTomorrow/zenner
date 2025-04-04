// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.Async.ReflectedAsyncControllerDescriptor
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Collections.Generic;

#nullable disable
namespace System.Web.Mvc.Async
{
  public class ReflectedAsyncControllerDescriptor : ControllerDescriptor
  {
    private static readonly ActionDescriptor[] _emptyCanonicalActions = new ActionDescriptor[0];
    private readonly Type _controllerType;
    private readonly AsyncActionMethodSelector _selector;

    public ReflectedAsyncControllerDescriptor(Type controllerType)
    {
      this._controllerType = !(controllerType == (Type) null) ? controllerType : throw new ArgumentNullException(nameof (controllerType));
      this._selector = new AsyncActionMethodSelector(this._controllerType, ReflectedAsyncControllerDescriptor.AllowLegacyAsyncActions(this._controllerType));
    }

    public override sealed Type ControllerType => this._controllerType;

    private static bool AllowLegacyAsyncActions(Type controllerType)
    {
      return typeof (AsyncController).IsAssignableFrom(controllerType) || !typeof (Controller).IsAssignableFrom(controllerType) && typeof (IAsyncController).IsAssignableFrom(controllerType);
    }

    public override ActionDescriptor FindAction(
      ControllerContext controllerContext,
      string actionName)
    {
      if (controllerContext == null)
        throw new ArgumentNullException(nameof (controllerContext));
      if (string.IsNullOrEmpty(actionName))
        throw Error.ParameterCannotBeNullOrEmpty(nameof (actionName));
      ActionDescriptorCreator action = this._selector.FindAction(controllerContext, actionName);
      return action == null ? (ActionDescriptor) null : action(actionName, (ControllerDescriptor) this);
    }

    public override ActionDescriptor[] GetCanonicalActions()
    {
      return ReflectedAsyncControllerDescriptor._emptyCanonicalActions;
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
      return useCache && this.GetType() == typeof (ReflectedAsyncControllerDescriptor) ? (IEnumerable<FilterAttribute>) ReflectedAttributeCache.GetTypeFilterAttributes(this.ControllerType) : base.GetFilterAttributes(useCache);
    }

    public override bool IsDefined(Type attributeType, bool inherit)
    {
      return this.ControllerType.IsDefined(attributeType, inherit);
    }
  }
}
