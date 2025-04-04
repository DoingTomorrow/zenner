// Decompiled with JetBrains decompiler
// Type: EQATEC.Analytics.Monitor.Messaging.MessageSendingHelper
// Assembly: EQATEC.Analytics.Monitor, Version=3.2.1.0, Culture=neutral, PublicKeyToken=213c7c68adb58a17
// MVID: 227B2302-8342-4A73-A9B6-18C9F29BF2BB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EQATEC.Analytics.Monitor.dll

using EQATEC.Analytics.Monitor.Policy;
using System;
using System.Net;

#nullable disable
namespace EQATEC.Analytics.Monitor.Messaging
{
  internal static class MessageSendingHelper
  {
    internal static void ApplyProxy(ProxyConfiguration proxyConfiguration, WebRequest request)
    {
      if (proxyConfiguration == null || string.IsNullOrEmpty(proxyConfiguration.ProxyConnectionString))
        return;
      WebProxy webProxy = new WebProxy(proxyConfiguration.ProxyConnectionString);
      if (!string.IsNullOrEmpty(proxyConfiguration.ProxyUserName) && !string.IsNullOrEmpty(proxyConfiguration.ProxyPassword))
      {
        NetworkCredential networkCredential = new NetworkCredential(proxyConfiguration.ProxyUserName, proxyConfiguration.ProxyPassword);
        webProxy.Credentials = (ICredentials) networkCredential;
      }
      request.Proxy = (IWebProxy) webProxy;
    }

    internal static bool CanSendWithinDailyBandwidth(MonitorPolicy policy, long payloadLength)
    {
      long num = payloadLength;
      if (policy.Info.BandwidthUtilizationDate == Timing.Now.Date)
        num = policy.Info.BandwidthUtilization + payloadLength;
      return num / 1024L < (long) policy.SettingsRestrictions.MaxBandwidthUsagePerDayInKB.Value;
    }

    internal static void AddToDailyBandwidth(MonitorPolicy policy, long payloadLength)
    {
      if (policy.Info.BandwidthUtilizationDate == Timing.Now.Date)
      {
        policy.Info.BandwidthUtilization += payloadLength;
      }
      else
      {
        policy.Info.BandwidthUtilizationDate = Timing.Now.Date;
        policy.Info.BandwidthUtilization = payloadLength;
      }
    }

    internal static Uri GetUri(Uri baseUri, MessageContent content)
    {
      string str = content == null ? "" : content.BuildQueryString();
      return new Uri(baseUri, "monitor.ashx" + str);
    }
  }
}
