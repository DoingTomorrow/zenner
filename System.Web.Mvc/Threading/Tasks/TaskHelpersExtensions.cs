// Decompiled with JetBrains decompiler
// Type: System.Threading.Tasks.TaskHelpersExtensions
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices;

#nullable disable
namespace System.Threading.Tasks
{
  internal static class TaskHelpersExtensions
  {
    private static Task<TaskHelpersExtensions.AsyncVoid> _defaultCompleted = TaskHelpers.FromResult<TaskHelpersExtensions.AsyncVoid>(new TaskHelpersExtensions.AsyncVoid());
    private static readonly Action<Task> _rethrowWithNoStackLossDelegate = TaskHelpersExtensions.GetRethrowWithNoStackLossDelegate();

    internal static Task Catch(
      this Task task,
      Func<CatchInfo, CatchInfoBase<Task>.CatchResult> continuation,
      CancellationToken cancellationToken = default (CancellationToken))
    {
      return task.Status == TaskStatus.RanToCompletion ? task : (Task) task.CatchImpl<TaskHelpersExtensions.AsyncVoid>((Func<Task<TaskHelpersExtensions.AsyncVoid>>) (() => continuation(new CatchInfo(task)).Task.ToTask<TaskHelpersExtensions.AsyncVoid>()), cancellationToken);
    }

    internal static Task<TResult> Catch<TResult>(
      this Task<TResult> task,
      Func<CatchInfo<TResult>, CatchInfoBase<Task<TResult>>.CatchResult> continuation,
      CancellationToken cancellationToken = default (CancellationToken))
    {
      return task.Status == TaskStatus.RanToCompletion ? task : task.CatchImpl<TResult>((Func<Task<TResult>>) (() => continuation(new CatchInfo<TResult>(task)).Task), cancellationToken);
    }

    private static Task<TResult> CatchImpl<TResult>(
      this Task task,
      Func<Task<TResult>> continuation,
      CancellationToken cancellationToken)
    {
      if (task.IsCompleted)
      {
        if (task.IsFaulted)
        {
          try
          {
            return continuation() ?? throw new InvalidOperationException("You must set the Task property of the CatchInfo returned from the TaskHelpersExtensions.Catch continuation.");
          }
          catch (Exception ex)
          {
            return TaskHelpers.FromError<TResult>(ex);
          }
        }
        else
        {
          if (task.IsCanceled || cancellationToken.IsCancellationRequested)
            return TaskHelpers.Canceled<TResult>();
          if (task.Status == TaskStatus.RanToCompletion)
          {
            TaskCompletionSource<TResult> tcs = new TaskCompletionSource<TResult>();
            tcs.TrySetFromTask<TResult>(task);
            return tcs.Task;
          }
        }
      }
      return TaskHelpersExtensions.CatchImplContinuation<TResult>(task, continuation);
    }

    private static Task<TResult> CatchImplContinuation<TResult>(
      Task task,
      Func<Task<TResult>> continuation)
    {
      SynchronizationContext syncContext = SynchronizationContext.Current;
      TaskCompletionSource<Task<TResult>> tcs = new TaskCompletionSource<Task<TResult>>();
      task.ContinueWith<bool>((Func<Task, bool>) (innerTask => tcs.TrySetFromTask<TResult>(innerTask)), TaskContinuationOptions.NotOnFaulted | TaskContinuationOptions.ExecuteSynchronously);
      task.ContinueWith((Action<Task>) (innerTask =>
      {
        if (syncContext != null)
        {
          syncContext.Post((SendOrPostCallback) (state =>
          {
            try
            {
              tcs.TrySetResult(continuation() ?? throw new InvalidOperationException("You cannot return null from the TaskHelpersExtensions.Catch continuation. You must return a valid task or throw an exception."));
            }
            catch (Exception ex)
            {
              tcs.TrySetException(ex);
            }
          }), (object) null);
        }
        else
        {
          try
          {
            tcs.TrySetResult(continuation() ?? throw new InvalidOperationException("You cannot return null from the TaskHelpersExtensions.Catch continuation. You must return a valid task or throw an exception."));
          }
          catch (Exception ex)
          {
            tcs.TrySetException(ex);
          }
        }
      }), TaskContinuationOptions.OnlyOnFaulted);
      return tcs.Task.FastUnwrap<TResult>();
    }

    internal static Task CopyResultToCompletionSource<TResult>(
      this Task task,
      TaskCompletionSource<TResult> tcs,
      TResult completionResult)
    {
      return task.CopyResultToCompletionSourceImpl<Task, TResult>(tcs, (Func<Task, TResult>) (innerTask => completionResult));
    }

    internal static Task CopyResultToCompletionSource<TResult>(
      this Task<TResult> task,
      TaskCompletionSource<TResult> tcs)
    {
      return task.CopyResultToCompletionSourceImpl<Task<TResult>, TResult>(tcs, (Func<Task<TResult>, TResult>) (innerTask => innerTask.Result));
    }

    private static Task CopyResultToCompletionSourceImpl<TTask, TResult>(
      this TTask task,
      TaskCompletionSource<TResult> tcs,
      Func<TTask, TResult> resultThunk)
      where TTask : Task
    {
      if (!task.IsCompleted)
        return TaskHelpersExtensions.CopyResultToCompletionSourceImplContinuation<TTask, TResult>(task, tcs, resultThunk);
      switch (task.Status)
      {
        case TaskStatus.RanToCompletion:
          tcs.TrySetResult(resultThunk(task));
          break;
        case TaskStatus.Canceled:
        case TaskStatus.Faulted:
          tcs.TrySetFromTask<TResult>((Task) task);
          break;
      }
      return TaskHelpers.Completed();
    }

    private static Task CopyResultToCompletionSourceImplContinuation<TTask, TResult>(
      TTask task,
      TaskCompletionSource<TResult> tcs,
      Func<TTask, TResult> resultThunk)
      where TTask : Task
    {
      return task.ContinueWith((Action<Task>) (innerTask =>
      {
        switch (innerTask.Status)
        {
          case TaskStatus.RanToCompletion:
            tcs.TrySetResult(resultThunk(task));
            break;
          case TaskStatus.Canceled:
          case TaskStatus.Faulted:
            tcs.TrySetFromTask<TResult>(innerTask);
            break;
        }
      }), TaskContinuationOptions.ExecuteSynchronously);
    }

    internal static Task<object> CastToObject(this Task task)
    {
      if (task.IsCompleted)
      {
        if (task.IsFaulted)
          return TaskHelpers.FromErrors<object>((IEnumerable<Exception>) task.Exception.InnerExceptions);
        if (task.IsCanceled)
          return TaskHelpers.Canceled<object>();
        if (task.Status == TaskStatus.RanToCompletion)
          return TaskHelpers.FromResult<object>((object) null);
      }
      TaskCompletionSource<object> tcs = new TaskCompletionSource<object>();
      task.ContinueWith((Action<Task>) (innerTask =>
      {
        if (innerTask.IsFaulted)
          tcs.SetException((IEnumerable<Exception>) innerTask.Exception.InnerExceptions);
        else if (innerTask.IsCanceled)
          tcs.SetCanceled();
        else
          tcs.SetResult((object) null);
      }), TaskContinuationOptions.ExecuteSynchronously);
      return tcs.Task;
    }

    internal static Task<object> CastToObject<T>(this Task<T> task)
    {
      if (task.IsCompleted)
      {
        if (task.IsFaulted)
          return TaskHelpers.FromErrors<object>((IEnumerable<Exception>) task.Exception.InnerExceptions);
        if (task.IsCanceled)
          return TaskHelpers.Canceled<object>();
        if (task.Status == TaskStatus.RanToCompletion)
          return TaskHelpers.FromResult<object>((object) task.Result);
      }
      TaskCompletionSource<object> tcs = new TaskCompletionSource<object>();
      task.ContinueWith((Action<Task<T>>) (innerTask =>
      {
        if (innerTask.IsFaulted)
          tcs.SetException((IEnumerable<Exception>) innerTask.Exception.InnerExceptions);
        else if (innerTask.IsCanceled)
          tcs.SetCanceled();
        else
          tcs.SetResult((object) innerTask.Result);
      }), TaskContinuationOptions.ExecuteSynchronously);
      return tcs.Task;
    }

    internal static Task<TOuterResult> CastFromObject<TOuterResult>(this Task<object> task)
    {
      if (task.IsCompleted)
      {
        if (task.IsFaulted)
          return TaskHelpers.FromErrors<TOuterResult>((IEnumerable<Exception>) task.Exception.InnerExceptions);
        if (task.IsCanceled)
          return TaskHelpers.Canceled<TOuterResult>();
        if (task.Status == TaskStatus.RanToCompletion)
        {
          try
          {
            return TaskHelpers.FromResult<TOuterResult>((TOuterResult) task.Result);
          }
          catch (Exception ex)
          {
            return TaskHelpers.FromError<TOuterResult>(ex);
          }
        }
      }
      TaskCompletionSource<TOuterResult> tcs = new TaskCompletionSource<TOuterResult>();
      task.ContinueWith((Action<Task<object>>) (innerTask =>
      {
        if (innerTask.IsFaulted)
          tcs.SetException((IEnumerable<Exception>) innerTask.Exception.InnerExceptions);
        else if (innerTask.IsCanceled)
        {
          tcs.SetCanceled();
        }
        else
        {
          try
          {
            tcs.SetResult((TOuterResult) innerTask.Result);
          }
          catch (Exception ex)
          {
            tcs.SetException(ex);
          }
        }
      }), TaskContinuationOptions.ExecuteSynchronously);
      return tcs.Task;
    }

    internal static Task FastUnwrap(this Task<Task> task)
    {
      return (task.Status == TaskStatus.RanToCompletion ? task.Result : (Task) null) ?? task.Unwrap();
    }

    internal static Task<TResult> FastUnwrap<TResult>(this Task<Task<TResult>> task)
    {
      return (task.Status == TaskStatus.RanToCompletion ? task.Result : (Task<TResult>) null) ?? task.Unwrap<TResult>();
    }

    internal static Task Finally(this Task task, Action continuation, bool runSynchronously = false)
    {
      if (!task.IsCompleted)
        return (Task) TaskHelpersExtensions.FinallyImplContinuation<TaskHelpersExtensions.AsyncVoid>(task, continuation, runSynchronously);
      try
      {
        continuation();
        return task;
      }
      catch (Exception ex)
      {
        task.MarkExceptionsObserved();
        return TaskHelpers.FromError(ex);
      }
    }

    internal static Task<TResult> Finally<TResult>(
      this Task<TResult> task,
      Action continuation,
      bool runSynchronously = false)
    {
      if (!task.IsCompleted)
        return TaskHelpersExtensions.FinallyImplContinuation<TResult>((Task) task, continuation, runSynchronously);
      try
      {
        continuation();
        return task;
      }
      catch (Exception ex)
      {
        task.MarkExceptionsObserved();
        return TaskHelpers.FromError<TResult>(ex);
      }
    }

    private static Task<TResult> FinallyImplContinuation<TResult>(
      Task task,
      Action continuation,
      bool runSynchronously = false)
    {
      SynchronizationContext syncContext = SynchronizationContext.Current;
      TaskCompletionSource<TResult> tcs = new TaskCompletionSource<TResult>();
      task.ContinueWith((Action<Task>) (innerTask =>
      {
        try
        {
          if (syncContext != null)
          {
            syncContext.Post((SendOrPostCallback) (state =>
            {
              try
              {
                continuation();
                tcs.TrySetFromTask<TResult>(innerTask);
              }
              catch (Exception ex)
              {
                innerTask.MarkExceptionsObserved();
                tcs.SetException(ex);
              }
            }), (object) null);
          }
          else
          {
            continuation();
            tcs.TrySetFromTask<TResult>(innerTask);
          }
        }
        catch (Exception ex)
        {
          innerTask.MarkExceptionsObserved();
          tcs.TrySetException(ex);
        }
      }), runSynchronously ? TaskContinuationOptions.ExecuteSynchronously : TaskContinuationOptions.None);
      return tcs.Task;
    }

    private static Action<Task> GetRethrowWithNoStackLossDelegate()
    {
      MethodInfo method = typeof (Task).GetMethod("GetAwaiter", Type.EmptyTypes);
      if (method != (MethodInfo) null)
        return ((Expression<Action<Task>>) (task => Expression.Call(Expression.Call(task, method), "GetResult", Type.EmptyTypes))).Compile();
      Func<Exception, Exception> prepForRemoting = (Func<Exception, Exception>) null;
      try
      {
        if (AppDomain.CurrentDomain.IsFullyTrusted)
        {
          Func<Exception, Exception> func = ((Expression<Func<Exception, Exception>>) (exception => Expression.Call(exception, "PrepForRemoting", Type.EmptyTypes, new Expression[0]))).Compile();
          Exception exception1 = func(new Exception());
          prepForRemoting = func;
        }
      }
      catch
      {
      }
      return (Action<Task>) (task =>
      {
        try
        {
          task.Wait();
        }
        catch (AggregateException ex)
        {
          Exception exception = ex.GetBaseException();
          if (prepForRemoting != null)
            exception = prepForRemoting(exception);
          throw exception;
        }
      });
    }

    private static void MarkExceptionsObserved(this Task task)
    {
      AggregateException exception = task.Exception;
    }

    internal static Task Then(
      this Task task,
      Action continuation,
      CancellationToken cancellationToken = default (CancellationToken),
      bool runSynchronously = false)
    {
      return (Task) task.ThenImpl<Task, TaskHelpersExtensions.AsyncVoid>((Func<Task, Task<TaskHelpersExtensions.AsyncVoid>>) (t => TaskHelpersExtensions.ToAsyncVoidTask(continuation)), cancellationToken, runSynchronously);
    }

    internal static Task<TOuterResult> Then<TOuterResult>(
      this Task task,
      Func<TOuterResult> continuation,
      CancellationToken cancellationToken = default (CancellationToken),
      bool runSynchronously = false)
    {
      return task.ThenImpl<Task, TOuterResult>((Func<Task, Task<TOuterResult>>) (t => TaskHelpers.FromResult<TOuterResult>(continuation())), cancellationToken, runSynchronously);
    }

    internal static Task Then(
      this Task task,
      Func<Task> continuation,
      CancellationToken cancellationToken = default (CancellationToken),
      bool runSynchronously = false)
    {
      return (Task) task.Then<TaskHelpersExtensions.AsyncVoid>((Func<Task<TaskHelpersExtensions.AsyncVoid>>) (() => continuation().Then<TaskHelpersExtensions.AsyncVoid>((Func<TaskHelpersExtensions.AsyncVoid>) (() => new TaskHelpersExtensions.AsyncVoid()))), cancellationToken, runSynchronously);
    }

    internal static Task<TOuterResult> Then<TOuterResult>(
      this Task task,
      Func<Task<TOuterResult>> continuation,
      CancellationToken cancellationToken = default (CancellationToken),
      bool runSynchronously = false)
    {
      return task.ThenImpl<Task, TOuterResult>((Func<Task, Task<TOuterResult>>) (t => continuation()), cancellationToken, runSynchronously);
    }

    internal static Task Then<TInnerResult>(
      this Task<TInnerResult> task,
      Action<TInnerResult> continuation,
      CancellationToken cancellationToken = default (CancellationToken),
      bool runSynchronously = false)
    {
      return (Task) task.ThenImpl<Task<TInnerResult>, TaskHelpersExtensions.AsyncVoid>((Func<Task<TInnerResult>, Task<TaskHelpersExtensions.AsyncVoid>>) (t => TaskHelpersExtensions.ToAsyncVoidTask((Action) (() => continuation(t.Result)))), cancellationToken, runSynchronously);
    }

    internal static Task<TOuterResult> Then<TInnerResult, TOuterResult>(
      this Task<TInnerResult> task,
      Func<TInnerResult, TOuterResult> continuation,
      CancellationToken cancellationToken = default (CancellationToken),
      bool runSynchronously = false)
    {
      return task.ThenImpl<Task<TInnerResult>, TOuterResult>((Func<Task<TInnerResult>, Task<TOuterResult>>) (t => TaskHelpers.FromResult<TOuterResult>(continuation(t.Result))), cancellationToken, runSynchronously);
    }

    internal static Task Then<TInnerResult>(
      this Task<TInnerResult> task,
      Func<TInnerResult, Task> continuation,
      CancellationToken token = default (CancellationToken),
      bool runSynchronously = false)
    {
      return (Task) task.ThenImpl<Task<TInnerResult>, TaskHelpersExtensions.AsyncVoid>((Func<Task<TInnerResult>, Task<TaskHelpersExtensions.AsyncVoid>>) (t => continuation(t.Result).ToTask<TaskHelpersExtensions.AsyncVoid>()), token, runSynchronously);
    }

    internal static Task<TOuterResult> Then<TInnerResult, TOuterResult>(
      this Task<TInnerResult> task,
      Func<TInnerResult, Task<TOuterResult>> continuation,
      CancellationToken cancellationToken = default (CancellationToken),
      bool runSynchronously = false)
    {
      return task.ThenImpl<Task<TInnerResult>, TOuterResult>((Func<Task<TInnerResult>, Task<TOuterResult>>) (t => continuation(t.Result)), cancellationToken, runSynchronously);
    }

    private static Task<TOuterResult> ThenImpl<TTask, TOuterResult>(
      this TTask task,
      Func<TTask, Task<TOuterResult>> continuation,
      CancellationToken cancellationToken,
      bool runSynchronously)
      where TTask : Task
    {
      if (task.IsCompleted)
      {
        if (task.IsFaulted)
          return TaskHelpers.FromErrors<TOuterResult>((IEnumerable<Exception>) task.Exception.InnerExceptions);
        if (task.IsCanceled || cancellationToken.IsCancellationRequested)
          return TaskHelpers.Canceled<TOuterResult>();
        if (task.Status == TaskStatus.RanToCompletion)
        {
          try
          {
            return continuation(task);
          }
          catch (Exception ex)
          {
            return TaskHelpers.FromError<TOuterResult>(ex);
          }
        }
      }
      return TaskHelpersExtensions.ThenImplContinuation<TOuterResult, TTask>(task, continuation, cancellationToken, runSynchronously);
    }

    private static Task<TOuterResult> ThenImplContinuation<TOuterResult, TTask>(
      TTask task,
      Func<TTask, Task<TOuterResult>> continuation,
      CancellationToken cancellationToken,
      bool runSynchronously = false)
      where TTask : Task
    {
      SynchronizationContext syncContext = SynchronizationContext.Current;
      TaskCompletionSource<Task<TOuterResult>> tcs = new TaskCompletionSource<Task<TOuterResult>>();
      task.ContinueWith((Action<Task>) (innerTask =>
      {
        if (innerTask.IsFaulted)
          tcs.TrySetException((IEnumerable<Exception>) innerTask.Exception.InnerExceptions);
        else if (innerTask.IsCanceled || cancellationToken.IsCancellationRequested)
          tcs.TrySetCanceled();
        else if (syncContext != null)
          syncContext.Post((SendOrPostCallback) (state =>
          {
            try
            {
              tcs.TrySetResult(continuation(task));
            }
            catch (Exception ex)
            {
              tcs.TrySetException(ex);
            }
          }), (object) null);
        else
          tcs.TrySetResult(continuation(task));
      }), runSynchronously ? TaskContinuationOptions.ExecuteSynchronously : TaskContinuationOptions.None);
      return tcs.Task.FastUnwrap<TOuterResult>();
    }

    internal static void ThrowIfFaulted(this Task task)
    {
      TaskHelpersExtensions._rethrowWithNoStackLossDelegate(task);
    }

    private static Task<TaskHelpersExtensions.AsyncVoid> ToAsyncVoidTask(Action action)
    {
      return TaskHelpers.RunSynchronously<TaskHelpersExtensions.AsyncVoid>((Func<Task<TaskHelpersExtensions.AsyncVoid>>) (() =>
      {
        action();
        return TaskHelpersExtensions._defaultCompleted;
      }));
    }

    internal static Task<TResult> ToTask<TResult>(
      this Task task,
      CancellationToken cancellationToken = default (CancellationToken),
      TResult result = null)
    {
      if (task == null)
        return (Task<TResult>) null;
      if (task.IsCompleted)
      {
        if (task.IsFaulted)
          return TaskHelpers.FromErrors<TResult>((IEnumerable<Exception>) task.Exception.InnerExceptions);
        if (task.IsCanceled || cancellationToken.IsCancellationRequested)
          return TaskHelpers.Canceled<TResult>();
        if (task.Status == TaskStatus.RanToCompletion)
          return TaskHelpers.FromResult<TResult>(result);
      }
      return TaskHelpersExtensions.ToTaskContinuation<TResult>(task, result);
    }

    private static Task<TResult> ToTaskContinuation<TResult>(Task task, TResult result)
    {
      TaskCompletionSource<TResult> tcs = new TaskCompletionSource<TResult>();
      task.ContinueWith((Action<Task>) (innerTask =>
      {
        if (task.Status == TaskStatus.RanToCompletion)
          tcs.TrySetResult(result);
        else
          tcs.TrySetFromTask<TResult>(innerTask);
      }), TaskContinuationOptions.ExecuteSynchronously);
      return tcs.Task;
    }

    internal static bool TryGetResult<TResult>(this Task<TResult> task, out TResult result)
    {
      if (task.Status == TaskStatus.RanToCompletion)
      {
        result = task.Result;
        return true;
      }
      result = default (TResult);
      return false;
    }

    [StructLayout(LayoutKind.Sequential, Size = 1)]
    private struct AsyncVoid
    {
    }
  }
}
