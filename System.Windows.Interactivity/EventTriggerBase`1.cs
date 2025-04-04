// Decompiled with JetBrains decompiler
// Type: System.Windows.Interactivity.EventTriggerBase`1
// Assembly: System.Windows.Interactivity, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 794D0242-5078-4CF1-BEBC-5ADC9BB01BDC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Windows.Interactivity.dll

#nullable disable
namespace System.Windows.Interactivity
{
  public abstract class EventTriggerBase<T> : EventTriggerBase where T : class
  {
    protected EventTriggerBase()
      : base(typeof (T))
    {
    }

    public T Source => (T) base.Source;

    internal override sealed void OnSourceChangedImpl(object oldSource, object newSource)
    {
      base.OnSourceChangedImpl(oldSource, newSource);
      this.OnSourceChanged(oldSource as T, newSource as T);
    }

    protected virtual void OnSourceChanged(T oldSource, T newSource)
    {
    }
  }
}
