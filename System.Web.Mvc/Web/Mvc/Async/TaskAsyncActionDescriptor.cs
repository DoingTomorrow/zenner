// Decompiled with JetBrains decompiler
// Type: System.Web.Mvc.Async.TaskAsyncActionDescriptor
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc.Properties;

#nullable disable
namespace System.Web.Mvc.Async
{
  public class TaskAsyncActionDescriptor : AsyncActionDescriptor
  {
    private static readonly ConcurrentDictionary<Type, Func<object, object>> _taskValueExtractors = new ConcurrentDictionary<Type, Func<object, object>>();
    private readonly string _actionName;
    private readonly ControllerDescriptor _controllerDescriptor;
    private readonly Lazy<string> _uniqueId;
    private ParameterDescriptor[] _parametersCache;

    public TaskAsyncActionDescriptor(
      MethodInfo taskMethodInfo,
      string actionName,
      ControllerDescriptor controllerDescriptor)
      : this(taskMethodInfo, actionName, controllerDescriptor, true)
    {
    }

    internal TaskAsyncActionDescriptor(
      MethodInfo taskMethodInfo,
      string actionName,
      ControllerDescriptor controllerDescriptor,
      bool validateMethod)
    {
      if (taskMethodInfo == (MethodInfo) null)
        throw new ArgumentNullException(nameof (taskMethodInfo));
      if (string.IsNullOrEmpty(actionName))
        throw Error.ParameterCannotBeNullOrEmpty(nameof (actionName));
      if (controllerDescriptor == null)
        throw new ArgumentNullException(nameof (controllerDescriptor));
      if (validateMethod)
      {
        string message = ActionDescriptor.VerifyActionMethodIsCallable(taskMethodInfo);
        if (message != null)
          throw new ArgumentException(message, nameof (taskMethodInfo));
      }
      this.TaskMethodInfo = taskMethodInfo;
      this._actionName = actionName;
      this._controllerDescriptor = controllerDescriptor;
      this._uniqueId = new Lazy<string>(new Func<string>(this.CreateUniqueId));
    }

    public override string ActionName => this._actionName;

    public MethodInfo TaskMethodInfo { get; private set; }

    public override ControllerDescriptor ControllerDescriptor => this._controllerDescriptor;

    public override string UniqueId => this._uniqueId.Value;

    private string CreateUniqueId()
    {
      return base.UniqueId + DescriptorUtil.CreateUniqueId((object) this.TaskMethodInfo);
    }

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
      object[] array = ((IEnumerable<ParameterInfo>) this.TaskMethodInfo.GetParameters()).Select<ParameterInfo, object>((Func<ParameterInfo, object>) (parameterInfo => ActionDescriptor.ExtractParameterFromDictionary(parameterInfo, parameters, this.TaskMethodInfo))).ToArray<object>();
      CancellationTokenSource tokenSource = (CancellationTokenSource) null;
      bool disposedTimer = false;
      Timer taskCancelledTimer = (Timer) null;
      bool flag = false;
      int timeout = AsyncActionDescriptor.GetAsyncManager(controllerContext.Controller).Timeout;
      for (int index = 0; index < array.Length; ++index)
      {
        if (new CancellationToken().Equals(array[index]))
        {
          tokenSource = new CancellationTokenSource();
          array[index] = (object) tokenSource.Token;
          flag = timeout > -1;
          break;
        }
      }
      ActionMethodDispatcher dispatcher = this.DispatcherCache.GetDispatcher(this.TaskMethodInfo);
      if (flag)
        taskCancelledTimer = new Timer((TimerCallback) (_ =>
        {
          lock (tokenSource)
          {
            if (disposedTimer)
              return;
            tokenSource.Cancel();
          }
        }), (object) null, timeout, -1);
      Task task = dispatcher.Execute(controllerContext.Controller, array) as Task;
      Action cleanupThunk = (Action) (() =>
      {
        taskCancelledTimer?.Dispose();
        if (tokenSource == null)
          return;
        lock (tokenSource)
        {
          disposedTimer = true;
          tokenSource.Dispose();
          if (tokenSource.IsCancellationRequested)
            throw new TimeoutException();
        }
      });
      TaskWrapperAsyncResult result = new TaskWrapperAsyncResult(task, state, cleanupThunk);
      if (callback != null)
      {
        if (task.IsCompleted)
        {
          result.CompletedSynchronously = true;
          callback((IAsyncResult) result);
        }
        else
        {
          result.CompletedSynchronously = false;
          task.ContinueWith((Action<Task>) (_ => callback((IAsyncResult) result)));
        }
      }
      return (IAsyncResult) result;
    }

    public override object Execute(
      ControllerContext controllerContext,
      IDictionary<string, object> parameters)
    {
      throw new InvalidOperationException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, MvcResources.TaskAsyncActionDescriptor_CannotExecuteSynchronously, new object[1]
      {
        (object) this.ActionName
      }));
    }

    public override object EndExecute(IAsyncResult asyncResult)
    {
      TaskWrapperAsyncResult wrapperAsyncResult = (TaskWrapperAsyncResult) asyncResult;
      try
      {
        wrapperAsyncResult.Task.ThrowIfFaulted();
      }
      finally
      {
        if (wrapperAsyncResult.CleanupThunk != null)
          wrapperAsyncResult.CleanupThunk();
      }
      return TaskAsyncActionDescriptor._taskValueExtractors.GetOrAdd(this.TaskMethodInfo.ReturnType, new Func<Type, Func<object, object>>(TaskAsyncActionDescriptor.CreateTaskValueExtractor))((object) wrapperAsyncResult.Task);
    }

    private static Func<object, object> CreateTaskValueExtractor(Type taskType)
    {
      if (!taskType.IsGenericType || !(taskType.GetGenericTypeDefinition() == typeof (Task<>)))
        return (Func<object, object>) (theTask => (object) null);
      ParameterExpression parameterExpression;
      return Expression.Lambda<Func<object, object>>((Expression) Expression.Convert((Expression) Expression.Property((Expression) Expression.Convert((Expression) parameterExpression, taskType), "Result"), typeof (object)), parameterExpression).Compile();
    }

    public override object[] GetCustomAttributes(bool inherit)
    {
      return ActionDescriptorHelper.GetCustomAttributes((MemberInfo) this.TaskMethodInfo, inherit);
    }

    public override object[] GetCustomAttributes(Type attributeType, bool inherit)
    {
      return ActionDescriptorHelper.GetCustomAttributes((MemberInfo) this.TaskMethodInfo, attributeType, inherit);
    }

    public override ParameterDescriptor[] GetParameters()
    {
      return ActionDescriptorHelper.GetParameters((ActionDescriptor) this, this.TaskMethodInfo, ref this._parametersCache);
    }

    public override ICollection<ActionSelector> GetSelectors()
    {
      return ActionDescriptorHelper.GetSelectors(this.TaskMethodInfo);
    }

    public override bool IsDefined(Type attributeType, bool inherit)
    {
      return ActionDescriptorHelper.IsDefined((MemberInfo) this.TaskMethodInfo, attributeType, inherit);
    }

    public override IEnumerable<FilterAttribute> GetFilterAttributes(bool useCache)
    {
      return useCache && this.GetType() == typeof (TaskAsyncActionDescriptor) ? (IEnumerable<FilterAttribute>) ReflectedAttributeCache.GetMethodFilterAttributes(this.TaskMethodInfo) : base.GetFilterAttributes(useCache);
    }
  }
}
