// Decompiled with JetBrains decompiler
// Type: SmartFunctionCompiler.MainWindow
// Assembly: SmartFunctionCompiler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: E49EBEEE-4E03-4F25-A9DE-0F245CFB9A90
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\SmartFunctionCompiler.exe

using CommonWPF;
using HandlerLib;
using Microsoft.Win32;
using SmartFunctionCompiler.Properties;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using ZR_ClassLibrary;

#nullable disable
namespace SmartFunctionCompiler
{
  public partial class MainWindow : Window, IComponentConnector
  {
    public byte[] FunctionForUnCompile;
    public byte[] FunctionCompiled;
    public byte[] FunctionManipulated;
    public string FunctionDescription;
    public string RequiredFunctions;
    public string MemberOfGroups;
    private string FileOpenPath;
    private DateTime FileOpenPathLastWriteTime;
    private ushort FunctionOffset;
    internal TabItem TabItemEditorAndCompiler;
    internal MenuItem MenuItemFile;
    internal MenuItem MenuItemFileOpen;
    internal MenuItem MenuItemFileSave;
    internal MenuItem MenuItemFileSaveAs;
    internal MenuItem MenuItemReloadSource;
    internal MenuItem MenuItemTests;
    internal MenuItem MenuItemCheckCalendar2000;
    internal Button ButtonCompile;
    internal Button ButtonDeCompile;
    internal Button ButtonDeleteLineNumbers;
    internal TextBox TextBoxCode;
    internal TextBox TextBoxResult;
    internal TabItem TabItemSimulationParameters;
    internal TabControl TabControl1;
    internal TabItem TabItemMnemonics;
    internal TextBox TextBoxMnemonics;
    internal TabItem TabItemSimulator;
    internal TextBox TextBoxRunEvents;
    internal Button ButtonRun;
    internal Button ButtonReset;
    internal Button ButtonSingleEvent;
    internal Button ButtonLoadSimulator;
    internal TextBox TextBoxParameters;
    internal TextBox TextBoxDeviceTime;
    internal TextBox TextBoxCycleTime;
    internal TextBox TextBoxVolume;
    internal TextBox TextBoxFlowVolume;
    internal TextBox TextBoxReturnVolume;
    internal TextBox TextBoxFlow;
    internal TextBox TextBoxFlowIncrement;
    internal ComboBox ComboBoxParameterName;
    internal TextBox TextBoxInitValue;
    internal TextBox TextBoxValueIncrement;
    internal TextBox TextBoxValue;
    internal TextBox TextBoxOutput;
    private bool _contentLoaded;

    public MainWindow()
    {
      this.InitializeComponent();
      string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Mnemonics.txt");
      try
      {
        using (StreamReader streamReader = new StreamReader(path))
          this.TextBoxMnemonics.Text = streamReader.ReadToEnd();
      }
      catch
      {
        this.TextBoxMnemonics.Text = "Mnemonics file not available";
      }
      this.TextBoxDeviceTime.Text = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss");
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      string str = "";
      try
      {
        string[] commandLineArgs = Environment.GetCommandLineArgs();
        if (this.FunctionForUnCompile != null)
        {
          str = "Function for un-compile: " + SmartFunctionCompiler.Compiler.GetHexStringFromArray(this.FunctionForUnCompile);
          this.TextBoxCode.Text = new SmartFunction(this.FunctionForUnCompile).GetUncompiled();
        }
        else if (commandLineArgs != null && commandLineArgs.Length > 1)
        {
          if (!File.Exists(commandLineArgs[1]))
            return;
          this.LoadFile(commandLineArgs[1]);
        }
        else
        {
          if (string.IsNullOrEmpty(Settings.Default.LastFile))
            return;
          if (!File.Exists(Settings.Default.LastFile))
            Settings.Default.LastFile = "";
          else
            this.LoadFile(Settings.Default.LastFile);
        }
      }
      catch (Exception ex)
      {
        this.TextBoxCode.Text = "Load exception:" + Environment.NewLine + str + Environment.NewLine + ex.ToString();
      }
    }

    private void Window_Closing(object sender, CancelEventArgs e)
    {
      Settings.Default.Save();
      if (SmartFunctionCompiler.Compiler.TheOnlyOneCompiler == null)
      {
        this.FunctionCompiled = (byte[]) null;
      }
      else
      {
        this.FunctionCompiled = SmartFunctionCompiler.Compiler.TheOnlyOneCompiler.CompiledFunctionCode;
        this.FunctionManipulated = SmartFunctionCompiler.Compiler.GetBinaryCodeFromText(this.TextBoxResult.Text);
        this.FunctionDescription = SmartFunctionCompiler.Compiler.TheOnlyOneCompiler.FunctionDescription;
        this.RequiredFunctions = SmartFunctionCompiler.Compiler.TheOnlyOneCompiler.RequiredFunctions;
        this.MemberOfGroups = SmartFunctionCompiler.Compiler.TheOnlyOneCompiler.MemberOfGroups;
      }
    }

    private void MenuItemFileOpen_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        OpenFileDialog openFileDialog = new OpenFileDialog();
        openFileDialog.DefaultExt = ".lsf";
        openFileDialog.Filter = "Smart function files (.lsf)|*.lsf";
        openFileDialog.CheckFileExists = true;
        if (!string.IsNullOrEmpty(Settings.Default.LastPath) && Directory.Exists(Settings.Default.LastPath))
          openFileDialog.InitialDirectory = Settings.Default.LastPath;
        bool? nullable = openFileDialog.ShowDialog();
        bool flag = true;
        if (!(nullable.GetValueOrDefault() == flag & nullable.HasValue))
          return;
        Settings.Default.LastPath = Path.GetDirectoryName(openFileDialog.FileName);
        this.OpenFile(openFileDialog.FileName);
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex, "File open error");
      }
    }

    public void OpenFile(string filenNameOrPath)
    {
      string str = filenNameOrPath;
      if (!str.Contains<char>(Path.DirectorySeparatorChar))
      {
        str = Path.Combine(Settings.Default.LastPath, filenNameOrPath + ".lsf");
        if (!File.Exists(str))
        {
          str = Path.Combine(Settings.Default.LastPath, "..", filenNameOrPath + ".lsf");
          if (!File.Exists(str))
            throw new Exception("File not found: " + str + Environment.NewLine + "Please be sure the last file open path of SmartFunction compiler is the path of the smart functions.");
        }
      }
      this.LoadFile(str);
      this.TextBoxResult.Clear();
      this.TextBoxOutput.Clear();
      this.TextBoxParameters.Clear();
      this.ButtonDeCompile.IsEnabled = false;
      this.ButtonLoadSimulator.IsEnabled = false;
      this.ButtonRun.IsEnabled = false;
      this.ButtonSingleEvent.IsEnabled = false;
      this.ButtonReset.IsEnabled = false;
      this.TabItemEditorAndCompiler.IsSelected = true;
      this.TabItemSimulationParameters.IsEnabled = false;
    }

    private void LoadFile(string filePath)
    {
      this.TabControl1.SelectedItem = (object) this.TabItemMnemonics;
      using (StreamReader streamReader = new StreamReader(filePath))
      {
        StringBuilder stringBuilder = new StringBuilder();
        while (true)
        {
          string str = streamReader.ReadLine();
          if (str != null)
            stringBuilder.AppendLine(str);
          else
            break;
        }
        this.TextBoxCode.Text = stringBuilder.ToString();
        Settings.Default.LastFile = filePath;
      }
      this.Title = "Smart Function Compiler  (" + Path.GetFileName(Settings.Default.LastFile) + ")";
      this.FileOpenPath = filePath;
      this.FileOpenPathLastWriteTime = File.GetLastWriteTime(filePath);
    }

    private void MenuItemFileSave_Click(object sender, RoutedEventArgs e) => this.SaveFile();

    private void MenuItemFileSaveAs_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        this.ClearLineNumbers();
        int num1 = this.TextBoxCode.Text.IndexOf(':');
        int num2 = this.TextBoxCode.Text.IndexOf(Environment.NewLine);
        int num3 = this.TextBoxCode.Text.IndexOf('#');
        if (num3 > num1 && num3 < num2)
          num2 = num3;
        string path2 = (string) null;
        if (num1 > 0 && num2 > 0 && num2 > num1)
          path2 = this.TextBoxCode.Text.Substring(num1 + 1, num2 - num1 - 1).Trim();
        SaveFileDialog saveFileDialog = new SaveFileDialog();
        if (!string.IsNullOrEmpty(path2))
        {
          if (Settings.Default.LastFile != null && Settings.Default.LastFile != string.Empty)
          {
            string directoryName = Path.GetDirectoryName(Settings.Default.LastFile);
            saveFileDialog.FileName = Path.Combine(directoryName, path2);
          }
          else
            saveFileDialog.FileName = path2;
        }
        else if (Settings.Default.LastFile != null)
          saveFileDialog.FileName = Settings.Default.LastFile;
        saveFileDialog.DefaultExt = ".lsf";
        saveFileDialog.Filter = "Smart function files (.lsf)|*.lsf";
        bool? nullable = saveFileDialog.ShowDialog();
        bool flag = true;
        if (!(nullable.GetValueOrDefault() == flag & nullable.HasValue))
          return;
        Settings.Default.LastFile = saveFileDialog.FileName;
        this.SaveFile();
        this.LoadFile(saveFileDialog.FileName);
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex, "File SaveAs error");
      }
    }

    private void ButtonCompile_Click(object sender, RoutedEventArgs e)
    {
      this.ClearLineNumbers();
      this.TextBoxResult.Clear();
      this.TextBoxParameters.Clear();
      this.TextBoxOutput.Clear();
      this.TabControl1.SelectedItem = (object) this.TabItemSimulator;
      try
      {
        this.ButtonLoadSimulator.IsEnabled = false;
        this.TabItemSimulationParameters.IsEnabled = false;
        this.Compile();
        byte[] binaryCodeFromText = SmartFunctionCompiler.Compiler.GetBinaryCodeFromText(this.TextBoxResult.Text);
        this.ShowDiff(SmartFunctionCompiler.Compiler.TheOnlyOneCompiler.CompiledFunctionCode, binaryCodeFromText);
        this.ButtonLoadSimulator.IsEnabled = true;
        this.ButtonDeCompile.IsEnabled = true;
      }
      catch (Exception ex)
      {
        this.TextBoxResult.Text = ex.Message + Environment.NewLine + Environment.NewLine + ex.StackTrace;
      }
    }

    private void Compile()
    {
      string[] linesFromSourceText = this.GetLinesFromSourceText();
      this.SetLineNumbers();
      SmartFunctionCompiler.Compiler.TheOnlyOneCompiler = new SmartFunctionCompiler.Compiler(linesFromSourceText);
      string message = SmartFunctionCompiler.Compiler.TheOnlyOneCompiler.Compile();
      if (message.Length > 0)
        GmmMessage.Show(message, "Compiler warnings");
      this.TextBoxResult.Text = SmartFunctionCompiler.Compiler.TheOnlyOneCompiler.ToString();
    }

    private void ShowDiff(byte[] compiled, byte[] backCompiled)
    {
      int length = compiled.Length;
      if (backCompiled.Length < length)
        length = backCompiled.Length;
      int index = 0;
      while (index < length && (int) compiled[index] == (int) backCompiled[index])
        ++index;
      if (compiled.Length == backCompiled.Length && index == length)
        return;
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine("**** Back created code different from compiled code ****");
      stringBuilder.AppendLine();
      stringBuilder.AppendLine("Diff offset = " + index.ToString());
      stringBuilder.AppendLine();
      stringBuilder.AppendLine("Compiled code:");
      stringBuilder.AppendLine(SmartFunctionCompiler.Compiler.GetHexStringFromArray(compiled));
      stringBuilder.AppendLine();
      stringBuilder.AppendLine("Back crearted code:");
      stringBuilder.AppendLine(SmartFunctionCompiler.Compiler.GetHexStringFromArray(backCompiled));
      GmmMessage.Show_Ok(stringBuilder.ToString());
    }

    private void ButtonDeCompile_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        this.TextBoxCode.Text = new SmartFunction(SmartFunctionCompiler.Compiler.TheOnlyOneCompiler.CompiledFunctionCode).GetUncompiled();
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.ToString());
      }
    }

    private void SaveFile()
    {
      this.ClearLineNumbers();
      string lastFile = Settings.Default.LastFile;
      using (StreamWriter streamWriter = new StreamWriter(lastFile))
        streamWriter.Write(this.TextBoxCode.Text);
      this.FileOpenPath = lastFile;
      this.FileOpenPathLastWriteTime = File.GetLastWriteTime(lastFile);
    }

    private string[] GetLinesFromSourceText()
    {
      string[] array = this.TextBoxCode.Text.Split(new string[1]
      {
        Environment.NewLine
      }, StringSplitOptions.None);
      if (array[array.Length - 1].Length == 0)
        Array.Resize<string>(ref array, array.Length - 1);
      return array;
    }

    private void ClearLineNumbers()
    {
      StringBuilder stringBuilder = new StringBuilder();
      foreach (string str in this.GetLinesFromSourceText())
      {
        if (str.Length > 5 && str[4] == ':' && str[5] == ' ')
          str = str.Length != 6 ? str.Substring(6) : "";
        stringBuilder.AppendLine(str);
      }
      this.TextBoxCode.Text = stringBuilder.ToString();
    }

    private void SetLineNumbers()
    {
      StringBuilder stringBuilder = new StringBuilder();
      string[] linesFromSourceText = this.GetLinesFromSourceText();
      for (int index = 0; index < linesFromSourceText.Length; ++index)
      {
        string str = linesFromSourceText[index];
        if (str.Length > 5 && str[4] == ':' && str[5] == ' ')
          str = str.Length != 6 ? str.Substring(6) : "";
        stringBuilder.Append(index.ToString("d04") + ": ");
        stringBuilder.AppendLine(str);
      }
      this.TextBoxCode.Text = stringBuilder.ToString();
    }

    private void ButtonDeleteLineNumbers_Click(object sender, RoutedEventArgs e)
    {
      this.ClearLineNumbers();
    }

    private void ButtonLoadSimulator_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        byte[] binaryCodeFromText = SmartFunctionCompiler.Compiler.GetBinaryCodeFromText(this.TextBoxResult.Text);
        FunctionLoader.ResetStorage();
        ushort flashLoadOffset = FunctionLoader.FlashLoadOffset;
        FunctionLoader.LoadFunction(binaryCodeFromText);
        if (Interpreter.IsError)
        {
          StringBuilder stringBuilder = new StringBuilder();
          stringBuilder.AppendLine("*** Loader error ***");
          stringBuilder.AppendLine("CheckResult: " + Interpreter.Error.ToString());
          stringBuilder.AppendLine("ErrorOffset: " + Interpreter.ErrorOffset.ToString("x03"));
          this.TextBoxParameters.Text = stringBuilder.ToString();
        }
        else
        {
          this.TextBoxOutput.Text = "";
          Interpreter.RunLog.Clear();
          Interpreter.RunLog.AppendLine("Function loaded");
          this.FunctionOffset = flashLoadOffset;
          new FunctionAccessStorage(FunctionLoader.FlashStorage, this.FunctionOffset).GetOverview();
          this.SetInterpreterValues();
          Interpreter.RunCondition = Interpreter.RunConditions.FunctionLoad;
          Interpreter.Run(this.FunctionOffset);
          this.SimulationCycle();
          this.ShowResults();
          string text = this.TextBoxParameters.Text;
          this.TextBoxParameters.Text = SmartFunctionCompiler.Compiler.TheOnlyOneCompiler.FunctionDescription + Environment.NewLine + Environment.NewLine + text;
          this.TabItemSimulationParameters.IsEnabled = true;
        }
        this.ButtonSingleEvent.IsEnabled = true;
        this.ButtonReset.IsEnabled = true;
        this.ButtonRun.IsEnabled = true;
        return;
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
      this.ButtonSingleEvent.IsEnabled = false;
      this.ButtonReset.IsEnabled = false;
      this.ButtonRun.IsEnabled = false;
    }

    private void TextBoxCode_TextChanged(object sender, TextChangedEventArgs e)
    {
      this.ButtonLoadSimulator.IsEnabled = false;
      this.TabItemSimulationParameters.IsEnabled = false;
    }

    private void ButtonReset_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        Interpreter.RunLog.Clear();
        Interpreter.RunLog.AppendLine();
        Interpreter.RunLog.AppendLine("Hardware reset");
        this.SetInterpreterValues();
        Interpreter.RunCondition = Interpreter.RunConditions.HardwareReset;
        Interpreter.Run(this.FunctionOffset);
        this.SimulationCycle();
        this.ShowResults();
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex, "*** Exception without error management ***");
      }
    }

    private void ButtonSingleEvent_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        Interpreter.RunLog.Clear();
        Interpreter.RunLog.AppendLine();
        Interpreter.RunLog.AppendLine("Single event");
        this.SetInterpreterValues();
        Interpreter.RunCondition = Interpreter.RunConditions.Event;
        Interpreter.Run(this.FunctionOffset);
        this.SimulationCycle();
        this.ShowResults();
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex, "*** Exception without error management ***");
      }
    }

    private void ButtonRun_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        int result;
        if (!int.TryParse(this.TextBoxRunEvents.Text, out result))
          throw new Exception("Illegal number of run events");
        Interpreter.RunLog.Clear();
        Interpreter.RunLog.AppendLine();
        Interpreter.RunLog.AppendLine("Multi events: " + result.ToString());
        while (result-- > 0)
        {
          this.SetInterpreterValues();
          Interpreter.RunCondition = Interpreter.RunConditions.Event;
          Interpreter.Run(this.FunctionOffset);
          this.SimulationCycle();
        }
        this.ShowResults();
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex, "*** Exception without error management ***");
      }
    }

    private void ShowResults()
    {
      this.ShowRegAndParam();
      this.TextBoxOutput.AppendText(Interpreter.RunLog.ToString());
      this.TextBoxOutput.ScrollToEnd();
    }

    private void SetInterpreterValues()
    {
      DateTime result;
      if (DateTime.TryParse(this.TextBoxDeviceTime.Text, out result))
        Interpreter.DeviceTime = CalendarBase2000.Cal_GetMeterTime(result);
      uint.TryParse(this.TextBoxCycleTime.Text, out Interpreter.CycleTime);
      double.TryParse(this.TextBoxVolume.Text, out Interpreter.Volume);
      double.TryParse(this.TextBoxFlowVolume.Text, out Interpreter.FlowVolume);
      double.TryParse(this.TextBoxReturnVolume.Text, out Interpreter.ReturnVolume);
      float.TryParse(this.TextBoxFlow.Text, out Interpreter.Flow);
      float.TryParse(this.TextBoxFlowIncrement.Text, out Interpreter.FlowIncrement);
    }

    private void SimulationCycle()
    {
      Interpreter.SimulationCycle();
      this.TextBoxDeviceTime.Text = CalendarBase2000.Cal_GetDateTime(Interpreter.DeviceTime).ToString("dd.MM.yyyy HH:mm:ss");
      this.TextBoxVolume.Text = Interpreter.Volume.ToString();
      this.TextBoxFlowVolume.Text = Interpreter.FlowVolume.ToString();
      this.TextBoxReturnVolume.Text = Interpreter.ReturnVolume.ToString();
      this.TextBoxFlow.Text = Interpreter.Flow.ToString();
    }

    private void ShowRegAndParam()
    {
      StringBuilder stringBuilder1 = new StringBuilder();
      stringBuilder1.AppendLine("RegA = " + Interpreter.RegA.GetValueAndRegisterType());
      stringBuilder1.AppendLine("RegB = " + Interpreter.RegB.GetValueAndRegisterType());
      FunctionAccessStorage functionAccessStorage = new FunctionAccessStorage(FunctionLoader.FlashStorage, this.FunctionOffset);
      for (byte parameterNumber = 0; (int) parameterNumber < (int) functionAccessStorage.NumberOfRuntimeParameters; ++parameterNumber)
      {
        RuntimeParameter parameterObject = functionAccessStorage.GetParameterObject(parameterNumber);
        string name = parameterObject.Name;
        if (parameterObject.TypeCode != DataTypeCodes.ByteList)
        {
          if (name == "Logger")
          {
            LoggerHeader loggerHeader = functionAccessStorage.GetLoggerHeader();
            stringBuilder1.AppendLine();
            stringBuilder1.AppendLine("Logger:");
            stringBuilder1.AppendLine("   Storagetype: ........ " + functionAccessStorage.GetParameterObject(parameterNumber).StorageCode.ToString());
            StringBuilder stringBuilder2 = stringBuilder1;
            ushort num = functionAccessStorage.GetRuntimeParameterDataOffset((byte) 0);
            string str1 = "   LoggerHeaderOffset: . 0x" + num.ToString("x04");
            stringBuilder2.AppendLine(str1);
            StringBuilder stringBuilder3 = stringBuilder1;
            num = loggerHeader.LoggerStorageStartOffset;
            string str2 = "   StorageOffset: ...... 0x" + num.ToString("x04");
            stringBuilder3.AppendLine(str2);
            StringBuilder stringBuilder4 = stringBuilder1;
            num = loggerHeader.LoggerStorageEndOffset;
            string str3 = "   StorageEndOffset: ... 0x" + num.ToString("x04");
            stringBuilder4.AppendLine(str3);
            StringBuilder stringBuilder5 = stringBuilder1;
            num = loggerHeader.LoggerWriteOffset;
            string str4 = "   WriteOffset: ........ 0x" + num.ToString("x04");
            stringBuilder5.AppendLine(str4);
            StringBuilder stringBuilder6 = stringBuilder1;
            num = loggerHeader.GetMaxNumberOfEntries();
            string str5 = "   MaxNumberOfEntries: . " + num.ToString();
            stringBuilder6.AppendLine(str5);
            StringBuilder stringBuilder7 = stringBuilder1;
            num = loggerHeader.GetNumberOfEntries();
            string str6 = "   NumberOfEntries: .... " + num.ToString();
            stringBuilder7.AppendLine(str6);
            stringBuilder1.AppendLine("   LoggerData: ......... " + SmartFunctionCompiler.Compiler.GetHexInitStringFromArray(loggerHeader.GetLoggerBytes()));
            stringBuilder1.AppendLine("   LoggerReadProtocol: . " + SmartFunctionCompiler.Compiler.GetHexInitStringFromArray(loggerHeader.GetLoggerProtokoll()));
            stringBuilder1.AppendLine();
          }
          else
            stringBuilder1.AppendLine(name + " = " + functionAccessStorage.GetParameterValue(parameterNumber));
        }
        else
        {
          SmartFunctionByteList functionByteList = new SmartFunctionByteList(parameterObject.ParameterBytes, 2);
          stringBuilder1.AppendLine(name + " = ByteList," + functionByteList.ToString());
        }
      }
      this.TextBoxParameters.Text = stringBuilder1.ToString();
    }

    private async void MenuItemCheckCalendar2000_Click(object sender, RoutedEventArgs e)
    {
      DateTime now = DateTime.Now;
      Random myRandom = new Random(now.Millisecond);
      now = DateTime.Now;
      DateTime endTime = now.AddSeconds(60.0);
      DateTime reportTime = DateTime.Now;
      int checks = 0;
      while (endTime > DateTime.Now)
      {
        ++checks;
        int year = myRandom.Next(2000, 2099);
        int month = myRandom.Next(1, 12);
        DateTime testTime = new DateTime(year, month, 1);
        testTime = testTime.AddMonths(1);
        testTime = testTime.AddDays(-1.0);
        int day = myRandom.Next(1, testTime.Day);
        int hour = myRandom.Next(0, 23);
        int minute = myRandom.Next(0, 59);
        int second = myRandom.Next(0, 59);
        DateTime pcTime = new DateTime(year, month, day, hour, minute, second);
        uint meterTime = CalendarBase2000.Cal_GetMeterTime(pcTime);
        DateTime pcTimeBack = CalendarBase2000.Cal_GetDateTime(meterTime);
        CalStruct calStruct = CalendarBase2000.Cal_Sec2000ToStruct(meterTime);
        uint meterTimeBack = CalendarBase2000.Cal_StructToSec2000(calStruct);
        if (pcTime != pcTimeBack || (int) meterTime != (int) meterTimeBack || (long) calStruct.Year != (long) year || (long) calStruct.Month != (long) month || (long) calStruct.Day != (long) day || (long) calStruct.Hour != (long) hour || (long) calStruct.Minute != (long) minute || (long) calStruct.Secound != (long) second)
        {
          this.TextBoxOutput.Text = "Error on test time:" + pcTime.ToString() + Environment.NewLine + "Back time:" + pcTimeBack.ToString() + Environment.NewLine + "MeterTime: " + meterTime.ToString();
          myRandom = (Random) null;
          return;
        }
        if (reportTime < DateTime.Now)
        {
          reportTime = reportTime.AddSeconds(1.0);
          this.TextBoxOutput.Text = "CalendarBase2000 checks: " + checks.ToString() + Environment.NewLine + "Last check: " + pcTime.ToString();
        }
        await Task.Delay(1);
        calStruct = (CalStruct) null;
      }
      this.TextBoxOutput.Text = "CalendarBase2000 check finished";
      myRandom = (Random) null;
    }

    private void MenuItemReloadSource_Click(object sender, RoutedEventArgs e)
    {
      this.ReloadSource();
    }

    private void ReloadSource()
    {
      try
      {
        this.ClearLineNumbers();
        string[] poorCodeLines = SmartFunctionCompiler.Compiler.TheOnlyOneCompiler.GetPoorCodeLines(this.GetLinesFromSourceText());
        int lineNumber = 0;
        string tokenValue = HeaderInfo.GetTokenValue("Function name:", poorCodeLines, ref lineNumber);
        if (string.IsNullOrEmpty(tokenValue))
          throw new Exception("Funtion name not found");
        this.OpenFile(tokenValue);
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
    }

    public void CompareCodeToSource(byte[] theCode)
    {
      this.OpenFile(new SmartFunction(theCode).FunctionName);
      this.Compile();
      if (SmartFunctionCompiler.Compiler.TheOnlyOneCompiler.CompiledFunctionCode.Length != theCode.Length)
        throw new Exception("Code length changed");
      for (int index = 0; index < theCode.Length; ++index)
      {
        if ((int) theCode[index] != (int) SmartFunctionCompiler.Compiler.TheOnlyOneCompiler.CompiledFunctionCode[index])
          throw new Exception("Code changed at index " + index.ToString() + " from source: 0x" + SmartFunctionCompiler.Compiler.TheOnlyOneCompiler.CompiledFunctionCode[index].ToString("x02") + " to: 0x" + theCode[index].ToString("x02"));
      }
    }

    private void Window_Activated(object sender, EventArgs e)
    {
      if (this.FileOpenPath == null || !(this.FileOpenPathLastWriteTime != File.GetLastWriteTime(this.FileOpenPath)))
        return;
      if (MessageBox.Show("File changed outside of editor. Reload?", "File changed", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
        this.LoadFile(this.FileOpenPath);
      else
        this.FileOpenPath = (string) null;
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/SmartFunctionCompiler;component/mainwindow.xaml", UriKind.Relative));
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
          ((FrameworkElement) target).Loaded += new RoutedEventHandler(this.Window_Loaded);
          ((Window) target).Activated += new System.EventHandler(this.Window_Activated);
          break;
        case 2:
          this.TabItemEditorAndCompiler = (TabItem) target;
          break;
        case 3:
          this.MenuItemFile = (MenuItem) target;
          break;
        case 4:
          this.MenuItemFileOpen = (MenuItem) target;
          this.MenuItemFileOpen.Click += new RoutedEventHandler(this.MenuItemFileOpen_Click);
          break;
        case 5:
          this.MenuItemFileSave = (MenuItem) target;
          this.MenuItemFileSave.Click += new RoutedEventHandler(this.MenuItemFileSave_Click);
          break;
        case 6:
          this.MenuItemFileSaveAs = (MenuItem) target;
          this.MenuItemFileSaveAs.Click += new RoutedEventHandler(this.MenuItemFileSaveAs_Click);
          break;
        case 7:
          this.MenuItemReloadSource = (MenuItem) target;
          this.MenuItemReloadSource.Click += new RoutedEventHandler(this.MenuItemReloadSource_Click);
          break;
        case 8:
          this.MenuItemTests = (MenuItem) target;
          break;
        case 9:
          this.MenuItemCheckCalendar2000 = (MenuItem) target;
          this.MenuItemCheckCalendar2000.Click += new RoutedEventHandler(this.MenuItemCheckCalendar2000_Click);
          break;
        case 10:
          this.ButtonCompile = (Button) target;
          this.ButtonCompile.Click += new RoutedEventHandler(this.ButtonCompile_Click);
          break;
        case 11:
          this.ButtonDeCompile = (Button) target;
          this.ButtonDeCompile.Click += new RoutedEventHandler(this.ButtonDeCompile_Click);
          break;
        case 12:
          this.ButtonDeleteLineNumbers = (Button) target;
          this.ButtonDeleteLineNumbers.Click += new RoutedEventHandler(this.ButtonDeleteLineNumbers_Click);
          break;
        case 13:
          this.TextBoxCode = (TextBox) target;
          this.TextBoxCode.TextChanged += new TextChangedEventHandler(this.TextBoxCode_TextChanged);
          break;
        case 14:
          this.TextBoxResult = (TextBox) target;
          break;
        case 15:
          this.TabItemSimulationParameters = (TabItem) target;
          break;
        case 16:
          this.TabControl1 = (TabControl) target;
          break;
        case 17:
          this.TabItemMnemonics = (TabItem) target;
          break;
        case 18:
          this.TextBoxMnemonics = (TextBox) target;
          break;
        case 19:
          this.TabItemSimulator = (TabItem) target;
          break;
        case 20:
          this.TextBoxRunEvents = (TextBox) target;
          break;
        case 21:
          this.ButtonRun = (Button) target;
          this.ButtonRun.Click += new RoutedEventHandler(this.ButtonRun_Click);
          break;
        case 22:
          this.ButtonReset = (Button) target;
          this.ButtonReset.Click += new RoutedEventHandler(this.ButtonReset_Click);
          break;
        case 23:
          this.ButtonSingleEvent = (Button) target;
          this.ButtonSingleEvent.Click += new RoutedEventHandler(this.ButtonSingleEvent_Click);
          break;
        case 24:
          this.ButtonLoadSimulator = (Button) target;
          this.ButtonLoadSimulator.Click += new RoutedEventHandler(this.ButtonLoadSimulator_Click);
          break;
        case 25:
          this.TextBoxParameters = (TextBox) target;
          break;
        case 26:
          this.TextBoxDeviceTime = (TextBox) target;
          break;
        case 27:
          this.TextBoxCycleTime = (TextBox) target;
          break;
        case 28:
          this.TextBoxVolume = (TextBox) target;
          break;
        case 29:
          this.TextBoxFlowVolume = (TextBox) target;
          break;
        case 30:
          this.TextBoxReturnVolume = (TextBox) target;
          break;
        case 31:
          this.TextBoxFlow = (TextBox) target;
          break;
        case 32:
          this.TextBoxFlowIncrement = (TextBox) target;
          break;
        case 33:
          this.ComboBoxParameterName = (ComboBox) target;
          break;
        case 34:
          this.TextBoxInitValue = (TextBox) target;
          break;
        case 35:
          this.TextBoxValueIncrement = (TextBox) target;
          break;
        case 36:
          this.TextBoxValue = (TextBox) target;
          break;
        case 37:
          this.TextBoxOutput = (TextBox) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
