// Decompiled with JetBrains decompiler
// Type: DeviceCollector.BusInfo
// Assembly: DeviceCollector, Version=2.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 9FEEAFEA-5E87-41DE-B6A2-FE832F42FF58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\DeviceCollector.dll

using NLog;
using StartupLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using ZR_ClassLibrary;

#nullable disable
namespace DeviceCollector
{
  public class BusInfo
  {
    private static Logger logger = LogManager.GetLogger(nameof (BusInfo));
    internal string BusInfoFilename;
    internal ArrayList CommParam = new ArrayList();
    internal DeviceCollectorFunctions MyBus;
    internal SortedList<DeviceCollectorSettings, object> ReadoutSettingsList;

    internal BusInfo(DeviceCollectorFunctions MyBusRef)
    {
      this.MyBus = MyBusRef;
      this.GetLastBusInfoFilename();
    }

    internal BusInfo(DeviceCollectorFunctions MyBusRef, string BusName)
    {
      this.MyBus = MyBusRef;
      this.BusInfoFilename = this.UseFilenameRules(BusName);
      this.ReadBusInfoFromFile(false);
    }

    internal BusInfo(DeviceCollectorFunctions MyBusRef, bool IgnoreError, bool useFileBusSettings)
    {
      this.MyBus = MyBusRef;
      this.BusInfoFilename = "";
      if (this.MyBus.MyDeviceList != null)
        this.MyBus.MyDeviceList.DeleteBusList();
      if (!useFileBusSettings)
        return;
      this.MyBus.SetBaseMode(BusMode.MBusPointToPoint);
      this.MyBus.AddDevice(DeviceTypes.ZR_Serie3, 0);
      if (this.GetLastBusInfoFilename() && this.ReadBusInfoFromFile(IgnoreError))
        this.SaveBusInfoFileNameForPlugIn();
    }

    internal BusInfo(DeviceCollectorFunctions MyBusRef, string BusName, bool IgnoreError)
    {
      this.MyBus = MyBusRef;
      this.BusInfoFilename = this.UseFilenameRules(BusName);
      if (!this.ReadBusInfoFromFile(IgnoreError))
        return;
      this.SaveBusInfoFileNameForPlugIn();
    }

    private bool GetLastBusInfoFilename()
    {
      this.BusInfoFilename = PlugInLoader.GmmConfiguration.GetValue("DeviceCollector", "LastBus");
      if (this.BusInfoFilename.Length == 0)
      {
        this.BusInfoFilename = this.UseFilenameRules(string.Empty);
        PlugInLoader.GmmConfiguration.SetOrUpdateValue("DeviceCollector", "LastBus", this.BusInfoFilename);
        return false;
      }
      this.BusInfoFilename = this.UseFilenameRules(this.BusInfoFilename);
      return true;
    }

    internal void SetBusinfoFilename(string Busname)
    {
      this.BusInfoFilename = this.UseFilenameRules(Busname);
    }

    internal void SaveBusInfoFileNameForPlugIn()
    {
      PlugInLoader.GmmConfiguration.SetOrUpdateValue("DeviceCollector", "LastBus", this.BusInfoFilename);
    }

    internal void SetBusinfoFilenameToDefault()
    {
      this.BusInfoFilename = this.UseFilenameRules(string.Empty);
    }

    internal string UseFilenameRules(string FilenameIn)
    {
      string path1 = "";
      string path2 = "";
      try
      {
        if (!string.IsNullOrEmpty(FilenameIn) && Directory.Exists(Path.GetPathRoot(FilenameIn)))
          path1 = Path.GetDirectoryName(FilenameIn);
      }
      catch
      {
      }
      if (path1.Length == 0)
        path1 = SystemValues.BussesPath;
      try
      {
        path2 = Path.GetFileName(FilenameIn);
      }
      catch
      {
      }
      if (path2.Length == 0)
        path2 = "DefaultBus";
      string path = Path.Combine(path1, path2);
      if (Path.GetExtension(path).Length == 0)
        path += ".bus";
      string directoryName = Path.GetDirectoryName(path);
      if (!Directory.Exists(directoryName))
        Directory.CreateDirectory(directoryName);
      return path;
    }

    internal bool SelectBusinfoOpenFilename()
    {
      if (string.IsNullOrEmpty(this.BusInfoFilename) && !this.GetLastBusInfoFilename())
        this.BusInfoFilename = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), Path.Combine("ZENNER", "GMM"));
      OpenFileDialog openFileDialog = new OpenFileDialog();
      openFileDialog.InitialDirectory = Path.GetDirectoryName(this.BusInfoFilename);
      try
      {
        openFileDialog.FileName = Path.GetFileName(this.BusInfoFilename);
      }
      catch
      {
      }
      openFileDialog.Filter = "Businfo files (*.bus)|*.bus|Default businfo files  *.defbus)|*.defbus|All files (*.*)|*.*";
      openFileDialog.FilterIndex = 1;
      openFileDialog.RestoreDirectory = true;
      openFileDialog.Title = DeviceCollectorFunctions.SerialBusMessage.GetString("ReadBusCollection");
      openFileDialog.CheckFileExists = true;
      if (openFileDialog.ShowDialog() != DialogResult.OK)
        return false;
      this.BusInfoFilename = openFileDialog.FileName;
      return true;
    }

    internal bool SelectBusinfoSaveFilename()
    {
      SaveFileDialog saveFileDialog = new SaveFileDialog();
      saveFileDialog.InitialDirectory = Path.GetDirectoryName(this.BusInfoFilename);
      try
      {
        saveFileDialog.FileName = Path.GetFileName(this.BusInfoFilename);
      }
      catch
      {
      }
      saveFileDialog.Filter = "Businfo files (*.bus)|*.bus| All files (*.*)|*.*";
      saveFileDialog.FilterIndex = 1;
      saveFileDialog.RestoreDirectory = true;
      saveFileDialog.Title = DeviceCollectorFunctions.SerialBusMessage.GetString("WriteBusCollection");
      saveFileDialog.CheckFileExists = false;
      if (saveFileDialog.ShowDialog() != DialogResult.OK)
        return false;
      this.BusInfoFilename = saveFileDialog.FileName;
      return true;
    }

    public bool ReadBusInfoFromFile(bool IgnoreError)
    {
      XmlTextReader reader = (XmlTextReader) null;
      if (!File.Exists(this.BusInfoFilename))
      {
        ZR_ClassLibMessages.AddErrorDescription("Businfo file not found!");
        BusInfo.logger.Warn(string.Format("ReadBusInfoFromFile: file {0} does not exist.", (object) this.BusInfoFilename));
      }
      else
      {
        try
        {
          reader = new XmlTextReader(this.BusInfoFilename);
          while (reader.Read())
          {
            if (reader.NodeType == XmlNodeType.Element)
            {
              if (reader.Name == "Communication")
              {
                this.CommParam.Clear();
                while (reader.MoveToNextAttribute())
                {
                  this.CommParam.Add((object) reader.Name);
                  this.CommParam.Add((object) reader.Value);
                }
              }
              else if (reader.Name == "SerialBusSettings")
              {
                string attribute = reader.GetAttribute("BusMode");
                try
                {
                  this.MyBus.SetBaseMode((BusMode) Enum.Parse(typeof (BusMode), attribute, true));
                }
                catch (ArgumentException ex)
                {
                  ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.InternalError, "Unknown bus mode: " + attribute + "Please check the BusInfo file!");
                }
                this.TryLoadReadoutSettings(reader);
              }
              else if (reader.Name == "Device")
              {
                SortedList<string, string> DeviceParameter = new SortedList<string, string>();
                for (int i = 0; i < reader.AttributeCount; ++i)
                {
                  reader.MoveToAttribute(i);
                  string str = reader.Value;
                  string name = reader.Name;
                  if (name.Length > 0)
                    DeviceParameter.Add(name, str);
                }
                if (this.MyBus.MyBusMode != BusMode.MBus)
                  this.MyBus.MyDeviceList.DeleteBusList();
                this.AddDeviceToDeviceList(DeviceParameter);
              }
            }
          }
        }
        catch (Exception ex)
        {
          BusInfo.logger.Error(string.Format("ReadBusInfoFromFile: catched exception {0}.", (object) ex.Message));
          goto label_29;
        }
        finally
        {
          reader?.Close();
        }
        BusInfo.logger.Info(string.Format("ReadBusInfoFromFile: loaded file {0} successfully.", (object) this.BusInfoFilename));
        return true;
      }
label_29:
      ZR_ClassLibMessages.AddErrorDescription(ZR_ClassLibMessages.LastErrors.MissingData, "Error on read businfo");
      this.MyBus.DeleteBusInfo();
      return false;
    }

    private bool TryLoadReadoutSettings(XmlTextReader reader)
    {
      if (this.ReadoutSettingsList == null)
        this.ReadoutSettingsList = new SortedList<DeviceCollectorSettings, object>();
      else
        this.ReadoutSettingsList.Clear();
      try
      {
        foreach (string name in ZR_ClassLibrary.Util.GetNamesOfEnum(typeof (DeviceCollectorSettings)))
        {
          DeviceCollectorSettings key = (DeviceCollectorSettings) Enum.Parse(typeof (DeviceCollectorSettings), name, true);
          string attribute = reader.GetAttribute(name);
          if (attribute != null)
            this.ReadoutSettingsList.Add(key, (object) attribute);
        }
      }
      catch (Exception ex)
      {
        BusInfo.logger.Error(string.Format("TryLoadReadoutSettings: catched exception {0}.", (object) ex.Message));
        return false;
      }
      return true;
    }

    private void AddDeviceToDeviceList(SortedList<string, string> DeviceParameter)
    {
      if (this.MyBus.MyDeviceList == null)
        return;
      int index1 = DeviceParameter.IndexOfKey("Selected");
      bool flag1 = index1 >= 0 && bool.Parse(DeviceParameter.Values[index1]);
      int index2 = DeviceParameter.IndexOfKey("Address");
      byte num1 = index2 < 0 ? (byte) 0 : byte.Parse(DeviceParameter.Values[index2]);
      int index3 = DeviceParameter.IndexOfKey("AddressOk");
      bool flag2 = index3 >= 0 && bool.Parse(DeviceParameter.Values[index3]);
      int index4 = DeviceParameter.IndexOfKey("AddressKnown");
      bool flag3 = index4 >= 0 && bool.Parse(DeviceParameter.Values[index4]);
      int index5 = DeviceParameter.IndexOfKey("IdNo");
      string str1 = index5 < 0 ? string.Empty : DeviceParameter.Values[index5].Trim();
      int index6 = DeviceParameter.IndexOfKey("Manufacturer");
      short ManufacturerCode = index6 < 0 ? (short) 0 : short.Parse(DeviceParameter.Values[index6]);
      int index7 = DeviceParameter.IndexOfKey("Generation");
      byte num2 = index7 < 0 ? (byte) 0 : byte.Parse(DeviceParameter.Values[index7]);
      int index8 = DeviceParameter.IndexOfKey("Medium");
      byte num3 = index8 < 0 ? (byte) 0 : byte.Parse(DeviceParameter.Values[index8]);
      int index9 = DeviceParameter.IndexOfKey("DeviceInfoText");
      string str2 = index9 < 0 ? string.Empty : DeviceParameter.Values[index9];
      try
      {
        string str3 = DeviceParameter["Type"];
        if (str3 == "unknown" || !Enum.IsDefined(typeof (DeviceTypes), (object) str3))
          return;
        this.MyBus.MyDeviceList.AddDevice((DeviceTypes) Enum.Parse(typeof (DeviceTypes), str3, true), true);
        object bu = this.MyBus.MyDeviceList.bus[this.MyBus.MyDeviceList.bus.Count - 1];
        if (bu is RadioDevice)
        {
          this.MyBus.MyDeviceList.bus.RemoveAt(this.MyBus.MyDeviceList.bus.Count - 1);
          if (DeviceParameter == null || !DeviceParameter.ContainsKey("FunkId") || !DeviceParameter.ContainsKey("LastSeen") || !DeviceParameter.ContainsKey("Packets"))
            return;
          RadioList deviceList = (RadioList) this.MyBus.MyDeviceList;
          Convert.ToInt64(DeviceParameter["FunkId"], (IFormatProvider) CultureInfo.InvariantCulture);
          DeviceTypes deviceTypes = (DeviceTypes) Enum.Parse(typeof (DeviceTypes), str3, true);
          DateTime receivedAt = DateTime.Parse(DeviceParameter["LastSeen"], (IFormatProvider) FixedFormates.TheFormates.DateTimeFormat);
          string str4 = DeviceParameter["Packets"];
          char[] chArray = new char[1]{ ',' };
          foreach (string hex in str4.Split(chArray))
          {
            if (!string.IsNullOrEmpty(hex))
            {
              byte[] byteArray = ZR_ClassLibrary.Util.HexStringToByteArray(hex);
              RadioPacket packet;
              if (this.MyBus.MyBusMode == BusMode.Radio2)
                packet = (RadioPacket) new RadioPacketRadio2();
              else if (this.MyBus.MyBusMode == BusMode.Radio3 || this.MyBus.MyBusMode == BusMode.Radio3_868_95_RUSSIA || this.MyBus.MyBusMode == BusMode.Radio4)
                packet = (RadioPacket) new RadioPacketRadio3();
              else if (this.MyBus.MyBusMode == BusMode.wMBusC1A || this.MyBus.MyBusMode == BusMode.wMBusC1B || this.MyBus.MyBusMode == BusMode.wMBusS1 || this.MyBus.MyBusMode == BusMode.wMBusS1M || this.MyBus.MyBusMode == BusMode.wMBusS2 || this.MyBus.MyBusMode == BusMode.wMBusT1 || this.MyBus.MyBusMode == BusMode.wMBusT2_meter || this.MyBus.MyBusMode == BusMode.wMBusT2_other)
              {
                packet = (RadioPacket) new RadioPacketWirelessMBus();
              }
              else
              {
                BusInfo.logger.Error("Invalid BusMode where adding the WalkBy devices.");
                continue;
              }
              packet.MyFunctions = this.MyBus;
              if (packet.Parse(byteArray, receivedAt, false))
              {
                deviceList.AddPacket(packet);
                deviceList.AddRadioDevice(packet);
              }
            }
          }
        }
        else if (bu is MBusDevice)
        {
          MBusDevice selectedDevice = this.MyBus.MyDeviceList.SelectedDevice as MBusDevice;
          selectedDevice.PrimaryAddressOk = flag2;
          selectedDevice.PrimaryAddressKnown = flag3;
          selectedDevice.PrimaryDeviceAddress = num1;
          if (str1.Length > 0)
          {
            selectedDevice.Info.MeterNumber = str1;
            selectedDevice.Info.ManufacturerCode = ManufacturerCode;
            selectedDevice.Info.Manufacturer = MBusDevice.GetManufacturer(ManufacturerCode);
            selectedDevice.Info.Version = num2;
            selectedDevice.Info.Medium = num3;
          }
          selectedDevice.DeviceInfoText = str2;
          this.MyBus.MyDeviceList.WorkBusAddresses();
        }
        else if (this.MyBus.MyBusMode == BusMode.WaveFlowRadio)
        {
          WaveFlowDevice selectedDevice = this.MyBus.MyDeviceList.SelectedDevice as WaveFlowDevice;
          if (str1.Length > 0)
          {
            DeviceInfo deviceInfo = new DeviceInfo();
            selectedDevice.Info = deviceInfo;
            selectedDevice.Info.MeterNumber = str1;
            selectedDevice.Info.ManufacturerCode = ManufacturerCode;
            selectedDevice.Info.Manufacturer = MBusDevice.GetManufacturer(ManufacturerCode);
            selectedDevice.Info.Version = num2;
            selectedDevice.Info.Medium = num3;
          }
        }
      }
      catch (Exception ex)
      {
        string message = string.Format("Error: {0}, Trace: {1}", (object) ex.Message, (object) ex.StackTrace);
        BusInfo.logger.Error(ex, message);
      }
    }

    public bool WriteBusInfoToFile()
    {
      XmlTextWriter myXmlTextWriter = (XmlTextWriter) null;
      try
      {
        this.MyBus.MyCom.GetCommParameter(ref this.MyBus.MyBusInfo.CommParam);
        myXmlTextWriter = new XmlTextWriter(this.BusInfoFilename, (Encoding) null);
        myXmlTextWriter.Formatting = Formatting.Indented;
        myXmlTextWriter.WriteStartDocument(false);
        myXmlTextWriter.WriteComment("Businfo für Zenner Global Meter Manager");
        myXmlTextWriter.WriteStartElement("Businfo");
        myXmlTextWriter.WriteComment("Definition der Schnittstelle");
        myXmlTextWriter.WriteStartElement("Communication", (string) null);
        for (int index = 0; index < this.CommParam.Count; index += 2)
          myXmlTextWriter.WriteAttributeString((string) this.CommParam[index], (string) this.CommParam[index + 1]);
        myXmlTextWriter.WriteEndElement();
        myXmlTextWriter.WriteComment("DeviceCollector base settings");
        myXmlTextWriter.WriteStartElement("SerialBusSettings", (string) null);
        myXmlTextWriter.WriteAttributeString(DeviceCollectorSettings.BusMode.ToString(), this.MyBus.MyBusMode.ToString());
        myXmlTextWriter.WriteAttributeString(DeviceCollectorSettings.DaKonId.ToString(), this.MyBus.DaKonId.ToString());
        XmlTextWriter xmlTextWriter1 = myXmlTextWriter;
        DeviceCollectorSettings collectorSettings = DeviceCollectorSettings.FromTime;
        string localName1 = collectorSettings.ToString();
        string str1 = this.MyBus.ReadFromTime.ToString();
        xmlTextWriter1.WriteAttributeString(localName1, str1);
        XmlTextWriter xmlTextWriter2 = myXmlTextWriter;
        collectorSettings = DeviceCollectorSettings.ToTime;
        string localName2 = collectorSettings.ToString();
        string str2 = this.MyBus.ReadToTime.ToString();
        xmlTextWriter2.WriteAttributeString(localName2, str2);
        XmlTextWriter xmlTextWriter3 = myXmlTextWriter;
        collectorSettings = DeviceCollectorSettings.Password;
        string localName3 = collectorSettings.ToString();
        string str3 = this.MyBus.Password.ToString();
        xmlTextWriter3.WriteAttributeString(localName3, str3);
        XmlTextWriter xmlTextWriter4 = myXmlTextWriter;
        collectorSettings = DeviceCollectorSettings.MaxRequestRepeat;
        string localName4 = collectorSettings.ToString();
        string str4 = this.MyBus.MaxRequestRepeat.ToString();
        xmlTextWriter4.WriteAttributeString(localName4, str4);
        XmlTextWriter xmlTextWriter5 = myXmlTextWriter;
        collectorSettings = DeviceCollectorSettings.ScanStartAddress;
        string localName5 = collectorSettings.ToString();
        string str5 = this.MyBus.ScanStartAddress.ToString();
        xmlTextWriter5.WriteAttributeString(localName5, str5);
        XmlTextWriter xmlTextWriter6 = myXmlTextWriter;
        collectorSettings = DeviceCollectorSettings.ScanStartSerialnumber;
        string localName6 = collectorSettings.ToString();
        string str6 = this.MyBus.ScanStartSerialnumber.ToString();
        xmlTextWriter6.WriteAttributeString(localName6, str6);
        XmlTextWriter xmlTextWriter7 = myXmlTextWriter;
        collectorSettings = DeviceCollectorSettings.OrganizeStartAddress;
        string localName7 = collectorSettings.ToString();
        string str7 = this.MyBus.OrganizeStartAddress.ToString();
        xmlTextWriter7.WriteAttributeString(localName7, str7);
        XmlTextWriter xmlTextWriter8 = myXmlTextWriter;
        collectorSettings = DeviceCollectorSettings.CycleTime;
        string localName8 = collectorSettings.ToString();
        string str8 = this.MyBus.CycleTime.ToString();
        xmlTextWriter8.WriteAttributeString(localName8, str8);
        XmlTextWriter xmlTextWriter9 = myXmlTextWriter;
        collectorSettings = DeviceCollectorSettings.OnlySecondaryAddressing;
        string localName9 = collectorSettings.ToString();
        string str9 = this.MyBus.OnlySecondaryAddressing.ToString();
        xmlTextWriter9.WriteAttributeString(localName9, str9);
        XmlTextWriter xmlTextWriter10 = myXmlTextWriter;
        collectorSettings = DeviceCollectorSettings.FastSecondaryAddressing;
        string localName10 = collectorSettings.ToString();
        string str10 = this.MyBus.FastSecondaryAddressing.ToString();
        xmlTextWriter10.WriteAttributeString(localName10, str10);
        XmlTextWriter xmlTextWriter11 = myXmlTextWriter;
        collectorSettings = DeviceCollectorSettings.KeepExistingDestinationAddress;
        string localName11 = collectorSettings.ToString();
        string str11 = this.MyBus.KeepExistingDestinationAddress.ToString();
        xmlTextWriter11.WriteAttributeString(localName11, str11);
        XmlTextWriter xmlTextWriter12 = myXmlTextWriter;
        collectorSettings = DeviceCollectorSettings.ChangeInterfaceBaudrateToo;
        string localName12 = collectorSettings.ToString();
        string str12 = this.MyBus.ChangeInterfaceBaudrateToo.ToString();
        xmlTextWriter12.WriteAttributeString(localName12, str12);
        XmlTextWriter xmlTextWriter13 = myXmlTextWriter;
        collectorSettings = DeviceCollectorSettings.UseExternalKeyForReading;
        string localName13 = collectorSettings.ToString();
        string str13 = this.MyBus.UseExternalKeyForReading.ToString();
        xmlTextWriter13.WriteAttributeString(localName13, str13);
        XmlTextWriter xmlTextWriter14 = myXmlTextWriter;
        collectorSettings = DeviceCollectorSettings.BeepSignalOnReadResult;
        string localName14 = collectorSettings.ToString();
        string str14 = this.MyBus.BeepSignalForReadResult.ToString();
        xmlTextWriter14.WriteAttributeString(localName14, str14);
        XmlTextWriter xmlTextWriter15 = myXmlTextWriter;
        collectorSettings = DeviceCollectorSettings.LogToFileEnabled;
        string localName15 = collectorSettings.ToString();
        string str15 = this.MyBus.LogToFileEnabled.ToString();
        xmlTextWriter15.WriteAttributeString(localName15, str15);
        XmlTextWriter xmlTextWriter16 = myXmlTextWriter;
        collectorSettings = DeviceCollectorSettings.LogFilePath;
        string localName16 = collectorSettings.ToString();
        string str16 = this.MyBus.LogFilePath.ToString();
        xmlTextWriter16.WriteAttributeString(localName16, str16);
        XmlTextWriter xmlTextWriter17 = myXmlTextWriter;
        collectorSettings = DeviceCollectorSettings.IsMultiTelegrammEnabled;
        string localName17 = collectorSettings.ToString();
        string str17 = this.MyBus.IsMultiTelegrammEnabled.ToString();
        xmlTextWriter17.WriteAttributeString(localName17, str17);
        XmlTextWriter xmlTextWriter18 = myXmlTextWriter;
        collectorSettings = DeviceCollectorSettings.UseREQ_UD2_5B;
        string localName18 = collectorSettings.ToString();
        string str18 = this.MyBus.UseREQ_UD2_5B.ToString();
        xmlTextWriter18.WriteAttributeString(localName18, str18);
        XmlTextWriter xmlTextWriter19 = myXmlTextWriter;
        collectorSettings = DeviceCollectorSettings.SendFirstApplicationReset;
        string localName19 = collectorSettings.ToString();
        string str19 = this.MyBus.SendFirstApplicationReset.ToString();
        xmlTextWriter19.WriteAttributeString(localName19, str19);
        XmlTextWriter xmlTextWriter20 = myXmlTextWriter;
        collectorSettings = DeviceCollectorSettings.SendFirstSND_NKE;
        string localName20 = collectorSettings.ToString();
        string str20 = this.MyBus.SendFirstSND_NKE.ToString();
        xmlTextWriter20.WriteAttributeString(localName20, str20);
        myXmlTextWriter.WriteEndElement();
        if (this.MyBus.MyDeviceList != null)
        {
          myXmlTextWriter.WriteComment("Liste der Geräte");
          for (int index = 0; index < this.MyBus.MyDeviceList.bus.Count; ++index)
          {
            myXmlTextWriter.WriteStartElement("Device", (string) null);
            myXmlTextWriter.WriteAttributeString("Type", ((BusDevice) this.MyBus.MyDeviceList.bus[index]).DeviceType.ToString());
            bool flag;
            if (this.MyBus.MyDeviceList.bus[index] == this.MyBus.MyDeviceList.SelectedDevice)
            {
              XmlTextWriter xmlTextWriter21 = myXmlTextWriter;
              flag = true;
              string str21 = flag.ToString();
              xmlTextWriter21.WriteAttributeString("Selected", str21);
            }
            else
            {
              XmlTextWriter xmlTextWriter22 = myXmlTextWriter;
              flag = false;
              string str22 = flag.ToString();
              xmlTextWriter22.WriteAttributeString("Selected", str22);
            }
            if (this.MyBus.MyDeviceList.bus[index] is RadioDevice)
            {
              Dictionary<long, RadioDataSet> receivedData = ((RadioList) this.MyBus.MyDeviceList).ReceivedData;
              RadioDevice bu = (RadioDevice) this.MyBus.MyDeviceList.bus[index];
              if (receivedData.Count > 0 && bu.Info != null && bu.Device != null)
              {
                myXmlTextWriter.WriteAttributeString("FunkId", bu.Device.FunkId.ToString());
                myXmlTextWriter.WriteAttributeString("LastSeen", bu.Info.LastReadingDate.ToString(FixedFormates.TheFormates.DateTimeFormat.FullDateTimePattern));
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(ZR_ClassLibrary.Util.ByteArrayToHexString(receivedData[bu.Device.FunkId].LastRadioPacket.Buffer)).Append(',');
                myXmlTextWriter.WriteAttributeString("Packets", stringBuilder.ToString());
              }
            }
            else if (this.MyBus.MyDeviceList.bus[index] is MBusDevice)
            {
              MBusDevice bu = this.MyBus.MyDeviceList.bus[index] as MBusDevice;
              myXmlTextWriter.WriteAttributeString("Address", bu.PrimaryDeviceAddress.ToString());
              myXmlTextWriter.WriteAttributeString("AddressOk", bu.PrimaryAddressOk.ToString());
              myXmlTextWriter.WriteAttributeString("AddressKnown", bu.PrimaryAddressKnown.ToString());
              if (bu.Info != null)
              {
                myXmlTextWriter.WriteAttributeString("IdNo", bu.Info.MeterNumber);
                myXmlTextWriter.WriteAttributeString("Manufacturer", bu.Info.ManufacturerCode.ToString());
                myXmlTextWriter.WriteAttributeString("Generation", bu.Info.Version.ToString());
                myXmlTextWriter.WriteAttributeString("Medium", bu.Info.Medium.ToString());
              }
              if (bu.TableDataRow != null && bu.TableDataRow.Table.Columns["DeviceInfoText"] != null)
              {
                int ordinal = bu.TableDataRow.Table.Columns["DeviceInfoText"].Ordinal;
                if (bu.TableDataRow != null && ordinal != -1)
                  myXmlTextWriter.WriteAttributeString("DeviceInfoText", bu.TableDataRow[ordinal].ToString());
              }
            }
            else if (this.MyBus.MyBusMode == BusMode.WaveFlowRadio)
              this.WriteWaveFlowRadioToFile(myXmlTextWriter, index);
            myXmlTextWriter.WriteEndElement();
          }
          myXmlTextWriter.WriteEndElement();
        }
      }
      catch (Exception ex)
      {
        BusInfo.logger.Error(string.Format("WriteBusInfoToFile: catched exception {0}.", (object) ex.Message));
        return false;
      }
      finally
      {
        if (myXmlTextWriter != null)
        {
          myXmlTextWriter.Flush();
          myXmlTextWriter.Close();
        }
      }
      BusInfo.logger.Info(string.Format("WriteBusInfoToFile: wrote file {0} successfully.", (object) this.BusInfoFilename));
      return true;
    }

    private void WriteWaveFlowRadioToFile(XmlTextWriter myXmlTextWriter, int i)
    {
      if (this.MyBus.MyDeviceList != null)
        return;
      WaveFlowDevice bu = this.MyBus.MyDeviceList.bus[i] as WaveFlowDevice;
      if (bu.Info == null)
        return;
      myXmlTextWriter.WriteAttributeString("IdNo", bu.Info.MeterNumber);
      myXmlTextWriter.WriteAttributeString("Manufacturer", bu.Info.ManufacturerCode.ToString());
      myXmlTextWriter.WriteAttributeString("Generation", bu.Info.Version.ToString());
      myXmlTextWriter.WriteAttributeString("Medium", bu.Info.Medium.ToString());
    }

    private void SetDefaultCommParameter()
    {
      if (this.MyBus.MyCom == null)
        return;
      this.MyBus.MyCom.GetCommParameter(ref this.CommParam);
    }

    internal string GetBusSettings(AsyncComSettings key)
    {
      this.MyBus.MyCom.GetCommParameter(ref this.MyBus.MyBusInfo.CommParam);
      for (int index = 0; index < this.MyBus.MyBusInfo.CommParam.Count; index += 2)
      {
        string str = (string) this.MyBus.MyBusInfo.CommParam[index];
        if (key.ToString() == str)
          return (string) this.MyBus.MyBusInfo.CommParam[index + 1];
      }
      return string.Empty;
    }

    internal SortedList<string, string> GetBusSettings()
    {
      SortedList<string, string> busSettings = new SortedList<string, string>();
      busSettings.Add("BusMode", this.MyBus.MyBusMode.ToString());
      if (this.MyBus.MyDeviceList != null && this.MyBus.MyDeviceList.bus != null && this.MyBus.MyDeviceList.bus.Count > 0)
      {
        StringBuilder DeviceData = new StringBuilder();
        for (int index = 0; index < this.MyBus.MyDeviceList.bus.Count; ++index)
        {
          DeviceData.Length = 0;
          DeviceData.Append("Type:" + ((BusDevice) this.MyBus.MyDeviceList.bus[index]).DeviceType.ToString());
          DeviceData.Append("|Selected:");
          bool flag;
          if (this.MyBus.MyDeviceList.bus[index] == this.MyBus.MyDeviceList.SelectedDevice)
          {
            StringBuilder stringBuilder = DeviceData;
            flag = true;
            string str = flag.ToString();
            stringBuilder.Append(str);
          }
          else
          {
            StringBuilder stringBuilder = DeviceData;
            flag = false;
            string str = flag.ToString();
            stringBuilder.Append(str);
          }
          if (!(this.MyBus.MyDeviceList.bus[index] is MinolDevice))
          {
            if (this.MyBus.MyDeviceList.bus[index] is MBusDevice)
            {
              MBusDevice bu = this.MyBus.MyDeviceList.bus[index] as MBusDevice;
              DeviceData.Append("|Address:" + bu.PrimaryDeviceAddress.ToString());
              DeviceData.Append("|AddressOk:" + bu.PrimaryAddressOk.ToString());
              DeviceData.Append("|AddressKnown:" + bu.PrimaryAddressKnown.ToString());
              if (bu.Info != null)
              {
                DeviceData.Append("|IdNo:" + bu.Info.MeterNumber);
                DeviceData.Append("|Manufacturer:" + bu.Info.ManufacturerCode.ToString());
                DeviceData.Append("|Generation:" + bu.Info.Version.ToString());
                DeviceData.Append("|Medium:" + bu.Info.Medium.ToString());
              }
            }
            else if (this.MyBus.MyDeviceList.MyBus.MyBusMode == BusMode.WaveFlowRadio)
              this.AddWaveFlowInfo(DeviceData, index);
          }
          busSettings.Add("Device" + index.ToString(), DeviceData.ToString());
        }
      }
      this.MyBus.MyCom.GetCommParameter(ref this.MyBus.MyBusInfo.CommParam);
      for (int index = 0; index < this.MyBus.MyBusInfo.CommParam.Count; index += 2)
      {
        string key = (string) this.MyBus.MyBusInfo.CommParam[index];
        if (!busSettings.ContainsKey(key))
          busSettings.Add(key, (string) this.MyBus.MyBusInfo.CommParam[index + 1]);
      }
      return busSettings;
    }

    private void AddWaveFlowInfo(StringBuilder DeviceData, int i)
    {
      if (this.MyBus.MyDeviceList != null)
        return;
      WaveFlowDevice bu = this.MyBus.MyDeviceList.bus[i] as WaveFlowDevice;
      if (bu.Info == null)
        return;
      DeviceData.Append("|IdNo:" + bu.Info.MeterNumber);
      DeviceData.Append("|Manufacturer:" + bu.Info.ManufacturerCode.ToString());
      DeviceData.Append("|Generation:" + bu.Info.Version.ToString());
      DeviceData.Append("|Medium:" + bu.Info.Medium.ToString());
    }

    internal void SetAsyncComSettings(SortedList<string, string> settings)
    {
      if (this.MyBus.MyDeviceList != null)
        this.MyBus.MyDeviceList.DeleteBusList();
      if (settings.ContainsKey(AsyncComSettings.Port.ToString()))
      {
        SortedList<string, string> busSettings = this.GetBusSettings();
        if (busSettings != null && busSettings.ContainsKey(AsyncComSettings.Port.ToString()) && busSettings[AsyncComSettings.Port.ToString()] != settings[AsyncComSettings.Port.ToString()])
          this.MyBus.ComClose();
      }
      ArrayList ParameterList = new ArrayList();
      for (int index = 0; index < settings.Count; ++index)
      {
        string key = settings.Keys[index];
        ParameterList.Add((object) key);
        ParameterList.Add((object) settings.Values[index]);
      }
      this.MyBus.MyCom.SetCommParameter(ParameterList);
    }

    internal void SetAsyncComSettings(SortedList<AsyncComSettings, object> settings)
    {
      SortedList<string, string> busSettings = this.GetBusSettings();
      if (this.MyBus.MyDeviceList != null)
        this.MyBus.MyDeviceList.DeleteBusList();
      AsyncComSettings asyncComSettings;
      if (settings.ContainsKey(AsyncComSettings.Port))
      {
        SortedList<string, string> sortedList = busSettings;
        asyncComSettings = AsyncComSettings.Port;
        string key = asyncComSettings.ToString();
        if (sortedList[key].ToString() != settings[AsyncComSettings.Port].ToString())
          this.MyBus.ComClose();
      }
      ArrayList ParameterList = new ArrayList();
      for (int index = 0; index < settings.Count; ++index)
      {
        asyncComSettings = settings.Keys[index];
        string str = asyncComSettings.ToString();
        ParameterList.Add((object) str);
        ParameterList.Add(settings.Values[index]);
      }
      this.MyBus.MyCom.SetCommParameter(ParameterList);
    }
  }
}
