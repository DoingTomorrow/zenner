// Decompiled with JetBrains decompiler
// Type: CommonWPF.NLogConfigSelector
// Assembly: CommonWPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: FC3FF060-22A9-4729-A79E-14B5F4740E69
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\CommonWPF.dll

using NLog;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

#nullable disable
namespace CommonWPF
{
  public partial class NLogConfigSelector : UserControl, IComponentConnector
  {
    private string pathToNLogFiles = string.Empty;
    private string fileType = "NLog*";
    private string NLogFile = "NLog.config";
    private string fullNLogFile = string.Empty;
    private string origNLogFile = string.Empty;
    internal Grid ButtonSetOriginNLogFile;
    internal ListView ListViewNLogConfigFiles;
    internal Label LabelNLogconfigFiles;
    internal ListBox ListBoxStatus;
    internal Button ButtonSelectNLogFile;
    internal Button ButtonSetOriginNLog;
    private bool _contentLoaded;

    public NLogConfigSelector()
    {
      this.InitializeComponent();
      this.SetNLogConfigFilesPath(Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName));
    }

    public void SetNLogConfigFilesPath(string pathToNLogConfigFiles)
    {
      this.pathToNLogFiles = !string.IsNullOrEmpty(pathToNLogConfigFiles) ? pathToNLogConfigFiles : throw new Exception("Wrong path to NLog files submitted !!!");
      this.fullNLogFile = Path.Combine(this.pathToNLogFiles, this.NLogFile);
      this.origNLogFile = Path.Combine(this.pathToNLogFiles, "original_" + this.NLogFile);
      this.seachNLogConfigFilesFromPath();
    }

    private void ButtonSelectNLogFile_Click(object sender, RoutedEventArgs e)
    {
      if (this.ListViewNLogConfigFiles.SelectedItems.Count != 1)
        return;
      FileInfo tag = (FileInfo) ((FrameworkElement) this.ListViewNLogConfigFiles.SelectedItem).Tag;
      if (!File.Exists(this.origNLogFile))
      {
        File.Move(this.fullNLogFile, this.origNLogFile);
        this.ListBoxStatus.Items.Add((object) " origin NLog.config saved! ");
      }
      File.Copy(tag.FullName, this.fullNLogFile, true);
      this.ListBoxStatus.Items.Add((object) (" set " + tag.Name + " to NLog.config! "));
      LogManager.Configuration.Reload();
      this.ListBoxStatus.Items.Add((object) " restart NLog ... done.  ");
    }

    private void ButtonSetOriginNLog_Click(object sender, RoutedEventArgs e)
    {
      if (File.Exists(this.origNLogFile))
      {
        File.Copy(this.origNLogFile, this.fullNLogFile, true);
        this.ListBoxStatus.Items.Add((object) " original NLog config file set! ");
      }
      else
        this.ListBoxStatus.Items.Add((object) "No original NLog config file found.");
    }

    private void seachNLogConfigFilesFromPath()
    {
      this.ListViewNLogConfigFiles.Items.Clear();
      FileInfo[] files = new DirectoryInfo(this.pathToNLogFiles).GetFiles(this.fileType);
      if (files.Length != 0)
      {
        foreach (FileInfo fileInfo in files)
        {
          if (fileInfo.Name.Contains(".config"))
          {
            ListViewItem newItem = new ListViewItem();
            newItem.Tag = (object) fileInfo;
            newItem.Content = (object) fileInfo.Name;
            this.ListViewNLogConfigFiles.Items.Add((object) newItem);
          }
        }
        this.ListBoxStatus.Items.Add((object) (" -> " + files.Length.ToString() + " NLog config files found."));
      }
      else
        this.ListBoxStatus.Items.Add((object) "-!- No NLog config files found.");
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/CommonWPF;component/nlogconfigselector.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.ButtonSetOriginNLogFile = (Grid) target;
          break;
        case 2:
          this.ListViewNLogConfigFiles = (ListView) target;
          break;
        case 3:
          this.LabelNLogconfigFiles = (Label) target;
          break;
        case 4:
          this.ListBoxStatus = (ListBox) target;
          break;
        case 5:
          this.ButtonSelectNLogFile = (Button) target;
          this.ButtonSelectNLogFile.Click += new RoutedEventHandler(this.ButtonSelectNLogFile_Click);
          break;
        case 6:
          this.ButtonSetOriginNLog = (Button) target;
          this.ButtonSetOriginNLog.Click += new RoutedEventHandler(this.ButtonSetOriginNLog_Click);
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
