// Decompiled with JetBrains decompiler
// Type: MSS.Client.UI.Desktop.View.Configuration.ConfigurationUserControl
// Assembly: MSS.Client.UI.Desktop, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B34A4718-63B5-4C6C-93C2-0A28BCAE0F44
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Client.UI.Desktop.dll

using Microsoft.CSharp.RuntimeBinder;
using MSS.Business.Events;
using MSS.Business.Modules.Configuration;
using MSS.Client.UI.Common.Utils;
using Styles.Controls;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;
using Telerik.Windows.Controls;
using ZENNER.CommonLibrary.Entities;

#nullable disable
namespace MSS.Client.UI.Desktop.View.Configuration
{
  public partial class ConfigurationUserControl : UserControl, IComponentConnector
  {
    private readonly Action<ActionSearch<string>, ConfigurationUserControl> ApplySearchActions = (Action<ActionSearch<string>, ConfigurationUserControl>) ((s, c) =>
    {
      if (!c.IsStackPanelCreated())
        return;
      c.SetColorForSearch(s.Message.MessageText);
      c.ActivateMatchedTab(s.Message.MessageText);
      c.ScrollToPropertyIndex(s.Message.MessageText);
    });
    internal ScrollViewer scrollViewer;
    internal RadTabControl ConfigUserControl;
    internal RadBusyIndicator BusyIndicator;
    internal Button ReadConfigurationButton;
    internal TextBlock ReadConfigTextBox;
    internal Button WriteConfigurationButton;
    internal TextBlock WriteConfigTextBox;
    internal Button ExportConfigurationParametersButton;
    internal Button ReadValuesButton;
    internal TextBlock ReadValuesTextBox;
    internal Button UpgradeFirmwareButton;
    internal ToggleButton equipmentModelButton;
    internal Popup popupEquipmentModel;
    internal RadComboBox SelectedEquipmentGroupComboBox;
    internal RadComboBox SelectedEquipmentModelComboBox;
    internal StackPanel EquipmentChangeableParametersStackPanel;
    internal RadComboBox ProfileTypeComboBox;
    internal ToggleButton profileTypeButton;
    internal Popup popupProfileType;
    internal StackPanel ProfileTypeChangeableParametersStackPanel;
    internal ToggleButton deviceModelButton;
    internal Popup popupDeviceModel;
    internal RadComboBox DeviceModelComboBox;
    internal RadComboBox DeviceGroupComboBox;
    internal StackPanel DeviceModelChangeableParametersStackPanel;
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
      EventPublisher.Register<ActionSearch<string>>((Action<ActionSearch<string>>) (searchText => this.ApplySearchActions(searchText, this)));
      EventPublisher.Register<ChangeableParametersLoadedEvent>(new Action<ChangeableParametersLoadedEvent>(this.RefreshChangeableParametersGrid));
    }

    private Dictionary<StackPanel, IEnumerable<TextBlock>> GetPanelProperties()
    {
      return this.GetScrollViewer().Select<ScrollViewer, StackPanel>((Func<ScrollViewer, StackPanel>) (_ => (StackPanel) _.Content)).ToDictionary<StackPanel, StackPanel, IEnumerable<TextBlock>>((Func<StackPanel, StackPanel>) (_ => _), (Func<StackPanel, IEnumerable<TextBlock>>) (_ => _.ChildrenOfType<TextBlock>()));
    }

    private bool IsStackPanelCreated()
    {
      return this.GetScrollViewer().ToList<ScrollViewer>().FirstOrDefault<ScrollViewer>().ChildrenOfType<StackPanel>().FirstOrDefault<StackPanel>() != null;
    }

    private void ResetColorForProperties(
      Dictionary<StackPanel, IEnumerable<TextBlock>> properties)
    {
      properties.Values.ToList<IEnumerable<TextBlock>>().ForEach((Action<IEnumerable<TextBlock>>) (_ => _.ResetColor()));
    }

    private StackPanel FindMatchedPanel(
      Dictionary<StackPanel, IEnumerable<TextBlock>> properties,
      string searchText)
    {
      return properties.FirstOrDefault<KeyValuePair<StackPanel, IEnumerable<TextBlock>>>((Func<KeyValuePair<StackPanel, IEnumerable<TextBlock>>, bool>) (v => v.Value.Any<TextBlock>((Func<TextBlock, bool>) (_ => _.Text.ToLower() == searchText.ToLower())))).Key;
    }

    private IEnumerable<TextBlock> GetMatchedProperties(
      Dictionary<StackPanel, IEnumerable<TextBlock>> properties,
      string searchText)
    {
      return properties.Values.SelectMany<IEnumerable<TextBlock>, TextBlock>((Func<IEnumerable<TextBlock>, IEnumerable<TextBlock>>) (_ => _)).Where<TextBlock>((Func<TextBlock, bool>) (_ => _.Text.ToLower() == searchText.ToLower()));
    }

    private int GetIndexForMatchedProperty(
      Dictionary<StackPanel, IEnumerable<TextBlock>> properties,
      string searchText)
    {
      return properties.Values.FirstOrDefault<IEnumerable<TextBlock>>().ToList<TextBlock>().IndexOf(this.GetMatchedProperties(properties, searchText).FirstOrDefault<TextBlock>());
    }

    private void ActivateMatchedTab(string searchText)
    {
      this.FindMatchedPanel(this.GetPanelProperties(), searchText).IfNotNullAction<StackPanel>((Action<StackPanel>) (_ => this.GetScrollViewer().FirstOrDefault<ScrollViewer>((Func<ScrollViewer, bool>) (p => p == this.FindMatchedPanel(this.GetPanelProperties(), searchText).Parent)).ParentOfType<RadTabItem>().IfNotNull<RadTabItem, bool>((Func<RadTabItem, bool>) (t => t.IsSelected = true))));
    }

    private void ScrollToPropertyIndex(string searchText)
    {
      int indexMatchedProperty = this.GetIndexForMatchedProperty(this.GetPanelProperties(), searchText);
      this.GetScrollViewer().ToList<ScrollViewer>().ForEach((Action<ScrollViewer>) (_ => _.ScrollToVerticalOffset((double) ((indexMatchedProperty - 3) * 14))));
    }

    private void SetColorForSearch(string searchText)
    {
      Dictionary<StackPanel, IEnumerable<TextBlock>> panelProperties = this.GetPanelProperties();
      this.ResetColorForProperties(panelProperties);
      this.GetMatchedProperties(panelProperties, searchText).SetTextBlockColor();
    }

    public IEnumerable<ScrollViewer> GetScrollViewer()
    {
      return Enumerable.Cast<ScrollViewer>(((Panel) this.GeneralTabItem.Content).Children).Concat<ScrollViewer>(Enumerable.Cast<ScrollViewer>(((Panel) this.Channel1Tab.Content).Children)).Concat<ScrollViewer>(Enumerable.Cast<ScrollViewer>(((Panel) this.Channel2Tab.Content).Children)).Concat<ScrollViewer>(Enumerable.Cast<ScrollViewer>(((Panel) this.Channel3Tab.Content).Children));
    }

    private void RefreshView(MeterConfigurationEvent obj)
    {
      this.BuildGrid(obj.ConfigValuesPerChannelList, obj.ComboboxCommand, obj.SelectedTab);
    }

    public void BuildGrid(
      List<ConfigurationPerChannel> configDict,
      ICommand comboboxCommand,
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
      if (configDict.Count > 0)
      {
        this.MeterConfigStackPanelDynamic.Children.Clear();
        GridControl gridControl = new GridControl();
        gridControl.Name = "GridConfigs";
        GridControl dynamicGrid1 = gridControl;
        int dynamicGrid2 = (int) DynamicGridControl.CreateDynamicGrid((IList<Config>) configDict[0].ConfigValues, out dynamicGrid1, comboboxCommand, 800.0, 30.0, hasDescriptionTextBoxes: true, addColumnForMeasureUnit: true, thirdColumnWidth: 80.0);
        this.MeterConfigStackPanelDynamic.Children.Add((UIElement) dynamicGrid1);
        this.MeterConfigStackPanelDynamic.Tag = (object) configDict[0].ConfigValues;
        // ISSUE: reference to a compiler-generated field
        if (ConfigurationUserControl.\u003C\u003Eo__13.\u003C\u003Ep__1 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ConfigurationUserControl.\u003C\u003Eo__13.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, List<Config>, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "DynamicGridTag", typeof (ConfigurationUserControl), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, List<Config>, object> target = ConfigurationUserControl.\u003C\u003Eo__13.\u003C\u003Ep__1.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, List<Config>, object>> p1 = ConfigurationUserControl.\u003C\u003Eo__13.\u003C\u003Ep__1;
        // ISSUE: reference to a compiler-generated field
        if (ConfigurationUserControl.\u003C\u003Eo__13.\u003C\u003Ep__0 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ConfigurationUserControl.\u003C\u003Eo__13.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ConfigurationParameters", typeof (ConfigurationUserControl), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj1 = ConfigurationUserControl.\u003C\u003Eo__13.\u003C\u003Ep__0.Target((CallSite) ConfigurationUserControl.\u003C\u003Eo__13.\u003C\u003Ep__0, this.DataContext);
        List<Config> configValues = configDict[0].ConfigValues;
        object obj2 = target((CallSite) p1, obj1, configValues);
      }
      if (configDict.Count > 1)
      {
        this.MeterConfigChannel1StackPanelDynamic.Children.Clear();
        GridControl gridControl = new GridControl();
        gridControl.Name = "GridConfigsChannel1";
        GridControl dynamicGrid3 = gridControl;
        int dynamicGrid4 = (int) DynamicGridControl.CreateDynamicGrid((IList<Config>) configDict[1].ConfigValues, out dynamicGrid3, gridWidth: 800.0, firstColumnPercentage: 30.0, hasDescriptionTextBoxes: true, addColumnForMeasureUnit: true, thirdColumnWidth: 80.0);
        this.MeterConfigChannel1StackPanelDynamic.Children.Add((UIElement) dynamicGrid3);
        this.MeterConfigChannel1StackPanelDynamic.Tag = (object) configDict[1].ConfigValues;
        // ISSUE: reference to a compiler-generated field
        if (ConfigurationUserControl.\u003C\u003Eo__13.\u003C\u003Ep__3 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ConfigurationUserControl.\u003C\u003Eo__13.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, List<Config>, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "Channel1DynamicGridTag", typeof (ConfigurationUserControl), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, List<Config>, object> target = ConfigurationUserControl.\u003C\u003Eo__13.\u003C\u003Ep__3.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, List<Config>, object>> p3 = ConfigurationUserControl.\u003C\u003Eo__13.\u003C\u003Ep__3;
        // ISSUE: reference to a compiler-generated field
        if (ConfigurationUserControl.\u003C\u003Eo__13.\u003C\u003Ep__2 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ConfigurationUserControl.\u003C\u003Eo__13.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ConfigurationParameters", typeof (ConfigurationUserControl), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj3 = ConfigurationUserControl.\u003C\u003Eo__13.\u003C\u003Ep__2.Target((CallSite) ConfigurationUserControl.\u003C\u003Eo__13.\u003C\u003Ep__2, this.DataContext);
        List<Config> configValues = configDict[1].ConfigValues;
        object obj4 = target((CallSite) p3, obj3, configValues);
      }
      if (configDict.Count > 2)
      {
        this.MeterConfigChannel2StackPanelDynamic.Children.Clear();
        GridControl gridControl = new GridControl();
        gridControl.Name = "GridConfigsChannel2";
        GridControl dynamicGrid5 = gridControl;
        int dynamicGrid6 = (int) DynamicGridControl.CreateDynamicGrid((IList<Config>) configDict[2].ConfigValues, out dynamicGrid5, gridWidth: 800.0, firstColumnPercentage: 30.0, hasDescriptionTextBoxes: true, addColumnForMeasureUnit: true, thirdColumnWidth: 80.0);
        this.MeterConfigChannel2StackPanelDynamic.Children.Add((UIElement) dynamicGrid5);
        this.MeterConfigChannel2StackPanelDynamic.Tag = (object) configDict[2].ConfigValues;
        // ISSUE: reference to a compiler-generated field
        if (ConfigurationUserControl.\u003C\u003Eo__13.\u003C\u003Ep__5 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ConfigurationUserControl.\u003C\u003Eo__13.\u003C\u003Ep__5 = CallSite<Func<CallSite, object, List<Config>, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "Channel2DynamicGridTag", typeof (ConfigurationUserControl), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, List<Config>, object> target = ConfigurationUserControl.\u003C\u003Eo__13.\u003C\u003Ep__5.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, List<Config>, object>> p5 = ConfigurationUserControl.\u003C\u003Eo__13.\u003C\u003Ep__5;
        // ISSUE: reference to a compiler-generated field
        if (ConfigurationUserControl.\u003C\u003Eo__13.\u003C\u003Ep__4 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ConfigurationUserControl.\u003C\u003Eo__13.\u003C\u003Ep__4 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ConfigurationParameters", typeof (ConfigurationUserControl), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj5 = ConfigurationUserControl.\u003C\u003Eo__13.\u003C\u003Ep__4.Target((CallSite) ConfigurationUserControl.\u003C\u003Eo__13.\u003C\u003Ep__4, this.DataContext);
        List<Config> configValues = configDict[2].ConfigValues;
        object obj6 = target((CallSite) p5, obj5, configValues);
      }
      if (configDict.Count <= 3)
        return;
      this.MeterConfigChannel3StackPanelDynamic.Children.Clear();
      GridControl gridControl1 = new GridControl();
      gridControl1.Name = "GridConfigsChannel3";
      GridControl dynamicGrid7 = gridControl1;
      int dynamicGrid8 = (int) DynamicGridControl.CreateDynamicGrid((IList<Config>) configDict[3].ConfigValues, out dynamicGrid7, gridWidth: 800.0, firstColumnPercentage: 30.0, hasDescriptionTextBoxes: true, addColumnForMeasureUnit: true, thirdColumnWidth: 80.0);
      this.MeterConfigChannel3StackPanelDynamic.Children.Add((UIElement) dynamicGrid7);
      this.MeterConfigChannel3StackPanelDynamic.Tag = (object) configDict[3].ConfigValues;
      // ISSUE: reference to a compiler-generated field
      if (ConfigurationUserControl.\u003C\u003Eo__13.\u003C\u003Ep__7 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ConfigurationUserControl.\u003C\u003Eo__13.\u003C\u003Ep__7 = CallSite<Func<CallSite, object, List<Config>, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "Channel3DynamicGridTag", typeof (ConfigurationUserControl), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, List<Config>, object> target1 = ConfigurationUserControl.\u003C\u003Eo__13.\u003C\u003Ep__7.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, List<Config>, object>> p7 = ConfigurationUserControl.\u003C\u003Eo__13.\u003C\u003Ep__7;
      // ISSUE: reference to a compiler-generated field
      if (ConfigurationUserControl.\u003C\u003Eo__13.\u003C\u003Ep__6 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ConfigurationUserControl.\u003C\u003Eo__13.\u003C\u003Ep__6 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ConfigurationParameters", typeof (ConfigurationUserControl), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj7 = ConfigurationUserControl.\u003C\u003Eo__13.\u003C\u003Ep__6.Target((CallSite) ConfigurationUserControl.\u003C\u003Eo__13.\u003C\u003Ep__6, this.DataContext);
      List<Config> configValues1 = configDict[3].ConfigValues;
      object obj8 = target1((CallSite) p7, obj7, configValues1);
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
        bool? isTabletMode = new bool?();
        ChangeableParameterUsings? type = new ChangeableParameterUsings?(args.Type);
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
      Application.LoadComponent((object) this, new Uri("/MSS.Client.UI.Desktop;component/view/configuration/configurationusercontrol.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.scrollViewer = (ScrollViewer) target;
          break;
        case 2:
          this.ConfigUserControl = (RadTabControl) target;
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
          this.WriteConfigurationButton = (Button) target;
          break;
        case 7:
          this.WriteConfigTextBox = (TextBlock) target;
          break;
        case 8:
          this.ExportConfigurationParametersButton = (Button) target;
          break;
        case 9:
          this.ReadValuesButton = (Button) target;
          break;
        case 10:
          this.ReadValuesTextBox = (TextBlock) target;
          break;
        case 11:
          this.UpgradeFirmwareButton = (Button) target;
          break;
        case 12:
          this.equipmentModelButton = (ToggleButton) target;
          break;
        case 13:
          this.popupEquipmentModel = (Popup) target;
          break;
        case 14:
          this.SelectedEquipmentGroupComboBox = (RadComboBox) target;
          break;
        case 15:
          this.SelectedEquipmentModelComboBox = (RadComboBox) target;
          break;
        case 16:
          this.EquipmentChangeableParametersStackPanel = (StackPanel) target;
          break;
        case 17:
          this.ProfileTypeComboBox = (RadComboBox) target;
          break;
        case 18:
          this.profileTypeButton = (ToggleButton) target;
          break;
        case 19:
          this.popupProfileType = (Popup) target;
          break;
        case 20:
          this.ProfileTypeChangeableParametersStackPanel = (StackPanel) target;
          break;
        case 21:
          this.deviceModelButton = (ToggleButton) target;
          break;
        case 22:
          this.popupDeviceModel = (Popup) target;
          break;
        case 23:
          this.DeviceModelComboBox = (RadComboBox) target;
          break;
        case 24:
          this.DeviceGroupComboBox = (RadComboBox) target;
          break;
        case 25:
          this.DeviceModelChangeableParametersStackPanel = (StackPanel) target;
          break;
        case 26:
          this.btnExpertConfiguration = (Button) target;
          break;
        case 27:
          this.ConfigTabControl = (RadTabControl) target;
          break;
        case 28:
          this.GeneralTabItem = (RadTabItem) target;
          break;
        case 29:
          this.MeterConfigStackPanelDynamic = (StackPanel) target;
          break;
        case 30:
          this.Channel1Tab = (RadTabItem) target;
          break;
        case 31:
          this.MeterConfigChannel1StackPanelDynamic = (StackPanel) target;
          break;
        case 32:
          this.Channel2Tab = (RadTabItem) target;
          break;
        case 33:
          this.MeterConfigChannel2StackPanelDynamic = (StackPanel) target;
          break;
        case 34:
          this.Channel3Tab = (RadTabItem) target;
          break;
        case 35:
          this.MeterConfigChannel3StackPanelDynamic = (StackPanel) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
