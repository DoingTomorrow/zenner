// Decompiled with JetBrains decompiler
// Type: System.Windows.Interactivity.TriggerBase`1
// Assembly: System.Windows.Interactivity, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 794D0242-5078-4CF1-BEBC-5ADC9BB01BDC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Windows.Interactivity.dll

#nullable disable
namespace System.Windows.Interactivity
{
  public abstract class TriggerBase<T> : TriggerBase where T : DependencyObject
  {
    protected TriggerBase()
      : base(typeof (T))
    {
    }

    protected T AssociatedObject => (T) base.AssociatedObject;

    protected override sealed Type AssociatedObjectTypeConstraint
    {
      get => base.AssociatedObjectTypeConstraint;
    }
  }
}
