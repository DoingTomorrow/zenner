// Decompiled with JetBrains decompiler
// Type: EQATEC.Analytics.Monitor.Policy.PolicyXmlSerializer
// Assembly: EQATEC.Analytics.Monitor, Version=3.2.1.0, Culture=neutral, PublicKeyToken=213c7c68adb58a17
// MVID: 227B2302-8342-4A73-A9B6-18C9F29BF2BB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EQATEC.Analytics.Monitor.dll

using System;
using System.Xml;

#nullable disable
namespace EQATEC.Analytics.Monitor.Policy
{
  internal class PolicyXmlSerializer
  {
    public static void SerializeStatistics(XmlWriter xtw, MonitorPolicy policy)
    {
      XmlUtil.WriteStartElement(xtw, "Stats", "startCount", policy.RuntimeStatus.SessionStartCount.ToString(), "resId", policy.DataTypeRestrictions.RestrictionsVersion.ToString());
      xtw.WriteEndElement();
    }

    public static void DeserializeStatistics(XmlReader xr, MonitorPolicy policy)
    {
      int num1 = 0;
      while (xr.Read())
      {
        if (xr.NodeType == XmlNodeType.Element)
          ++num1;
        if (xr.NodeType == XmlNodeType.Element && num1 == 1)
        {
          switch (xr.Name)
          {
            case "Stats":
              int num2;
              if (Parser.TryParseInt(xr.GetAttribute("startCount"), out num2))
                policy.RuntimeStatus.SessionStartCount = num2;
              int num3;
              if (Parser.TryParseInt(xr.GetAttribute("resId"), out num3))
              {
                policy.DataTypeRestrictions.RestrictionsVersion = num3;
                continue;
              }
              continue;
            default:
              continue;
          }
        }
      }
    }

    internal static void Serialize(
      XmlWriter xtw,
      MonitorPolicy policy,
      DateTime time,
      TimeSpan uptime)
    {
      XmlUtil.WriteStartElement(xtw, "Policy");
      XmlUtil.WriteStartElement(xtw, "Info", "alturi", policy.Info.AlternativeUri == (Uri) null ? (string) null : policy.Info.AlternativeUri.ToString(), "cookie", policy.Info.Cookie, "localid", policy.Info.LocalIdentifier);
      xtw.WriteEndElement();
      XmlUtil.WriteStartElement(xtw, "Data", "version", policy.DataTypeRestrictions.RestrictionsVersion.ToString());
      PolicyXmlSerializer.WriteBlockingRestriction(xtw, "Sessions", time, uptime, policy.DataTypeRestrictions.Sessions);
      PolicyXmlSerializer.WriteBlockingRestriction(xtw, "Exceptions", time, uptime, policy.DataTypeRestrictions.Exceptions);
      PolicyXmlSerializer.WriteBlockingRestriction(xtw, "FeatureUsages", time, uptime, policy.DataTypeRestrictions.FeatureUsages);
      PolicyXmlSerializer.WriteBlockingRestriction(xtw, "FeatureValues", time, uptime, policy.DataTypeRestrictions.FeatureValues);
      PolicyXmlSerializer.WriteBlockingRestriction(xtw, "FeatureTiming", time, uptime, policy.DataTypeRestrictions.FeatureTiming);
      xtw.WriteEndElement();
      PolicyXmlSerializer.WriteBlockingRestriction(xtw, "Transmission", time, uptime, policy.TransmissionBlocking);
      SettingsRestrictions settingsRestrictions = policy.SettingsRestrictions;
      XmlUtil.WriteStartElement(xtw, "Settings", "version", settingsRestrictions.SettingsVersion.ToString());
      PolicyXmlSerializer.WriteSettingsChange<int>(xtw, "maxStackTrace", settingsRestrictions.MaxExceptionStackTrace);
      PolicyXmlSerializer.WriteSettingsChange<int>(xtw, "maxExceptionExtraInfo", settingsRestrictions.MaxExceptionExtraInfo);
      PolicyXmlSerializer.WriteSettingsChange<int>(xtw, "maxExceptionMessage", settingsRestrictions.MaxExceptionMessage);
      PolicyXmlSerializer.WriteSettingsChange<int>(xtw, "maxNestedExceptions", settingsRestrictions.MaxNestedExceptions);
      PolicyXmlSerializer.WriteSettingsChange<int>(xtw, "maxFeatureIdSize", settingsRestrictions.MaxFeatureIdSize);
      PolicyXmlSerializer.WriteSettingsChange<int>(xtw, "maxSessions", settingsRestrictions.MaxSessions);
      PolicyXmlSerializer.WriteSettingsChange<int>(xtw, "maxSessionExceptions", settingsRestrictions.MaxSessionExceptions);
      PolicyXmlSerializer.WriteSettingsChange<int>(xtw, "maxSessionFeatureValues", settingsRestrictions.MaxSessionFeatureValues);
      PolicyXmlSerializer.WriteSettingsChange<int>(xtw, "maxInstallationPropertyKeySize", settingsRestrictions.MaxInstallationPropertyKeySize);
      PolicyXmlSerializer.WriteSettingsChange<int>(xtw, "maxInstallationProperties", settingsRestrictions.MaxInstallationProperties);
      PolicyXmlSerializer.WriteSettingsChange<int>(xtw, "maxInstallationSettingsHashMapToStore", settingsRestrictions.MaxInstallationSettingsHashMapToStore);
      PolicyXmlSerializer.WriteSettingsChange<int>(xtw, "maxStorageSizeInKB", settingsRestrictions.MaxStorageSizeInKB);
      PolicyXmlSerializer.WriteSettingsChange<int>(xtw, "maxInstallationIDSize", settingsRestrictions.MaxInstallationIDSize);
      PolicyXmlSerializer.WriteSettingsChange<int>(xtw, "maxMessageAttempts", settingsRestrictions.MaxMessageAttempts);
      PolicyXmlSerializer.WriteSettingsChange<TimeSpan>(xtw, "retrySendInterval", settingsRestrictions.RetrySendInterval, (PolicyXmlSerializer.Stringify<TimeSpan>) (r => r.Ticks.ToString()));
      PolicyXmlSerializer.WriteSettingsChange<TimeSpan>(xtw, "autoSendInterval", settingsRestrictions.AutoSendInterval, (PolicyXmlSerializer.Stringify<TimeSpan>) (r => r.Ticks.ToString()));
      PolicyXmlSerializer.WriteSettingsChange<int>(xtw, "maxBandwidthUsagePerDayInKB", settingsRestrictions.MaxBandwidthUsagePerDayInKB);
      PolicyXmlSerializer.WriteSettingsChange<int>(xtw, "minSecondsBetweenForceSync", settingsRestrictions.MinSecondsBetweenForceSync);
      PolicyXmlSerializer.WriteSettingsChange<int>(xtw, "maxDataPayloadSizeKB", settingsRestrictions.MaxDataPayloadSizeKB);
      PolicyXmlSerializer.WriteSettingsChange<int>(xtw, "maxFlowNameLength", settingsRestrictions.MaxFlowNameLength);
      PolicyXmlSerializer.WriteSettingsChange<int>(xtw, "maxWaypointNameLength", settingsRestrictions.MaxWaypointNameLength);
      PolicyXmlSerializer.WriteSettingsChange<int>(xtw, "maxLicenseIDSize", settingsRestrictions.MaxLicenseIDSize);
      PolicyXmlSerializer.WriteSettingsChange<int>(xtw, "maxLicenseTypeSize", settingsRestrictions.MaxLicenseTypeSize);
      PolicyXmlSerializer.WriteSettingsChange<int>(xtw, "maxNumberOfLicenseInfos", settingsRestrictions.MaxNumberOfLicenseInfos);
      PolicyXmlSerializer.WriteSettingsChange<int>(xtw, "maxFlowQueueLength", settingsRestrictions.MaxFlowQueueLength);
      PolicyXmlSerializer.WriteSettingsChange<int>(xtw, "maxFlowsInSession", settingsRestrictions.MaxFlowsInSession);
      PolicyXmlSerializer.WriteSettingsChange<int>(xtw, "maxGoalsInFlow", settingsRestrictions.MaxGoalsInFlow);
      PolicyXmlSerializer.WriteSettingsChange<int>(xtw, "maxTransitionsInFlow", settingsRestrictions.MaxTransitionsInFlow);
      xtw.WriteEndElement();
      xtw.WriteEndElement();
    }

    private static void WriteSettingsChange<T>(
      XmlWriter writer,
      string name,
      SettingsValue<T> value)
      where T : struct
    {
      PolicyXmlSerializer.WriteSettingsChange<T>(writer, name, value, (PolicyXmlSerializer.Stringify<T>) (v => v.ToString()));
    }

    private static void WriteSettingsChange<T>(
      XmlWriter writer,
      string name,
      SettingsValue<T> value,
      PolicyXmlSerializer.Stringify<T> stringify)
      where T : struct
    {
      if (value.Value.Equals((object) value.Default))
        return;
      writer.WriteAttributeString(name, stringify(value.Value));
    }

    private static void WriteBlockingRestriction(
      XmlWriter xtw,
      string name,
      DateTime time,
      TimeSpan uptime,
      BlockingRestriction restriction)
    {
      if (!restriction.IsBlocking(uptime))
        return;
      DateTime dateTime1 = time + (restriction.BlockingStart - uptime);
      DateTime dateTime2 = time + (restriction.BlockingUntil - uptime);
      XmlUtil.WriteFullElement(xtw, name, "blockstart", dateTime1.Ticks.ToString(), "blockuntil", dateTime2.Ticks.ToString(), "type", ((int) restriction.BlockingType).ToString());
    }

    private static void ReadBlockingRestriction(
      XmlReader reader,
      DateTime time,
      TimeSpan uptime,
      BlockingRestriction restriction)
    {
      string attribute1 = reader.GetAttribute("blockstart");
      string attribute2 = reader.GetAttribute("blockuntil");
      string attribute3 = reader.GetAttribute("type");
      long ticks1;
      long ticks2;
      int num;
      if (!Parser.TryParseLong(attribute1, out ticks1) || !Parser.TryParseLong(attribute2, out ticks2) || !Parser.TryParseInt(attribute3, out num))
        return;
      DateTime dateTime1 = new DateTime(ticks1);
      DateTime dateTime2 = new DateTime(ticks2);
      restriction.BlockingStart = dateTime1 != DateTime.MinValue ? uptime + (dateTime1 - time) : TimeSpan.MinValue;
      restriction.BlockingUntil = dateTime1 != DateTime.MinValue ? uptime + (dateTime2 - time) : TimeSpan.MinValue;
      restriction.BlockingType = (BlockingRestriction.BlockingTypes) num;
    }

    internal static void Deserialize(
      XmlReader xr,
      MonitorPolicy policy,
      DateTime time,
      TimeSpan uptime)
    {
      int num = 0;
      while (xr.Read())
      {
        if (xr.NodeType == XmlNodeType.Element)
          ++num;
        if (xr.NodeType == XmlNodeType.Element)
        {
          switch (xr.Name)
          {
            case "Info":
              PolicyXmlSerializer.FromAttribute(xr, "alturi", (Action<string>) (s => policy.Info.AlternativeUri = new Uri(s)));
              PolicyXmlSerializer.FromAttribute(xr, "cookie", (Action<string>) (s => policy.Info.Cookie = s));
              PolicyXmlSerializer.FromAttribute(xr, "localid", (Action<string>) (s => policy.Info.LocalIdentifier = s));
              break;
            case "Data":
              PolicyXmlSerializer.FromAttribute(xr, "version", (Action<string>) (s => policy.DataTypeRestrictions.RestrictionsVersion = int.Parse(s)));
              break;
            case "Sessions":
              PolicyXmlSerializer.ReadBlockingRestriction(xr, time, uptime, policy.DataTypeRestrictions.Sessions);
              break;
            case "Exceptions":
              PolicyXmlSerializer.ReadBlockingRestriction(xr, time, uptime, policy.DataTypeRestrictions.Exceptions);
              break;
            case "FeatureUsages":
              PolicyXmlSerializer.ReadBlockingRestriction(xr, time, uptime, policy.DataTypeRestrictions.FeatureUsages);
              break;
            case "FeatureValues":
              PolicyXmlSerializer.ReadBlockingRestriction(xr, time, uptime, policy.DataTypeRestrictions.FeatureValues);
              break;
            case "FeatureTiming":
              PolicyXmlSerializer.ReadBlockingRestriction(xr, time, uptime, policy.DataTypeRestrictions.FeatureTiming);
              break;
            case "Transmission":
              PolicyXmlSerializer.ReadBlockingRestriction(xr, time, uptime, policy.TransmissionBlocking);
              break;
            case "Settings":
              PolicyXmlSerializer.FromAttribute(xr, "version", (Action<string>) (s => policy.SettingsRestrictions.SettingsVersion = int.Parse(s)));
              PolicyXmlSerializer.FromAttribute(xr, "maxStackTrace", (Action<string>) (s => policy.SettingsRestrictions.MaxExceptionStackTrace.SetValue(int.Parse(s))));
              PolicyXmlSerializer.FromAttribute(xr, "maxExceptionExtraInfo", (Action<string>) (s => policy.SettingsRestrictions.MaxExceptionExtraInfo.SetValue(int.Parse(s))));
              PolicyXmlSerializer.FromAttribute(xr, "maxExceptionMessage", (Action<string>) (s => policy.SettingsRestrictions.MaxExceptionMessage.SetValue(int.Parse(s))));
              PolicyXmlSerializer.FromAttribute(xr, "maxNestedExceptions", (Action<string>) (s => policy.SettingsRestrictions.MaxNestedExceptions.SetValue(int.Parse(s))));
              PolicyXmlSerializer.FromAttribute(xr, "maxFeatureIdSize", (Action<string>) (s => policy.SettingsRestrictions.MaxFeatureIdSize.SetValue(int.Parse(s))));
              PolicyXmlSerializer.FromAttribute(xr, "maxSessions", (Action<string>) (s => policy.SettingsRestrictions.MaxSessions.SetValue(int.Parse(s))));
              PolicyXmlSerializer.FromAttribute(xr, "maxSessionExceptions", (Action<string>) (s => policy.SettingsRestrictions.MaxSessionExceptions.SetValue(int.Parse(s))));
              PolicyXmlSerializer.FromAttribute(xr, "maxSessionFeatureValues", (Action<string>) (s => policy.SettingsRestrictions.MaxSessionFeatureValues.SetValue(int.Parse(s))));
              PolicyXmlSerializer.FromAttribute(xr, "maxInstallationPropertyKeySize", (Action<string>) (s => policy.SettingsRestrictions.MaxInstallationPropertyKeySize.SetValue(int.Parse(s))));
              PolicyXmlSerializer.FromAttribute(xr, "maxInstallationProperties", (Action<string>) (s => policy.SettingsRestrictions.MaxInstallationProperties.SetValue(int.Parse(s))));
              PolicyXmlSerializer.FromAttribute(xr, "maxInstallationSettingsHashMapToStore", (Action<string>) (s => policy.SettingsRestrictions.MaxInstallationSettingsHashMapToStore.SetValue(int.Parse(s))));
              PolicyXmlSerializer.FromAttribute(xr, "maxStorageSizeInKB", (Action<string>) (s => policy.SettingsRestrictions.MaxStorageSizeInKB.SetValue(int.Parse(s))));
              PolicyXmlSerializer.FromAttribute(xr, "maxInstallationIDSize", (Action<string>) (s => policy.SettingsRestrictions.MaxInstallationIDSize.SetValue(int.Parse(s))));
              PolicyXmlSerializer.FromAttribute(xr, "maxMessageAttempts", (Action<string>) (s => policy.SettingsRestrictions.MaxMessageAttempts.SetValue(int.Parse(s))));
              PolicyXmlSerializer.FromAttribute(xr, "retrySendInterval", (Action<string>) (s => policy.SettingsRestrictions.RetrySendInterval.SetValue(new TimeSpan(long.Parse(s)))));
              PolicyXmlSerializer.FromAttribute(xr, "autoSendInterval", (Action<string>) (s => policy.SettingsRestrictions.AutoSendInterval.SetValue(new TimeSpan(long.Parse(s)))));
              PolicyXmlSerializer.FromAttribute(xr, "maxBandwidthUsagePerDayInKB", (Action<string>) (s => policy.SettingsRestrictions.MaxBandwidthUsagePerDayInKB.SetValue(int.Parse(s))));
              PolicyXmlSerializer.FromAttribute(xr, "minSecondsBetweenForceSync", (Action<string>) (s => policy.SettingsRestrictions.MinSecondsBetweenForceSync.SetValue(int.Parse(s))));
              PolicyXmlSerializer.FromAttribute(xr, "maxDataPayloadSizeKB", (Action<string>) (s => policy.SettingsRestrictions.MaxDataPayloadSizeKB.SetValue(int.Parse(s))));
              PolicyXmlSerializer.FromAttribute(xr, "maxFlowNameLength", (Action<string>) (s => policy.SettingsRestrictions.MaxFlowNameLength.SetValue(int.Parse(s))));
              PolicyXmlSerializer.FromAttribute(xr, "maxWaypointNameLength", (Action<string>) (s => policy.SettingsRestrictions.MaxWaypointNameLength.SetValue(int.Parse(s))));
              PolicyXmlSerializer.FromAttribute(xr, "maxLicenseIDSize", (Action<string>) (s => policy.SettingsRestrictions.MaxLicenseIDSize.SetValue(int.Parse(s))));
              PolicyXmlSerializer.FromAttribute(xr, "maxLicenseTypeSize", (Action<string>) (s => policy.SettingsRestrictions.MaxLicenseTypeSize.SetValue(int.Parse(s))));
              PolicyXmlSerializer.FromAttribute(xr, "maxNumberOfLicenseInfos", (Action<string>) (s => policy.SettingsRestrictions.MaxNumberOfLicenseInfos.SetValue(int.Parse(s))));
              PolicyXmlSerializer.FromAttribute(xr, "maxFlowQueueLength", (Action<string>) (s => policy.SettingsRestrictions.MaxFlowQueueLength.SetValue(int.Parse(s))));
              PolicyXmlSerializer.FromAttribute(xr, "maxFlowsInSession", (Action<string>) (s => policy.SettingsRestrictions.MaxFlowsInSession.SetValue(int.Parse(s))));
              PolicyXmlSerializer.FromAttribute(xr, "maxGoalsInFlow", (Action<string>) (s => policy.SettingsRestrictions.MaxGoalsInFlow.SetValue(int.Parse(s))));
              PolicyXmlSerializer.FromAttribute(xr, "maxTransitionsInFlow", (Action<string>) (s => policy.SettingsRestrictions.MaxTransitionsInFlow.SetValue(int.Parse(s))));
              break;
          }
        }
        if (xr.NodeType == XmlNodeType.EndElement || xr.NodeType == XmlNodeType.Element && xr.IsEmptyElement)
          --num;
      }
    }

    private static void FromAttribute(
      XmlReader reader,
      string attributeName,
      Action<string> action)
    {
      string attribute = reader.GetAttribute(attributeName);
      if (string.IsNullOrEmpty(attribute) || action == null)
        return;
      action(attribute);
    }

    private delegate string Stringify<T>(T val) where T : struct;
  }
}
