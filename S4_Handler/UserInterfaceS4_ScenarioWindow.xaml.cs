// Decompiled with JetBrains decompiler
// Type: S4_Handler.UserInterface.S4_ScenarioWindow
// Assembly: S4_Handler, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 6FB3FA3B-A643-4E86-9555-EAB58D0F89E2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S4_Handler.dll

using CommonWPF;
using Microsoft.Win32;
using S4_Handler.Functions;
using StartupLib;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using ZENNER.CommonLibrary;
using ZR_ClassLibrary;

#nullable disable
namespace S4_Handler.UserInterface
{
  public partial class S4_ScenarioWindow : Window, IComponentConnector
  {
    private bool IsPluginObject;
    private bool IsDeviceConnected;
    private S4_ScenarioManager ScenarioManager;
    private Cursor defaultCursor;
    private CancellationToken cancelToken;
    private ProgressHandler progress;
    internal TabControl TabControlButtons;
    internal TabItem TabItemDevice;
    internal ComboBox ComboBoxScenarioNumber;
    internal Button ButtonReadScenarioListFromDevice;
    internal Button ButtonSendScenarioConfig;
    internal Button ButtonDeleteScenarioConfig;
    internal Button ButtonDeleteScenarioRange;
    internal CheckBox CheckBoxReadOnlySetup;
    internal Button ButtonReadScenarioConfig;
    internal Button ButtonDeleteAllConfigurationsFromDevice;
    internal Button ButtonWriteAllPreparedConfigurations;
    internal Button ButtonCompareAllLoadedConfigurations;
    internal TabItem TabItemMap;
    internal Button ButtonReadMap;
    internal Button ButtonShowScenarioListFromMap;
    internal ComboBox ComboBoxMapScenarios;
    internal Button ButtonShowSelectedFromMap;
    internal TabItem TabItemPrepared;
    internal Button ButtonLoadAllScenarioFromoDatabase;
    internal Button ButtonShowPreparedScenarios;
    internal Button ButtonDeleteAllPreparedScenarios;
    internal Button ButtonAddScenarioFromFile;
    internal Button ButtonAddScenarioFromFileToDatabase;
    internal ComboBox ComboBoxPreparedScenarios;
    internal Button ButtonShowScenario;
    internal Button ButtonUnloadPreparedScenario;
    internal Button ButtonComparePreparedScenario;
    internal Button ButtonCopyPreparedScenario;
    internal Button ButtonCheckChangedScenarios;
    internal Button ButtonCopyChangedScenarios;
    internal Button ButtonShowTimeManagement;
    internal GroupBox GroupBoxScenarioCommands;
    internal TextBox TextBoxScenario;
    internal Button ButtonScenarioSendToFromDevice;
    internal Button ButtonScenarioReadFromDevice;
    internal Button ButtonSetGroups;
    internal TextBox TextBoxModuleCode;
    internal ComboBox ComboBoxModulOptionSend;
    internal ComboBox ComboBoxModulOptionRead;
    internal TextBox TextBoxScenarioInternal;
    internal Button ButtonScenarioSendToDeviceInternal;
    internal Button ButtonScenarioReadFromDeviceInternal;
    internal TextBox TextBoxOut;
    private bool _contentLoaded;

    internal S4_ScenarioWindow(
      S4_ScenarioManager scenarioManager,
      bool isDeviceConnected,
      bool isPluginObject)
    {
      this.ScenarioManager = scenarioManager;
      this.IsPluginObject = isPluginObject;
      this.IsDeviceConnected = isDeviceConnected;
      this.InitializeComponent();
      this.defaultCursor = this.Cursor;
      try
      {
        string str1 = PlugInLoader.GmmConfiguration.GetValue("S4_Handler", S4_HandlerWindowFunctions.ConfigVariables.Scenario.ToString());
        if (!string.IsNullOrEmpty(str1))
          this.TextBoxScenario.Text = str1;
        string str2 = PlugInLoader.GmmConfiguration.GetValue("S4_Handler", S4_HandlerWindowFunctions.ConfigVariables.ScenarioInternal.ToString());
        if (!string.IsNullOrEmpty(str2))
          this.TextBoxScenarioInternal.Text = str2;
        PlugInLoader.GmmConfiguration.GetValue("S4_Handler", S4_HandlerWindowFunctions.ConfigVariables.ScenarioModule.ToString());
        string str3 = PlugInLoader.GmmConfiguration.GetValue("S4_Handler", S4_HandlerWindowFunctions.ConfigVariables.ScenarioNumber.ToString());
        if (!string.IsNullOrEmpty(str3))
          this.ComboBoxScenarioNumber.Text = str3;
        string str4 = PlugInLoader.GmmConfiguration.GetValue("S4_Handler", S4_HandlerWindowFunctions.ConfigVariables.ModuleCode.ToString());
        if (!string.IsNullOrEmpty(str4))
          this.TextBoxModuleCode.Text = str4;
      }
      catch
      {
      }
      this.progress = new ProgressHandler(new Action<ProgressArg>(this.OnProgress));
      this.SetStopState();
    }

    private void Window_Closing(object sender, CancelEventArgs e)
    {
      if (!this.IsPluginObject)
        return;
      GMMConfig gmmConfiguration1 = PlugInLoader.GmmConfiguration;
      S4_HandlerWindowFunctions.ConfigVariables configVariables = S4_HandlerWindowFunctions.ConfigVariables.Scenario;
      string strVariable1 = configVariables.ToString();
      string text1 = this.TextBoxScenario.Text;
      gmmConfiguration1.SetOrUpdateValue("S4_Handler", strVariable1, text1);
      GMMConfig gmmConfiguration2 = PlugInLoader.GmmConfiguration;
      configVariables = S4_HandlerWindowFunctions.ConfigVariables.ScenarioInternal;
      string strVariable2 = configVariables.ToString();
      string text2 = this.TextBoxScenarioInternal.Text;
      gmmConfiguration2.SetOrUpdateValue("S4_Handler", strVariable2, text2);
      GMMConfig gmmConfiguration3 = PlugInLoader.GmmConfiguration;
      configVariables = S4_HandlerWindowFunctions.ConfigVariables.ScenarioNumber;
      string strVariable3 = configVariables.ToString();
      string text3 = this.ComboBoxScenarioNumber.Text;
      gmmConfiguration3.SetOrUpdateValue("S4_Handler", strVariable3, text3);
      GMMConfig gmmConfiguration4 = PlugInLoader.GmmConfiguration;
      configVariables = S4_HandlerWindowFunctions.ConfigVariables.ModuleCode;
      string strVariable4 = configVariables.ToString();
      string text4 = this.TextBoxModuleCode.Text;
      gmmConfiguration4.SetOrUpdateValue("S4_Handler", strVariable4, text4);
    }

    private void OnProgress(ProgressArg obj)
    {
    }

    private void SetRunState()
    {
      this.cancelToken = new CancellationTokenSource().Token;
      this.progress.Reset();
      this.progress.Split(new double[2]{ 2.0, 98.0 });
      this.progress.Report("Run");
      if (this.Cursor == Cursors.Wait)
        return;
      this.Cursor = Cursors.Wait;
    }

    private void SetStopState()
    {
      this.Cursor = this.defaultCursor;
      this.progress.Reset();
      if (!this.IsDeviceConnected)
      {
        this.GroupBoxScenarioCommands.IsEnabled = false;
        this.TabItemDevice.IsEnabled = false;
        this.TabItemMap.IsEnabled = false;
        this.TabItemPrepared.IsSelected = true;
      }
      else
      {
        if (this.ScenarioManager.myDeviceMemory != null)
          return;
        this.TabItemMap.IsEnabled = false;
      }
    }

    private async void ButtonScenarioSendToDeviceByOptions_Click(object sender, RoutedEventArgs e)
    {
      byte ModuleOption = 0;
      ushort Scenario = 0;
      try
      {
        this.SetRunState();
        ModuleOption = this.GetModuleOption(this.ComboBoxModulOptionSend.SelectedIndex);
        Scenario = ushort.Parse(this.TextBoxScenarioInternal.Text);
        await this.ScenarioManager.WriteCommunicationScenario(this.progress, this.cancelToken, Scenario, new byte?(ModuleOption));
        this.TextBoxOut.Text = "Send scenario by options done" + Environment.NewLine + "Option: 0x" + ModuleOption.ToString("x02");
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
        this.TextBoxOut.Text = "Error on send scenario" + Environment.NewLine + "Scenario: " + Scenario.ToString() + Environment.NewLine + "Option: 0x" + ModuleOption.ToString("x02");
      }
      finally
      {
        this.SetStopState();
      }
    }

    private byte GetModuleOption(int selection)
    {
      return selection < 2 ? (byte) selection : (byte) (selection - 2 + 16);
    }

    private async void ButtonScenarioReadFromDeviceByOptions_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        this.SetRunState();
        byte ModuleOption = this.GetModuleOption(this.ComboBoxModulOptionRead.SelectedIndex);
        ushort scenario = await this.ScenarioManager.ReadCommunicationScenarioByOptions(this.progress, this.cancelToken, byte.Parse(this.TextBoxModuleCode.Text), ModuleOption);
        this.TextBoxOut.Text = "Read scenario by options done" + Environment.NewLine + "Option: 0x" + ModuleOption.ToString("x02") + Environment.NewLine + "Result scenario: " + scenario.ToString();
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
      finally
      {
        this.SetStopState();
      }
    }

    private async void ButtonScenarioSendToDevice_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        this.SetRunState();
        await this.ScenarioManager.WriteCommunicationScenario(this.progress, this.cancelToken, ushort.Parse(this.TextBoxScenario.Text));
        this.TextBoxOut.Text = "Set scenario done.";
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
      finally
      {
        this.SetStopState();
      }
    }

    private async void ButtonScenarioReadFromDevice_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        this.SetRunState();
        ushort scenario = await this.ScenarioManager.ReadCommunicationScenario(this.progress, this.cancelToken);
        this.TextBoxOut.Text = "Read scenario done: " + scenario.ToString();
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
      finally
      {
        this.SetStopState();
      }
    }

    private async void ButtonSetGroups_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        this.SetRunState();
        await this.ScenarioManager.SetGroupsForScenario(this.progress, this.cancelToken, ushort.Parse(this.TextBoxScenario.Text));
        this.TextBoxOut.Text = "Set groups done.";
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
      finally
      {
        this.SetStopState();
      }
    }

    private void ButtonShowScenario_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        this.TextBoxOut.Text = this.ScenarioManager.GetAdaptedConfigurationAsText((ushort) this.ComboBoxPreparedScenarios.SelectedItem);
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex, "Illegal scenario");
      }
    }

    private async void ButtonWriteScenarioConfig_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        this.SetRunState();
        ushort scenario = ushort.Parse(this.ComboBoxScenarioNumber.Text);
        byte[] configData = this.ScenarioManager.GetAdaptedConfiguration(scenario);
        await this.ScenarioManager.WriteModuleConfiguration(this.progress, this.cancelToken, scenario, configData);
        this.TextBoxOut.Text = configData.Length == 0 ? "!!! ModuleConfiguration not usable !!" : "SetModuleConfiguration done: " + scenario.ToString();
        configData = (byte[]) null;
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex, "Error on SetModuleConfiguration");
      }
      finally
      {
        this.SetStopState();
      }
    }

    private async void ButtonDeleteScenarioConfig_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        this.SetRunState();
        ushort scenario = ushort.Parse(this.ComboBoxScenarioNumber.Text);
        byte[] configData = new byte[0];
        await this.ScenarioManager.WriteModuleConfiguration(this.progress, this.cancelToken, scenario, configData);
        this.TextBoxOut.Text = "DeleteModuleConfiguration done: " + scenario.ToString();
        configData = (byte[]) null;
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex, "Error on SetModuleConfiguration");
      }
      finally
      {
        this.SetStopState();
      }
    }

    private async void ButtonDeleteScenarioRange_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        this.SetRunState();
        ushort scenario = ushort.Parse(this.ComboBoxScenarioNumber.Text);
        ScenarioRanges scenarioRange = (ScenarioRanges) ((int) scenario / 100 * 100);
        await this.ScenarioManager.DeleteScenarioRange(this.progress, this.cancelToken, scenarioRange);
        this.TextBoxOut.Text = "DeleteScenarioRange done: " + scenarioRange.ToString();
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex, "Error on DeleteScenarioRang");
      }
      finally
      {
        this.SetStopState();
      }
    }

    private async void ButtonReadScenarioConfig_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        this.SetRunState();
        ushort scenario = ushort.Parse(this.ComboBoxScenarioNumber.Text);
        S4_ScenarioManager scenarioManager = this.ScenarioManager;
        ProgressHandler progress = this.progress;
        CancellationToken cancelToken = this.cancelToken;
        int scenario1 = (int) scenario;
        bool? isChecked = this.CheckBoxReadOnlySetup.IsChecked;
        int num = !isChecked.Value ? 1 : 0;
        byte[] configData = await scenarioManager.ReadModuleConfiguration(progress, cancelToken, (ushort) scenario1, num != 0);
        TextBox textBoxOut = this.TextBoxOut;
        int communicationScenario = (int) scenario;
        byte[] configData1 = configData;
        isChecked = this.CheckBoxReadOnlySetup.IsChecked;
        bool? onlySetup = new bool?(isChecked.Value);
        string configurationText = ScenarioConfigurations.GetConfigurationText((ushort) communicationScenario, configData1, onlySetup);
        textBoxOut.Text = configurationText;
        configData = (byte[]) null;
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
      finally
      {
        this.SetStopState();
      }
    }

    private void ButtonShowPreparedScenarios_Click(object sender, RoutedEventArgs e)
    {
      this.TextBoxOut.Text = ScenarioConfigurations.GetPreparedConfigurationsAsText();
    }

    private async void ButtonReadScenarioListFromDevice_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        this.SetRunState();
        ushort[] scenarios = await this.ScenarioManager.ReadAvailableModuleConfigurations(this.progress, this.cancelToken);
        this.ComboBoxScenarioNumber.ItemsSource = (IEnumerable) scenarios;
        if (this.ComboBoxScenarioNumber.Items.Count > 0)
          this.ComboBoxScenarioNumber.SelectedIndex = 0;
        StringBuilder loadedScenarios = new StringBuilder();
        loadedScenarios.AppendLine("Configued scenarios in device. (Read from device):");
        loadedScenarios.AppendLine();
        ushort[] numArray = scenarios;
        for (int index = 0; index < numArray.Length; ++index)
        {
          ushort scenario = numArray[index];
          loadedScenarios.AppendLine(scenario.ToString());
        }
        numArray = (ushort[]) null;
        this.TextBoxOut.Text = loadedScenarios.ToString();
        scenarios = (ushort[]) null;
        loadedScenarios = (StringBuilder) null;
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
      finally
      {
        this.SetStopState();
      }
    }

    private void ButtonAddScenarioFromFile_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        string fullPath = Path.GetFullPath(Path.Combine(SystemValues.AppPath, "..", "Source", "Handlers", "S4_Handler", "ScenarioDefinitions"));
        OpenFileDialog openFileDialog = new OpenFileDialog();
        openFileDialog.DefaultExt = ".sce";
        openFileDialog.Filter = "Scenario definition (.sce)|*.sce";
        openFileDialog.CheckFileExists = true;
        openFileDialog.Multiselect = true;
        if (Directory.Exists(fullPath))
          openFileDialog.InitialDirectory = fullPath;
        bool? nullable = openFileDialog.ShowDialog();
        bool flag = true;
        if (!(nullable.GetValueOrDefault() == flag & nullable.HasValue))
          return;
        foreach (string fileName in openFileDialog.FileNames)
        {
          using (StreamReader streamReader = new StreamReader(fileName))
            ScenarioConfigurations.AddScenarioFromFile(streamReader.ReadToEnd());
        }
        this.LoadAndShowPreparedScenarios();
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
    }

    private void ButtonAddScenarioFromFileToDatabase_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        string fullPath = Path.GetFullPath(Path.Combine(SystemValues.AppPath, "..", "Source", "Handlers", "S4_Handler", "ScenarioDefinitions"));
        OpenFileDialog openFileDialog = new OpenFileDialog();
        openFileDialog.DefaultExt = ".sce";
        openFileDialog.Filter = "Scenario definition (.sce)|*.sce";
        openFileDialog.CheckFileExists = true;
        openFileDialog.Multiselect = true;
        if (Directory.Exists(fullPath))
          openFileDialog.InitialDirectory = fullPath;
        bool? nullable = openFileDialog.ShowDialog();
        bool flag = true;
        if (!(nullable.GetValueOrDefault() == flag & nullable.HasValue))
          return;
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("Scenarios written to database: ");
        foreach (string fileName in openFileDialog.FileNames)
        {
          using (StreamReader streamReader = new StreamReader(fileName))
          {
            string end = streamReader.ReadToEnd();
            ushort scenario = ScenarioConfigurations.AddScenarioFromFileContent(new Dictionary<ushort, List<KeyValuePair<string, object>>>(), end);
            ScenarioConfigurations.WriteScenarioToDatabase(scenario, end);
            stringBuilder.AppendLine(scenario.ToString());
          }
        }
        this.TextBoxOut.Text = stringBuilder.ToString();
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
    }

    private void ButtonLoadAllScenarioFromoDatabase_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        ScenarioConfigurations.LoadSelectedScenariosFromDatabase("IUW", "IUWO");
        this.LoadAndShowPreparedScenarios();
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
    }

    private void LoadAndShowPreparedScenarios()
    {
      List<ushort> preparedScenarios = ScenarioConfigurations.GetPreparedScenarios();
      this.ComboBoxPreparedScenarios.Items.Clear();
      foreach (int newItem in preparedScenarios)
        this.ComboBoxPreparedScenarios.Items.Add((object) (ushort) newItem);
      if (this.ComboBoxPreparedScenarios.Items.Count > 0)
        this.ComboBoxPreparedScenarios.SelectedIndex = 0;
      this.TextBoxOut.Text = ScenarioConfigurations.GetPreparedConfigurationsAsText();
    }

    private void ButtonDeleteAllPreparedScenarios_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        ScenarioConfigurations.DeleteAllPreparedScenarios();
        this.ComboBoxPreparedScenarios.Items.Clear();
        this.TextBoxOut.Text = "All prepared scenarios deleted";
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
    }

    private async void ButtonDeleteAllConfigurationsFromDevice_Click(
      object sender,
      RoutedEventArgs e)
    {
      try
      {
        this.SetRunState();
        await this.ScenarioManager.Nfc.DeleteAllModuleConfigurations(this.progress, this.cancelToken);
        this.TextBoxOut.Text = "All configurations cleared inside the device.";
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex, "Error on DeleteAllConfigurationsFromDevice");
      }
      finally
      {
        this.SetStopState();
      }
    }

    private async void ButtonReadMap_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        this.SetRunState();
        await this.ScenarioManager.ReadConfigurationFromMap(this.progress, this.cancelToken);
        this.TextBoxOut.Text = "Read form map done";
        this.AddScenariosListFromMap();
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex, "Error on read form map");
      }
      finally
      {
        this.SetStopState();
      }
    }

    private void ButtonShowScenarioListFromMap_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        this.TextBoxOut.Text = "Show from map";
        if (!this.ScenarioManager.PrepareConfigurationFromMap())
          this.TextBoxOut.AppendText(Environment.NewLine + "Scenario data not available inside the map");
        else
          this.AddScenariosListFromMap();
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex, "Error on show list from map");
      }
      finally
      {
        this.SetStopState();
      }
    }

    private void ButtonShowSelectedFromMap_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        if (this.ComboBoxMapScenarios.SelectedIndex < 0)
          return;
        ushort key = this.ScenarioManager.ConfigsFromMap[this.ComboBoxMapScenarios.SelectedIndex].Key;
        byte[] configData = this.ScenarioManager.ConfigsFromMap[this.ComboBoxMapScenarios.SelectedIndex].Value;
        string configurationText = ScenarioConfigurations.GetConfigurationText(key, configData);
        this.TextBoxOut.Text = "Show selected from map" + Environment.NewLine + "Scenario: " + key.ToString() + Environment.NewLine + Environment.NewLine + configurationText;
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex, "Error on show list from map");
      }
      finally
      {
        this.SetStopState();
      }
    }

    private void AddScenariosListFromMap()
    {
      this.ComboBoxMapScenarios.Items.Clear();
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine();
      stringBuilder.AppendLine();
      stringBuilder.AppendLine("Scenario list form Map");
      stringBuilder.AppendLine();
      if (this.ScenarioManager.ConfigsFromMap == null)
        stringBuilder.AppendLine("Config not available");
      else if (this.ScenarioManager.ConfigsFromMap.Count == 0)
      {
        stringBuilder.AppendLine("Scenario list is empty");
      }
      else
      {
        foreach (KeyValuePair<ushort, byte[]> configsFrom in this.ScenarioManager.ConfigsFromMap)
        {
          stringBuilder.AppendLine(configsFrom.Key.ToString());
          this.ComboBoxMapScenarios.Items.Add((object) configsFrom.Key.ToString());
        }
        if (this.ComboBoxMapScenarios.Items.Count > 0)
          this.ComboBoxMapScenarios.SelectedIndex = 0;
      }
      this.TextBoxOut.AppendText(stringBuilder.ToString());
    }

    private async void ButtonWriteAllPreparedConfigurations_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        this.SetRunState();
        List<ushort> preparedScenarios = this.ScenarioManager.GetPreparedScenarios();
        foreach (ushort scenario in preparedScenarios)
        {
          byte[] configData = this.ScenarioManager.GetAdaptedConfiguration(scenario);
          await this.ScenarioManager.WriteModuleConfiguration(this.progress, this.cancelToken, scenario, configData);
          configData = (byte[]) null;
        }
        this.TextBoxOut.Text = "Write all configurations done";
        preparedScenarios = (List<ushort>) null;
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex, "Error on SetModuleConfiguration");
      }
      finally
      {
        this.SetStopState();
      }
    }

    private void TabItemPrepared_Loaded(object sender, RoutedEventArgs e)
    {
      this.LoadAndShowPreparedScenarios();
    }

    private void ButtonUnloadPreparedScenario_Click(object sender, RoutedEventArgs e)
    {
      if (this.ComboBoxPreparedScenarios.SelectedItem == null)
        return;
      try
      {
        ScenarioConfigurations.DeletePreparedScenario((ushort) this.ComboBoxPreparedScenarios.SelectedItem);
        this.LoadAndShowPreparedScenarios();
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex, "Illegal scenario");
      }
    }

    private void ButtonComparePreparedScenario_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        ushort selectedItem = (ushort) this.ComboBoxPreparedScenarios.SelectedItem;
        StringBuilder compareInfo = new StringBuilder();
        if (ScenarioConfigurations.IsScenarioEqualInSecondaryDB(selectedItem, compareInfo))
          this.TextBoxOut.Text = selectedItem.ToString() + " is equal in secondary database";
        else
          this.TextBoxOut.Text = selectedItem.ToString() + " is not equal in secondary database !!!" + Environment.NewLine + compareInfo.ToString();
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex, "Illegal scenario");
      }
    }

    private void ButtonCopyPreparedScenario_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        ushort selectedItem = (ushort) this.ComboBoxPreparedScenarios.SelectedItem;
        ScenarioConfigurations.CopyScenarioToSecondaryDB(selectedItem);
        this.TextBoxOut.Text = selectedItem.ToString() + " copy done";
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex, "Copy error");
      }
    }

    private void ButtonCheckChangedScenarios_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        this.TextBoxOut.Text = ScenarioConfigurations.CopyChangedScenariosToSecondDatabase(true);
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex, "Copy error");
      }
    }

    private void ButtonCopyChangedScenarios_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        this.TextBoxOut.Text = ScenarioConfigurations.CopyChangedScenariosToSecondDatabase(false);
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex, "Copy error");
      }
    }

    private async void ButtonCompareAllLoadedConfigurations_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        this.SetRunState();
        ushort[] scenarios = await this.ScenarioManager.ReadAvailableModuleConfigurations(this.progress, this.cancelToken);
        StringBuilder loadedScenarios = new StringBuilder();
        loadedScenarios.AppendLine("Compared scenarios in device. (Read from device):");
        ushort[] numArray = scenarios;
        for (int index = 0; index < numArray.Length; ++index)
        {
          ushort scenario = numArray[index];
          loadedScenarios.AppendLine();
          loadedScenarios.AppendLine("Scenario: " + scenario.ToString());
          byte[] deviceConfigData = await this.ScenarioManager.ReadModuleConfiguration(this.progress, this.cancelToken, scenario);
          byte[] poorConfigData = new byte[deviceConfigData.Length - 2];
          Buffer.BlockCopy((Array) deviceConfigData, 2, (Array) poorConfigData, 0, poorConfigData.Length);
          byte[] preparedConfigData;
          try
          {
            preparedConfigData = this.ScenarioManager.GetAdaptedConfiguration(scenario);
          }
          catch
          {
            loadedScenarios.AppendLine("Scenario not prepared");
            continue;
          }
          bool diff = false;
          if (poorConfigData.Length != preparedConfigData.Length)
          {
            diff = true;
          }
          else
          {
            for (int i = 0; i < poorConfigData.Length; ++i)
            {
              if ((int) poorConfigData[i] != (int) preparedConfigData[i])
              {
                diff = true;
                break;
              }
            }
          }
          if (diff)
          {
            loadedScenarios.AppendLine("  Device: " + ZR_ClassLibrary.Util.ByteArrayToHexString(poorConfigData));
            loadedScenarios.AppendLine("  Prepared: " + ZR_ClassLibrary.Util.ByteArrayToHexString(preparedConfigData));
          }
          else
            loadedScenarios.AppendLine("  Equal");
          deviceConfigData = (byte[]) null;
          poorConfigData = (byte[]) null;
          preparedConfigData = (byte[]) null;
        }
        numArray = (ushort[]) null;
        this.TextBoxOut.Text = loadedScenarios.ToString();
        scenarios = (ushort[]) null;
        loadedScenarios = (StringBuilder) null;
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
      finally
      {
        this.SetStopState();
      }
    }

    private void ButtonShowTimeManagement_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        foreach (KeyValuePair<string, object> adaptedConfiguration in this.ScenarioManager.GetAdaptedConfigurationList((ushort) this.ComboBoxPreparedScenarios.SelectedItem))
        {
          if (adaptedConfiguration.Key == "RadioOnSelection")
          {
            RadioOffTimes radioOffTimes = new RadioOffTimes((RadioOffTimeManagement) adaptedConfiguration.Value);
            radioOffTimes.Owner = (Window) this;
            radioOffTimes.ShowDialog();
            return;
          }
        }
        throw new Exception("Time definition not found");
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex, "Show time management error");
      }
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/S4_Handler;component/userinterface/s4_scenariowindow.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          ((Window) target).Closing += new CancelEventHandler(this.Window_Closing);
          break;
        case 2:
          this.TabControlButtons = (TabControl) target;
          break;
        case 3:
          this.TabItemDevice = (TabItem) target;
          break;
        case 4:
          this.ComboBoxScenarioNumber = (ComboBox) target;
          break;
        case 5:
          this.ButtonReadScenarioListFromDevice = (Button) target;
          this.ButtonReadScenarioListFromDevice.Click += new RoutedEventHandler(this.ButtonReadScenarioListFromDevice_Click);
          break;
        case 6:
          this.ButtonSendScenarioConfig = (Button) target;
          this.ButtonSendScenarioConfig.Click += new RoutedEventHandler(this.ButtonWriteScenarioConfig_Click);
          break;
        case 7:
          this.ButtonDeleteScenarioConfig = (Button) target;
          this.ButtonDeleteScenarioConfig.Click += new RoutedEventHandler(this.ButtonDeleteScenarioConfig_Click);
          break;
        case 8:
          this.ButtonDeleteScenarioRange = (Button) target;
          this.ButtonDeleteScenarioRange.Click += new RoutedEventHandler(this.ButtonDeleteScenarioRange_Click);
          break;
        case 9:
          this.CheckBoxReadOnlySetup = (CheckBox) target;
          break;
        case 10:
          this.ButtonReadScenarioConfig = (Button) target;
          this.ButtonReadScenarioConfig.Click += new RoutedEventHandler(this.ButtonReadScenarioConfig_Click);
          break;
        case 11:
          this.ButtonDeleteAllConfigurationsFromDevice = (Button) target;
          this.ButtonDeleteAllConfigurationsFromDevice.Click += new RoutedEventHandler(this.ButtonDeleteAllConfigurationsFromDevice_Click);
          break;
        case 12:
          this.ButtonWriteAllPreparedConfigurations = (Button) target;
          this.ButtonWriteAllPreparedConfigurations.Click += new RoutedEventHandler(this.ButtonWriteAllPreparedConfigurations_Click);
          break;
        case 13:
          this.ButtonCompareAllLoadedConfigurations = (Button) target;
          this.ButtonCompareAllLoadedConfigurations.Click += new RoutedEventHandler(this.ButtonCompareAllLoadedConfigurations_Click);
          break;
        case 14:
          this.TabItemMap = (TabItem) target;
          break;
        case 15:
          this.ButtonReadMap = (Button) target;
          this.ButtonReadMap.Click += new RoutedEventHandler(this.ButtonReadMap_Click);
          break;
        case 16:
          this.ButtonShowScenarioListFromMap = (Button) target;
          this.ButtonShowScenarioListFromMap.Click += new RoutedEventHandler(this.ButtonShowScenarioListFromMap_Click);
          break;
        case 17:
          this.ComboBoxMapScenarios = (ComboBox) target;
          break;
        case 18:
          this.ButtonShowSelectedFromMap = (Button) target;
          this.ButtonShowSelectedFromMap.Click += new RoutedEventHandler(this.ButtonShowSelectedFromMap_Click);
          break;
        case 19:
          this.TabItemPrepared = (TabItem) target;
          this.TabItemPrepared.Loaded += new RoutedEventHandler(this.TabItemPrepared_Loaded);
          break;
        case 20:
          this.ButtonLoadAllScenarioFromoDatabase = (Button) target;
          this.ButtonLoadAllScenarioFromoDatabase.Click += new RoutedEventHandler(this.ButtonLoadAllScenarioFromoDatabase_Click);
          break;
        case 21:
          this.ButtonShowPreparedScenarios = (Button) target;
          this.ButtonShowPreparedScenarios.Click += new RoutedEventHandler(this.ButtonShowPreparedScenarios_Click);
          break;
        case 22:
          this.ButtonDeleteAllPreparedScenarios = (Button) target;
          this.ButtonDeleteAllPreparedScenarios.Click += new RoutedEventHandler(this.ButtonDeleteAllPreparedScenarios_Click);
          break;
        case 23:
          this.ButtonAddScenarioFromFile = (Button) target;
          this.ButtonAddScenarioFromFile.Click += new RoutedEventHandler(this.ButtonAddScenarioFromFile_Click);
          break;
        case 24:
          this.ButtonAddScenarioFromFileToDatabase = (Button) target;
          this.ButtonAddScenarioFromFileToDatabase.Click += new RoutedEventHandler(this.ButtonAddScenarioFromFileToDatabase_Click);
          break;
        case 25:
          this.ComboBoxPreparedScenarios = (ComboBox) target;
          break;
        case 26:
          this.ButtonShowScenario = (Button) target;
          this.ButtonShowScenario.Click += new RoutedEventHandler(this.ButtonShowScenario_Click);
          break;
        case 27:
          this.ButtonUnloadPreparedScenario = (Button) target;
          this.ButtonUnloadPreparedScenario.Click += new RoutedEventHandler(this.ButtonUnloadPreparedScenario_Click);
          break;
        case 28:
          this.ButtonComparePreparedScenario = (Button) target;
          this.ButtonComparePreparedScenario.Click += new RoutedEventHandler(this.ButtonComparePreparedScenario_Click);
          break;
        case 29:
          this.ButtonCopyPreparedScenario = (Button) target;
          this.ButtonCopyPreparedScenario.Click += new RoutedEventHandler(this.ButtonCopyPreparedScenario_Click);
          break;
        case 30:
          this.ButtonCheckChangedScenarios = (Button) target;
          this.ButtonCheckChangedScenarios.Click += new RoutedEventHandler(this.ButtonCheckChangedScenarios_Click);
          break;
        case 31:
          this.ButtonCopyChangedScenarios = (Button) target;
          this.ButtonCopyChangedScenarios.Click += new RoutedEventHandler(this.ButtonCopyChangedScenarios_Click);
          break;
        case 32:
          this.ButtonShowTimeManagement = (Button) target;
          this.ButtonShowTimeManagement.Click += new RoutedEventHandler(this.ButtonShowTimeManagement_Click);
          break;
        case 33:
          this.GroupBoxScenarioCommands = (GroupBox) target;
          break;
        case 34:
          this.TextBoxScenario = (TextBox) target;
          break;
        case 35:
          this.ButtonScenarioSendToFromDevice = (Button) target;
          this.ButtonScenarioSendToFromDevice.Click += new RoutedEventHandler(this.ButtonScenarioSendToDevice_Click);
          break;
        case 36:
          this.ButtonScenarioReadFromDevice = (Button) target;
          this.ButtonScenarioReadFromDevice.Click += new RoutedEventHandler(this.ButtonScenarioReadFromDevice_Click);
          break;
        case 37:
          this.ButtonSetGroups = (Button) target;
          this.ButtonSetGroups.Click += new RoutedEventHandler(this.ButtonSetGroups_Click);
          break;
        case 38:
          this.TextBoxModuleCode = (TextBox) target;
          break;
        case 39:
          this.ComboBoxModulOptionSend = (ComboBox) target;
          break;
        case 40:
          this.ComboBoxModulOptionRead = (ComboBox) target;
          break;
        case 41:
          this.TextBoxScenarioInternal = (TextBox) target;
          break;
        case 42:
          this.ButtonScenarioSendToDeviceInternal = (Button) target;
          this.ButtonScenarioSendToDeviceInternal.Click += new RoutedEventHandler(this.ButtonScenarioSendToDeviceByOptions_Click);
          break;
        case 43:
          this.ButtonScenarioReadFromDeviceInternal = (Button) target;
          this.ButtonScenarioReadFromDeviceInternal.Click += new RoutedEventHandler(this.ButtonScenarioReadFromDeviceByOptions_Click);
          break;
        case 44:
          this.TextBoxOut = (TextBox) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
