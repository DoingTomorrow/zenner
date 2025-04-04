// Decompiled with JetBrains decompiler
// Type: Styles.Resources.AppResourcesTablet
// Assembly: Styles, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: ABC9E615-D09A-48E5-A13F-BC53DD762FA1
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Styles.dll

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using Telerik.Windows.Controls;

#nullable disable
namespace Styles.Resources
{
  public partial class AppResourcesTablet : ResourceDictionary, IComponentConnector, IStyleConnector
  {
    private bool _contentLoaded;

    public AppResourcesTablet() => this.InitializeComponent();

    private void PreviewMouseDownHandler(object sender, MouseButtonEventArgs e)
    {
      FrameworkElement originalSource = e.OriginalSource as FrameworkElement;
      RadExpander radExpander = originalSource.ParentOfType<RadExpander>();
      Border ancestor = AppResourcesTablet.FindAncestor<Border>((DependencyObject) originalSource, "ContainerBorder");
      if (radExpander != null || ancestor != null || e.LeftButton != MouseButtonState.Pressed)
        return;
      DataGridRow dataGridRow = originalSource.ParentOfType<DataGridRow>();
      dataGridRow.IsSelected = !dataGridRow.IsSelected;
      dataGridRow.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
      e.Handled = true;
    }

    public static T FindAncestor<T>(DependencyObject current, string parentName) where T : DependencyObject
    {
      for (; current != null; current = VisualTreeHelper.GetParent(current))
      {
        if (!string.IsNullOrEmpty(parentName))
        {
          FrameworkElement frameworkElement = current as FrameworkElement;
          if (current is T && frameworkElement != null && frameworkElement.Name == parentName)
            return (T) current;
        }
        else if (current is T ancestor)
          return ancestor;
      }
      return default (T);
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Styles;component/resources/appresourcestablet.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target) => this._contentLoaded = true;

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IStyleConnector.Connect(int connectionId, object target)
    {
      if (connectionId != 1)
        return;
      ((Style) target).Setters.Add((SetterBase) new EventSetter()
      {
        Event = UIElement.PreviewMouseDownEvent,
        Handler = (Delegate) new MouseButtonEventHandler(this.PreviewMouseDownHandler)
      });
    }
  }
}
