// Decompiled with JetBrains decompiler
// Type: Fluent.Backstage
// Assembly: Fluent, Version=2.0.0.0, Culture=neutral, PublicKeyToken=3e436e32a8c5546f
// MVID: 92E1D420-45B3-46DC-A0AE-B5212E3C377F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Fluent.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Forms.Integration;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;

#nullable disable
namespace Fluent
{
  [System.Windows.Markup.ContentProperty("Content")]
  public class Backstage : RibbonControl
  {
    private BackstageAdorner adorner;
    public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register(nameof (IsOpen), typeof (bool), typeof (Backstage), (PropertyMetadata) new UIPropertyMetadata((object) false, new PropertyChangedCallback(Backstage.OnIsOpenChanged)));
    public static readonly DependencyProperty ContentProperty = DependencyProperty.Register(nameof (Content), typeof (UIElement), typeof (Backstage), (PropertyMetadata) new UIPropertyMetadata((object) null, new PropertyChangedCallback(Backstage.OnContentChanged)));
    private Dictionary<FrameworkElement, Visibility> collapsedElements = new Dictionary<FrameworkElement, Visibility>();
    private RibbonTabItem savedTabItem;
    private double savedMinWidth;
    private double savedMinHeight;
    private int savedWidth;
    private int savedHeight;

    public bool IsOpen
    {
      get => (bool) this.GetValue(Backstage.IsOpenProperty);
      set => this.SetValue(Backstage.IsOpenProperty, (object) value);
    }

    private static void OnIsOpenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      Backstage backstage = (Backstage) d;
      if ((bool) e.NewValue)
        backstage.Show();
      else
        backstage.Hide();
    }

    public UIElement Content
    {
      get => (UIElement) this.GetValue(Backstage.ContentProperty);
      set => this.SetValue(Backstage.ContentProperty, (object) value);
    }

    private static void OnContentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      Backstage backstage = (Backstage) d;
      if (e.OldValue != null)
        backstage.RemoveLogicalChild(e.OldValue);
      if (e.NewValue == null)
        return;
      backstage.AddLogicalChild(e.NewValue);
    }

    protected override IEnumerator LogicalChildren
    {
      get
      {
        ArrayList arrayList = new ArrayList();
        if (this.Content != null)
          arrayList.Add((object) this.Content);
        return arrayList.GetEnumerator();
      }
    }

    [SuppressMessage("Microsoft.Performance", "CA1810")]
    static Backstage()
    {
      FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (Backstage), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (Backstage)));
      RibbonControl.CanAddToQuickAccessToolBarProperty.OverrideMetadata(typeof (Backstage), (PropertyMetadata) new FrameworkPropertyMetadata((object) false));
      RibbonControl.HeaderProperty.OverrideMetadata(typeof (Backstage), (PropertyMetadata) new FrameworkPropertyMetadata((object) null, (PropertyChangedCallback) null, new CoerceValueCallback(Backstage.CoerceHeader)));
      KeyTip.KeysProperty.AddOwner(typeof (Backstage), (PropertyMetadata) new FrameworkPropertyMetadata((object) null, (PropertyChangedCallback) null, new CoerceValueCallback(Backstage.CoerceKeyTipKeys)));
      FrameworkElement.StyleProperty.OverrideMetadata(typeof (Backstage), (PropertyMetadata) new FrameworkPropertyMetadata((PropertyChangedCallback) null, new CoerceValueCallback(Backstage.OnCoerceStyle)));
    }

    private static object OnCoerceStyle(DependencyObject d, object basevalue)
    {
      if (basevalue == null)
        basevalue = (d as FrameworkElement).TryFindResource((object) typeof (Backstage));
      return basevalue;
    }

    private static object CoerceHeader(DependencyObject d, object basevalue)
    {
      return basevalue ?? (object) Ribbon.Localization.BackstageButtonText;
    }

    private static object CoerceKeyTipKeys(DependencyObject d, object basevalue)
    {
      return basevalue ?? (object) Ribbon.Localization.BackstageButtonKeyTip;
    }

    public Backstage()
    {
      this.CoerceValue(RibbonControl.HeaderProperty);
      this.CoerceValue(KeyTip.KeysProperty);
    }

    private void Click() => this.IsOpen = !this.IsOpen;

    private void Show()
    {
      if (!this.IsLoaded)
      {
        this.Loaded += new RoutedEventHandler(this.OnDelayedShow);
      }
      else
      {
        if (this.Content == null)
          return;
        AdornerLayer adornerLayer = Backstage.GetAdornerLayer((UIElement) this);
        if (this.adorner == null)
        {
          if (DesignerProperties.GetIsInDesignMode((DependencyObject) this))
          {
            FrameworkElement parent = (FrameworkElement) VisualTreeHelper.GetParent((DependencyObject) this);
            double y = this.TranslatePoint(new Point(0.0, this.ActualHeight), (UIElement) parent).Y;
            this.adorner = new BackstageAdorner(parent, this.Content, y);
          }
          else
          {
            Window window = Window.GetWindow((DependencyObject) this);
            if (window == null)
              return;
            FrameworkElement content = (FrameworkElement) window.Content;
            if (content == null)
              return;
            double y = this.TranslatePoint(new Point(0.0, this.ActualHeight), (UIElement) content).Y;
            this.adorner = new BackstageAdorner(content, this.Content, y);
          }
        }
        adornerLayer.Add((Adorner) this.adorner);
        Ribbon ribbon = this.FindRibbon();
        if (ribbon != null)
        {
          this.savedTabItem = ribbon.SelectedTabItem;
          if (this.savedTabItem == null && ribbon.Tabs.Count > 0)
            this.savedTabItem = ribbon.Tabs[0];
          ribbon.SelectedTabItem = (RibbonTabItem) null;
          ribbon.SelectedTabChanged += new SelectionChangedEventHandler(this.OnSelectedRibbonTabChanged);
          if (ribbon.QuickAccessToolBar != null)
            ribbon.QuickAccessToolBar.IsEnabled = false;
          if (ribbon.TitleBar != null)
            ribbon.TitleBar.IsEnabled = false;
        }
        Window window1 = Window.GetWindow((DependencyObject) this);
        if (window1 != null)
        {
          window1.PreviewKeyDown += new KeyEventHandler(this.OnBackstageEscapeKeyDown);
          this.savedMinWidth = window1.MinWidth;
          this.savedMinHeight = window1.MinHeight;
          this.SaveWindowSize(window1);
          if (this.savedMinWidth < 500.0)
            window1.MinWidth = 500.0;
          if (this.savedMinHeight < 400.0)
            window1.MinHeight = 400.0;
          window1.SizeChanged += new SizeChangedEventHandler(this.OnWindowSizeChanged);
          this.CollapseWindowsFormsHosts((DependencyObject) window1);
        }
        this.Content?.Focus();
      }
    }

    private void OnDelayedShow(object sender, EventArgs args)
    {
      this.Loaded -= new RoutedEventHandler(this.OnDelayedShow);
      this.Show();
    }

    private void Hide()
    {
      this.Loaded -= new RoutedEventHandler(this.OnDelayedShow);
      if (this.Content == null || !this.IsLoaded || this.adorner == null)
        return;
      Backstage.GetAdornerLayer((UIElement) this).Remove((Adorner) this.adorner);
      Ribbon ribbon = this.FindRibbon();
      if (ribbon != null)
      {
        ribbon.SelectedTabChanged -= new SelectionChangedEventHandler(this.OnSelectedRibbonTabChanged);
        ribbon.SelectedTabItem = this.savedTabItem;
        if (ribbon.QuickAccessToolBar != null)
          ribbon.QuickAccessToolBar.IsEnabled = true;
        if (ribbon.TitleBar != null)
          ribbon.TitleBar.IsEnabled = true;
      }
      Window window = Window.GetWindow((DependencyObject) this);
      if (window != null)
      {
        window.PreviewKeyDown -= new KeyEventHandler(this.OnBackstageEscapeKeyDown);
        window.SizeChanged -= new SizeChangedEventHandler(this.OnWindowSizeChanged);
        window.MinWidth = this.savedMinWidth;
        window.MinHeight = this.savedMinHeight;
        NativeMethods.SetWindowPos(new WindowInteropHelper(window).Handle, new IntPtr(-2), 0, 0, this.savedWidth, this.savedHeight, 2);
      }
      foreach (KeyValuePair<FrameworkElement, Visibility> collapsedElement in this.collapsedElements)
        collapsedElement.Key.Visibility = collapsedElement.Value;
      this.collapsedElements.Clear();
      if (ribbon == null)
        return;
      ribbon.SelectedTabItem = this.savedTabItem;
    }

    private Ribbon FindRibbon()
    {
      DependencyObject reference = (DependencyObject) this;
      while (true)
      {
        switch (reference)
        {
          case null:
          case Ribbon _:
            goto label_3;
          default:
            reference = VisualTreeHelper.GetParent(reference);
            continue;
        }
      }
label_3:
      return (Ribbon) reference;
    }

    private void SaveWindowSize(Window wnd)
    {
      NativeMethods.WINDOWINFO pwi = new NativeMethods.WINDOWINFO();
      pwi.cbSize = (uint) Marshal.SizeOf((object) pwi);
      NativeMethods.GetWindowInfo(new WindowInteropHelper(wnd).Handle, ref pwi);
      this.savedWidth = pwi.rcWindow.Right - pwi.rcWindow.Left;
      this.savedHeight = pwi.rcWindow.Bottom - pwi.rcWindow.Top;
    }

    private void OnWindowSizeChanged(object sender, SizeChangedEventArgs e)
    {
      this.SaveWindowSize(Window.GetWindow((DependencyObject) this));
    }

    private void OnSelectedRibbonTabChanged(object sender, EventArgs e)
    {
      Ribbon ribbon = this.FindRibbon();
      if (ribbon != null)
        this.savedTabItem = ribbon.SelectedTabItem;
      this.IsOpen = false;
    }

    private void CollapseWindowsFormsHosts(DependencyObject parent)
    {
      if (parent == null)
        return;
      if (parent is FrameworkElement key && (parent is WindowsFormsHost || parent is WebBrowser) && key.Visibility != Visibility.Collapsed)
      {
        this.collapsedElements.Add(key, key.Visibility);
        key.Visibility = Visibility.Collapsed;
      }
      else
      {
        for (int childIndex = 0; childIndex < VisualTreeHelper.GetChildrenCount(parent); ++childIndex)
          this.CollapseWindowsFormsHosts(VisualTreeHelper.GetChild(parent, childIndex));
      }
    }

    private void OnBackstageEscapeKeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key != Key.Escape)
        return;
      this.IsOpen = false;
    }

    private static AdornerLayer GetAdornerLayer(UIElement element)
    {
      UIElement reference = element;
      do
      {
        reference = (UIElement) VisualTreeHelper.GetParent((DependencyObject) reference);
      }
      while (!(reference is AdornerDecorator));
      return AdornerLayer.GetAdornerLayer((Visual) VisualTreeHelper.GetChild((DependencyObject) reference, 0));
    }

    protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e) => this.Click();

    public override void OnKeyTipPressed()
    {
      this.Click();
      base.OnKeyTipPressed();
    }

    public override void OnApplyTemplate()
    {
      base.OnApplyTemplate();
      if (this.adorner == null)
        return;
      if (this.IsOpen)
      {
        this.Hide();
        this.IsOpen = false;
        this.adorner.Clear();
        this.adorner = (BackstageAdorner) null;
      }
      else
      {
        this.adorner.Clear();
        this.adorner = (BackstageAdorner) null;
      }
    }

    public override FrameworkElement CreateQuickAccessItem() => throw new NotImplementedException();
  }
}
