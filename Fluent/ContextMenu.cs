// Decompiled with JetBrains decompiler
// Type: Fluent.ContextMenu
// Assembly: Fluent, Version=2.0.0.0, Culture=neutral, PublicKeyToken=3e436e32a8c5546f
// MVID: 92E1D420-45B3-46DC-A0AE-B5212E3C377F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Fluent.dll

using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls.Primitives;

#nullable disable
namespace Fluent
{
  public class ContextMenu : System.Windows.Controls.ContextMenu
  {
    private Thumb resizeBothThumb;
    private Thumb resizeVerticalThumb;
    public static readonly DependencyProperty ResizeModeProperty = DependencyProperty.Register(nameof (ResizeMode), typeof (ContextMenuResizeMode), typeof (ContextMenu), (PropertyMetadata) new UIPropertyMetadata((object) ContextMenuResizeMode.None));

    public ContextMenuResizeMode ResizeMode
    {
      get => (ContextMenuResizeMode) this.GetValue(ContextMenu.ResizeModeProperty);
      set => this.SetValue(ContextMenu.ResizeModeProperty, (object) value);
    }

    [SuppressMessage("Microsoft.Performance", "CA1810")]
    static ContextMenu()
    {
      FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (ContextMenu), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (ContextMenu)));
      FrameworkElement.FocusVisualStyleProperty.OverrideMetadata(typeof (ContextMenu), (PropertyMetadata) new FrameworkPropertyMetadata((PropertyChangedCallback) null));
    }

    public override void OnApplyTemplate()
    {
      if (this.resizeVerticalThumb != null)
        this.resizeVerticalThumb.DragDelta -= new DragDeltaEventHandler(this.OnResizeVerticalDelta);
      this.resizeVerticalThumb = this.GetTemplateChild("PART_ResizeVerticalThumb") as Thumb;
      if (this.resizeVerticalThumb != null)
        this.resizeVerticalThumb.DragDelta += new DragDeltaEventHandler(this.OnResizeVerticalDelta);
      if (this.resizeBothThumb != null)
        this.resizeBothThumb.DragDelta -= new DragDeltaEventHandler(this.OnResizeBothDelta);
      this.resizeBothThumb = this.GetTemplateChild("PART_ResizeBothThumb") as Thumb;
      if (this.resizeBothThumb == null)
        return;
      this.resizeBothThumb.DragDelta += new DragDeltaEventHandler(this.OnResizeBothDelta);
    }

    protected override DependencyObject GetContainerForItemOverride()
    {
      return (DependencyObject) new MenuItem();
    }

    protected override bool IsItemItsOwnContainerOverride(object item) => item is FrameworkElement;

    private void OnResizeBothDelta(object sender, DragDeltaEventArgs e)
    {
      if (double.IsNaN(this.Width))
        this.Width = this.ActualWidth;
      if (double.IsNaN(this.Height))
        this.Height = this.ActualHeight;
      this.Width = Math.Max(this.MinWidth, this.Width + e.HorizontalChange);
      this.Height = Math.Max(this.MinHeight, this.Height + e.VerticalChange);
    }

    private void OnResizeVerticalDelta(object sender, DragDeltaEventArgs e)
    {
      if (double.IsNaN(this.Height))
        this.Height = this.ActualHeight;
      this.Height = Math.Max(this.MinHeight, this.Height + e.VerticalChange);
    }
  }
}
