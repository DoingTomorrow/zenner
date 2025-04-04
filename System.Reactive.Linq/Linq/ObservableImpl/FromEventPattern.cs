// Decompiled with JetBrains decompiler
// Type: System.Reactive.Linq.ObservableImpl.FromEventPattern
// Assembly: System.Reactive.Linq, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: DAE9EF48-B730-438F-A95F-19E6CE1D672C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Linq.dll

using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reflection;

#nullable disable
namespace System.Reactive.Linq.ObservableImpl
{
  internal class FromEventPattern
  {
    public class Impl<TDelegate, TEventArgs> : 
      ClassicEventProducer<TDelegate, EventPattern<TEventArgs>>
    {
      private readonly Func<EventHandler<TEventArgs>, TDelegate> _conversion;

      public Impl(
        Action<TDelegate> addHandler,
        Action<TDelegate> removeHandler,
        IScheduler scheduler)
        : base(addHandler, removeHandler, scheduler)
      {
      }

      public Impl(
        Func<EventHandler<TEventArgs>, TDelegate> conversion,
        Action<TDelegate> addHandler,
        Action<TDelegate> removeHandler,
        IScheduler scheduler)
        : base(addHandler, removeHandler, scheduler)
      {
        this._conversion = conversion;
      }

      protected override TDelegate GetHandler(Action<EventPattern<TEventArgs>> onNext)
      {
        TDelegate @delegate = default (TDelegate);
        return this._conversion != null ? this._conversion((EventHandler<TEventArgs>) ((sender, eventArgs) => onNext(new EventPattern<TEventArgs>(sender, eventArgs)))) : ReflectionUtils.CreateDelegate<TDelegate>((object) (Action<object, TEventArgs>) ((sender, eventArgs) => onNext(new EventPattern<TEventArgs>(sender, eventArgs))), typeof (Action<object, TEventArgs>).GetMethod("Invoke"));
      }
    }

    public class Impl<TDelegate, TSender, TEventArgs>(
      Action<TDelegate> addHandler,
      Action<TDelegate> removeHandler,
      IScheduler scheduler) : ClassicEventProducer<TDelegate, EventPattern<TSender, TEventArgs>>(addHandler, removeHandler, scheduler)
    {
      protected override TDelegate GetHandler(Action<EventPattern<TSender, TEventArgs>> onNext)
      {
        return ReflectionUtils.CreateDelegate<TDelegate>((object) (Action<TSender, TEventArgs>) ((sender, eventArgs) => onNext(new EventPattern<TSender, TEventArgs>(sender, eventArgs))), typeof (Action<TSender, TEventArgs>).GetMethod("Invoke"));
      }
    }

    public class Handler<TSender, TEventArgs, TResult> : EventProducer<Delegate, TResult>
    {
      private readonly object _target;
      private readonly Type _delegateType;
      private readonly MethodInfo _addMethod;
      private readonly MethodInfo _removeMethod;
      private readonly Func<TSender, TEventArgs, TResult> _getResult;
      private readonly bool _isWinRT;

      public Handler(
        object target,
        Type delegateType,
        MethodInfo addMethod,
        MethodInfo removeMethod,
        Func<TSender, TEventArgs, TResult> getResult,
        bool isWinRT,
        IScheduler scheduler)
        : base(scheduler)
      {
        this._isWinRT = isWinRT;
        this._target = target;
        this._delegateType = delegateType;
        this._addMethod = addMethod;
        this._removeMethod = removeMethod;
        this._getResult = getResult;
      }

      protected override Delegate GetHandler(Action<TResult> onNext)
      {
        return ReflectionUtils.CreateDelegate(this._delegateType, (object) (Action<TSender, TEventArgs>) ((sender, eventArgs) => onNext(this._getResult(sender, eventArgs))), typeof (Action<TSender, TEventArgs>).GetMethod("Invoke"));
      }

      protected override IDisposable AddHandler(Delegate handler)
      {
        Action removeHandler = (Action) null;
        try
        {
          if (this._isWinRT)
          {
            object token = this._addMethod.Invoke(this._target, new object[1]
            {
              (object) handler
            });
            removeHandler = (Action) (() => this._removeMethod.Invoke(this._target, new object[1]
            {
              token
            }));
          }
          else
          {
            this._addMethod.Invoke(this._target, new object[1]
            {
              (object) handler
            });
            removeHandler = (Action) (() => this._removeMethod.Invoke(this._target, new object[1]
            {
              (object) handler
            }));
          }
        }
        catch (TargetInvocationException ex)
        {
          throw ex.InnerException;
        }
        return Disposable.Create((Action) (() =>
        {
          try
          {
            removeHandler();
          }
          catch (TargetInvocationException ex)
          {
            throw ex.InnerException;
          }
        }));
      }
    }
  }
}
