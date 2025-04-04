// Decompiled with JetBrains decompiler
// Type: MahApps.Metro.Behaviours.StylizedBehaviors
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using System.ComponentModel;
using System.Windows;
using System.Windows.Interactivity;

#nullable disable
namespace MahApps.Metro.Behaviours
{
  public class StylizedBehaviors
  {
    private static readonly DependencyProperty OriginalBehaviorProperty = DependencyProperty.RegisterAttached("OriginalBehaviorInternal", typeof (Behavior), typeof (StylizedBehaviors), (PropertyMetadata) new UIPropertyMetadata((PropertyChangedCallback) null));
    public static readonly DependencyProperty BehaviorsProperty = DependencyProperty.RegisterAttached("Behaviors", typeof (StylizedBehaviorCollection), typeof (StylizedBehaviors), (PropertyMetadata) new FrameworkPropertyMetadata((object) null, new PropertyChangedCallback(StylizedBehaviors.OnPropertyChanged)));

    [Category("MahApps.Metro")]
    public static StylizedBehaviorCollection GetBehaviors(DependencyObject uie)
    {
      return (StylizedBehaviorCollection) uie.GetValue(StylizedBehaviors.BehaviorsProperty);
    }

    public static void SetBehaviors(DependencyObject uie, StylizedBehaviorCollection value)
    {
      uie.SetValue(StylizedBehaviors.BehaviorsProperty, (object) value);
    }

    private static Behavior GetOriginalBehavior(DependencyObject obj)
    {
      return obj.GetValue(StylizedBehaviors.OriginalBehaviorProperty) as Behavior;
    }

    private static int GetIndexOf(BehaviorCollection itemBehaviors, Behavior behavior)
    {
      int indexOf = -1;
      Behavior originalBehavior1 = StylizedBehaviors.GetOriginalBehavior((DependencyObject) behavior);
      for (int index = 0; index < itemBehaviors.Count; ++index)
      {
        Behavior itemBehavior = itemBehaviors[index];
        if (itemBehavior == behavior || itemBehavior == originalBehavior1)
        {
          indexOf = index;
          break;
        }
        Behavior originalBehavior2 = StylizedBehaviors.GetOriginalBehavior((DependencyObject) itemBehavior);
        if (originalBehavior2 == behavior || originalBehavior2 == originalBehavior1)
        {
          indexOf = index;
          break;
        }
      }
      return indexOf;
    }

    private static void OnPropertyChanged(
      DependencyObject dpo,
      DependencyPropertyChangedEventArgs e)
    {
      if (!(dpo is UIElement uiElement))
        return;
      BehaviorCollection behaviors = Interaction.GetBehaviors((DependencyObject) uiElement);
      StylizedBehaviorCollection newValue = e.NewValue as StylizedBehaviorCollection;
      StylizedBehaviorCollection oldValue = e.OldValue as StylizedBehaviorCollection;
      if (newValue == oldValue)
        return;
      if (oldValue != null)
      {
        foreach (Behavior behavior in (FreezableCollection<Behavior>) oldValue)
        {
          int indexOf = StylizedBehaviors.GetIndexOf(behaviors, behavior);
          if (indexOf >= 0)
            behaviors.RemoveAt(indexOf);
        }
      }
      if (newValue == null)
        return;
      foreach (Behavior behavior1 in (FreezableCollection<Behavior>) newValue)
      {
        if (StylizedBehaviors.GetIndexOf(behaviors, behavior1) < 0)
        {
          Behavior behavior2 = (Behavior) behavior1.Clone();
          StylizedBehaviors.SetOriginalBehavior((DependencyObject) behavior2, behavior1);
          behaviors.Add(behavior2);
        }
      }
    }

    private static void SetOriginalBehavior(DependencyObject obj, Behavior value)
    {
      obj.SetValue(StylizedBehaviors.OriginalBehaviorProperty, (object) value);
    }
  }
}
