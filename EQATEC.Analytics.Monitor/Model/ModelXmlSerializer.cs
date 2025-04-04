// Decompiled with JetBrains decompiler
// Type: EQATEC.Analytics.Monitor.Model.ModelXmlSerializer
// Assembly: EQATEC.Analytics.Monitor, Version=3.2.1.0, Culture=neutral, PublicKeyToken=213c7c68adb58a17
// MVID: 227B2302-8342-4A73-A9B6-18C9F29BF2BB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EQATEC.Analytics.Monitor.dll

using EQATEC.Analytics.Monitor.Policy;
using EQATEC.Analytics.Monitor.Storage;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

#nullable disable
namespace EQATEC.Analytics.Monitor.Model
{
  internal class ModelXmlSerializer
  {
    public static void Serialize(
      XmlWriter xtw,
      Statistics statistics,
      DateTime time,
      TimeSpan uptime,
      SerializationTarget target,
      MonitorPolicy policy,
      StorageLevel storageLevel)
    {
      XmlUtil.WriteStartElement(xtw, "Stats", "resId", policy.DataTypeRestrictions.RestrictionsVersion.ToString());
      DateTime utcNow = Timing.UtcNow;
      if (!policy.DataTypeRestrictions.Sessions.IsBlocking(uptime) && storageLevel != StorageLevel.OnlyMonitorSettings)
      {
        foreach (Session session in statistics.Sessions)
        {
          if (storageLevel != StorageLevel.CurrentSession || statistics.CurrentSession == session)
          {
            string str = session.ApplicationVersion.ToString();
            bool flag1 = !string.IsNullOrEmpty(session.InstallationSettings.InstallationId);
            TimeSpan timeSpan = session.StartTime - uptime;
            XmlUtil.WriteStartElement(xtw, "Session", "version", str, "id", session.Id.ToString(), "count", session.StartCount.ToString(), "startTime", (time + timeSpan).Ticks.ToString(), "runtime", session.Runtime.Ticks.ToString(), "utcstart", DateTime.MinValue != session.UtcStartTime ? session.UtcStartTime.Ticks.ToString() : (string) null, "diff", DateTime.MinValue != session.UtcStartTime ? (utcNow.Ticks - session.UtcStartTime.Ticks).ToString() : (string) null, "installation", flag1 ? session.InstallationSettings.InstallationId : (string) null, "cur", statistics.CurrentSession == session ? "1" : (string) null, "stopped", session.Stopped ? "1" : (string) null, "hassend", session.HasSendData ? "1" : "0", "clearisp", !session.InstallationSettings.HasChanged || session.InstallationSettings.InstallationProperties.Count != 0 ? (string) null : "1");
            if (session.InstallationSettings.HasChanged || target == SerializationTarget.Storage)
            {
              string elementName = target == SerializationTarget.Storage ? "InstallationProperties" : "ISP";
              foreach (KeyValuePair<string, string> installationProperty in session.InstallationSettings.InstallationProperties)
                XmlUtil.WriteFullElement(xtw, elementName, "key", installationProperty.Key, "value", installationProperty.Value);
            }
            foreach (LicenseInfo licenseInfo in session.InstallationSettings.LicenseInfo.Values)
              XmlUtil.WriteFullElement(xtw, "LicInfo", "licenseid", licenseInfo.LicenseIdentifier, "id", policy.Info.LocalIdentifier, "type", licenseInfo.LicenseType);
            if (!policy.DataTypeRestrictions.FeatureUsages.IsBlocking(uptime))
            {
              foreach (KeyValuePair<string, Feature> feature1 in session.Features)
              {
                Feature feature2 = feature1.Value;
                if (feature2.SessionHit >= 1U && (target != SerializationTarget.Server || feature2.SessionHit > feature2.SyncedHits))
                  XmlUtil.WriteFullElement(xtw, "Feature", "name", feature1.Key, "synced", target == SerializationTarget.Storage ? feature2.SyncedHits.ToString() : (string) null, "sessionHit", feature2.SessionHit.ToString());
              }
            }
            bool flag2 = policy.DataTypeRestrictions.FeatureValues.IsBlocking(uptime);
            bool flag3 = policy.DataTypeRestrictions.FeatureTiming.IsBlocking(uptime);
            if (!flag2 || !flag3)
            {
              Dictionary<string, List<FeatureValue>> dictionary = new Dictionary<string, List<FeatureValue>>();
              foreach (FeatureValue featureValue in session.FeatureValues)
              {
                if ((featureValue.Type != FeatureValueType.Timing || !flag3) && (featureValue.Type != FeatureValueType.Value || !flag2))
                {
                  if (!dictionary.ContainsKey(featureValue.Name))
                    dictionary.Add(featureValue.Name, new List<FeatureValue>());
                  dictionary[featureValue.Name].Add(featureValue);
                }
              }
              foreach (KeyValuePair<string, List<FeatureValue>> keyValuePair in dictionary)
              {
                StringBuilder stringBuilder = new StringBuilder();
                foreach (FeatureValue featureValue in keyValuePair.Value)
                  stringBuilder.Append(string.Format("{0}:{1}:{2};", (object) (uint) featureValue.Type, (object) featureValue.TimeStamp, (object) featureValue.Value));
                XmlUtil.WriteFullElement(xtw, "FeatureValue", "name", keyValuePair.Key, "values", stringBuilder.ToString());
              }
            }
            if (!policy.DataTypeRestrictions.Flows.IsBlocking(uptime) || !policy.DataTypeRestrictions.Goals.IsBlocking(uptime))
            {
              foreach (KeyValuePair<string, Flow> flow in session.Flows)
              {
                if (SerializationTarget.Server != target || !flow.Value.IsSynced)
                {
                  Dictionary<int, Waypoint> dictionary = new Dictionary<int, Waypoint>();
                  if (!policy.DataTypeRestrictions.Flows.IsBlocking(uptime))
                  {
                    foreach (KeyValuePair<string, FlowTransition> transition in flow.Value.Transitions)
                    {
                      FlowTransition flowTransition = transition.Value;
                      if (!flowTransition.Synced)
                      {
                        if (flowTransition.LastWayPoint != null)
                          dictionary[flowTransition.LastWayPoint.ID] = flowTransition.LastWayPoint;
                        if (flowTransition.ThisWayPoint != null)
                          dictionary[flowTransition.ThisWayPoint.ID] = flowTransition.ThisWayPoint;
                      }
                    }
                  }
                  if (!policy.DataTypeRestrictions.Goals.IsBlocking(uptime))
                  {
                    foreach (TrackedGoal goal in flow.Value.Goals)
                    {
                      foreach (WaypointWithRuntime waypoint in goal.Waypoints)
                      {
                        if (waypoint.Waypoint != null)
                          dictionary[waypoint.Waypoint.ID] = waypoint.Waypoint;
                      }
                    }
                  }
                  if (dictionary.Count != 0)
                  {
                    XmlUtil.WriteStartElement(xtw, "Flow", "name", flow.Value.FlowName);
                    foreach (Waypoint waypoint in dictionary.Values)
                      XmlUtil.WriteFullElement(xtw, "Wp", "id", waypoint.ID.ToString(), "name", waypoint.Name);
                    if (!policy.DataTypeRestrictions.Flows.IsBlocking(uptime))
                    {
                      foreach (KeyValuePair<string, FlowTransition> transition in flow.Value.Transitions)
                      {
                        FlowTransition flowTransition = transition.Value;
                        if (!flowTransition.Synced)
                          XmlUtil.WriteFullElement(xtw, "T", "from", flowTransition.LastWayPoint == null ? (string) null : flowTransition.LastWayPoint.ID.ToString(), "to", flowTransition.ThisWayPoint == null ? (string) null : flowTransition.ThisWayPoint.ID.ToString(), "count", flowTransition.TotalTransitions.ToString(), nameof (time), flowTransition.TotalTransitionTimeInMilliseconds.ToString(), "sync", target == SerializationTarget.Storage ? flowTransition.TransitionsSynced.ToString() : (string) null);
                      }
                    }
                    if (!policy.DataTypeRestrictions.Goals.IsBlocking(uptime))
                    {
                      foreach (TrackedGoal goal in flow.Value.Goals)
                        XmlUtil.WriteFullElement(xtw, "G", "name", goal.Name, "order", StringUtil.ToList<WaypointWithRuntime>((IEnumerable<WaypointWithRuntime>) goal.Waypoints, (StringUtil.StringFunc<WaypointWithRuntime>) (w => w.Waypoint.ID.ToString() + ":" + (object) (int) w.Runtime.TotalMilliseconds), ";"));
                    }
                    xtw.WriteEndElement();
                  }
                }
              }
            }
            if (!policy.DataTypeRestrictions.Exceptions.IsBlocking(uptime))
            {
              using (List<ExceptionEntry>.Enumerator enumerator = session.Exceptions.GetEnumerator())
              {
label_96:
                while (enumerator.MoveNext())
                {
                  ExceptionEntry current = enumerator.Current;
                  ExceptionEntry exceptionEntry1 = current;
                  for (int index = 0; exceptionEntry1 != null && index < 10; exceptionEntry1 = exceptionEntry1.InnerException)
                  {
                    XmlUtil.WriteStartElement(xtw, "Exception", nameof (time), exceptionEntry1.Time.Ticks.ToString(), "type", exceptionEntry1.Type, "message", exceptionEntry1.Message, "stacktrace", exceptionEntry1.StackTrace, "info", exceptionEntry1.ExtraInfo, "custom", exceptionEntry1.CustomStacktrace ? "1" : (string) null);
                    ++index;
                  }
                  ExceptionEntry exceptionEntry2 = current;
                  int num = 0;
                  while (true)
                  {
                    if (exceptionEntry2 != null && num < 10)
                    {
                      xtw.WriteEndElement();
                      ++num;
                      exceptionEntry2 = exceptionEntry2.InnerException;
                    }
                    else
                      goto label_96;
                  }
                }
              }
            }
            session.InternalAnalytics.SaveToXml(xtw);
            xtw.WriteEndElement();
          }
        }
      }
      xtw.WriteEndElement();
    }

    public static Statistics Deserialize(XmlReader xr, DateTime time, TimeSpan uptime)
    {
      Statistics statistics = (Statistics) null;
      Session session = (Session) null;
      int num1 = 0;
      while (xr.Read())
      {
        if (xr.NodeType == XmlNodeType.Element)
          ++num1;
        if (num1 <= 1)
          session = (Session) null;
        if (xr.NodeType == XmlNodeType.Element)
        {
          switch (num1)
          {
            case 1:
              switch (xr.Name)
              {
                case "Stats":
                  statistics = statistics == null ? new Statistics() : throw new Exception("Multiple UsageStatistics elements");
                  break;
              }
              break;
            case 2:
              if (statistics != null && xr.Name == "Session")
              {
                Version version = Parser.TryParseVersion(xr.GetAttribute("version"));
                Guid sessionID = new Guid(xr.GetAttribute("id"));
                statistics.Sessions.Add(session = new Session(sessionID, new InstallationSettings(), version)
                {
                  StartCount = int.Parse(xr.GetAttribute("count")),
                  StartTime = uptime + (new DateTime(long.Parse(xr.GetAttribute("startTime"))) - time),
                  Runtime = new TimeSpan(long.Parse(xr.GetAttribute("runtime"))),
                  Stopped = "1" == xr.GetAttribute("stopped"),
                  HasSendData = "1" == xr.GetAttribute("hassend")
                });
                session.InstallationSettings.InstallationId = !string.IsNullOrEmpty(xr.GetAttribute("installation")) ? xr.GetAttribute("installation") : (string) null;
                string attribute = xr.GetAttribute("utcstart");
                long ticks;
                if (!string.IsNullOrEmpty(attribute) && Parser.TryParseLong(attribute, out ticks))
                {
                  session.UtcStartTime = new DateTime(ticks);
                  break;
                }
                break;
              }
              break;
            case 3:
              if (session != null)
              {
                switch (xr.Name)
                {
                  case "Feature":
                    string attribute1 = xr.GetAttribute("name");
                    uint sessionHit;
                    Parser.TryParseUint(xr.GetAttribute("sessionHit"), out sessionHit);
                    uint val2;
                    Parser.TryParseUint(xr.GetAttribute("synced"), out val2);
                    session.Features.Add(attribute1, new Feature(sessionHit)
                    {
                      SyncedHits = Math.Max(0U, val2)
                    });
                    break;
                  case "Exception":
                    session.Exceptions.Add(ModelXmlSerializer.ReadException(xr));
                    break;
                  case "FeatureValue":
                    string attribute2 = xr.GetAttribute("name");
                    if (!string.IsNullOrEmpty(attribute2))
                    {
                      string attribute3 = xr.GetAttribute("values");
                      if (!string.IsNullOrEmpty(attribute3))
                      {
                        string str1 = attribute3;
                        char[] chArray = new char[1]{ ';' };
                        foreach (string str2 in str1.Split(chArray))
                        {
                          if (str2.Length != 0)
                          {
                            string[] strArray = str2.Split(':');
                            uint type;
                            long ticks;
                            long num2;
                            if (strArray.Length == 3 && Parser.TryParseUint(strArray[0], out type) && Parser.TryParseLong(strArray[1], out ticks) && Parser.TryParseLong(strArray[2], out num2))
                            {
                              FeatureValue featureValue = new FeatureValue(attribute2, (FeatureValueType) type, num2, new TimeSpan(ticks));
                              session.FeatureValues.Add(featureValue);
                            }
                          }
                        }
                        break;
                      }
                      break;
                    }
                    break;
                  case "InstallationProperties":
                    string attribute4 = xr.GetAttribute("key");
                    string attribute5 = xr.GetAttribute("value");
                    if (!string.IsNullOrEmpty(attribute4))
                    {
                      session.InstallationSettings.InstallationProperties[attribute4] = attribute5;
                      break;
                    }
                    break;
                  case "Internal":
                    session.InternalAnalytics.LoadFromXml(xr);
                    break;
                  case "Flow":
                    Flow flow = ModelXmlSerializer.ReadFlow(xr);
                    if (flow != null)
                    {
                      session.Flows.Add(flow.FlowName, flow);
                      break;
                    }
                    break;
                  case "LicInfo":
                    string attribute6 = xr.GetAttribute("licenseid");
                    string attribute7 = xr.GetAttribute("type");
                    if (!string.IsNullOrEmpty(attribute6))
                    {
                      LicenseInfo licenseInfo = new LicenseInfo(attribute6, attribute7);
                      session.InstallationSettings.LicenseInfo[attribute6] = licenseInfo;
                      break;
                    }
                    break;
                }
              }
              else
                break;
              break;
          }
        }
        if (xr.NodeType == XmlNodeType.EndElement || xr.NodeType == XmlNodeType.Element && xr.IsEmptyElement)
          --num1;
      }
      if (num1 != 0)
        throw new Exception("Invalid end level");
      return statistics;
    }

    private static ExceptionEntry ReadException(XmlReader outer)
    {
      long ticks;
      Parser.TryParseLong(outer.GetAttribute("time"), out ticks);
      string attribute1 = outer.GetAttribute("type");
      string attribute2 = outer.GetAttribute("message");
      string attribute3 = outer.GetAttribute("stacktrace");
      string attribute4 = outer.GetAttribute("info");
      string attribute5 = outer.GetAttribute("custom");
      ExceptionEntry innerException = (ExceptionEntry) null;
      XmlReader outer1 = outer.ReadSubtree();
      int num = 0;
      while (outer1.Read())
      {
        if (outer1.NodeType == XmlNodeType.Element)
          ++num;
        if (outer1.NodeType == XmlNodeType.Element && num == 2 && outer1.Name == "Exception")
          innerException = innerException == null ? ModelXmlSerializer.ReadException(outer1) : throw new Exception("Multiple inner exception");
        if (outer1.NodeType == XmlNodeType.EndElement || outer1.NodeType == XmlNodeType.Element && outer1.IsEmptyElement)
          --num;
      }
      if (num != 0)
        throw new Exception("Invalid end level");
      return new ExceptionEntry(new TimeSpan(ticks), attribute4, attribute1, attribute2, attribute3, innerException, !string.IsNullOrEmpty(attribute5));
    }

    private static Flow ReadFlow(XmlReader outer)
    {
      Flow flow = new Flow(outer.GetAttribute("name"), TimeSpan.Zero);
      Dictionary<int, Waypoint> dictionary = new Dictionary<int, Waypoint>();
      XmlReader xmlReader = outer.ReadSubtree();
      int num1 = 0;
      while (xmlReader.Read())
      {
        if (xmlReader.NodeType == XmlNodeType.Element)
          ++num1;
        if (xmlReader.NodeType == XmlNodeType.Element)
        {
          int num2;
          if (num1 == 2 && xmlReader.Name == "Wp" && Parser.TryParseInt(xmlReader.GetAttribute("id"), out num2))
          {
            Waypoint waypoint = new Waypoint(xmlReader.GetAttribute("name"), num2);
            dictionary.Add(num2, waypoint);
            flow.Waypoints.Add(waypoint.Name, waypoint);
          }
          if (num1 == 2 && xmlReader.Name == "T")
          {
            int key1;
            Parser.TryParseInt(xmlReader.GetAttribute("from"), out key1);
            int key2;
            Parser.TryParseInt(xmlReader.GetAttribute("to"), out key2);
            int count;
            int sync;
            long timeInMilliseconds;
            if (Parser.TryParseInt(xmlReader.GetAttribute("count"), out count) && Parser.TryParseInt(xmlReader.GetAttribute("sync"), out sync) && Parser.TryParseLong(xmlReader.GetAttribute("time"), out timeInMilliseconds))
            {
              Waypoint lastWayPoint;
              dictionary.TryGetValue(key1, out lastWayPoint);
              Waypoint waypoint;
              dictionary.TryGetValue(key2, out waypoint);
              string transitionKey = Flow.GetTransitionKey(lastWayPoint, waypoint);
              FlowTransition flowTransition = new FlowTransition(lastWayPoint, waypoint, count, sync, timeInMilliseconds);
              flow.Transitions.Add(transitionKey, flowTransition);
            }
            else
              continue;
          }
          if (num1 == 2 && xmlReader.Name == "G")
          {
            string attribute1 = xmlReader.GetAttribute("name");
            string attribute2 = xmlReader.GetAttribute("order");
            List<WaypointWithRuntime> waypointWithRuntimeList = new List<WaypointWithRuntime>();
            foreach (string input in StringUtil.SplitString(attribute2, ";"))
            {
              string[] strArray = StringUtil.SplitString(input, ":");
              int key;
              long num3;
              Waypoint waypoint;
              if (strArray.Length == 2 && Parser.TryParseInt(strArray[0], out key) && Parser.TryParseLong(strArray[1], out num3) && dictionary.TryGetValue(key, out waypoint))
                waypointWithRuntimeList.Add(new WaypointWithRuntime(waypoint, TimeSpan.FromMilliseconds((double) num3)));
            }
            flow.Goals.Add(new TrackedGoal(attribute1, waypointWithRuntimeList.ToArray()));
          }
        }
        if (xmlReader.NodeType == XmlNodeType.EndElement || xmlReader.NodeType == XmlNodeType.Element && xmlReader.IsEmptyElement)
          --num1;
      }
      if (num1 != 0)
        throw new Exception("Invalid end level");
      return flow;
    }
  }
}
