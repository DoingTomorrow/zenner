// Decompiled with JetBrains decompiler
// Type: System.Windows.Interactivity.TargetedTriggerAction`1
// Assembly: System.Windows.Interactivity, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 794D0242-5078-4CF1-BEBC-5ADC9BB01BDC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Windows.Interactivity.dll

#nullable disable
namespace System.Windows.Interactivity
{
  public abstract class TargetedTriggerAction<T> : TargetedTriggerAction where T : class
  {
    protected TargetedTriggerAction()
      : base(typeof (T))
    {
    }

    protected T Target => (T) base.Target;

    internal override sealed void OnTargetChangedImpl(object oldTarget, object newTarget)
    {
      base.OnTargetChangedImpl(oldTarget, newTarget);
      this.OnTargetChanged(oldTarget as T, newTarget as T);
    }

    protected virtual void OnTargetChanged(T oldTarget, T newTarget)
    {
    }
  }
}
