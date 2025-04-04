// Decompiled with JetBrains decompiler
// Type: Fluent.ScreenTip
// Assembly: Fluent, Version=2.0.0.0, Culture=neutral, PublicKeyToken=3e436e32a8c5546f
// MVID: 92E1D420-45B3-46DC-A0AE-B5212E3C377F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Fluent.dll

using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

#nullable disable
namespace Fluent
{
  public class ScreenTip : System.Windows.Controls.ToolTip
  {
    public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(nameof (Title), typeof (string), typeof (ScreenTip), (PropertyMetadata) new UIPropertyMetadata((object) ""));
    public static readonly DependencyProperty TextProperty = DependencyProperty.Register(nameof (Text), typeof (string), typeof (ScreenTip), (PropertyMetadata) new UIPropertyMetadata((object) ""));
    public static readonly DependencyProperty DisableReasonProperty = DependencyProperty.Register(nameof (DisableReason), typeof (string), typeof (ScreenTip), (PropertyMetadata) new UIPropertyMetadata((object) ""));
    public static readonly DependencyProperty HelpTopicProperty = DependencyProperty.Register(nameof (HelpTopic), typeof (object), typeof (ScreenTip), (PropertyMetadata) new UIPropertyMetadata((PropertyChangedCallback) null));
    public static readonly DependencyProperty ImageProperty = DependencyProperty.Register(nameof (Image), typeof (ImageSource), typeof (ScreenTip), (PropertyMetadata) new UIPropertyMetadata((PropertyChangedCallback) null));
    public static readonly DependencyProperty IsRibbonAlignedProperty = DependencyProperty.Register("BelowRibbon", typeof (bool), typeof (ScreenTip), (PropertyMetadata) new UIPropertyMetadata((object) true));
    private IInputElement focusedElement;

    [SuppressMessage("Microsoft.Performance", "CA1810")]
    static ScreenTip()
    {
      FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (ScreenTip), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (ScreenTip)));
      FrameworkElement.StyleProperty.OverrideMetadata(typeof (ScreenTip), (PropertyMetadata) new FrameworkPropertyMetadata((PropertyChangedCallback) null, new CoerceValueCallback(ScreenTip.OnCoerceStyle)));
    }

    private static object OnCoerceStyle(DependencyObject d, object basevalue)
    {
      if (basevalue == null)
        basevalue = (d as FrameworkElement).TryFindResource((object) typeof (ScreenTip));
      return basevalue;
    }

    public ScreenTip()
    {
      this.Opened += new RoutedEventHandler(this.OnToolTipOpened);
      this.Closed += new RoutedEventHandler(this.OnToolTipClosed);
      this.CustomPopupPlacementCallback = new CustomPopupPlacementCallback(this.CustomPopupPlacementMethod);
      this.Placement = PlacementMode.Custom;
    }

    private CustomPopupPlacement[] CustomPopupPlacementMethod(
      Size popupSize,
      Size targetSize,
      Point offset)
    {
      if (this.PlacementTarget == null)
        return new CustomPopupPlacement[0];
      Ribbon ribbon = (Ribbon) null;
      UIElement topLevelElement = (UIElement) null;
      this.FindControls(this.PlacementTarget, ref ribbon, ref topLevelElement);
      bool flag1 = !ScreenTip.IsQuickAccessItem(this.PlacementTarget);
      bool flag2 = !ScreenTip.IsContextMenuChild(this.PlacementTarget);
      double x = this.FlowDirection == FlowDirection.RightToLeft ? -popupSize.Width : 0.0;
      UIElement decoratorChild = this.GetDecoratorChild(topLevelElement);
      if (flag1 && this.IsRibbonAligned && ribbon != null)
      {
        double y = ribbon.TranslatePoint(new Point(0.0, ribbon.ActualHeight), this.PlacementTarget).Y;
        double num = ribbon.TranslatePoint(new Point(0.0, 0.0), this.PlacementTarget).Y - popupSize.Height;
        return new CustomPopupPlacement[2]
        {
          new CustomPopupPlacement(new Point(x, y + 1.0), PopupPrimaryAxis.Horizontal),
          new CustomPopupPlacement(new Point(x, num - 1.0), PopupPrimaryAxis.Horizontal)
        };
      }
      if (flag1 && this.IsRibbonAligned && flag2 && !(topLevelElement is Window) && decoratorChild != null)
      {
        double y = decoratorChild.TranslatePoint(new Point(0.0, ((FrameworkElement) decoratorChild).ActualHeight), this.PlacementTarget).Y;
        double num = decoratorChild.TranslatePoint(new Point(0.0, 0.0), this.PlacementTarget).Y - popupSize.Height;
        return new CustomPopupPlacement[2]
        {
          new CustomPopupPlacement(new Point(x, y + 1.0), PopupPrimaryAxis.Horizontal),
          new CustomPopupPlacement(new Point(x, num - 1.0), PopupPrimaryAxis.Horizontal)
        };
      }
      return new CustomPopupPlacement[2]
      {
        new CustomPopupPlacement(new Point(x, this.PlacementTarget.RenderSize.Height + 1.0), PopupPrimaryAxis.Horizontal),
        new CustomPopupPlacement(new Point(x, -popupSize.Height - 1.0), PopupPrimaryAxis.Horizontal)
      };
    }

    private static bool IsContextMenuChild(UIElement element)
    {
      do
      {
        element = VisualTreeHelper.GetParent((DependencyObject) element) as UIElement;
      }
      while (element != null);
      return false;
    }

    private static bool IsQuickAccessItem(UIElement element)
    {
      do
      {
        UIElement parent = VisualTreeHelper.GetParent((DependencyObject) element) as UIElement;
        if (parent is QuickAccessToolBar)
          return true;
        element = parent;
      }
      while (element != null);
      return false;
    }

    private UIElement GetDecoratorChild(UIElement popupRoot)
    {
      if (popupRoot == null)
        return (UIElement) null;
      if (popupRoot is AdornerDecorator adornerDecorator)
        return adornerDecorator.Child;
      for (int childIndex = 0; childIndex < VisualTreeHelper.GetChildrenCount((DependencyObject) popupRoot); ++childIndex)
      {
        UIElement decoratorChild = this.GetDecoratorChild(VisualTreeHelper.GetChild((DependencyObject) popupRoot, childIndex) as UIElement);
        if (decoratorChild != null)
          return decoratorChild;
      }
      return (UIElement) null;
    }

    private void FindControls(UIElement obj, ref Ribbon ribbon, ref UIElement topLevelElement)
    {
      if (obj == null)
        return;
      if (obj is Ribbon ribbon1)
        ribbon = ribbon1;
      if (!(VisualTreeHelper.GetParent((DependencyObject) obj) is UIElement parent))
        topLevelElement = obj;
      else
        this.FindControls(parent, ref ribbon, ref topLevelElement);
    }

    [DisplayName("Title")]
    [Category("Screen Tip")]
    [Description("Title of the screen tip")]
    public string Title
    {
      get => (string) this.GetValue(ScreenTip.TitleProperty);
      set => this.SetValue(ScreenTip.TitleProperty, (object) value);
    }

    [Category("Screen Tip")]
    [Description("Main text of the screen tip")]
    [DisplayName("Text")]
    public string Text
    {
      get => (string) this.GetValue(ScreenTip.TextProperty);
      set => this.SetValue(ScreenTip.TextProperty, (object) value);
    }

    [Description("Describe here what would cause disable of the control")]
    [DisplayName("Disable Reason")]
    [Category("Screen Tip")]
    public string DisableReason
    {
      get => (string) this.GetValue(ScreenTip.DisableReasonProperty);
      set => this.SetValue(ScreenTip.DisableReasonProperty, (object) value);
    }

    [Description("Help topic (it will be used to execute help)")]
    [Category("Screen Tip")]
    [DisplayName("Help Topic")]
    public object HelpTopic
    {
      get => this.GetValue(ScreenTip.HelpTopicProperty);
      set => this.SetValue(ScreenTip.HelpTopicProperty, value);
    }

    [Category("Screen Tip")]
    [Description("Image of the screen tip")]
    [DisplayName("Image")]
    public ImageSource Image
    {
      get => (ImageSource) this.GetValue(ScreenTip.ImageProperty);
      set => this.SetValue(ScreenTip.ImageProperty, (object) value);
    }

    public static event EventHandler<ScreenTipHelpEventArgs> HelpPressed;

    public bool IsRibbonAligned
    {
      get => (bool) this.GetValue(ScreenTip.IsRibbonAlignedProperty);
      set => this.SetValue(ScreenTip.IsRibbonAlignedProperty, (object) value);
    }

    private void OnToolTipClosed(object sender, RoutedEventArgs e)
    {
      if (this.focusedElement == null)
        return;
      this.focusedElement.PreviewKeyDown -= new KeyEventHandler(this.OnFocusedElementPreviewKeyDown);
      this.focusedElement = (IInputElement) null;
    }

    private void OnToolTipOpened(object sender, RoutedEventArgs e)
    {
      if (this.HelpTopic == null)
        return;
      this.focusedElement = Keyboard.FocusedElement;
      if (this.focusedElement == null)
        return;
      this.focusedElement.PreviewKeyDown += new KeyEventHandler(this.OnFocusedElementPreviewKeyDown);
    }

    private void OnFocusedElementPreviewKeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key != Key.F1)
        return;
      e.Handled = true;
      if (ScreenTip.HelpPressed == null)
        return;
      ScreenTip.HelpPressed((object) null, new ScreenTipHelpEventArgs(this.HelpTopic));
    }
  }
}
