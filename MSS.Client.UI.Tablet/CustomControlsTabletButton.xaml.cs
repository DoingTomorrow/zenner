// Decompiled with JetBrains decompiler
// Type: MSS.Client.UI.Tablet.CustomControls.TabletButton
// Assembly: MSS.Client.UI.Tablet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5E385CF5-9E3C-48E5-A180-D55EEE638A8F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Client.UI.Tablet.dll

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;

#nullable disable
namespace MSS.Client.UI.Tablet.CustomControls
{
  public partial class TabletButton : UserControl, IComponentConnector
  {
    public static DependencyProperty CommandProperty = DependencyProperty.Register(nameof (ButtonCommand), typeof (ICommand), typeof (TabletButton), (PropertyMetadata) new FrameworkPropertyMetadata());
    public static DependencyProperty CommandParameterProperty;
    public static readonly DependencyProperty ButtonTextProperty;
    public static readonly DependencyProperty ButtonPathProperty;
    internal TabletButton TargetButton;
    private bool _contentLoaded;

    public TabletButton() => this.InitializeComponent();

    public ICommand ButtonCommand
    {
      get => (ICommand) this.GetValue(TabletButton.CommandProperty);
      set => this.SetValue(TabletButton.CommandProperty, (object) value);
    }

    public string ButtonText
    {
      get => (string) this.GetValue(TabletButton.ButtonTextProperty);
      set => this.SetValue(TabletButton.ButtonTextProperty, (object) value);
    }

    public string ButtonPath
    {
      get => (string) this.GetValue(TabletButton.ButtonPathProperty);
      set => this.SetValue(TabletButton.ButtonPathProperty, (object) value);
    }

    public object ButtonCommandParameter
    {
      get => this.GetValue(TabletButton.ButtonPathProperty);
      set => this.SetValue(TabletButton.ButtonPathProperty, value);
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/MSS.Client.UI.Tablet;component/customcontrols/tabletbutton.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      if (connectionId == 1)
        this.TargetButton = (TabletButton) target;
      else
        this._contentLoaded = true;
    }

    static TabletButton()
    {
      Type propertyType = typeof (object);
      Type ownerType = typeof (TabletButton);
      FrameworkPropertyMetadata propertyMetadata = new FrameworkPropertyMetadata();
      propertyMetadata.PropertyChangedCallback = (PropertyChangedCallback) ((d, e) => { });
      FrameworkPropertyMetadata typeMetadata = propertyMetadata;
      TabletButton.CommandParameterProperty = DependencyProperty.Register(nameof (ButtonCommandParameter), propertyType, ownerType, (PropertyMetadata) typeMetadata);
      TabletButton.ButtonTextProperty = DependencyProperty.Register(nameof (ButtonText), typeof (string), typeof (TabletButton), (PropertyMetadata) new FrameworkPropertyMetadata());
      TabletButton.ButtonPathProperty = DependencyProperty.Register(nameof (ButtonPath), typeof (string), typeof (TabletButton), (PropertyMetadata) new FrameworkPropertyMetadata());
    }
  }
}
