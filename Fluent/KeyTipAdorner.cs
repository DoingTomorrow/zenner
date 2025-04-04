// Decompiled with JetBrains decompiler
// Type: Fluent.KeyTipAdorner
// Assembly: Fluent, Version=2.0.0.0, Culture=neutral, PublicKeyToken=3e436e32a8c5546f
// MVID: 92E1D420-45B3-46DC-A0AE-B5212E3C377F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Fluent.dll

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Threading;

#nullable disable
namespace Fluent
{
  internal class KeyTipAdorner : Adorner
  {
    private List<KeyTip> keyTips = new List<KeyTip>();
    private List<UIElement> associatedElements = new List<UIElement>();
    private FrameworkElement oneOfAssociatedElements;
    private Point[] keyTipPositions;
    private KeyTipAdorner parentAdorner;
    private KeyTipAdorner childAdorner;
    private IInputElement focusedElement;
    private bool attached;
    private HwndSource attachedHwndSource;
    private string enteredKeys = "";
    private bool terminated;
    private DispatcherTimer timerFocusTracking;
    private AdornerLayer adornerLayer;
    private Visibility[] backupedVisibilities;

    public event EventHandler Terminated;

    public bool IsAdornerChainAlive
    {
      get
      {
        if (this.attached)
          return true;
        return this.childAdorner != null && this.childAdorner.IsAdornerChainAlive;
      }
    }

    public KeyTipAdorner(
      UIElement adornedElement,
      UIElement keyTipElementContainer,
      KeyTipAdorner parentAdorner)
      : base(adornedElement)
    {
      this.parentAdorner = parentAdorner;
      this.FindKeyTips(keyTipElementContainer, false);
      this.oneOfAssociatedElements = this.associatedElements.Count != 0 ? (FrameworkElement) this.associatedElements[0] : (FrameworkElement) adornedElement;
      this.keyTipPositions = new Point[this.keyTips.Count];
    }

    private void FindKeyTips(UIElement element, bool hide)
    {
      foreach (object child1 in LogicalTreeHelper.GetChildren((DependencyObject) element))
      {
        UIElement element1 = child1 as UIElement;
        RibbonGroupBox ribbonGroupBox = element1 as RibbonGroupBox;
        if (element1 != null)
        {
          string keys = KeyTip.GetKeys((DependencyObject) element1);
          if (keys != null)
          {
            KeyTip child2 = new KeyTip();
            child2.Content = (object) keys;
            child2.Visibility = hide ? Visibility.Collapsed : Visibility.Visible;
            this.keyTips.Add(child2);
            this.AddVisualChild((Visual) child2);
            this.associatedElements.Add(element1);
            if (ribbonGroupBox != null)
            {
              if (ribbonGroupBox.State == RibbonGroupBoxState.Collapsed)
              {
                child2.Visibility = Visibility.Visible;
                this.FindKeyTips(element1, true);
                continue;
              }
              child2.Visibility = Visibility.Collapsed;
            }
            else
            {
              child2.SetBinding(UIElement.IsEnabledProperty, (BindingBase) new Binding("IsEnabled")
              {
                Source = (object) element1,
                Mode = BindingMode.OneWay
              });
              continue;
            }
          }
          if (ribbonGroupBox != null && ribbonGroupBox.State == RibbonGroupBoxState.Collapsed)
            this.FindKeyTips(element1, true);
          else
            this.FindKeyTips(element1, hide);
        }
      }
    }

    public void Attach()
    {
      if (this.attached)
        return;
      this.Log("Attach begin");
      if (!this.oneOfAssociatedElements.IsLoaded)
      {
        this.oneOfAssociatedElements.Loaded += new RoutedEventHandler(this.OnDelayAttach);
      }
      else
      {
        this.focusedElement = Keyboard.FocusedElement;
        if (this.focusedElement != null)
        {
          this.Log("Focus Attached to " + this.focusedElement.ToString());
          this.focusedElement.LostKeyboardFocus += new KeyboardFocusChangedEventHandler(this.OnFocusLost);
          this.focusedElement.PreviewKeyDown += new KeyEventHandler(this.OnPreviewKeyDown);
          this.focusedElement.PreviewKeyUp += new KeyEventHandler(this.OnPreviewKeyUp);
        }
        else
          this.Log("[!] Focus Setup Failed");
        KeyTipAdorner.GetTopLevelElement((UIElement) this.oneOfAssociatedElements).PreviewMouseDown += new MouseButtonEventHandler(this.OnInputActionOccured);
        this.adornerLayer = KeyTipAdorner.GetAdornerLayer((UIElement) this.oneOfAssociatedElements);
        this.adornerLayer.Add((Adorner) this);
        this.enteredKeys = "";
        this.FilterKeyTips();
        this.attachedHwndSource = (HwndSource) PresentationSource.FromVisual((Visual) this.oneOfAssociatedElements);
        if (this.attachedHwndSource != null)
          this.attachedHwndSource.AddHook(new HwndSourceHook(this.WindowProc));
        if (this.timerFocusTracking == null)
        {
          this.timerFocusTracking = new DispatcherTimer(DispatcherPriority.ApplicationIdle, Dispatcher.CurrentDispatcher);
          this.timerFocusTracking.Interval = TimeSpan.FromMilliseconds(50.0);
          this.timerFocusTracking.Tick += new EventHandler(this.OnTimerFocusTrackingTick);
        }
        this.timerFocusTracking.Start();
        this.attached = true;
        this.Log("Attach end");
      }
    }

    private void OnTimerFocusTrackingTick(object sender, EventArgs e)
    {
      if (this.focusedElement == Keyboard.FocusedElement)
        return;
      this.Log("Focus is changed, but focus lost is not occured");
      if (this.focusedElement != null)
      {
        this.focusedElement.LostKeyboardFocus -= new KeyboardFocusChangedEventHandler(this.OnFocusLost);
        this.focusedElement.PreviewKeyDown -= new KeyEventHandler(this.OnPreviewKeyDown);
        this.focusedElement.PreviewKeyUp -= new KeyEventHandler(this.OnPreviewKeyUp);
      }
      this.focusedElement = Keyboard.FocusedElement;
      if (this.focusedElement == null)
        return;
      this.focusedElement.LostKeyboardFocus += new KeyboardFocusChangedEventHandler(this.OnFocusLost);
      this.focusedElement.PreviewKeyDown += new KeyEventHandler(this.OnPreviewKeyDown);
      this.focusedElement.PreviewKeyUp += new KeyEventHandler(this.OnPreviewKeyUp);
    }

    private IntPtr WindowProc(
      IntPtr hwnd,
      int msg,
      IntPtr wParam,
      IntPtr lParam,
      ref bool handled)
    {
      if (msg == 6 && wParam == IntPtr.Zero && this.attached)
      {
        this.Log("The host window is deactivated, keytips will be terminated");
        this.Terminate();
      }
      return IntPtr.Zero;
    }

    private void OnDelayAttach(object sender, EventArgs args)
    {
      this.oneOfAssociatedElements.Loaded -= new RoutedEventHandler(this.OnDelayAttach);
      this.Attach();
    }

    public void Detach()
    {
      if (this.childAdorner != null)
        this.childAdorner.Detach();
      if (!this.attached)
        return;
      this.Log("Detach Begin");
      if (this.attachedHwndSource != null && !this.attachedHwndSource.IsDisposed)
        this.AdornedElement.Dispatcher.BeginInvoke((Delegate) (() => this.attachedHwndSource.RemoveHook(new HwndSourceHook(this.WindowProc))));
      this.oneOfAssociatedElements.Loaded -= new RoutedEventHandler(this.OnDelayAttach);
      if (this.focusedElement != null)
      {
        this.focusedElement.LostKeyboardFocus -= new KeyboardFocusChangedEventHandler(this.OnFocusLost);
        this.focusedElement.PreviewKeyDown -= new KeyEventHandler(this.OnPreviewKeyDown);
        this.focusedElement.PreviewKeyUp -= new KeyEventHandler(this.OnPreviewKeyUp);
        this.focusedElement = (IInputElement) null;
      }
      KeyTipAdorner.GetTopLevelElement((UIElement) this.oneOfAssociatedElements).PreviewMouseDown -= new MouseButtonEventHandler(this.OnInputActionOccured);
      this.adornerLayer.Remove((Adorner) this);
      this.enteredKeys = "";
      this.attached = false;
      this.timerFocusTracking.Stop();
      this.Log("Detach End");
    }

    public void Terminate()
    {
      if (this.terminated)
        return;
      this.terminated = true;
      this.Detach();
      if (this.parentAdorner != null)
        this.parentAdorner.Terminate();
      if (this.childAdorner != null)
        this.childAdorner.Terminate();
      if (this.Terminated != null)
        this.Terminated((object) this, EventArgs.Empty);
      this.Log("Termination");
    }

    [SuppressMessage("Microsoft.Maintainability", "CA1502")]
    private void OnPreviewKeyDown(object sender, KeyEventArgs e)
    {
      this.Log("Key Down " + e.Key.ToString() + " (" + e.OriginalSource.ToString() + ")");
      if (e.IsRepeat || this.Visibility == Visibility.Hidden)
        return;
      if (!(this.AdornedElement is ContextMenu) && (e.Key == Key.Left || e.Key == Key.Right || e.Key == Key.Up || e.Key == Key.Down || e.Key == Key.Return || e.Key == Key.Tab))
        this.Visibility = Visibility.Hidden;
      else if (e.Key == Key.Escape)
      {
        this.Back();
      }
      else
      {
        Key key = e.Key == Key.System ? e.SystemKey : e.Key;
        char? nullable1 = KeyTranslator.KeyToChar(key, CultureInfo.InvariantCulture);
        char? nullable2 = KeyTranslator.KeyToChar(key, CultureInfo.CurrentUICulture);
        char? nullable3 = nullable1;
        if ((nullable3.HasValue ? new int?((int) nullable3.GetValueOrDefault()) : new int?()).HasValue)
        {
          e.Handled = true;
          nullable1 = new char?(char.ToUpper(nullable1.Value, CultureInfo.InvariantCulture));
          if (this.IsElementsStartWith(this.enteredKeys + (object) nullable1))
          {
            this.enteredKeys += (string) (object) nullable1;
            UIElement element = this.TryGetElement(this.enteredKeys);
            if (element != null)
            {
              this.Forward(element);
              return;
            }
            this.FilterKeyTips();
            return;
          }
        }
        char? nullable4 = nullable2;
        if (!(nullable4.HasValue ? new int?((int) nullable4.GetValueOrDefault()) : new int?()).HasValue)
          return;
        e.Handled = true;
        nullable2 = new char?(char.ToUpper(nullable2.Value, CultureInfo.CurrentUICulture));
        if (this.IsElementsStartWith(this.enteredKeys + (object) nullable2))
        {
          this.enteredKeys += (string) (object) nullable2;
          UIElement element = this.TryGetElement(this.enteredKeys);
          if (element != null)
            this.Forward(element);
          else
            this.FilterKeyTips();
        }
        else
          SystemSounds.Beep.Play();
      }
    }

    private void OnPreviewKeyUp(object sender, KeyEventArgs e)
    {
    }

    private void OnInputActionOccured(object sender, RoutedEventArgs e)
    {
      if (!this.attached)
        return;
      this.Log("Input Action, Keystips will be terminated");
      this.Terminate();
    }

    private void OnFocusLost(object sender, RoutedEventArgs e)
    {
      if (!this.attached)
        return;
      this.Log("Focus Lost");
      IInputElement focusedElement = this.focusedElement;
      this.focusedElement.LostKeyboardFocus -= new KeyboardFocusChangedEventHandler(this.OnFocusLost);
      this.focusedElement.PreviewKeyDown -= new KeyEventHandler(this.OnPreviewKeyDown);
      this.focusedElement.PreviewKeyUp -= new KeyEventHandler(this.OnPreviewKeyUp);
      this.focusedElement = Keyboard.FocusedElement;
      if (this.focusedElement != null)
      {
        this.Log("Focus Changed from " + focusedElement.ToString() + " to " + this.focusedElement.ToString());
        this.focusedElement.LostKeyboardFocus += new KeyboardFocusChangedEventHandler(this.OnFocusLost);
        this.focusedElement.PreviewKeyDown += new KeyEventHandler(this.OnPreviewKeyDown);
        this.focusedElement.PreviewKeyUp += new KeyEventHandler(this.OnPreviewKeyUp);
      }
      else
        this.Log("Focus Not Restored");
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

    private static UIElement GetTopLevelElement(UIElement element)
    {
      while (VisualTreeHelper.GetParent((DependencyObject) element) is UIElement parent)
        element = parent;
      return element;
    }

    private void Back()
    {
      if (this.parentAdorner != null)
      {
        this.Log(nameof (Back));
        this.Detach();
        this.parentAdorner.Attach();
      }
      else
        this.Terminate();
    }

    public bool Forward(string keys, bool click)
    {
      UIElement element = this.TryGetElement(keys);
      if (element == null)
        return false;
      this.Forward(element, click);
      return true;
    }

    private void Forward(UIElement element) => this.Forward(element, true);

    private void Forward(UIElement element, bool click)
    {
      this.Detach();
      if (click)
      {
        if (element is IKeyTipedControl keyTipedControl)
          keyTipedControl.OnKeyTipPressed();
        element.UpdateLayout();
      }
      UIElement[] array = LogicalTreeHelper.GetChildren((DependencyObject) element).Cast<object>().Where<object>((Func<object, bool>) (x => x is UIElement)).Cast<UIElement>().ToArray<UIElement>();
      if (array.Length == 0)
      {
        this.Terminate();
      }
      else
      {
        this.childAdorner = KeyTipAdorner.GetTopLevelElement(array[0]) != KeyTipAdorner.GetTopLevelElement(element) ? new KeyTipAdorner(array[0], element, this) : new KeyTipAdorner(element, element, this);
        this.Detach();
        this.childAdorner.Attach();
      }
    }

    private UIElement TryGetElement(string keys)
    {
      for (int index = 0; index < this.keyTips.Count; ++index)
      {
        if (this.keyTips[index].IsEnabled && keys.ToUpper(CultureInfo.CurrentUICulture) == ((string) this.keyTips[index].Content).ToUpper(CultureInfo.CurrentUICulture))
          return this.associatedElements[index];
      }
      return (UIElement) null;
    }

    public bool IsElementsStartWith(string keys)
    {
      for (int index = 0; index < this.keyTips.Count; ++index)
      {
        if (this.keyTips[index].IsEnabled)
        {
          string upper = keys.ToUpper(CultureInfo.CurrentUICulture);
          if (((string) this.keyTips[index].Content).ToUpper(CultureInfo.CurrentUICulture).StartsWith(upper, StringComparison.CurrentCulture))
            return true;
        }
      }
      return false;
    }

    private void FilterKeyTips()
    {
      if (this.backupedVisibilities == null)
      {
        this.backupedVisibilities = new Visibility[this.keyTips.Count];
        for (int index = 0; index < this.backupedVisibilities.Length; ++index)
          this.backupedVisibilities[index] = this.keyTips[index].Visibility;
      }
      for (int index = 0; index < this.keyTips.Count; ++index)
      {
        string content = (string) this.keyTips[index].Content;
        if (string.IsNullOrEmpty(this.enteredKeys))
          this.keyTips[index].Visibility = this.backupedVisibilities[index];
        else
          this.keyTips[index].Visibility = content.StartsWith(this.enteredKeys, StringComparison.CurrentCulture) ? this.backupedVisibilities[index] : Visibility.Collapsed;
      }
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
      System.Windows.Rect rect = new System.Windows.Rect(finalSize);
      for (int index = 0; index < this.keyTips.Count; ++index)
        this.keyTips[index].Arrange(new System.Windows.Rect(this.keyTipPositions[index], this.keyTips[index].DesiredSize));
      return finalSize;
    }

    protected override Size MeasureOverride(Size constraint)
    {
      Size availableSize = new Size(double.PositiveInfinity, double.PositiveInfinity);
      foreach (UIElement keyTip in this.keyTips)
        keyTip.Measure(availableSize);
      this.UpdateKeyTipPositions();
      Size size = new Size(0.0, 0.0);
      for (int index = 0; index < this.keyTips.Count; ++index)
      {
        double num1 = this.keyTips[index].DesiredSize.Width + this.keyTipPositions[index].X;
        double num2 = this.keyTips[index].DesiredSize.Height + this.keyTipPositions[index].Y;
        if (num1 > size.Width)
          size.Width = num1;
        if (num2 > size.Height)
          size.Height = num2;
      }
      return size;
    }

    private static RibbonGroupBox GetGroupBox(DependencyObject element)
    {
      if (element == null)
        return (RibbonGroupBox) null;
      return element is RibbonGroupBox ribbonGroupBox ? ribbonGroupBox : KeyTipAdorner.GetGroupBox(VisualTreeHelper.GetParent(element));
    }

    [SuppressMessage("Microsoft.Maintainability", "CA1502")]
    private void UpdateKeyTipPositions()
    {
      if (this.keyTips.Count == 0)
        return;
      double[] numArray = (double[]) null;
      RibbonGroupBox groupBox = KeyTipAdorner.GetGroupBox((DependencyObject) this.oneOfAssociatedElements);
      if (groupBox != null)
      {
        Panel panel = groupBox.GetPanel();
        if (panel != null)
        {
          double height = groupBox.GetLayoutRoot().DesiredSize.Height;
          numArray = new double[4]
          {
            groupBox.GetLayoutRoot().TranslatePoint(new Point(0.0, 0.0), this.AdornedElement).Y,
            groupBox.GetLayoutRoot().TranslatePoint(new Point(0.0, panel.DesiredSize.Height / 2.0), this.AdornedElement).Y,
            groupBox.GetLayoutRoot().TranslatePoint(new Point(0.0, panel.DesiredSize.Height), this.AdornedElement).Y,
            groupBox.GetLayoutRoot().TranslatePoint(new Point(0.0, height + 1.0), this.AdornedElement).Y
          };
        }
      }
      for (int index1 = 0; index1 < this.keyTips.Count; ++index1)
      {
        if (this.keyTips[index1].Visibility == Visibility.Visible)
        {
          bool flag1 = this.associatedElements[index1].Visibility == Visibility.Visible;
          bool flag2 = VisualTreeHelper.GetParent((DependencyObject) this.associatedElements[index1]) != null;
          this.keyTips[index1].Visibility = !flag1 || !flag2 ? Visibility.Collapsed : Visibility.Visible;
          if (!KeyTip.GetAutoPlacement((DependencyObject) this.associatedElements[index1]))
          {
            Size desiredSize = this.keyTips[index1].DesiredSize;
            Size renderSize = this.associatedElements[index1].RenderSize;
            double x = 0.0;
            double y = 0.0;
            Thickness margin = KeyTip.GetMargin((DependencyObject) this.associatedElements[index1]);
            switch (KeyTip.GetHorizontalAlignment((DependencyObject) this.associatedElements[index1]))
            {
              case HorizontalAlignment.Left:
                x = margin.Left;
                break;
              case HorizontalAlignment.Center:
              case HorizontalAlignment.Stretch:
                x = renderSize.Width / 2.0 - desiredSize.Width / 2.0 + margin.Left;
                break;
              case HorizontalAlignment.Right:
                x = renderSize.Width - desiredSize.Width - margin.Right;
                break;
            }
            switch (KeyTip.GetVerticalAlignment((DependencyObject) this.associatedElements[index1]))
            {
              case VerticalAlignment.Top:
                y = margin.Top;
                break;
              case VerticalAlignment.Center:
              case VerticalAlignment.Stretch:
                y = renderSize.Height / 2.0 - desiredSize.Height / 2.0 + margin.Top;
                break;
              case VerticalAlignment.Bottom:
                y = renderSize.Height - desiredSize.Height - margin.Bottom;
                break;
            }
            this.keyTipPositions[index1] = this.associatedElements[index1].TranslatePoint(new Point(x, y), this.AdornedElement);
          }
          else if (((FrameworkElement) this.associatedElements[index1]).Name == "PART_DialogLauncherButton")
          {
            Size desiredSize = this.keyTips[index1].DesiredSize;
            Size renderSize = this.associatedElements[index1].RenderSize;
            if (numArray != null)
            {
              this.keyTipPositions[index1] = this.associatedElements[index1].TranslatePoint(new Point(renderSize.Width / 2.0 - desiredSize.Width / 2.0, 0.0), this.AdornedElement);
              this.keyTipPositions[index1].Y = numArray[3];
            }
          }
          else if (this.associatedElements[index1] is InRibbonGallery && !((InRibbonGallery) this.associatedElements[index1]).IsCollapsed)
          {
            Size desiredSize = this.keyTips[index1].DesiredSize;
            Size renderSize = this.associatedElements[index1].RenderSize;
            if (numArray != null)
            {
              this.keyTipPositions[index1] = this.associatedElements[index1].TranslatePoint(new Point(renderSize.Width - desiredSize.Width / 2.0, 0.0), this.AdornedElement);
              this.keyTipPositions[index1].Y = numArray[2] - desiredSize.Height / 2.0;
            }
          }
          else if (this.associatedElements[index1] is RibbonTabItem || this.associatedElements[index1] is Backstage)
          {
            Size desiredSize = this.keyTips[index1].DesiredSize;
            Size renderSize = this.associatedElements[index1].RenderSize;
            this.keyTipPositions[index1] = this.associatedElements[index1].TranslatePoint(new Point(renderSize.Width / 2.0 - desiredSize.Width / 2.0, renderSize.Height - desiredSize.Height / 2.0), this.AdornedElement);
          }
          else if (this.associatedElements[index1] is RibbonGroupBox)
          {
            Size desiredSize1 = this.keyTips[index1].DesiredSize;
            Size desiredSize2 = this.associatedElements[index1].DesiredSize;
            this.keyTipPositions[index1] = this.associatedElements[index1].TranslatePoint(new Point(desiredSize2.Width / 2.0 - desiredSize1.Width / 2.0, desiredSize2.Height + 1.0), this.AdornedElement);
          }
          else if (KeyTipAdorner.IsWithinQuickAccessToolbar((DependencyObject) this.associatedElements[index1]))
          {
            Point point = this.associatedElements[index1].TranslatePoint(new Point(this.associatedElements[index1].DesiredSize.Width / 2.0 - this.keyTips[index1].DesiredSize.Width / 2.0, this.associatedElements[index1].DesiredSize.Height - this.keyTips[index1].DesiredSize.Height / 2.0), this.AdornedElement);
            this.keyTipPositions[index1] = point;
          }
          else if (this.associatedElements[index1] is MenuItem)
          {
            Size desiredSize3 = this.keyTips[index1].DesiredSize;
            Size desiredSize4 = this.associatedElements[index1].DesiredSize;
            this.keyTipPositions[index1] = this.associatedElements[index1].TranslatePoint(new Point(desiredSize4.Height / 2.0 + 2.0, desiredSize4.Height / 2.0 + 2.0), this.AdornedElement);
          }
          else if (this.associatedElements[index1] is BackstageTabItem)
          {
            Size desiredSize5 = this.keyTips[index1].DesiredSize;
            Size desiredSize6 = this.associatedElements[index1].DesiredSize;
            this.keyTipPositions[index1] = this.associatedElements[index1].TranslatePoint(new Point(5.0, desiredSize6.Height / 2.0 - desiredSize5.Height), this.AdornedElement);
          }
          else if (((FrameworkElement) this.associatedElements[index1]).Parent is BackstageTabControl)
          {
            Size desiredSize7 = this.keyTips[index1].DesiredSize;
            Size desiredSize8 = this.associatedElements[index1].DesiredSize;
            this.keyTipPositions[index1] = this.associatedElements[index1].TranslatePoint(new Point(20.0, desiredSize8.Height / 2.0 - desiredSize7.Height), this.AdornedElement);
          }
          else if (this.associatedElements[index1] is IRibbonControl && ((IRibbonControl) this.associatedElements[index1]).Size != RibbonControlSize.Large || this.associatedElements[index1] is Spinner || this.associatedElements[index1] is ComboBox || this.associatedElements[index1] is TextBox || this.associatedElements[index1] is CheckBox)
          {
            bool flag3 = KeyTipAdorner.IsWithinRibbonToolbarInTwoLine((DependencyObject) this.associatedElements[index1]);
            Point point = this.associatedElements[index1].TranslatePoint(new Point(this.keyTips[index1].DesiredSize.Width / 2.0, this.keyTips[index1].DesiredSize.Height / 2.0), this.AdornedElement);
            if (numArray != null)
            {
              int index2 = 0;
              double num1 = Math.Abs(numArray[0] - point.Y);
              for (int index3 = 1; index3 < numArray.Length; ++index3)
              {
                if (!flag3 || index3 != 1)
                {
                  double num2 = Math.Abs(numArray[index3] - point.Y);
                  if (num2 < num1)
                  {
                    num1 = num2;
                    index2 = index3;
                  }
                }
              }
              point.Y = numArray[index2] - this.keyTips[index1].DesiredSize.Height / 2.0;
            }
            this.keyTipPositions[index1] = point;
          }
          else
          {
            Point point = this.associatedElements[index1].TranslatePoint(new Point(this.associatedElements[index1].DesiredSize.Width / 2.0 - this.keyTips[index1].DesiredSize.Width / 2.0, this.associatedElements[index1].DesiredSize.Height - 8.0), this.AdornedElement);
            if (numArray != null)
              point.Y = numArray[2] - this.keyTips[index1].DesiredSize.Height / 2.0;
            this.keyTipPositions[index1] = point;
          }
        }
      }
    }

    private static bool IsWithinRibbonToolbarInTwoLine(DependencyObject element)
    {
      UIElement parent = LogicalTreeHelper.GetParent(element) as UIElement;
      if (parent is RibbonToolBar ribbonToolBar)
      {
        RibbonToolBarLayoutDefinition layoutDefinition = ribbonToolBar.GetCurrentLayoutDefinition();
        return layoutDefinition != null && (layoutDefinition.RowCount == 2 || layoutDefinition.Rows.Count == 2);
      }
      return parent != null && KeyTipAdorner.IsWithinRibbonToolbarInTwoLine((DependencyObject) parent);
    }

    private static bool IsWithinQuickAccessToolbar(DependencyObject element)
    {
      UIElement parent = LogicalTreeHelper.GetParent(element) as UIElement;
      if (parent is QuickAccessToolBar)
        return true;
      return parent != null && KeyTipAdorner.IsWithinQuickAccessToolbar((DependencyObject) parent);
    }

    protected override int VisualChildrenCount => this.keyTips.Count;

    protected override Visual GetVisualChild(int index) => (Visual) this.keyTips[index];

    [SuppressMessage("Microsoft.Performance", "CA1801")]
    [SuppressMessage("Microsoft.Performance", "CA1822")]
    private void Log(string message)
    {
    }
  }
}
