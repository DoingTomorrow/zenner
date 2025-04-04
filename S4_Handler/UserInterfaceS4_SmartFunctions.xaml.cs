// Decompiled with JetBrains decompiler
// Type: S4_Handler.UserInterface.S4_SmartFunctions
// Assembly: S4_Handler, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 6FB3FA3B-A643-4E86-9555-EAB58D0F89E2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S4_Handler.dll

using CommonWPF;
using HandlerLib;
using S4_Handler.Functions;
using SmartFunctionCompiler;
using StartupLib;
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

#nullable disable
namespace S4_Handler.UserInterface
{
  public partial class S4_SmartFunctions : Window, IComponentConnector
  {
    private MainWindow smc;
    private SmartFunction FunctionFromEditor;
    private byte[] FunctionManipulated;
    private S4_SmartFunctionManager SmartFunctionManager;
    private List<string> SmartFunctionGroups;
    private ProgressHandler progress;
    private CancellationToken cancellationToken;
    private bool IsPlugin;
    private List<SmartFunctionParameter> ReadedParameterList;
    internal StartupLib.GmmCorporateControl gmmCorporateControl1;
    internal Button ButtonShowEditor;
    internal StackPanel StackPanelDeviceCommands;
    internal Button ButtonReadLoadedFromDevice;
    internal Button ButtonDeleteAllFunctionsInDevice;
    internal Button ButtonWriteFromEditorToDevice;
    internal Button ButtonCompareAllToDevice;
    internal Button ButtonReadFunctionParameter;
    internal Button ButtonWriteChangedFunctionParameters;
    internal Button ButtonSaveFromEditorToDatabase;
    internal GroupBox GroupBoxFunctionFromEditor;
    internal TextBlock TextBlockFunctionFromEditor;
    internal ComboBox ComboBoxGroups;
    internal DataGrid DataGridDatabaseFunctions;
    internal MenuItem DB_MenuItemWriteToDevice;
    internal MenuItem DB_MenuItemWriteAllToDevice;
    internal MenuItem DB_MenuItemEdit;
    internal MenuItem DB_MenuItemEditSource;
    internal MenuItem DB_MenuItemCompare;
    internal MenuItem DB_MenuItemDelete;
    internal MenuItem DB_MenuItemCompareAll;
    internal TextBox TextBoxFunctionDescription;
    internal DataGrid DataGridLoadedFunctions;
    internal ContextMenu ContextMenuDeviceCommands;
    internal MenuItem FL_MenuItemReadFromDeviceAndEdit;
    internal MenuItem FL_MenuItemReadFromDeviceAndCompare;
    internal MenuItem FL_MenuItemActivateFunction;
    internal DataGrid DataGridFunctionsParameters;
    private bool _contentLoaded;

    public S4_SmartFunctions(NfcDeviceCommands nfcCmd, bool isPlugin)
    {
      this.IsPlugin = isPlugin;
      this.InitializeComponent();
      this.progress = new ProgressHandler(new Action<ProgressArg>(this.OnProgress));
      this.SmartFunctionManager = new S4_SmartFunctionManager(nfcCmd);
      this.LoadFunctionsFromDatabase();
      this.PrepareFunctionListOfSelectedGroup();
      this.LoadSettings();
      if (nfcCmd == null)
      {
        this.DB_MenuItemWriteToDevice.IsEnabled = false;
        this.DB_MenuItemWriteAllToDevice.IsEnabled = false;
        this.StackPanelDeviceCommands.IsEnabled = false;
        this.ContextMenuDeviceCommands.Visibility = Visibility.Hidden;
      }
      this.SetStopState();
    }

    private void Window_Closing(object sender, CancelEventArgs e) => this.SaveSettings();

    private void LoadSettings()
    {
      if (!this.IsPlugin)
        return;
      try
      {
        string str = PlugInLoader.GmmConfiguration.GetValue("S4_Handler", S4_HandlerWindowFunctions.ConfigVariables.SmartFunctionsGroup.ToString());
        if (!string.IsNullOrEmpty(str))
        {
          int num = this.SmartFunctionGroups.IndexOf(str);
          if (num >= 0)
            this.ComboBoxGroups.SelectedIndex = num;
        }
      }
      catch
      {
      }
    }

    private void SaveSettings()
    {
      if (!this.IsPlugin)
        return;
      try
      {
        if (this.ComboBoxGroups.SelectedItem != null)
          PlugInLoader.GmmConfiguration.SetOrUpdateValue("S4_Handler", S4_HandlerWindowFunctions.ConfigVariables.SmartFunctionsGroup.ToString(), this.ComboBoxGroups.SelectedItem.ToString());
      }
      catch
      {
      }
    }

    private void DataGridLoadedFunctions_AutoGeneratingColumn(
      object sender,
      DataGridAutoGeneratingColumnEventArgs e)
    {
      if (!(e.PropertyName == "ErrorOffset"))
        return;
      e.Column.ClipboardContentBinding.StringFormat = "x02";
    }

    private void OnProgress(ProgressArg obj)
    {
      if (this.CheckAccess())
        return;
      this.Dispatcher.Invoke((Action) (() => this.OnProgress(obj)));
    }

    private void SetRunState() => this.cancellationToken = new CancellationTokenSource().Token;

    private void SetStopState()
    {
      bool flag1 = false;
      bool flag2 = false;
      bool flag3 = false;
      if (this.DataGridDatabaseFunctions.ItemsSource != null && ((List<SmartFunctionIdent>) this.DataGridDatabaseFunctions.ItemsSource).Count > 0)
        flag2 = true;
      if (this.DataGridLoadedFunctions.ItemsSource != null && ((List<SmartFunctionIdentResultAndCalls>) this.DataGridLoadedFunctions.ItemsSource).Count > 0)
        flag1 = true;
      if (this.DataGridFunctionsParameters.ItemsSource != null && ((List<SmartFunctionParameter>) this.DataGridFunctionsParameters.ItemsSource).Count > 0)
        flag3 = true;
      this.ButtonReadFunctionParameter.IsEnabled = flag1;
      this.ButtonWriteChangedFunctionParameters.IsEnabled = flag3;
      this.DB_MenuItemDelete.IsEnabled = flag2;
      this.DB_MenuItemCompareAll.IsEnabled = flag2;
      this.ButtonSaveFromEditorToDatabase.IsEnabled = this.FunctionFromEditor != null;
      this.FL_MenuItemReadFromDeviceAndEdit.IsEnabled = flag1;
    }

    private void ComboBoxGroups_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      this.PrepareFunctionListOfSelectedGroup();
    }

    private void PrepareFunctionListOfSelectedGroup()
    {
      try
      {
        if (this.SmartFunctionManager.UsableFunctionIdent.Count > 0)
        {
          string str1 = "NotDefined";
          if (this.ComboBoxGroups.SelectedItem != null)
            str1 = this.ComboBoxGroups.SelectedItem.ToString();
          this.DataGridDatabaseFunctions.SelectedIndex = 0;
          List<SmartFunctionIdent> smartFunctionIdentList = new List<SmartFunctionIdent>();
          foreach (SmartFunctionIdent smartFunctionIdent in this.SmartFunctionManager.UsableFunctionIdent)
          {
            if (smartFunctionIdent.MemberOfGroups != null)
            {
              string memberOfGroups = smartFunctionIdent.MemberOfGroups;
              char[] separator = new char[1]{ ';' };
              foreach (string str2 in memberOfGroups.Split(separator, StringSplitOptions.RemoveEmptyEntries))
              {
                if (str2 == str1)
                  smartFunctionIdentList.Add(smartFunctionIdent);
              }
            }
            else if (str1 == "NotDefined")
              smartFunctionIdentList.Add(smartFunctionIdent);
          }
          this.DataGridDatabaseFunctions.ItemsSource = (IEnumerable) smartFunctionIdentList;
        }
        else
          this.DataGridDatabaseFunctions.ItemsSource = (IEnumerable) null;
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex, "Load function list");
      }
    }

    private void LoadFunctionsFromDatabase()
    {
      try
      {
        S4_SmartFunctionManager.DatabaseSmartFunctions = (List<SmartFunctionIdentAndCode>) null;
        this.SmartFunctionManager.RefreshDatabaseFunctions();
        if (this.SmartFunctionManager.UsableFunctionIdent.Count > 0)
        {
          this.SmartFunctionGroups = new List<string>();
          foreach (SmartFunctionIdent smartFunctionIdent in this.SmartFunctionManager.UsableFunctionIdent)
          {
            if (smartFunctionIdent.MemberOfGroups != null)
            {
              string memberOfGroups = smartFunctionIdent.MemberOfGroups;
              char[] separator = new char[1]{ ';' };
              foreach (string str in memberOfGroups.Split(separator, StringSplitOptions.RemoveEmptyEntries))
              {
                if (!this.SmartFunctionGroups.Contains(str))
                  this.SmartFunctionGroups.Add(str);
              }
            }
            else if (!this.SmartFunctionGroups.Contains("NotDefined"))
              this.SmartFunctionGroups.Add("NotDefined");
          }
          this.ComboBoxGroups.ItemsSource = (IEnumerable) this.SmartFunctionGroups;
          if (this.SmartFunctionGroups.Count <= 0)
            return;
          this.ComboBoxGroups.SelectedIndex = 0;
        }
        else
          this.ComboBoxGroups.ItemsSource = (IEnumerable) null;
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex, "Load function list");
      }
    }

    private void ButtonShowEditor_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        this.smc = new MainWindow();
        this.smc.Owner = (Window) this;
        this.smc.ShowDialog();
        this.LoadFunctionFromEditor();
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
      this.SetStopState();
    }

    private void EditFunction(byte[] theFunction)
    {
      this.smc = new MainWindow();
      this.smc.Owner = (Window) this;
      if (theFunction != null)
        this.smc.FunctionForUnCompile = theFunction;
      this.smc.ShowDialog();
      this.LoadFunctionFromEditor();
    }

    private void LoadFunctionFromEditor()
    {
      if (this.smc.FunctionCompiled != null)
      {
        this.FunctionFromEditor = new SmartFunction(this.smc.FunctionCompiled);
        this.TextBlockFunctionFromEditor.Text = this.FunctionFromEditor.ToString();
        this.FunctionManipulated = this.smc.FunctionManipulated;
      }
      else
      {
        this.FunctionFromEditor = (SmartFunction) null;
        this.TextBlockFunctionFromEditor.Text = "";
        this.FunctionManipulated = (byte[]) null;
      }
    }

    private void ButtonSaveFromEditorToDatabase_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        bool flag = S4_SmartFunctionManager.SaveSmartFunction(this.FunctionFromEditor, this.smc.FunctionDescription, this.smc.RequiredFunctions, this.smc.MemberOfGroups);
        this.SaveSettings();
        this.LoadFunctionsFromDatabase();
        this.PrepareFunctionListOfSelectedGroup();
        this.LoadSettings();
        if (flag)
          GmmMessage.Show("New function saved to data base");
        else
          GmmMessage.Show("Function overwritten in the database");
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
    }

    private async void ButtonReadLoadedFromDevice_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        this.SetRunState();
        await this.ReadLoadedFunctionsFromDevice();
        this.SetStopState();
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex, "Read loaded function from device");
      }
    }

    private async Task ReadLoadedFunctionsFromDevice()
    {
      this.DataGridLoadedFunctions.ItemsSource = (IEnumerable) null;
      await this.SmartFunctionManager.ReadLoadedFunctionsAsync(this.progress, this.cancellationToken);
      if (this.SmartFunctionManager.FunctionsInDevice != null && this.SmartFunctionManager.FunctionsInDevice.Count > 0)
      {
        this.DataGridLoadedFunctions.ItemsSource = (IEnumerable) this.SmartFunctionManager.FunctionsInDevice;
        this.DataGridLoadedFunctions.SelectedIndex = 0;
      }
      else
        GmmMessage.Show("No functions loaded", "Smart function manager");
    }

    private void DataGridLoadedFunctions_SelectionChanged(
      object sender,
      SelectionChangedEventArgs e)
    {
      this.DataGridFunctionsParameters.ItemsSource = (IEnumerable) null;
      this.SetStopState();
    }

    private async void ButtonDeleteAllFunctionsInDevice_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        this.SetRunState();
        await this.SmartFunctionManager.DeleteAllFunctionsAsync(this.progress, this.cancellationToken);
        this.DataGridLoadedFunctions.ItemsSource = (IEnumerable) null;
        this.DataGridFunctionsParameters.ItemsSource = (IEnumerable) null;
        await this.ReadLoadedFunctionsFromDevice();
        GmmMessage.Show("All loadable functions deleted", "Smart function manager");
        this.SetStopState();
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex, "Delete all functions");
      }
    }

    private async void FL_MenuItemReadFromDeviceAndEdit_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        this.SetRunState();
        SmartFunctionIdentResultAndCalls selectedFunction = (SmartFunctionIdentResultAndCalls) this.DataGridLoadedFunctions.SelectedItem;
        byte[] theFunction = await this.SmartFunctionManager.GetFunctionAsync(this.progress, this.cancellationToken, selectedFunction.Name);
        this.EditFunction(theFunction);
        this.SetStopState();
        selectedFunction = (SmartFunctionIdentResultAndCalls) null;
        theFunction = (byte[]) null;
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex, "Read from device and edit");
      }
    }

    private async void ButtonWriteFromEditorToDevice_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        this.SetRunState();
        bool flag = await this.LoadFunction(this.FunctionFromEditor.FunctionName, this.FunctionFromEditor.InterpreterVersion, this.FunctionManipulated);
        if (!flag)
          return;
        S4_SmartFunctionInfo smartFunctionInfo = await this.SmartFunctionManager.ReadSmartFunctionInfoAsync(this.progress, this.cancellationToken);
        GmmMessage.Show("Function " + this.FunctionFromEditor.FunctionName + " loaded to device" + Environment.NewLine + smartFunctionInfo.ToTextBlock(), "Smart function manager");
        smartFunctionInfo = (S4_SmartFunctionInfo) null;
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex, "Write from editor to device");
      }
      finally
      {
        this.SetStopState();
      }
    }

    private async Task<bool> LoadFunction(
      string functionName,
      byte requiredInterpreterVersion,
      byte[] functionCode)
    {
      SmartFunctionRuntimeResult loadResult = await this.SmartFunctionManager.LoadFunctionAsync(this.progress, this.cancellationToken, functionCode);
      if (!loadResult.Blocked)
      {
        await this.ReadLoadedFunctionsFromDevice();
        return true;
      }
      StringBuilder message = new StringBuilder();
      message.AppendLine("!!!! Function not loaded !!!!");
      message.AppendLine();
      if (loadResult.Error == "NotSupportedInterpreterVersion")
      {
        message.AppendLine("The firmware does't support the required interpreter Version " + requiredInterpreterVersion.ToString());
      }
      else
      {
        message.AppendLine("Error: " + loadResult.Error);
        message.AppendLine("Error byte offset: 0x" + loadResult.ErrorOffset.Value.ToString("x04"));
      }
      GmmMessage.Show(message.ToString(), "Smart function manager");
      return false;
    }

    private async void ButtonReadFunctionParameter_Click(object sender, RoutedEventArgs e)
    {
      await this.ReadFunctionParameter();
    }

    private async void DataGridLoadedFunctions_MouseDoubleClick(
      object sender,
      MouseButtonEventArgs e)
    {
      await this.ReadFunctionParameter();
    }

    private async Task ReadFunctionParameter()
    {
      if (this.DataGridLoadedFunctions.Items == null || this.DataGridLoadedFunctions.Items.Count == 0)
        return;
      try
      {
        this.SetRunState();
        SmartFunctionIdentResultAndCalls selectedFunction = (SmartFunctionIdentResultAndCalls) this.DataGridLoadedFunctions.SelectedItem;
        List<SmartFunctionParameter> parameterFromCodeList = await this.SmartFunctionManager.GetFunctionParametersFromCodeAsync(this.progress, this.cancellationToken, selectedFunction.Name);
        List<SmartFunctionParameter> functionParameterList = await this.SmartFunctionManager.GetFunctionParametersAsync(this.progress, this.cancellationToken, selectedFunction.Name);
        this.ReadedParameterList = functionParameterList;
        functionParameterList = (List<SmartFunctionParameter>) null;
        if (this.ReadedParameterList != null && this.ReadedParameterList.Count > 0)
        {
          List<SmartFunctionParameter> editList = new List<SmartFunctionParameter>();
          foreach (SmartFunctionParameter readedParameter in this.ReadedParameterList)
          {
            SmartFunctionParameter theParam = readedParameter;
            SmartFunctionParameter parameterFromCode = parameterFromCodeList.Find((Predicate<SmartFunctionParameter>) (x => x.ParameterName == theParam.ParameterName));
            SmartFunctionParameter parameterClone = theParam.Clone(parameterFromCode, true);
            editList.Add(parameterClone);
            parameterFromCode = (SmartFunctionParameter) null;
            parameterClone = (SmartFunctionParameter) null;
          }
          this.DataGridFunctionsParameters.ItemsSource = (IEnumerable) editList;
          editList = (List<SmartFunctionParameter>) null;
        }
        this.SetStopState();
        selectedFunction = (SmartFunctionIdentResultAndCalls) null;
        parameterFromCodeList = (List<SmartFunctionParameter>) null;
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex, "Read function parameters");
      }
    }

    private async void ButtonWriteChangedFunctionParameters_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        this.SetRunState();
        SmartFunctionIdentResultAndCalls selectedFunction = (SmartFunctionIdentResultAndCalls) this.DataGridLoadedFunctions.SelectedItem;
        List<SmartFunctionParameter> editedParameters = (List<SmartFunctionParameter>) this.DataGridFunctionsParameters.ItemsSource;
        List<SmartFunctionParameter> changtedParameterList = new List<SmartFunctionParameter>();
        foreach (SmartFunctionParameter readedParameter in this.ReadedParameterList)
        {
          SmartFunctionParameter originalParam = readedParameter;
          SmartFunctionParameter editParam = editedParameters.Find((Predicate<SmartFunctionParameter>) (x => x.ParameterName == originalParam.ParameterName));
          if (editParam.ParameterValue != originalParam.ParameterValue)
          {
            if (editParam.ParameterType == DataTypeCodes.ByteList && editParam.ParameterValue.Length != originalParam.Clone(removeByteListLength: true).ParameterValue.Length)
              throw new Exception("Parameter length changing not alowed.");
            changtedParameterList.Add(editParam);
          }
          editParam = (SmartFunctionParameter) null;
        }
        if (changtedParameterList.Count > 0)
        {
          SmartFunctionRuntimeResult setResult = await this.SmartFunctionManager.SetFunctionParametersAsync(this.progress, this.cancellationToken, selectedFunction.Name, changtedParameterList);
          if (!setResult.Blocked)
          {
            GmmMessage.Show("Changes written to device", "Smart function manager");
          }
          else
          {
            StringBuilder message = new StringBuilder();
            message.AppendLine("!!!! Write changes error !!!!");
            message.AppendLine("Maybe parameters before the error are changed.");
            message.AppendLine("Please check all parameter values.");
            message.AppendLine();
            message.AppendLine("Error: " + setResult.Error);
            message.AppendLine("Error byte offset: 0x" + setResult.ErrorOffset.Value.ToString("x04"));
            GmmMessage.Show(message.ToString(), "Smart function manager");
            message = (StringBuilder) null;
          }
          setResult = (SmartFunctionRuntimeResult) null;
        }
        this.SetStopState();
        selectedFunction = (SmartFunctionIdentResultAndCalls) null;
        editedParameters = (List<SmartFunctionParameter>) null;
        changtedParameterList = (List<SmartFunctionParameter>) null;
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex, "Write changed parameters");
      }
    }

    private void DB_MenuItemDelete_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        if (this.DataGridDatabaseFunctions.SelectedItem == null)
          return;
        this.SaveSettings();
        SmartFunctionIdent selectedItem = (SmartFunctionIdent) this.DataGridDatabaseFunctions.SelectedItem;
        S4_SmartFunctionManager.DeleteFunctionFromDatabase(selectedItem);
        GmmMessage.Show_Ok("Function deleted: " + selectedItem.Name);
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
      finally
      {
        this.LoadFunctionsFromDatabase();
        this.PrepareFunctionListOfSelectedGroup();
        this.LoadSettings();
      }
    }

    private async void DB_MenuItemWriteToDevice_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        if (this.DataGridDatabaseFunctions.SelectedItem == null)
          return;
        SmartFunctionIdent selectedFunction = (SmartFunctionIdent) this.DataGridDatabaseFunctions.SelectedItem;
        this.SetRunState();
        byte[] theFunction = S4_SmartFunctionManager.GetSelectedFunctionCodeFromDatabase(selectedFunction);
        if (await this.LoadFunction(selectedFunction.Name, selectedFunction.InterpreterVersion, theFunction))
          GmmMessage.Show("Function " + selectedFunction.Name + " loaded to device", "Smart function manager");
        this.SetStopState();
        selectedFunction = (SmartFunctionIdent) null;
        theFunction = (byte[]) null;
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex, "Write from database to device");
      }
    }

    private void DB_MenuItemEdit_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        if (this.DataGridDatabaseFunctions.SelectedItem == null)
          return;
        this.EditFunction(S4_SmartFunctionManager.GetSelectedFunctionCodeFromDatabase((SmartFunctionIdent) this.DataGridDatabaseFunctions.SelectedItem));
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex, "Edit function from database.");
      }
      this.SetStopState();
    }

    private void DB_MenuItemEditSource_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        if (this.DataGridDatabaseFunctions.SelectedItem == null)
          return;
        SmartFunctionIdent selectedItem = (SmartFunctionIdent) this.DataGridDatabaseFunctions.SelectedItem;
        this.smc = new MainWindow();
        this.smc.Owner = (Window) this;
        this.smc.OpenFile(selectedItem.Name);
        this.smc.ShowDialog();
        this.LoadFunctionFromEditor();
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex, "Edit function from database.");
      }
      this.SetStopState();
    }

    private async void DB_MenuItemWriteAllToDevice_Click(object sender, RoutedEventArgs e)
    {
      StringBuilder info;
      if (this.DataGridDatabaseFunctions.Items == null || this.DataGridDatabaseFunctions.Items.Count == 0)
      {
        info = (StringBuilder) null;
      }
      else
      {
        info = new StringBuilder();
        info.AppendLine("Loaded functions:");
        info.AppendLine();
        try
        {
          this.SetRunState();
          foreach (object theItem in (IEnumerable) this.DataGridDatabaseFunctions.Items)
          {
            SmartFunctionIdent theFunction = (SmartFunctionIdent) theItem;
            string smartFunctionInfo = theFunction.ToString();
            S4_SmartFunctionManager.S4_SmartFunctionLogger.Trace(smartFunctionInfo);
            byte[] functionBytes = S4_SmartFunctionManager.GetSelectedFunctionCodeFromDatabase(theFunction);
            SmartFunctionRuntimeResult result = await this.SmartFunctionManager.LoadFunctionAsync(this.progress, this.cancellationToken, functionBytes);
            if (result.FunctionResult != 0)
              info.AppendLine(smartFunctionInfo + " -> " + result.FunctionResult.ToString());
            else
              info.AppendLine(smartFunctionInfo);
            theFunction = (SmartFunctionIdent) null;
            smartFunctionInfo = (string) null;
            functionBytes = (byte[]) null;
            result = (SmartFunctionRuntimeResult) null;
          }
          await this.ReadLoadedFunctionsFromDevice();
          this.SetStopState();
          GmmMessage.Show(info.ToString(), "Loaded smart functions");
          info = (StringBuilder) null;
        }
        catch (Exception ex)
        {
          info.AppendLine();
          info.AppendLine("Write all database functions to device error.");
          info.AppendLine("Error by loading last function at the list");
          ExceptionViewer.Show(ex, info.ToString());
          info = (StringBuilder) null;
        }
      }
    }

    private void DB_MenuItemCompare_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        if (this.DataGridDatabaseFunctions.SelectedItem == null)
          return;
        SmartFunctionIdent selectedItem = (SmartFunctionIdent) this.DataGridDatabaseFunctions.SelectedItem;
        byte[] codeFromDatabase = S4_SmartFunctionManager.GetSelectedFunctionCodeFromDatabase(selectedItem);
        this.smc = new MainWindow();
        this.smc.CompareCodeToSource(codeFromDatabase);
        GmmMessage.Show("Function " + selectedItem.Name + " is eaqual to source");
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex, "Compare error");
      }
    }

    private void DB_MenuItemCompareAll_Click(object sender, RoutedEventArgs e)
    {
      string str = "";
      try
      {
        int num = 0;
        foreach (SmartFunctionIdent selectedFunction in (IEnumerable) this.DataGridDatabaseFunctions.Items)
        {
          str = selectedFunction.Name;
          byte[] codeFromDatabase = S4_SmartFunctionManager.GetSelectedFunctionCodeFromDatabase(selectedFunction);
          this.smc = new MainWindow();
          this.smc.CompareCodeToSource(codeFromDatabase);
          ++num;
        }
        GmmMessage.Show(num.ToString() + " functions checked. All function are eaqual to source");
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex, "Compare error. Function name: " + str);
      }
    }

    private async void ButtonCompareAllToDevice_Click(object sender, RoutedEventArgs e)
    {
      string functionName = "";
      try
      {
        this.SetRunState();
        await this.ReadLoadedFunctionsFromDevice();
        int count = 0;
        foreach (SmartFunctionIdentResultAndCalls selectedFunction in (IEnumerable) this.DataGridLoadedFunctions.Items)
        {
          functionName = selectedFunction.Name;
          byte[] function = await this.SmartFunctionManager.GetFunctionAsync(this.progress, this.cancellationToken, selectedFunction.Name);
          this.smc = new MainWindow();
          this.smc.CompareCodeToSource(function);
          ++count;
          function = (byte[]) null;
        }
        GmmMessage.Show(count.ToString() + " functions checked. All function are eaqual to source");
        functionName = (string) null;
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex, "Compare error. Function name: " + functionName);
        functionName = (string) null;
      }
      finally
      {
        this.SetStopState();
      }
    }

    private async void FL_MenuItemReadFromDeviceAndCompare_Click(object sender, RoutedEventArgs e)
    {
      string functionName = "";
      try
      {
        if (this.DataGridDatabaseFunctions.SelectedItem == null)
        {
          functionName = (string) null;
        }
        else
        {
          SmartFunctionIdentResultAndCalls selectedFunction = (SmartFunctionIdentResultAndCalls) this.DataGridLoadedFunctions.SelectedItem;
          functionName = selectedFunction.Name;
          byte[] function = await this.SmartFunctionManager.GetFunctionAsync(this.progress, this.cancellationToken, functionName);
          this.smc = new MainWindow();
          this.smc.CompareCodeToSource(function);
          GmmMessage.Show("Function " + selectedFunction.Name + " is eaqual to source");
          selectedFunction = (SmartFunctionIdentResultAndCalls) null;
          function = (byte[]) null;
          functionName = (string) null;
        }
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex, "Compare error. Function name: " + functionName);
        functionName = (string) null;
      }
    }

    private void DataGridLoadedFunctions_ContextMenuOpening(object sender, ContextMenuEventArgs e)
    {
      if (this.DataGridDatabaseFunctions.SelectedItem == null)
      {
        this.ContextMenuDeviceCommands.IsEnabled = false;
      }
      else
      {
        this.ContextMenuDeviceCommands.IsEnabled = true;
        SmartFunctionIdentResultAndCalls selectedItem = (SmartFunctionIdentResultAndCalls) this.DataGridLoadedFunctions.SelectedItem;
        switch (selectedItem.FunctionResult)
        {
          case SmartFunctionResult.NoError:
            this.FL_MenuItemActivateFunction.Header = (object) ("Deactivate smart function: " + selectedItem.Name);
            break;
          case SmartFunctionResult.DeactivatedByCommand:
            this.FL_MenuItemActivateFunction.Header = (object) ("Activate smart function: " + selectedItem.Name);
            break;
          default:
            this.FL_MenuItemActivateFunction.Header = (object) ("Smart function: " + selectedItem.Name + " in blocked");
            this.FL_MenuItemActivateFunction.IsEnabled = false;
            return;
        }
        this.FL_MenuItemActivateFunction.IsEnabled = true;
      }
    }

    private async void FL_MenuItemActivateFunction_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        this.SetRunState();
        SmartFunctionIdentResultAndCalls selectedFunction = (SmartFunctionIdentResultAndCalls) this.DataGridLoadedFunctions.SelectedItem;
        bool activate = false;
        if (selectedFunction.FunctionResult == SmartFunctionResult.DeactivatedByCommand)
          activate = true;
        int num = (int) await this.SmartFunctionManager.NfcCmd.SetSmartFunctionActivationAsync(this.progress, this.cancellationToken, selectedFunction.Name, activate);
        await this.ReadLoadedFunctionsFromDevice();
        this.SetStopState();
        selectedFunction = (SmartFunctionIdentResultAndCalls) null;
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
    }

    private void DataGridDatabaseFunctions_SelectionChanged(
      object sender,
      SelectionChangedEventArgs e)
    {
      try
      {
        this.TextBoxFunctionDescription.Text = "";
        if (this.DataGridDatabaseFunctions.SelectedItem == null)
          return;
        SmartFunctionIdent selectedItem = (SmartFunctionIdent) this.DataGridDatabaseFunctions.SelectedItem;
        StringBuilder stringBuilder = new StringBuilder();
        if (selectedItem.FunctionDescription != null)
          stringBuilder.AppendLine(selectedItem.FunctionDescription);
        if (selectedItem.RequiredFunctions != null)
          stringBuilder.AppendLine("Required functions: " + selectedItem.RequiredFunctions);
        if (selectedItem.MemberOfGroups != null)
          stringBuilder.AppendLine("Member of groups: " + selectedItem.MemberOfGroups);
        this.TextBoxFunctionDescription.Text = stringBuilder.ToString();
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex, "Edit function from database.");
      }
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/S4_Handler;component/userinterface/s4_smartfunctions.xaml", UriKind.Relative));
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
          this.gmmCorporateControl1 = (StartupLib.GmmCorporateControl) target;
          break;
        case 3:
          this.ButtonShowEditor = (Button) target;
          this.ButtonShowEditor.Click += new RoutedEventHandler(this.ButtonShowEditor_Click);
          break;
        case 4:
          this.StackPanelDeviceCommands = (StackPanel) target;
          break;
        case 5:
          this.ButtonReadLoadedFromDevice = (Button) target;
          this.ButtonReadLoadedFromDevice.Click += new RoutedEventHandler(this.ButtonReadLoadedFromDevice_Click);
          break;
        case 6:
          this.ButtonDeleteAllFunctionsInDevice = (Button) target;
          this.ButtonDeleteAllFunctionsInDevice.Click += new RoutedEventHandler(this.ButtonDeleteAllFunctionsInDevice_Click);
          break;
        case 7:
          this.ButtonWriteFromEditorToDevice = (Button) target;
          this.ButtonWriteFromEditorToDevice.Click += new RoutedEventHandler(this.ButtonWriteFromEditorToDevice_Click);
          break;
        case 8:
          this.ButtonCompareAllToDevice = (Button) target;
          this.ButtonCompareAllToDevice.Click += new RoutedEventHandler(this.ButtonCompareAllToDevice_Click);
          break;
        case 9:
          this.ButtonReadFunctionParameter = (Button) target;
          this.ButtonReadFunctionParameter.Click += new RoutedEventHandler(this.ButtonReadFunctionParameter_Click);
          break;
        case 10:
          this.ButtonWriteChangedFunctionParameters = (Button) target;
          this.ButtonWriteChangedFunctionParameters.Click += new RoutedEventHandler(this.ButtonWriteChangedFunctionParameters_Click);
          break;
        case 11:
          this.ButtonSaveFromEditorToDatabase = (Button) target;
          this.ButtonSaveFromEditorToDatabase.Click += new RoutedEventHandler(this.ButtonSaveFromEditorToDatabase_Click);
          break;
        case 12:
          this.GroupBoxFunctionFromEditor = (GroupBox) target;
          break;
        case 13:
          this.TextBlockFunctionFromEditor = (TextBlock) target;
          break;
        case 14:
          this.ComboBoxGroups = (ComboBox) target;
          this.ComboBoxGroups.SelectionChanged += new SelectionChangedEventHandler(this.ComboBoxGroups_SelectionChanged);
          break;
        case 15:
          this.DataGridDatabaseFunctions = (DataGrid) target;
          this.DataGridDatabaseFunctions.SelectionChanged += new SelectionChangedEventHandler(this.DataGridDatabaseFunctions_SelectionChanged);
          break;
        case 16:
          this.DB_MenuItemWriteToDevice = (MenuItem) target;
          this.DB_MenuItemWriteToDevice.Click += new RoutedEventHandler(this.DB_MenuItemWriteToDevice_Click);
          break;
        case 17:
          this.DB_MenuItemWriteAllToDevice = (MenuItem) target;
          this.DB_MenuItemWriteAllToDevice.Click += new RoutedEventHandler(this.DB_MenuItemWriteAllToDevice_Click);
          break;
        case 18:
          this.DB_MenuItemEdit = (MenuItem) target;
          this.DB_MenuItemEdit.Click += new RoutedEventHandler(this.DB_MenuItemEdit_Click);
          break;
        case 19:
          this.DB_MenuItemEditSource = (MenuItem) target;
          this.DB_MenuItemEditSource.Click += new RoutedEventHandler(this.DB_MenuItemEditSource_Click);
          break;
        case 20:
          this.DB_MenuItemCompare = (MenuItem) target;
          this.DB_MenuItemCompare.Click += new RoutedEventHandler(this.DB_MenuItemCompare_Click);
          break;
        case 21:
          this.DB_MenuItemDelete = (MenuItem) target;
          this.DB_MenuItemDelete.Click += new RoutedEventHandler(this.DB_MenuItemDelete_Click);
          break;
        case 22:
          this.DB_MenuItemCompareAll = (MenuItem) target;
          this.DB_MenuItemCompareAll.Click += new RoutedEventHandler(this.DB_MenuItemCompareAll_Click);
          break;
        case 23:
          this.TextBoxFunctionDescription = (TextBox) target;
          break;
        case 24:
          this.DataGridLoadedFunctions = (DataGrid) target;
          this.DataGridLoadedFunctions.SelectionChanged += new SelectionChangedEventHandler(this.DataGridLoadedFunctions_SelectionChanged);
          this.DataGridLoadedFunctions.MouseDoubleClick += new MouseButtonEventHandler(this.DataGridLoadedFunctions_MouseDoubleClick);
          this.DataGridLoadedFunctions.AutoGeneratingColumn += new EventHandler<DataGridAutoGeneratingColumnEventArgs>(this.DataGridLoadedFunctions_AutoGeneratingColumn);
          this.DataGridLoadedFunctions.ContextMenuOpening += new ContextMenuEventHandler(this.DataGridLoadedFunctions_ContextMenuOpening);
          break;
        case 25:
          this.ContextMenuDeviceCommands = (ContextMenu) target;
          break;
        case 26:
          this.FL_MenuItemReadFromDeviceAndEdit = (MenuItem) target;
          this.FL_MenuItemReadFromDeviceAndEdit.Click += new RoutedEventHandler(this.FL_MenuItemReadFromDeviceAndEdit_Click);
          break;
        case 27:
          this.FL_MenuItemReadFromDeviceAndCompare = (MenuItem) target;
          this.FL_MenuItemReadFromDeviceAndCompare.Click += new RoutedEventHandler(this.FL_MenuItemReadFromDeviceAndCompare_Click);
          break;
        case 28:
          this.FL_MenuItemActivateFunction = (MenuItem) target;
          this.FL_MenuItemActivateFunction.Click += new RoutedEventHandler(this.FL_MenuItemActivateFunction_Click);
          break;
        case 29:
          this.DataGridFunctionsParameters = (DataGrid) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
