// Decompiled with JetBrains decompiler
// Type: System.Reactive.Concurrency.ScheduledItem`1
// Assembly: System.Reactive.Core, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: 688278AD-98CB-433C-8F7D-49B6FA93B621
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Core.dll

using System.Collections.Generic;
using System.Reactive.Disposables;

#nullable disable
namespace System.Reactive.Concurrency
{
  public abstract class ScheduledItem<TAbsolute> : 
    IScheduledItem<TAbsolute>,
    IComparable<ScheduledItem<TAbsolute>>
    where TAbsolute : IComparable<TAbsolute>
  {
    private readonly SingleAssignmentDisposable _disposable = new SingleAssignmentDisposable();
    private readonly TAbsolute _dueTime;
    private readonly IComparer<TAbsolute> _comparer;

    protected ScheduledItem(TAbsolute dueTime, IComparer<TAbsolute> comparer)
    {
      if (comparer == null)
        throw new ArgumentNullException(nameof (comparer));
      this._dueTime = dueTime;
      this._comparer = comparer;
    }

    public TAbsolute DueTime => this._dueTime;

    public void Invoke()
    {
      if (this._disposable.IsDisposed)
        return;
      this._disposable.Disposable = this.InvokeCore();
    }

    protected abstract IDisposable InvokeCore();

    public int CompareTo(ScheduledItem<TAbsolute> other)
    {
      return (object) other == null ? 1 : this._comparer.Compare(this.DueTime, other.DueTime);
    }

    public static bool operator <(ScheduledItem<TAbsolute> left, ScheduledItem<TAbsolute> right)
    {
      return Comparer<ScheduledItem<TAbsolute>>.Default.Compare(left, right) < 0;
    }

    public static bool operator <=(ScheduledItem<TAbsolute> left, ScheduledItem<TAbsolute> right)
    {
      return Comparer<ScheduledItem<TAbsolute>>.Default.Compare(left, right) <= 0;
    }

    public static bool operator >(ScheduledItem<TAbsolute> left, ScheduledItem<TAbsolute> right)
    {
      return Comparer<ScheduledItem<TAbsolute>>.Default.Compare(left, right) > 0;
    }

    public static bool operator >=(ScheduledItem<TAbsolute> left, ScheduledItem<TAbsolute> right)
    {
      return Comparer<ScheduledItem<TAbsolute>>.Default.Compare(left, right) >= 0;
    }

    public static bool operator ==(ScheduledItem<TAbsolute> left, ScheduledItem<TAbsolute> right)
    {
      return (object) left == (object) right;
    }

    public static bool operator !=(ScheduledItem<TAbsolute> left, ScheduledItem<TAbsolute> right)
    {
      return !(left == right);
    }

    public override bool Equals(object obj) => (object) this == obj;

    public override int GetHashCode() => base.GetHashCode();

    public void Cancel() => this._disposable.Dispose();

    public bool IsCanceled => this._disposable.IsDisposed;
  }
}
