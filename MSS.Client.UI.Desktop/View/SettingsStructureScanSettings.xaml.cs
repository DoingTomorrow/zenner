// Decompiled with JetBrains decompiler
// Type: MSS.Client.UI.Desktop.View.Settings.StructureScanSettings
// Assembly: MSS.Client.UI.Desktop, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B34A4718-63B5-4C6C-93C2-0A28BCAE0F44
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Client.UI.Desktop.dll

using Microsoft.CSharp.RuntimeBinder;
using MSS.Business.Events;
using MSS.Business.Modules.Configuration;
using MSS.Client.UI.Common;
using MSS.Client.UI.Common.Utils;
using Styles.Controls;
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
namespace MSS.Client.UI.Desktop.View.Settings
{
  public partial class StructureScanSettings : ResizableMetroWindow, IComponentConnector
  {
    internal DockPanel dockpanel;
    internal Grid EquipmentGrid;
    internal RadComboBox SelectedEquipmentGroupComboBox;
    internal RadComboBox SelectedEquipmentModelComboBox;
    internal ToggleButton equipmentModelButton;
    internal Popup popupEquipmentModel;
    internal StackPanel StaticStackPanel;
    internal Button RefreshPortsButton;
    internal RadComboBox SelectedProfileTypeComboBox;
    internal StackPanel ScanModeStackPanelDynamic;
    internal Button SaveButton;
    internal Button CancelButton;
    private bool _contentLoaded;

    public StructureScanSettings()
    {
      this.InitializeComponent();
      this.Loaded += new RoutedEventHandler(this.OnLoaded);
      this.MouseLeftButtonUp += new MouseButtonEventHandler(((ResizableMetroWindow) this).OnMouseLeftButtonUp);
      this.StateChanged += new EventHandler(((ResizableMetroWindow) this).OnStateChanged);
      this.Deactivated += new EventHandler(((ResizableMetroWindow) this).Window_Deactivated);
      EventPublisher.Register<ScanModeConfigEvent>(new Action<ScanModeConfigEvent>(this.RefreshScanView));
    }

    ~StructureScanSettings()
    {
      this.Loaded -= new RoutedEventHandler(this.OnLoaded);
      this.MouseLeftButtonUp -= new MouseButtonEventHandler(((ResizableMetroWindow) this).OnMouseLeftButtonUp);
      this.StateChanged -= new EventHandler(((ResizableMetroWindow) this).OnStateChanged);
      this.Deactivated -= new EventHandler(((ResizableMetroWindow) this).Window_Deactivated);
    }

    private void RefreshScanView(ScanModeConfigEvent obj)
    {
      this.BuildGrid(obj.ScanModeConfigValues, this.ScanModeStackPanelDynamic);
    }

    private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
    {
      // ISSUE: reference to a compiler-generated field
      if (StructureScanSettings.\u003C\u003Eo__3.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        StructureScanSettings.\u003C\u003Eo__3.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ScanConfigsList", typeof (StructureScanSettings), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj = StructureScanSettings.\u003C\u003Eo__3.\u003C\u003Ep__0.Target((CallSite) StructureScanSettings.\u003C\u003Eo__3.\u003C\u003Ep__0, this.DataContext);
      // ISSUE: reference to a compiler-generated field
      if (StructureScanSettings.\u003C\u003Eo__3.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        StructureScanSettings.\u003C\u003Eo__3.\u003C\u003Ep__1 = CallSite<Action<CallSite, StructureScanSettings, object, StackPanel>>.Create(Binder.InvokeMember(CSharpBinderFlags.InvokeSimpleName | CSharpBinderFlags.ResultDiscarded, "BuildGrid", (IEnumerable<Type>) null, typeof (StructureScanSettings), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      StructureScanSettings.\u003C\u003Eo__3.\u003C\u003Ep__1.Target((CallSite) StructureScanSettings.\u003C\u003Eo__3.\u003C\u003Ep__1, this, obj, this.ScanModeStackPanelDynamic);
    }

    private void BuildGrid(List<Config> configs, StackPanel stackPanelDynamic)
    {
      stackPanelDynamic.Children.Clear();
      GridControl dynamicGrid1;
      int dynamicGrid2 = (int) DynamicGridControl.CreateDynamicGrid((IList<Config>) configs, out dynamicGrid1, gridWidth: 700.0, firstColumnPercentage: 40.0);
      stackPanelDynamic.Children.Add((UIElement) dynamicGrid1);
      stackPanelDynamic.Tag = (object) configs;
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/MSS.Client.UI.Desktop;component/view/settings/structurescansettings.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.dockpanel = (DockPanel) target;
          break;
        case 2:
          this.EquipmentGrid = (Grid) target;
          break;
        case 3:
          this.SelectedEquipmentGroupComboBox = (RadComboBox) target;
          break;
        case 4:
          this.SelectedEquipmentModelComboBox = (RadComboBox) target;
          break;
        case 5:
          this.equipmentModelButton = (ToggleButton) target;
          break;
        case 6:
          this.popupEquipmentModel = (Popup) target;
          break;
        case 7:
          this.StaticStackPanel = (StackPanel) target;
          break;
        case 8:
          this.RefreshPortsButton = (Button) target;
          break;
        case 9:
          this.SelectedProfileTypeComboBox = (RadComboBox) target;
          break;
        case 10:
          this.ScanModeStackPanelDynamic = (StackPanel) target;
          break;
        case 11:
          this.SaveButton = (Button) target;
          break;
        case 12:
          this.CancelButton = (Button) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
