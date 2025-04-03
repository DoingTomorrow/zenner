// Decompiled with JetBrains decompiler
// Type: HandlerLib.ConfiguratorControl
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

using CommonWPF;
using GmmDbLib;
using HandlerLib.Properties;
using Microsoft.Win32;
using StartupLib;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using ZENNER.CommonLibrary;
using ZR_ClassLibrary;

#nullable disable
namespace HandlerLib
{
  public class ConfiguratorControl : UserControl, IComponentConnector
  {
    private ProgressHandler progress;
    private IHandler handler;
    private bool dailyAutosaveOn = false;
    private List<ConfigurationLevel> AllowedLevels;
    private static string lastFilePath;
    internal TabControl tabCtrl;
    internal TabItem tab0;
    internal DataGrid DataGridParameterMain;
    internal TabItem tab1;
    internal DataGrid DataGridParameterSub1;
    internal TabItem tab2;
    internal DataGrid DataGridParameterSub2;
    internal TabItem tab3;
    internal DataGrid DataGridParameterSub3;
    internal Button ButtonSave;
    internal TextBlock TextBlockSave;
    internal ComboBox ComboBoxLevel;
    internal Button ButtonLoadValuesFromFile;
    internal Button ButtonRead;
    internal Button ButtonWrite;
    internal TextBox TextBlockParameterInfo;
    internal ProgressBar ProgressBar1;
    private bool _contentLoaded;

    public ConfiguratorControl()
    {
      this.InitializeComponent();
      WpfTranslatorSupport.TranslateUserControl(Tg.DeviceConfigurator, (UserControl) this);
      this.progress = new ProgressHandler(new Action<ProgressArg>(this.OnProgress));
    }

    private void OnProgress(ProgressArg obj)
    {
      if (!this.CheckAccess())
        this.Dispatcher.Invoke((Action) (() => this.OnProgress(obj)));
      else
        this.ProgressBar1.Value = obj.ProgressPercentage;
    }

    public void InitializeComponent(IHandler handler, bool showReadWriteButton = false)
    {
      this.handler = handler != null ? handler : throw new ArgumentNullException(nameof (handler));
      this.ProgressBar1.Visibility = Visibility.Hidden;
      this.tab0.Visibility = Visibility.Hidden;
      this.tab1.Visibility = Visibility.Hidden;
      this.tab2.Visibility = Visibility.Hidden;
      this.tab3.Visibility = Visibility.Hidden;
      this.tabCtrl.Visibility = Visibility.Hidden;
      if (!showReadWriteButton)
      {
        this.ButtonRead.Visibility = Visibility.Hidden;
        this.ButtonWrite.Visibility = Visibility.Hidden;
        if (UserManager.CheckPermission("Role\\Developer"))
          this.ButtonLoadValuesFromFile.Visibility = Visibility.Visible;
      }
      this.DataGridParameterMain.Tag = (object) null;
      this.DataGridParameterSub1.Tag = (object) null;
      this.DataGridParameterSub2.Tag = (object) null;
      this.DataGridParameterSub3.Tag = (object) null;
      this.AllowedLevels = new List<ConfigurationLevel>();
      if (UserManager.CheckPermission("Right\\ConfiguratorLevel\\Standard"))
        this.AllowedLevels.Add(ConfigurationLevel.Standard);
      if (UserManager.CheckPermission("Right\\ConfiguratorLevel\\Advanced"))
        this.AllowedLevels.Add(ConfigurationLevel.Advanced);
      if (UserManager.CheckPermission("Right\\ConfiguratorLevel\\Huge"))
        this.AllowedLevels.Add(ConfigurationLevel.Huge);
      if (UserManager.CheckPermission("Right\\ConfiguratorLevel\\Native"))
        this.AllowedLevels.Add(ConfigurationLevel.Native);
      if (this.AllowedLevels.Count == 0)
      {
        this.AllowedLevels.Add(ConfigurationLevel.Standard);
        this.AllowedLevels.Add(ConfigurationLevel.Advanced);
        this.AllowedLevels.Add(ConfigurationLevel.Huge);
        this.AllowedLevels.Add(ConfigurationLevel.Native);
      }
      this.ComboBoxLevel.ItemsSource = (IEnumerable) this.AllowedLevels.ToArray();
      this.ComboBoxLevel.SelectedValue = (object) (ConfigurationLevel) Enum.Parse(typeof (ConfigurationLevel), Settings.Default.ConfiguratorLevel);
      try
      {
        this.ShowConfigurationParameter();
      }
      catch (Exception ex)
      {
        if (!showReadWriteButton)
          ExceptionViewer.Show(ex);
        ZR_ClassLibMessages.ClearErrors();
      }
      this.dailyAutosaveOn = DatabaseIdentification.GetValue(DbBasis.PrimaryDB.BaseDbConnection, "DatabaseSaveOption") == "DailyAutosaveOn";
    }

    private void ShowConfigurationParameter()
    {
      SortedList<OverrideID, ConfigurationParameter> configurationParameters1 = this.handler.GetConfigurationParameters();
      if (configurationParameters1 != null && configurationParameters1.Count > 0)
      {
        this.tabCtrl.Visibility = Visibility.Visible;
        this.tab0.Visibility = Visibility.Visible;
        this.ShowParameter(this.DataGridParameterMain, configurationParameters1);
        this.tabCtrl.SelectedIndex = 0;
      }
      SortedList<OverrideID, ConfigurationParameter> configurationParameters2 = this.handler.GetConfigurationParameters(1);
      if (configurationParameters2 != null && configurationParameters2.Count > 0)
      {
        this.tab1.Visibility = Visibility.Visible;
        this.ShowParameter(this.DataGridParameterSub1, configurationParameters2);
      }
      SortedList<OverrideID, ConfigurationParameter> configurationParameters3 = this.handler.GetConfigurationParameters(2);
      if (configurationParameters3 != null && configurationParameters3.Count > 0)
      {
        this.tab2.Visibility = Visibility.Visible;
        this.ShowParameter(this.DataGridParameterSub2, configurationParameters3);
      }
      SortedList<OverrideID, ConfigurationParameter> configurationParameters4 = this.handler.GetConfigurationParameters(3);
      if (configurationParameters4 != null && configurationParameters4.Count > 0)
      {
        this.tab3.Visibility = Visibility.Visible;
        this.ShowParameter(this.DataGridParameterSub3, configurationParameters4);
      }
      this.ButtonSave.IsEnabled = true;
      this.tabCtrl.IsEnabled = true;
    }

    private void ShowParameter(
      DataGrid grid,
      SortedList<OverrideID, ConfigurationParameter> prms)
    {
      grid.ItemsSource = (IEnumerable) null;
      grid.Tag = (object) prms;
      if (prms == null)
        return;
      List<ConfigurationParameter> orderdList = ConfigurationParameter.GetOrderdList(prms);
      AsyncObservableCollection<ConfiguratorControl.Item> observableCollection = new AsyncObservableCollection<ConfiguratorControl.Item>();
      foreach (ConfigurationParameter configParameter in orderdList)
        observableCollection.Add(new ConfiguratorControl.Item(configParameter));
      grid.ItemsSource = (IEnumerable) observableCollection;
    }

    private void RefreshParameters()
    {
      if (this.tab0.Visibility == Visibility.Visible && this.tab0.IsSelected)
      {
        this.SaveParameter(0);
        this.ShowParameter(this.DataGridParameterMain, this.handler.GetConfigurationParameters());
      }
      if (this.tab1.Visibility == Visibility.Visible && this.tab1.IsSelected)
      {
        this.SaveParameter(1);
        this.ShowParameter(this.DataGridParameterSub1, this.handler.GetConfigurationParameters(1));
      }
      if (this.tab2.Visibility == Visibility.Visible && this.tab2.IsSelected)
      {
        this.SaveParameter(2);
        this.ShowParameter(this.DataGridParameterSub2, this.handler.GetConfigurationParameters(2));
      }
      if (this.tab3.Visibility != Visibility.Visible || !this.tab3.IsSelected)
        return;
      this.SaveParameter(3);
      this.ShowParameter(this.DataGridParameterSub3, this.handler.GetConfigurationParameters(3));
    }

    private void SaveParameter(int channel)
    {
      SortedList<OverrideID, ConfigurationParameter> tag1 = this.DataGridParameterMain.Tag as SortedList<OverrideID, ConfigurationParameter>;
      SortedList<OverrideID, ConfigurationParameter> tag2 = this.DataGridParameterSub1.Tag as SortedList<OverrideID, ConfigurationParameter>;
      SortedList<OverrideID, ConfigurationParameter> tag3 = this.DataGridParameterSub2.Tag as SortedList<OverrideID, ConfigurationParameter>;
      SortedList<OverrideID, ConfigurationParameter> tag4 = this.DataGridParameterSub3.Tag as SortedList<OverrideID, ConfigurationParameter>;
      try
      {
        if (tag1 != null && channel == 0)
        {
          SortedList<OverrideID, ConfigurationParameter> parameter = this.UpdateValues(this.DataGridParameterMain);
          if (parameter != null)
            this.handler.SetConfigurationParameters(parameter);
        }
        if (tag1 != null && channel == 1)
        {
          SortedList<OverrideID, ConfigurationParameter> parameter = this.UpdateValues(this.DataGridParameterSub1);
          if (parameter != null)
            this.handler.SetConfigurationParameters(parameter, 1);
        }
        if (tag1 != null && channel == 2)
        {
          SortedList<OverrideID, ConfigurationParameter> parameter = this.UpdateValues(this.DataGridParameterSub2);
          if (parameter != null)
            this.handler.SetConfigurationParameters(parameter, 2);
        }
        if (tag1 == null || channel != 3)
          return;
        SortedList<OverrideID, ConfigurationParameter> parameter1 = this.UpdateValues(this.DataGridParameterSub3);
        if (parameter1 != null)
          this.handler.SetConfigurationParameters(parameter1, 3);
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
    }

    private SortedList<OverrideID, ConfigurationParameter> UpdateValues(DataGrid grid)
    {
      SortedList<OverrideID, ConfigurationParameter> sortedList = new SortedList<OverrideID, ConfigurationParameter>();
      SortedList<OverrideID, ConfigurationParameter> tag = grid.Tag as SortedList<OverrideID, ConfigurationParameter>;
      foreach (ConfiguratorControl.Item obj in (Collection<ConfiguratorControl.Item>) (grid.ItemsSource as AsyncObservableCollection<ConfiguratorControl.Item>))
      {
        if (obj.IsChanged)
        {
          if (!string.IsNullOrEmpty(obj.Value) || obj.Options != null)
          {
            try
            {
              tag[obj.ConfigParameter.ParameterID].Pars(obj.Value);
              sortedList.Add(obj.ConfigParameter.ParameterID, tag[obj.ConfigParameter.ParameterID]);
            }
            catch (Exception ex)
            {
              ExceptionViewer.Show(ex, obj.ToString());
              this.TextBlockParameterInfo.Text = obj.ToString() + " Error: " + ex.Message;
            }
          }
        }
      }
      return sortedList.Count == 0 ? (SortedList<OverrideID, ConfigurationParameter>) null : sortedList;
    }

    private void DataGridParameter_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (e.AddedItems.Count != 1)
        return;
      ConfiguratorControl.Item addedItem = e.AddedItems[0] as ConfiguratorControl.Item;
      this.TextBlockParameterInfo.Text = addedItem != null ? addedItem.Info : string.Empty;
    }

    private void ButtonSave_Click(object sender, RoutedEventArgs e)
    {
      this.RefreshParameters();
      this.ButtonWrite.IsEnabled = true;
    }

    private void ComboBoxLevel_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      this.DataGridParameterMain.Tag = (object) null;
      this.DataGridParameterSub1.Tag = (object) null;
      this.DataGridParameterSub2.Tag = (object) null;
      this.DataGridParameterSub3.Tag = (object) null;
      if (this.ComboBoxLevel.SelectedItem == null)
        return;
      ConfigurationParameter.ActiveConfigurationLevel = (ConfigurationLevel) this.ComboBoxLevel.SelectedItem;
      Settings.Default.ConfiguratorLevel = ((ConfigurationLevel) this.ComboBoxLevel.SelectedItem).ToString();
      Settings.Default.Save();
      this.RefreshParameters();
    }

    private async void ButtonRead_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        this.ProgressBar1.Visibility = Visibility.Visible;
        this.progress.Reset();
        this.tabCtrl.Visibility = Visibility.Hidden;
        int num = await this.handler.ReadDeviceAsync(this.progress, CancellationToken.None, ReadPartsSelection.All);
        this.ShowConfigurationParameter();
        this.DailyAutosave();
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex, "Read error");
      }
      finally
      {
        this.ProgressBar1.Visibility = Visibility.Hidden;
      }
    }

    private void DailyAutosave()
    {
      if (!this.dailyAutosaveOn)
        return;
      SortedList<OverrideID, ConfigurationParameter> configurationParameters = this.handler.GetConfigurationParameters();
      if (configurationParameters == null || !configurationParameters.ContainsKey(OverrideID.MeterID) || configurationParameters[OverrideID.MeterID].ParameterValue == null)
        return;
      try
      {
        int int32 = Convert.ToInt32(Convert.ToUInt64(configurationParameters[OverrideID.MeterID].ParameterValue));
        DateTime? nullable = GmmDbLib.MeterData.LoadLastBackupTimepoint(DbBasis.PrimaryDB.BaseDbConnection, int32);
        if (nullable.HasValue && !(nullable.Value < DateTime.Now.ToUniversalTime().Date))
          return;
        this.handler.SaveMeter();
      }
      catch (OverflowException ex)
      {
      }
    }

    private async void ButtonWrite_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        this.ProgressBar1.Visibility = Visibility.Visible;
        this.RefreshParameters();
        await this.handler.WriteDeviceAsync(this.progress, CancellationToken.None);
        this.ShowConfigurationParameter();
        DateTime? timepoint = this.handler.SaveMeter();
        this.ButtonSave.IsEnabled = false;
        this.ButtonWrite.IsEnabled = false;
        this.tabCtrl.IsEnabled = false;
        timepoint = new DateTime?();
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex, "Write error");
      }
      finally
      {
        this.ProgressBar1.Visibility = Visibility.Hidden;
        this.tab0.Visibility = Visibility.Hidden;
        this.tab1.Visibility = Visibility.Hidden;
        this.tab2.Visibility = Visibility.Hidden;
        this.tab3.Visibility = Visibility.Hidden;
        this.tabCtrl.Visibility = Visibility.Hidden;
      }
    }

    private void ButtonLoadValuesFromFile_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        OpenFileDialog openFileDialog = new OpenFileDialog();
        openFileDialog.DefaultExt = ".txt";
        openFileDialog.CheckFileExists = true;
        openFileDialog.Filter = "Text file (.txt)|*.txt|All Files|*.*";
        if (ConfiguratorControl.lastFilePath != null)
          openFileDialog.InitialDirectory = ConfiguratorControl.lastFilePath;
        bool? nullable = openFileDialog.ShowDialog();
        bool flag = true;
        if (!(nullable.GetValueOrDefault() == flag & nullable.HasValue))
          return;
        ConfiguratorControl.lastFilePath = Path.GetDirectoryName(openFileDialog.FileName);
        using (StreamReader streamReader = new StreamReader(openFileDialog.FileName))
        {
label_15:
          string[] strArray;
          do
          {
            string str = streamReader.ReadLine();
            if (str != null)
              strArray = str.Split(new char[1]{ '=' }, StringSplitOptions.RemoveEmptyEntries);
            else
              goto label_7;
          }
          while (strArray.Length != 2);
          goto label_8;
label_7:
          return;
label_8:
          for (int index = 0; index < this.DataGridParameterMain.Items.Count; ++index)
          {
            object obj1 = this.DataGridParameterMain.Items[index];
            if (obj1 is ConfiguratorControl.Item)
            {
              ConfiguratorControl.Item obj2 = (ConfiguratorControl.Item) obj1;
              if (obj2.ConfigParameter.ParameterID.ToString() == strArray[0])
              {
                obj2.Value = strArray[1];
                obj2.ConfigParameter.SetValueFromStringWin(strArray[1]);
              }
            }
          }
          goto label_15;
        }
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/HandlerLib;component/view/configuratorcontrol.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.tabCtrl = (TabControl) target;
          break;
        case 2:
          this.tab0 = (TabItem) target;
          break;
        case 3:
          this.DataGridParameterMain = (DataGrid) target;
          this.DataGridParameterMain.SelectionChanged += new SelectionChangedEventHandler(this.DataGridParameter_SelectionChanged);
          break;
        case 4:
          this.tab1 = (TabItem) target;
          break;
        case 5:
          this.DataGridParameterSub1 = (DataGrid) target;
          this.DataGridParameterSub1.SelectionChanged += new SelectionChangedEventHandler(this.DataGridParameter_SelectionChanged);
          break;
        case 6:
          this.tab2 = (TabItem) target;
          break;
        case 7:
          this.DataGridParameterSub2 = (DataGrid) target;
          this.DataGridParameterSub2.SelectionChanged += new SelectionChangedEventHandler(this.DataGridParameter_SelectionChanged);
          break;
        case 8:
          this.tab3 = (TabItem) target;
          break;
        case 9:
          this.DataGridParameterSub3 = (DataGrid) target;
          this.DataGridParameterSub3.SelectionChanged += new SelectionChangedEventHandler(this.DataGridParameter_SelectionChanged);
          break;
        case 10:
          this.ButtonSave = (Button) target;
          this.ButtonSave.Click += new RoutedEventHandler(this.ButtonSave_Click);
          break;
        case 11:
          this.TextBlockSave = (TextBlock) target;
          break;
        case 12:
          this.ComboBoxLevel = (ComboBox) target;
          this.ComboBoxLevel.SelectionChanged += new SelectionChangedEventHandler(this.ComboBoxLevel_SelectionChanged);
          break;
        case 13:
          this.ButtonLoadValuesFromFile = (Button) target;
          this.ButtonLoadValuesFromFile.Click += new RoutedEventHandler(this.ButtonLoadValuesFromFile_Click);
          break;
        case 14:
          this.ButtonRead = (Button) target;
          this.ButtonRead.Click += new RoutedEventHandler(this.ButtonRead_Click);
          break;
        case 15:
          this.ButtonWrite = (Button) target;
          this.ButtonWrite.Click += new RoutedEventHandler(this.ButtonWrite_Click);
          break;
        case 16:
          this.TextBlockParameterInfo = (TextBox) target;
          break;
        case 17:
          this.ProgressBar1 = (ProgressBar) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }

    public enum ControlType
    {
      TextBox,
      CheckBox,
      ComboBox,
      ComboBoxWithCheckBox,
    }

    public class Options : List<ConfiguratorControl.OptionItem>
    {
      public string[] Parameter { get; set; }

      public Options(string[] allowed, string[] enabled)
      {
        this.Parameter = enabled;
        List<string> stringList = new List<string>((IEnumerable<string>) enabled);
        foreach (string str in allowed)
          this.Add(new ConfiguratorControl.OptionItem()
          {
            IsChecked = stringList.Contains(str),
            Name = str
          });
      }

      public string LimmitedSelectedNames
      {
        get
        {
          string selectedNames = this.SelectedNames;
          return selectedNames.Length > 24 ? selectedNames.Substring(0, 24) + "..." : selectedNames;
        }
        set
        {
        }
      }

      public string SelectedNames
      {
        get
        {
          StringBuilder stringBuilder = new StringBuilder();
          foreach (ConfiguratorControl.OptionItem optionItem in (List<ConfiguratorControl.OptionItem>) this)
          {
            if (optionItem.IsChecked)
              stringBuilder.Append(optionItem.Name).Append(';');
          }
          return stringBuilder.ToString().TrimEnd(';');
        }
        set
        {
        }
      }
    }

    public class OptionItem : INotifyPropertyChanged
    {
      private bool isChecked;

      public string Name { get; set; }

      public bool IsChecked
      {
        get => this.isChecked;
        set
        {
          if (this.isChecked == value)
            return;
          this.isChecked = value;
          if (this.PropertyChanged != null)
            this.PropertyChanged((object) this, new PropertyChangedEventArgs(nameof (IsChecked)));
        }
      }

      public event PropertyChangedEventHandler PropertyChanged;
    }

    [DebuggerDisplay("{Parameter} = {Value}")]
    public class Item : INotifyPropertyChanged
    {
      private string value;

      public event PropertyChangedEventHandler PropertyChanged;

      public ConfigurationParameter ConfigParameter { get; set; }

      public string Parameter { get; set; }

      public string ValueOriginal { get; set; }

      public string Value
      {
        get => this.value;
        set
        {
          if (!(this.value != value))
            return;
          this.value = value;
          this.NotifyPropertyChanged("IsChanged");
        }
      }

      public string Unit { get; set; }

      public string Info { get; set; }

      public bool IsEnabled { get; set; }

      public bool IsReadonly { get; set; }

      public bool IsFunction { get; set; }

      public bool IsEditable { get; set; }

      public bool IsChanged => this.ValueOriginal != this.Value;

      public List<string> PossibleValues { get; set; }

      public ConfiguratorControl.ControlType ControlType { get; set; }

      public ConfiguratorControl.Options Options { get; set; }

      public Item(ConfigurationParameter configParameter)
      {
        this.ConfigParameter = configParameter;
        this.Parameter = EnumTranslator.GetTranslatedEnumName((object) configParameter.ParameterID);
        this.ValueOriginal = configParameter.GetStringValueWin();
        this.Value = this.ValueOriginal;
        this.Unit = configParameter.Unit;
        this.IsReadonly = !configParameter.HasWritePermission;
        this.IsEnabled = !this.IsReadonly;
        this.IsFunction = configParameter.IsFunction;
        this.IsEditable = configParameter.IsEditable;
        Type enumType = (Type) null;
        if (configParameter.ParameterValue != null)
          enumType = configParameter.ParameterValue.GetType();
        if (this.IsFunction)
          this.ControlType = ConfiguratorControl.ControlType.CheckBox;
        else if (enumType != (Type) null && enumType == typeof (bool))
          this.ControlType = ConfiguratorControl.ControlType.CheckBox;
        else if (enumType != (Type) null && enumType.IsEnum && configParameter.AllowedValues != null && configParameter.AllowedValues.Length != 0)
        {
          List<string> stringList = new List<string>((IEnumerable<string>) configParameter.AllowedValues);
          for (int index = stringList.Count - 1; index >= 0; --index)
          {
            if (!Enum.IsDefined(enumType, (object) stringList[index]))
              stringList.RemoveAt(index);
          }
          this.ControlType = ConfiguratorControl.ControlType.ComboBox;
          this.PossibleValues = stringList;
        }
        else if (enumType != (Type) null && enumType == typeof (string[]) && configParameter.ParameterValue != null && configParameter.ParameterValue.GetType() == typeof (string[]) && configParameter.AllowedValues != null && configParameter.AllowedValues.GetType() == typeof (string[]))
        {
          this.ControlType = ConfiguratorControl.ControlType.ComboBoxWithCheckBox;
          this.Options = new ConfiguratorControl.Options(configParameter.AllowedValues, (string[]) configParameter.ParameterValue);
          foreach (INotifyPropertyChanged option in (List<ConfiguratorControl.OptionItem>) this.Options)
            option.PropertyChanged += new PropertyChangedEventHandler(this.Item_PropertyChanged);
        }
        else if (enumType != (Type) null && enumType.IsEnum)
        {
          this.ControlType = ConfiguratorControl.ControlType.ComboBox;
          this.PossibleValues = new List<string>((IEnumerable<string>) Enum.GetNames(enumType));
        }
        else if (configParameter.AllowedValues != null && configParameter.AllowedValues.Length != 0)
        {
          this.ControlType = ConfiguratorControl.ControlType.ComboBox;
          this.PossibleValues = new List<string>((IEnumerable<string>) configParameter.AllowedValues);
        }
        else
          this.ControlType = ConfiguratorControl.ControlType.TextBox;
        string str = EnumTranslator.GetTranslatedEnumDescription((object) configParameter.ParameterID);
        if (configParameter.MaxParameterValue != null || configParameter.MinParameterValue != null)
        {
          str = str + Environment.NewLine + Environment.NewLine;
          if (configParameter.MinParameterValue != null)
            str = str + "min: " + configParameter.MinParameterValue.ToString() + Environment.NewLine;
          if (configParameter.MaxParameterValue != null)
            str = str + "max: " + configParameter.MaxParameterValue.ToString() + Environment.NewLine;
        }
        this.Info = str;
      }

      private void Item_PropertyChanged(object sender, PropertyChangedEventArgs e)
      {
        List<string> stringList = new List<string>();
        foreach (ConfiguratorControl.OptionItem option in (List<ConfiguratorControl.OptionItem>) this.Options)
        {
          if (option.IsChecked)
            stringList.Add(option.Name);
        }
        this.Options.Parameter = stringList.ToArray();
        this.Value = this.Options.SelectedNames;
        this.ConfigParameter.SetValueFromStringWin(this.Value);
        this.NotifyPropertyChanged("Options");
      }

      public void NotifyPropertyChanged(string propName)
      {
        if (this.PropertyChanged == null)
          return;
        this.PropertyChanged((object) this, new PropertyChangedEventArgs(propName));
      }

      public override string ToString() => this.Parameter + " = " + this.Value;
    }
  }
}
