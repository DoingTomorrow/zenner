// Decompiled with JetBrains decompiler
// Type: CommunicationPort.Functions.CommunicationPortFunctions
// Assembly: CommunicationPort, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 4F7EB5DB-4517-47DC-B5F2-757F0B03AE01
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\CommunicationPort.dll

using StartupLib;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using ZENNER.CommonLibrary;
using ZENNER.CommonLibrary.Exceptions;

#nullable disable
namespace CommunicationPort.Functions
{
  public class CommunicationPortFunctions : IReadoutConfig, IPort, ICommunicationFunctions
  {
    internal ConfigList configList;
    public static int SystemCommunicationTimeoutOffset = 100;
    internal bool IsPluginObject = false;
    public CommunicationBase communicationObject;

    public string TransceiverDeviceInfo { get; set; }

    public event EventHandler<string> OnMessageEvent;

    public event EventHandler<bool> OnKeyEvent;

    public event EventHandler<int> OnAliveEvent;

    public event EventHandler<byte[]> OnRequest;

    public event EventHandler<byte[]> OnResponse;

    public event EventHandler OnBatteryLow;

    public CommunicationPortFunctions() => this.TransceiverDeviceInfo = "";

    public void SetReadoutConfiguration(ConfigList configList)
    {
      if (configList == null)
        throw new ArgumentNullException(nameof (configList));
      if (this.configList == null)
      {
        this.configList = configList;
        this.configList.PropertyChanged += new PropertyChangedEventHandler(this.ConfigList_PropertyChanged);
        this.configList.CollectionChanged += new NotifyCollectionChangedEventHandler(this.configList_CollectionChanged);
        this.RecreateCommunicationObject();
      }
      else if (this.configList != configList)
        throw new ArgumentException("this.configList != configList");
    }

    [Obsolete("Use: SetReadoutConfiguration(ConfigList configList)")]
    public void SetReadoutConfiguration(SortedList<string, string> readoutConfiguration)
    {
      if (this.configList == null)
        this.SetReadoutConfiguration(new ConfigList(readoutConfiguration));
      else
        this.configList.Reset(readoutConfiguration);
    }

    private void configList_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
      if (this.communicationObject == null)
        return;
      this.RecreateCommunicationObject();
    }

    private void ConfigList_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      if (e.PropertyName == "Type" || e.PropertyName == "TransceiverDevice")
        this.RecreateCommunicationObject();
      if (e.PropertyName == "COMserver" && this.communicationObject is CommunicationByIP)
        this.Close();
      if (!(e.PropertyName == ParameterKey.Port.ToString()))
        return;
      this.Close();
    }

    public ConfigList GetReadoutConfiguration() => this.configList;

    public void RaiseMessageEvent(string theMessage)
    {
      if (this.OnMessageEvent == null)
        return;
      this.OnMessageEvent((object) this, theMessage);
    }

    public void RaiseBatteryLowEvent()
    {
      if (this.OnBatteryLow == null)
        return;
      this.OnBatteryLow((object) this, (EventArgs) null);
    }

    public void RaiseKeyEvent()
    {
      if (this.OnKeyEvent == null)
        return;
      this.OnKeyEvent((object) this, true);
    }

    public void RaiseAliveEvent(int aliveCounter)
    {
      if (this.OnAliveEvent == null)
        return;
      this.OnAliveEvent((object) this, aliveCounter);
    }

    public PortTypes PortType
    {
      get
      {
        if (this.communicationObject == null)
          return PortTypes.None;
        if (this.communicationObject is CommunicationByComPort)
          return PortTypes.COM;
        if (this.communicationObject is CommunicationByMinoConnect)
          return PortTypes.MinoConnect;
        return this.communicationObject is CommunicationByIP ? PortTypes.AsynchronIP : PortTypes.None;
      }
      set
      {
        if (value == this.PortType)
          return;
        switch (value)
        {
          case PortTypes.COM:
            this.configList.Type = "COM";
            this.configList.TransceiverDevice = string.Empty;
            break;
          case PortTypes.MinoConnect:
            this.configList.Type = "COM";
            this.configList.TransceiverDevice = "MinoConnect";
            break;
          case PortTypes.AsynchronIP:
            this.configList.Type = "AsynchronIP";
            break;
        }
        this.RecreateCommunicationObject();
      }
    }

    public CommunicationByMinoConnect GetCommunicationByMinoConnect()
    {
      if (this.configList == null)
        throw new ArgumentNullException("configList");
      if (this.communicationObject == null)
        throw new ArgumentNullException("communicationObject");
      return this.communicationObject is CommunicationByMinoConnect communicationObject ? communicationObject : throw new InvalidSettingsException(this.configList.GetSortedList(), "Invalid profile for MinoConnect!");
    }

    private void RecreateCommunicationObject()
    {
      if (this.communicationObject != null)
        this.communicationObject.Close();
      if (string.IsNullOrEmpty(this.configList.Type))
        throw new Exception("Type not defined! CommunicationObject can not be initialised without Type");
      if (this.configList.Type == "COM")
      {
        if (this.configList.TransceiverDevice == "MinoConnect")
        {
          if (this.communicationObject is CommunicationByMinoConnect)
            return;
          if (this.communicationObject != null)
            this.communicationObject.Dispose();
          this.communicationObject = (CommunicationBase) new CommunicationByMinoConnect((ICommunicationFunctions) this);
        }
        else if (!(this.communicationObject is CommunicationByComPort))
        {
          if (this.communicationObject != null)
            this.communicationObject.Dispose();
          this.communicationObject = (CommunicationBase) new CommunicationByComPort((ICommunicationFunctions) this);
        }
      }
      else
      {
        if (!(this.configList.Type == "Remote_VPN") && !(this.configList.Type == "Remote"))
          throw new Exception("Invalid COM type: " + this.configList.Type);
        if (this.communicationObject is CommunicationByIP)
          return;
        if (this.communicationObject != null)
          this.communicationObject.Dispose();
        this.communicationObject = (CommunicationBase) new CommunicationByIP((ICommunicationFunctions) this);
      }
    }

    public void Dispose()
    {
      if (this.communicationObject == null)
        return;
      this.communicationObject.Close();
      this.communicationObject.Dispose();
      this.communicationObject = (CommunicationBase) null;
    }

    public bool DiscardInBuffer() => this.communicationObject.DiscardInBuffer();

    public void ForceWakeup() => this.communicationObject.ForceWakeup();

    public bool IsOpen => this.communicationObject.IsOpen;

    public int BytesToRead => this.communicationObject.BytesToRead;

    public void Open()
    {
      if (this.IsPluginObject)
      {
        try
        {
          if (PlugInLoader.IsPluginLoaded("AsyncCom"))
          {
            object obj = PlugInLoader.GetPlugIn("AsyncCom").GetPluginInfo().Interface;
            obj.GetType().GetMethod("Close").Invoke(obj, (object[]) null);
          }
        }
        catch
        {
        }
      }
      this.communicationObject.Open();
    }

    public void Close()
    {
      if (this.communicationObject == null)
        return;
      this.communicationObject.Close();
    }

    public byte[] ReadExisting()
    {
      if (this.communicationObject == null)
        return (byte[]) null;
      int bytesToRead = this.communicationObject.BytesToRead;
      if (bytesToRead <= 0)
        return (byte[]) null;
      byte[] buffer = new byte[bytesToRead];
      this.communicationObject.Read(buffer, 0, buffer.Length);
      return buffer;
    }

    public byte[] ReadHeader(int count)
    {
      byte[] e = this.communicationObject.ReadHeader(count);
      if (this.OnResponse != null)
        this.OnResponse((object) this, e);
      return e;
    }

    public byte[] ReadEnd(int count)
    {
      byte[] e = this.communicationObject.ReadEnd(count);
      if (this.OnResponse != null)
        this.OnResponse((object) this, e);
      return e;
    }

    public void Write(byte[] request)
    {
      this.communicationObject.Write(request);
      if (this.OnRequest == null)
        return;
      this.OnRequest((object) this, request);
    }

    public void WriteWithoutDiscardInputBuffer(byte[] request)
    {
      this.communicationObject.WriteWithoutDiscardInputBuffer(request);
      if (this.OnRequest == null)
        return;
      this.OnRequest((object) this, request);
    }

    public void DiscardCurrentInBuffer() => this.communicationObject.DiscardCurrentInBuffer();

    public Version GetTransceiverVersion()
    {
      return this.communicationObject != null && this.communicationObject is CommunicationByMinoConnect ? ((CommunicationByMinoConnect) this.communicationObject).GetTransceiverVersion() : throw new Exception("GetTransceiverVersion is only supported for MinoConnect");
    }
  }
}
