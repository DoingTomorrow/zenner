// Decompiled with JetBrains decompiler
// Type: MinomatHandler.MinomatV4Settings
// Assembly: MinomatHandler, Version=1.0.3.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 7EF7C01F-958A-42C5-BD1F-5A50D1BCE76C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MinomatHandler.dll

using System.Collections.Generic;
using System.Text;

#nullable disable
namespace MinomatHandler
{
  public sealed class MinomatV4Settings : SortedList<SCGiCommand, MinomatV4Parameter>
  {
    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      if (this.ContainsKey(SCGiCommand.UserappName))
        stringBuilder.Append((object) this[SCGiCommand.UserappName]);
      if (this.ContainsKey(SCGiCommand.FirmwareVersion))
        stringBuilder.Append((object) this[SCGiCommand.FirmwareVersion]);
      if (this.ContainsKey(SCGiCommand.FirmwareBuildTime))
        stringBuilder.Append((object) this[SCGiCommand.FirmwareBuildTime]);
      if (this.ContainsKey(SCGiCommand.UserappBuildTime))
        stringBuilder.Append((object) this[SCGiCommand.UserappBuildTime]);
      if (this.ContainsKey(SCGiCommand.NodeId))
        stringBuilder.Append((object) this[SCGiCommand.NodeId]);
      if (this.ContainsKey(SCGiCommand.NetworkId))
        stringBuilder.Append((object) this[SCGiCommand.NetworkId]);
      if (this.ContainsKey(SCGiCommand.ErrorFlags))
        stringBuilder.Append((object) this[SCGiCommand.ErrorFlags]);
      if (this.ContainsKey(SCGiCommand.TransmissionPower))
        stringBuilder.Append((object) this[SCGiCommand.TransmissionPower]);
      if (this.ContainsKey(SCGiCommand.TransceiverChannelId))
        stringBuilder.Append((object) this[SCGiCommand.TransceiverChannelId]);
      if (this.ContainsKey(SCGiCommand.MultiChannelSettings))
        stringBuilder.Append((object) this[SCGiCommand.MultiChannelSettings]);
      if (this.ContainsKey(SCGiCommand.TransceiverFrequencyOffset))
        stringBuilder.Append((object) this[SCGiCommand.TransceiverFrequencyOffset]);
      if (this.ContainsKey(SCGiCommand.SystemTime))
        stringBuilder.Append((object) this[SCGiCommand.SystemTime]);
      if (this.ContainsKey(SCGiCommand.TemperatureOffset))
        stringBuilder.Append((object) this[SCGiCommand.TemperatureOffset]);
      if (this.ContainsKey(SCGiCommand.ResetConfigurationState))
        stringBuilder.Append((object) this[SCGiCommand.ResetConfigurationState]);
      if (this.ContainsKey(SCGiCommand.RegisteredMessUnits))
        stringBuilder.Append((object) this[SCGiCommand.RegisteredMessUnits]);
      if (this.ContainsKey(SCGiCommand.TestReceptionResult))
        stringBuilder.Append((object) this[SCGiCommand.TestReceptionResult]);
      if (this.ContainsKey(SCGiCommand.RadioChannel))
        stringBuilder.Append((object) this[SCGiCommand.RadioChannel]);
      if (this.ContainsKey(SCGiCommand.PhaseDetailsBuffer))
        stringBuilder.Append((object) this[SCGiCommand.PhaseDetailsBuffer]);
      if (this.ContainsKey(SCGiCommand.RegisteredSlavesIntegrated))
        stringBuilder.Append((object) this[SCGiCommand.RegisteredSlavesIntegrated]);
      if (this.ContainsKey(SCGiCommand.RegisteredSlavesNotIntegrated))
        stringBuilder.Append((object) this[SCGiCommand.RegisteredSlavesNotIntegrated]);
      if (this.ContainsKey(SCGiCommand.MinolId))
        stringBuilder.Append((object) this[SCGiCommand.MinolId]);
      if (this.ContainsKey(SCGiCommand.PhaseDetails))
        stringBuilder.Append((object) this[SCGiCommand.PhaseDetails]);
      if (this.ContainsKey(SCGiCommand.MessUnitNumberMax))
        stringBuilder.Append((object) this[SCGiCommand.MessUnitNumberMax]);
      if (this.ContainsKey(SCGiCommand.MaxMessUnitNumberNotConfigured))
        stringBuilder.Append((object) this[SCGiCommand.MaxMessUnitNumberNotConfigured]);
      if (this.ContainsKey(SCGiCommand.Scenario))
        stringBuilder.Append((object) this[SCGiCommand.Scenario]);
      if (this.ContainsKey(SCGiCommand.RoutingTable))
        stringBuilder.Append((object) this[SCGiCommand.RoutingTable]);
      if (this.ContainsKey(SCGiCommand.MasterMinolID))
        stringBuilder.Append(this[SCGiCommand.MasterMinolID].ToString());
      if (this.ContainsKey(SCGiCommand.GsmState))
        stringBuilder.Append(this[SCGiCommand.GsmState].ToString());
      if (this.ContainsKey(SCGiCommand.HttpState))
        stringBuilder.Append(this[SCGiCommand.HttpState].ToString());
      if (this.ContainsKey(SCGiCommand.GSMTestReceptionState))
        stringBuilder.Append(this[SCGiCommand.GSMTestReceptionState].ToString());
      if (this.ContainsKey(SCGiCommand.AppInitialSettings))
        stringBuilder.Append(this[SCGiCommand.AppInitialSettings].ToString());
      if (this.ContainsKey(SCGiCommand.SimPin))
        stringBuilder.Append(this[SCGiCommand.SimPin].ToString());
      if (this.ContainsKey(SCGiCommand.APN))
        stringBuilder.Append(this[SCGiCommand.APN].ToString());
      if (this.ContainsKey(SCGiCommand.GPRSUserName))
        stringBuilder.Append(this[SCGiCommand.GPRSUserName].ToString());
      if (this.ContainsKey(SCGiCommand.GPRSPassword))
        stringBuilder.Append(this[SCGiCommand.GPRSPassword].ToString());
      if (this.ContainsKey(SCGiCommand.HttpServer))
        stringBuilder.Append(this[SCGiCommand.HttpServer].ToString());
      if (this.ContainsKey(SCGiCommand.HttpResourceName))
        stringBuilder.Append(this[SCGiCommand.HttpResourceName].ToString());
      if (this.ContainsKey(SCGiCommand.ModemBuildDate))
        stringBuilder.Append(this[SCGiCommand.ModemBuildDate].ToString());
      return stringBuilder.ToString();
    }
  }
}
