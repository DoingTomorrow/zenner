// Decompiled with JetBrains decompiler
// Type: MahApps.Metro.Controls.MetroTabItem
// Assembly: MahApps.Metro, Version=1.2.4.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f
// MVID: EE31BA95-0C4D-429A-911D-E79739A50CF7
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MahApps.Metro.dll

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

#nullable disable
namespace MahApps.Metro.Controls
{
  public class MetroTabItem : TabItem
  {
    internal Button closeButton;
    internal Thickness newButtonMargin;
    internal ContentPresenter contentSite;
    private bool closeButtonClickUnloaded;
    public static readonly DependencyProperty CloseButtonEnabledProperty = DependencyProperty.Register(nameof (CloseButtonEnabled), typeof (bool), typeof (MetroTabItem), (PropertyMetadata) new FrameworkPropertyMetadata((object) false, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.Inherits, new PropertyChangedCallback(MetroTabItem.OnCloseButtonEnabledPropertyChangedCallback)));
    public static readonly DependencyProperty CloseTabCommandProperty = DependencyProperty.Register(nameof (CloseTabCommand), typeof (ICommand), typeof (MetroTabItem));
    public static readonly DependencyProperty CloseTabCommandParameterProperty = DependencyProperty.Register(nameof (CloseTabCommandParameter), typeof (object), typeof (MetroTabItem), new PropertyMetadata((PropertyChangedCallback) null));

    public MetroTabItem()
    {
      this.DefaultStyleKey = (object) typeof (MetroTabItem);
      this.Unloaded += new RoutedEventHandler(this.MetroTabItem_Unloaded);
      this.Loaded += new RoutedEventHandler(this.MetroTabItem_Loaded);
    }

    private void MetroTabItem_Loaded(object sender, RoutedEventArgs e)
    {
      if (this.closeButton == null || !this.closeButtonClickUnloaded)
        return;
      this.closeButton.Click += new RoutedEventHandler(this.closeButton_Click);
      this.closeButtonClickUnloaded = false;
    }

    private void MetroTabItem_Unloaded(object sender, RoutedEventArgs e)
    {
      this.Unloaded -= new RoutedEventHandler(this.MetroTabItem_Unloaded);
      if (this.closeButton != null)
        this.closeButton.Click -= new RoutedEventHandler(this.closeButton_Click);
      this.closeButtonClickUnloaded = true;
    }

    public override void OnApplyTemplate()
    {
      base.OnApplyTemplate();
      this.AdjustCloseButton();
      this.contentSite = this.GetTemplateChild("ContentSite") as ContentPresenter;
    }

    private void AdjustCloseButton()
    {
      this.closeButton = this.closeButton ?? this.GetTemplateChild("PART_CloseButton") as Button;
      if (this.closeButton == null)
        return;
      this.closeButton.Margin = this.newButtonMargin;
      this.closeButton.Click -= new RoutedEventHandler(this.closeButton_Click);
      this.closeButton.Click += new RoutedEventHandler(this.closeButton_Click);
    }

    private void closeButton_Click(object sender, RoutedEventArgs e)
    {
      ICommand closeTabCommand = this.CloseTabCommand;
      object parameter = this.CloseTabCommandParameter ?? (object) this;
      if (closeTabCommand != null)
      {
        if (closeTabCommand.CanExecute(parameter))
          closeTabCommand.Execute(parameter);
        this.CloseTabCommand = (ICommand) null;
        this.CloseTabCommandParameter = (object) null;
      }
      BaseMetroTabControl parent = this.TryFindParent<BaseMetroTabControl>();
      object obj1 = parent != null ? parent.ItemContainerGenerator.ItemFromContainer((DependencyObject) this) : throw new InvalidOperationException();
      object obj2 = obj1 == DependencyProperty.UnsetValue ? this.Content : obj1;
      parent.InternalCloseTabCommand.Execute((object) new Tuple<object, MetroTabItem>(obj2, this));
    }

    private static void OnCloseButtonEnabledPropertyChangedCallback(
      DependencyObject dependencyObject,
      DependencyPropertyChangedEventArgs e)
    {
      if (!(dependencyObject is MetroTabItem metroTabItem))
        return;
      metroTabItem.AdjustCloseButton();
    }

    public bool CloseButtonEnabled
    {
      get => (bool) this.GetValue(MetroTabItem.CloseButtonEnabledProperty);
      set => this.SetValue(MetroTabItem.CloseButtonEnabledProperty, (object) value);
    }

    public ICommand CloseTabCommand
    {
      get => (ICommand) this.GetValue(MetroTabItem.CloseTabCommandProperty);
      set => this.SetValue(MetroTabItem.CloseTabCommandProperty, (object) value);
    }

    public object CloseTabCommandParameter
    {
      get => this.GetValue(MetroTabItem.CloseTabCommandParameterProperty);
      set => this.SetValue(MetroTabItem.CloseTabCommandParameterProperty, value);
    }
  }
}
