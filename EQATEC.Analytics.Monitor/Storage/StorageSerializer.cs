// Decompiled with JetBrains decompiler
// Type: EQATEC.Analytics.Monitor.Storage.StorageSerializer
// Assembly: EQATEC.Analytics.Monitor, Version=3.2.1.0, Culture=neutral, PublicKeyToken=213c7c68adb58a17
// MVID: 227B2302-8342-4A73-A9B6-18C9F29BF2BB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EQATEC.Analytics.Monitor.dll

using EQATEC.Analytics.Monitor.Model;
using EQATEC.Analytics.Monitor.Policy;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

#nullable disable
namespace EQATEC.Analytics.Monitor.Storage
{
  internal static class StorageSerializer
  {
    internal static byte[] SerializeSessionStatistics(
      int version,
      MonitorPolicy policy,
      Statistics statistics,
      DateTime time,
      TimeSpan uptime,
      StorageLevel level)
    {
      MemoryStream ms = new MemoryStream();
      using (XmlWriter xmlWriter = XmlUtil.CreateXmlWriter((Stream) ms))
      {
        xmlWriter.WriteStartDocument();
        XmlUtil.WriteStartElement(xmlWriter, "Analytics", "monitorversion", version.ToString());
        ModelXmlSerializer.Serialize(xmlWriter, statistics, time, uptime, SerializationTarget.Storage, policy, level);
        xmlWriter.WriteEndDocument();
      }
      return ms.ToArray();
    }

    internal static byte[] SerializePolicy(
      int version,
      MonitorPolicy policy,
      DateTime time,
      TimeSpan uptime)
    {
      MemoryStream ms = new MemoryStream();
      using (XmlWriter xmlWriter = XmlUtil.CreateXmlWriter((Stream) ms))
      {
        xmlWriter.WriteStartDocument();
        XmlUtil.WriteStartElement(xmlWriter, "Analytics", "monitorversion", version.ToString());
        PolicyXmlSerializer.Serialize(xmlWriter, policy, time, uptime);
        xmlWriter.WriteEndDocument();
      }
      return ms.ToArray();
    }

    internal static byte[] SerializeStatistics(int version, MonitorPolicy policy)
    {
      MemoryStream ms = new MemoryStream();
      using (XmlWriter xmlWriter = XmlUtil.CreateXmlWriter((Stream) ms))
      {
        xmlWriter.WriteStartDocument();
        XmlUtil.WriteStartElement(xmlWriter, "Analytics", "monitorversion", version.ToString());
        PolicyXmlSerializer.SerializeStatistics(xmlWriter, policy);
        xmlWriter.WriteEndDocument();
      }
      return ms.ToArray();
    }

    internal static void DeserializeStatistics(byte[] storageData, MonitorPolicy policy)
    {
      if (storageData == null || policy == null)
        return;
      string s = Encoding.UTF8.GetString(storageData, 0, storageData.Length);
      if (string.IsNullOrEmpty(s))
        return;
      using (XmlReader xmlReader = XmlReader.Create((TextReader) new StringReader(s)))
      {
        int num = 0;
        while (xmlReader.Read())
        {
          if (xmlReader.NodeType == XmlNodeType.Element)
            ++num;
          if (xmlReader.NodeType == XmlNodeType.Element)
          {
            switch (num)
            {
              case 1:
                if (xmlReader.Name != "Analytics" || !Parser.TryParseUint(xmlReader.GetAttribute("monitorversion"), out uint _))
                  return;
                break;
              case 2:
                switch (xmlReader.Name)
                {
                  case "Stats":
                    PolicyXmlSerializer.DeserializeStatistics(xmlReader.ReadSubtree(), policy);
                    break;
                }
                break;
            }
          }
          if (xmlReader.NodeType == XmlNodeType.EndElement || xmlReader.NodeType == XmlNodeType.Element && xmlReader.IsEmptyElement)
            --num;
        }
      }
    }

    internal static Statistics DeserializeSessionStatistics(
      byte[] storageData,
      DateTime time,
      TimeSpan uptime)
    {
      Statistics statistics = new Statistics();
      if (storageData == null)
        return statistics;
      uint num1 = 0;
      string s = Encoding.UTF8.GetString(storageData, 0, storageData.Length);
      if (string.IsNullOrEmpty(s))
        return statistics;
      using (XmlReader xmlReader = XmlReader.Create((TextReader) new StringReader(s)))
      {
        int num2 = 0;
        while (xmlReader.Read())
        {
          if (xmlReader.NodeType == XmlNodeType.Element)
            ++num2;
          if (xmlReader.NodeType == XmlNodeType.Element)
          {
            switch (num2)
            {
              case 1:
                if (xmlReader.Name != "Analytics" || !Parser.TryParseUint(xmlReader.GetAttribute("monitorversion"), out num1))
                  return (Statistics) null;
                break;
              case 2:
                switch (xmlReader.Name)
                {
                  case "Stats":
                    statistics = ModelXmlSerializer.Deserialize(xmlReader.ReadSubtree(), time, uptime);
                    break;
                }
                break;
            }
          }
          if (xmlReader.NodeType == XmlNodeType.EndElement || xmlReader.NodeType == XmlNodeType.Element && xmlReader.IsEmptyElement)
            --num2;
        }
      }
      return statistics;
    }

    internal static void DeserializeIntoPolicy(
      byte[] storageData,
      MonitorPolicy policy,
      DateTime time,
      TimeSpan uptime)
    {
      if (storageData == null)
        return;
      string s = Encoding.UTF8.GetString(storageData, 0, storageData.Length);
      if (string.IsNullOrEmpty(s))
        return;
      uint num1 = 0;
      Uri uri = (Uri) null;
      DateTime dateTime1 = DateTime.MinValue;
      DateTime dateTime2 = DateTime.MinValue;
      string str = (string) null;
      Dictionary<string, string> dictionary = new Dictionary<string, string>();
      using (XmlReader xr = XmlReader.Create((TextReader) new StringReader(s)))
      {
        int num2 = 0;
        while (xr.Read())
        {
          if (xr.NodeType == XmlNodeType.Element)
            ++num2;
          if (xr.NodeType == XmlNodeType.Element)
          {
            switch (num2)
            {
              case 1:
                if (xr.Name != "Analytics" || !Parser.TryParseUint(xr.GetAttribute("monitorversion"), out num1))
                  return;
                break;
              case 2:
                switch (xr.Name)
                {
                  case "Policy":
                    if (num1 < 3U)
                      throw new Exception("Unexpected policy element loaded on data before version 3");
                    PolicyXmlSerializer.Deserialize(xr.ReadSubtree(), policy, time, uptime);
                    break;
                  case "AlternativeUri":
                    uri = new Uri(XmlUtil.GetInnerText(xr));
                    break;
                  case "BlockStart":
                    dateTime1 = new DateTime(long.Parse(XmlUtil.GetInnerText(xr)));
                    break;
                  case "BlockUntil":
                    dateTime2 = new DateTime(long.Parse(XmlUtil.GetInnerText(xr)));
                    break;
                  case "Cookie":
                    str = XmlUtil.GetInnerText(xr);
                    break;
                  case "InstallationPropertiesHashMap":
                    dictionary[xr.GetAttribute("key")] = xr.GetAttribute("value");
                    break;
                }
                break;
            }
          }
          if (xr.NodeType == XmlNodeType.EndElement || xr.NodeType == XmlNodeType.Element && xr.IsEmptyElement)
            --num2;
        }
      }
      if (num1 >= 3U)
        return;
      policy.Info.AlternativeUri = uri;
      policy.TransmissionBlocking.BlockingStart = dateTime1 != DateTime.MinValue ? uptime + (dateTime1 - time) : TimeSpan.MinValue;
      policy.TransmissionBlocking.BlockingUntil = dateTime1 != DateTime.MinValue ? uptime + (dateTime2 - time) : TimeSpan.MinValue;
      policy.Info.Cookie = str;
    }
  }
}
