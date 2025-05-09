﻿// Decompiled with JetBrains decompiler
// Type: System.Reactive.Notification`1
// Assembly: System.Reactive.Core, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: 688278AD-98CB-433C-8F7D-49B6FA93B621
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Core.dll

using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Reactive.Concurrency;

#nullable disable
namespace System.Reactive
{
  [Serializable]
  public abstract class Notification<T> : IEquatable<Notification<T>>
  {
    protected internal Notification()
    {
    }

    public abstract T Value { get; }

    public abstract bool HasValue { get; }

    public abstract Exception Exception { get; }

    public abstract NotificationKind Kind { get; }

    public abstract bool Equals(Notification<T> other);

    public static bool operator ==(Notification<T> left, Notification<T> right)
    {
      if ((object) left == (object) right)
        return true;
      return (object) left != null && (object) right != null && left.Equals(right);
    }

    public static bool operator !=(Notification<T> left, Notification<T> right) => !(left == right);

    public override bool Equals(object obj) => this.Equals(obj as Notification<T>);

    public abstract void Accept(IObserver<T> observer);

    public abstract TResult Accept<TResult>(IObserver<T, TResult> observer);

    public abstract void Accept(Action<T> onNext, Action<Exception> onError, Action onCompleted);

    public abstract TResult Accept<TResult>(
      Func<T, TResult> onNext,
      Func<Exception, TResult> onError,
      Func<TResult> onCompleted);

    public IObservable<T> ToObservable()
    {
      return this.ToObservable((IScheduler) ImmediateScheduler.Instance);
    }

    public IObservable<T> ToObservable(IScheduler scheduler)
    {
      if (scheduler == null)
        throw new ArgumentNullException(nameof (scheduler));
      return (IObservable<T>) new AnonymousObservable<T>((Func<IObserver<T>, IDisposable>) (observer => scheduler.Schedule((Action) (() =>
      {
        this.Accept(observer);
        if (this.Kind != NotificationKind.OnNext)
          return;
        observer.OnCompleted();
      }))));
    }

    [DebuggerDisplay("OnNext({Value})")]
    [Serializable]
    internal sealed class OnNextNotification : Notification<T>
    {
      private T value;

      public OnNextNotification(T value) => this.value = value;

      public override T Value => this.value;

      public override Exception Exception => (Exception) null;

      public override bool HasValue => true;

      public override NotificationKind Kind => NotificationKind.OnNext;

      public override int GetHashCode() => EqualityComparer<T>.Default.GetHashCode(this.Value);

      public override bool Equals(Notification<T> other)
      {
        if ((object) this == (object) other)
          return true;
        return (object) other != null && other.Kind == NotificationKind.OnNext && EqualityComparer<T>.Default.Equals(this.Value, other.Value);
      }

      public override string ToString()
      {
        return string.Format((IFormatProvider) CultureInfo.CurrentCulture, "OnNext({0})", new object[1]
        {
          (object) this.Value
        });
      }

      public override void Accept(IObserver<T> observer)
      {
        if (observer == null)
          throw new ArgumentNullException(nameof (observer));
        observer.OnNext(this.Value);
      }

      public override TResult Accept<TResult>(IObserver<T, TResult> observer)
      {
        return observer != null ? observer.OnNext(this.Value) : throw new ArgumentNullException(nameof (observer));
      }

      public override void Accept(Action<T> onNext, Action<Exception> onError, Action onCompleted)
      {
        if (onNext == null)
          throw new ArgumentNullException(nameof (onNext));
        if (onError == null)
          throw new ArgumentNullException(nameof (onError));
        if (onCompleted == null)
          throw new ArgumentNullException(nameof (onCompleted));
        onNext(this.Value);
      }

      public override TResult Accept<TResult>(
        Func<T, TResult> onNext,
        Func<Exception, TResult> onError,
        Func<TResult> onCompleted)
      {
        if (onNext == null)
          throw new ArgumentNullException(nameof (onNext));
        if (onError == null)
          throw new ArgumentNullException(nameof (onError));
        if (onCompleted == null)
          throw new ArgumentNullException(nameof (onCompleted));
        return onNext(this.Value);
      }
    }

    [DebuggerDisplay("OnError({Exception})")]
    [Serializable]
    internal sealed class OnErrorNotification : Notification<T>
    {
      private Exception exception;

      public OnErrorNotification(Exception exception) => this.exception = exception;

      public override T Value
      {
        get
        {
          this.exception.Throw();
          return default (T);
        }
      }

      public override Exception Exception => this.exception;

      public override bool HasValue => false;

      public override NotificationKind Kind => NotificationKind.OnError;

      public override int GetHashCode() => this.Exception.GetHashCode();

      public override bool Equals(Notification<T> other)
      {
        if ((object) this == (object) other)
          return true;
        return (object) other != null && other.Kind == NotificationKind.OnError && object.Equals((object) this.Exception, (object) other.Exception);
      }

      public override string ToString()
      {
        return string.Format((IFormatProvider) CultureInfo.CurrentCulture, "OnError({0})", new object[1]
        {
          (object) this.Exception.GetType().FullName
        });
      }

      public override void Accept(IObserver<T> observer)
      {
        if (observer == null)
          throw new ArgumentNullException(nameof (observer));
        observer.OnError(this.Exception);
      }

      public override TResult Accept<TResult>(IObserver<T, TResult> observer)
      {
        return observer != null ? observer.OnError(this.Exception) : throw new ArgumentNullException(nameof (observer));
      }

      public override void Accept(Action<T> onNext, Action<Exception> onError, Action onCompleted)
      {
        if (onNext == null)
          throw new ArgumentNullException(nameof (onNext));
        if (onError == null)
          throw new ArgumentNullException(nameof (onError));
        if (onCompleted == null)
          throw new ArgumentNullException(nameof (onCompleted));
        onError(this.Exception);
      }

      public override TResult Accept<TResult>(
        Func<T, TResult> onNext,
        Func<Exception, TResult> onError,
        Func<TResult> onCompleted)
      {
        if (onNext == null)
          throw new ArgumentNullException(nameof (onNext));
        if (onError == null)
          throw new ArgumentNullException(nameof (onError));
        if (onCompleted == null)
          throw new ArgumentNullException(nameof (onCompleted));
        return onError(this.Exception);
      }
    }

    [DebuggerDisplay("OnCompleted()")]
    [Serializable]
    internal sealed class OnCompletedNotification : Notification<T>
    {
      public override T Value
      {
        get => throw new InvalidOperationException(Strings_Core.COMPLETED_NO_VALUE);
      }

      public override Exception Exception => (Exception) null;

      public override bool HasValue => false;

      public override NotificationKind Kind => NotificationKind.OnCompleted;

      public override int GetHashCode() => typeof (T).GetHashCode() ^ 8510;

      public override bool Equals(Notification<T> other)
      {
        if ((object) this == (object) other)
          return true;
        return (object) other != null && other.Kind == NotificationKind.OnCompleted;
      }

      public override string ToString() => "OnCompleted()";

      public override void Accept(IObserver<T> observer)
      {
        if (observer == null)
          throw new ArgumentNullException(nameof (observer));
        observer.OnCompleted();
      }

      public override TResult Accept<TResult>(IObserver<T, TResult> observer)
      {
        return observer != null ? observer.OnCompleted() : throw new ArgumentNullException(nameof (observer));
      }

      public override void Accept(Action<T> onNext, Action<Exception> onError, Action onCompleted)
      {
        if (onNext == null)
          throw new ArgumentNullException(nameof (onNext));
        if (onError == null)
          throw new ArgumentNullException(nameof (onError));
        if (onCompleted == null)
          throw new ArgumentNullException(nameof (onCompleted));
        onCompleted();
      }

      public override TResult Accept<TResult>(
        Func<T, TResult> onNext,
        Func<Exception, TResult> onError,
        Func<TResult> onCompleted)
      {
        if (onNext == null)
          throw new ArgumentNullException(nameof (onNext));
        if (onError == null)
          throw new ArgumentNullException(nameof (onError));
        if (onCompleted == null)
          throw new ArgumentNullException(nameof (onCompleted));
        return onCompleted();
      }
    }
  }
}
