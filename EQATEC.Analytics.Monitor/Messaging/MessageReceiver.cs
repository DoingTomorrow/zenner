// Decompiled with JetBrains decompiler
// Type: EQATEC.Analytics.Monitor.Messaging.MessageReceiver
// Assembly: EQATEC.Analytics.Monitor, Version=3.2.1.0, Culture=neutral, PublicKeyToken=213c7c68adb58a17
// MVID: 227B2302-8342-4A73-A9B6-18C9F29BF2BB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EQATEC.Analytics.Monitor.dll

using EQATEC.Analytics.Monitor.Policy;
using System;
using System.IO;
using System.Xml;

#nullable disable
namespace EQATEC.Analytics.Monitor.Messaging
{
  internal class MessageReceiver : IMessageReceiver
  {
    private const string RESET_TOKEN = "#reset#";
    private readonly ILogAnalyticsMonitor m_log;
    private readonly int m_messageFactoryVersion;
    private MonitorPolicy m_policy;

    public MessageReceiver(
      int messageFactoryVersion,
      ILogAnalyticsMonitor log,
      MonitorPolicy policy)
    {
      this.m_messageFactoryVersion = messageFactoryVersion;
      this.m_log = log;
      this.m_policy = Guard.IsNotNull<MonitorPolicy>(policy, nameof (policy));
    }

    public SendMessageResult ParseResponse(MessageResponse messageResponse)
    {
      if (messageResponse != null)
      {
        if (messageResponse.ResponsePayload != null)
        {
          try
          {
            using (XmlReader xr = XmlReader.Create((TextReader) new StringReader(messageResponse.ResponsePayload)))
            {
              bool flag1 = false;
              string str = "";
              int num1 = 0;
              bool flag2 = false;
              bool flag3 = false;
              int num2 = 0;
              while (xr.Read())
              {
                if (xr.NodeType == XmlNodeType.Element)
                  ++num2;
                if (num2 == 1 && xr.NodeType == XmlNodeType.Element)
                {
                  flag3 = xr.GetAttribute("status") == "success";
                  str = !flag3 ? xr.GetAttribute("message") ?? "" : (string) null;
                  string attribute = xr.GetAttribute("ver");
                  if (Parser.TryParseInt(attribute, out num1) && num1 != this.m_messageFactoryVersion)
                    return new SendMessageResult()
                    {
                      Message = string.Format("Server responded with wrong version: expected {0} but got {1}", (object) this.m_messageFactoryVersion, string.IsNullOrEmpty(attribute) ? (object) "nothing" : (object) attribute),
                      Result = SendResult.Failure,
                      ShouldResend = false
                    };
                }
                if (num2 == 2 && xr.NodeType == XmlNodeType.Element)
                {
                  if (xr.Name == "Redirecting")
                  {
                    string attribute = xr.GetAttribute("uri");
                    this.m_policy.Info.AlternativeUri = !string.IsNullOrEmpty(attribute) ? new Uri(attribute) : (Uri) null;
                    flag2 = true;
                    flag1 = true;
                  }
                  else if (xr.Name == "Blocking")
                    this.m_policy.TransmissionBlocking.SetBlockingTime(Timing.Uptime, TimeSpan.FromMinutes(double.Parse(xr.GetAttribute("inMinutes"))), BlockingRestriction.BlockingTypes.Other);
                  else if (xr.Name == "Cookie")
                  {
                    this.m_policy.Info.Cookie = XmlUtil.GetInnerText(xr);
                    flag1 = true;
                  }
                  else if (xr.Name == "Restriction")
                  {
                    int num3 = int.Parse(xr.GetAttribute("id"));
                    if (num3 > this.m_policy.DataTypeRestrictions.RestrictionsVersion)
                    {
                      this.m_policy.DataTypeRestrictions.RestrictionsVersion = num3;
                      flag1 = true;
                      this.UpdateBlockingRestrictions(xr, this.m_policy.DataTypeRestrictions.Sessions, "Session");
                      this.UpdateBlockingRestrictions(xr, this.m_policy.DataTypeRestrictions.Exceptions, "Exception");
                      this.UpdateBlockingRestrictions(xr, this.m_policy.DataTypeRestrictions.FeatureTiming, "FeatureTiming");
                      this.UpdateBlockingRestrictions(xr, this.m_policy.DataTypeRestrictions.FeatureValues, "FeatureValue");
                      this.UpdateBlockingRestrictions(xr, this.m_policy.DataTypeRestrictions.FeatureUsages, "Feature");
                    }
                  }
                  else if (xr.Name == "SettingsChange")
                  {
                    int num4 = int.Parse(xr.GetAttribute("id"));
                    if (num4 > this.m_policy.SettingsRestrictions.SettingsVersion)
                    {
                      this.m_policy.SettingsRestrictions.SettingsVersion = num4;
                      flag1 = true;
                    }
                    string value = xr.GetAttribute("value");
                    string attribute = xr.GetAttribute("name");
                    SettingsRestrictions settingsRestrictions = this.m_policy.SettingsRestrictions;
                    bool reset = value.Equals("#reset#", StringComparison.InvariantCultureIgnoreCase);
                    switch (attribute)
                    {
                      case "AutoSendInterval":
                        MessageReceiver.FromAttribute<TimeSpan>(reset, value, settingsRestrictions.AutoSendInterval, (MessageReceiver.ParseFunc<TimeSpan>) (s => TimeSpan.FromMinutes(double.Parse(value))));
                        break;
                      case "MaxBandwidthUsagePerDayInKB":
                        MessageReceiver.FromAttribute<int>(reset, value, settingsRestrictions.MaxBandwidthUsagePerDayInKB, new MessageReceiver.ParseFunc<int>(int.Parse));
                        break;
                      case "MaxExceptionExtraInfo":
                        MessageReceiver.FromAttribute<int>(reset, value, settingsRestrictions.MaxExceptionExtraInfo, new MessageReceiver.ParseFunc<int>(int.Parse));
                        break;
                      case "MaxNestedExceptions":
                        MessageReceiver.FromAttribute<int>(reset, value, settingsRestrictions.MaxNestedExceptions, new MessageReceiver.ParseFunc<int>(int.Parse));
                        break;
                      case "MaxExceptionMessage":
                        MessageReceiver.FromAttribute<int>(reset, value, settingsRestrictions.MaxExceptionMessage, new MessageReceiver.ParseFunc<int>(int.Parse));
                        break;
                      case "MaxExceptionStackTrace":
                        MessageReceiver.FromAttribute<int>(reset, value, settingsRestrictions.MaxExceptionStackTrace, new MessageReceiver.ParseFunc<int>(int.Parse));
                        break;
                      case "MaxFeatureIdSize":
                        MessageReceiver.FromAttribute<int>(reset, value, settingsRestrictions.MaxFeatureIdSize, new MessageReceiver.ParseFunc<int>(int.Parse));
                        break;
                      case "MaxInstallationProperties":
                        MessageReceiver.FromAttribute<int>(reset, value, settingsRestrictions.MaxInstallationProperties, new MessageReceiver.ParseFunc<int>(int.Parse));
                        break;
                      case "MaxInstallationPropertyKeySize":
                        MessageReceiver.FromAttribute<int>(reset, value, settingsRestrictions.MaxInstallationPropertyKeySize, new MessageReceiver.ParseFunc<int>(int.Parse));
                        break;
                      case "MaxInstallationSettingsHashMapToStore":
                        MessageReceiver.FromAttribute<int>(reset, value, settingsRestrictions.MaxInstallationSettingsHashMapToStore, new MessageReceiver.ParseFunc<int>(int.Parse));
                        break;
                      case "MaxMessageAttempts":
                        MessageReceiver.FromAttribute<int>(reset, value, settingsRestrictions.MaxMessageAttempts, new MessageReceiver.ParseFunc<int>(int.Parse));
                        break;
                      case "MaxSessionExceptions":
                        MessageReceiver.FromAttribute<int>(reset, value, settingsRestrictions.MaxSessionExceptions, new MessageReceiver.ParseFunc<int>(int.Parse));
                        break;
                      case "MaxSessionFeatureValues":
                        MessageReceiver.FromAttribute<int>(reset, value, settingsRestrictions.MaxSessionFeatureValues, new MessageReceiver.ParseFunc<int>(int.Parse));
                        break;
                      case "MaxSessions":
                        MessageReceiver.FromAttribute<int>(reset, value, settingsRestrictions.MaxSessions, new MessageReceiver.ParseFunc<int>(int.Parse));
                        break;
                      case "MaxStorageSizeInKB":
                        MessageReceiver.FromAttribute<int>(reset, value, settingsRestrictions.MaxStorageSizeInKB, new MessageReceiver.ParseFunc<int>(int.Parse));
                        break;
                      case "MaxInstallationIDSize":
                        MessageReceiver.FromAttribute<int>(reset, value, settingsRestrictions.MaxInstallationIDSize, new MessageReceiver.ParseFunc<int>(int.Parse));
                        break;
                      case "RetrySendInterval":
                        MessageReceiver.FromAttribute<TimeSpan>(reset, value, settingsRestrictions.RetrySendInterval, (MessageReceiver.ParseFunc<TimeSpan>) (s => TimeSpan.FromMinutes(double.Parse(value))));
                        break;
                      case "MinSecondsBetweenForceSync":
                        MessageReceiver.FromAttribute<int>(reset, value, settingsRestrictions.MinSecondsBetweenForceSync, new MessageReceiver.ParseFunc<int>(int.Parse));
                        break;
                      case "MaxDataPayloadSizeKB":
                        MessageReceiver.FromAttribute<int>(reset, value, settingsRestrictions.MaxDataPayloadSizeKB, new MessageReceiver.ParseFunc<int>(int.Parse));
                        break;
                      case "MaxFlowNameLength":
                        MessageReceiver.FromAttribute<int>(reset, value, settingsRestrictions.MaxFlowNameLength, new MessageReceiver.ParseFunc<int>(int.Parse));
                        break;
                      case "MaxWaypointNameLength":
                        MessageReceiver.FromAttribute<int>(reset, value, settingsRestrictions.MaxWaypointNameLength, new MessageReceiver.ParseFunc<int>(int.Parse));
                        break;
                      case "MaxLicenseIDSize":
                        MessageReceiver.FromAttribute<int>(reset, value, settingsRestrictions.MaxLicenseIDSize, new MessageReceiver.ParseFunc<int>(int.Parse));
                        break;
                      case "MaxLicenseTypeSize":
                        MessageReceiver.FromAttribute<int>(reset, value, settingsRestrictions.MaxLicenseTypeSize, new MessageReceiver.ParseFunc<int>(int.Parse));
                        break;
                      case "MaxNumberOfLicenseInfos":
                        MessageReceiver.FromAttribute<int>(reset, value, settingsRestrictions.MaxNumberOfLicenseInfos, new MessageReceiver.ParseFunc<int>(int.Parse));
                        break;
                      case "MaxFlowQueueLength":
                        MessageReceiver.FromAttribute<int>(reset, value, settingsRestrictions.MaxFlowQueueLength, new MessageReceiver.ParseFunc<int>(int.Parse));
                        break;
                      case "MaxFlowsInSession":
                        MessageReceiver.FromAttribute<int>(reset, value, settingsRestrictions.MaxFlowsInSession, new MessageReceiver.ParseFunc<int>(int.Parse));
                        break;
                      case "MaxGoalsInFlow":
                        MessageReceiver.FromAttribute<int>(reset, value, settingsRestrictions.MaxGoalsInFlow, new MessageReceiver.ParseFunc<int>(int.Parse));
                        break;
                      case "MaxTransitionsInFlow":
                        MessageReceiver.FromAttribute<int>(reset, value, settingsRestrictions.MaxTransitionsInFlow, new MessageReceiver.ParseFunc<int>(int.Parse));
                        break;
                    }
                  }
                }
                if (xr.NodeType == XmlNodeType.EndElement || xr.NodeType == XmlNodeType.Element && xr.IsEmptyElement)
                  --num2;
              }
              if (num2 != 0)
                return new SendMessageResult()
                {
                  Result = SendResult.Failure,
                  ShouldResend = false,
                  Message = "Response from server cannot correctly be parsed"
                };
              if (flag1)
                this.m_policy.RaiseChangedEvent();
              return new SendMessageResult()
              {
                Result = flag3 ? SendResult.Success : SendResult.Failure,
                ShouldResend = flag2,
                Message = str
              };
            }
          }
          catch (Exception ex)
          {
            int length = messageResponse == null || messageResponse.ResponsePayload == null ? 0 : messageResponse.ResponsePayload.Length;
            string stringExtract = StringUtil.GetStringExtract(messageResponse != null ? messageResponse.ResponsePayload : "", 100);
            this.m_log.LogError(string.Format("Failed to parse the incoming payload ({0} characters, '{1}'). Error message is {2}", (object) length, (object) stringExtract, (object) ex.Message));
            return new SendMessageResult()
            {
              Message = string.Format("Failed to parse the incoming payload ({0} characters, '{1}')", (object) length, (object) stringExtract),
              Result = SendResult.Failure,
              ShouldResend = false
            };
          }
        }
      }
      return new SendMessageResult()
      {
        Message = "Empty response from server",
        Result = SendResult.Failure,
        ShouldResend = false
      };
    }

    private void UpdateBlockingRestrictions(
      XmlReader xr,
      BlockingRestriction restriction,
      string attributeName)
    {
      restriction.Clear();
      string attribute = xr.GetAttribute(attributeName);
      if (string.IsNullOrEmpty(attribute))
        return;
      restriction.SetBlockingTime(Timing.Uptime, TimeSpan.FromMinutes(double.Parse(attribute)), (BlockingRestriction.BlockingTypes) int.Parse(xr.GetAttribute(attributeName + "Type")));
    }

    private static void FromAttribute<TValue>(
      bool reset,
      string value,
      SettingsValue<TValue> settingsObject,
      MessageReceiver.ParseFunc<TValue> parseAction)
      where TValue : struct
    {
      if (reset)
        settingsObject.Reset();
      else
        settingsObject.SetValue(parseAction(value));
    }

    private delegate TValue ParseFunc<TValue>(string input) where TValue : struct;
  }
}
