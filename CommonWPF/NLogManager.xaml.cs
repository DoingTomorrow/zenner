// Decompiled with JetBrains decompiler
// Type: CommonWPF.NLogManager
// Assembly: CommonWPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: FC3FF060-22A9-4729-A79E-14B5F4740E69
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\CommonWPF.dll

using Microsoft.Win32;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using ZENNER.CommonLibrary;

#nullable disable
namespace CommonWPF
{
  public partial class NLogManager : Window, IComponentConnector
  {
    private List<NLogFileRecord> NLogRecords;
    private List<NLogFileRecord> VisibleNLogRecords;
    internal DockPanel StackPanelSetup;
    internal GroupBox GroupBoxNLogSetup;
    internal TextBox TextBoxCurrentNLogSetup;
    internal ComboBox ComboBoxSetupFiles;
    internal Button ButtonChangeAndRestart;
    internal Button ButtonChangeTemporary;
    internal Button ButtonShowNLogOutputFile;
    internal Button ButtonOpenNLogOutputFile;
    internal Button ButtonDeleteNLogOutputFile;
    internal Button ButtonSaveNLogOutputFile;
    internal Button ButtonAddNLogOutputFile;
    internal StackPanel StackPanelLoggers;
    internal Label LabelStartDate;
    internal TextBox TextBoxMessage;
    internal DataGrid DataGridNlogFileData;
    private bool _contentLoaded;

    public NLogManager()
    {
      this.InitializeComponent();
      this.ComboBoxSetupFiles.ItemsSource = (IEnumerable) NLogSupport.NLogSetupFileNames;
      this.ComboBoxSetupFiles.SelectedItem = (object) NLogSupport.GetActiveNLogConfigurationFileName();
      this.TextBoxCurrentNLogSetup.Text = NLogSupport.GetCurrentNLogSetup();
    }

    public NLogManager(bool loadNLogOutputFile)
      : this()
    {
      try
      {
        if (!loadNLogOutputFile)
          return;
        this.NLogRecords = NLogSupport.GetNLogOutputfileRecords();
        this.ShowNLogRecords();
      }
      catch
      {
      }
    }

    public NLogManager(string loadFilePath)
      : this()
    {
      try
      {
        if (!File.Exists(loadFilePath))
          return;
        this.NLogRecords = NLogSupport.GetNLogOutputfileRecords(loadFilePath);
        this.ShowNLogRecords();
      }
      catch
      {
      }
    }

    private void ComboBoxSetupFiles_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (this.ComboBoxSetupFiles.SelectedItem == null)
        return;
      this.TextBoxCurrentNLogSetup.Text = NLogSupport.GetCurrentNLogSetup();
    }

    private void ButtonChangeAndRestart_Click(object sender, RoutedEventArgs e)
    {
      if (!NLogSupport.ChangeDefaultToSetupFile(this.ComboBoxSetupFiles.SelectedItem.ToString()))
        return;
      this.DialogResult = new bool?(true);
      this.Close();
    }

    private void ButtonChangeTemporary_Click(object sender, RoutedEventArgs e)
    {
      NLogSupport.ChangeTemporaryToSetupFile(this.ComboBoxSetupFiles.SelectedItem.ToString());
      this.TextBoxCurrentNLogSetup.Text = NLogSupport.GetCurrentNLogSetup();
    }

    private void ButtonDeleteNLogOutputFile_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        NLogSupport.DeleteNLogOutputFile();
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
    }

    private void ButtonSaveNLogOutputFile_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        SaveFileDialog saveFileDialog = new SaveFileDialog();
        saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        saveFileDialog.FileName = "NLogOut_" + DateTime.Now.ToString("yyMMdd_HHmm") + ".json";
        bool? nullable = saveFileDialog.ShowDialog();
        bool flag = true;
        if (!(nullable.GetValueOrDefault() == flag & nullable.HasValue))
          return;
        NLogSupport.SaveLogFile(saveFileDialog.FileName);
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
    }

    private void ButtonShowNLogOutputFile_Click(object sender, RoutedEventArgs e)
    {
      this.NLogRecords = NLogSupport.GetNLogOutputfileRecords();
      this.ShowNLogRecords();
    }

    private void ButtonOpenNLogOutputFile_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        OpenFileDialog openFileDialog = new OpenFileDialog();
        openFileDialog.Filter = "NLog output file(*.json)|*.json| All files (*.*)|*.*";
        openFileDialog.FilterIndex = 1;
        openFileDialog.RestoreDirectory = true;
        openFileDialog.Title = "Open file which contains json coded NLog records";
        openFileDialog.CheckFileExists = true;
        bool? nullable = openFileDialog.ShowDialog();
        bool flag = true;
        if (!(nullable.GetValueOrDefault() == flag & nullable.HasValue))
          return;
        this.NLogRecords = NLogSupport.GetNLogOutputfileRecords(openFileDialog.FileName);
        this.ShowNLogRecords();
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
    }

    private void ButtonAddNLogOutputFile_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        OpenFileDialog openFileDialog = new OpenFileDialog();
        openFileDialog.Filter = "Text file(*.txt)|*.txt| All files (*.*)|*.*";
        openFileDialog.FilterIndex = 1;
        openFileDialog.RestoreDirectory = true;
        openFileDialog.Title = "Open file to add NLog output";
        openFileDialog.CheckFileExists = true;
        bool? nullable = openFileDialog.ShowDialog();
        bool flag = true;
        if (!(nullable.GetValueOrDefault() == flag & nullable.HasValue))
          return;
        StringBuilder fileContent = new StringBuilder(File.ReadAllText(openFileDialog.FileName));
        NLogSupport.AddNlogOutputfileContent(fileContent);
        string str = openFileDialog.FileName + ".save";
        if (File.Exists(str))
          File.Delete(str);
        File.Move(openFileDialog.FileName, str);
        File.WriteAllText(openFileDialog.FileName, fileContent.ToString());
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
    }

    private void ShowNLogRecords()
    {
      this.StackPanelLoggers.Children.Clear();
      List<string> stringList = new List<string>();
      foreach (NLogFileRecord nlogRecord in this.NLogRecords)
      {
        if (!stringList.Contains(nlogRecord.LoggerName))
          stringList.Add(nlogRecord.LoggerName);
      }
      stringList.Sort();
      foreach (string str in stringList)
      {
        CheckBox element = new CheckBox();
        element.Content = (object) str;
        element.Checked += new RoutedEventHandler(this.LoggerSelectionChanged);
        element.Unchecked += new RoutedEventHandler(this.LoggerSelectionChanged);
        this.StackPanelLoggers.Children.Add((UIElement) element);
      }
      if (this.NLogRecords.Count > 0)
        this.LabelStartDate.Content = (object) this.NLogRecords[0].LogTime.ToShortDateString();
      this.DataGridNlogFileData.ItemsSource = (IEnumerable) this.NLogRecords;
    }

    private void LoggerSelectionChanged(object sender, RoutedEventArgs e)
    {
      List<string> stringList = new List<string>();
      foreach (CheckBox child in this.StackPanelLoggers.Children)
      {
        if (child.IsChecked.Value)
        {
          string str = child.Content.ToString();
          stringList.Add(str);
        }
      }
      this.VisibleNLogRecords = new List<NLogFileRecord>();
      foreach (NLogFileRecord nlogRecord in this.NLogRecords)
      {
        if (!stringList.Contains(nlogRecord.LoggerName))
          this.VisibleNLogRecords.Add(nlogRecord);
      }
      this.DataGridNlogFileData.ItemsSource = (IEnumerable) this.VisibleNLogRecords;
    }

    private void DataGridNlogFileData_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (this.DataGridNlogFileData.SelectedItem == null || !(this.DataGridNlogFileData.SelectedItem is NLogFileRecord))
        return;
      NLogFileRecord selectedItem = (NLogFileRecord) this.DataGridNlogFileData.SelectedItem;
      if (selectedItem.Message == null)
        return;
      this.TextBoxMessage.Text = selectedItem.Message;
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/CommonWPF;component/nlogmanager.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    internal Delegate _CreateDelegate(Type delegateType, string handler)
    {
      return Delegate.CreateDelegate(delegateType, (object) this, handler);
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.StackPanelSetup = (DockPanel) target;
          break;
        case 2:
          this.GroupBoxNLogSetup = (GroupBox) target;
          break;
        case 3:
          this.TextBoxCurrentNLogSetup = (TextBox) target;
          break;
        case 4:
          this.ComboBoxSetupFiles = (ComboBox) target;
          this.ComboBoxSetupFiles.SelectionChanged += new SelectionChangedEventHandler(this.ComboBoxSetupFiles_SelectionChanged);
          break;
        case 5:
          this.ButtonChangeAndRestart = (Button) target;
          this.ButtonChangeAndRestart.Click += new RoutedEventHandler(this.ButtonChangeAndRestart_Click);
          break;
        case 6:
          this.ButtonChangeTemporary = (Button) target;
          this.ButtonChangeTemporary.Click += new RoutedEventHandler(this.ButtonChangeTemporary_Click);
          break;
        case 7:
          this.ButtonShowNLogOutputFile = (Button) target;
          this.ButtonShowNLogOutputFile.Click += new RoutedEventHandler(this.ButtonShowNLogOutputFile_Click);
          break;
        case 8:
          this.ButtonOpenNLogOutputFile = (Button) target;
          this.ButtonOpenNLogOutputFile.Click += new RoutedEventHandler(this.ButtonOpenNLogOutputFile_Click);
          break;
        case 9:
          this.ButtonDeleteNLogOutputFile = (Button) target;
          this.ButtonDeleteNLogOutputFile.Click += new RoutedEventHandler(this.ButtonDeleteNLogOutputFile_Click);
          break;
        case 10:
          this.ButtonSaveNLogOutputFile = (Button) target;
          this.ButtonSaveNLogOutputFile.Click += new RoutedEventHandler(this.ButtonSaveNLogOutputFile_Click);
          break;
        case 11:
          this.ButtonAddNLogOutputFile = (Button) target;
          this.ButtonAddNLogOutputFile.Click += new RoutedEventHandler(this.ButtonAddNLogOutputFile_Click);
          break;
        case 12:
          this.StackPanelLoggers = (StackPanel) target;
          break;
        case 13:
          this.LabelStartDate = (Label) target;
          break;
        case 14:
          this.TextBoxMessage = (TextBox) target;
          break;
        case 15:
          this.DataGridNlogFileData = (DataGrid) target;
          this.DataGridNlogFileData.SelectionChanged += new SelectionChangedEventHandler(this.DataGridNlogFileData_SelectionChanged);
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
