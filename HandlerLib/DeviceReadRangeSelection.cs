// Decompiled with JetBrains decompiler
// Type: HandlerLib.DeviceReadRangeSelection
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

#nullable disable
namespace HandlerLib
{
  public class DeviceReadRangeSelection : Window, IComponentConnector
  {
    private static List<DeviceReadRangeSelection.RangeInfos> _rangeInfos = new List<DeviceReadRangeSelection.RangeInfos>();
    internal Button BottonOk;
    internal StackPanel StackPanelProtocolOnly;
    internal CheckBox CheckBoxProtocolOnlyMode;
    internal StackPanel StackPanelSetup;
    private bool _contentLoaded;

    static DeviceReadRangeSelection()
    {
      DeviceReadRangeSelection._rangeInfos.Add(new DeviceReadRangeSelection.RangeInfos(ReadPartsSelection.RAM_range, "RAM", "All defined RAM ranges"));
      DeviceReadRangeSelection._rangeInfos.Add(new DeviceReadRangeSelection.RangeInfos(ReadPartsSelection.FLASH_range, "FLASH", "All defined FLASH ranges"));
      DeviceReadRangeSelection._rangeInfos.Add(new DeviceReadRangeSelection.RangeInfos(ReadPartsSelection.BACKUP_range, "BACKUP", "Ranges to write to the data base as complete backup"));
      DeviceReadRangeSelection._rangeInfos.Add(new DeviceReadRangeSelection.RangeInfos(ReadPartsSelection.CLONE_range, "CLONE", "Parts of the BACKUP ranges which are used to write to the device by cloning"));
      DeviceReadRangeSelection._rangeInfos.Add(new DeviceReadRangeSelection.RangeInfos(ReadPartsSelection.FirmwareVersion, "FirmwareVersion", "Us only get version protocol to read the identification of a device."));
      DeviceReadRangeSelection._rangeInfos.Add(new DeviceReadRangeSelection.RangeInfos(ReadPartsSelection.Identification, "Identification", "All given identification data of device and used types."));
      DeviceReadRangeSelection._rangeInfos.Add(new DeviceReadRangeSelection.RangeInfos(ReadPartsSelection.EnhancedIdentification, "EnhancedIdentification", "Additional identifications like compiler versions and chip id's, "));
      DeviceReadRangeSelection._rangeInfos.Add(new DeviceReadRangeSelection.RangeInfos(ReadPartsSelection.BasicConfiguration, "BasicConfiguration", "Most used configuration data"));
      DeviceReadRangeSelection._rangeInfos.Add(new DeviceReadRangeSelection.RangeInfos(ReadPartsSelection.Calibration, "Calibration", "Calibaration part of configuration"));
      DeviceReadRangeSelection._rangeInfos.Add(new DeviceReadRangeSelection.RangeInfos(ReadPartsSelection.CurrentMeasurementValues, "CurrentMeasurementValues", "Current accumulated and measured values."));
      DeviceReadRangeSelection._rangeInfos.Add(new DeviceReadRangeSelection.RangeInfos(ReadPartsSelection.LoggersMask, "AllLoggers", "Key date values, month values and other logger values"));
      DeviceReadRangeSelection._rangeInfos.Add(new DeviceReadRangeSelection.RangeInfos(ReadPartsSelection.KeyData, "KeyData", "Key date storages and loggers"));
      DeviceReadRangeSelection._rangeInfos.Add(new DeviceReadRangeSelection.RangeInfos(ReadPartsSelection.MonthLogger, "MonthLogger", "Month logger storages"));
      DeviceReadRangeSelection._rangeInfos.Add(new DeviceReadRangeSelection.RangeInfos(ReadPartsSelection.SmartFunctionLoggers, "SmartFunctionLoggers", "SmartFunctionLoggerStorages"));
      DeviceReadRangeSelection._rangeInfos.Add(new DeviceReadRangeSelection.RangeInfos(ReadPartsSelection.SmartFunctions, "SmartFunctions", "Smart function memory ranges"));
      DeviceReadRangeSelection._rangeInfos.Add(new DeviceReadRangeSelection.RangeInfos(ReadPartsSelection.ScenarioConfiguration, "ScenarioConfiguration", "Communication scenario configuration memory range"));
      DeviceReadRangeSelection._rangeInfos.Add(new DeviceReadRangeSelection.RangeInfos(ReadPartsSelection.BackupBlocks, "BackupBlocks", "Firmware internal data backups"));
      DeviceReadRangeSelection._rangeInfos.Add(new DeviceReadRangeSelection.RangeInfos(ReadPartsSelection.RamDiagnosticParameters, "RamDiagnosticParameters", ""));
      DeviceReadRangeSelection._rangeInfos.Add(new DeviceReadRangeSelection.RangeInfos(ReadPartsSelection.ProtocolOnlyMode, "ProtocolOnlyMode", "No memory access used."));
    }

    public static void DefineReadPartsSelections(
      ReadPartsSelection supportedSelections,
      ref ReadPartsSelection selection,
      Window owner)
    {
      DeviceReadRangeSelection readRangeSelection = new DeviceReadRangeSelection(supportedSelections, selection);
      readRangeSelection.Owner = owner;
      if (!readRangeSelection.ShowDialog().Value)
        return;
      selection = readRangeSelection.GetSelection();
    }

    internal DeviceReadRangeSelection(
      ReadPartsSelection supportedSelections,
      ReadPartsSelection selection)
    {
      this.InitializeComponent();
      ReadPartsSelection readPartsSelection = supportedSelections & ReadPartsSelection.Dump;
      foreach (DeviceReadRangeSelection.RangeInfos rangeInfo in DeviceReadRangeSelection._rangeInfos)
      {
        if ((rangeInfo.SelectBits & readPartsSelection) == rangeInfo.SelectBits)
        {
          DockPanel element1 = new DockPanel();
          Separator element2 = new Separator();
          DockPanel.SetDock((UIElement) element2, Dock.Top);
          element1.Children.Add((UIElement) element2);
          StackPanel element3 = new StackPanel();
          element3.Orientation = Orientation.Horizontal;
          element3.VerticalAlignment = VerticalAlignment.Top;
          DockPanel.SetDock((UIElement) element3, Dock.Left);
          element1.Children.Add((UIElement) element3);
          CheckBox element4 = new CheckBox();
          element4.Tag = (object) rangeInfo.SelectBits;
          if ((selection & rangeInfo.SelectBits) == rangeInfo.SelectBits)
            element4.IsChecked = new bool?(true);
          element3.Children.Add((UIElement) element4);
          element3.Children.Add((UIElement) new TextBlock()
          {
            Text = rangeInfo.ShortDescription
          });
          element1.Children.Add((UIElement) new TextBlock()
          {
            TextWrapping = TextWrapping.WrapWithOverflow,
            Text = rangeInfo.Description
          });
          this.StackPanelSetup.Children.Add((UIElement) element1);
        }
      }
      if ((supportedSelections & ReadPartsSelection.ProtocolOnlyMode) == ~(ReadPartsSelection.Dump | ReadPartsSelection.ProtocolOnlyMode))
        this.StackPanelProtocolOnly.Visibility = Visibility.Hidden;
      else
        this.CheckBoxProtocolOnlyMode.IsChecked = new bool?((selection & ReadPartsSelection.ProtocolOnlyMode) > ~(ReadPartsSelection.Dump | ReadPartsSelection.ProtocolOnlyMode));
    }

    private void BottonOk_Click(object sender, RoutedEventArgs e)
    {
      this.DialogResult = new bool?(true);
      this.Close();
    }

    internal ReadPartsSelection GetSelection()
    {
      ReadPartsSelection selection = ~(ReadPartsSelection.Dump | ReadPartsSelection.ProtocolOnlyMode);
      this.AddSelectedBits(ref selection, this.StackPanelSetup.Children);
      if (this.StackPanelProtocolOnly.Visibility != Visibility.Hidden)
      {
        bool? isChecked = this.CheckBoxProtocolOnlyMode.IsChecked;
        bool flag = true;
        if (isChecked.GetValueOrDefault() == flag & isChecked.HasValue)
          selection |= ReadPartsSelection.ProtocolOnlyMode;
      }
      return selection;
    }

    private void AddSelectedBits(ref ReadPartsSelection selection, UIElementCollection childs)
    {
      foreach (object child in childs)
      {
        if (child is CheckBox)
        {
          CheckBox checkBox = (CheckBox) child;
          if (!checkBox.IsChecked.Value)
            break;
          selection |= (ReadPartsSelection) checkBox.Tag;
          break;
        }
        if (child is DockPanel)
          this.AddSelectedBits(ref selection, ((Panel) child).Children);
        if (child is StackPanel)
        {
          this.AddSelectedBits(ref selection, ((Panel) child).Children);
          break;
        }
      }
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/HandlerLib;component/view/devicereadrangeselection.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.BottonOk = (Button) target;
          this.BottonOk.Click += new RoutedEventHandler(this.BottonOk_Click);
          break;
        case 2:
          this.StackPanelProtocolOnly = (StackPanel) target;
          break;
        case 3:
          this.CheckBoxProtocolOnlyMode = (CheckBox) target;
          break;
        case 4:
          this.StackPanelSetup = (StackPanel) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }

    internal class RangeInfos
    {
      internal ReadPartsSelection SelectBits;
      internal string ShortDescription;
      internal string Description;

      internal RangeInfos(
        ReadPartsSelection selectBit,
        string shortDescription,
        string description)
      {
        this.SelectBits = selectBit;
        this.ShortDescription = shortDescription;
        this.Description = description;
      }
    }
  }
}
