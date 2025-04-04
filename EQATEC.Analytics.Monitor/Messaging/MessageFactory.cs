// Decompiled with JetBrains decompiler
// Type: EQATEC.Analytics.Monitor.Messaging.MessageFactory
// Assembly: EQATEC.Analytics.Monitor, Version=3.2.1.0, Culture=neutral, PublicKeyToken=213c7c68adb58a17
// MVID: 227B2302-8342-4A73-A9B6-18C9F29BF2BB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EQATEC.Analytics.Monitor.dll

using EQATEC.Analytics.Monitor.Model;
using EQATEC.Analytics.Monitor.Policy;
using EQATEC.Analytics.Monitor.Storage;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml;

#nullable disable
namespace EQATEC.Analytics.Monitor.Messaging
{
  internal class MessageFactory : IMessageFactory
  {
    private const string MonitorBuild = "3.2.1";
    private readonly Guid m_productId;
    private readonly ILogAnalyticsMonitor m_log;
    private readonly OSInfoObject m_osInfo;
    private static readonly NumberFormatInfo s_numberFormat;
    private static readonly System.Version s_monitorBuild;
    internal static readonly int Version = 4;

    static MessageFactory()
    {
      MessageFactory.s_numberFormat = new NumberFormatInfo()
      {
        NumberDecimalSeparator = ".",
        NumberGroupSeparator = ""
      };
      MessageFactory.s_monitorBuild = Parser.TryParseVersion("3.2.1");
    }

    internal MessageFactory(Guid productId, ILogAnalyticsMonitor log, OSInfoObject osInfo)
    {
      this.m_productId = productId;
      this.m_log = Guard.IsNotNull<ILogAnalyticsMonitor>(log, nameof (log));
      this.m_osInfo = Guard.IsNotNull<OSInfoObject>(osInfo, nameof (osInfo));
    }

    public int MessageFactoryVersion => MessageFactory.Version;

    public MessagePayload BuildStatisticsMessage(MessageContext messageContext)
    {
      TimeSpan uptime = Timing.Uptime;
      DateTime now = Timing.Now;
      Statistics statistics = messageContext.Statistics.Statistics;
      int num1 = 0;
      int num2 = messageContext.Policy.SettingsRestrictions.MaxDataPayloadSizeKB.Value;
      try
      {
        StorageLevel[] storageLevelArray = new StorageLevel[4]
        {
          StorageLevel.All,
          StorageLevel.CurrentSession,
          StorageLevel.CurrentSessionDivided,
          StorageLevel.CurrentSessionEmpty
        };
        foreach (StorageLevel storageLevel in storageLevelArray)
        {
          MessageFactory.TrimStatistics(statistics, storageLevel);
          using (MemoryStream ms = new MemoryStream())
          {
            MessageContent messageContent = new MessageContent();
            using (XmlWriter xmlWriter = XmlUtil.CreateXmlWriter((Stream) ms))
            {
              MessageFactory.WriteStart(xmlWriter, this.m_productId, messageContext.Policy, this.m_osInfo, messageContext.Statistics.IsForceSync, messageContent);
              ModelXmlSerializer.Serialize(xmlWriter, statistics, now, uptime, SerializationTarget.Server, messageContext.Policy, StorageLevel.All);
              MessageFactory.WriteEnd(xmlWriter);
            }
            ms.Flush();
            byte[] array = ms.ToArray();
            num1 = array.Length;
            if (num1 <= num2 * 1024)
              return new MessagePayload(array, messageContent);
          }
        }
        this.m_log.LogMessage(string.Format("Unable to serialize message within payload size constraints ({0} > {1} bytes). Unable to send data at this time.", (object) num1, (object) (num2 * 1024)));
        return (MessagePayload) null;
      }
      catch (Exception ex)
      {
        this.m_log.LogError("Failed to construct message for statistical information: " + ex.Message);
        return (MessagePayload) null;
      }
    }

    private static void TrimStatistics(Statistics stat, StorageLevel storageLevel)
    {
      switch (storageLevel)
      {
        case StorageLevel.CurrentSession:
          MessageFactory.RemoveAllMatching<Session>(stat.Sessions, (Predicate<Session>) (x => x != stat.CurrentSession));
          break;
        case StorageLevel.CurrentSessionDivided:
          if (stat.CurrentSession == null)
            break;
          MessageFactory.RemoveHalf<ExceptionEntry>(stat.CurrentSession.Exceptions);
          MessageFactory.RemoveHalf<FeatureValue>(stat.CurrentSession.FeatureValues);
          break;
        case StorageLevel.CurrentSessionEmpty:
          if (stat.CurrentSession == null)
            break;
          stat.CurrentSession.Exceptions.Clear();
          stat.CurrentSession.FeatureValues.Clear();
          stat.CurrentSession.Features.Clear();
          break;
      }
    }

    internal static void RemoveAllMatching<T>(List<T> list, Predicate<T> predicate)
    {
      List<T> objList = new List<T>();
      foreach (T obj in list)
      {
        if (predicate(obj))
          objList.Add(obj);
      }
      foreach (T obj in objList)
        list.Remove(obj);
    }

    internal static void RemoveHalf<T>(List<T> list)
    {
      int count = list.Count;
      int index = count / 2;
      list.RemoveRange(index, count - index);
    }

    internal static void WriteStart(
      XmlWriter xtw,
      Guid productId,
      MonitorPolicy policy,
      OSInfoObject osInfo,
      bool isForced,
      MessageContent messageContent)
    {
      xtw.WriteStartDocument();
      XmlUtil.WriteStartElement(xtw, "Monitor", "type", "dotnet", "ver", MessageFactory.Version.ToString(), "localTime", Timing.Now.Ticks.ToString(), "utcTime", Timing.UtcNow.Ticks.ToString(), "build", MessageFactory.s_monitorBuild.ToString(), "forced", isForced ? "1" : (string) null, "mode", policy.RuntimeStatus.TestMode ? "test" : (string) null);
      messageContent.ProtocolVersion = MessageFactory.Version;
      messageContent.MonitorType = "dotnet";
      messageContent.Build = MessageFactory.s_monitorBuild;
      messageContent.IsTestMode = policy.RuntimeStatus.TestMode;
      messageContent.Cookie = policy.Info.Cookie ?? "";
      if (messageContent.Cookie.Length > 0)
        XmlUtil.WriteFullElement(xtw, "Cookie", messageContent.Cookie);
      XmlUtil.WriteFullElement(xtw, "Product", "id", productId.ToString(), "ver", policy.RuntimeStatus.CurrentApplicationVersion.ToString(), "setId", policy.SettingsRestrictions.SettingsVersion.ToString(), "resId", policy.DataTypeRestrictions.RestrictionsVersion.ToString());
      messageContent.ProductID = productId;
      messageContent.ApplicationVersion = policy.RuntimeStatus.CurrentApplicationVersion.ToString();
      messageContent.MonitorSettingsID = policy.SettingsRestrictions.SettingsVersion;
      messageContent.RestrictionID = policy.DataTypeRestrictions.RestrictionsVersion;
      if (osInfo == null || policy.RuntimeStatus.HasSentOSInfo)
        return;
      string str1 = (string) null;
      string str2 = (string) null;
      if (policy.RuntimeStatus.Location != null && !policy.RuntimeStatus.Location.IsEmpty())
      {
        str1 = MessageFactory.FormatDoubles(policy.RuntimeStatus.Location.Latitude);
        str2 = MessageFactory.FormatDoubles(policy.RuntimeStatus.Location.Longitude);
      }
      XmlUtil.WriteStartElement(xtw, "OSInfo", "arch", osInfo.Architecture, "numOfProc", osInfo.NumberOfProcessors == 0 ? (string) null : osInfo.NumberOfProcessors.ToString(), "lcUI", osInfo.LocaleUi, "lc", osInfo.LocaleThread, "totMem", osInfo.TotalPhysMemory == 0UL ? (string) null : osInfo.TotalPhysMemory.ToString(), "screenRes", osInfo.ScreenResWidth == 0 || osInfo.ScreenResHeight == 0 ? (string) null : osInfo.ScreenResWidth.ToString() + "x" + (object) osInfo.ScreenResHeight, "dpi", osInfo.ScreenDPI == 0 ? (string) null : osInfo.ScreenDPI.ToString(), "numScreens", osInfo.NumberOfScreens == 0 ? (string) null : osInfo.NumberOfScreens.ToString(), "lat", str1, "long", str2);
      XmlUtil.WriteFullElement(xtw, "Win", "osVer", osInfo.OSVersion == (System.Version) null ? (string) null : osInfo.OSVersion.ToString(), "platform", osInfo.Platform.ToString(), "bit", osInfo.OSBit.ToString(), "prodType", osInfo.ProductType.ToString(), "prodInfo", osInfo.ProductInfo.ToString(), "spVer", osInfo.SPVersion == (System.Version) null ? (string) null : osInfo.SPVersion.ToString(), "suite", osInfo.SuiteMask.ToString(), "fVer", osInfo.FrameworkVersion, "fSpVer", osInfo.FrameworkSPVersion, "manufacturer", osInfo.Manufacturer, "model", osInfo.Model);
      xtw.WriteEndElement();
    }

    private static string FormatDoubles(double value)
    {
      return double.IsInfinity(value) || double.IsNaN(value) || double.IsNegativeInfinity(value) || double.IsPositiveInfinity(value) || value == 0.0 ? (string) null : value.ToString("0.######", (IFormatProvider) MessageFactory.s_numberFormat);
    }

    private static void WriteEnd(XmlWriter xtw)
    {
      xtw.WriteEndElement();
      xtw.WriteEndDocument();
    }
  }
}
