// Decompiled with JetBrains decompiler
// Type: System.Reactive.Threading.Tasks.TaskObservableExtensions
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Linq.ObservableImpl;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;

#nullable disable
namespace System.Reactive.Threading.Tasks
{
  public static class TaskObservableExtensions
  {
    public static IObservable<Unit> ToObservable(this Task task)
    {
      return task != null ? TaskObservableExtensions.ToObservableImpl(task, (IScheduler) null) : throw new ArgumentNullException(nameof (task));
    }

    public static IObservable<Unit> ToObservable(this Task task, IScheduler scheduler)
    {
      if (task == null)
        throw new ArgumentNullException(nameof (task));
      return scheduler != null ? TaskObservableExtensions.ToObservableImpl(task, scheduler) : throw new ArgumentNullException(nameof (scheduler));
    }

    private static IObservable<Unit> ToObservableImpl(Task task, IScheduler scheduler)
    {
      IObservable<Unit> observableImpl = (IObservable<Unit>) null;
      if (task.IsCompleted)
      {
        scheduler = scheduler ?? (IScheduler) ImmediateScheduler.Instance;
        switch (task.Status)
        {
          case TaskStatus.RanToCompletion:
            observableImpl = (IObservable<Unit>) new Return<Unit>(Unit.Default, scheduler);
            break;
          case TaskStatus.Canceled:
            observableImpl = (IObservable<Unit>) new Throw<Unit>((Exception) new TaskCanceledException(task), scheduler);
            break;
          case TaskStatus.Faulted:
            observableImpl = (IObservable<Unit>) new Throw<Unit>(task.Exception.InnerException, scheduler);
            break;
        }
      }
      else
        observableImpl = TaskObservableExtensions.ToObservableSlow(task, scheduler);
      return observableImpl;
    }

    private static IObservable<Unit> ToObservableSlow(Task task, IScheduler scheduler)
    {
      AsyncSubject<Unit> subject = new AsyncSubject<Unit>();
      TaskContinuationOptions continuationOptions = TaskObservableExtensions.GetTaskContinuationOptions(scheduler);
      task.ContinueWith((Action<Task>) (t => TaskObservableExtensions.ToObservableDone(task, (IObserver<Unit>) subject)), continuationOptions);
      return TaskObservableExtensions.ToObservableResult<Unit>(subject, scheduler);
    }

    private static void ToObservableDone(Task task, IObserver<Unit> subject)
    {
      switch (task.Status)
      {
        case TaskStatus.RanToCompletion:
          subject.OnNext(Unit.Default);
          subject.OnCompleted();
          break;
        case TaskStatus.Canceled:
          subject.OnError((Exception) new TaskCanceledException(task));
          break;
        case TaskStatus.Faulted:
          subject.OnError(task.Exception.InnerException);
          break;
      }
    }

    public static IObservable<TResult> ToObservable<TResult>(this Task<TResult> task)
    {
      return task != null ? TaskObservableExtensions.ToObservableImpl<TResult>(task, (IScheduler) null) : throw new ArgumentNullException(nameof (task));
    }

    public static IObservable<TResult> ToObservable<TResult>(
      this Task<TResult> task,
      IScheduler scheduler)
    {
      if (task == null)
        throw new ArgumentNullException(nameof (task));
      return scheduler != null ? TaskObservableExtensions.ToObservableImpl<TResult>(task, scheduler) : throw new ArgumentNullException(nameof (scheduler));
    }

    private static IObservable<TResult> ToObservableImpl<TResult>(
      Task<TResult> task,
      IScheduler scheduler)
    {
      IObservable<TResult> observableImpl = (IObservable<TResult>) null;
      if (task.IsCompleted)
      {
        scheduler = scheduler ?? (IScheduler) ImmediateScheduler.Instance;
        switch (task.Status)
        {
          case TaskStatus.RanToCompletion:
            observableImpl = (IObservable<TResult>) new Return<TResult>(task.Result, scheduler);
            break;
          case TaskStatus.Canceled:
            observableImpl = (IObservable<TResult>) new Throw<TResult>((Exception) new TaskCanceledException((Task) task), scheduler);
            break;
          case TaskStatus.Faulted:
            observableImpl = (IObservable<TResult>) new Throw<TResult>(task.Exception.InnerException, scheduler);
            break;
        }
      }
      else
        observableImpl = TaskObservableExtensions.ToObservableSlow<TResult>(task, scheduler);
      return observableImpl;
    }

    private static IObservable<TResult> ToObservableSlow<TResult>(
      Task<TResult> task,
      IScheduler scheduler)
    {
      AsyncSubject<TResult> subject = new AsyncSubject<TResult>();
      TaskContinuationOptions continuationOptions = TaskObservableExtensions.GetTaskContinuationOptions(scheduler);
      task.ContinueWith((Action<Task<TResult>>) (t => TaskObservableExtensions.ToObservableDone<TResult>(task, (IObserver<TResult>) subject)), continuationOptions);
      return TaskObservableExtensions.ToObservableResult<TResult>(subject, scheduler);
    }

    private static void ToObservableDone<TResult>(Task<TResult> task, IObserver<TResult> subject)
    {
      switch (task.Status)
      {
        case TaskStatus.RanToCompletion:
          subject.OnNext(task.Result);
          subject.OnCompleted();
          break;
        case TaskStatus.Canceled:
          subject.OnError((Exception) new TaskCanceledException((Task) task));
          break;
        case TaskStatus.Faulted:
          subject.OnError(task.Exception.InnerException);
          break;
      }
    }

    private static TaskContinuationOptions GetTaskContinuationOptions(IScheduler scheduler)
    {
      TaskContinuationOptions continuationOptions = TaskContinuationOptions.None;
      if (scheduler != null)
        continuationOptions |= TaskContinuationOptions.ExecuteSynchronously;
      return continuationOptions;
    }

    private static IObservable<TResult> ToObservableResult<TResult>(
      AsyncSubject<TResult> subject,
      IScheduler scheduler)
    {
      return scheduler != null ? subject.ObserveOn<TResult>(scheduler) : subject.AsObservable<TResult>();
    }

    public static Task<TResult> ToTask<TResult>(this IObservable<TResult> observable)
    {
      return observable != null ? observable.ToTask<TResult>(new CancellationToken(), (object) null) : throw new ArgumentNullException(nameof (observable));
    }

    public static Task<TResult> ToTask<TResult>(this IObservable<TResult> observable, object state)
    {
      return observable != null ? observable.ToTask<TResult>(new CancellationToken(), state) : throw new ArgumentNullException(nameof (observable));
    }

    public static Task<TResult> ToTask<TResult>(
      this IObservable<TResult> observable,
      CancellationToken cancellationToken)
    {
      return observable != null ? observable.ToTask<TResult>(cancellationToken, (object) null) : throw new ArgumentNullException(nameof (observable));
    }

    public static Task<TResult> ToTask<TResult>(
      this IObservable<TResult> observable,
      CancellationToken cancellationToken,
      object state)
    {
      if (observable == null)
        throw new ArgumentNullException(nameof (observable));
      bool hasValue = false;
      TResult lastValue = default (TResult);
      TaskCompletionSource<TResult> tcs = new TaskCompletionSource<TResult>(state);
      SingleAssignmentDisposable disposable = new SingleAssignmentDisposable();
      CancellationTokenRegistration ctr = new CancellationTokenRegistration();
      if (cancellationToken.CanBeCanceled)
        ctr = cancellationToken.Register((Action) (() =>
        {
          disposable.Dispose();
          tcs.TrySetCanceled<TResult>(cancellationToken);
        }));
      AnonymousObserver<TResult> anonymousObserver = new AnonymousObserver<TResult>((Action<TResult>) (value =>
      {
        hasValue = true;
        lastValue = value;
      }), (Action<Exception>) (ex =>
      {
        tcs.TrySetException(ex);
        ctr.Dispose();
        disposable.Dispose();
      }), (Action) (() =>
      {
        if (hasValue)
          tcs.TrySetResult(lastValue);
        else
          tcs.TrySetException((Exception) new InvalidOperationException(Strings_Linq.NO_ELEMENTS));
        ctr.Dispose();
        disposable.Dispose();
      }));
      try
      {
        disposable.Disposable = observable.Subscribe((IObserver<TResult>) anonymousObserver);
      }
      catch (Exception ex)
      {
        tcs.TrySetException(ex);
      }
      return tcs.Task;
    }
  }
}
