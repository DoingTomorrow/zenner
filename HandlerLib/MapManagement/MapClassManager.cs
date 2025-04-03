// Decompiled with JetBrains decompiler
// Type: HandlerLib.MapManagement.MapClassManager
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

using Microsoft.Win32;
using StartupLib;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Threading;
using ZENNER.CommonLibrary;
using ZR_ClassLibrary;

#nullable disable
namespace HandlerLib.MapManagement
{
  public class MapClassManager : Window, IComponentConnector
  {
    internal string NextPlugin = "";
    private string MapFolder = string.Empty;
    private string TheFolder = string.Empty;
    private string InitMapFolder = string.Empty;
    private string ClassName = string.Empty;
    private string ClassNamespace = string.Empty;
    private string MappingDataFolder = SystemValues.MappingPath;
    private string DynamicMappingFolder = string.Empty;
    private bool UseListFiles = false;
    private MapReader mapReader;
    private List<AddressRange> AddressRanges;
    internal List<string> RequiredSections;
    private bool InitFinished = false;
    private bool IsMapSelected = false;
    private bool IsMapRead = false;
    private bool IsListAvail = false;
    private bool IsVersionEnabled = false;
    private static Action EmptyDelegate = (Action) (() => { });
    internal Menu menuMain;
    internal MenuItem MenuItemComponents;
    internal GmmCorporateControl gmmCorporateControl1;
    internal TextBox TextBoxMapPath;
    internal TextBox TextBoxVersion;
    internal TextBox TextBoxVersionHex;
    internal TextBox TextBoxClassPath;
    internal TextBox TextBoxLinkerType;
    internal TextBox ComboBoxAvailableReader;
    internal TextBox TextBoxReaderDescription;
    internal Button ButtonReadMapFile;
    internal Button ButtonCreateClass;
    internal Button ButtonSelectMapFile;
    internal CheckBox CheckBoxReadListFiles;
    internal TextBox TextBoxStatus;
    internal Button ButtonGenerateParameterFile;
    private bool _contentLoaded;

    public MapClassManager(
      string classNamespace,
      MapReader mapReader = null,
      bool useListFiles = false,
      List<AddressRange> addressRanges = null,
      string pathToMapClasses = null,
      List<string> requiredSections = null)
    {
      this.InitializeComponent();
      UserInterfaceServices.AddDefaultMenu((MenuItem) this.menuMain.Items[0], new RoutedEventHandler(this.componentsClick));
      this.mapReader = mapReader;
      if (this.mapReader != null)
        this.ClassName = this.mapReader.ReaderName;
      this.ClassNamespace = classNamespace;
      this.UseListFiles = useListFiles;
      this.AddressRanges = addressRanges;
      this.InitMapFolder = pathToMapClasses;
      this.RequiredSections = requiredSections;
      this.DataContext = (object) SynchronizationContext.Current;
      this.InitFormular();
    }

    private string searchDirectory(string theFolder)
    {
      int num = 0;
      while (num++ <= 5)
      {
        foreach (string directory in Directory.GetDirectories(theFolder))
        {
          if (Path.GetFileName(directory) == this.ClassNamespace)
          {
            theFolder = directory;
            this.MapFolder = theFolder;
            break;
          }
          if (Path.GetFileName(directory) == "MapClasses")
          {
            this.MapFolder = directory;
            num = 99;
            break;
          }
          this.MapFolder = (string) null;
        }
        if (string.IsNullOrEmpty(this.MapFolder))
          theFolder = theFolder.Substring(0, theFolder.LastIndexOf('\\'));
      }
      return this.MapFolder;
    }

    private void InitFormular()
    {
      try
      {
        this.TextBoxMapPath.TextChanged += new TextChangedEventHandler(this.TextBoxMapPath_TextChanged);
        this.ComboBoxAvailableReader.Text = this.ClassName;
        this.TheFolder = Path.GetDirectoryName(new StackTrace(true).GetFrame(1).GetFileName());
        if (!string.IsNullOrEmpty(this.InitMapFolder))
          this.TheFolder = this.InitMapFolder;
        this.MapFolder = this.searchDirectory(this.TheFolder);
      }
      catch (Exception ex)
      {
        throw new MapExceptionClass(MAP_EXCEPTION_HANDLE.MAP_PATH_EMPTY, "MapClassManager Exception!!!", new Exception("Error occurred in class: MapClassManager\n, Switch to DEBUG mode in Visual Studio and try compile the code again :)", ex));
      }
      if (this.MapFolder == null)
      {
        int num = (int) MessageBox.Show("Map class folder not found", "Find folder error");
        this.MapFolder = "";
      }
      else
      {
        this.SetValuesToDefault();
        this.InitFinished = true;
        this.SetMapClassPath();
        this.CheckStatus();
      }
    }

    private void componentsClick(object sender, RoutedEventArgs e)
    {
      this.NextPlugin = ((HeaderedItemsControl) sender).Header.ToString();
      this.Close();
    }

    private void CheckStatus()
    {
      if (!this.IsMapSelected)
        this.IsMapRead = false;
      this.CheckBoxReadListFiles.IsEnabled = false;
      this.CheckBoxReadListFiles.IsChecked = new bool?(this.UseListFiles);
      this.ButtonCreateClass.IsEnabled = this.IsMapRead;
      this.ButtonReadMapFile.IsEnabled = this.IsMapSelected;
      this.TextBoxVersionHex.IsEnabled = this.IsVersionEnabled;
      this.UpdateLayout();
    }

    private void SetValuesToDefault()
    {
      this.TextBoxMapPath.Text = string.Empty;
      this.TextBoxVersionHex.Text = "00000000";
      this.SetStatusString("");
    }

    private void SetMapClassPath()
    {
      if (!this.InitFinished)
        return;
      this.ClassName = "MapDefClass" + this.TextBoxVersionHex.Text.PadLeft(8, '0');
      this.TextBoxClassPath.Text = Path.Combine(this.MapFolder, this.ClassName + ".cs");
      this.DynamicMappingFolder = Path.Combine(this.MappingDataFolder, this.ClassName + ".cs");
    }

    private void ButtonSelectMapFile_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        this.SetValuesToDefault();
        OpenFileDialog openFileDialog = new OpenFileDialog();
        openFileDialog.Filter = "map files (*.map)|*.map|All files (*.*)|*.*";
        openFileDialog.FilterIndex = 1;
        openFileDialog.CheckFileExists = true;
        string str = PlugInLoader.GmmConfiguration.GetValue(nameof (MapClassManager), "OpenMapPath");
        if (!string.IsNullOrEmpty(str))
          openFileDialog.InitialDirectory = str;
        else
          openFileDialog.RestoreDirectory = true;
        if (!openFileDialog.ShowDialog().Value)
          return;
        string directoryName = Path.GetDirectoryName(Path.GetFullPath(openFileDialog.FileName));
        PlugInLoader.GmmConfiguration.SetOrUpdateValue(nameof (MapClassManager), "OpenMapPath", directoryName);
        this.TextBoxMapPath.Text = openFileDialog.FileName;
        this.TextBoxLinkerType.Text = string.Empty;
        string Linker = string.Empty;
        if (this.mapReader != null && this.mapReader.ReaderName.Contains("S3"))
          return;
        this.mapReader = MapReader.GetReaderForMapFile(this.TextBoxMapPath.Text, out Linker);
        this.ClassName = this.mapReader.ReaderName;
        this.TextBoxLinkerType.Text = Linker;
        this.ComboBoxAvailableReader.Text = this.ClassName;
        this.SetMapClassPath();
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message, "Reader Error");
      }
    }

    private async void ButtonReadMapFile_Click(object sender, RoutedEventArgs e)
    {
      int num1;
      if (num1 == 0 || !string.IsNullOrEmpty(this.TextBoxMapPath.Text))
      {
        try
        {
          this.TextBoxStatus.Text = " ... start reading map file ... ";
          string Path = this.TextBoxMapPath.Text.Trim();
          this.TextBoxLinkerType.Text = string.Empty;
          this.InvalidateVisual();
          this.mapReader.AddressRanges = this.AddressRanges;
          await Task.Run((Action) (() => this.mapReader.ReadMap(Path, this)));
          bool? isChecked = this.CheckBoxReadListFiles.IsChecked;
          bool flag = true;
          if (!(isChecked.GetValueOrDefault() == flag & isChecked.HasValue))
            ;
          if (!string.IsNullOrEmpty(this.mapReader.FirmwareVersion))
            this.TextBoxVersionHex.Text = Convert.ToUInt32(this.mapReader.FirmwareVersion).ToString("X4");
          else
            this.IsVersionEnabled = true;
          if (!string.IsNullOrEmpty(this.mapReader.LinkerTypeAndVersion))
            this.TextBoxLinkerType.Text = this.mapReader.LinkerTypeAndVersion;
          this.IsMapRead = true;
          this.CheckStatus();
          this.SetStatusString(" ... DONE ... ");
        }
        catch (Exception ex)
        {
          this.SetStatusString(" ... WRONG FORMAT OF MAP FILE ...");
          int num2 = (int) MessageBox.Show("An Error occoured:\n" + ex.ToString(), "Reader Error ...");
        }
      }
      else
      {
        int num3 = (int) MessageBox.Show("Please, select a map file first.", "Error");
      }
    }

    private void DoWorkReadingList() => this.mapReader.ReadLists(this.TextBoxMapPath.Text, this);

    public void SetStatusString(string status)
    {
      this.TextBoxStatus.Dispatcher.BeginInvoke((Delegate) (() => this.TextBoxStatus.Text = status));
      this.TextBoxStatus.Dispatcher.Invoke(DispatcherPriority.Render, (Delegate) MapClassManager.EmptyDelegate);
      this.InvalidateVisual();
    }

    private void ButtonCreateClass_Click(object sender, RoutedEventArgs e)
    {
      if (File.Exists(this.TextBoxClassPath.Text) && MessageBox.Show("The file exists! Overwrite?", "File overwrite", MessageBoxButton.OKCancel) != MessageBoxResult.OK)
        return;
      try
      {
        this.mapReader.GenerateByteArray(this.ClassNamespace, this.ClassName, this.TextBoxClassPath.Text);
        this.mapReader.GenerateByteArray(this.ClassNamespace, this.ClassName, this.DynamicMappingFolder);
        int num = (int) MessageBox.Show("Class file saved");
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message);
      }
    }

    private void TextBoxMapPath_TextChanged(object sender, TextChangedEventArgs e)
    {
      this.IsMapSelected = File.Exists(this.TextBoxMapPath.Text);
      this.CheckStatus();
    }

    private void TextBoxVersionHex_TextChanged(object sender, TextChangedEventArgs e)
    {
      uint result;
      if (!uint.TryParse(this.TextBoxVersionHex.Text, NumberStyles.HexNumber, (IFormatProvider) null, out result))
        return;
      this.TextBoxVersion.Text = new FirmwareVersion(result).ToString();
      this.SetMapClassPath();
    }

    private void ComboBoxAvailableReader_SelectionChanged(object sender, TextChangedEventArgs e)
    {
      this.TextBoxReaderDescription.Text = this.mapReader.ReaderDescription;
      this.IsListAvail = this.UseListFiles;
      this.IsVersionEnabled = this.mapReader.ReaderName.Equals("MapReaderS3");
      this.IsVersionEnabled = this.TextBoxVersion.Text.Equals("00000000");
      this.CheckStatus();
    }

    private void ButtonGenerateParameterFile_Click(object sender, RoutedEventArgs e)
    {
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/HandlerLib;component/hardwaremanagement/mapclassmanager.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.menuMain = (Menu) target;
          break;
        case 2:
          this.MenuItemComponents = (MenuItem) target;
          break;
        case 3:
          this.gmmCorporateControl1 = (GmmCorporateControl) target;
          break;
        case 4:
          this.TextBoxMapPath = (TextBox) target;
          break;
        case 5:
          this.TextBoxVersion = (TextBox) target;
          break;
        case 6:
          this.TextBoxVersionHex = (TextBox) target;
          this.TextBoxVersionHex.TextChanged += new TextChangedEventHandler(this.TextBoxVersionHex_TextChanged);
          break;
        case 7:
          this.TextBoxClassPath = (TextBox) target;
          break;
        case 8:
          this.TextBoxLinkerType = (TextBox) target;
          break;
        case 9:
          this.ComboBoxAvailableReader = (TextBox) target;
          this.ComboBoxAvailableReader.TextChanged += new TextChangedEventHandler(this.ComboBoxAvailableReader_SelectionChanged);
          break;
        case 10:
          this.TextBoxReaderDescription = (TextBox) target;
          break;
        case 11:
          this.ButtonReadMapFile = (Button) target;
          this.ButtonReadMapFile.Click += new RoutedEventHandler(this.ButtonReadMapFile_Click);
          break;
        case 12:
          this.ButtonCreateClass = (Button) target;
          this.ButtonCreateClass.Click += new RoutedEventHandler(this.ButtonCreateClass_Click);
          break;
        case 13:
          this.ButtonSelectMapFile = (Button) target;
          this.ButtonSelectMapFile.Click += new RoutedEventHandler(this.ButtonSelectMapFile_Click);
          break;
        case 14:
          this.CheckBoxReadListFiles = (CheckBox) target;
          break;
        case 15:
          this.TextBoxStatus = (TextBox) target;
          break;
        case 16:
          this.ButtonGenerateParameterFile = (Button) target;
          this.ButtonGenerateParameterFile.Click += new RoutedEventHandler(this.ButtonGenerateParameterFile_Click);
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
