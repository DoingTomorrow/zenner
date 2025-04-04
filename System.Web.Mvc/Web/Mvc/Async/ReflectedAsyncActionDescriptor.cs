// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.Async.ReflectedAsyncActionDescriptor
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;

#nullable disable
namespace System.Web.Mvc.Async
{
  public class ReflectedAsyncActionDescriptor : AsyncActionDescriptor
  {
    private readonly object _executeTag = new object();
    private readonly string _actionName;
    private readonly ControllerDescriptor _controllerDescriptor;
    private readonly Lazy<string> _uniqueId;
    private ParameterDescriptor[] _parametersCache;

    public ReflectedAsyncActionDescriptor(
      MethodInfo asyncMethodInfo,
      MethodInfo completedMethodInfo,
      string actionName,
      ControllerDescriptor controllerDescriptor)
      : this(asyncMethodInfo, completedMethodInfo, actionName, controllerDescriptor, true)
    {
    }

    internal ReflectedAsyncActionDescriptor(
      MethodInfo asyncMethodInfo,
      MethodInfo completedMethodInfo,
      string actionName,
      ControllerDescriptor controllerDescriptor,
      bool validateMethods)
    {
      if (asyncMethodInfo == (MethodInfo) null)
        throw new ArgumentNullException(nameof (asyncMethodInfo));
      if (completedMethodInfo == (MethodInfo) null)
        throw new ArgumentNullException(nameof (completedMethodInfo));
      if (string.IsNullOrEmpty(actionName))
        throw Error.ParameterCannotBeNullOrEmpty(nameof (actionName));
      if (controllerDescriptor == null)
        throw new ArgumentNullException(nameof (controllerDescriptor));
      if (validateMethods)
      {
        string message1 = ActionDescriptor.VerifyActionMethodIsCallable(asyncMethodInfo);
        if (message1 != null)
          throw new ArgumentException(message1, nameof (asyncMethodInfo));
        string message2 = ActionDescriptor.VerifyActionMethodIsCallable(completedMethodInfo);
        if (message2 != null)
          throw new ArgumentException(message2, nameof (completedMethodInfo));
      }
      this.AsyncMethodInfo = asyncMethodInfo;
      this.CompletedMethodInfo = completedMethodInfo;
      this._actionName = actionName;
      this._controllerDescriptor = controllerDescriptor;
      this._uniqueId = new Lazy<string>(new Func<string>(this.CreateUniqueId));
    }

    public override string ActionName => this._actionName;

    public MethodInfo AsyncMethodInfo { get; private set; }

    public MethodInfo CompletedMethodInfo { get; private set; }

    public override ControllerDescriptor ControllerDescriptor => this._controllerDescriptor;

    public override string UniqueId => this._uniqueId.Value;

    public override IAsyncResult BeginExecute(
      ControllerContext controllerContext,
      IDictionary<string, object> parameters,
      AsyncCallback callback,
      object state)
    {
      if (controllerContext == null)
        throw new ArgumentNullException(nameof (controllerContext));
      if (parameters == null)
        throw new ArgumentNullException(nameof (parameters));
      AsyncManager asyncManager = AsyncActionDescriptor.GetAsyncManager(controllerContext.Controller);
      BeginInvokeDelegate beginDelegate = (BeginInvokeDelegate) ((asyncCallback, asyncState) =>
      {
        object[] array = ((IEnumerable<ParameterInfo>) this.AsyncMethodInfo.GetParameters()).Select<ParameterInfo, object>((Func<ParameterInfo, object>) (parameterInfo => ActionDescriptor.ExtractParameterFromDictionary(parameterInfo, parameters, this.AsyncMethodInfo))).ToArray<object>();
        TriggerListener triggerListener = new TriggerListener();
        SimpleAsyncResult asyncResult = new SimpleAsyncResult(asyncState);
        Trigger finishTrigger = triggerListener.CreateTrigger();
        asyncManager.Finished += (EventHandler) ((param0, param1) => finishTrigger.Fire());
        asyncManager.OutstandingOperations.Increment();
        triggerListener.SetContinuation((Action) (() => ThreadPool.QueueUserWorkItem((WaitCallback) (_ => asyncResult.MarkCompleted(false, asyncCallback)))));
        this.DispatcherCache.GetDispatcher(this.AsyncMethodInfo).Execute(controllerContext.Controller, array);
        asyncManager.OutstandingOperations.Decrement();
        triggerListener.Activate();
        return (IAsyncResult) asyncResult;
      });
      EndInvokeDelegate<object> endDelegate = (EndInvokeDelegate<object>) (asyncResult =>
      {
        object[] array = ((IEnumerable<ParameterInfo>) this.CompletedMethodInfo.GetParameters()).Select<ParameterInfo, object>((Func<ParameterInfo, object>) (parameterInfo => ActionDescriptor.ExtractParameterOrDefaultFromDictionary(parameterInfo, asyncManager.Parameters))).ToArray<object>();
        return this.DispatcherCache.GetDispatcher(this.CompletedMethodInfo).Execute(controllerContext.Controller, array);
      });
      return AsyncResultWrapper.Begin<object>(callback, state, beginDelegate, endDelegate, this._executeTag, asyncManager.Timeout);
    }

    private string CreateUniqueId()
    {
      return base.UniqueId + DescriptorUtil.CreateUniqueId((object) this.AsyncMethodInfo, (object) this.CompletedMethodInfo);
    }

    public override object EndExecute(IAsyncResult asyncResult)
    {
      return AsyncResultWrapper.End<object>(asyncResult, this._executeTag);
    }

    public override object[] GetCustomAttributes(bool inherit)
    {
      return ActionDescriptorHelper.GetCustomAttributes((MemberInfo) this.AsyncMethodInfo, inherit);
    }

    public override object[] GetCustomAttributes(Type attributeType, bool inherit)
    {
      return ActionDescriptorHelper.GetCustomAttributes((MemberInfo) this.AsyncMethodInfo, attributeType, inherit);
    }

    public override IEnumerable<FilterAttribute> GetFilterAttributes(bool useCache)
    {
      return useCache && this.GetType() == typeof (ReflectedAsyncActionDescriptor) ? (IEnumerable<FilterAttribute>) ReflectedAttributeCache.GetMethodFilterAttributes(this.AsyncMethodInfo) : base.GetFilterAttributes(useCache);
    }

    public override ParameterDescriptor[] GetParameters()
    {
      return ActionDescriptorHelper.GetParameters((ActionDescriptor) this, this.AsyncMethodInfo, ref this._parametersCache);
    }

    public override ICollection<ActionSelector> GetSelectors()
    {
      return ActionDescriptorHelper.GetSelectors(this.AsyncMethodInfo);
    }

    public override bool IsDefined(Type attributeType, bool inherit)
    {
      return ActionDescriptorHelper.IsDefined((MemberInfo) this.AsyncMethodInfo, attributeType, inherit);
    }
  }
}
