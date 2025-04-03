// Decompiled with JetBrains decompiler
// Type: ZR_ClassLibrary.OpenNETRAPI
// Assembly: ZR_ClassLibrary, Version=6.16.22.24524, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: EF8F26C8-41DE-4472-B020-D54F7F8B6357
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\ZR_ClassLibrary.dll

using OpenNETCF.Desktop.Communication;
using System.Collections;

#nullable disable
namespace ZR_ClassLibrary
{
  public class OpenNETRAPI
  {
    private RAPI m_rapi;

    public event OpenNETRAPI.DeviceConnectionHandler DeviceConnectionEvent;

    public OpenNETRAPI() => this.m_rapi = new RAPI();

    public void InitEvents()
    {
      // ISSUE: method pointer
      this.m_rapi.ActiveSync.Active += new ActiveHandler((object) this, __methodptr(ActiveSync_Active));
      // ISSUE: method pointer
      this.m_rapi.ActiveSync.Disconnect += new DisconnectHandler((object) this, __methodptr(ActiveSync_Disconnect));
      if (!this.m_rapi.DevicePresent || this.DeviceConnectionEvent == null)
        return;
      this.DeviceConnectionEvent(OpenNETRAPI.ActiveSyncState.DeviceConnected);
    }

    public bool ConnectAsync()
    {
      try
      {
        if (!this.m_rapi.DevicePresent)
          return false;
        this.m_rapi.Connect(true, -1);
      }
      catch
      {
        return false;
      }
      return true;
    }

    public bool ConnectSync()
    {
      try
      {
        if (!this.m_rapi.DevicePresent)
          return false;
        this.m_rapi.Connect(true, 3);
      }
      catch
      {
        return false;
      }
      return true;
    }

    public void Disconnect() => this.m_rapi.Disconnect();

    public bool GetFileList(out ArrayList Files, string Filter)
    {
      Files = new ArrayList();
      try
      {
        if (!this.m_rapi.Connected)
          return false;
        foreach (FileInformation enumFile in (CollectionBase) this.m_rapi.EnumFiles("My Documents\\GMM\\Database\\" + Filter))
        {
          if (enumFile.FileName != "")
          {
            if (enumFile.FileSize > 0L)
              Files.Add((object) enumFile.FileName);
            else
              this.m_rapi.DeleteDeviceFile("My Documents\\GMM\\Database\\" + enumFile.FileName);
          }
        }
      }
      catch
      {
        return false;
      }
      return true;
    }

    public bool copyTo(string LocalFilename, string RemoteFileName)
    {
      try
      {
        if (!this.m_rapi.Connected)
          return false;
        this.m_rapi.CopyFileToDevice(LocalFilename, RemoteFileName, true);
      }
      catch
      {
        return false;
      }
      return true;
    }

    public bool copyFrom(string RemoteFileName, string LocalFilename)
    {
      try
      {
        if (!this.m_rapi.Connected)
          return false;
        this.m_rapi.CopyFileFromDevice(LocalFilename, RemoteFileName, true);
      }
      catch
      {
        return false;
      }
      return true;
    }

    public bool deleteFrom(string RemoteFileName)
    {
      try
      {
        if (!this.m_rapi.Connected)
          return false;
        this.m_rapi.DeleteDeviceFile(RemoteFileName);
      }
      catch
      {
        return false;
      }
      return true;
    }

    public bool getDeviceIdentifier(out string DeviceIdentifier)
    {
      DeviceIdentifier = string.Empty;
      try
      {
        CERegistryKey ceRegistryKey = CERegistry.LocalMachine.OpenSubKey("Ident", false);
        if (ceRegistryKey != null)
        {
          object obj = ceRegistryKey.GetValue("Name");
          if (obj != null)
            DeviceIdentifier = obj.ToString();
          ceRegistryKey.Close();
          ceRegistryKey = (CERegistryKey) null;
        }
        ceRegistryKey?.Close();
      }
      catch
      {
        return false;
      }
      return true;
    }

    private void ActiveSync_Active()
    {
      if (this.DeviceConnectionEvent == null)
        return;
      this.DeviceConnectionEvent(OpenNETRAPI.ActiveSyncState.DeviceConnected);
    }

    private void ActiveSync_Disconnect()
    {
      if (this.DeviceConnectionEvent == null)
        return;
      this.DeviceConnectionEvent(OpenNETRAPI.ActiveSyncState.DeviceDisconnected);
    }

    public delegate void DeviceConnectionHandler(OpenNETRAPI.ActiveSyncState activeSyncState);

    public delegate void DeviceTransferHandler(int percentage);

    public enum ActiveSyncState
    {
      DeviceConnected,
      DeviceDisconnected,
    }
  }
}
