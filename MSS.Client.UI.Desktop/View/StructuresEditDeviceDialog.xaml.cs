// Decompiled with JetBrains decompiler
// Type: MSS.Client.UI.Desktop.View.Structures.EditDeviceDialog
// Assembly: MSS.Client.UI.Desktop, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B34A4718-63B5-4C6C-93C2-0A28BCAE0F44
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Client.UI.Desktop.dll

using Microsoft.CSharp.RuntimeBinder;
using MSS.Business.DTO;
using MSS.Business.Events;
using MSS.Business.Modules.Configuration;
using MSS.Client.UI.Common;
using MSS.Client.UI.Common.Utils;
using MSS.Core.Model.Structures;
using Styles.Controls;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using Telerik.Windows.Controls;

#nullable disable
namespace MSS.Client.UI.Desktop.View.Structures
{
  public partial class EditDeviceDialog : ResizableMetroWindow, IComponentConnector
  {
    internal RadBusyIndicator BusyIndicator;
    internal ScrollViewer scrollviewer;
    internal Button ReadButton;
    internal Button WriteButton;
    internal TextBox PrimaryAddressTextBox;
    internal TextBox InputNumberTextBox;
    internal RadMaskedNumericInput radMaskedNumericInput;
    internal ToggleButton deviceModelButton;
    internal Popup popupDeviceModel;
    internal RadTabItem GeneralTabItem;
    internal StackPanel MeterConfigStackPanelDynamic;
    internal StackPanel MeterConfigChannel1StackPanelDynamic;
    internal StackPanel MeterConfigChannel2StackPanelDynamic;
    internal StackPanel MeterConfigChannel3StackPanelDynamic;
    internal RadGridView MeterReplacementHistoryGridView;
    internal RadGridView DeviceInfoGridView;
    internal Button btnImportTranslationRules;
    internal TextBlock textBlockImportTranslationRules;
    internal Button CreateEditRuleButton;
    internal Button ReplaceMeterButton;
    internal Button SaveAndCreateNewButton;
    internal Button AddButton;
    internal Button EditButton;
    internal Button CancelButton;
    private bool _contentLoaded;

    public EditDeviceDialog(StructureNodeDTO node)
    {
      this.InitializeComponent();
      this.SourceInitialized += new EventHandler(((ResizableMetroWindow) this).win_SourceInitialized);
      Windows8Palette.Palette.AccentColor = Color.FromRgb((byte) 190, (byte) 190, (byte) 190);
      this.Loaded += new RoutedEventHandler(this.OnLoaded);
      this.MouseLeftButtonUp += new MouseButtonEventHandler(((ResizableMetroWindow) this).OnMouseLeftButtonUp);
      this.StateChanged += new EventHandler(((ResizableMetroWindow) this).OnStateChanged);
      this.Deactivated += new EventHandler(((ResizableMetroWindow) this).Window_Deactivated);
      EventPublisher.Register<MeterConfigurationEvent>(new Action<MeterConfigurationEvent>(this.RefreshView));
    }

    ~EditDeviceDialog()
    {
      this.SourceInitialized += new EventHandler(((ResizableMetroWindow) this).win_SourceInitialized);
      this.Loaded -= new RoutedEventHandler(this.OnLoaded);
      this.MouseLeftButtonUp -= new MouseButtonEventHandler(((ResizableMetroWindow) this).OnMouseLeftButtonUp);
      this.StateChanged -= new EventHandler(((ResizableMetroWindow) this).OnStateChanged);
      this.Deactivated -= new EventHandler(((ResizableMetroWindow) this).Window_Deactivated);
      this.PrimaryAddressTextBox.PreviewTextInput -= new TextCompositionEventHandler(this.NumericOnly);
    }

    private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
    {
      // ISSUE: reference to a compiler-generated field
      if (EditDeviceDialog.\u003C\u003Eo__2.\u003C\u003Ep__2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        EditDeviceDialog.\u003C\u003Eo__2.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (EditDeviceDialog), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target1 = EditDeviceDialog.\u003C\u003Eo__2.\u003C\u003Ep__2.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p2 = EditDeviceDialog.\u003C\u003Eo__2.\u003C\u003Ep__2;
      // ISSUE: reference to a compiler-generated field
      if (EditDeviceDialog.\u003C\u003Eo__2.\u003C\u003Ep__1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        EditDeviceDialog.\u003C\u003Eo__2.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, StructureTypeEnum, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof (EditDeviceDialog), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, StructureTypeEnum, object> target2 = EditDeviceDialog.\u003C\u003Eo__2.\u003C\u003Ep__1.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, StructureTypeEnum, object>> p1 = EditDeviceDialog.\u003C\u003Eo__2.\u003C\u003Ep__1;
      // ISSUE: reference to a compiler-generated field
      if (EditDeviceDialog.\u003C\u003Eo__2.\u003C\u003Ep__0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        EditDeviceDialog.\u003C\u003Eo__2.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "StructureType", typeof (EditDeviceDialog), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj1 = EditDeviceDialog.\u003C\u003Eo__2.\u003C\u003Ep__0.Target((CallSite) EditDeviceDialog.\u003C\u003Eo__2.\u003C\u003Ep__0, this.DataContext);
      object obj2 = target2((CallSite) p1, obj1, StructureTypeEnum.Fixed);
      if (!target1((CallSite) p2, obj2))
        return;
      // ISSUE: reference to a compiler-generated field
      if (EditDeviceDialog.\u003C\u003Eo__2.\u003C\u003Ep__3 == null)
      {
        // ISSUE: reference to a compiler-generated field
        EditDeviceDialog.\u003C\u003Eo__2.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ConfigValuesCollection", typeof (EditDeviceDialog), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj3 = EditDeviceDialog.\u003C\u003Eo__2.\u003C\u003Ep__3.Target((CallSite) EditDeviceDialog.\u003C\u003Eo__2.\u003C\u003Ep__3, this.DataContext);
      // ISSUE: reference to a compiler-generated field
      if (EditDeviceDialog.\u003C\u003Eo__2.\u003C\u003Ep__8 == null)
      {
        // ISSUE: reference to a compiler-generated field
        EditDeviceDialog.\u003C\u003Eo__2.\u003C\u003Ep__8 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (EditDeviceDialog), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target3 = EditDeviceDialog.\u003C\u003Eo__2.\u003C\u003Ep__8.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> p8 = EditDeviceDialog.\u003C\u003Eo__2.\u003C\u003Ep__8;
      // ISSUE: reference to a compiler-generated field
      if (EditDeviceDialog.\u003C\u003Eo__2.\u003C\u003Ep__4 == null)
      {
        // ISSUE: reference to a compiler-generated field
        EditDeviceDialog.\u003C\u003Eo__2.\u003C\u003Ep__4 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof (EditDeviceDialog), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj4 = EditDeviceDialog.\u003C\u003Eo__2.\u003C\u003Ep__4.Target((CallSite) EditDeviceDialog.\u003C\u003Eo__2.\u003C\u003Ep__4, obj3, (object) null);
      // ISSUE: reference to a compiler-generated field
      if (EditDeviceDialog.\u003C\u003Eo__2.\u003C\u003Ep__7 == null)
      {
        // ISSUE: reference to a compiler-generated field
        EditDeviceDialog.\u003C\u003Eo__2.\u003C\u003Ep__7 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsFalse, typeof (EditDeviceDialog), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      object obj5;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (!EditDeviceDialog.\u003C\u003Eo__2.\u003C\u003Ep__7.Target((CallSite) EditDeviceDialog.\u003C\u003Eo__2.\u003C\u003Ep__7, obj4))
      {
        // ISSUE: reference to a compiler-generated field
        if (EditDeviceDialog.\u003C\u003Eo__2.\u003C\u003Ep__6 == null)
        {
          // ISSUE: reference to a compiler-generated field
          EditDeviceDialog.\u003C\u003Eo__2.\u003C\u003Ep__6 = CallSite<Func<CallSite, object, bool, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.BinaryOperationLogical, ExpressionType.And, typeof (EditDeviceDialog), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, bool, object> target4 = EditDeviceDialog.\u003C\u003Eo__2.\u003C\u003Ep__6.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, bool, object>> p6 = EditDeviceDialog.\u003C\u003Eo__2.\u003C\u003Ep__6;
        object obj6 = obj4;
        // ISSUE: reference to a compiler-generated field
        if (EditDeviceDialog.\u003C\u003Eo__2.\u003C\u003Ep__5 == null)
        {
          // ISSUE: reference to a compiler-generated field
          EditDeviceDialog.\u003C\u003Eo__2.\u003C\u003Ep__5 = CallSite<Func<CallSite, object, List<ConfigurationPerChannel>>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof (List<ConfigurationPerChannel>), typeof (EditDeviceDialog)));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        int num = EditDeviceDialog.\u003C\u003Eo__2.\u003C\u003Ep__5.Target((CallSite) EditDeviceDialog.\u003C\u003Eo__2.\u003C\u003Ep__5, obj3).Any<ConfigurationPerChannel>() ? 1 : 0;
        obj5 = target4((CallSite) p6, obj6, num != 0);
      }
      else
        obj5 = obj4;
      if (target3((CallSite) p8, obj5))
      {
        // ISSUE: reference to a compiler-generated field
        if (EditDeviceDialog.\u003C\u003Eo__2.\u003C\u003Ep__9 == null)
        {
          // ISSUE: reference to a compiler-generated field
          EditDeviceDialog.\u003C\u003Eo__2.\u003C\u003Ep__9 = CallSite<Action<CallSite, EditDeviceDialog, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.InvokeSimpleName | CSharpBinderFlags.ResultDiscarded, "BuildGrid", (IEnumerable<Type>) null, typeof (EditDeviceDialog), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        EditDeviceDialog.\u003C\u003Eo__2.\u003C\u003Ep__9.Target((CallSite) EditDeviceDialog.\u003C\u003Eo__2.\u003C\u003Ep__9, this, obj3);
      }
    }

    private void RefreshView(MeterConfigurationEvent obj)
    {
      this.BuildGrid(obj.ConfigValuesPerChannelList);
    }

    public void BuildGrid(List<ConfigurationPerChannel> configDict)
    {
      this.MeterConfigStackPanelDynamic.Tag = (object) null;
      this.MeterConfigStackPanelDynamic.Children.Clear();
      this.MeterConfigChannel1StackPanelDynamic.Tag = (object) null;
      this.MeterConfigChannel1StackPanelDynamic.Children.Clear();
      this.MeterConfigChannel2StackPanelDynamic.Tag = (object) null;
      this.MeterConfigChannel2StackPanelDynamic.Children.Clear();
      this.MeterConfigChannel3StackPanelDynamic.Tag = (object) null;
      this.MeterConfigChannel3StackPanelDynamic.Children.Clear();
      this.GeneralTabItem.IsSelected = true;
      if (configDict.Count > 0)
      {
        this.MeterConfigStackPanelDynamic.Children.Clear();
        GridControl gridControl = new GridControl();
        gridControl.Name = "GridConfigs";
        GridControl dynamicGrid1 = gridControl;
        int dynamicGrid2 = (int) DynamicGridControl.CreateDynamicGrid((IList<Config>) configDict[0].ConfigValues, out dynamicGrid1, gridWidth: 840.0, firstColumnPercentage: 20.0);
        this.MeterConfigStackPanelDynamic.Children.Add((UIElement) dynamicGrid1);
        this.MeterConfigStackPanelDynamic.Tag = (object) configDict[0].ConfigValues;
        // ISSUE: reference to a compiler-generated field
        if (EditDeviceDialog.\u003C\u003Eo__4.\u003C\u003Ep__0 == null)
        {
          // ISSUE: reference to a compiler-generated field
          EditDeviceDialog.\u003C\u003Eo__4.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, List<Config>, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "DynamicGridTag", typeof (EditDeviceDialog), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj = EditDeviceDialog.\u003C\u003Eo__4.\u003C\u003Ep__0.Target((CallSite) EditDeviceDialog.\u003C\u003Eo__4.\u003C\u003Ep__0, this.DataContext, configDict[0].ConfigValues);
      }
      if (configDict.Count > 1)
      {
        this.MeterConfigChannel1StackPanelDynamic.Children.Clear();
        GridControl gridControl = new GridControl();
        gridControl.Name = "GridConfigsChannel1";
        GridControl dynamicGrid3 = gridControl;
        int dynamicGrid4 = (int) DynamicGridControl.CreateDynamicGrid((IList<Config>) configDict[1].ConfigValues, out dynamicGrid3, gridWidth: 840.0, firstColumnPercentage: 20.0);
        this.MeterConfigChannel1StackPanelDynamic.Children.Add((UIElement) dynamicGrid3);
        this.MeterConfigChannel1StackPanelDynamic.Tag = (object) configDict[1].ConfigValues;
        // ISSUE: reference to a compiler-generated field
        if (EditDeviceDialog.\u003C\u003Eo__4.\u003C\u003Ep__1 == null)
        {
          // ISSUE: reference to a compiler-generated field
          EditDeviceDialog.\u003C\u003Eo__4.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, List<Config>, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "Channel1DynamicGridTag", typeof (EditDeviceDialog), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj = EditDeviceDialog.\u003C\u003Eo__4.\u003C\u003Ep__1.Target((CallSite) EditDeviceDialog.\u003C\u003Eo__4.\u003C\u003Ep__1, this.DataContext, configDict[1].ConfigValues);
      }
      if (configDict.Count > 2)
      {
        this.MeterConfigChannel2StackPanelDynamic.Children.Clear();
        GridControl gridControl = new GridControl();
        gridControl.Name = "GridConfigsChannel2";
        GridControl dynamicGrid5 = gridControl;
        int dynamicGrid6 = (int) DynamicGridControl.CreateDynamicGrid((IList<Config>) configDict[2].ConfigValues, out dynamicGrid5, gridWidth: 840.0, firstColumnPercentage: 20.0);
        this.MeterConfigChannel2StackPanelDynamic.Children.Add((UIElement) dynamicGrid5);
        this.MeterConfigChannel2StackPanelDynamic.Tag = (object) configDict[2].ConfigValues;
        // ISSUE: reference to a compiler-generated field
        if (EditDeviceDialog.\u003C\u003Eo__4.\u003C\u003Ep__2 == null)
        {
          // ISSUE: reference to a compiler-generated field
          EditDeviceDialog.\u003C\u003Eo__4.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, List<Config>, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "Channel2DynamicGridTag", typeof (EditDeviceDialog), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj = EditDeviceDialog.\u003C\u003Eo__4.\u003C\u003Ep__2.Target((CallSite) EditDeviceDialog.\u003C\u003Eo__4.\u003C\u003Ep__2, this.DataContext, configDict[2].ConfigValues);
      }
      if (configDict.Count <= 3)
        return;
      this.MeterConfigChannel3StackPanelDynamic.Children.Clear();
      GridControl gridControl1 = new GridControl();
      gridControl1.Name = "GridConfigsChannel3";
      GridControl dynamicGrid7 = gridControl1;
      int dynamicGrid8 = (int) DynamicGridControl.CreateDynamicGrid((IList<Config>) configDict[3].ConfigValues, out dynamicGrid7, gridWidth: 840.0, firstColumnPercentage: 20.0);
      this.MeterConfigChannel3StackPanelDynamic.Children.Add((UIElement) dynamicGrid7);
      this.MeterConfigChannel3StackPanelDynamic.Tag = (object) configDict[3].ConfigValues;
      // ISSUE: reference to a compiler-generated field
      if (EditDeviceDialog.\u003C\u003Eo__4.\u003C\u003Ep__3 == null)
      {
        // ISSUE: reference to a compiler-generated field
        EditDeviceDialog.\u003C\u003Eo__4.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, List<Config>, object>>.Create(Binder.SetMember(CSharpBinderFlags.None, "Channel3DynamicGridTag", typeof (EditDeviceDialog), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj1 = EditDeviceDialog.\u003C\u003Eo__4.\u003C\u003Ep__3.Target((CallSite) EditDeviceDialog.\u003C\u003Eo__4.\u003C\u003Ep__3, this.DataContext, configDict[3].ConfigValues);
    }

    private void NumericOnly(object sender, TextCompositionEventArgs e)
    {
      e.Handled = EditDeviceDialog.IsTextNumeric(e.Text);
    }

    private static bool IsTextNumeric(string str) => new Regex("[^0-9]").IsMatch(str);

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/MSS.Client.UI.Desktop;component/view/structures/editdevicedialog.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.BusyIndicator = (RadBusyIndicator) target;
          break;
        case 2:
          this.scrollviewer = (ScrollViewer) target;
          break;
        case 3:
          this.ReadButton = (Button) target;
          break;
        case 4:
          this.WriteButton = (Button) target;
          break;
        case 5:
          this.PrimaryAddressTextBox = (TextBox) target;
          this.PrimaryAddressTextBox.PreviewTextInput += new TextCompositionEventHandler(this.NumericOnly);
          break;
        case 6:
          this.InputNumberTextBox = (TextBox) target;
          break;
        case 7:
          this.radMaskedNumericInput = (RadMaskedNumericInput) target;
          break;
        case 8:
          this.deviceModelButton = (ToggleButton) target;
          break;
        case 9:
          this.popupDeviceModel = (Popup) target;
          break;
        case 10:
          this.GeneralTabItem = (RadTabItem) target;
          break;
        case 11:
          this.MeterConfigStackPanelDynamic = (StackPanel) target;
          break;
        case 12:
          this.MeterConfigChannel1StackPanelDynamic = (StackPanel) target;
          break;
        case 13:
          this.MeterConfigChannel2StackPanelDynamic = (StackPanel) target;
          break;
        case 14:
          this.MeterConfigChannel3StackPanelDynamic = (StackPanel) target;
          break;
        case 15:
          this.MeterReplacementHistoryGridView = (RadGridView) target;
          break;
        case 16:
          this.DeviceInfoGridView = (RadGridView) target;
          break;
        case 17:
          this.btnImportTranslationRules = (Button) target;
          break;
        case 18:
          this.textBlockImportTranslationRules = (TextBlock) target;
          break;
        case 19:
          this.CreateEditRuleButton = (Button) target;
          break;
        case 20:
          this.ReplaceMeterButton = (Button) target;
          break;
        case 21:
          this.SaveAndCreateNewButton = (Button) target;
          break;
        case 22:
          this.AddButton = (Button) target;
          break;
        case 23:
          this.EditButton = (Button) target;
          break;
        case 24:
          this.CancelButton = (Button) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
