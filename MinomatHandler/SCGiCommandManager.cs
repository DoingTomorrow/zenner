// Decompiled with JetBrains decompiler
// Type: MinomatHandler.SCGiCommandManager
// Assembly: MinomatHandler, Version=1.0.3.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 7EF7C01F-958A-42C5-BD1F-5A50D1BCE76C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MinomatHandler.dll

using System.Collections.Generic;
using ZR_ClassLibrary;

#nullable disable
namespace MinomatHandler
{
  public static class SCGiCommandManager
  {
    internal static byte[] GetBytes(SCGiCommand? cmd)
    {
      return !cmd.HasValue ? (byte[]) null : SCGiCommandManager.GetBytes(cmd.Value);
    }

    internal static byte[] GetBytes(SCGiCommand cmd)
    {
      switch (cmd)
      {
        case SCGiCommand.MinolId:
          return new byte[2]{ (byte) 4, (byte) 15 };
        case SCGiCommand.NodeId:
          return new byte[2]{ (byte) 5, (byte) 0 };
        case SCGiCommand.NetworkId:
          return new byte[2]{ (byte) 6, (byte) 0 };
        case SCGiCommand.SystemTime:
          return new byte[2]{ (byte) 12, (byte) 0 };
        case SCGiCommand.RadioChannel:
          return new byte[2]{ (byte) 4, (byte) 9 };
        case SCGiCommand.TransceiverChannelId:
          return new byte[2]{ (byte) 9, (byte) 0 };
        case SCGiCommand.RoutingTable:
          return new byte[2]{ (byte) 48, (byte) 0 };
        case SCGiCommand.FirmwareVersion:
          return new byte[2]{ (byte) 2, (byte) 0 };
        case SCGiCommand.UserappName:
          return new byte[2]{ (byte) 1, (byte) 1 };
        case SCGiCommand.FirmwareBuildTime:
          return new byte[2]{ (byte) 3, (byte) 0 };
        case SCGiCommand.UserappBuildTime:
          return new byte[2]{ (byte) 4, (byte) 0 };
        case SCGiCommand.ErrorFlags:
          return new byte[2]{ (byte) 7, (byte) 0 };
        case SCGiCommand.TransmissionPower:
          return new byte[2]{ (byte) 8, (byte) 0 };
        case SCGiCommand.MultiChannelSettings:
          return new byte[2]{ (byte) 10, (byte) 0 };
        case SCGiCommand.TransceiverFrequencyOffset:
          return new byte[2]{ (byte) 11, (byte) 0 };
        case SCGiCommand.TemperatureOffset:
          return new byte[2]{ (byte) 13, (byte) 0 };
        case SCGiCommand.PhaseDetailsBuffer:
          return new byte[2]{ (byte) 4, (byte) 10 };
        case SCGiCommand.PhaseDetails:
          return new byte[2]{ (byte) 4, (byte) 19 };
        case SCGiCommand.RestartMinomat:
          return new byte[2]{ (byte) 16, (byte) 0 };
        case SCGiCommand.MessUnitNumberMax:
          return new byte[2]{ (byte) 4, (byte) 33 };
        case SCGiCommand.MaxMessUnitNumberNotConfigured:
          return new byte[2]{ (byte) 4, (byte) 35 };
        case SCGiCommand.Scenario:
          return new byte[2]{ (byte) 4, (byte) 27 };
        case SCGiCommand.StartTestReception:
          return new byte[2]{ (byte) 4, (byte) 5 };
        case SCGiCommand.TestReceptionResult:
          return new byte[2]{ (byte) 4, (byte) 6 };
        case SCGiCommand.RegisterMessUnit:
          return new byte[2]{ (byte) 4, (byte) 4 };
        case SCGiCommand.ResetConfiguration:
          return new byte[2]{ (byte) 4, (byte) 12 };
        case SCGiCommand.StartNetworkSetup:
          return new byte[2]{ (byte) 4, (byte) 11 };
        case SCGiCommand.DeleteMessUnit:
          return new byte[2]{ (byte) 4, (byte) 7 };
        case SCGiCommand.InfoOfRegisteredMessUnit:
          return new byte[2]{ (byte) 4, (byte) 3 };
        case SCGiCommand.RegisteredMessUnits:
          return new byte[2]{ (byte) 4, (byte) 1 };
        case SCGiCommand.SimPin:
          return new byte[1]{ (byte) 193 };
        case SCGiCommand.MasterMinolID:
          return new byte[1]{ (byte) 4 };
        case SCGiCommand.APN:
          return new byte[1]{ (byte) 194 };
        case SCGiCommand.GPRSUserName:
          return new byte[1]{ (byte) 195 };
        case SCGiCommand.GPRSPassword:
          return new byte[1]{ (byte) 196 };
        case SCGiCommand.HttpServer:
          return new byte[1]{ (byte) 202 };
        case SCGiCommand.HttpResourceName:
          return new byte[1]{ (byte) 203 };
        case SCGiCommand.StartGSMTestReception:
          return new byte[1]{ (byte) 10 };
        case SCGiCommand.GSMTestReceptionState:
          return new byte[1]{ (byte) 169 };
        case SCGiCommand.ForceNetworkOptimization:
          return new byte[2]{ (byte) 4, (byte) 18 };
        case SCGiCommand.StartNetworkOptimization:
          return new byte[2]{ (byte) 4, (byte) 25 };
        case SCGiCommand.RegisterOrDeregisterSlave:
          return new byte[2]{ (byte) 4, (byte) 13 };
        case SCGiCommand.RegisteredSlaves:
          return new byte[2]{ (byte) 4, (byte) 14 };
        case SCGiCommand.Flash:
          return new byte[2]{ (byte) 5, (byte) 1 };
        case SCGiCommand.Eeprom:
          return new byte[2]{ (byte) 5, (byte) 2 };
        case SCGiCommand.AppInitialSettings:
          return new byte[1]{ (byte) 49 };
        case SCGiCommand.ActionTimepoint:
          return new byte[1]{ (byte) 208 };
        case SCGiCommand.MeasurementData:
          return new byte[2]{ (byte) 1, (byte) 0 };
        case SCGiCommand.HttpState:
          return new byte[1]{ (byte) 167 };
        case SCGiCommand.GsmState:
          return new byte[1]{ (byte) 160 };
        case SCGiCommand.ModemBuildDate:
          return new byte[1]{ (byte) 170 };
        case SCGiCommand.StartHttpConnection:
          return new byte[1]{ (byte) 9 };
        case SCGiCommand.ConfigurationString:
          return new byte[4]
          {
            (byte) 114,
            (byte) 99,
            (byte) 102,
            (byte) 10
          };
        case SCGiCommand.SwitchToNetworkModel:
          return new byte[2]{ (byte) 4, (byte) 20 };
        case SCGiCommand.GetComServerFile:
          return new byte[2]{ (byte) 15, (byte) 3 };
        case SCGiCommand.SetComServerFile:
          return new byte[2]{ (byte) 15, (byte) 4 };
        case SCGiCommand.MessUnitMetadata:
          return new byte[2]{ (byte) 1, (byte) 1 };
        case SCGiCommand.ActivateModemAT_Mode:
          return new byte[1]{ (byte) 8 };
        case SCGiCommand.LED:
          return new byte[2]{ (byte) 5, (byte) 6 };
        case SCGiCommand.ModemUpdateImageClear:
          return new byte[1]{ (byte) 238 };
        case SCGiCommand.ModemUpdate:
          return new byte[1]{ (byte) 239 };
        case SCGiCommand.ModemReboot:
          return new byte[1]{ (byte) 241 };
        case SCGiCommand.ModemCounter:
          return new byte[1]{ (byte) 223 };
        case SCGiCommand.TcpConfiguration:
          return new byte[1]{ (byte) 197 };
        case SCGiCommand.SendSMS:
          return new byte[1]{ (byte) 1 };
        case SCGiCommand.ModemDueDate:
          return new byte[1]{ (byte) 204 };
        case SCGiCommand.GsmLinkQuality:
          return new byte[1]{ (byte) 168 };
        case SCGiCommand.ModemUpdateTiming:
          return new byte[1]{ (byte) 237 };
        case SCGiCommand.ModemUpdateTest:
          return new byte[1]{ (byte) 236 };
        default:
          return (byte[]) null;
      }
    }

    internal static SCGiCommand? GetCommand(List<byte> payload)
    {
      if (payload == null)
        return new SCGiCommand?();
      if (Util.ByteArrayCompare(payload, new byte[4]
      {
        (byte) 114,
        (byte) 99,
        (byte) 102,
        (byte) 10
      }, 4))
        return new SCGiCommand?(SCGiCommand.ConfigurationString);
      if (Util.ByteArrayCompare(payload, new byte[2]
      {
        (byte) 4,
        (byte) 15
      }, 2))
        return new SCGiCommand?(SCGiCommand.MinolId);
      if (Util.ByteArrayCompare(payload, new byte[2]
      {
        (byte) 4,
        (byte) 9
      }, 2))
        return new SCGiCommand?(SCGiCommand.RadioChannel);
      if (Util.ByteArrayCompare(payload, new byte[2]
      {
        (byte) 4,
        (byte) 10
      }, 2))
        return new SCGiCommand?(SCGiCommand.PhaseDetailsBuffer);
      if (Util.ByteArrayCompare(payload, new byte[2]
      {
        (byte) 4,
        (byte) 19
      }, 2))
        return new SCGiCommand?(SCGiCommand.PhaseDetails);
      if (Util.ByteArrayCompare(payload, new byte[2]
      {
        (byte) 4,
        (byte) 33
      }, 2))
        return new SCGiCommand?(SCGiCommand.MessUnitNumberMax);
      if (Util.ByteArrayCompare(payload, new byte[2]
      {
        (byte) 4,
        (byte) 35
      }, 2))
        return new SCGiCommand?(SCGiCommand.MaxMessUnitNumberNotConfigured);
      if (Util.ByteArrayCompare(payload, new byte[2]
      {
        (byte) 4,
        (byte) 27
      }, 2))
        return new SCGiCommand?(SCGiCommand.Scenario);
      if (Util.ByteArrayCompare(payload, new byte[2]
      {
        (byte) 4,
        (byte) 5
      }, 2))
        return new SCGiCommand?(SCGiCommand.StartTestReception);
      if (Util.ByteArrayCompare(payload, new byte[2]
      {
        (byte) 4,
        (byte) 6
      }, 2))
        return new SCGiCommand?(SCGiCommand.TestReceptionResult);
      if (Util.ByteArrayCompare(payload, new byte[2]
      {
        (byte) 4,
        (byte) 4
      }, 2))
        return new SCGiCommand?(SCGiCommand.RegisterMessUnit);
      if (Util.ByteArrayCompare(payload, new byte[2]
      {
        (byte) 4,
        (byte) 12
      }, 2))
        return new SCGiCommand?(SCGiCommand.ResetConfiguration);
      if (Util.ByteArrayCompare(payload, new byte[2]
      {
        (byte) 4,
        (byte) 11
      }, 2))
        return new SCGiCommand?(SCGiCommand.StartNetworkSetup);
      if (Util.ByteArrayCompare(payload, new byte[2]
      {
        (byte) 4,
        (byte) 7
      }, 2))
        return new SCGiCommand?(SCGiCommand.DeleteMessUnit);
      if (Util.ByteArrayCompare(payload, new byte[2]
      {
        (byte) 4,
        (byte) 3
      }, 2))
        return new SCGiCommand?(SCGiCommand.InfoOfRegisteredMessUnit);
      if (Util.ByteArrayCompare(payload, new byte[2]
      {
        (byte) 4,
        (byte) 1
      }, 2))
        return new SCGiCommand?(SCGiCommand.RegisteredMessUnits);
      if (Util.ByteArrayCompare(payload, new byte[2]
      {
        (byte) 4,
        (byte) 18
      }, 2))
        return new SCGiCommand?(SCGiCommand.ForceNetworkOptimization);
      if (Util.ByteArrayCompare(payload, new byte[2]
      {
        (byte) 4,
        (byte) 25
      }, 2))
        return new SCGiCommand?(SCGiCommand.StartNetworkOptimization);
      if (Util.ByteArrayCompare(payload, new byte[2]
      {
        (byte) 4,
        (byte) 13
      }, 2))
        return new SCGiCommand?(SCGiCommand.RegisterOrDeregisterSlave);
      if (Util.ByteArrayCompare(payload, new byte[2]
      {
        (byte) 4,
        (byte) 14
      }, 2))
        return new SCGiCommand?(SCGiCommand.RegisteredSlaves);
      if (Util.ByteArrayCompare(payload, new byte[2]
      {
        (byte) 5,
        (byte) 1
      }, 2))
        return new SCGiCommand?(SCGiCommand.Flash);
      if (Util.ByteArrayCompare(payload, new byte[2]
      {
        (byte) 5,
        (byte) 2
      }, 2))
        return new SCGiCommand?(SCGiCommand.Eeprom);
      if (Util.ByteArrayCompare(payload, new byte[2]
      {
        (byte) 1,
        (byte) 0
      }, 2))
        return new SCGiCommand?(SCGiCommand.MeasurementData);
      if (Util.ByteArrayCompare(payload, new byte[2]
      {
        (byte) 4,
        (byte) 20
      }, 2))
        return new SCGiCommand?(SCGiCommand.SwitchToNetworkModel);
      if (Util.ByteArrayCompare(payload, new byte[2]
      {
        (byte) 1,
        (byte) 1
      }, 2))
        return new SCGiCommand?(SCGiCommand.MessUnitMetadata);
      if (Util.ByteArrayCompare(payload, new byte[2]
      {
        (byte) 5,
        (byte) 6
      }, 2))
        return new SCGiCommand?(SCGiCommand.LED);
      if (Util.ByteArrayCompare(payload, new byte[2]
      {
        (byte) 5,
        (byte) 0
      }, 2))
        return new SCGiCommand?(SCGiCommand.NodeId);
      if (Util.ByteArrayCompare(payload, new byte[2]
      {
        (byte) 6,
        (byte) 0
      }, 2))
        return new SCGiCommand?(SCGiCommand.NetworkId);
      if (Util.ByteArrayCompare(payload, new byte[2]
      {
        (byte) 12,
        (byte) 0
      }, 2))
        return new SCGiCommand?(SCGiCommand.SystemTime);
      if (Util.ByteArrayCompare(payload, new byte[2]
      {
        (byte) 9,
        (byte) 0
      }, 2))
        return new SCGiCommand?(SCGiCommand.TransceiverChannelId);
      if (Util.ByteArrayCompare(payload, new byte[2]
      {
        (byte) 2,
        (byte) 0
      }, 2))
        return new SCGiCommand?(SCGiCommand.FirmwareVersion);
      if (Util.ByteArrayCompare(payload, new byte[2]
      {
        (byte) 1,
        (byte) 1
      }, 2))
        return new SCGiCommand?(SCGiCommand.UserappName);
      if (Util.ByteArrayCompare(payload, new byte[2]
      {
        (byte) 3,
        (byte) 0
      }, 2))
        return new SCGiCommand?(SCGiCommand.FirmwareBuildTime);
      if (Util.ByteArrayCompare(payload, new byte[2]
      {
        (byte) 4,
        (byte) 0
      }, 2))
        return new SCGiCommand?(SCGiCommand.UserappBuildTime);
      if (Util.ByteArrayCompare(payload, new byte[2]
      {
        (byte) 7,
        (byte) 0
      }, 2))
        return new SCGiCommand?(SCGiCommand.ErrorFlags);
      if (Util.ByteArrayCompare(payload, new byte[2]
      {
        (byte) 8,
        (byte) 0
      }, 2))
        return new SCGiCommand?(SCGiCommand.TransmissionPower);
      if (Util.ByteArrayCompare(payload, new byte[2]
      {
        (byte) 10,
        (byte) 0
      }, 2))
        return new SCGiCommand?(SCGiCommand.MultiChannelSettings);
      if (Util.ByteArrayCompare(payload, new byte[2]
      {
        (byte) 11,
        (byte) 0
      }, 2))
        return new SCGiCommand?(SCGiCommand.TransceiverFrequencyOffset);
      if (Util.ByteArrayCompare(payload, new byte[2]
      {
        (byte) 13,
        (byte) 0
      }, 2))
        return new SCGiCommand?(SCGiCommand.TemperatureOffset);
      if (Util.ByteArrayCompare(payload, new byte[2]
      {
        (byte) 16,
        (byte) 0
      }, 2))
        return new SCGiCommand?(SCGiCommand.RestartMinomat);
      if (Util.ByteArrayCompare(payload, new byte[2]
      {
        (byte) 48,
        (byte) 0
      }, 2))
        return new SCGiCommand?(SCGiCommand.RoutingTable);
      if (Util.ByteArrayCompare(payload, new byte[2]
      {
        (byte) 15,
        (byte) 3
      }, 2))
        return new SCGiCommand?(SCGiCommand.GetComServerFile);
      if (Util.ByteArrayCompare(payload, new byte[2]
      {
        (byte) 15,
        (byte) 4
      }, 2))
        return new SCGiCommand?(SCGiCommand.SetComServerFile);
      if (Util.ByteArrayCompare(payload, new byte[1]
      {
        (byte) 4
      }, 1))
        return new SCGiCommand?(SCGiCommand.MasterMinolID);
      if (Util.ByteArrayCompare(payload, new byte[1]
      {
        (byte) 193
      }, 1))
        return new SCGiCommand?(SCGiCommand.SimPin);
      if (Util.ByteArrayCompare(payload, new byte[1]
      {
        (byte) 194
      }, 1))
        return new SCGiCommand?(SCGiCommand.APN);
      if (Util.ByteArrayCompare(payload, new byte[1]
      {
        (byte) 195
      }, 1))
        return new SCGiCommand?(SCGiCommand.GPRSUserName);
      if (Util.ByteArrayCompare(payload, new byte[1]
      {
        (byte) 196
      }, 1))
        return new SCGiCommand?(SCGiCommand.GPRSPassword);
      if (Util.ByteArrayCompare(payload, new byte[1]
      {
        (byte) 202
      }, 1))
        return new SCGiCommand?(SCGiCommand.HttpServer);
      if (Util.ByteArrayCompare(payload, new byte[1]
      {
        (byte) 203
      }, 1))
        return new SCGiCommand?(SCGiCommand.HttpResourceName);
      if (Util.ByteArrayCompare(payload, new byte[1]
      {
        (byte) 10
      }, 1))
        return new SCGiCommand?(SCGiCommand.StartGSMTestReception);
      if (Util.ByteArrayCompare(payload, new byte[1]
      {
        (byte) 169
      }, 1))
        return new SCGiCommand?(SCGiCommand.GSMTestReceptionState);
      if (Util.ByteArrayCompare(payload, new byte[1]
      {
        (byte) 49
      }, 1))
        return new SCGiCommand?(SCGiCommand.AppInitialSettings);
      if (Util.ByteArrayCompare(payload, new byte[1]
      {
        (byte) 208
      }, 1))
        return new SCGiCommand?(SCGiCommand.ActionTimepoint);
      if (Util.ByteArrayCompare(payload, new byte[1]
      {
        (byte) 167
      }, 1))
        return new SCGiCommand?(SCGiCommand.HttpState);
      if (Util.ByteArrayCompare(payload, new byte[1]
      {
        (byte) 160
      }, 1))
        return new SCGiCommand?(SCGiCommand.GsmState);
      if (Util.ByteArrayCompare(payload, new byte[1]
      {
        (byte) 170
      }, 1))
        return new SCGiCommand?(SCGiCommand.ModemBuildDate);
      if (Util.ByteArrayCompare(payload, new byte[1]
      {
        (byte) 9
      }, 1))
        return new SCGiCommand?(SCGiCommand.StartHttpConnection);
      if (Util.ByteArrayCompare(payload, new byte[1]
      {
        (byte) 8
      }, 1))
        return new SCGiCommand?(SCGiCommand.ActivateModemAT_Mode);
      if (Util.ByteArrayCompare(payload, new byte[1]
      {
        (byte) 236
      }, 1))
        return new SCGiCommand?(SCGiCommand.ModemUpdateTest);
      if (Util.ByteArrayCompare(payload, new byte[1]
      {
        (byte) 237
      }, 1))
        return new SCGiCommand?(SCGiCommand.ModemUpdateTiming);
      if (Util.ByteArrayCompare(payload, new byte[1]
      {
        (byte) 238
      }, 1))
        return new SCGiCommand?(SCGiCommand.ModemUpdateImageClear);
      if (Util.ByteArrayCompare(payload, new byte[1]
      {
        (byte) 239
      }, 1))
        return new SCGiCommand?(SCGiCommand.ModemUpdate);
      if (Util.ByteArrayCompare(payload, new byte[1]
      {
        (byte) 241
      }, 1))
        return new SCGiCommand?(SCGiCommand.ModemReboot);
      if (Util.ByteArrayCompare(payload, new byte[1]
      {
        (byte) 223
      }, 1))
        return new SCGiCommand?(SCGiCommand.ModemCounter);
      if (Util.ByteArrayCompare(payload, new byte[1]
      {
        (byte) 197
      }, 1))
        return new SCGiCommand?(SCGiCommand.TcpConfiguration);
      if (Util.ByteArrayCompare(payload, new byte[1]
      {
        (byte) 1
      }, 1))
        return new SCGiCommand?(SCGiCommand.SendSMS);
      if (Util.ByteArrayCompare(payload, new byte[1]
      {
        (byte) 204
      }, 1))
        return new SCGiCommand?(SCGiCommand.ModemDueDate);
      return Util.ByteArrayCompare(payload, new byte[1]
      {
        (byte) 168
      }, 1) ? new SCGiCommand?(SCGiCommand.GsmLinkQuality) : new SCGiCommand?();
    }

    internal static string GetAsHexString(SCGiCommand cmd)
    {
      byte[] bytes = SCGiCommandManager.GetBytes(cmd);
      return bytes == null || bytes.Length == 0 ? string.Empty : Util.ByteArrayToHexString(bytes);
    }
  }
}
