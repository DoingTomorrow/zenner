// Decompiled with JetBrains decompiler
// Type: EQATEC.Analytics.Monitor.Messaging.MessageContent
// Assembly: EQATEC.Analytics.Monitor, Version=3.2.1.0, Culture=neutral, PublicKeyToken=213c7c68adb58a17
// MVID: 227B2302-8342-4A73-A9B6-18C9F29BF2BB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EQATEC.Analytics.Monitor.dll

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

#nullable disable
namespace EQATEC.Analytics.Monitor.Messaging
{
  internal class MessageContent
  {
    public int ProtocolVersion { get; set; }

    public string MonitorType { get; set; }

    public Version Build { get; set; }

    public string Cookie { get; set; }

    public bool IsTestMode { get; set; }

    public Guid ProductID { get; set; }

    public string ApplicationVersion { get; set; }

    public int MonitorSettingsID { get; set; }

    public int RestrictionID { get; set; }

    internal string BuildQueryString()
    {
      List<KeyValuePair<string, string>> keyValuePairList = new List<KeyValuePair<string, string>>();
      keyValuePairList.Add(new KeyValuePair<string, string>("pv", this.ProtocolVersion.ToString((IFormatProvider) CultureInfo.InvariantCulture)));
      keyValuePairList.Add(new KeyValuePair<string, string>("mt", this.MonitorType ?? ""));
      keyValuePairList.Add(new KeyValuePair<string, string>("mb", this.Build == (Version) null ? "0.0.0.0" : this.Build.ToString()));
      keyValuePairList.Add(new KeyValuePair<string, string>("cv", this.Cookie ?? ""));
      keyValuePairList.Add(new KeyValuePair<string, string>("av", this.ApplicationVersion ?? ""));
      keyValuePairList.Add(new KeyValuePair<string, string>("pi", this.ProductID.ToString("N")));
      keyValuePairList.Add(new KeyValuePair<string, string>("ms", this.MonitorSettingsID.ToString((IFormatProvider) CultureInfo.InvariantCulture)));
      keyValuePairList.Add(new KeyValuePair<string, string>("rs", this.RestrictionID.ToString((IFormatProvider) CultureInfo.InvariantCulture)));
      keyValuePairList.Add(new KeyValuePair<string, string>("tm", this.IsTestMode ? "1" : "0"));
      StringBuilder stringBuilder = new StringBuilder();
      foreach (KeyValuePair<string, string> keyValuePair in keyValuePairList)
      {
        stringBuilder.Append(stringBuilder.Length == 0 ? "?" : "&");
        stringBuilder.Append(string.Format("{0}={1}", (object) keyValuePair.Key, (object) Uri.EscapeDataString(keyValuePair.Value)));
      }
      return stringBuilder.ToString();
    }
  }
}
