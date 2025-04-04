// Decompiled with JetBrains decompiler
// Type: S4_Handler.UserInterface.S4_TestWindowCommunication
// Assembly: S4_Handler, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 6FB3FA3B-A643-4E86-9555-EAB58D0F89E2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S4_Handler.dll

using CommonWPF;
using HandlerLib;
using HandlerLib.NFC;
using S4_Handler.Functions;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using ZENNER.CommonLibrary;
using ZR_ClassLibrary;

#nullable disable
namespace S4_Handler.UserInterface
{
  public class S4_TestWindowCommunication : Window, IComponentConnector
  {
    private const int NamePadSize = 40;
    private Cursor defaultCursor;
    private CancellationTokenSource cancelTokenSource;
    private ProgressHandler progress;
    private S4_HandlerWindowFunctions myWindowFunctions;
    private S4_HandlerFunctions myFunctions;
    private S4_DeviceCommandsNFC myCommands;
    private NfcDeviceIdentification NFC_DeviceIdentification = (NfcDeviceIdentification) null;
    internal ProgressBar ProgressBar1;
    internal Button ButtonBreak;
    internal Button ButtonReadCommunicationStatus;
    internal Button ButtonClearCommunicationGroups;
    internal Button ButtonReadAvailableGroupDefinition;
    internal Button ButtonReadAvailableGroupData;
    internal Button ButtonReadAllGroupDefinitions;
    internal Button ButtonReadAllGroupData;
    internal TextBox TextBoxGroupNumber;
    internal Button ButtonReadSelectedGroupDefinition;
    internal Button ButtonReadSelectedGroupData;
    internal ComboBox ComboBoxScenarioNumber;
    internal Button ButtonReadAvailableScenarios;
    internal Button ButtonSetGroupsForScenario;
    internal Button ButtonAddCommunicationGroupSensus;
    internal Button ButtonRunScalingTest;
    internal TextBlock TextBlockStatus;
    internal TextBox TextBoxDemoCommandResult;
    private bool _contentLoaded;

    public S4_TestWindowCommunication(S4_HandlerWindowFunctions myWindowFunctions)
    {
      this.myWindowFunctions = myWindowFunctions;
      this.myFunctions = myWindowFunctions.MyFunctions;
      this.myCommands = this.myFunctions.myDeviceCommands;
      this.cancelTokenSource = new CancellationTokenSource();
      this.progress = new ProgressHandler(new Action<ProgressArg>(this.OnProgress));
      this.InitializeComponent();
    }

    private void SetRunState()
    {
      this.cancelTokenSource = new CancellationTokenSource();
      this.progress.Reset();
      this.progress.Split(new double[2]{ 2.0, 98.0 });
      this.progress.Report("Run");
      this.ButtonBreak.IsEnabled = true;
      this.defaultCursor = this.Cursor;
      this.Cursor = Cursors.Wait;
    }

    private void SetStopState()
    {
      this.ButtonBreak.IsEnabled = false;
      this.Cursor = this.defaultCursor;
      this.progress.Reset();
      this.TextBlockStatus.Text = "";
    }

    private void OnProgress(ProgressArg obj)
    {
      if (!this.CheckAccess())
      {
        this.Dispatcher.Invoke((Action) (() => this.OnProgress(obj)));
      }
      else
      {
        this.ProgressBar1.Value = obj.ProgressPercentage;
        if (obj.Tag != null && obj.Tag.GetType() == typeof (string))
        {
          string tag = (string) obj.Tag;
          if (this.TextBoxDemoCommandResult.Text.Length == 0)
            this.TextBoxDemoCommandResult.AppendText(tag);
          else
            this.TextBoxDemoCommandResult.AppendText(Environment.NewLine + tag);
          if (this.TextBoxDemoCommandResult.Text.Length > 10000)
          {
            int start = this.TextBoxDemoCommandResult.Text.IndexOf(Environment.NewLine);
            int num = this.TextBoxDemoCommandResult.Text.IndexOf(Environment.NewLine, 1000);
            if (start > 0 && num > 0)
            {
              int length = num - start;
              this.TextBoxDemoCommandResult.Select(start, length);
              this.TextBoxDemoCommandResult.SelectedText = "";
            }
          }
          this.TextBoxDemoCommandResult.ScrollToEnd();
        }
        else
          this.TextBlockStatus.Text = obj.Message;
      }
    }

    private void ButtonBreak_Click(object sender, RoutedEventArgs e)
    {
      this.cancelTokenSource.Cancel();
    }

    private async void ButtonCommand_Click(object sender, RoutedEventArgs e)
    {
      this.SetRunState();
      try
      {
        if (sender == this.ButtonReadCommunicationStatus)
          await this.ReadAndShowCommunicationStatusAsync();
        else if (sender == this.ButtonReadAllGroupDefinitions)
        {
          await this.ReadCommunicationStatusAsync();
          StringBuilder result = new StringBuilder();
          result.AppendLine("All groups definitions");
          result.AppendLine();
          this.TextBoxDemoCommandResult.Text = result.ToString();
          result.AppendLine("Selected parameter groups");
          int i1 = 0;
          byte? nullable1;
          while (true)
          {
            int num = i1;
            nullable1 = this.NFC_DeviceIdentification.NumberOfSelectedParameterGroups;
            int? nullable2 = nullable1.HasValue ? new int?((int) nullable1.GetValueOrDefault()) : new int?();
            int valueOrDefault = nullable2.GetValueOrDefault();
            if (num < valueOrDefault & nullable2.HasValue)
            {
              result.AppendLine();
              result.AppendLine("   Selected parameter group:" + i1.ToString());
              result.AppendLine();
              byte[] groupRequest = new byte[1]{ (byte) i1 };
              byte[] response = await this.myCommands.CommonNfcCommands.SendCommandAndGetResultAsync(this.progress, this.cancelTokenSource.Token, NfcCommands.GetSelectedParameterGroupSettings, groupRequest);
              List<S4_DifVif_Parameter> groupParams = S4_DifVif_Parameter.GetParametersFromNfcProtocolData(response);
              result.AppendLine("      Protocol:" + Util.ByteArrayToHexString(response));
              result.AppendLine();
              foreach (S4_DifVif_Parameter theParam in groupParams)
                result.AppendLine("      " + theParam.ToStringPadNameRight(40));
              groupRequest = (byte[]) null;
              response = (byte[]) null;
              groupParams = (List<S4_DifVif_Parameter>) null;
              ++i1;
            }
            else
              break;
          }
          this.TextBoxDemoCommandResult.Text = result.ToString();
          result.AppendLine();
          result.AppendLine("Available parameter groups");
          int i2 = 0;
          while (true)
          {
            int num = i2;
            nullable1 = this.NFC_DeviceIdentification.NumberOfAvailableParameterGroups;
            int? nullable3 = nullable1.HasValue ? new int?((int) nullable1.GetValueOrDefault()) : new int?();
            int valueOrDefault = nullable3.GetValueOrDefault();
            if (num < valueOrDefault & nullable3.HasValue)
            {
              result.AppendLine();
              result.AppendLine("   Available parameter group:" + i2.ToString());
              result.AppendLine();
              try
              {
                byte[] groupRequest = new byte[1]
                {
                  (byte) i2
                };
                byte[] response = await this.myCommands.CommonNfcCommands.SendCommandAndGetResultAsync(this.progress, this.cancelTokenSource.Token, NfcCommands.GetAvailableParameterGroupSettings, groupRequest);
                List<S4_DifVif_Parameter> groupParams = S4_DifVif_Parameter.GetParametersFromNfcProtocolData(response);
                result.AppendLine("      Protocol:" + Util.ByteArrayToHexString(response));
                result.AppendLine();
                foreach (S4_DifVif_Parameter theParam in groupParams)
                  result.AppendLine("      " + theParam.ToStringPadNameRight(40));
                groupRequest = (byte[]) null;
                response = (byte[]) null;
                groupParams = (List<S4_DifVif_Parameter>) null;
              }
              catch (Exception ex)
              {
                result.AppendLine("Exception: " + ex.Message);
              }
              ++i2;
            }
            else
              break;
          }
          this.TextBoxDemoCommandResult.Text = result.ToString();
          result = (StringBuilder) null;
        }
        else if (sender == this.ButtonReadAllGroupData)
        {
          await this.ReadCommunicationStatusAsync();
          StringBuilder result = new StringBuilder();
          result.AppendLine("All groups data");
          result.AppendLine();
          this.TextBoxDemoCommandResult.Text = result.ToString();
          result.AppendLine("Selected parameter groups");
          uint i3 = 0;
          byte? nullable4;
          while (true)
          {
            int num = (int) i3;
            nullable4 = this.NFC_DeviceIdentification.NumberOfSelectedParameterGroups;
            uint? nullable5 = nullable4.HasValue ? new uint?((uint) nullable4.GetValueOrDefault()) : new uint?();
            int valueOrDefault = (int) nullable5.GetValueOrDefault();
            if ((uint) num < (uint) valueOrDefault & nullable5.HasValue)
            {
              result.AppendLine();
              result.AppendLine("   Selected parameter group:" + i3.ToString());
              result.AppendLine();
              await this.ReadAndAddGroupData(result, i3, false);
              ++i3;
            }
            else
              break;
          }
          this.TextBoxDemoCommandResult.Text = result.ToString();
          result.AppendLine();
          result.AppendLine("Available parameter groups");
          uint i4 = 0;
          while (true)
          {
            int num = (int) i4;
            nullable4 = this.NFC_DeviceIdentification.NumberOfAvailableParameterGroups;
            uint? nullable6 = nullable4.HasValue ? new uint?((uint) nullable4.GetValueOrDefault()) : new uint?();
            int valueOrDefault = (int) nullable6.GetValueOrDefault();
            if ((uint) num < (uint) valueOrDefault & nullable6.HasValue)
            {
              result.AppendLine();
              result.AppendLine("   Available parameter group:" + i4.ToString());
              result.AppendLine();
              try
              {
                await this.ReadAndAddGroupData(result, i4, true);
              }
              catch (Exception ex)
              {
                result.AppendLine("Exception: " + ex.Message);
              }
              ++i4;
            }
            else
              break;
          }
          this.TextBoxDemoCommandResult.Text = result.ToString();
          result = (StringBuilder) null;
        }
        else if (sender == this.ButtonClearCommunicationGroups)
        {
          DeviceIdentification deviceIdentification = await this.myCommands.ReadVersionAsync(this.progress, this.cancelTokenSource.Token);
          await this.myCommands.CommonNfcCommands.StandardCommandAsync(this.progress, this.cancelTokenSource.Token, NfcCommands.ClearAllConfigurableParameterGroups);
          await this.ReadAndShowCommunicationStatusAsync();
        }
        else if (sender == this.ButtonAddCommunicationGroupSensus)
        {
          this.TextBoxDemoCommandResult.Clear();
          StringBuilder result = new StringBuilder();
          result.AppendLine("Add Sensis group");
          result.AppendLine();
          this.TextBoxDemoCommandResult.Text = result.ToString();
          DeviceIdentification deviceIdentification = await this.myCommands.ReadVersionAsync(this.progress, this.cancelTokenSource.Token);
          byte[] SensusSelectData = S4_DifVif_Parameter.GetSelectDataForSensusGroup();
          byte[] numArray = await this.myCommands.CommonNfcCommands.StandardCommandAsync(this.progress, this.cancelTokenSource.Token, NfcCommands.AddConfigurableParameterGroup, SensusSelectData);
          await this.ReadCommunicationStatusAsync();
          result.AppendLine("Done");
          result.AppendLine("Protocol data: " + Util.ByteArrayToHexString(SensusSelectData));
          this.TextBoxDemoCommandResult.Text = result.ToString();
          result = (StringBuilder) null;
          SensusSelectData = (byte[]) null;
        }
        else if (sender == this.ButtonReadSelectedGroupDefinition)
        {
          uint groupNumber = await this.GetGroupNumber(false);
          StringBuilder result = new StringBuilder();
          result.AppendLine("Parameter definition of selected group: " + groupNumber.ToString());
          result.AppendLine();
          byte[] groupRequest = new byte[1]
          {
            (byte) groupNumber
          };
          byte[] response = await this.myCommands.CommonNfcCommands.SendCommandAndGetResultAsync(this.progress, this.cancelTokenSource.Token, NfcCommands.GetSelectedParameterGroupSettings, groupRequest);
          List<S4_DifVif_Parameter> groupParams = S4_DifVif_Parameter.GetParametersFromNfcProtocolData(response);
          foreach (S4_DifVif_Parameter theParam in groupParams)
            result.AppendLine("   " + theParam.ToString() + "; Protocol:" + Util.ByteArrayToHexString(response));
          this.TextBoxDemoCommandResult.Text = result.ToString();
          result = (StringBuilder) null;
          groupRequest = (byte[]) null;
          response = (byte[]) null;
          groupParams = (List<S4_DifVif_Parameter>) null;
        }
        else if (sender == this.ButtonReadSelectedGroupData)
        {
          uint groupNumber = await this.GetGroupNumber(false);
          StringBuilder result = new StringBuilder();
          result.AppendLine("Parameter data of selected group: " + groupNumber.ToString());
          result.AppendLine();
          byte[] groupRequest = new byte[1]
          {
            (byte) groupNumber
          };
          await this.ReadAndAddGroupData(result, groupNumber, false);
          this.TextBoxDemoCommandResult.Text = result.ToString();
          result = (StringBuilder) null;
          groupRequest = (byte[]) null;
        }
        else if (sender == this.ButtonReadAvailableGroupDefinition)
        {
          uint groupNumber = await this.GetGroupNumber(true);
          StringBuilder result = new StringBuilder();
          result.AppendLine("Parameter definition of available group: " + groupNumber.ToString());
          result.AppendLine();
          byte[] groupRequest = new byte[1]
          {
            (byte) groupNumber
          };
          byte[] response = await this.myCommands.CommonNfcCommands.SendCommandAndGetResultAsync(this.progress, this.cancelTokenSource.Token, NfcCommands.GetAvailableParameterGroupSettings, groupRequest);
          List<S4_DifVif_Parameter> groupParams = S4_DifVif_Parameter.GetParametersFromNfcProtocolData(response);
          foreach (S4_DifVif_Parameter theParam in groupParams)
            result.AppendLine("   " + theParam.ToString() + "; Protocol:" + Util.ByteArrayToHexString(response));
          this.TextBoxDemoCommandResult.Text = result.ToString();
          result = (StringBuilder) null;
          groupRequest = (byte[]) null;
          response = (byte[]) null;
          groupParams = (List<S4_DifVif_Parameter>) null;
        }
        else if (sender == this.ButtonReadAvailableGroupData)
        {
          uint groupNumber = await this.GetGroupNumber(false);
          StringBuilder result = new StringBuilder();
          result.AppendLine("Parameter data of available group: " + groupNumber.ToString());
          result.AppendLine();
          byte[] groupRequest = new byte[1]
          {
            (byte) groupNumber
          };
          await this.ReadAndAddGroupData(result, groupNumber, true);
          this.TextBoxDemoCommandResult.Text = result.ToString();
          result = (StringBuilder) null;
          groupRequest = (byte[]) null;
        }
        else if (sender == this.ButtonReadAvailableScenarios)
        {
          S4_ScenarioManager scenarioManager = new S4_ScenarioManager(this.myFunctions.myDeviceCommands);
          ushort[] scenarios = await scenarioManager.ReadAvailableModuleConfigurations(this.progress, this.cancelTokenSource.Token);
          this.ComboBoxScenarioNumber.ItemsSource = (IEnumerable) scenarios;
          if (this.ComboBoxScenarioNumber.Items.Count > 0)
            this.ComboBoxScenarioNumber.SelectedIndex = 0;
          this.TextBoxDemoCommandResult.Text = "Read of available scenarios done.";
          scenarioManager = (S4_ScenarioManager) null;
          scenarios = (ushort[]) null;
        }
        else if (sender == this.ButtonSetGroupsForScenario)
        {
          if (this.ComboBoxScenarioNumber.SelectedItem == null)
            return;
          ushort scenario = (ushort) this.ComboBoxScenarioNumber.SelectedItem;
          S4_ScenarioManager scenarioManager = new S4_ScenarioManager(this.myFunctions.myDeviceCommands);
          await scenarioManager.SetGroupsForScenario(this.progress, this.cancelTokenSource.Token, scenario);
          this.TextBoxDemoCommandResult.Text = "Selected groups prepared for scenario: " + scenario.ToString();
          scenarioManager = (S4_ScenarioManager) null;
        }
        else if (sender == this.ButtonRunScalingTest)
        {
          StringBuilder result = new StringBuilder();
          result.AppendLine("Run scaling test result");
          result.AppendLine();
          DeviceIdentification deviceIdentification = await this.myCommands.ReadVersionAsync(this.progress, this.cancelTokenSource.Token);
          await this.myCommands.CommonNfcCommands.StandardCommandAsync(this.progress, this.cancelTokenSource.Token, NfcCommands.ClearAllConfigurableParameterGroups);
          List<S4_DifVif_Parameter> protocolSetupParameters = new List<S4_DifVif_Parameter>();
          protocolSetupParameters.Add(S4_DifVif_Parameter.PreDefParams.Find((Predicate<S4_DifVif_Parameter>) (x => x.Name == "Volume")));
          protocolSetupParameters.Add(S4_DifVif_Parameter.PreDefParams.Find((Predicate<S4_DifVif_Parameter>) (x => x.Name == "Volume, flow direction")));
          protocolSetupParameters.Add(S4_DifVif_Parameter.PreDefParams.Find((Predicate<S4_DifVif_Parameter>) (x => x.Name == "Volume, return direction")));
          protocolSetupParameters.Add(S4_DifVif_Parameter.PreDefParams.Find((Predicate<S4_DifVif_Parameter>) (x => x.Name == "Flow")));
          protocolSetupParameters.Add(S4_DifVif_Parameter.PreDefParams.Find((Predicate<S4_DifVif_Parameter>) (x => x.Name == "Temperature")));
          protocolSetupParameters.Add(S4_DifVif_Parameter.PreDefParams.Find((Predicate<S4_DifVif_Parameter>) (x => x.Name == "System state")));
          protocolSetupParameters.Add(S4_DifVif_Parameter.PreDefParams.Find((Predicate<S4_DifVif_Parameter>) (x => x.Name == "State")));
          byte[] groupSelectDifVif = S4_DifVif_Parameter.PrepareSelectData(protocolSetupParameters);
          byte[] numArray = await this.myCommands.CommonNfcCommands.StandardCommandAsync(this.progress, this.cancelTokenSource.Token, NfcCommands.AddConfigurableParameterGroup, groupSelectDifVif);
          byte[] group_0_Request = new byte[1];
          byte[] settingsResponse = await this.myCommands.CommonNfcCommands.SendCommandAndGetResultAsync(this.progress, this.cancelTokenSource.Token, NfcCommands.GetSelectedParameterGroupSettings, group_0_Request);
          List<S4_DifVif_Parameter> groupParams = S4_DifVif_Parameter.GetParametersFromNfcProtocolData(settingsResponse);
          if (protocolSetupParameters.Count != groupParams.Count)
            throw new Exception("Group setup problem. Selected parameter count != setup parameter count");
          byte[] dataResponse = await this.myCommands.CommonNfcCommands.SendCommandAndGetResultAsync(this.progress, this.cancelTokenSource.Token, NfcCommands.GetSelectedParameterGroupData, group_0_Request);
          S4_CurrentData currentData = await this.myFunctions.ReadCurrentDataAsync(this.progress, this.cancelTokenSource.Token);
          S4_FunctionalState functionalState = await this.myFunctions.ReadAlliveAndStateAsync(this.progress, this.cancelTokenSource.Token);
          DeviceStateCounter stateCounter = await this.myFunctions.ReadStateCounterAsync(this.progress, this.cancelTokenSource.Token);
          S4_SystemState systemState = await this.myFunctions.GetDeviceState(this.progress, this.cancelTokenSource.Token);
          int offset = 1;
          for (int i = 0; i < groupParams.Count; ++i)
          {
            S4_DifVif_Parameter theParam = groupParams[i];
            string parameterName = theParam.Name;
            if (parameterName != protocolSetupParameters[i].Name)
              throw new Exception("Parameter name mistake. Required : " + protocolSetupParameters[i].Name + " found: " + parameterName);
            result.AppendLine("Parameter name: " + parameterName);
            string groupValue = theParam.GetValueFromProtocol(dataResponse, ref offset);
            string originalValue = "";
            string str = parameterName;
            string str1 = str;
            double num1;
            float num2;
            if (str1 != null)
            {
              switch (str1.Length)
              {
                case 4:
                  if (str1 == "Flow")
                  {
                    num2 = currentData.Flow;
                    originalValue = num2.ToString() + currentData.Units.FlowUnitString;
                    break;
                  }
                  break;
                case 5:
                  if (str1 == "State")
                  {
                    originalValue = functionalState.ToString();
                    break;
                  }
                  break;
                case 6:
                  if (str1 == "Volume")
                  {
                    num1 = currentData.Volume;
                    originalValue = num1.ToString() + currentData.Units.VolumeUnitString;
                    break;
                  }
                  break;
                case 11:
                  if (str1 == "Temperature")
                  {
                    num2 = currentData.WaterTemperature;
                    originalValue = num2.ToString() + "°C";
                    break;
                  }
                  break;
                case 12:
                  if (str1 == "System state")
                  {
                    originalValue = systemState.ToString();
                    break;
                  }
                  break;
                case 22:
                  if (str1 == "Volume, flow direction")
                  {
                    num1 = currentData.FlowVolume;
                    originalValue = num1.ToString() + currentData.Units.VolumeUnitString;
                    break;
                  }
                  break;
                case 24:
                  if (str1 == "Volume, return direction")
                  {
                    num1 = currentData.ReturnVolume;
                    originalValue = num1.ToString() + currentData.Units.VolumeUnitString;
                    break;
                  }
                  break;
              }
            }
            str = (string) null;
            result.AppendLine("  Value from original protocol: " + originalValue);
            result.AppendLine("  Value back scaled from group: " + groupValue);
            result.AppendLine();
            theParam = (S4_DifVif_Parameter) null;
            parameterName = (string) null;
            groupValue = (string) null;
            originalValue = (string) null;
          }
          this.TextBoxDemoCommandResult.Text = result.ToString();
          result = (StringBuilder) null;
          protocolSetupParameters = (List<S4_DifVif_Parameter>) null;
          groupSelectDifVif = (byte[]) null;
          group_0_Request = (byte[]) null;
          settingsResponse = (byte[]) null;
          groupParams = (List<S4_DifVif_Parameter>) null;
          dataResponse = (byte[]) null;
          currentData = (S4_CurrentData) null;
          functionalState = (S4_FunctionalState) null;
          stateCounter = (DeviceStateCounter) null;
          systemState = (S4_SystemState) null;
        }
        else
          this.TextBoxDemoCommandResult.Text = "Not supported button";
      }
      catch (OperationCanceledException ex)
      {
      }
      catch (TimeoutException ex)
      {
        this.TextBoxDemoCommandResult.Text = "*** Timeout ***" + Environment.NewLine + ex.Message;
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
        this.TextBoxDemoCommandResult.Text = "*** Exception ***" + Environment.NewLine + ex.Message;
      }
      finally
      {
        this.SetStopState();
      }
    }

    private async Task ReadAndShowCommunicationStatusAsync()
    {
      this.TextBoxDemoCommandResult.Clear();
      await this.ReadCommunicationStatusAsync();
      StringBuilder result = new StringBuilder();
      result.AppendLine("Communication status");
      result.AppendLine();
      result.AppendLine("Number of available parameter groups:" + this.NFC_DeviceIdentification.NumberOfAvailableParameterGroups.ToString());
      StringBuilder stringBuilder1 = result;
      ushort? nullable = this.NFC_DeviceIdentification.NumberOfAvailableParameters;
      string str1 = "Number of available parameters:" + nullable.ToString();
      stringBuilder1.AppendLine(str1);
      result.AppendLine();
      result.AppendLine("Number of selected parameter groups:" + this.NFC_DeviceIdentification.NumberOfSelectedParameterGroups.ToString());
      StringBuilder stringBuilder2 = result;
      nullable = this.NFC_DeviceIdentification.NumberOfSelectedParameters;
      string str2 = "Number of selected parameters:" + nullable.ToString();
      stringBuilder2.AppendLine(str2);
      this.TextBoxDemoCommandResult.Text = result.ToString();
      result = (StringBuilder) null;
    }

    private async Task ReadCommunicationStatusAsync()
    {
      DeviceIdentification identification = await this.myCommands.ReadVersionAsync(this.progress, this.cancelTokenSource.Token);
      this.NFC_DeviceIdentification = identification is NfcDeviceIdentification ? (NfcDeviceIdentification) identification : throw new Exception("Illegal identification object");
      identification = (DeviceIdentification) null;
    }

    private async Task<uint> GetGroupNumber(bool checkAvailableGroups)
    {
      uint inputGroup = 0;
      if (!uint.TryParse(this.TextBoxGroupNumber.Text, out inputGroup))
        throw new Exception("Illegal group number");
      await this.CheckGroupNumber(inputGroup, checkAvailableGroups);
      return inputGroup;
    }

    private async Task CheckGroupNumber(uint groupNumber, bool checkAvailableGroups)
    {
      await this.ReadCommunicationStatusAsync();
      if (checkAvailableGroups)
      {
        int num = (int) groupNumber;
        byte? availableParameterGroups = this.NFC_DeviceIdentification.NumberOfAvailableParameterGroups;
        uint? nullable = availableParameterGroups.HasValue ? new uint?((uint) availableParameterGroups.GetValueOrDefault()) : new uint?();
        int valueOrDefault = (int) nullable.GetValueOrDefault();
        if ((uint) num >= (uint) valueOrDefault & nullable.HasValue)
        {
          availableParameterGroups = this.NFC_DeviceIdentification.NumberOfAvailableParameterGroups;
          throw new Exception("Illegal group number. Defined available parameter groups: " + availableParameterGroups.ToString());
        }
      }
      else
      {
        int num = (int) groupNumber;
        byte? selectedParameterGroups = this.NFC_DeviceIdentification.NumberOfSelectedParameterGroups;
        uint? nullable = selectedParameterGroups.HasValue ? new uint?((uint) selectedParameterGroups.GetValueOrDefault()) : new uint?();
        int valueOrDefault = (int) nullable.GetValueOrDefault();
        if ((uint) num >= (uint) valueOrDefault & nullable.HasValue)
        {
          selectedParameterGroups = this.NFC_DeviceIdentification.NumberOfSelectedParameterGroups;
          throw new Exception("Illegal group number. Defined selected parameter groups: " + selectedParameterGroups.ToString());
        }
      }
    }

    private async Task ReadAndAddGroupData(
      StringBuilder result,
      uint groupNumber,
      bool useAvailableGroup,
      bool addTelegram = true)
    {
      await this.CheckGroupNumber(groupNumber, useAvailableGroup);
      NfcCommands groupSettingsCommand;
      NfcCommands groupDataCommand;
      if (useAvailableGroup)
      {
        groupSettingsCommand = NfcCommands.GetAvailableParameterGroupSettings;
        groupDataCommand = NfcCommands.GetAvailableParameterGroupData;
      }
      else
      {
        groupSettingsCommand = NfcCommands.GetSelectedParameterGroupSettings;
        groupDataCommand = NfcCommands.GetSelectedParameterGroupData;
      }
      byte[] groupRequest = new byte[1]
      {
        (byte) groupNumber
      };
      byte[] settingsResponse = await this.myCommands.CommonNfcCommands.SendCommandAndGetResultAsync(this.progress, this.cancelTokenSource.Token, groupSettingsCommand, groupRequest);
      List<S4_DifVif_Parameter> groupParams = S4_DifVif_Parameter.GetParametersFromNfcProtocolData(settingsResponse);
      byte[] dataResponse = await this.myCommands.CommonNfcCommands.SendCommandAndGetResultAsync(this.progress, this.cancelTokenSource.Token, groupDataCommand, groupRequest);
      int offset = 1;
      foreach (S4_DifVif_Parameter theParam in groupParams)
      {
        string newValue = theParam.GetNameAndValueFromProtocol(dataResponse, ref offset);
        result.AppendLine(newValue);
        newValue = (string) null;
      }
      result.AppendLine();
      result.AppendLine("Received protocol:" + Util.ByteArrayToHexString(dataResponse));
      result.AppendLine();
      groupRequest = (byte[]) null;
      settingsResponse = (byte[]) null;
      groupParams = (List<S4_DifVif_Parameter>) null;
      dataResponse = (byte[]) null;
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/S4_Handler;component/userinterface/s4_testwindowscommunication.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.ProgressBar1 = (ProgressBar) target;
          break;
        case 2:
          this.ButtonBreak = (Button) target;
          this.ButtonBreak.Click += new RoutedEventHandler(this.ButtonBreak_Click);
          break;
        case 3:
          this.ButtonReadCommunicationStatus = (Button) target;
          this.ButtonReadCommunicationStatus.Click += new RoutedEventHandler(this.ButtonCommand_Click);
          break;
        case 4:
          this.ButtonClearCommunicationGroups = (Button) target;
          this.ButtonClearCommunicationGroups.Click += new RoutedEventHandler(this.ButtonCommand_Click);
          break;
        case 5:
          this.ButtonReadAvailableGroupDefinition = (Button) target;
          this.ButtonReadAvailableGroupDefinition.Click += new RoutedEventHandler(this.ButtonCommand_Click);
          break;
        case 6:
          this.ButtonReadAvailableGroupData = (Button) target;
          this.ButtonReadAvailableGroupData.Click += new RoutedEventHandler(this.ButtonCommand_Click);
          break;
        case 7:
          this.ButtonReadAllGroupDefinitions = (Button) target;
          this.ButtonReadAllGroupDefinitions.Click += new RoutedEventHandler(this.ButtonCommand_Click);
          break;
        case 8:
          this.ButtonReadAllGroupData = (Button) target;
          this.ButtonReadAllGroupData.Click += new RoutedEventHandler(this.ButtonCommand_Click);
          break;
        case 9:
          this.TextBoxGroupNumber = (TextBox) target;
          break;
        case 10:
          this.ButtonReadSelectedGroupDefinition = (Button) target;
          this.ButtonReadSelectedGroupDefinition.Click += new RoutedEventHandler(this.ButtonCommand_Click);
          break;
        case 11:
          this.ButtonReadSelectedGroupData = (Button) target;
          this.ButtonReadSelectedGroupData.Click += new RoutedEventHandler(this.ButtonCommand_Click);
          break;
        case 12:
          this.ComboBoxScenarioNumber = (ComboBox) target;
          break;
        case 13:
          this.ButtonReadAvailableScenarios = (Button) target;
          this.ButtonReadAvailableScenarios.Click += new RoutedEventHandler(this.ButtonCommand_Click);
          break;
        case 14:
          this.ButtonSetGroupsForScenario = (Button) target;
          this.ButtonSetGroupsForScenario.Click += new RoutedEventHandler(this.ButtonCommand_Click);
          break;
        case 15:
          this.ButtonAddCommunicationGroupSensus = (Button) target;
          this.ButtonAddCommunicationGroupSensus.Click += new RoutedEventHandler(this.ButtonCommand_Click);
          break;
        case 16:
          this.ButtonRunScalingTest = (Button) target;
          this.ButtonRunScalingTest.Click += new RoutedEventHandler(this.ButtonCommand_Click);
          break;
        case 17:
          this.TextBlockStatus = (TextBlock) target;
          break;
        case 18:
          this.TextBoxDemoCommandResult = (TextBox) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
