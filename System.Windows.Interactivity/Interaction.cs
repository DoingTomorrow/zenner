// Decompiled with JetBrains decompiler
// Type: System.Windows.Interactivity.Interaction
// Assembly: System.Windows.Interactivity, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 794D0242-5078-4CF1-BEBC-5ADC9BB01BDC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Windows.Interactivity.dll

#nullable disable
namespace System.Windows.Interactivity
{
  public static class Interaction
  {
    private static readonly DependencyProperty TriggersProperty = DependencyProperty.RegisterAttached("ShadowTriggers", typeof (TriggerCollection), typeof (Interaction), (PropertyMetadata) new FrameworkPropertyMetadata(new PropertyChangedCallback(Interaction.OnTriggersChanged)));
    private static readonly DependencyProperty BehaviorsProperty = DependencyProperty.RegisterAttached("ShadowBehaviors", typeof (BehaviorCollection), typeof (Interaction), (PropertyMetadata) new FrameworkPropertyMetadata(new PropertyChangedCallback(Interaction.OnBehaviorsChanged)));

    internal static bool ShouldRunInDesignMode { get; set; }

    public static TriggerCollection GetTriggers(DependencyObject obj)
    {
      TriggerCollection triggers = (TriggerCollection) obj.GetValue(Interaction.TriggersProperty);
      if (triggers == null)
      {
        triggers = new TriggerCollection();
        obj.SetValue(Interaction.TriggersProperty, (object) triggers);
      }
      return triggers;
    }

    public static BehaviorCollection GetBehaviors(DependencyObject obj)
    {
      BehaviorCollection behaviors = (BehaviorCollection) obj.GetValue(Interaction.BehaviorsProperty);
      if (behaviors == null)
      {
        behaviors = new BehaviorCollection();
        obj.SetValue(Interaction.BehaviorsProperty, (object) behaviors);
      }
      return behaviors;
    }

    private static void OnBehaviorsChanged(
      DependencyObject obj,
      DependencyPropertyChangedEventArgs args)
    {
      BehaviorCollection oldValue = (BehaviorCollection) args.OldValue;
      BehaviorCollection newValue = (BehaviorCollection) args.NewValue;
      if (oldValue == newValue)
        return;
      if (oldValue != null && ((IAttachedObject) oldValue).AssociatedObject != null)
        oldValue.Detach();
      if (newValue == null || obj == null)
        return;
      if (((IAttachedObject) newValue).AssociatedObject != null)
        throw new InvalidOperationException(ExceptionStringTable.CannotHostBehaviorCollectionMultipleTimesExceptionMessage);
      newValue.Attach(obj);
    }

    private static void OnTriggersChanged(
      DependencyObject obj,
      DependencyPropertyChangedEventArgs args)
    {
      TriggerCollection oldValue = args.OldValue as TriggerCollection;
      TriggerCollection newValue = args.NewValue as TriggerCollection;
      if (oldValue == newValue)
        return;
      if (oldValue != null && ((IAttachedObject) oldValue).AssociatedObject != null)
        oldValue.Detach();
      if (newValue == null || obj == null)
        return;
      if (((IAttachedObject) newValue).AssociatedObject != null)
        throw new InvalidOperationException(ExceptionStringTable.CannotHostTriggerCollectionMultipleTimesExceptionMessage);
      newValue.Attach(obj);
    }

    internal static bool IsElementLoaded(FrameworkElement element) => element.IsLoaded;
  }
}
