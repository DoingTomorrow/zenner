// Decompiled with JetBrains decompiler
// Type: HandlerLib.ChangeIdentWindow
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

using CommonWPF;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using ZENNER.CommonLibrary;

#nullable disable
namespace HandlerLib
{
  public class ChangeIdentWindow : Window, IComponentConnector
  {
    private DeviceIdentification theIdent;
    private Brush defaultBackground;
    internal StartupLib.GmmCorporateControl gmmCorporateControl1;
    internal TextBox TextBoxFirmwareVersion;
    internal TextBox TextBoxUnique_ID;
    internal TextBox TextBoxSvnRevision;
    internal TextBox TextBoxBuildTime;
    internal TextBox TextBoxSignature;
    internal TextBox TextBoxHardwareID;
    internal Button ButtonHardwareID;
    internal TextBox TextBoxFullSerialNumber;
    internal Button ButtonFullSerialNumber;
    internal TextBox TextBoxIdentIficationPrefix;
    internal Button ButtonIdentificationPrefix;
    internal TextBox TextBoxFabricationNumber;
    internal Button ButtonFabricationNumber;
    internal TextBox TextBoxID_BCD;
    internal Button ButtonID_BCD;
    internal TextBox TextBoxManufacturer;
    internal TextBox TextBoxManufacturerName;
    internal Button ButtonManufacturer;
    internal TextBox TextBoxGeneration;
    internal Button ButtonGeneration;
    internal TextBox TextBoxMedium;
    internal TextBox TextBoxMediumText;
    internal Button ButtonMedium;
    internal TextBox TextBoxAesKey;
    internal Button ButtonAesKey;
    internal TextBox TextBoxDevEUI;
    internal Button ButtonDevEUI;
    internal TextBox TextBoxJoinEUI;
    internal Button ButtonAppEUI;
    internal TextBox TextBoxAppKey;
    internal Button ButtonAppKey;
    internal TextBox TextBoxMeterID;
    internal Button ButtonMeterID;
    internal TextBox TextBoxHardwareTypeID;
    internal Button ButtonHardwareTypeID;
    internal TextBox TextBoxMeterInfoID;
    internal Button ButtonMeterInfoID;
    internal TextBox TextBoxMeterTypeID;
    internal Button ButtonMeterTypeID;
    internal TextBox TextBoxBaseTypeID;
    internal Button ButtonBaseTypeID;
    internal TextBox TextBoxSubPartNumber;
    internal Button ButtonSubPartNumber;
    internal TextBox TextBoxSAP_MaterialNumber;
    internal Button ButtonSAP_MaterialNumber;
    internal TextBox TextBoxSAP_ProductionOrderNumber;
    internal Button ButtonSAP_ProductionOrderNumber;
    internal TextBox TextBoxPrintedSerialNumber;
    internal Button ButtonPrintedSerialNumber;
    internal TextBox TextBoxApprovalRevision;
    internal Button ButtonApprovalRevision;
    internal TextBox TextBox_FD_ID_BCD;
    internal TextBox TextBox_FD_Manufacturer;
    internal TextBox TextBox_FD_ManufacturerName;
    internal TextBox TextBox_FD_Generation;
    internal TextBox TextBox_FD_Medium;
    internal TextBox TextBox_FD_MediumText;
    internal TextBox TextBox_FD_AES_Key;
    internal TextBox TextBox_FD_DevEUI;
    internal TextBox TextBox_FD_JoinEUI;
    internal TextBox TextBox_FD_AppKey;
    internal Button ButtonDevEUI_FromFullSerialNumber;
    internal Button ButtonSetFactoryDefaultValues;
    private bool _contentLoaded;

    public ChangeIdentWindow(DeviceIdentification currentIdentification)
    {
      this.theIdent = currentIdentification;
      this.InitializeComponent();
      this.defaultBackground = this.TextBoxMeterID.Background;
      this.UpdateData();
    }

    private void UpdateData()
    {
      try
      {
        this.TextBoxFirmwareVersion.Text = this.theIdent.GetFirmwareVersionString();
        this.TextBoxUnique_ID.Text = this.theIdent.GetUnique_ID_String();
        this.TextBoxSvnRevision.Text = this.theIdent.GetSvnRevisionString();
        this.TextBoxBuildTime.Text = this.theIdent.GetBuildTimeString();
        this.TextBoxSignature.Text = this.theIdent.GetSignaturString();
        this.TextBoxHardwareID.Text = this.theIdent.GetHardwareIDString();
        this.TextBoxID_BCD.Text = this.theIdent.ID_BCD_AsString;
        this.TextBoxManufacturer.Text = this.theIdent.ManufacturerAsString;
        this.TextBoxManufacturerName.Text = this.theIdent.ManufacturerName;
        this.TextBoxGeneration.Text = this.theIdent.GenerationAsString;
        this.TextBoxMedium.Text = this.theIdent.MediumAsString;
        this.TextBoxMediumText.Text = this.theIdent.GetMediumAsText();
        this.TextBoxAesKey.Text = this.theIdent.AES_Key_AsString;
        this.TextBoxPrintedSerialNumber.Text = this.theIdent.PrintedSerialNumberAsString;
        this.TextBoxApprovalRevision.Text = this.theIdent.ApprovalRevisionAsString;
        this.TextBoxFullSerialNumber.Text = this.theIdent.FullSerialNumber;
        this.TextBoxIdentIficationPrefix.Text = this.theIdent.IdentificationPrefix;
        this.TextBoxFabricationNumber.Text = this.theIdent.FabricationNumberAsString;
        this.TextBoxDevEUI.Text = this.theIdent.LoRa_DevEUI_AsString;
        this.TextBoxJoinEUI.Text = this.theIdent.LoRa_JoinEUI_AsString;
        this.TextBoxAppKey.Text = this.theIdent.LoRa_AppKey_AsString;
        this.TextBoxMeterID.Text = this.theIdent.GetMeterID_String();
        this.TextBoxHardwareTypeID.Text = this.theIdent.GetHardwareTypeID_String();
        this.TextBoxMeterInfoID.Text = this.theIdent.MeterInfoID_AsString;
        this.TextBoxMeterTypeID.Text = this.theIdent.GetMeterTypeID_String();
        this.TextBoxBaseTypeID.Text = this.theIdent.GetBaseTypeID_String();
        this.TextBoxSubPartNumber.Text = this.theIdent.GetSubPartNumber_String();
        this.TextBoxSAP_MaterialNumber.Text = this.theIdent.GetSAP_MaterialNumberString();
        this.TextBoxSAP_ProductionOrderNumber.Text = this.theIdent.SAP_ProductionOrderNumber;
        this.TextBox_FD_ID_BCD.Text = this.theIdent.FD_ID_BCD_AsString;
        this.TextBox_FD_Manufacturer.Text = this.theIdent.FD_ManufacturerAsString;
        this.TextBox_FD_ManufacturerName.Text = this.theIdent.FD_ManufacturerName;
        this.TextBox_FD_Generation.Text = this.theIdent.FD_GenerationAsString;
        this.TextBox_FD_Medium.Text = this.theIdent.FD_MediumAsString;
        this.TextBox_FD_MediumText.Text = this.theIdent.GetFD_MediumAsText();
        this.TextBox_FD_AES_Key.Text = this.theIdent.FD_AES_Key_AsString;
        this.TextBox_FD_DevEUI.Text = this.theIdent.FD_LoRa_DevEUI_AsString;
        this.TextBox_FD_JoinEUI.Text = this.theIdent.FD_LoRa_JoinEUI_AsString;
        this.TextBox_FD_AppKey.Text = this.theIdent.FD_LoRa_AppKey_AsString;
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex, "Update data exception");
      }
      this.ShowChangedColors();
    }

    private void ShowChangedColors()
    {
      try
      {
        this.SetTextBoxColor((object) this.TextBoxHardwareID, this.theIdent.GetHardwareIDString());
        this.SetTextBoxColor((object) this.TextBoxID_BCD, this.theIdent.ID_BCD_AsString);
        this.SetTextBoxColor((object) this.TextBoxManufacturer, this.theIdent.ManufacturerAsString);
        this.SetTextBoxColor((object) this.TextBoxManufacturerName, this.theIdent.ManufacturerName);
        this.SetTextBoxColor((object) this.TextBoxGeneration, this.theIdent.GenerationAsString);
        this.SetTextBoxColor((object) this.TextBoxMedium, this.theIdent.MediumAsString);
        this.SetTextBoxColor((object) this.TextBoxAesKey, this.theIdent.AES_Key_AsString);
        this.SetTextBoxColor((object) this.TextBoxPrintedSerialNumber, this.theIdent.PrintedSerialNumberAsString);
        this.SetTextBoxColor((object) this.TextBoxApprovalRevision, this.theIdent.ApprovalRevisionAsString);
        this.SetTextBoxColor((object) this.TextBoxFullSerialNumber, this.theIdent.FullSerialNumber);
        this.SetTextBoxColor((object) this.TextBoxIdentIficationPrefix, this.theIdent.IdentificationPrefix);
        this.SetTextBoxColor((object) this.TextBoxFabricationNumber, this.theIdent.FabricationNumberAsString);
        this.SetTextBoxColor((object) this.TextBoxDevEUI, this.theIdent.LoRa_DevEUI_AsString);
        this.SetTextBoxColor((object) this.TextBoxJoinEUI, this.theIdent.LoRa_JoinEUI_AsString);
        this.SetTextBoxColor((object) this.TextBoxAppKey, this.theIdent.LoRa_AppKey_AsString);
        this.SetTextBoxColor((object) this.TextBoxMeterID, this.theIdent.GetMeterID_String());
        this.SetTextBoxColor((object) this.TextBoxHardwareTypeID, this.theIdent.GetHardwareTypeID_String());
        this.SetTextBoxColor((object) this.TextBoxMeterInfoID, this.theIdent.MeterInfoID_AsString);
        this.SetTextBoxColor((object) this.TextBoxMeterTypeID, this.theIdent.GetMeterTypeID_String());
        this.SetTextBoxColor((object) this.TextBoxBaseTypeID, this.theIdent.GetBaseTypeID_String());
        this.SetTextBoxColor((object) this.TextBoxSubPartNumber, this.theIdent.GetSubPartNumber_String());
        this.SetTextBoxColor((object) this.TextBoxSAP_MaterialNumber, this.theIdent.GetSAP_MaterialNumberString());
        this.SetTextBoxColor((object) this.TextBoxSAP_ProductionOrderNumber, this.theIdent.SAP_ProductionOrderNumber);
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex, "ShowChangedColors exception");
      }
    }

    private void SetTextBox(object sender, string theValue)
    {
      try
      {
        ((TextBox) sender).Text = theValue;
        this.SetTextBoxColor(sender, theValue);
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex, "SetTextBox exception");
      }
    }

    private void SetTextBoxColor(object sender, string theValue)
    {
      if (((TextBox) sender).Text.Length == 0)
      {
        if (string.IsNullOrEmpty(theValue))
          ((Control) sender).Background = this.defaultBackground.Clone();
        else
          ((Control) sender).Background = (Brush) Brushes.LightYellow;
      }
      else if (string.IsNullOrEmpty(theValue))
        ((Control) sender).Background = (Brush) Brushes.LightYellow;
      else if (((TextBox) sender).Text == theValue)
        ((Control) sender).Background = this.defaultBackground.Clone();
      else
        ((Control) sender).Background = (Brush) Brushes.LightYellow;
    }

    private void TextBoxHardwareID_TextChanged(object sender, TextChangedEventArgs e)
    {
      try
      {
        this.SetTextBoxColor(sender, this.theIdent.GetHardwareIDString());
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
    }

    private void ButtonHardwareID_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        uint result;
        if (uint.TryParse(this.TextBoxHardwareID.Text, out result))
          this.theIdent.HardwareID = new uint?(result);
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
      this.TextBoxHardwareID.Text = this.theIdent.GetHardwareIDString();
    }

    private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
      try
      {
        this.SetTextBoxColor(sender, this.theIdent.ID_BCD_AsString);
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
    }

    private void ButtonID_BCD_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        uint result;
        if (uint.TryParse(this.TextBoxID_BCD.Text, NumberStyles.HexNumber, (IFormatProvider) null, out result))
          this.theIdent.ID_BCD = new uint?(result);
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
      this.UpdateData();
    }

    private void TextBoxManufacturer_TextChanged(object sender, TextChangedEventArgs e)
    {
      try
      {
        this.SetTextBoxColor(sender, this.theIdent.ManufacturerAsString);
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
    }

    private void TextBoxManufacturerText_TextChanged(object sender, TextChangedEventArgs e)
    {
      try
      {
        this.SetTextBoxColor(sender, this.theIdent.ManufacturerName);
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
    }

    private void ButtonManufacturer_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        if (this.TextBoxManufacturerName.Background == Brushes.LightYellow)
          this.theIdent.ManufacturerName = this.TextBoxManufacturerName.Text;
        else
          this.theIdent.ManufacturerAsString = this.TextBoxManufacturer.Text;
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
      this.UpdateData();
    }

    private void TextBoxGeneration_TextChanged(object sender, TextChangedEventArgs e)
    {
      try
      {
        this.SetTextBoxColor(sender, this.theIdent.GenerationAsString);
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
    }

    private void ButtonGeneration_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        byte result;
        if (byte.TryParse(this.TextBoxGeneration.Text, NumberStyles.HexNumber, (IFormatProvider) null, out result))
          this.theIdent.Generation = new byte?(result);
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
      this.UpdateData();
    }

    private void TextBoxMedium_TextChanged(object sender, TextChangedEventArgs e)
    {
      try
      {
        this.SetTextBoxColor(sender, this.theIdent.MediumAsString);
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
    }

    private void ButtonMedium_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        byte result;
        if (byte.TryParse(this.TextBoxMedium.Text.Substring(0, 2), NumberStyles.HexNumber, (IFormatProvider) null, out result))
          this.theIdent.Medium = new byte?(result);
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
      this.UpdateData();
    }

    private void TextBoxPrintedSerialNumber_TextChanged(object sender, TextChangedEventArgs e)
    {
      try
      {
        this.SetTextBoxColor(sender, this.theIdent.PrintedSerialNumberAsString);
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
    }

    private void ButtonPrintedSerialNumber_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        this.theIdent.PrintedSerialNumberAsString = this.TextBoxPrintedSerialNumber.Text;
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
      this.UpdateData();
    }

    private void TextBoxApprovalRevision_TextChanged(object sender, TextChangedEventArgs e)
    {
      try
      {
        ushort? approvalRevision = this.theIdent.ApprovalRevision;
        if (!approvalRevision.HasValue)
        {
          this.SetTextBoxColor(sender, (string) null);
        }
        else
        {
          object sender1 = sender;
          approvalRevision = this.theIdent.ApprovalRevision;
          string theValue = approvalRevision.ToString();
          this.SetTextBoxColor(sender1, theValue);
        }
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
    }

    private void ButtonApprovalRevision_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        this.theIdent.ApprovalRevision = new ushort?(ushort.Parse(this.TextBoxApprovalRevision.Text));
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
      this.UpdateData();
    }

    private void TextBoxFullSerialNumber_TextChanged(object sender, TextChangedEventArgs e)
    {
      try
      {
        this.SetTextBoxColor(sender, this.theIdent.FullSerialNumber);
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
    }

    private void ButtonFullSerialNumber_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        this.theIdent.FullSerialNumber = this.TextBoxFullSerialNumber.Text.Trim();
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
      this.UpdateData();
    }

    private void TextBoxIdentIficationPrefix_TextChanged(object sender, TextChangedEventArgs e)
    {
      try
      {
        this.SetTextBoxColor(sender, this.theIdent.IdentificationPrefix);
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
    }

    private void ButtonIdentificationPrefix_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        this.theIdent.IdentificationPrefix = this.TextBoxIdentIficationPrefix.Text;
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
      this.UpdateData();
    }

    private void TextBoxFabricationNumber_TextChanged(object sender, TextChangedEventArgs e)
    {
      try
      {
        this.SetTextBoxColor(sender, this.theIdent.FabricationNumberAsString);
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
    }

    private void ButtonFabricationNumber_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        this.theIdent.FabricationNumberAsString = this.TextBoxFabricationNumber.Text;
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
      this.UpdateData();
    }

    private void TextBoxDevEUI_TextChanged(object sender, TextChangedEventArgs e)
    {
      try
      {
        this.SetTextBoxColor(sender, this.theIdent.LoRa_DevEUI_AsString);
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
    }

    private void ButtonDevEUI_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        ulong result;
        if (ulong.TryParse(this.TextBoxDevEUI.Text, NumberStyles.HexNumber, (IFormatProvider) null, out result))
          this.theIdent.LoRa_DevEUI = new ulong?(result);
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
      this.UpdateData();
    }

    private void TextBoxAppEUI_TextChanged(object sender, TextChangedEventArgs e)
    {
      try
      {
        this.SetTextBoxColor(sender, this.theIdent.LoRa_JoinEUI_AsString);
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
    }

    private void ButtonAppEUI_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        ulong result;
        if (ulong.TryParse(this.TextBoxJoinEUI.Text, NumberStyles.HexNumber, (IFormatProvider) null, out result))
          this.theIdent.LoRa_JoinEUI = new ulong?(result);
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
      this.UpdateData();
    }

    private void TextBoxAppKey_TextChanged(object sender, TextChangedEventArgs e)
    {
      try
      {
        this.SetTextBoxColor(sender, this.theIdent.LoRa_AppKey_AsString);
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
    }

    private void ButtonAppKey_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        this.theIdent.LoRa_AppKey = Utility.HexStringToByteArray(this.TextBoxAppKey.Text);
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
      this.UpdateData();
    }

    private void TextBoxMeterID_TextChanged(object sender, TextChangedEventArgs e)
    {
      try
      {
        this.SetTextBoxColor(sender, this.theIdent.GetMeterID_String());
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
    }

    private void ButtonMeterID_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        uint result;
        if (uint.TryParse(this.TextBoxMeterID.Text, out result))
          this.theIdent.MeterID = new uint?(result);
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
      this.UpdateData();
    }

    private void TextBoxHardwareTypeID_TextChanged(object sender, TextChangedEventArgs e)
    {
      try
      {
        this.SetTextBoxColor(sender, this.theIdent.GetHardwareTypeID_String());
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
    }

    private void ButtonHardwareTypeID_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        uint result;
        if (uint.TryParse(this.TextBoxHardwareTypeID.Text, out result))
          this.theIdent.HardwareTypeID = new uint?(result);
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
      this.UpdateData();
    }

    private void TextBoxMeterInfoID_TextChanged(object sender, TextChangedEventArgs e)
    {
      try
      {
        this.SetTextBoxColor(sender, this.theIdent.MeterInfoID_AsString);
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
    }

    private void ButtonMeterInfoID_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        this.theIdent.MeterInfoID_AsString = this.TextBoxMeterInfoID.Text.Trim();
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
      this.UpdateData();
    }

    private void TextBoxMeterTypeID_TextChanged(object sender, TextChangedEventArgs e)
    {
      try
      {
        this.SetTextBoxColor(sender, this.theIdent.GetMeterTypeID_String());
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
    }

    private void ButtonMeterTypeID_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        uint result;
        if (uint.TryParse(this.TextBoxMeterTypeID.Text, out result))
          this.theIdent.MeterTypeID = new uint?(result);
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
      this.UpdateData();
    }

    private void TextBoxBaseTypeID_TextChanged(object sender, TextChangedEventArgs e)
    {
      try
      {
        this.SetTextBoxColor(sender, this.theIdent.GetBaseTypeID_String());
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
    }

    private void ButtonBaseTypeID_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        uint result;
        if (uint.TryParse(this.TextBoxBaseTypeID.Text, out result))
          this.theIdent.BaseTypeID = new uint?(result);
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
      this.UpdateData();
    }

    private void TextBoxSubPartNumber_TextChanged(object sender, TextChangedEventArgs e)
    {
      try
      {
        this.SetTextBoxColor(sender, this.theIdent.GetSubPartNumber_String());
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
    }

    private void ButtonSubPartNumber_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        uint result;
        if (uint.TryParse(this.TextBoxSubPartNumber.Text, out result))
          this.theIdent.SubPartNumber = new uint?(result);
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
      this.UpdateData();
    }

    private void TextBoxSAP_MaterialNumber_TextChanged(object sender, TextChangedEventArgs e)
    {
      try
      {
        this.SetTextBoxColor(sender, this.theIdent.GetSAP_MaterialNumberString());
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
    }

    private void ButtonSAP_MaterialNumber_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        uint result;
        if (uint.TryParse(this.TextBoxSAP_MaterialNumber.Text, out result))
          this.theIdent.SAP_MaterialNumber = new uint?(result);
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
      this.UpdateData();
    }

    private void TextBoxSAP_ProductionOrderNumber_TextChanged(object sender, TextChangedEventArgs e)
    {
      try
      {
        this.SetTextBoxColor(sender, this.theIdent.SAP_ProductionOrderNumber);
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
    }

    private void ButtonSAP_ProductionOrderNumber_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        string str = this.TextBoxSAP_ProductionOrderNumber.Text.Trim();
        this.theIdent.SAP_ProductionOrderNumber = string.IsNullOrEmpty(str) ? (string) null : str;
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
      this.UpdateData();
    }

    private void ButtonSetFactoryDefaultValues_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        this.theIdent.Set_FD_Values();
        this.UpdateData();
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex, "Error on set FD_Values");
      }
    }

    private void ButtonDevEUI_FromFullSerialNumber_Click(object sender, RoutedEventArgs e)
    {
      if (this.theIdent.FullSerialNumber == null)
        return;
      try
      {
        this.theIdent.LoRa_DevEUI = new ulong?(new IdentificationMapping(this.theIdent.FullSerialNumber).GetAsDevEUI_Value());
        this.UpdateData();
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex, "Can not create DevEUI");
      }
    }

    private void TextBoxAesKey_TextChanged(object sender, TextChangedEventArgs e)
    {
      try
      {
        this.SetTextBoxColor(sender, this.theIdent.AES_Key_AsString);
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
    }

    private void ButtonAesKey_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        this.theIdent.AES_Key = Utility.HexStringToByteArray(this.TextBoxAesKey.Text);
      }
      catch (Exception ex)
      {
        ExceptionViewer.Show(ex);
      }
      this.UpdateData();
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/HandlerLib;component/view/changeidentwindow.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.gmmCorporateControl1 = (StartupLib.GmmCorporateControl) target;
          break;
        case 2:
          this.TextBoxFirmwareVersion = (TextBox) target;
          break;
        case 3:
          this.TextBoxUnique_ID = (TextBox) target;
          break;
        case 4:
          this.TextBoxSvnRevision = (TextBox) target;
          break;
        case 5:
          this.TextBoxBuildTime = (TextBox) target;
          break;
        case 6:
          this.TextBoxSignature = (TextBox) target;
          break;
        case 7:
          this.TextBoxHardwareID = (TextBox) target;
          this.TextBoxHardwareID.TextChanged += new TextChangedEventHandler(this.TextBoxHardwareID_TextChanged);
          break;
        case 8:
          this.ButtonHardwareID = (Button) target;
          this.ButtonHardwareID.Click += new RoutedEventHandler(this.ButtonHardwareID_Click);
          break;
        case 9:
          this.TextBoxFullSerialNumber = (TextBox) target;
          this.TextBoxFullSerialNumber.TextChanged += new TextChangedEventHandler(this.TextBoxFullSerialNumber_TextChanged);
          break;
        case 10:
          this.ButtonFullSerialNumber = (Button) target;
          this.ButtonFullSerialNumber.Click += new RoutedEventHandler(this.ButtonFullSerialNumber_Click);
          break;
        case 11:
          this.TextBoxIdentIficationPrefix = (TextBox) target;
          this.TextBoxIdentIficationPrefix.TextChanged += new TextChangedEventHandler(this.TextBoxIdentIficationPrefix_TextChanged);
          break;
        case 12:
          this.ButtonIdentificationPrefix = (Button) target;
          this.ButtonIdentificationPrefix.Click += new RoutedEventHandler(this.ButtonIdentificationPrefix_Click);
          break;
        case 13:
          this.TextBoxFabricationNumber = (TextBox) target;
          this.TextBoxFabricationNumber.TextChanged += new TextChangedEventHandler(this.TextBoxFabricationNumber_TextChanged);
          break;
        case 14:
          this.ButtonFabricationNumber = (Button) target;
          this.ButtonFabricationNumber.Click += new RoutedEventHandler(this.ButtonFabricationNumber_Click);
          break;
        case 15:
          this.TextBoxID_BCD = (TextBox) target;
          this.TextBoxID_BCD.TextChanged += new TextChangedEventHandler(this.TextBox_TextChanged);
          break;
        case 16:
          this.ButtonID_BCD = (Button) target;
          this.ButtonID_BCD.Click += new RoutedEventHandler(this.ButtonID_BCD_Click);
          break;
        case 17:
          this.TextBoxManufacturer = (TextBox) target;
          this.TextBoxManufacturer.TextChanged += new TextChangedEventHandler(this.TextBoxManufacturer_TextChanged);
          break;
        case 18:
          this.TextBoxManufacturerName = (TextBox) target;
          this.TextBoxManufacturerName.TextChanged += new TextChangedEventHandler(this.TextBoxManufacturerText_TextChanged);
          break;
        case 19:
          this.ButtonManufacturer = (Button) target;
          this.ButtonManufacturer.Click += new RoutedEventHandler(this.ButtonManufacturer_Click);
          break;
        case 20:
          this.TextBoxGeneration = (TextBox) target;
          this.TextBoxGeneration.TextChanged += new TextChangedEventHandler(this.TextBoxGeneration_TextChanged);
          break;
        case 21:
          this.ButtonGeneration = (Button) target;
          this.ButtonGeneration.Click += new RoutedEventHandler(this.ButtonGeneration_Click);
          break;
        case 22:
          this.TextBoxMedium = (TextBox) target;
          this.TextBoxMedium.TextChanged += new TextChangedEventHandler(this.TextBoxMedium_TextChanged);
          break;
        case 23:
          this.TextBoxMediumText = (TextBox) target;
          break;
        case 24:
          this.ButtonMedium = (Button) target;
          this.ButtonMedium.Click += new RoutedEventHandler(this.ButtonMedium_Click);
          break;
        case 25:
          this.TextBoxAesKey = (TextBox) target;
          this.TextBoxAesKey.TextChanged += new TextChangedEventHandler(this.TextBoxAesKey_TextChanged);
          break;
        case 26:
          this.ButtonAesKey = (Button) target;
          this.ButtonAesKey.Click += new RoutedEventHandler(this.ButtonAesKey_Click);
          break;
        case 27:
          this.TextBoxDevEUI = (TextBox) target;
          this.TextBoxDevEUI.TextChanged += new TextChangedEventHandler(this.TextBoxDevEUI_TextChanged);
          break;
        case 28:
          this.ButtonDevEUI = (Button) target;
          this.ButtonDevEUI.Click += new RoutedEventHandler(this.ButtonDevEUI_Click);
          break;
        case 29:
          this.TextBoxJoinEUI = (TextBox) target;
          this.TextBoxJoinEUI.TextChanged += new TextChangedEventHandler(this.TextBoxAppEUI_TextChanged);
          break;
        case 30:
          this.ButtonAppEUI = (Button) target;
          this.ButtonAppEUI.Click += new RoutedEventHandler(this.ButtonAppEUI_Click);
          break;
        case 31:
          this.TextBoxAppKey = (TextBox) target;
          this.TextBoxAppKey.TextChanged += new TextChangedEventHandler(this.TextBoxAppKey_TextChanged);
          break;
        case 32:
          this.ButtonAppKey = (Button) target;
          this.ButtonAppKey.Click += new RoutedEventHandler(this.ButtonAppKey_Click);
          break;
        case 33:
          this.TextBoxMeterID = (TextBox) target;
          this.TextBoxMeterID.TextChanged += new TextChangedEventHandler(this.TextBoxMeterID_TextChanged);
          break;
        case 34:
          this.ButtonMeterID = (Button) target;
          this.ButtonMeterID.Click += new RoutedEventHandler(this.ButtonMeterID_Click);
          break;
        case 35:
          this.TextBoxHardwareTypeID = (TextBox) target;
          this.TextBoxHardwareTypeID.TextChanged += new TextChangedEventHandler(this.TextBoxHardwareTypeID_TextChanged);
          break;
        case 36:
          this.ButtonHardwareTypeID = (Button) target;
          this.ButtonHardwareTypeID.Click += new RoutedEventHandler(this.ButtonHardwareTypeID_Click);
          break;
        case 37:
          this.TextBoxMeterInfoID = (TextBox) target;
          this.TextBoxMeterInfoID.TextChanged += new TextChangedEventHandler(this.TextBoxMeterInfoID_TextChanged);
          break;
        case 38:
          this.ButtonMeterInfoID = (Button) target;
          this.ButtonMeterInfoID.Click += new RoutedEventHandler(this.ButtonMeterInfoID_Click);
          break;
        case 39:
          this.TextBoxMeterTypeID = (TextBox) target;
          this.TextBoxMeterTypeID.TextChanged += new TextChangedEventHandler(this.TextBoxMeterTypeID_TextChanged);
          break;
        case 40:
          this.ButtonMeterTypeID = (Button) target;
          this.ButtonMeterTypeID.Click += new RoutedEventHandler(this.ButtonMeterTypeID_Click);
          break;
        case 41:
          this.TextBoxBaseTypeID = (TextBox) target;
          this.TextBoxBaseTypeID.TextChanged += new TextChangedEventHandler(this.TextBoxBaseTypeID_TextChanged);
          break;
        case 42:
          this.ButtonBaseTypeID = (Button) target;
          this.ButtonBaseTypeID.Click += new RoutedEventHandler(this.ButtonBaseTypeID_Click);
          break;
        case 43:
          this.TextBoxSubPartNumber = (TextBox) target;
          this.TextBoxSubPartNumber.TextChanged += new TextChangedEventHandler(this.TextBoxSubPartNumber_TextChanged);
          break;
        case 44:
          this.ButtonSubPartNumber = (Button) target;
          this.ButtonSubPartNumber.Click += new RoutedEventHandler(this.ButtonSubPartNumber_Click);
          break;
        case 45:
          this.TextBoxSAP_MaterialNumber = (TextBox) target;
          this.TextBoxSAP_MaterialNumber.TextChanged += new TextChangedEventHandler(this.TextBoxSAP_MaterialNumber_TextChanged);
          break;
        case 46:
          this.ButtonSAP_MaterialNumber = (Button) target;
          this.ButtonSAP_MaterialNumber.Click += new RoutedEventHandler(this.ButtonSAP_MaterialNumber_Click);
          break;
        case 47:
          this.TextBoxSAP_ProductionOrderNumber = (TextBox) target;
          this.TextBoxSAP_ProductionOrderNumber.TextChanged += new TextChangedEventHandler(this.TextBoxSAP_ProductionOrderNumber_TextChanged);
          break;
        case 48:
          this.ButtonSAP_ProductionOrderNumber = (Button) target;
          this.ButtonSAP_ProductionOrderNumber.Click += new RoutedEventHandler(this.ButtonSAP_ProductionOrderNumber_Click);
          break;
        case 49:
          this.TextBoxPrintedSerialNumber = (TextBox) target;
          this.TextBoxPrintedSerialNumber.TextChanged += new TextChangedEventHandler(this.TextBoxPrintedSerialNumber_TextChanged);
          break;
        case 50:
          this.ButtonPrintedSerialNumber = (Button) target;
          this.ButtonPrintedSerialNumber.Click += new RoutedEventHandler(this.ButtonPrintedSerialNumber_Click);
          break;
        case 51:
          this.TextBoxApprovalRevision = (TextBox) target;
          this.TextBoxApprovalRevision.TextChanged += new TextChangedEventHandler(this.TextBoxApprovalRevision_TextChanged);
          break;
        case 52:
          this.ButtonApprovalRevision = (Button) target;
          this.ButtonApprovalRevision.Click += new RoutedEventHandler(this.ButtonApprovalRevision_Click);
          break;
        case 53:
          this.TextBox_FD_ID_BCD = (TextBox) target;
          break;
        case 54:
          this.TextBox_FD_Manufacturer = (TextBox) target;
          break;
        case 55:
          this.TextBox_FD_ManufacturerName = (TextBox) target;
          break;
        case 56:
          this.TextBox_FD_Generation = (TextBox) target;
          break;
        case 57:
          this.TextBox_FD_Medium = (TextBox) target;
          break;
        case 58:
          this.TextBox_FD_MediumText = (TextBox) target;
          break;
        case 59:
          this.TextBox_FD_AES_Key = (TextBox) target;
          break;
        case 60:
          this.TextBox_FD_DevEUI = (TextBox) target;
          break;
        case 61:
          this.TextBox_FD_JoinEUI = (TextBox) target;
          break;
        case 62:
          this.TextBox_FD_AppKey = (TextBox) target;
          break;
        case 63:
          this.ButtonDevEUI_FromFullSerialNumber = (Button) target;
          this.ButtonDevEUI_FromFullSerialNumber.Click += new RoutedEventHandler(this.ButtonDevEUI_FromFullSerialNumber_Click);
          break;
        case 64:
          this.ButtonSetFactoryDefaultValues = (Button) target;
          this.ButtonSetFactoryDefaultValues.Click += new RoutedEventHandler(this.ButtonSetFactoryDefaultValues_Click);
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
