// Decompiled with JetBrains decompiler
// Type: HandlerLib.TypeWindow
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

using GmmDbLib;
using StartupLib;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;

#nullable disable
namespace HandlerLib
{
  public class TypeWindow : Window, IComponentConnector
  {
    private MeterInfo meterInfo;
    private string[] hardwareName;
    internal TextBox TextBoxSapNumber;
    internal Button ButtonOpen;
    internal DataGrid DataGridTypes;
    internal MenuItem MenuItemDelete;
    private bool _contentLoaded;

    private TypeWindow() => this.InitializeComponent();

    public static MeterInfo ShowDialog(Window owner, string hardwareName)
    {
      return TypeWindow.ShowDialog(owner, new string[1]
      {
        hardwareName
      });
    }

    public static MeterInfo ShowDialog(Window owner, string[] hardwareName)
    {
      List<MeterInfo> meterInfoList = MeterInfo.LoadMeterInfo(hardwareName);
      TypeWindow typeWindow = new TypeWindow();
      typeWindow.Owner = owner;
      typeWindow.hardwareName = hardwareName;
      typeWindow.DataGridTypes.ItemsSource = (IEnumerable) meterInfoList;
      if (meterInfoList != null)
        typeWindow.Title = "Types (" + meterInfoList.Count.ToString() + ")";
      bool? nullable = typeWindow.ShowDialog();
      bool flag = true;
      return !(nullable.GetValueOrDefault() == flag & nullable.HasValue) || typeWindow.meterInfo == null ? (MeterInfo) null : typeWindow.meterInfo;
    }

    private void TextBoxSapNumber_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key != Key.Return || !this.OpenBySapNumber())
        return;
      this.DialogResult = new bool?(true);
      this.Close();
    }

    private void ButtonOpen_Click(object sender, RoutedEventArgs e)
    {
      if (!this.OpenBySapNumber())
        this.meterInfo = this.DataGridTypes.SelectedItem as MeterInfo;
      this.DialogResult = new bool?(true);
      this.Close();
    }

    private bool OpenBySapNumber()
    {
      string sapNumber = this.TextBoxSapNumber.Text.Trim();
      if (sapNumber != null && sapNumber.Length >= 6 && this.DataGridTypes.ItemsSource is List<MeterInfo>)
      {
        List<MeterInfo> itemsSource = (List<MeterInfo>) this.DataGridTypes.ItemsSource;
        int index = itemsSource.FindIndex((Predicate<MeterInfo>) (x => x.PPSArtikelNr == sapNumber));
        if (index >= 0)
        {
          this.meterInfo = itemsSource[index];
          return true;
        }
      }
      return false;
    }

    private void DataGridTypes_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
      if (e.LeftButton != MouseButtonState.Pressed || this.DataGridTypes.SelectedItem == null)
        return;
      this.ButtonOpen_Click(sender, (RoutedEventArgs) null);
    }

    private void MenuItemDelete_Click(object sender, RoutedEventArgs e)
    {
      this.meterInfo = this.DataGridTypes.SelectedItem as MeterInfo;
      if (this.meterInfo == null)
        return;
      if (!UserManager.CheckPermission("Developer"))
      {
        int num1 = (int) MessageBox.Show((Window) this, "You have no permission to delete type!", "Access forbidden", MessageBoxButton.OK, MessageBoxImage.Asterisk);
      }
      else
      {
        if (MessageBox.Show((Window) this, "Are you sure?", "Delete?", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
          return;
        try
        {
          BaseType.DeleteType(this.meterInfo);
          List<MeterInfo> meterInfoList = MeterInfo.LoadMeterInfo(this.hardwareName);
          this.DataGridTypes.ItemsSource = (IEnumerable) meterInfoList;
          if (meterInfoList == null)
            return;
          this.Title = "Types (" + meterInfoList.Count.ToString() + ")";
        }
        catch (Exception ex)
        {
          int num2 = (int) MessageBox.Show((Window) this, ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
        }
      }
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/HandlerLib;component/view/typewindow.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.TextBoxSapNumber = (TextBox) target;
          this.TextBoxSapNumber.KeyDown += new KeyEventHandler(this.TextBoxSapNumber_KeyDown);
          break;
        case 2:
          this.ButtonOpen = (Button) target;
          this.ButtonOpen.Click += new RoutedEventHandler(this.ButtonOpen_Click);
          break;
        case 3:
          this.DataGridTypes = (DataGrid) target;
          this.DataGridTypes.MouseDoubleClick += new MouseButtonEventHandler(this.DataGridTypes_MouseDoubleClick);
          break;
        case 4:
          this.MenuItemDelete = (MenuItem) target;
          this.MenuItemDelete.Click += new RoutedEventHandler(this.MenuItemDelete_Click);
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
