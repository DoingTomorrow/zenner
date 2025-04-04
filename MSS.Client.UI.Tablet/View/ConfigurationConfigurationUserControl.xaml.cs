// Decompiled with JetBrains decompiler
// Type: MSS.Client.UI.Tablet.View.Configuration.ConfigurationUserControl
// Assembly: MSS.Client.UI.Tablet, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5E385CF5-9E3C-48E5-A180-D55EEE638A8F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Client.UI.Tablet.dll

using Microsoft.CSharp.RuntimeBinder;
using MSS.Business.Events;
using MSS.Business.Modules.Configuration;
using MSS.Client.UI.Common.Utils;
using MSS.Client.UI.Tablet.Common;
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
using System.Windows.Threading;
using Telerik.Windows.Controls;
using WpfKb.Controls;
using ZENNER.CommonLibrary.Entities;

#nullable disable
namespace MSS.Client.UI.Tablet.View.Configuration
{
  public partial class ConfigurationUserControl : UserControl, IComponentConnector
  {
    internal ConfigurationUserControl ConfigUserControl;
    internal ScrollViewer scrollViewer;
    internal RadBusyIndicator BusyIndicator;
    internal Button ReadConfigurationButton;
    internal TextBlock ReadConfigTextBox;
    internal Button TestConfigurationButton;
    internal TextBlock TestConfigTextBox;
    internal Button WriteConfigurationButton;
    internal TextBlock WriteConfigTextBox;
    internal Button ExportConfigurationParametersButton;
    internal TextBlock PrintConfigTextBox;
    internal Button ReadValuesButton;
    internal TextBlock ReadValuesTextBox;
    internal Button UpgradeFirmwareButton;
    internal ToggleButton equipmentModelButton;
    internal Popup popupEquipmentModel;
    internal RadComboBox SelectedEquipmentGroupComboBox;
    internal RadComboBox SelectedEquipmentModelComboBox;
    internal StackPanel EquipmentChangeableParametersStackPanel;
    internal ToggleButton deviceModelButton;
    internal Popup popupDeviceModel;
    internal RadComboBox DeviceModelComboBox;
    internal RadComboBox DeviceGroupComboBox;
    internal StackPanel DeviceModelChangeableParametersStackPanel;
    internal RadComboBox ProfileTypeComboBox;
    internal ToggleButton profileTypeButton;
    internal Popup popupProfileType;
    internal StackPanel ProfileTypeChangeableParametersStackPanel;
    internal Button btnExpertConfiguration;
    internal RadTabControl ConfigTabControl;
    internal RadTabItem GeneralTabItem;
    internal StackPanel MeterConfigStackPanelDynamic;
    internal RadTabItem Channel1Tab;
    internal StackPanel MeterConfigChannel1StackPanelDynamic;
    internal RadTabItem Channel2Tab;
    internal StackPanel MeterConfigChannel2StackPanelDynamic;
    internal RadTabItem Channel3Tab;
    internal StackPanel MeterConfigChannel3StackPanelDynamic;
    private bool _contentLoaded;

    public ConfigurationUserControl()
    {
      this.InitializeComponent();
      EventPublisher.Register<MeterConfigurationEvent>(new Action<MeterConfigurationEvent>(this.RefreshView));
      EventPublisher.Register<MeterConfigurationParamsLoadedEvent>((Action<MeterConfigurationParamsLoadedEvent>) (_ => Application.Current.Dispatcher.Invoke((Action) (() => this.ConfigureTextBox()), DispatcherPriority.ApplicationIdle)));
      EventPublisher.Register<ChangeableParametersLoadedEvent>(new Action<ChangeableParametersLoadedEvent>(this.RefreshChangeableParametersGrid));
    }

    ~ConfigurationUserControl()
    {
      this.GeneralTabItem.Loaded -= new RoutedEventHandler(this.GeneralTabItem_Loaded);
    }

    private void ConfigureTextBox()
    {
      this.FindVisualChildren<TextBox>().ForEach<TextBox>((Action<TextBox>) (_ => _.InputScope = new InputScope()
      {
        Names = {
          (object) new InputScopeName()
          {
            NameValue = InputScopeNameValue.EmailSmtpAddress
          }
        }
      }));
    }

    private void RefreshView(MeterConfigurationEvent obj)
    {
      this.BuildGrid(obj.ConfigValuesPerChannelList, obj.ComboboxCommand, obj.SelectedTab);
    }

    public void BuildGrid(
      List<ConfigurationPerChannel> configPerChannelList,
      System.Windows.Input.ICommand comboBoxCommand,
      int selectedTab = 0)
    {
      this.MeterConfigStackPanelDynamic.Tag = (object) null;
      this.MeterConfigStackPanelDynamic.Children.Clear();
      this.MeterConfigChannel1StackPanelDynamic.Tag = (object) null;
      this.MeterConfigChannel1StackPanelDynamic.Children.Clear();
      this.MeterConfigChannel2StackPanelDynamic.Tag = (object) null;
      this.MeterConfigChannel2StackPanelDynamic.Children.Clear();
      this.MeterConfigChannel3StackPanelDynamic.Tag = (object) null;
      this.MeterConfigChannel3StackPanelDynamic.Children.Clear();
      switch (selectedTab)
      {
        case 0:
          this.GeneralTabItem.IsSelected = true;
          break;
        case 1:
          this.Channel1Tab.IsSelected = true;
          break;
        case 2:
          this.Channel2Tab.IsSelected = true;
          break;
        case 3:
          this.Channel3Tab.IsSelected = true;
          break;
      }
      if (configPerChannelList.Count > 0)
      {
        this.MeterConfigStackPanelDynamic.Children.Clear();
        GridControl gridControl = new GridControl();
        gridControl.Name = "GridConfigs";
        GridControl dynamicGrid1 = gridControl;
        int dynamicGrid2 = (int) DynamicGridControl.CreateDynamicGrid((IList<Config>) configPerChannelList[0].ConfigValues, out dynamicGrid1, comboBoxCommand, 1200.0, 30.0, new bool?(true), true);
        this.MeterConfigStackPanelDynamic.Children.Add((UIElement) dynamicGrid1);
        this.MeterConfigStackPanelDynamic.Tag = (object) configPerChannelList[0].ConfigValues;
        // ISSUE: reference to a compiler-generated field
        if (ConfigurationUserControl.\u003C\u003Eo__4.\u003C\u003Ep__1 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ConfigurationUserControl.\u003C\u003Eo__4.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, List<Config>, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "DynamicGridTag", typeof (ConfigurationUserControl), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, List<Config>, object> target = ConfigurationUserControl.\u003C\u003Eo__4.\u003C\u003Ep__1.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, List<Config>, object>> p1 = ConfigurationUserControl.\u003C\u003Eo__4.\u003C\u003Ep__1;
        // ISSUE: reference to a compiler-generated field
        if (ConfigurationUserControl.\u003C\u003Eo__4.\u003C\u003Ep__0 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ConfigurationUserControl.\u003C\u003Eo__4.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ConfigurationParameters", typeof (ConfigurationUserControl), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj1 = ConfigurationUserControl.\u003C\u003Eo__4.\u003C\u003Ep__0.Target((CallSite) ConfigurationUserControl.\u003C\u003Eo__4.\u003C\u003Ep__0, this.DataContext);
        List<Config> configValues = configPerChannelList[0].ConfigValues;
        object obj2 = target((CallSite) p1, obj1, configValues);
      }
      if (configPerChannelList.Count > 1)
      {
        this.MeterConfigChannel1StackPanelDynamic.Children.Clear();
        GridControl gridControl = new GridControl();
        gridControl.Name = "GridConfigsChannel1";
        GridControl dynamicGrid3 = gridControl;
        int dynamicGrid4 = (int) DynamicGridControl.CreateDynamicGrid((IList<Config>) configPerChannelList[1].ConfigValues, out dynamicGrid3, gridWidth: 1200.0, firstColumnPercentage: 30.0, isTabletMode: new bool?(true), hasDescriptionTextBoxes: true);
        this.MeterConfigChannel1StackPanelDynamic.Children.Add((UIElement) dynamicGrid3);
        this.MeterConfigChannel1StackPanelDynamic.Tag = (object) configPerChannelList[1].ConfigValues;
        // ISSUE: reference to a compiler-generated field
        if (ConfigurationUserControl.\u003C\u003Eo__4.\u003C\u003Ep__3 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ConfigurationUserControl.\u003C\u003Eo__4.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, List<Config>, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "Channel1DynamicGridTag", typeof (ConfigurationUserControl), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, List<Config>, object> target = ConfigurationUserControl.\u003C\u003Eo__4.\u003C\u003Ep__3.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, List<Config>, object>> p3 = ConfigurationUserControl.\u003C\u003Eo__4.\u003C\u003Ep__3;
        // ISSUE: reference to a compiler-generated field
        if (ConfigurationUserControl.\u003C\u003Eo__4.\u003C\u003Ep__2 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ConfigurationUserControl.\u003C\u003Eo__4.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ConfigurationParameters", typeof (ConfigurationUserControl), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj3 = ConfigurationUserControl.\u003C\u003Eo__4.\u003C\u003Ep__2.Target((CallSite) ConfigurationUserControl.\u003C\u003Eo__4.\u003C\u003Ep__2, this.DataContext);
        List<Config> configValues = configPerChannelList[1].ConfigValues;
        object obj4 = target((CallSite) p3, obj3, configValues);
      }
      if (configPerChannelList.Count > 2)
      {
        this.MeterConfigChannel2StackPanelDynamic.Children.Clear();
        GridControl gridControl = new GridControl();
        gridControl.Name = "GridConfigsChannel2";
        GridControl dynamicGrid5 = gridControl;
        int dynamicGrid6 = (int) DynamicGridControl.CreateDynamicGrid((IList<Config>) configPerChannelList[2].ConfigValues, out dynamicGrid5, gridWidth: 1200.0, firstColumnPercentage: 30.0, isTabletMode: new bool?(true), hasDescriptionTextBoxes: true);
        this.MeterConfigChannel2StackPanelDynamic.Children.Add((UIElement) dynamicGrid5);
        this.MeterConfigChannel2StackPanelDynamic.Tag = (object) configPerChannelList[2].ConfigValues;
        // ISSUE: reference to a compiler-generated field
        if (ConfigurationUserControl.\u003C\u003Eo__4.\u003C\u003Ep__5 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ConfigurationUserControl.\u003C\u003Eo__4.\u003C\u003Ep__5 = CallSite<Func<CallSite, object, List<Config>, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "Channel2DynamicGridTag", typeof (ConfigurationUserControl), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, List<Config>, object> target = ConfigurationUserControl.\u003C\u003Eo__4.\u003C\u003Ep__5.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, List<Config>, object>> p5 = ConfigurationUserControl.\u003C\u003Eo__4.\u003C\u003Ep__5;
        // ISSUE: reference to a compiler-generated field
        if (ConfigurationUserControl.\u003C\u003Eo__4.\u003C\u003Ep__4 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ConfigurationUserControl.\u003C\u003Eo__4.\u003C\u003Ep__4 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ConfigurationParameters", typeof (ConfigurationUserControl), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj5 = ConfigurationUserControl.\u003C\u003Eo__4.\u003C\u003Ep__4.Target((CallSite) ConfigurationUserControl.\u003C\u003Eo__4.\u003C\u003Ep__4, this.DataContext);
        List<Config> configValues = configPerChannelList[2].ConfigValues;
        object obj6 = target((CallSite) p5, obj5, configValues);
      }
      if (configPerChannelList.Count <= 3)
        return;
      this.MeterConfigChannel3StackPanelDynamic.Children.Clear();
      GridControl gridControl1 = new GridControl();
      gridControl1.Name = "GridConfigsChannel3";
      GridControl dynamicGrid7 = gridControl1;
      int dynamicGrid8 = (int) DynamicGridControl.CreateDynamicGrid((IList<Config>) configPerChannelList[3].ConfigValues, out dynamicGrid7, gridWidth: 1200.0, firstColumnPercentage: 30.0, isTabletMode: new bool?(true), hasDescriptionTextBoxes: true);
      this.MeterConfigChannel3StackPanelDynamic.Children.Add((UIElement) dynamicGrid7);
      this.MeterConfigChannel3StackPanelDynamic.Tag = (object) configPerChannelList[3].ConfigValues;
      // ISSUE: reference to a compiler-generated field
      if (ConfigurationUserControl.\u003C\u003Eo__4.\u003C\u003Ep__7 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ConfigurationUserControl.\u003C\u003Eo__4.\u003C\u003Ep__7 = CallSite<Func<CallSite, object, List<Config>, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "Channel3DynamicGridTag", typeof (ConfigurationUserControl), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, List<Config>, object> target1 = ConfigurationUserControl.\u003C\u003Eo__4.\u003C\u003Ep__7.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, List<Config>, object>> p7 = ConfigurationUserControl.\u003C\u003Eo__4.\u003C\u003Ep__7;
      // ISSUE: reference to a compiler-generated field
      if (ConfigurationUserControl.\u003C\u003Eo__4.\u003C\u003Ep__6 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ConfigurationUserControl.\u003C\u003Eo__4.\u003C\u003Ep__6 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ConfigurationParameters", typeof (ConfigurationUserControl), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj7 = ConfigurationUserControl.\u003C\u003Eo__4.\u003C\u003Ep__6.Target((CallSite) ConfigurationUserControl.\u003C\u003Eo__4.\u003C\u003Ep__6, this.DataContext);
      List<Config> configValues1 = configPerChannelList[3].ConfigValues;
      object obj8 = target1((CallSite) p7, obj7, configValues1);
    }

    private void GenerateDynamicGrid(
      List<ConfigurationPerChannel> configPerChannelList,
      object configValues,
      object dynamicPanel,
      int index)
    {
      // ISSUE: reference to a compiler-generated field
      if (ConfigurationUserControl.\u003C\u003Eo__5.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ConfigurationUserControl.\u003C\u003Eo__5.\u003C\u003Ep__1 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Clear", (IEnumerable<Type>) null, typeof (ConfigurationUserControl), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Action<CallSite, object> target1 = ConfigurationUserControl.\u003C\u003Eo__5.\u003C\u003Ep__1.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Action<CallSite, object>> p1 = ConfigurationUserControl.\u003C\u003Eo__5.\u003C\u003Ep__1;
      // ISSUE: reference to a compiler-generated field
      if (ConfigurationUserControl.\u003C\u003Eo__5.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ConfigurationUserControl.\u003C\u003Eo__5.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Children", typeof (ConfigurationUserControl), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj1 = ConfigurationUserControl.\u003C\u003Eo__5.\u003C\u003Ep__0.Target((CallSite) ConfigurationUserControl.\u003C\u003Eo__5.\u003C\u003Ep__0, dynamicPanel);
      target1((CallSite) p1, obj1);
      GridControl gridControl1 = new GridControl();
      gridControl1.Name = string.Format("GridConfigsChannel{0}", (object) configPerChannelList.Count);
      GridControl dynamicGrid1 = gridControl1;
      int dynamicGrid2 = (int) DynamicGridControl.CreateDynamicGrid((IList<Config>) configPerChannelList[index].ConfigValues, out dynamicGrid1, gridWidth: 1200.0, firstColumnPercentage: 30.0, isTabletMode: new bool?(true), hasDescriptionTextBoxes: true);
      // ISSUE: reference to a compiler-generated field
      if (ConfigurationUserControl.\u003C\u003Eo__5.\u003C\u003Ep__3 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ConfigurationUserControl.\u003C\u003Eo__5.\u003C\u003Ep__3 = CallSite<Action<CallSite, object, GridControl>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Add", (IEnumerable<Type>) null, typeof (ConfigurationUserControl), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Action<CallSite, object, GridControl> target2 = ConfigurationUserControl.\u003C\u003Eo__5.\u003C\u003Ep__3.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Action<CallSite, object, GridControl>> p3 = ConfigurationUserControl.\u003C\u003Eo__5.\u003C\u003Ep__3;
      // ISSUE: reference to a compiler-generated field
      if (ConfigurationUserControl.\u003C\u003Eo__5.\u003C\u003Ep__2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ConfigurationUserControl.\u003C\u003Eo__5.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Children", typeof (ConfigurationUserControl), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj2 = ConfigurationUserControl.\u003C\u003Eo__5.\u003C\u003Ep__2.Target((CallSite) ConfigurationUserControl.\u003C\u003Eo__5.\u003C\u003Ep__2, dynamicPanel);
      GridControl gridControl2 = dynamicGrid1;
      target2((CallSite) p3, obj2, gridControl2);
      // ISSUE: reference to a compiler-generated field
      if (ConfigurationUserControl.\u003C\u003Eo__5.\u003C\u003Ep__4 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ConfigurationUserControl.\u003C\u003Eo__5.\u003C\u003Ep__4 = CallSite<Func<CallSite, object, List<Config>, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "Tag", typeof (ConfigurationUserControl), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj3 = ConfigurationUserControl.\u003C\u003Eo__5.\u003C\u003Ep__4.Target((CallSite) ConfigurationUserControl.\u003C\u003Eo__5.\u003C\u003Ep__4, dynamicPanel, configPerChannelList[index].ConfigValues);
    }

    private void AdornerDecorator_Loaded(object sender, RoutedEventArgs e)
    {
    }

    private void RegisterKeyboardEvents(TouchScreenKeyboardUserControl kb)
    {
      this.FindVisualChildren<TextBox>().ForEach<TextBox>((Action<TextBox>) (_ =>
      {
        _.InputScope = new InputScope()
        {
          Names = {
            (object) new InputScopeName()
            {
              NameValue = InputScopeNameValue.EmailSmtpAddress
            }
          }
        };
        CommonHandlers<OpenKeyboardEventParams>.RegisterKeyboardEvents((Control) _, kb);
      }));
    }

    private void GeneralTabItem_Loaded(object sender, RoutedEventArgs e)
    {
      if (!this.GeneralTabItem.IsVisible)
        ;
    }

    private void ConfigTabControl_SelectionChanged(object sender, RadSelectionChangedEventArgs e)
    {
      Console.WriteLine();
    }

    private void RefreshChangeableParametersGrid(ChangeableParametersLoadedEvent args)
    {
      if (!(this.FindName(args.StackPanelName) is StackPanel name))
        return;
      name.Children.Clear();
      if (args.ChangeableParameters != null && args.ChangeableParameters.Count > 0)
      {
        List<Config> changeableParameters = args.ChangeableParameters;
        GridControl element;
        ref GridControl local = ref element;
        double columnPercentage = args.GridFirstColumnPercentage;
        double gridWidth = args.GridWidth;
        double firstColumnPercentage = columnPercentage;
        ChangeableParameterUsings? nullable = new ChangeableParameterUsings?(args.Type);
        bool? isTabletMode = new bool?(true);
        ChangeableParameterUsings? type = nullable;
        int dynamicGrid = (int) DynamicGridControl.CreateDynamicGrid((IList<Config>) changeableParameters, out local, gridWidth: gridWidth, firstColumnPercentage: firstColumnPercentage, isTabletMode: isTabletMode, isValidationEnabled: true, type: type);
        name.Children.Add((UIElement) element);
      }
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/MSS.Client.UI.Tablet;component/view/configuration/configurationusercontrol.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.ConfigUserControl = (ConfigurationUserControl) target;
          break;
        case 2:
          this.scrollViewer = (ScrollViewer) target;
          break;
        case 3:
          this.BusyIndicator = (RadBusyIndicator) target;
          break;
        case 4:
          this.ReadConfigurationButton = (Button) target;
          break;
        case 5:
          this.ReadConfigTextBox = (TextBlock) target;
          break;
        case 6:
          this.TestConfigurationButton = (Button) target;
          break;
        case 7:
          this.TestConfigTextBox = (TextBlock) target;
          break;
        case 8:
          this.WriteConfigurationButton = (Button) target;
          break;
        case 9:
          this.WriteConfigTextBox = (TextBlock) target;
          break;
        case 10:
          this.ExportConfigurationParametersButton = (Button) target;
          break;
        case 11:
          this.PrintConfigTextBox = (TextBlock) target;
          break;
        case 12:
          this.ReadValuesButton = (Button) target;
          break;
        case 13:
          this.ReadValuesTextBox = (TextBlock) target;
          break;
        case 14:
          this.UpgradeFirmwareButton = (Button) target;
          break;
        case 15:
          this.equipmentModelButton = (ToggleButton) target;
          break;
        case 16:
          this.popupEquipmentModel = (Popup) target;
          break;
        case 17:
          this.SelectedEquipmentGroupComboBox = (RadComboBox) target;
          break;
        case 18:
          this.SelectedEquipmentModelComboBox = (RadComboBox) target;
          break;
        case 19:
          this.EquipmentChangeableParametersStackPanel = (StackPanel) target;
          break;
        case 20:
          this.deviceModelButton = (ToggleButton) target;
          break;
        case 21:
          this.popupDeviceModel = (Popup) target;
          break;
        case 22:
          this.DeviceModelComboBox = (RadComboBox) target;
          break;
        case 23:
          this.DeviceGroupComboBox = (RadComboBox) target;
          break;
        case 24:
          this.DeviceModelChangeableParametersStackPanel = (StackPanel) target;
          break;
        case 25:
          this.ProfileTypeComboBox = (RadComboBox) target;
          break;
        case 26:
          this.profileTypeButton = (ToggleButton) target;
          break;
        case 27:
          this.popupProfileType = (Popup) target;
          break;
        case 28:
          this.ProfileTypeChangeableParametersStackPanel = (StackPanel) target;
          break;
        case 29:
          this.btnExpertConfiguration = (Button) target;
          break;
        case 30:
          this.ConfigTabControl = (RadTabControl) target;
          this.ConfigTabControl.SelectionChanged += new RadSelectionChangedEventHandler(this.ConfigTabControl_SelectionChanged);
          break;
        case 31:
          this.GeneralTabItem = (RadTabItem) target;
          break;
        case 32:
          this.MeterConfigStackPanelDynamic = (StackPanel) target;
          break;
        case 33:
          this.Channel1Tab = (RadTabItem) target;
          break;
        case 34:
          this.MeterConfigChannel1StackPanelDynamic = (StackPanel) target;
          break;
        case 35:
          this.Channel2Tab = (RadTabItem) target;
          break;
        case 36:
          this.MeterConfigChannel2StackPanelDynamic = (StackPanel) target;
          break;
        case 37:
          this.Channel3Tab = (RadTabItem) target;
          break;
        case 38:
          this.MeterConfigChannel3StackPanelDynamic = (StackPanel) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
