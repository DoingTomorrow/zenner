// Decompiled with JetBrains decompiler
// Type: MSS.Client.UI.Tablet.View.Settings.ConfigChangeableParams
// Assembly: MSS.Client.UI.Tablet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5E385CF5-9E3C-48E5-A180-D55EEE638A8F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Client.UI.Tablet.dll

using Microsoft.CSharp.RuntimeBinder;
using MSS.Business.Events;
using MSS.Client.UI.Common;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;
using Telerik.Windows.Controls;

#nullable disable
namespace MSS.Client.UI.Tablet.View.Settings
{
  public partial class ConfigChangeableParams : ResizableMetroWindow, IComponentConnector
  {
    internal Grid EquipmentGrid;
    internal RadComboBox SelectedEquipmentGroupComboBox;
    internal RadComboBox SelectedEquipmentModelComboBox;
    internal ToggleButton equipmentModelButton;
    internal Popup popupEquipmentModel;
    internal StackPanel StaticStackPanel;
    internal Button RefreshPortsButton;
    internal Button SaveButton;
    internal Button CancelButton;
    private bool _contentLoaded;

    public ConfigChangeableParams()
    {
      this.InitializeComponent();
      this.SourceInitialized += new EventHandler(((ResizableMetroWindow) this).win_SourceInitialized);
      this.Loaded += new RoutedEventHandler(this.OnLoaded);
      this.MouseLeftButtonUp += new MouseButtonEventHandler(((ResizableMetroWindow) this).OnMouseLeftButtonUp);
      EventPublisher.Register<EquipmentConfigEvent>(new Action<EquipmentConfigEvent>(this.RefreshView));
    }

    ~ConfigChangeableParams()
    {
      this.SourceInitialized -= new EventHandler(((ResizableMetroWindow) this).win_SourceInitialized);
      this.Loaded -= new RoutedEventHandler(this.OnLoaded);
      this.MouseLeftButtonUp -= new MouseButtonEventHandler(((ResizableMetroWindow) this).OnMouseLeftButtonUp);
    }

    private void RefreshView(EquipmentConfigEvent obj)
    {
      this.StaticStackPanel.Tag = (object) obj.EquipmentConfigValues;
    }

    private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
    {
      // ISSUE: reference to a compiler-generated field
      if (ConfigChangeableParams.\u003C\u003Eo__3.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ConfigChangeableParams.\u003C\u003Eo__3.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "EquipmentConfigsList", typeof (ConfigChangeableParams), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, object> target = ConfigChangeableParams.\u003C\u003Eo__3.\u003C\u003Ep__1.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, object>> p1 = ConfigChangeableParams.\u003C\u003Eo__3.\u003C\u003Ep__1;
      // ISSUE: reference to a compiler-generated field
      if (ConfigChangeableParams.\u003C\u003Eo__3.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ConfigChangeableParams.\u003C\u003Eo__3.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "EquipmentSelectorProperty", typeof (ConfigChangeableParams), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj = ConfigChangeableParams.\u003C\u003Eo__3.\u003C\u003Ep__0.Target((CallSite) ConfigChangeableParams.\u003C\u003Eo__3.\u003C\u003Ep__0, this.DataContext);
      this.StaticStackPanel.Tag = target((CallSite) p1, obj);
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/MSS.Client.UI.Tablet;component/view/settings/configchangeableparams.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.EquipmentGrid = (Grid) target;
          break;
        case 2:
          this.SelectedEquipmentGroupComboBox = (RadComboBox) target;
          break;
        case 3:
          this.SelectedEquipmentModelComboBox = (RadComboBox) target;
          break;
        case 4:
          this.equipmentModelButton = (ToggleButton) target;
          break;
        case 5:
          this.popupEquipmentModel = (Popup) target;
          break;
        case 6:
          this.StaticStackPanel = (StackPanel) target;
          break;
        case 7:
          this.RefreshPortsButton = (Button) target;
          break;
        case 8:
          this.SaveButton = (Button) target;
          break;
        case 9:
          this.CancelButton = (Button) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
