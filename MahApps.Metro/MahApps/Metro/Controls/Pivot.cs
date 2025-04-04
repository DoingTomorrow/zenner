// Decompiled with JetBrains decompiler
// Type: MahApps.Metro.Controls.Pivot
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;

#nullable disable
namespace MahApps.Metro.Controls
{
  [TemplatePart(Name = "PART_Scroll", Type = typeof (ScrollViewer))]
  [TemplatePart(Name = "PART_Headers", Type = typeof (ListView))]
  [TemplatePart(Name = "PART_Mediator", Type = typeof (ScrollViewerOffsetMediator))]
  public class Pivot : ItemsControl
  {
    private ScrollViewer scroller;
    private ListView headers;
    private PivotItem selectedItem;
    private ScrollViewerOffsetMediator mediator;
    internal int internalIndex;
    public static readonly RoutedEvent SelectionChangedEvent = EventManager.RegisterRoutedEvent("SelectionChanged", RoutingStrategy.Bubble, typeof (RoutedEventHandler), typeof (Pivot));
    public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(nameof (Header), typeof (string), typeof (Pivot), new PropertyMetadata((object) null));
    public static readonly DependencyProperty HeaderTemplateProperty = DependencyProperty.Register(nameof (HeaderTemplate), typeof (DataTemplate), typeof (Pivot));
    public static readonly DependencyProperty SelectedIndexProperty = DependencyProperty.Register(nameof (SelectedIndex), typeof (int), typeof (Pivot), (PropertyMetadata) new FrameworkPropertyMetadata((object) 0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(Pivot.SelectedItemChanged)));

    public DataTemplate HeaderTemplate
    {
      get => (DataTemplate) this.GetValue(Pivot.HeaderTemplateProperty);
      set => this.SetValue(Pivot.HeaderTemplateProperty, (object) value);
    }

    public string Header
    {
      get => (string) this.GetValue(Pivot.HeaderProperty);
      set => this.SetValue(Pivot.HeaderProperty, (object) value);
    }

    public int SelectedIndex
    {
      get => (int) this.GetValue(Pivot.SelectedIndexProperty);
      set => this.SetValue(Pivot.SelectedIndexProperty, (object) value);
    }

    public event RoutedEventHandler SelectionChanged
    {
      add => this.AddHandler(Pivot.SelectionChangedEvent, (Delegate) value);
      remove => this.RemoveHandler(Pivot.SelectionChangedEvent, (Delegate) value);
    }

    public void GoToItem(PivotItem item)
    {
      if (item == null || item == this.selectedItem)
        return;
      double num = 0.0;
      for (int index = 0; index < this.Items.Count; ++index)
      {
        if (this.Items[index] == item)
        {
          this.internalIndex = index;
          break;
        }
        num += ((FrameworkElement) this.Items[index]).ActualWidth;
      }
      this.mediator.HorizontalOffset = this.scroller.HorizontalOffset;
      Storyboard resource = this.mediator.Resources[(object) "Storyboard1"] as Storyboard;
      ((DoubleKeyFrame) this.mediator.FindName("edkf")).Value = num;
      resource.Completed -= new EventHandler(this.sb_Completed);
      resource.Completed += new EventHandler(this.sb_Completed);
      resource.Begin();
      this.RaiseEvent(new RoutedEventArgs(Pivot.SelectionChangedEvent));
    }

    private void sb_Completed(object sender, EventArgs e)
    {
      this.SelectedIndex = this.internalIndex;
    }

    static Pivot()
    {
      FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (Pivot), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (Pivot)));
    }

    public override void OnApplyTemplate()
    {
      base.OnApplyTemplate();
      this.scroller = (ScrollViewer) this.GetTemplateChild("PART_Scroll");
      this.headers = (ListView) this.GetTemplateChild("PART_Headers");
      this.mediator = this.GetTemplateChild("PART_Mediator") as ScrollViewerOffsetMediator;
      if (this.scroller != null)
      {
        this.scroller.ScrollChanged += new ScrollChangedEventHandler(this.scroller_ScrollChanged);
        this.scroller.PreviewMouseWheel += new MouseWheelEventHandler(this.scroller_MouseWheel);
      }
      if (this.headers == null)
        return;
      this.headers.SelectionChanged += new SelectionChangedEventHandler(this.headers_SelectionChanged);
    }

    private void scroller_MouseWheel(object sender, MouseWheelEventArgs e)
    {
      this.scroller.ScrollToHorizontalOffset(this.scroller.HorizontalOffset + (double) -e.Delta);
    }

    private void headers_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      this.GoToItem((PivotItem) this.headers.SelectedItem);
    }

    private void scroller_ScrollChanged(object sender, ScrollChangedEventArgs e)
    {
      double num = 0.0;
      for (int index = 0; index < this.Items.Count; ++index)
      {
        PivotItem pivotItem = (PivotItem) this.Items[index];
        double actualWidth = pivotItem.ActualWidth;
        if (e.HorizontalOffset <= num + actualWidth - 1.0)
        {
          this.selectedItem = pivotItem;
          if (this.headers.SelectedItem == this.selectedItem)
            break;
          this.headers.SelectedItem = (object) this.selectedItem;
          this.internalIndex = index;
          this.SelectedIndex = index;
          this.RaiseEvent(new RoutedEventArgs(Pivot.SelectionChangedEvent));
          break;
        }
        num += actualWidth;
      }
    }

    private static void SelectedItemChanged(
      DependencyObject dependencyObject,
      DependencyPropertyChangedEventArgs e)
    {
      if (e.NewValue == e.OldValue)
        return;
      Pivot pivot = (Pivot) dependencyObject;
      int newValue = (int) e.NewValue;
      if (pivot.internalIndex == pivot.SelectedIndex || newValue < 0 || newValue >= pivot.Items.Count)
        return;
      PivotItem pivotItem = (PivotItem) pivot.Items[newValue];
      pivot.headers.SelectedItem = (object) pivotItem;
      pivot.GoToItem(pivotItem);
    }
  }
}
