// Decompiled with JetBrains decompiler
// Type: StartupLib.DatabaseSelector
// Assembly: StartupLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: F485B12B-6718-4E49-AD83-1AB4C51945B5
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\StartupLib.dll

using GmmDbLib;
using Microsoft.Win32;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using ZR_ClassLibrary;

#nullable disable
namespace StartupLib
{
  public partial class DatabaseSelector : Window, IComponentConnector
  {
    public DbConnectionInfo SavedPrimaryConnectionInfo;
    public DbConnectionInfo SavedSecundaryConnectionInfo;
    internal Label label;
    internal ComboBox ComboBoxDatabaseInstance;
    internal ComboBox ComboBoxDatabaseType;
    internal StackPanel DatabaseProperties;
    internal DockPanel DockPanalPath;
    internal Button ButtonSelectPath;
    internal TextBox TextBoxDatabasePath;
    internal DockPanel DockPanalURL;
    internal TextBox TextBoxDatabaseURL;
    internal DockPanel DockPanalDatabaseName;
    internal TextBox TextBoxDatabaseName;
    internal DockPanel DockPanalUserName;
    internal TextBox TextBoxUserName;
    internal DockPanel DockPanalPassword;
    internal PasswordBox PasswordBox1;
    internal TextBlock TextBoxDatabaseInfo;
    internal DockPanel DockPanelButtons;
    internal Button ButtonTestConnection;
    internal Button ButtonCopy;
    internal Button ButtonPast;
    internal Button ButtonSave;
    private bool _contentLoaded;

    public DatabaseSelectorViewModel DatabaseSelectorVM { get; set; }

    public DatabaseSelector(
      DbConnectionInfo activeConnectionInfo,
      List<DbConnectionInfo> allConnectionInfos)
    {
      this.InitializeComponent();
      this.DatabaseSelectorVM = new DatabaseSelectorViewModel(activeConnectionInfo, allConnectionInfos);
    }

    private void ButtonTestConnection_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        this.DatabaseSelectorVM.TestDatabaseConnection();
        this.ButtonSave.IsEnabled = true;
      }
      catch (Exception ex)
      {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("!!! Exception !!!");
        stringBuilder.AppendLine(ex.Message);
        if (ex.InnerException != null)
        {
          stringBuilder.AppendLine();
          stringBuilder.AppendLine("+++ Inner exception +++");
          stringBuilder.AppendLine(ex.InnerException.Message);
        }
        int num = (int) GMM_MessageBox.ShowMessage("Open error", stringBuilder.ToString(), true);
      }
    }

    private void ButtoSaveConnection_Click(object sender, RoutedEventArgs e)
    {
      DbConnectionInfo dbConnectionInfo = this.DatabaseSelectorVM.SaveDatabaseConnection();
      if (dbConnectionInfo.DbInstance == DbInstances.Primary)
        this.SavedPrimaryConnectionInfo = dbConnectionInfo;
      else
        this.SavedSecundaryConnectionInfo = dbConnectionInfo;
      StringBuilder stringBuilder = new StringBuilder();
      if (this.SavedPrimaryConnectionInfo != null)
        stringBuilder.AppendLine("*** Primary database settings saved ***");
      if (this.SavedSecundaryConnectionInfo != null)
        stringBuilder.AppendLine("*** Secundary database settings saved ***");
      if (this.SavedPrimaryConnectionInfo != null || this.SavedSecundaryConnectionInfo != null)
        stringBuilder.AppendLine("GMM will restarts by window closing.");
      else
        stringBuilder.AppendLine("Nothing saved");
      this.DatabaseSelectorVM.DatabaseInfo = stringBuilder.ToString();
      this.DatabaseSelectorVM.SelectionOk = false;
    }

    private void ButtonSelectPath_Click(object sender, RoutedEventArgs e)
    {
      OpenFileDialog openFileDialog = new OpenFileDialog();
      List<string> dbFileExtentions = this.DatabaseSelectorVM.GetDbFileExtentions();
      StringBuilder stringBuilder = new StringBuilder();
      for (int index = 0; index < dbFileExtentions.Count; ++index)
      {
        string str = dbFileExtentions[index];
        if (index == 0)
          stringBuilder.Append("Database files |*.");
        else
          stringBuilder.Append(";*.");
        stringBuilder.Append(dbFileExtentions[index]);
      }
      if (dbFileExtentions.Count > 0)
      {
        openFileDialog.DefaultExt = "." + dbFileExtentions[0];
        openFileDialog.Filter = stringBuilder.ToString() + "|All files |*.*";
      }
      else
      {
        openFileDialog.DefaultExt = "";
        openFileDialog.Filter = "All files |*.*";
      }
      bool? nullable = openFileDialog.ShowDialog();
      bool flag = true;
      if (!(nullable.GetValueOrDefault() == flag & nullable.HasValue))
        return;
      this.TextBoxDatabasePath.Text = openFileDialog.FileName;
    }

    private void PasswordBox1_LostFocus(object sender, RoutedEventArgs e)
    {
      this.DatabaseSelectorVM.Password = this.PasswordBox1.Password;
    }

    private void PasswordBox1_GotFocus(object sender, RoutedEventArgs e)
    {
      this.PasswordBox1.Password = this.DatabaseSelectorVM.Password;
    }

    private void DockPanelButtons_IsEnabledChanged(
      object sender,
      DependencyPropertyChangedEventArgs e)
    {
      if (!this.DockPanelButtons.IsEnabled)
        return;
      this.ButtonSave.IsEnabled = false;
    }

    private void ButtonCopy_Click(object sender, RoutedEventArgs e)
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine("ComboBoxDatabaseType;" + this.ComboBoxDatabaseType.Text);
      stringBuilder.AppendLine("TextBoxDatabaseURL;" + this.TextBoxDatabaseURL.Text);
      stringBuilder.AppendLine("TextBoxDatabasePath;" + this.TextBoxDatabasePath.Text);
      stringBuilder.AppendLine("TextBoxDatabaseName;" + this.TextBoxDatabaseName.Text);
      stringBuilder.AppendLine("TextBoxUserName;" + this.TextBoxUserName.Text);
      stringBuilder.AppendLine("PasswordBox1;" + this.PasswordBox1.Password);
      Clipboard.SetDataObject((object) stringBuilder.ToString());
    }

    private void ButtonPast_Click(object sender, RoutedEventArgs e)
    {
      string text = Clipboard.GetText();
      if (string.IsNullOrEmpty(text))
        return;
      string[] strArray1 = text.Split(new string[1]
      {
        Environment.NewLine
      }, StringSplitOptions.RemoveEmptyEntries);
      if (strArray1.Length != 6)
        return;
      string[] strArray2 = strArray1[0].Split(';');
      if (strArray2.Length != 2 || strArray2[0] != "ComboBoxDatabaseType")
        return;
      this.ComboBoxDatabaseType.SelectedValue = (object) strArray2[1];
      string[] strArray3 = strArray1[1].Split(';');
      if (strArray3.Length != 2 || strArray3[0] != "TextBoxDatabaseURL")
        return;
      string str1 = strArray3[1];
      if (!string.IsNullOrEmpty(str1))
        this.TextBoxDatabaseURL.Text = str1;
      string[] strArray4 = strArray1[2].Split(';');
      if (strArray4.Length != 2 || strArray4[0] != "TextBoxDatabasePath")
        return;
      string str2 = strArray4[1];
      if (!string.IsNullOrEmpty(str2))
        this.TextBoxDatabasePath.Text = str2;
      string[] strArray5 = strArray1[3].Split(';');
      if (strArray5.Length != 2 || strArray5[0] != "TextBoxDatabaseName")
        return;
      string str3 = strArray5[1];
      if (!string.IsNullOrEmpty(str3))
        this.TextBoxDatabaseName.Text = str3;
      string[] strArray6 = strArray1[4].Split(';');
      if (strArray6.Length != 2 || strArray6[0] != "TextBoxUserName")
        return;
      string str4 = strArray6[1];
      if (!string.IsNullOrEmpty(str4))
        this.TextBoxUserName.Text = str4;
      string[] strArray7 = strArray1[5].Split(';');
      if (strArray7.Length != 2 || strArray7[0] != "PasswordBox1")
        return;
      string str5 = strArray7[1];
      if (string.IsNullOrEmpty(str5))
        return;
      this.PasswordBox1.Password = str5;
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/StartupLib;component/databaseselector.xaml", UriKind.Relative));
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
          this.label = (Label) target;
          break;
        case 2:
          this.ComboBoxDatabaseInstance = (ComboBox) target;
          break;
        case 3:
          this.ComboBoxDatabaseType = (ComboBox) target;
          break;
        case 4:
          this.DatabaseProperties = (StackPanel) target;
          break;
        case 5:
          this.DockPanalPath = (DockPanel) target;
          break;
        case 6:
          this.ButtonSelectPath = (Button) target;
          this.ButtonSelectPath.Click += new RoutedEventHandler(this.ButtonSelectPath_Click);
          break;
        case 7:
          this.TextBoxDatabasePath = (TextBox) target;
          break;
        case 8:
          this.DockPanalURL = (DockPanel) target;
          break;
        case 9:
          this.TextBoxDatabaseURL = (TextBox) target;
          break;
        case 10:
          this.DockPanalDatabaseName = (DockPanel) target;
          break;
        case 11:
          this.TextBoxDatabaseName = (TextBox) target;
          break;
        case 12:
          this.DockPanalUserName = (DockPanel) target;
          break;
        case 13:
          this.TextBoxUserName = (TextBox) target;
          break;
        case 14:
          this.DockPanalPassword = (DockPanel) target;
          break;
        case 15:
          this.PasswordBox1 = (PasswordBox) target;
          this.PasswordBox1.GotFocus += new RoutedEventHandler(this.PasswordBox1_GotFocus);
          this.PasswordBox1.LostFocus += new RoutedEventHandler(this.PasswordBox1_LostFocus);
          break;
        case 16:
          this.TextBoxDatabaseInfo = (TextBlock) target;
          break;
        case 17:
          this.DockPanelButtons = (DockPanel) target;
          this.DockPanelButtons.IsEnabledChanged += new DependencyPropertyChangedEventHandler(this.DockPanelButtons_IsEnabledChanged);
          break;
        case 18:
          this.ButtonTestConnection = (Button) target;
          this.ButtonTestConnection.Click += new RoutedEventHandler(this.ButtonTestConnection_Click);
          break;
        case 19:
          this.ButtonCopy = (Button) target;
          this.ButtonCopy.Click += new RoutedEventHandler(this.ButtonCopy_Click);
          break;
        case 20:
          this.ButtonPast = (Button) target;
          this.ButtonPast.Click += new RoutedEventHandler(this.ButtonPast_Click);
          break;
        case 21:
          this.ButtonSave = (Button) target;
          this.ButtonSave.Click += new RoutedEventHandler(this.ButtoSaveConnection_Click);
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
