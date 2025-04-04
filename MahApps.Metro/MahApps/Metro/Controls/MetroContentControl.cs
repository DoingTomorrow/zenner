// Decompiled with JetBrains decompiler
// Type: MahApps.Metro.Controls.MetroContentControl
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

#nullable disable
namespace MahApps.Metro.Controls
{
  public class MetroContentControl : ContentControl
  {
    public static readonly DependencyProperty ReverseTransitionProperty = DependencyProperty.Register(nameof (ReverseTransition), typeof (bool), typeof (MetroContentControl), (PropertyMetadata) new FrameworkPropertyMetadata((object) false));
    public static readonly DependencyProperty TransitionsEnabledProperty = DependencyProperty.Register(nameof (TransitionsEnabled), typeof (bool), typeof (MetroContentControl), (PropertyMetadata) new FrameworkPropertyMetadata((object) true));
    public static readonly DependencyProperty OnlyLoadTransitionProperty = DependencyProperty.Register(nameof (OnlyLoadTransition), typeof (bool), typeof (MetroContentControl), (PropertyMetadata) new FrameworkPropertyMetadata((object) false));
    private bool transitionLoaded;

    public bool ReverseTransition
    {
      get => (bool) this.GetValue(MetroContentControl.ReverseTransitionProperty);
      set => this.SetValue(MetroContentControl.ReverseTransitionProperty, (object) value);
    }

    public bool TransitionsEnabled
    {
      get => (bool) this.GetValue(MetroContentControl.TransitionsEnabledProperty);
      set => this.SetValue(MetroContentControl.TransitionsEnabledProperty, (object) value);
    }

    public bool OnlyLoadTransition
    {
      get => (bool) this.GetValue(MetroContentControl.OnlyLoadTransitionProperty);
      set => this.SetValue(MetroContentControl.OnlyLoadTransitionProperty, (object) value);
    }

    public MetroContentControl()
    {
      this.DefaultStyleKey = (object) typeof (MetroContentControl);
      this.Loaded += new RoutedEventHandler(this.MetroContentControlLoaded);
      this.Unloaded += new RoutedEventHandler(this.MetroContentControlUnloaded);
    }

    private void MetroContentControlIsVisibleChanged(
      object sender,
      DependencyPropertyChangedEventArgs e)
    {
      if (!this.TransitionsEnabled || this.transitionLoaded)
        return;
      if (!this.IsVisible)
        VisualStateManager.GoToState((FrameworkElement) this, this.ReverseTransition ? "AfterUnLoadedReverse" : "AfterUnLoaded", false);
      else
        VisualStateManager.GoToState((FrameworkElement) this, this.ReverseTransition ? "AfterLoadedReverse" : "AfterLoaded", true);
    }

    private void MetroContentControlUnloaded(object sender, RoutedEventArgs e)
    {
      if (!this.TransitionsEnabled)
        return;
      if (this.transitionLoaded)
        VisualStateManager.GoToState((FrameworkElement) this, this.ReverseTransition ? "AfterUnLoadedReverse" : "AfterUnLoaded", false);
      this.IsVisibleChanged -= new DependencyPropertyChangedEventHandler(this.MetroContentControlIsVisibleChanged);
    }

    private void MetroContentControlLoaded(object sender, RoutedEventArgs e)
    {
      if (this.TransitionsEnabled)
      {
        if (!this.transitionLoaded)
        {
          this.transitionLoaded = this.OnlyLoadTransition;
          VisualStateManager.GoToState((FrameworkElement) this, this.ReverseTransition ? "AfterLoadedReverse" : "AfterLoaded", true);
        }
        this.IsVisibleChanged -= new DependencyPropertyChangedEventHandler(this.MetroContentControlIsVisibleChanged);
        this.IsVisibleChanged += new DependencyPropertyChangedEventHandler(this.MetroContentControlIsVisibleChanged);
      }
      else
      {
        Grid templateChild = (Grid) this.GetTemplateChild("root");
        templateChild.Opacity = 1.0;
        TranslateTransform renderTransform = (TranslateTransform) templateChild.RenderTransform;
        if (renderTransform.IsFrozen)
        {
          TranslateTransform translateTransform = renderTransform.Clone();
          translateTransform.X = 0.0;
          templateChild.RenderTransform = (Transform) translateTransform;
        }
        else
          renderTransform.X = 0.0;
      }
    }

    public void Reload()
    {
      if (!this.TransitionsEnabled || this.transitionLoaded)
        return;
      if (this.ReverseTransition)
      {
        VisualStateManager.GoToState((FrameworkElement) this, "BeforeLoaded", true);
        VisualStateManager.GoToState((FrameworkElement) this, "AfterUnLoadedReverse", true);
      }
      else
      {
        VisualStateManager.GoToState((FrameworkElement) this, "BeforeLoaded", true);
        VisualStateManager.GoToState((FrameworkElement) this, "AfterLoaded", true);
      }
    }
  }
}
