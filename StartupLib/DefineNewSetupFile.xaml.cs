// Decompiled with JetBrains decompiler
// Type: StartupLib.DefineNewSetupFile
// Assembly: StartupLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: F485B12B-6718-4E49-AD83-1AB4C51945B5
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\StartupLib.dll

using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;

#nullable disable
namespace StartupLib
{
  public partial class DefineNewSetupFile : Window, IComponentConnector
  {
    private const string nothingSelectedName = "__ create new __";
    private StartupManager myStartupManager;
    private Brush UsedFileBrush = (Brush) Brushes.Yellow;
    private Brush CreateCopiedFileBrush = (Brush) Brushes.LightBlue;
    private Brush CreateNewFileBrush = (Brush) Brushes.Azure;
    private Brush DefaultFileBrush;
    private string GmmApplicationPath;
    private string GmmSetupPath;
    public string NewSetupFilePath = (string) null;
    public string NewLicenseFilePath = (string) null;
    public bool ShowWindowRequired = false;
    private SortedList<string, DefineNewSetupFile.SetupFileState> allSetupFiles;
    private SortedList<string, int> allLicenseFiles;
    internal System.Windows.Controls.Label LabelFunction;
    internal System.Windows.Controls.Button ButtonOk;
    internal System.Windows.Controls.TextBox TextBoxNewFileName;
    internal System.Windows.Controls.ListBox ListBoxExistingStartupFiles;
    internal System.Windows.Controls.ListBox ListBoxExistingLicenseFiles;
    private bool _contentLoaded;

    public DefineNewSetupFile(StartupManager myStartupManager)
    {
      this.myStartupManager = myStartupManager;
      this.InitializeComponent();
      this.DefaultFileBrush = this.TextBoxNewFileName.Background;
      this.allSetupFiles = new SortedList<string, DefineNewSetupFile.SetupFileState>();
      this.allLicenseFiles = new SortedList<string, int>();
      if (myStartupManager.startupInfoList != null && myStartupManager.startupInfoList.startupInfo != null)
      {
        foreach (StartupInfo startupInfo in myStartupManager.startupInfoList.startupInfo)
        {
          if (!string.IsNullOrEmpty(startupInfo.StartupFile))
          {
            if (File.Exists(startupInfo.StartupFile) && !this.allSetupFiles.ContainsKey(startupInfo.StartupFile))
              this.allSetupFiles.Add(startupInfo.StartupFile, DefineNewSetupFile.SetupFileState.Used);
            if (!string.IsNullOrEmpty(startupInfo.LicenseFile) && !this.allLicenseFiles.ContainsKey(startupInfo.LicenseFile) && !File.Exists(startupInfo.LicenseFile))
              this.allLicenseFiles.Add(startupInfo.LicenseFile, 0);
          }
        }
      }
      this.GmmApplicationPath = Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath);
      this.GmmSetupPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "ZENNER", "GMM", "Settings");
      FileInfo[] files = new DirectoryInfo(this.GmmSetupPath).GetFiles();
      List<FileInfo> fileInfoList1 = new List<FileInfo>();
      fileInfoList1.AddRange(((IEnumerable<FileInfo>) files).Where<FileInfo>((Func<FileInfo, bool>) (x => x.Extension.Equals(".gmm"))));
      List<FileInfo> fileInfoList2 = new List<FileInfo>();
      fileInfoList2.AddRange(((IEnumerable<FileInfo>) files).Where<FileInfo>((Func<FileInfo, bool>) (x => x.Extension.Equals(".zlf"))));
      foreach (FileInfo fileInfo in fileInfoList1)
      {
        if (!this.allSetupFiles.ContainsKey(fileInfo.FullName))
          this.allSetupFiles.Add(fileInfo.FullName, DefineNewSetupFile.SetupFileState.FreeForUse);
      }
      foreach (FileInfo fileInfo in fileInfoList2)
      {
        if (!this.allLicenseFiles.ContainsKey(fileInfo.FullName))
          this.allLicenseFiles.Add(fileInfo.FullName, 0);
      }
      this.NewSetupFilePath = this.GetUniqueFilePath("Defaults");
      this.TextBoxNewFileName.Text = this.GetUniqueFileName(this.NewSetupFilePath);
      if (this.allSetupFiles.Count == 0 && this.allLicenseFiles.Count == 0)
      {
        this.Close();
      }
      else
      {
        this.ListBoxExistingStartupFiles.Items.Add((object) new TextBlock()
        {
          Text = "__ create new __"
        });
        foreach (KeyValuePair<string, DefineNewSetupFile.SetupFileState> allSetupFile in this.allSetupFiles)
        {
          TextBlock newItem = new TextBlock();
          newItem.Text = allSetupFile.Key;
          if (allSetupFile.Value == DefineNewSetupFile.SetupFileState.Used)
            newItem.Background = this.UsedFileBrush;
          this.ListBoxExistingStartupFiles.Items.Add((object) newItem);
        }
        this.ListBoxExistingStartupFiles.SelectedIndex = 0;
        this.ListBoxExistingLicenseFiles.Items.Add((object) new TextBlock()
        {
          Text = "__ create new __"
        });
        foreach (KeyValuePair<string, int> allLicenseFile in this.allLicenseFiles)
          this.ListBoxExistingLicenseFiles.Items.Add((object) new TextBlock()
          {
            Text = allLicenseFile.Key
          });
        this.ListBoxExistingLicenseFiles.SelectedIndex = 0;
        this.ShowWindowRequired = true;
      }
    }

    private void ListBoxExistingStartupFiles_SelectionChanged(
      object sender,
      SelectionChangedEventArgs e)
    {
      if (this.ListBoxExistingStartupFiles.SelectedValue == null)
        return;
      string text = ((TextBlock) this.ListBoxExistingStartupFiles.SelectedValue).Text;
      switch (this.GetSetupFileState())
      {
        case DefineNewSetupFile.SetupFileState.Used:
          this.TextBoxNewFileName.Text = this.GetUniqueFileName(Path.GetFileNameWithoutExtension(text));
          break;
        case DefineNewSetupFile.SetupFileState.FreeForUse:
          this.TextBoxNewFileName.Text = Path.GetFileNameWithoutExtension(text);
          break;
        case DefineNewSetupFile.SetupFileState.NotSelected:
          this.TextBoxNewFileName.Text = "";
          break;
        default:
          this.ButtonOk.IsEnabled = false;
          throw new Exception("Illegal file used mark");
      }
      this.CheckFileName();
    }

    private void ButtonOk_Click(object sender, RoutedEventArgs e)
    {
      string pathFromText = this.GetPathFromText();
      if (!File.Exists(pathFromText) && this.ListBoxExistingStartupFiles.SelectedValue != null)
      {
        TextBlock selectedValue = (TextBlock) this.ListBoxExistingStartupFiles.SelectedValue;
        if (selectedValue.Text != "__ create new __")
        {
          string text = selectedValue.Text;
          if (pathFromText != text)
            File.Copy(text, pathFromText);
        }
      }
      if (this.ListBoxExistingLicenseFiles.SelectedValue != null)
      {
        string text = ((TextBlock) this.ListBoxExistingLicenseFiles.SelectedValue).Text;
        if (text != "__ create new __")
          this.NewLicenseFilePath = text;
      }
      this.NewSetupFilePath = pathFromText;
      this.DialogResult = new bool?(true);
      this.Close();
    }

    private void TextBoxNewFileName_TextChanged(object sender, TextChangedEventArgs e)
    {
      this.CheckFileName();
    }

    private void CheckFileName()
    {
      if (this.TextBoxNewFileName.Text.Contains<char>(Path.DirectorySeparatorChar))
      {
        this.TextBoxNewFileName.Background = this.CreateCopiedFileBrush;
        this.LabelFunction.Content = (object) "Directory separator not allowed";
        this.ButtonOk.IsEnabled = false;
      }
      else if (this.TextBoxNewFileName.Text.Length < 3)
      {
        this.TextBoxNewFileName.Background = this.CreateCopiedFileBrush;
        this.LabelFunction.Content = (object) "Filename to short";
        this.ButtonOk.IsEnabled = false;
      }
      else
      {
        string pathFromText = this.GetPathFromText();
        int index = this.allSetupFiles.IndexOfKey(pathFromText);
        if (index >= 0)
        {
          if (this.allSetupFiles.Values[index] == DefineNewSetupFile.SetupFileState.FreeForUse)
          {
            this.TextBoxNewFileName.Background = this.DefaultFileBrush;
            this.LabelFunction.Content = (object) "Use existing file";
            this.ButtonOk.IsEnabled = true;
          }
          else
          {
            this.TextBoxNewFileName.Background = this.UsedFileBrush;
            this.LabelFunction.Content = (object) "File used";
            this.ButtonOk.IsEnabled = false;
          }
        }
        else if (File.Exists(pathFromText))
        {
          this.TextBoxNewFileName.Background = this.UsedFileBrush;
          this.LabelFunction.Content = (object) "File exists";
          this.ButtonOk.IsEnabled = false;
        }
        else if (this.ListBoxExistingStartupFiles.SelectedIndex < 1)
        {
          this.TextBoxNewFileName.Background = this.CreateNewFileBrush;
          this.LabelFunction.Content = (object) "Create new file";
          this.ButtonOk.IsEnabled = true;
        }
        else
        {
          this.TextBoxNewFileName.Background = this.CreateCopiedFileBrush;
          this.LabelFunction.Content = (object) "Copy selected file";
          this.ButtonOk.IsEnabled = true;
        }
      }
    }

    private string GetPathFromText()
    {
      string str = Path.GetFileName(this.TextBoxNewFileName.Text);
      if (str.ToUpper().EndsWith(".GMM"))
        str = str.Substring(0, str.Length - 4);
      return Path.Combine(this.GmmSetupPath, str + ".gmm");
    }

    private DefineNewSetupFile.SetupFileState GetSetupFileState()
    {
      return this.ListBoxExistingStartupFiles.SelectedIndex < 1 ? DefineNewSetupFile.SetupFileState.NotSelected : this.allSetupFiles.Values[this.ListBoxExistingStartupFiles.SelectedIndex - 1];
    }

    private string GetUniqueFilePath(string baseFileName)
    {
      int num = 0;
      string str = baseFileName;
      string path;
      while (true)
      {
        path = Path.Combine(this.GmmSetupPath, str + ".gmm");
        if (File.Exists(path))
        {
          str = baseFileName + num.ToString();
          ++num;
        }
        else
          break;
      }
      return path;
    }

    private string GetUniqueFileName(string baseFileName)
    {
      return Path.GetFileNameWithoutExtension(this.GetUniqueFilePath(baseFileName));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      System.Windows.Application.LoadComponent((object) this, new Uri("/StartupLib;component/definenewsetupfile.xaml", UriKind.Relative));
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
          this.LabelFunction = (System.Windows.Controls.Label) target;
          break;
        case 2:
          this.ButtonOk = (System.Windows.Controls.Button) target;
          this.ButtonOk.Click += new RoutedEventHandler(this.ButtonOk_Click);
          break;
        case 3:
          this.TextBoxNewFileName = (System.Windows.Controls.TextBox) target;
          this.TextBoxNewFileName.TextChanged += new TextChangedEventHandler(this.TextBoxNewFileName_TextChanged);
          break;
        case 4:
          this.ListBoxExistingStartupFiles = (System.Windows.Controls.ListBox) target;
          this.ListBoxExistingStartupFiles.SelectionChanged += new SelectionChangedEventHandler(this.ListBoxExistingStartupFiles_SelectionChanged);
          break;
        case 5:
          this.ListBoxExistingLicenseFiles = (System.Windows.Controls.ListBox) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }

    private enum SetupFileState
    {
      Used,
      FreeForUse,
      NotSelected,
    }
  }
}
