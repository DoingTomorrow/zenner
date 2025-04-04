// Decompiled with JetBrains decompiler
// Type: MahApps.Metro.Controls.FlyoutsControl
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

#nullable disable
namespace MahApps.Metro.Controls
{
  [StyleTypedProperty(Property = "ItemContainerStyle", StyleTargetType = typeof (Flyout))]
  public class FlyoutsControl : ItemsControl
  {
    public static readonly DependencyProperty OverrideExternalCloseButtonProperty = DependencyProperty.Register(nameof (OverrideExternalCloseButton), typeof (MouseButton?), typeof (FlyoutsControl), new PropertyMetadata((PropertyChangedCallback) null));
    public static readonly DependencyProperty OverrideIsPinnedProperty = DependencyProperty.Register(nameof (OverrideIsPinned), typeof (bool), typeof (FlyoutsControl), new PropertyMetadata((object) false));

    public MouseButton? OverrideExternalCloseButton
    {
      get => (MouseButton?) this.GetValue(FlyoutsControl.OverrideExternalCloseButtonProperty);
      set => this.SetValue(FlyoutsControl.OverrideExternalCloseButtonProperty, (object) value);
    }

    public bool OverrideIsPinned
    {
      get => (bool) this.GetValue(FlyoutsControl.OverrideIsPinnedProperty);
      set => this.SetValue(FlyoutsControl.OverrideIsPinnedProperty, (object) value);
    }

    static FlyoutsControl()
    {
      FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (FlyoutsControl), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (FlyoutsControl)));
    }

    protected override DependencyObject GetContainerForItemOverride()
    {
      return (DependencyObject) new Flyout();
    }

    protected override bool IsItemItsOwnContainerOverride(object item) => item is Flyout;

    protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
    {
      base.PrepareContainerForItemOverride(element, item);
      this.AttachHandlers((Flyout) element);
    }

    protected override void ClearContainerForItemOverride(DependencyObject element, object item)
    {
      ((Flyout) element).CleanUp(this);
      base.ClearContainerForItemOverride(element, item);
    }

    private void AttachHandlers(Flyout flyout)
    {
      PropertyChangeNotifier propertyChangeNotifier1 = new PropertyChangeNotifier((DependencyObject) flyout, Flyout.IsOpenProperty);
      propertyChangeNotifier1.ValueChanged += new EventHandler(this.FlyoutStatusChanged);
      flyout.IsOpenPropertyChangeNotifier = propertyChangeNotifier1;
      PropertyChangeNotifier propertyChangeNotifier2 = new PropertyChangeNotifier((DependencyObject) flyout, Flyout.ThemeProperty);
      propertyChangeNotifier2.ValueChanged += new EventHandler(this.FlyoutStatusChanged);
      flyout.ThemePropertyChangeNotifier = propertyChangeNotifier2;
    }

    private void FlyoutStatusChanged(object sender, EventArgs e)
    {
      this.HandleFlyoutStatusChange(this.GetFlyout(sender), this.TryFindParent<MetroWindow>());
    }

    internal void HandleFlyoutStatusChange(Flyout flyout, MetroWindow parentWindow)
    {
      if (flyout == null || parentWindow == null)
        return;
      this.ReorderZIndices(flyout);
      IOrderedEnumerable<Flyout> visibleFlyouts = this.GetFlyouts((IEnumerable) this.Items).Where<Flyout>((Func<Flyout, bool>) (i => i.IsOpen)).OrderBy<Flyout, int>(new Func<Flyout, int>(Panel.GetZIndex));
      parentWindow.HandleFlyoutStatusChange(flyout, (IEnumerable<Flyout>) visibleFlyouts);
    }

    private Flyout GetFlyout(object item)
    {
      return item is Flyout flyout ? flyout : (Flyout) this.ItemContainerGenerator.ContainerFromItem(item);
    }

    internal IEnumerable<Flyout> GetFlyouts() => this.GetFlyouts((IEnumerable) this.Items);

    private IEnumerable<Flyout> GetFlyouts(IEnumerable items)
    {
      return items.Cast<object>().Select<object, Flyout>((Func<object, Flyout>) (item => this.GetFlyout(item)));
    }

    private void ReorderZIndices(Flyout lastChanged)
    {
      IOrderedEnumerable<Flyout> orderedEnumerable = this.GetFlyouts((IEnumerable) this.Items).Where<Flyout>((Func<Flyout, bool>) (i => i.IsOpen && i != lastChanged)).OrderBy<Flyout, int>(new Func<Flyout, int>(Panel.GetZIndex));
      int num = 0;
      foreach (UIElement element in (IEnumerable<Flyout>) orderedEnumerable)
      {
        Panel.SetZIndex(element, num);
        ++num;
      }
      if (!lastChanged.IsOpen)
        return;
      Panel.SetZIndex((UIElement) lastChanged, num);
    }
  }
}
