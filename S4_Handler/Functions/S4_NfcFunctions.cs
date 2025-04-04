// Decompiled with JetBrains decompiler
// Type: S4_Handler.Functions.S4_NfcFunctions
// Assembly: S4_Handler, Version=1.0.1.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 6FB3FA3B-A643-4E86-9555-EAB58D0F89E2
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\S4_Handler.dll

using CommunicationPort.Functions;
using HandlerLib;
using HandlerLib.NFC;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZENNER.CommonLibrary;
using ZR_ClassLibrary;

#nullable disable
namespace S4_Handler.Functions
{
  internal class S4_NfcFunctions
  {
    private S4_HandlerFunctions myHF;

    internal S4_NfcFunctions(S4_HandlerFunctions myHandlerFunctions)
    {
      this.myHF = myHandlerFunctions;
    }

    internal async Task<string> CheckNfcCommunication(
      ProgressHandler progress,
      CancellationToken cancelToken)
    {
      StringBuilder checkResult = new StringBuilder();
      string CheckInfo = "";
      try
      {
        CheckInfo = "Open com port error";
        this.myHF.Open();
        checkResult.AppendLine("Open communicaton port is ok");
        CheckInfo = "";
        if (!(this.myHF.myPort.communicationObject is CommunicationByMinoConnect))
        {
          checkResult.AppendLine("ReadoutConfiguration for MinoConnect not ok");
        }
        else
        {
          checkResult.AppendLine("MinoConnect is connected.");
          checkResult.AppendLine(this.myHF.myPort.TransceiverDeviceInfo.Replace(Environment.NewLine, ";  "));
          CheckInfo = "Access NFC_MiCon_Connector error";
          MiConConnectorVersion mcc_version = await this.myHF.GetNFC_ConnectorVersionAsync(progress, cancelToken);
          checkResult.AppendLine("NFC_MiCon_Connector is connected");
          uint? nullable = mcc_version.NDC_Lib_Version;
          FirmwareVersion ndclibversion = new FirmwareVersion(nullable.Value);
          checkResult.AppendLine("NDC_Lib: " + ndclibversion.ToString());
          nullable = mcc_version.FirmwareVersion;
          FirmwareVersion version = new FirmwareVersion(nullable.Value);
          checkResult.AppendLine("NFC_MiCon_Connector firmware: " + version.ToString());
          checkResult.AppendLine("NFC_MiCon_Connector unique_ID: " + Util.ByteArrayToHexString(mcc_version.Unique_ID));
          CheckInfo = "";
          CheckInfo = "Access NFC_Coupler echo error";
          string couplerEchoAsync = await this.myHF.GetNFC_CouplerEchoAsync(progress, cancelToken);
          checkResult.AppendLine("NFC_Coupler is connected");
          CheckInfo = "";
          CheckInfo = "Access NFC_Coupler_Identification error";
          string str = await this.myHF.GetNFC_CouplerIdentAsync(progress, cancelToken);
          checkResult.AppendLine("NFC_Coupler Identification " + str);
          CheckInfo = "";
          CheckInfo = "Access IUW error";
          DeviceIdentification ident = await this.myHF.ReadVersionAsync(progress, cancelToken);
          checkResult.AppendLine("IUW is connected");
          nullable = ident.FirmwareVersion;
          version = new FirmwareVersion(nullable.Value);
          checkResult.AppendLine("IUW FirmwareVersion: " + version.ToString());
          checkResult.AppendLine("IUW SerialNumber: " + ident.FullSerialNumber.ToString());
          StringBuilder stringBuilder = checkResult;
          nullable = ident.MeterID;
          string str1 = "IUW Meter_ID: " + nullable.ToString();
          stringBuilder.AppendLine(str1);
          CheckInfo = "";
          CheckInfo = "Access IUW_Tag_Identification error";
          byte[] buffer = await this.myHF.NFC_GetTagIdentAsync(progress, cancelToken);
          string tagInfo = Util.ByteArrayToHexString(buffer);
          buffer = (byte[]) null;
          checkResult.AppendLine("IUW Tag_Identification: " + tagInfo.Substring(6, 16));
          CheckInfo = "";
          mcc_version = (MiConConnectorVersion) null;
          ndclibversion = new FirmwareVersion();
          version = new FirmwareVersion();
          str = (string) null;
          ident = (DeviceIdentification) null;
          tagInfo = (string) null;
        }
      }
      catch (Exception ex)
      {
        checkResult.AppendLine();
        checkResult.AppendLine(CheckInfo);
        checkResult.AppendLine();
        checkResult.AppendLine(ex.Message);
      }
      string str2 = checkResult.ToString();
      checkResult = (StringBuilder) null;
      CheckInfo = (string) null;
      return str2;
    }
  }
}
