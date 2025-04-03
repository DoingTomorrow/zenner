// Decompiled with JetBrains decompiler
// Type: HandlerLib.OverwriteAndCompare
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

using CommonWPF;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using ZR_ClassLibrary;

#nullable disable
namespace HandlerLib
{
  public class OverwriteAndCompare : Window, IComponentConnector
  {
    public bool restartWindow = false;
    private string HandlerName;
    private SortedList<HandlerMeterObjects, DeviceMemory> SourceMemories;
    private SortedList<HandlerMeterObjects, DeviceMemory> DestinationMemories;
    private HandlerFunctionsForProduction HandlerFunctions;
    private bool AddOverWriteDevice;
    internal StartupLib.GmmCorporateControl gmmCorporateControl1;
    internal ListBox ListBoxSource;
    internal ListBox ListBoxDestination;
    internal ListBox ListBoxOverwriteGroups;
    internal Button ButtonShowGroupInfo;
    internal Button ButtonOverwrite;
    internal CheckBox CheckBoxSuppressAddresses;
    internal CheckBox CheckBoxSuppressKnownDiffs;
    internal CheckBox CheckBoxOnlySelectedGroups;
    internal Button ButtonSourceMap;
    internal Button ButtonMapDiff;
    internal Button ButtonMemoryDiff;
    internal Button ButtonDataDiff;
    internal Button ButtonSaveSourceObject;
    internal Button ButtonOpenCompareFileFolder;
    private bool _contentLoaded;

    public OverwriteAndCompare(
      string handlerName,
      SortedList<HandlerMeterObjects, DeviceMemory> allMemories,
      HandlerFunctionsForProduction handlerFunctions,
      bool addOverWriteDevice = false)
    {
      this.HandlerName = handlerName;
      this.SourceMemories = allMemories;
      this.DestinationMemories = allMemories;
      this.HandlerFunctions = handlerFunctions;
      this.AddOverWriteDevice = addOverWriteDevice;
      this.InitializeComponent();
    }

    public OverwriteAndCompare(
      string handlerName,
      SortedList<HandlerMeterObjects, DeviceMemory> sourceMemories,
      SortedList<HandlerMeterObjects, DeviceMemory> destinationMemories,
      HandlerFunctionsForProduction handlerFunctions)
    {
      this.HandlerName = handlerName;
      this.SourceMemories = sourceMemories;
      this.DestinationMemories = destinationMemories;
      this.HandlerFunctions = handlerFunctions;
      this.InitializeComponent();
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      this.ListBoxSource.ItemsSource = (IEnumerable) this.SourceMemories.Keys.ToArray<HandlerMeterObjects>();
      if (!this.AddOverWriteDevice)
      {
        this.ListBoxDestination.ItemsSource = (IEnumerable) this.DestinationMemories.Keys.ToArray<HandlerMeterObjects>();
      }
      else
      {
        List<HandlerMeterObjects> list = this.DestinationMemories.Keys.ToList<HandlerMeterObjects>();
        if (list.Count == 0)
          list.Add(HandlerMeterObjects.ConnectedDevice);
        this.ListBoxDestination.ItemsSource = (IEnumerable) list;
      }
      bool flag = true;
      if (this.ListBoxSource.Items.Count > 0)
      {
        foreach (object obj in (IEnumerable) this.ListBoxSource.Items)
        {
          if (obj.ToString() == HandlerMeterObjects.TypeMeter.ToString())
          {
            this.ListBoxSource.SelectedItem = obj;
            break;
          }
        }
        if (this.ListBoxSource.SelectedIndex < 0)
        {
          foreach (object obj in (IEnumerable) this.ListBoxSource.Items)
          {
            if (obj.ToString() == HandlerMeterObjects.ConnectedMeter.ToString())
            {
              this.ListBoxSource.SelectedItem = obj;
              break;
            }
          }
        }
        if (this.ListBoxSource.SelectedIndex < 0)
          this.ListBoxSource.SelectedIndex = 0;
      }
      else
        flag = false;
      if (this.ListBoxDestination.Items.Count > 0)
      {
        foreach (object obj in (IEnumerable) this.ListBoxDestination.Items)
        {
          if (obj.ToString() == HandlerMeterObjects.WorkMeter.ToString())
          {
            this.ListBoxDestination.SelectedItem = obj;
            break;
          }
        }
        if (this.ListBoxDestination.SelectedIndex < 0)
          this.ListBoxDestination.SelectedIndex = 0;
      }
      else
        flag = false;
      if (!flag)
      {
        this.ButtonDataDiff.IsEnabled = false;
        this.ButtonMemoryDiff.IsEnabled = false;
      }
      this.ListBoxOverwriteGroups.ItemsSource = (IEnumerable) this.HandlerFunctions.GetAllOverwriteGroups();
    }

    private void ListBoxOverwriteGroups_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (this.ListBoxOverwriteGroups.SelectedItems.Count > 0)
        this.ButtonOverwrite.IsEnabled = true;
      else
        this.ButtonOverwrite.IsEnabled = false;
      if (this.ListBoxOverwriteGroups.SelectedItems.Count == 1)
        this.ButtonShowGroupInfo.IsEnabled = true;
      else
        this.ButtonShowGroupInfo.IsEnabled = false;
    }

    private void ButtonOverwrite_Click(object sender, RoutedEventArgs e)
    {
      if (this.ListBoxSource.SelectedItem == null)
        return;
      try
      {
        HandlerMeterObjects sourceObject = (HandlerMeterObjects) Enum.Parse(typeof (HandlerMeterObjects), this.ListBoxSource.SelectedItem.ToString());
        HandlerMeterObjects destinationObject = (HandlerMeterObjects) Enum.Parse(typeof (HandlerMeterObjects), this.ListBoxDestination.SelectedItem.ToString());
        CommonOverwriteGroups[] overwriteGroups = new CommonOverwriteGroups[this.ListBoxOverwriteGroups.SelectedItems.Count];
        for (int index = 0; index < overwriteGroups.Length; ++index)
        {
          string str = this.ListBoxOverwriteGroups.SelectedItems[index].ToString();
          overwriteGroups[index] = (CommonOverwriteGroups) Enum.Parse(typeof (CommonOverwriteGroups), str);
        }
        this.HandlerFunctions.OverwriteSrcToDest(sourceObject, destinationObject, overwriteGroups);
        int num = (int) MessageBox.Show("Overwrite done");
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
    }

    private void ButtonMemoryDiff_Click(object sender, RoutedEventArgs e)
    {
      DeviceMemory deviceMemory1 = this.SourceMemories.Values[this.ListBoxSource.SelectedIndex];
      DeviceMemory deviceMemory2 = this.DestinationMemories.Values[this.ListBoxDestination.SelectedIndex];
      DeviceMemory deviceMemory3 = deviceMemory1;
      string handlerName = this.HandlerName;
      HandlerMeterObjects key = this.SourceMemories.Keys[this.ListBoxSource.SelectedIndex];
      string originalTag = key.ToString();
      key = this.DestinationMemories.Keys[this.ListBoxDestination.SelectedIndex];
      string compareTag = key.ToString();
      DeviceMemory compareMemory = deviceMemory2;
      deviceMemory3.CompareMemoryInfo(handlerName, originalTag, compareTag, compareMemory);
    }

    private void ButtonSourceMap_Click(object sender, RoutedEventArgs e)
    {
      DeviceMemory deviceMemory = this.SourceMemories.Values[this.ListBoxSource.SelectedIndex];
      string handlerName = this.HandlerName;
      string originalTag = this.SourceMemories.Keys[this.ListBoxSource.SelectedIndex].ToString();
      bool? isChecked = this.CheckBoxSuppressAddresses.IsChecked;
      int num1 = isChecked.Value ? 1 : 0;
      isChecked = this.CheckBoxSuppressKnownDiffs.IsChecked;
      int num2 = isChecked.Value ? 1 : 0;
      CommonOverwriteGroups[] selectedOverwriteGroups = this.GetSelectedOverwriteGroups();
      deviceMemory.ShowParameterInfo(handlerName, originalTag, num1 != 0, num2 != 0, selectedOverwriteGroups);
    }

    private void ButtonMapDiff_Click(object sender, RoutedEventArgs e)
    {
      DeviceMemory deviceMemory1 = this.SourceMemories.Values[this.ListBoxSource.SelectedIndex];
      DeviceMemory deviceMemory2 = this.DestinationMemories.Values[this.ListBoxDestination.SelectedIndex];
      DeviceMemory deviceMemory3 = deviceMemory1;
      string handlerName = this.HandlerName;
      HandlerMeterObjects key = this.SourceMemories.Keys[this.ListBoxSource.SelectedIndex];
      string originalTag = key.ToString();
      key = this.DestinationMemories.Keys[this.ListBoxDestination.SelectedIndex];
      string compareTag = key.ToString();
      DeviceMemory compareMemory = deviceMemory2;
      bool? isChecked = this.CheckBoxSuppressAddresses.IsChecked;
      int num1 = isChecked.Value ? 1 : 0;
      isChecked = this.CheckBoxSuppressKnownDiffs.IsChecked;
      int num2 = isChecked.Value ? 1 : 0;
      CommonOverwriteGroups[] selectedOverwriteGroups = this.GetSelectedOverwriteGroups();
      deviceMemory3.CompareParameterInfo(handlerName, originalTag, compareTag, compareMemory, num1 != 0, num2 != 0, selectedOverwriteGroups);
    }

    private CommonOverwriteGroups[] GetSelectedOverwriteGroups()
    {
      CommonOverwriteGroups[] selectedOverwriteGroups = (CommonOverwriteGroups[]) null;
      if (this.CheckBoxOnlySelectedGroups.IsChecked.Value)
      {
        if (this.ListBoxOverwriteGroups.SelectedItems != null && this.ListBoxOverwriteGroups.SelectedItems.Count > 0)
        {
          try
          {
            selectedOverwriteGroups = new CommonOverwriteGroups[this.ListBoxOverwriteGroups.SelectedItems.Count];
            for (int index = 0; index < selectedOverwriteGroups.Length; ++index)
            {
              string str = this.ListBoxOverwriteGroups.SelectedItems[index].ToString();
              selectedOverwriteGroups[index] = (CommonOverwriteGroups) Enum.Parse(typeof (CommonOverwriteGroups), str);
            }
          }
          catch
          {
            selectedOverwriteGroups = (CommonOverwriteGroups[]) null;
          }
        }
      }
      return selectedOverwriteGroups;
    }

    private void ButtonDataDiff_Click(object sender, RoutedEventArgs e)
    {
      DeviceMemory deviceMemory1 = this.SourceMemories.Values[this.ListBoxSource.SelectedIndex];
      DeviceMemory deviceMemory2 = this.DestinationMemories.Values[this.ListBoxDestination.SelectedIndex];
      DeviceMemory deviceMemory3 = deviceMemory1;
      string handlerName = this.HandlerName;
      HandlerMeterObjects key = this.SourceMemories.Keys[this.ListBoxSource.SelectedIndex];
      string originalTag = key.ToString();
      key = this.DestinationMemories.Keys[this.ListBoxDestination.SelectedIndex];
      string compareTag = key.ToString();
      DeviceMemory compareMemory = deviceMemory2;
      deviceMemory3.CompareSortedParameterInfo(handlerName, originalTag, compareTag, compareMemory);
    }

    private void ButtonSaveSourceObject_Click(object sender, RoutedEventArgs e)
    {
      if (this.ListBoxSource.SelectedItem == null)
        return;
      try
      {
        this.HandlerFunctions.SaveMeterObject((HandlerMeterObjects) Enum.Parse(typeof (HandlerMeterObjects), this.ListBoxSource.SelectedItem.ToString()));
        this.restartWindow = true;
        this.Close();
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message);
      }
    }

    private void ListBoxSourceOrDestination_SelectionChanged(
      object sender,
      SelectionChangedEventArgs e)
    {
      if (this.ListBoxSource.SelectedItem == null || this.ListBoxDestination.SelectedItem == null)
      {
        this.ButtonDataDiff.IsEnabled = false;
        this.ButtonMemoryDiff.IsEnabled = false;
        this.ButtonOverwrite.IsEnabled = false;
      }
      else if (this.SourceMemories[(HandlerMeterObjects) this.ListBoxSource.SelectedItem] == null || this.SourceMemories[(HandlerMeterObjects) this.ListBoxSource.SelectedItem] == null)
      {
        this.ButtonDataDiff.IsEnabled = false;
        this.ButtonMemoryDiff.IsEnabled = false;
      }
      else
      {
        this.ButtonDataDiff.IsEnabled = true;
        this.ButtonMemoryDiff.IsEnabled = true;
      }
    }

    private void ButtonShowGroupInfo_Click(object sender, RoutedEventArgs e)
    {
      CommonOverwriteGroups overwriteGroupe = (CommonOverwriteGroups) Enum.Parse(typeof (CommonOverwriteGroups), this.ListBoxOverwriteGroups.SelectedItem.ToString());
      int num = (int) MessageBox.Show(this.HandlerFunctions.GetOverwriteGroupInfo(overwriteGroupe), overwriteGroupe.ToString() + " group infos");
    }

    private void ButtonOpenCompareFileFolder_Click(object sender, RoutedEventArgs e)
    {
      new Process()
      {
        StartInfo = {
          FileName = "explorer",
          Arguments = SystemValues.LoggDataPath
        }
      }.Start();
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/HandlerLib;component/view/overwriteandcompare.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          ((FrameworkElement) target).Loaded += new RoutedEventHandler(this.Window_Loaded);
          break;
        case 2:
          this.gmmCorporateControl1 = (StartupLib.GmmCorporateControl) target;
          break;
        case 3:
          this.ListBoxSource = (ListBox) target;
          this.ListBoxSource.SelectionChanged += new SelectionChangedEventHandler(this.ListBoxSourceOrDestination_SelectionChanged);
          break;
        case 4:
          this.ListBoxDestination = (ListBox) target;
          this.ListBoxDestination.SelectionChanged += new SelectionChangedEventHandler(this.ListBoxSourceOrDestination_SelectionChanged);
          break;
        case 5:
          this.ListBoxOverwriteGroups = (ListBox) target;
          this.ListBoxOverwriteGroups.SelectionChanged += new SelectionChangedEventHandler(this.ListBoxOverwriteGroups_SelectionChanged);
          break;
        case 6:
          this.ButtonShowGroupInfo = (Button) target;
          this.ButtonShowGroupInfo.Click += new RoutedEventHandler(this.ButtonShowGroupInfo_Click);
          break;
        case 7:
          this.ButtonOverwrite = (Button) target;
          this.ButtonOverwrite.Click += new RoutedEventHandler(this.ButtonOverwrite_Click);
          break;
        case 8:
          this.CheckBoxSuppressAddresses = (CheckBox) target;
          break;
        case 9:
          this.CheckBoxSuppressKnownDiffs = (CheckBox) target;
          break;
        case 10:
          this.CheckBoxOnlySelectedGroups = (CheckBox) target;
          break;
        case 11:
          this.ButtonSourceMap = (Button) target;
          this.ButtonSourceMap.Click += new RoutedEventHandler(this.ButtonSourceMap_Click);
          break;
        case 12:
          this.ButtonMapDiff = (Button) target;
          this.ButtonMapDiff.Click += new RoutedEventHandler(this.ButtonMapDiff_Click);
          break;
        case 13:
          this.ButtonMemoryDiff = (Button) target;
          this.ButtonMemoryDiff.Click += new RoutedEventHandler(this.ButtonMemoryDiff_Click);
          break;
        case 14:
          this.ButtonDataDiff = (Button) target;
          this.ButtonDataDiff.Click += new RoutedEventHandler(this.ButtonDataDiff_Click);
          break;
        case 15:
          this.ButtonSaveSourceObject = (Button) target;
          this.ButtonSaveSourceObject.Click += new RoutedEventHandler(this.ButtonSaveSourceObject_Click);
          break;
        case 16:
          this.ButtonOpenCompareFileFolder = (Button) target;
          this.ButtonOpenCompareFileFolder.Click += new RoutedEventHandler(this.ButtonOpenCompareFileFolder_Click);
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
