// Decompiled with JetBrains decompiler
// Type: ZENNER.CommonLibrary.ConfigList
// Assembly: CommonLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 53447886-5C7B-49AE-B18C-3692A1E343CC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\CommonLibrary.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using ZENNER.CommonLibrary.Entities;

#nullable disable
namespace ZENNER.CommonLibrary
{
  [Serializable]
  public class ConfigList : 
    INotifyPropertyChanged,
    IEnumerable<KeyValuePair<string, string>>,
    IEnumerable,
    INotifyCollectionChanged
  {
    private static readonly object lockObj = new object();
    private SortedList<string, string> configList;
    private string readingChannelIdentification = "Common";

    public event PropertyChangedEventHandler PropertyChanged;

    public event NotifyCollectionChangedEventHandler CollectionChanged;

    public ConfigList() => this.configList = new SortedList<string, string>();

    public ConfigList(SortedList<string, string> configList)
      : this()
    {
      this.Reset(configList);
    }

    public string this[string key]
    {
      get => this.Get<string>(key);
      set => this.Set(key, value);
    }

    public bool ContainsKey(string key) => this.configList.ContainsKey(key);

    public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
    {
      return this.configList.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

    public static void Save(string path, ConfigList configList)
    {
      if (string.IsNullOrWhiteSpace(path))
        throw new ArgumentNullException(nameof (path));
      if (configList == null)
        throw new ArgumentNullException(nameof (configList));
      List<string> list = configList.Select<KeyValuePair<string, string>, string>((Func<KeyValuePair<string, string>, string>) (x => x.Key + "=" + x.Value)).ToList<string>();
      File.WriteAllLines(path, (IEnumerable<string>) list);
    }

    public static ConfigList Load(string path)
    {
      if (string.IsNullOrWhiteSpace(path))
        throw new ArgumentNullException(nameof (path));
      return new ConfigList(new SortedList<string, string>((IDictionary<string, string>) ((IEnumerable<string>) File.ReadAllLines(path)).ToDictionary<string, string, string>((Func<string, string>) (x => x.Split('=')[0]), (Func<string, string>) (x => x.Split('=')[1]))));
    }

    public void Reset(ConfigList newConfigList) => this.Reset(newConfigList.GetSortedList());

    public void Reset(SortedList<string, string> newConfigList)
    {
      this.configList = ConfigList.DeepCopy(newConfigList);
      foreach (string name in Enum.GetNames(typeof (ParameterKey)))
      {
        bool flag = newConfigList != null && newConfigList.ContainsKey(name);
        BrowsableAttribute attribute = (BrowsableAttribute) (TypeDescriptor.GetProperties(this.GetType())[name] ?? throw new NotImplementedException(name)).Attributes[typeof (BrowsableAttribute)];
        attribute.GetType().GetField("browsable", BindingFlags.Instance | BindingFlags.NonPublic).SetValue((object) attribute, (object) flag);
      }
      this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
    }

    public SortedList<string, string> GetSortedList() => ConfigList.DeepCopy(this.configList);

    public string Name
    {
      get
      {
        StringBuilder stringBuilder = new StringBuilder();
        if (this.configList == null)
        {
          stringBuilder.Append("NotInitialised");
        }
        else
        {
          string str1 = "NoBusMode";
          SortedList<string, string> configList1 = this.configList;
          ParameterKey parameterKey = ParameterKey.BusMode;
          string key1 = parameterKey.ToString();
          int index1 = configList1.IndexOfKey(key1);
          if (index1 >= 0 && !string.IsNullOrEmpty(this.configList.Values[index1]))
            str1 = this.configList.Values[index1];
          stringBuilder.Append(str1);
          int index2 = this.configList.IndexOfKey("ID");
          if (index2 >= 0)
            stringBuilder.Append("_" + this.configList.Values[index2]);
          SortedList<string, string> configList2 = this.configList;
          parameterKey = ParameterKey.Baudrate;
          string key2 = parameterKey.ToString();
          int index3 = configList2.IndexOfKey(key2);
          if (index3 >= 0)
            stringBuilder.Append("_" + this.configList.Values[index3]);
          SortedList<string, string> configList3 = this.configList;
          parameterKey = ParameterKey.UseBreak;
          string key3 = parameterKey.ToString();
          int index4 = configList3.IndexOfKey(key3);
          if (index4 >= 0)
          {
            string str2 = this.configList.Values[index4];
            if (str2 != "None")
              stringBuilder.Append("_" + str2);
          }
          SortedList<string, string> configList4 = this.configList;
          parameterKey = ParameterKey.Wakeup;
          string key4 = parameterKey.ToString();
          int index5 = configList4.IndexOfKey(key4);
          if (index5 >= 0)
          {
            string str3 = this.configList.Values[index5];
            if (str3 != "None")
              stringBuilder.Append("_" + str3);
          }
          SortedList<string, string> configList5 = this.configList;
          parameterKey = ParameterKey.IrDaSelection;
          string key5 = parameterKey.ToString();
          int index6 = configList5.IndexOfKey(key5);
          if (index6 >= 0)
          {
            string str4 = this.configList.Values[index6];
            if (str4 != "None")
              stringBuilder.Append("_" + str4);
          }
          SortedList<string, string> configList6 = this.configList;
          parameterKey = ParameterKey.CombiHeadSelection;
          string key6 = parameterKey.ToString();
          int index7 = configList6.IndexOfKey(key6);
          if (index7 >= 0)
          {
            string str5 = this.configList.Values[index7];
            if (str5 != "None")
              stringBuilder.Append("_" + str5);
          }
          SortedList<string, string> configList7 = this.configList;
          parameterKey = ParameterKey.SelectedDeviceMBusType;
          string key7 = parameterKey.ToString();
          int index8 = configList7.IndexOfKey(key7);
          if (index8 >= 0)
          {
            string str6 = this.configList.Values[index8];
            stringBuilder.Append("_" + str6);
          }
          SortedList<string, string> configList8 = this.configList;
          parameterKey = ParameterKey.TransceiverType;
          string key8 = parameterKey.ToString();
          int index9 = configList8.IndexOfKey(key8);
          if (index9 >= 0)
          {
            string str7 = this.configList.Values[index9];
            stringBuilder.Append("_tt:" + str7);
          }
        }
        if (stringBuilder.Length == 0)
          stringBuilder.Append("NoParameters");
        return stringBuilder.ToString();
      }
    }

    [Browsable(true)]
    [ReadOnly(true)]
    [Category("ID")]
    public int ConnectionProfileID
    {
      get => this.Get<int>(ParameterKey.ConnectionProfileID);
      set => this.Set(ParameterKey.ConnectionProfileID, value);
    }

    [Browsable(true)]
    [Category("Communication")]
    [TypeConverter(typeof (ConfigList.GmmSettingsConverter))]
    [Description("Connection type.")]
    public string Type
    {
      get => this.Get<string>(ParameterKey.Type);
      set => this.Set(ParameterKey.Type, value);
    }

    [Browsable(true)]
    [DefaultValue("COM1")]
    [Category("Communication")]
    [TypeConverter(typeof (ConfigList.GmmSettingsConverter))]
    [Description("COM port number.")]
    public string Port
    {
      get => this.Get<string>(ParameterKey.Port);
      set => this.Set(ParameterKey.Port, value);
    }

    [Browsable(true)]
    [Category("Communication")]
    [TypeConverter(typeof (ConfigList.GmmSettingsConverter))]
    [Description("Parity")]
    public string Parity
    {
      get => this.Get<string>(ParameterKey.Parity);
      set => this.Set(ParameterKey.Parity, value);
    }

    [Browsable(true)]
    [Category("Communication")]
    [TypeConverter(typeof (ConfigList.GmmSettingsConverter))]
    [Description("THe Transceiver device used by communication.")]
    public string TransceiverDevice
    {
      get => this.Get<string>(ParameterKey.TransceiverDevice);
      set => this.Set(ParameterKey.TransceiverDevice, value);
    }

    [Browsable(true)]
    [Category("Communication")]
    [TypeConverter(typeof (ConfigList.GmmSettingsConverter))]
    [Description("A unit used to measure the speed of data transfer, equal to the number of bits per second.")]
    public int Baudrate
    {
      get => this.Get<int>(ParameterKey.Baudrate);
      set => this.Set(ParameterKey.Baudrate, value);
    }

    [Browsable(true)]
    [DefaultValue(3)]
    [Category("Communication")]
    [Description("Number of repeats used by communication between master and slave.")]
    public int MaxRequestRepeat
    {
      get => this.Get<int>(ParameterKey.MaxRequestRepeat);
      set => this.Set(ParameterKey.MaxRequestRepeat, value);
    }

    [Browsable(true)]
    [DefaultValue(3600)]
    [Category("Communication")]
    [Description("")]
    public int MinoConnectPowerOffTime
    {
      get => this.Get<int>(ParameterKey.MinoConnectPowerOffTime);
      set => this.Set(ParameterKey.MinoConnectPowerOffTime, value);
    }

    [Browsable(true)]
    [Category("Communication")]
    [Description("")]
    public int MinoConnectIrDaPulseTime
    {
      get => this.Get<int>(ParameterKey.MinoConnectIrDaPulseTime);
      set => this.Set(ParameterKey.MinoConnectIrDaPulseTime, value);
    }

    [Browsable(true)]
    [DefaultValue(5)]
    [Category("Communication")]
    [Description("Cycle time used by readout in seconds.")]
    public int CycleTime
    {
      get => this.Get<int>(ParameterKey.CycleTime);
      set => this.Set(ParameterKey.CycleTime, value);
    }

    [Browsable(true)]
    [DefaultValue(false)]
    [Category("Communication")]
    [Description("")]
    public bool EchoOn
    {
      get => this.Get<bool>(ParameterKey.EchoOn);
      set => this.Set(ParameterKey.EchoOn, value);
    }

    [Browsable(true)]
    [Category("Communication")]
    [Description("")]
    public bool HardwareHandshake
    {
      get => this.Get<bool>(ParameterKey.HardwareHandshake);
      set => this.Set(ParameterKey.HardwareHandshake, value);
    }

    [Browsable(true)]
    [Category("Communication")]
    [Description("")]
    public bool TestEcho
    {
      get => this.Get<bool>(ParameterKey.TestEcho);
      set => this.Set(ParameterKey.TestEcho, value);
    }

    [Browsable(true)]
    [Category("Communication")]
    [TypeConverter(typeof (ConfigList.GmmSettingsConverter))]
    [Description("COMserver managed by MeterVPN server.")]
    public string COMserver
    {
      get => this.Get<string>(ParameterKey.COMserver);
      set => this.Set(ParameterKey.COMserver, value);
    }

    [Browsable(true)]
    [Category("Communication")]
    [Description("")]
    public bool MinoConnectIsUSB
    {
      get => this.Get<bool>(ParameterKey.MinoConnectIsUSB);
      set => this.Set(ParameterKey.MinoConnectIsUSB, value);
    }

    [Browsable(true)]
    [Category("Communication")]
    [TypeConverter(typeof (ConfigList.GmmSettingsConverter))]
    [Description("")]
    public string MinoConnectBaseState
    {
      get => this.Get<string>(ParameterKey.MinoConnectBaseState);
      set => this.Set(ParameterKey.MinoConnectBaseState, value);
    }

    [Browsable(true)]
    [Category("Communication")]
    [Description("")]
    public string MinoConnectTestFor
    {
      get => this.Get<string>(ParameterKey.MinoConnectTestFor);
      set => this.Set(ParameterKey.MinoConnectTestFor, value);
    }

    [Browsable(true)]
    [Category("Communication")]
    [Description("")]
    public string ForceMinoConnectState
    {
      get => this.Get<string>(ParameterKey.ForceMinoConnectState);
      set => this.Set(ParameterKey.ForceMinoConnectState, value);
    }

    [Browsable(true)]
    [Category("Communication")]
    [TypeConverter(typeof (ConfigList.GmmSettingsConverter))]
    [Description("Bus mode used by DeviceCollector.")]
    public string BusMode
    {
      get => this.Get<string>(ParameterKey.BusMode);
      set => this.Set(ParameterKey.BusMode, value);
    }

    [Browsable(true)]
    [Category("Communication")]
    [TypeConverter(typeof (ConfigList.GmmSettingsConverter))]
    [Description("Used handler for configuratin and reading")]
    public string UsedHandler
    {
      get => this.Get<string>(ParameterKey.UsedHandler);
      set => this.Set(ParameterKey.UsedHandler, value);
    }

    [Browsable(true)]
    [Category("Communication")]
    [TypeConverter(typeof (ConfigList.GmmSettingsConverter))]
    [Description("AES protection key")]
    public string AES
    {
      get => this.Get<string>(ParameterKey.AES);
      set => this.Set(ParameterKey.AES, value);
    }

    [Browsable(true)]
    [Category("Communication")]
    [TypeConverter(typeof (ConfigList.GmmSettingsConverter))]
    [Description("Wake up sequence.")]
    public string Wakeup
    {
      get => this.Get<string>(ParameterKey.Wakeup);
      set => this.Set(ParameterKey.Wakeup, value);
    }

    [Browsable(true)]
    [Category("Communication")]
    [Description("")]
    public bool UseBreak
    {
      get => this.Get<bool>(ParameterKey.UseBreak);
      set => this.Set(ParameterKey.UseBreak, value);
    }

    [Browsable(true)]
    [Category("Communication")]
    [TypeConverter(typeof (ConfigList.GmmSettingsConverter))]
    [Description("MinoConnect IrDa selection.")]
    public string IrDaSelection
    {
      get => this.Get<string>(ParameterKey.IrDaSelection);
      set => this.Set(ParameterKey.IrDaSelection, value);
    }

    [Browsable(true)]
    [Category("Communication")]
    [TypeConverter(typeof (ConfigList.GmmSettingsConverter))]
    [Description("MinoConnect CombiHead selection.")]
    public string CombiHeadSelection
    {
      get => this.Get<string>(ParameterKey.CombiHeadSelection);
      set => this.Set(ParameterKey.CombiHeadSelection, value);
    }

    [Browsable(true)]
    [Category("M-Bus")]
    [Description("")]
    public bool KeepExistingDestinationAddress
    {
      get => this.Get<bool>(ParameterKey.KeepExistingDestinationAddress);
      set => this.Set(ParameterKey.KeepExistingDestinationAddress, value);
    }

    [Browsable(true)]
    [Category("M-Bus")]
    [Description("")]
    public bool SendFirstApplicationReset
    {
      get => this.Get<bool>(ParameterKey.SendFirstApplicationReset);
      set => this.Set(ParameterKey.SendFirstApplicationReset, value);
    }

    [Browsable(true)]
    [Category("M-Bus")]
    [Description("")]
    public bool SendFirstSND_NKE
    {
      get => this.Get<bool>(ParameterKey.SendFirstSND_NKE);
      set => this.Set(ParameterKey.SendFirstSND_NKE, value);
    }

    [Browsable(true)]
    [Category("M-Bus")]
    [Description("True will sends REQ_UD2 with 0x5B.")]
    public bool UseREQ_UD2_5B
    {
      get => this.Get<bool>(ParameterKey.UseREQ_UD2_5B);
      set => this.Set(ParameterKey.UseREQ_UD2_5B, value);
    }

    [Browsable(true)]
    [Category("M-Bus")]
    [Description("REQ_UD2 with Multi-telegram support.")]
    public bool IsMultiTelegrammEnabled
    {
      get => this.Get<bool>(ParameterKey.IsMultiTelegrammEnabled);
      set => this.Set(ParameterKey.IsMultiTelegrammEnabled, value);
    }

    [Browsable(true)]
    [DefaultValue(1)]
    [Category("M-Bus")]
    [Description("Start primary address used by scan.")]
    public byte ScanStartAddress
    {
      get => this.Get<byte>(ParameterKey.ScanStartAddress);
      set => this.Set(ParameterKey.ScanStartAddress, (int) value);
    }

    [Browsable(true)]
    [Category("M-Bus")]
    [Description("")]
    public bool ChangeInterfaceBaudrateToo
    {
      get => this.Get<bool>(ParameterKey.ChangeInterfaceBaudrateToo);
      set => this.Set(ParameterKey.ChangeInterfaceBaudrateToo, value);
    }

    [Browsable(true)]
    [Category("M-Bus")]
    [Description("")]
    public int OrganizeStartAddress
    {
      get => this.Get<int>(ParameterKey.OrganizeStartAddress);
      set => this.Set(ParameterKey.OrganizeStartAddress, value);
    }

    [Browsable(true)]
    [Category("M-Bus")]
    [Description("M-Bus primary address.")]
    public byte PrimaryAddress
    {
      get => this.Get<byte>(ParameterKey.PrimaryAddress);
      set => this.Set(ParameterKey.PrimaryAddress, (int) value);
    }

    [Browsable(true)]
    [Category("M-Bus")]
    [Description("M-Bus secondary address.")]
    public uint SecondaryAddress
    {
      get => this.Get<uint>(ParameterKey.SecondaryAddress);
      set => this.Set(ParameterKey.SecondaryAddress, value);
    }

    [Browsable(true)]
    [DefaultValue("FFFFFFF0")]
    [Category("M-Bus")]
    [Description("Start secondary address used by scan. 0fffffff = first to last. fffffff0 = last to first.")]
    public string ScanStartSerialnumber
    {
      get => this.Get<string>(ParameterKey.ScanStartSerialnumber);
      set => this.Set(ParameterKey.ScanStartSerialnumber, value);
    }

    [Browsable(true)]
    [Category("M-Bus")]
    [Description("Addressing mode used by REQ_UD2.")]
    public bool OnlySecondaryAddressing
    {
      get => this.Get<bool>(ParameterKey.OnlySecondaryAddressing);
      set => this.Set(ParameterKey.OnlySecondaryAddressing, value);
    }

    [Browsable(true)]
    [Category("M-Bus")]
    [Description("Start secondary address used by scan.")]
    public bool FastSecondaryAddressing
    {
      get => this.Get<bool>(ParameterKey.FastSecondaryAddressing);
      set => this.Set(ParameterKey.FastSecondaryAddressing, value);
    }

    [Browsable(true)]
    [Category("Timing")]
    [Description("")]
    public int TransTime_AfterBreak
    {
      get => this.Get<int>(ParameterKey.TransTime_AfterBreak);
      set => this.Set(ParameterKey.TransTime_AfterBreak, value);
    }

    [Browsable(true)]
    [Category("Timing")]
    [Description("")]
    public int TransTime_AfterOpen
    {
      get => this.Get<int>(ParameterKey.TransTime_AfterOpen);
      set => this.Set(ParameterKey.TransTime_AfterOpen, value);
    }

    [Browsable(true)]
    [Category("Timing")]
    [Description("")]
    public int TransTime_BreakTime
    {
      get => this.Get<int>(ParameterKey.TransTime_BreakTime);
      set => this.Set(ParameterKey.TransTime_BreakTime, value);
    }

    [Browsable(true)]
    [Category("Timing")]
    [Description("")]
    public int TransTime_GlobalOffset
    {
      get => this.Get<int>(ParameterKey.TransTime_GlobalOffset);
      set => this.Set(ParameterKey.TransTime_GlobalOffset, value);
    }

    [Browsable(true)]
    [Category("Timing")]
    [Description("")]
    public int RecTime_BeforFirstByte
    {
      get => this.Get<int>(ParameterKey.RecTime_BeforFirstByte);
      set => this.Set(ParameterKey.RecTime_BeforFirstByte, value);
    }

    [Browsable(true)]
    [Category("Timing")]
    [Description("")]
    public int RecTime_GlobalOffset
    {
      get => this.Get<int>(ParameterKey.RecTime_GlobalOffset);
      set => this.Set(ParameterKey.RecTime_GlobalOffset, value);
    }

    [Browsable(true)]
    [Category("Timing")]
    [Description("")]
    public int RecTime_OffsetPerBlock
    {
      get => this.Get<int>(ParameterKey.RecTime_OffsetPerBlock);
      set => this.Set(ParameterKey.RecTime_OffsetPerBlock, value);
    }

    [Browsable(true)]
    [Category("Timing")]
    [Description("")]
    public int RecTime_OffsetPerByte
    {
      get => this.Get<int>(ParameterKey.RecTime_OffsetPerByte);
      set => this.Set(ParameterKey.RecTime_OffsetPerByte, value);
    }

    [Browsable(true)]
    [Category("Timing")]
    [Description("")]
    public int RecTransTime
    {
      get => this.Get<int>(ParameterKey.RecTransTime);
      set => this.Set(ParameterKey.RecTransTime, value);
    }

    [Browsable(true)]
    [Category("Timing")]
    [Description("")]
    public int WaitBeforeRepeatTime
    {
      get => this.Get<int>(ParameterKey.WaitBeforeRepeatTime);
      set => this.Set(ParameterKey.WaitBeforeRepeatTime, value);
    }

    [Browsable(true)]
    [Category("Timing")]
    [Description("")]
    public int BreakIntervalTime
    {
      get => this.Get<int>(ParameterKey.BreakIntervalTime);
      set => this.Set(ParameterKey.BreakIntervalTime, value);
    }

    [Browsable(true)]
    [Category("Communication")]
    [Description("")]
    public string TransceiverType
    {
      get => this.Get<string>(ParameterKey.TransceiverType);
      set => this.Set(ParameterKey.TransceiverType, value);
    }

    [Browsable(true)]
    [Category("M-Bus")]
    [Description("")]
    public string SelectedDeviceMBusType
    {
      get => this.Get<string>(ParameterKey.SelectedDeviceMBusType);
      set => this.Set(ParameterKey.SelectedDeviceMBusType, value);
    }

    [Browsable(true)]
    [Category("Communication")]
    [Description("")]
    public DateTime FromTime
    {
      get => this.Get<DateTime>(ParameterKey.FromTime);
      set => this.Set(ParameterKey.FromTime, value);
    }

    [Browsable(true)]
    [Category("Communication")]
    [Description("")]
    public DateTime ToTime
    {
      get => this.Get<DateTime>(ParameterKey.ToTime);
      set => this.Set(ParameterKey.ToTime, value);
    }

    [Browsable(true)]
    [Category("Communication")]
    [Description("")]
    public bool BeepSignalOnReadResult
    {
      get => this.Get<bool>(ParameterKey.BeepSignalOnReadResult);
      set => this.Set(ParameterKey.BeepSignalOnReadResult, value);
    }

    [Browsable(true)]
    [Category("Communication")]
    [Description("")]
    public int DaKonId
    {
      get => this.Get<int>(ParameterKey.DaKonId);
      set => this.Set(ParameterKey.DaKonId, value);
    }

    [Browsable(true)]
    [Category("Communication")]
    [Description("")]
    public string LogFilePath
    {
      get => this.Get<string>(ParameterKey.LogFilePath);
      set => this.Set(ParameterKey.LogFilePath, value);
    }

    [Browsable(true)]
    [Category("Communication")]
    [Description("")]
    public string Password
    {
      get => this.Get<string>(ParameterKey.Password);
      set => this.Set(ParameterKey.Password, value);
    }

    [Browsable(true)]
    [Category("Communication")]
    [Description("")]
    public bool LogToFileEnabled
    {
      get => this.Get<bool>(ParameterKey.LogToFileEnabled);
      set => this.Set(ParameterKey.LogToFileEnabled, value);
    }

    [Browsable(true)]
    [Category("Communication")]
    [Description("")]
    public bool UseExternalKeyForReading
    {
      get => this.Get<bool>(ParameterKey.UseExternalKeyForReading);
      set => this.Set(ParameterKey.UseExternalKeyForReading, value);
    }

    public T Get<T>(ParameterKey key) => this.Get<T>(key.ToString());

    public T Get<T>(string key)
    {
      if (this.configList == null)
        return default (T);
      int index = this.configList.IndexOfKey(key);
      if (index < 0)
        return default (T);
      string str = this.configList.Values[index];
      return typeof (T).IsEnum ? (T) Enum.Parse(typeof (T), str) : (T) Convert.ChangeType((object) str, typeof (T), (IFormatProvider) CultureInfo.InvariantCulture);
    }

    private void Set(ParameterKey key, uint value) => this.Set(key, value.ToString());

    private void Set(ParameterKey key, int value) => this.Set(key, value.ToString());

    private void Set(ParameterKey key, bool value) => this.Set(key, value.ToString());

    private void Set(ParameterKey key, string value)
    {
      if (!string.IsNullOrEmpty(value))
        this.Set(key.ToString(), value.ToString());
      else
        this.Set(key.ToString(), string.Empty);
    }

    private void Set(ParameterKey key, DateTime value)
    {
      this.Set(key.ToString(), value.ToString((IFormatProvider) CultureInfo.InvariantCulture));
    }

    private void Set(string key, string value)
    {
      lock (ConfigList.lockObj)
      {
        if (this.configList == null)
          return;
        if ((key == ParameterKey.COMserver.ToString() || key == ParameterKey.Port.ToString()) && value.Contains("{"))
          value = value.Substring(0, value.IndexOf("{") - 1);
        if (this.configList.IndexOfKey(key) >= 0)
        {
          if (!(this.configList[key] != value))
            return;
          this.configList[key] = value;
          this.OnPropertyChanged(key);
        }
        else
        {
          this.configList.Add(key, value);
          this.OnPropertyChanged(key);
        }
      }
    }

    public void OnPropertyChanged(string propertyName)
    {
      if (this.PropertyChanged == null)
        return;
      this.PropertyChanged((object) this, new PropertyChangedEventArgs(propertyName));
    }

    public void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
    {
      if (this.CollectionChanged == null)
        return;
      this.CollectionChanged((object) this, e);
    }

    private static SortedList<string, string> DeepCopy(SortedList<string, string> collection)
    {
      if (collection == null)
        return (SortedList<string, string>) null;
      SortedList<string, string> sortedList = new SortedList<string, string>();
      lock (ConfigList.lockObj)
      {
        using (IEnumerator<KeyValuePair<string, string>> enumerator = collection.GetEnumerator())
        {
          while (enumerator.MoveNext())
            sortedList.Add(enumerator.Current.Key, enumerator.Current.Value);
        }
      }
      return sortedList;
    }

    public bool Equal(SortedList<string, string> list)
    {
      foreach (KeyValuePair<string, string> config in this.configList)
      {
        if (!list.ContainsKey(config.Key) || list[config.Key] != config.Value)
          return false;
      }
      return true;
    }

    public static ConfigList CreateM8(string port)
    {
      return new ConfigList()
      {
        Baudrate = 9600,
        BreakIntervalTime = 10000,
        BusMode = "MBusPointToPoint",
        IrDaSelection = "RoundSide",
        MaxRequestRepeat = 3,
        MinoConnectBaseState = "IrCombiHead",
        MinoConnectIrDaPulseTime = 0,
        MinoConnectPowerOffTime = 3600,
        OnlySecondaryAddressing = false,
        Parity = "even",
        RecTime_BeforFirstByte = 1500,
        RecTime_OffsetPerBlock = 50,
        RecTransTime = 30,
        SelectedDeviceMBusType = "M8",
        TestEcho = false,
        TransceiverType = "Reader",
        TransTime_AfterBreak = 100,
        TransTime_AfterOpen = 200,
        TransTime_BreakTime = 2600,
        Type = "COM",
        Port = port,
        TransceiverDevice = "MinoConnect",
        Wakeup = "BaudrateCarrier",
        WaitBeforeRepeatTime = 200
      };
    }

    public string ReadingChannelIdentification
    {
      get => this.readingChannelIdentification;
      set
      {
        if (!(this.readingChannelIdentification != value))
          return;
        this.readingChannelIdentification = value;
        this.OnPropertyChanged(this.ReadingChannelIdentification);
      }
    }

    private class GmmSettingsConverter : TypeConverter
    {
      public override bool CanConvertFrom(ITypeDescriptorContext context, System.Type sourceType)
      {
        return context.PropertyDescriptor.Name == ParameterKey.Baudrate.ToString();
      }

      public override object ConvertFrom(
        ITypeDescriptorContext context,
        CultureInfo culture,
        object value)
      {
        string s = value as string;
        if (!(context.PropertyDescriptor.Name == ParameterKey.Baudrate.ToString()))
          return base.ConvertFrom(context, culture, value);
        int result;
        return int.TryParse(s, out result) ? (object) result : (object) 2400;
      }

      public override bool GetStandardValuesSupported(ITypeDescriptorContext context) => true;

      public override TypeConverter.StandardValuesCollection GetStandardValues(
        ITypeDescriptorContext context)
      {
        if (context.PropertyDescriptor.Name == ParameterKey.Baudrate.ToString())
        {
          List<ValueItem> availableBaudrates = Constants.GetAvailableBaudrates();
          if (availableBaudrates != null)
            return new TypeConverter.StandardValuesCollection((ICollection) availableBaudrates);
        }
        if (context.PropertyDescriptor.Name == ParameterKey.COMserver.ToString())
        {
          List<ValueItem> availableCoMserver = Constants.GetAvailableCOMserver();
          if (availableCoMserver != null)
            return new TypeConverter.StandardValuesCollection((ICollection) availableCoMserver);
        }
        if (context.PropertyDescriptor.Name == ParameterKey.BusMode.ToString())
        {
          List<ValueItem> availableBusMode = Constants.GetAvailableBusMode();
          if (availableBusMode != null)
            return new TypeConverter.StandardValuesCollection((ICollection) availableBusMode);
        }
        if (context.PropertyDescriptor.Name == ParameterKey.Type.ToString())
        {
          List<ValueItem> comConnectionType = Constants.GetAvailableAsyncComConnectionType();
          if (comConnectionType != null)
            return new TypeConverter.StandardValuesCollection((ICollection) comConnectionType);
        }
        if (context.PropertyDescriptor.Name == ParameterKey.Port.ToString())
        {
          List<ValueItem> availableComPorts = Constants.GetAvailableComPorts();
          if (availableComPorts != null)
            return new TypeConverter.StandardValuesCollection((ICollection) availableComPorts);
        }
        if (context.PropertyDescriptor.Name == ParameterKey.Parity.ToString())
        {
          List<ValueItem> availableParity = Constants.GetAvailableParity();
          if (availableParity != null)
            return new TypeConverter.StandardValuesCollection((ICollection) availableParity);
        }
        if (context.PropertyDescriptor.Name == ParameterKey.Wakeup.ToString())
        {
          List<ValueItem> availableWakeup = Constants.GetAvailableWakeup();
          if (availableWakeup != null)
            return new TypeConverter.StandardValuesCollection((ICollection) availableWakeup);
        }
        if (context.PropertyDescriptor.Name == ParameterKey.TransceiverDevice.ToString())
        {
          List<ValueItem> transceiverDevice = Constants.GetAvailableTransceiverDevice();
          if (transceiverDevice != null)
            return new TypeConverter.StandardValuesCollection((ICollection) transceiverDevice);
        }
        if (context.PropertyDescriptor.Name == ParameterKey.IrDaSelection.ToString())
        {
          List<ValueItem> availableIrDaSelection = Constants.GetAvailableIrDaSelection();
          if (availableIrDaSelection != null)
            return new TypeConverter.StandardValuesCollection((ICollection) availableIrDaSelection);
        }
        if (context.PropertyDescriptor.Name == ParameterKey.CombiHeadSelection.ToString())
        {
          List<ValueItem> combiHeadSelection = Constants.GetAvailableCombiHeadSelection();
          if (combiHeadSelection != null)
            return new TypeConverter.StandardValuesCollection((ICollection) combiHeadSelection);
        }
        if (context.PropertyDescriptor.Name == ParameterKey.MinoConnectBaseState.ToString())
        {
          List<ValueItem> connectBaseStates = Constants.GetAvailableMinoConnectBaseStates();
          if (connectBaseStates != null)
            return new TypeConverter.StandardValuesCollection((ICollection) connectBaseStates);
        }
        return base.GetStandardValues(context);
      }
    }
  }
}
