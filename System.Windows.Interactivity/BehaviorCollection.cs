// Decompiled with JetBrains decompiler
// Type: System.Windows.Interactivity.BehaviorCollection
// Assembly: System.Windows.Interactivity, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 794D0242-5078-4CF1-BEBC-5ADC9BB01BDC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Windows.Interactivity.dll

#nullable disable
namespace System.Windows.Interactivity
{
  public sealed class BehaviorCollection : AttachableCollection<Behavior>
  {
    internal BehaviorCollection()
    {
    }

    protected override void OnAttached()
    {
      foreach (Behavior behavior in (FreezableCollection<Behavior>) this)
        behavior.Attach(this.AssociatedObject);
    }

    protected override void OnDetaching()
    {
      foreach (Behavior behavior in (FreezableCollection<Behavior>) this)
        behavior.Detach();
    }

    internal override void ItemAdded(Behavior item)
    {
      if (this.AssociatedObject == null)
        return;
      item.Attach(this.AssociatedObject);
    }

    internal override void ItemRemoved(Behavior item)
    {
      if (((IAttachedObject) item).AssociatedObject == null)
        return;
      item.Detach();
    }

    protected override Freezable CreateInstanceCore() => (Freezable) new BehaviorCollection();
  }
}
