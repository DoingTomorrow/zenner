// Decompiled with JetBrains decompiler
// Type: System.Threading.Tasks.TaskHelpers
// Assembly: System.Web.Mvc, Version=4.0.0.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 061F900E-706B-4992-B3F0-1F167FDE79D8
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Web.Mvc.dll

using System.Collections.Generic;
using System.Runtime.InteropServices;

#nullable disable
namespace System.Threading.Tasks
{
  internal static class TaskHelpers
  {
    private static readonly Task _defaultCompleted = (Task) TaskHelpers.FromResult<TaskHelpers.AsyncVoid>(new TaskHelpers.AsyncVoid());
    private static readonly Task<object> _completedTaskReturningNull = TaskHelpers.FromResult<object>((object) null);

    internal static Task Canceled()
    {
      return (Task) TaskHelpers.CancelCache<TaskHelpers.AsyncVoid>.Canceled;
    }

    internal static Task<TResult> Canceled<TResult>() => TaskHelpers.CancelCache<TResult>.Canceled;

    internal static Task Completed() => TaskHelpers._defaultCompleted;

    internal static Task FromError(Exception exception)
    {
      return (Task) TaskHelpers.FromError<TaskHelpers.AsyncVoid>(exception);
    }

    internal static Task<TResult> FromError<TResult>(Exception exception)
    {
      TaskCompletionSource<TResult> completionSource = new TaskCompletionSource<TResult>();
      completionSource.SetException(exception);
      return completionSource.Task;
    }

    internal static Task FromErrors(IEnumerable<Exception> exceptions)
    {
      return (Task) TaskHelpers.FromErrors<TaskHelpers.AsyncVoid>(exceptions);
    }

    internal static Task<TResult> FromErrors<TResult>(IEnumerable<Exception> exceptions)
    {
      TaskCompletionSource<TResult> completionSource = new TaskCompletionSource<TResult>();
      completionSource.SetException(exceptions);
      return completionSource.Task;
    }

    internal static Task<TResult> FromResult<TResult>(TResult result)
    {
      TaskCompletionSource<TResult> completionSource = new TaskCompletionSource<TResult>();
      completionSource.SetResult(result);
      return completionSource.Task;
    }

    internal static Task<object> NullResult() => TaskHelpers._completedTaskReturningNull;

    internal static Task Iterate(
      IEnumerable<Task> asyncIterator,
      CancellationToken cancellationToken = default (CancellationToken),
      bool disposeEnumerator = true)
    {
      try
      {
        IEnumerator<Task> enumerator = asyncIterator.GetEnumerator();
        Task task = TaskHelpers.IterateImpl(enumerator, cancellationToken);
        return !disposeEnumerator || enumerator == null ? task : task.Finally(new Action(((IDisposable) enumerator).Dispose), true);
      }
      catch (Exception ex)
      {
        return TaskHelpers.FromError(ex);
      }
    }

    internal static Task IterateImpl(
      IEnumerator<Task> enumerator,
      CancellationToken cancellationToken)
    {
      try
      {
        while (!cancellationToken.IsCancellationRequested)
        {
          if (!enumerator.MoveNext())
            return TaskHelpers.Completed();
          Task current = enumerator.Current;
          if (current.Status != TaskStatus.RanToCompletion)
            return current.IsCanceled || current.IsFaulted ? current : TaskHelpers.IterateImplIncompleteTask(enumerator, current, cancellationToken);
        }
        return TaskHelpers.Canceled();
      }
      catch (Exception ex)
      {
        return TaskHelpers.FromError(ex);
      }
    }

    internal static Task IterateImplIncompleteTask(
      IEnumerator<Task> enumerator,
      Task currentTask,
      CancellationToken cancellationToken)
    {
      return currentTask.Then((Func<Task>) (() => TaskHelpers.IterateImpl(enumerator, cancellationToken)));
    }

    public static Task RunSynchronously(Action action, CancellationToken token = default (CancellationToken))
    {
      if (token.IsCancellationRequested)
        return TaskHelpers.Canceled();
      try
      {
        action();
        return TaskHelpers.Completed();
      }
      catch (Exception ex)
      {
        return TaskHelpers.FromError(ex);
      }
    }

    internal static Task<TResult> RunSynchronously<TResult>(
      Func<TResult> func,
      CancellationToken cancellationToken = default (CancellationToken))
    {
      if (cancellationToken.IsCancellationRequested)
        return TaskHelpers.Canceled<TResult>();
      try
      {
        return TaskHelpers.FromResult<TResult>(func());
      }
      catch (Exception ex)
      {
        return TaskHelpers.FromError<TResult>(ex);
      }
    }

    internal static Task<TResult> RunSynchronously<TResult>(
      Func<Task<TResult>> func,
      CancellationToken cancellationToken = default (CancellationToken))
    {
      if (cancellationToken.IsCancellationRequested)
        return TaskHelpers.Canceled<TResult>();
      try
      {
        return func();
      }
      catch (Exception ex)
      {
        return TaskHelpers.FromError<TResult>(ex);
      }
    }

    internal static bool SetIfTaskFailed<TResult>(
      this TaskCompletionSource<TResult> tcs,
      Task source)
    {
      switch (source.Status)
      {
        case TaskStatus.Canceled:
        case TaskStatus.Faulted:
          return tcs.TrySetFromTask<TResult>(source);
        default:
          return false;
      }
    }

    internal static bool TrySetFromTask<TResult>(
      this TaskCompletionSource<TResult> tcs,
      Task source)
    {
      if (source.Status == TaskStatus.Canceled)
        return tcs.TrySetCanceled();
      if (source.Status == TaskStatus.Faulted)
        return tcs.TrySetException((IEnumerable<Exception>) source.Exception.InnerExceptions);
      if (source.Status != TaskStatus.RanToCompletion)
        return false;
      Task<TResult> task = source as Task<TResult>;
      return tcs.TrySetResult(task == null ? default (TResult) : task.Result);
    }

    internal static bool TrySetFromTask<TResult>(
      this TaskCompletionSource<Task<TResult>> tcs,
      Task source)
    {
      if (source.Status == TaskStatus.Canceled)
        return tcs.TrySetCanceled();
      if (source.Status == TaskStatus.Faulted)
        return tcs.TrySetException((IEnumerable<Exception>) source.Exception.InnerExceptions);
      if (source.Status != TaskStatus.RanToCompletion)
        return false;
      switch (source)
      {
        case Task<Task<TResult>> task:
          return tcs.TrySetResult(task.Result);
        case Task<TResult> result:
          return tcs.TrySetResult(result);
        default:
          return tcs.TrySetResult(TaskHelpers.FromResult<TResult>(default (TResult)));
      }
    }

    [StructLayout(LayoutKind.Sequential, Size = 1)]
    private struct AsyncVoid
    {
    }

    private static class CancelCache<TResult>
    {
      public static readonly Task<TResult> Canceled = TaskHelpers.CancelCache<TResult>.GetCancelledTask();

      private static Task<TResult> GetCancelledTask()
      {
        TaskCompletionSource<TResult> completionSource = new TaskCompletionSource<TResult>();
        completionSource.SetCanceled();
        return completionSource.Task;
      }
    }
  }
}
