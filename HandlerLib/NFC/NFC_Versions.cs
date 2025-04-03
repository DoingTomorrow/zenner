// Decompiled with JetBrains decompiler
// Type: HandlerLib.NFC.NFC_Versions
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

using CommunicationPort.Functions;
using GmmDbLib;
using GmmDbLib.DataSets;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZENNER.CommonLibrary;

#nullable disable
namespace HandlerLib.NFC
{
  public class NFC_Versions
  {
    private static SortedList<string, NFC_Versions.FirmwareVersionsInfo> VersionList = new SortedList<string, NFC_Versions.FirmwareVersionsInfo>();

    public static async Task<string> GetVersionInfoText(
      FirmwareVersion deviceFirmware,
      CommunicationPortFunctions thePort,
      NfcDeviceCommands nfcCommands,
      ProgressHandler progress,
      CancellationToken token)
    {
      Version MiConFirmwareVersion = thePort.GetTransceiverVersion();
      MiConConnectorVersion connectorVersion = await nfcCommands.mySubunitCommands.ReadMiConIdentificationAsync(progress, token);
      StringBuilder result = new StringBuilder();
      result.AppendLine("Device firmware version: " + deviceFirmware.ToString());
      result.AppendLine("MinoConnect firmware version: " + MiConFirmwareVersion.ToString());
      result.AppendLine("MiConConnector firmware version: " + connectorVersion.GetFirmwareVersion().ToString());
      string versionInfoText = result.ToString();
      MiConFirmwareVersion = (Version) null;
      connectorVersion = (MiConConnectorVersion) null;
      result = (StringBuilder) null;
      return versionInfoText;
    }

    public static async Task CheckVersions(
      FirmwareVersion deviceFirmware,
      CommunicationPortFunctions thePort,
      NfcDeviceCommands nfcCommands,
      ProgressHandler progress,
      CancellationToken token)
    {
      Version MiConFirmwareVersion = thePort.GetTransceiverVersion();
      MiConConnectorVersion connectorVersion = await nfcCommands.mySubunitCommands.ReadMiConIdentificationAsync(progress, token);
      Version MiConConnectorFirmwareVersion = connectorVersion.GetFirmwareVersion();
      string firmwareString = deviceFirmware.ToString();
      int listIndex = -1;
      lock (NFC_Versions.VersionList)
      {
        listIndex = NFC_Versions.VersionList.IndexOfKey(deviceFirmware.ToString());
        if (listIndex < 0)
        {
          try
          {
            using (DbConnection myConnection = DbBasis.PrimaryDB.BaseDbConnection.GetNewConnection())
            {
              string sql = "SELECT MapId,FirmwareVersion,Options FROM ProgFiles WHERE FirmwareVersion = " + deviceFirmware.Version.ToString();
              DbDataAdapter ProgFilesDataAdapter = DbBasis.PrimaryDB.BaseDbConnection.GetDataAdapter(sql.ToString(), myConnection);
              HardwareTypeTables.ProgFilesDataTable ProgFilesTable = new HardwareTypeTables.ProgFilesDataTable();
              ProgFilesDataAdapter.Fill((DataTable) ProgFilesTable);
              if (ProgFilesTable.Count == 1 && !ProgFilesTable[0].IsOptionsNull())
              {
                List<KeyValuePair<string, string>> theOptions = DbUtil.KeyValueStringListToKeyValuePairList(ProgFilesTable[0].Options);
                KeyValuePair<string, string> keyValuePair = theOptions.First<KeyValuePair<string, string>>((System.Func<KeyValuePair<string, string>, bool>) (x => x.Key == "MiConFw"));
                string miConFw = keyValuePair.Value;
                keyValuePair = theOptions.First<KeyValuePair<string, string>>((System.Func<KeyValuePair<string, string>, bool>) (x => x.Key == "MiConConFw"));
                string miConConFw = keyValuePair.Value;
                NFC_Versions.VersionList.Add(firmwareString, new NFC_Versions.FirmwareVersionsInfo()
                {
                  MinoConnect = new Version(miConFw),
                  MiConConnector = new Version(miConConFw)
                });
                listIndex = NFC_Versions.VersionList.IndexOfKey(deviceFirmware.ToString());
                theOptions = (List<KeyValuePair<string, string>>) null;
                miConFw = (string) null;
                miConConFw = (string) null;
              }
              sql = (string) null;
              ProgFilesDataAdapter = (DbDataAdapter) null;
              ProgFilesTable = (HardwareTypeTables.ProgFilesDataTable) null;
            }
          }
          catch
          {
            NFC_Versions.VersionList.Add(firmwareString, (NFC_Versions.FirmwareVersionsInfo) null);
          }
        }
      }
      if (listIndex < 0 || NFC_Versions.VersionList.Values[listIndex] == null)
      {
        MiConFirmwareVersion = (Version) null;
        connectorVersion = (MiConConnectorVersion) null;
        MiConConnectorFirmwareVersion = (Version) null;
        firmwareString = (string) null;
      }
      else
      {
        StringBuilder message = new StringBuilder();
        message.AppendLine("Device firmware version: " + firmwareString);
        message.AppendLine("MinoConnect firmware version: " + MiConFirmwareVersion.ToString());
        message.AppendLine("MiConConnector firmware version: " + MiConConnectorFirmwareVersion.ToString());
        message.AppendLine();
        NFC_Versions.FirmwareVersionsInfo versionInfo = NFC_Versions.VersionList.Values[listIndex];
        bool update = false;
        if (MiConFirmwareVersion < versionInfo.MinoConnect)
        {
          update = true;
          message.AppendLine("Minimal MinoConnect firmware version: " + versionInfo.MinoConnect.ToString());
        }
        if (MiConConnectorFirmwareVersion < versionInfo.MiConConnector)
        {
          update = true;
          message.AppendLine("Minimal MiConConnector firmware version: " + versionInfo.MiConConnector.ToString());
        }
        if (update)
        {
          message.AppendLine();
          message.AppendLine("Please update the firmware version(s)");
          throw new Exception(message.ToString());
        }
        message = (StringBuilder) null;
        versionInfo = (NFC_Versions.FirmwareVersionsInfo) null;
        MiConFirmwareVersion = (Version) null;
        connectorVersion = (MiConConnectorVersion) null;
        MiConConnectorFirmwareVersion = (Version) null;
        firmwareString = (string) null;
      }
    }

    private class FirmwareVersionsInfo
    {
      internal Version MinoConnect;
      internal Version MiConConnector;
    }
  }
}
