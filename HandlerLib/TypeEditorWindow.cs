// Decompiled with JetBrains decompiler
// Type: HandlerLib.TypeEditorWindow
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

using GmmDbLib;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

#nullable disable
namespace HandlerLib
{
  public class TypeEditorWindow : Window, IComponentConnector
  {
    private string[] hardwareName;
    private byte[] compressedData;
    private BaseType baseType;
    private int? meterInfoID;
    internal ComboBox ComboBoxHardwareType;
    internal TextBox TextBoxSapNumber;
    internal TextBox TextBoxDescription;
    internal TextBox TextBoxTypeCreationString;
    internal CheckBox CheckBoxBASETYPE;
    internal Button ButtonCreate;
    internal Button ButtonSave;
    private bool _contentLoaded;

    private TypeEditorWindow() => this.InitializeComponent();

    public static int? ShowDialog(
      Window owner,
      string hardwareName,
      BaseType baseType,
      byte[] compressedData,
      string newTypeCreationString = null)
    {
      return TypeEditorWindow.ShowDialog(owner, new string[1]
      {
        hardwareName
      }, baseType, compressedData, newTypeCreationString);
    }

    public static int? ShowDialog(
      Window owner,
      string[] hardwareName,
      BaseType baseType,
      byte[] compressedData,
      string newTypeCreationString = null)
    {
      List<HardwareType> hardwareTypeList = HardwareType.LoadHardwareType(hardwareName);
      TypeEditorWindow typeEditorWindow = new TypeEditorWindow();
      typeEditorWindow.Owner = owner;
      typeEditorWindow.baseType = baseType;
      typeEditorWindow.hardwareName = hardwareName;
      typeEditorWindow.compressedData = compressedData;
      typeEditorWindow.ComboBoxHardwareType.ItemsSource = (IEnumerable) hardwareTypeList;
      typeEditorWindow.ButtonSave.IsEnabled = baseType != null;
      if (baseType != null)
      {
        typeEditorWindow.TextBoxSapNumber.Text = baseType.MeterInfo.PPSArtikelNr;
        typeEditorWindow.TextBoxDescription.Text = baseType.MeterInfo.Description;
        typeEditorWindow.TextBoxTypeCreationString.Text = baseType.Data.TypeCreationString;
        if (hardwareTypeList != null)
          typeEditorWindow.ComboBoxHardwareType.SelectedItem = (object) hardwareTypeList.Find((Predicate<HardwareType>) (x => x.HardwareTypeID == baseType.MeterInfo.HardwareTypeID));
      }
      if (newTypeCreationString != null)
        typeEditorWindow.TextBoxTypeCreationString.Text = newTypeCreationString;
      bool? nullable = typeEditorWindow.ShowDialog();
      bool flag = false;
      return nullable.GetValueOrDefault() == flag & nullable.HasValue ? new int?() : typeEditorWindow.meterInfoID;
    }

    private void ButtonCreate_Click(object sender, RoutedEventArgs e)
    {
      if (string.IsNullOrEmpty(this.TextBoxSapNumber.Text.Trim()))
      {
        int num1 = (int) MessageBox.Show((Window) this, "SAP Number can not be empty!", "SAP Number", MessageBoxButton.OK, MessageBoxImage.Hand);
      }
      else if (!(this.ComboBoxHardwareType.SelectedItem is HardwareType selectedItem))
      {
        int num2 = (int) MessageBox.Show((Window) this, "Hardware type is not selected!", "Hardware type", MessageBoxButton.OK, MessageBoxImage.Hand);
      }
      else if (string.IsNullOrEmpty(this.TextBoxDescription.Text.Trim()))
      {
        int num3 = (int) MessageBox.Show((Window) this, "Description can not be empty!", "Description", MessageBoxButton.OK, MessageBoxImage.Hand);
      }
      else
      {
        if (!string.IsNullOrEmpty(this.TextBoxTypeCreationString.Text.Trim()))
        {
          try
          {
            OverwriteSupport.PrepareOverwriteData(this.TextBoxTypeCreationString.Text);
          }
          catch (Exception ex)
          {
            int num4 = (int) MessageBox.Show((Window) this, ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
            return;
          }
        }
        try
        {
          int meterHardwareId = this.baseType == null || this.baseType.MeterInfo == null ? 0 : this.baseType.MeterInfo.MeterHardwareID;
          this.meterInfoID = BaseType.CreateType(selectedItem.HardwareName, this.TextBoxSapNumber.Text.Trim(), selectedItem.HardwareTypeID, meterHardwareId, this.TextBoxDescription.Text.Trim(), this.compressedData, this.TextBoxTypeCreationString.Text, this.CheckBoxBASETYPE.IsChecked.Value);
        }
        catch (Exception ex)
        {
          int num5 = (int) MessageBox.Show((Window) this, ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
        }
        this.DialogResult = new bool?(true);
        this.Close();
      }
    }

    private void ButtonSave_Click(object sender, RoutedEventArgs e)
    {
      if (this.baseType == null || this.baseType.MeterInfo == null || this.baseType.MeterInfo.MeterInfoID == 0)
      {
        int num1 = (int) MessageBox.Show((Window) this, "Invalid BaseType!", "BaseType error", MessageBoxButton.OK, MessageBoxImage.Hand);
      }
      else if (string.IsNullOrEmpty(this.TextBoxSapNumber.Text.Trim()))
      {
        int num2 = (int) MessageBox.Show((Window) this, "SAP Number can not be empty!", "SAP Number", MessageBoxButton.OK, MessageBoxImage.Hand);
      }
      else if (!(this.ComboBoxHardwareType.SelectedItem is HardwareType selectedItem))
      {
        int num3 = (int) MessageBox.Show((Window) this, "Hardware type is not selected!", "Hardware type", MessageBoxButton.OK, MessageBoxImage.Hand);
      }
      else if (string.IsNullOrEmpty(this.TextBoxDescription.Text.Trim()))
      {
        int num4 = (int) MessageBox.Show((Window) this, "Description can not be empty!", "Description", MessageBoxButton.OK, MessageBoxImage.Hand);
      }
      else
      {
        try
        {
          int? meterTypeID = this.baseType.MeterType != null ? new int?(this.baseType.MeterType.MeterTypeID) : new int?();
          BaseType.UpdateType(selectedItem.HardwareName, this.baseType.MeterInfo.MeterInfoID, meterTypeID, this.TextBoxSapNumber.Text.Trim(), selectedItem.HardwareTypeID, this.TextBoxDescription.Text.Trim(), this.compressedData, this.TextBoxTypeCreationString.Text);
          this.meterInfoID = new int?(this.baseType.MeterInfo.MeterInfoID);
        }
        catch (Exception ex)
        {
          int num5 = (int) MessageBox.Show((Window) this, ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
        }
        this.DialogResult = new bool?(true);
        this.Close();
      }
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/HandlerLib;component/view/typeeditorwindow.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.ComboBoxHardwareType = (ComboBox) target;
          break;
        case 2:
          this.TextBoxSapNumber = (TextBox) target;
          break;
        case 3:
          this.TextBoxDescription = (TextBox) target;
          break;
        case 4:
          this.TextBoxTypeCreationString = (TextBox) target;
          break;
        case 5:
          this.CheckBoxBASETYPE = (CheckBox) target;
          break;
        case 6:
          this.ButtonCreate = (Button) target;
          this.ButtonCreate.Click += new RoutedEventHandler(this.ButtonCreate_Click);
          break;
        case 7:
          this.ButtonSave = (Button) target;
          this.ButtonSave.Click += new RoutedEventHandler(this.ButtonSave_Click);
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
