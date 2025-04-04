// Decompiled with JetBrains decompiler
// Type: EQATEC.Analytics.Monitor.Model.SelfAnalytics
// Assembly: EQATEC.Analytics.Monitor, Version=3.2.1.0, Culture=neutral, PublicKeyToken=213c7c68adb58a17
// MVID: 227B2302-8342-4A73-A9B6-18C9F29BF2BB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EQATEC.Analytics.Monitor.dll

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Xml;

#nullable disable
namespace EQATEC.Analytics.Monitor.Model
{
  internal class SelfAnalytics
  {
    private const int s_MaxInfoLength = 200;
    private readonly object m_lock = new object();
    private readonly Dictionary<string, SelfAnalyticsException> m_exceptions = new Dictionary<string, SelfAnalyticsException>();

    internal bool HasData
    {
      get
      {
        lock (this.m_lock)
          return this.m_exceptions.Count > 0;
      }
    }

    internal SelfAnalytics Copy()
    {
      SelfAnalytics selfAnalytics = new SelfAnalytics();
      lock (this.m_lock)
      {
        foreach (KeyValuePair<string, SelfAnalyticsException> exception in this.m_exceptions)
        {
          SelfAnalyticsException analyticsException = new SelfAnalyticsException()
          {
            Count = exception.Value.Count,
            Info = exception.Value.Info
          };
          selfAnalytics.m_exceptions.Add(exception.Key, analyticsException);
        }
      }
      return selfAnalytics;
    }

    internal void SubtractCopy(SelfAnalytics snapshotCopy)
    {
      if (snapshotCopy == null || !snapshotCopy.HasData)
        return;
      lock (this.m_lock)
      {
        foreach (KeyValuePair<string, SelfAnalyticsException> exception in snapshotCopy.m_exceptions)
          this.m_exceptions.Remove(exception.Key);
      }
    }

    public void SaveToXml(XmlWriter xtw)
    {
      lock (this.m_lock)
      {
        if (!this.HasData)
          return;
        XmlUtil.WriteStartElement(xtw, "Internal");
        foreach (KeyValuePair<string, SelfAnalyticsException> exception in this.m_exceptions)
        {
          try
          {
            XmlUtil.WriteFullElement(xtw, "Exception", "id", exception.Key, "count", exception.Value.Count > 1 ? exception.Value.Count.ToString() : (string) null, "info", exception.Value.Info);
          }
          catch
          {
          }
        }
        xtw.WriteEndElement();
      }
    }

    public void LoadFromXml(XmlReader xr)
    {
      int num = 1;
      while (xr.Read())
      {
        if (xr.NodeType == XmlNodeType.Element)
          ++num;
        if (xr.NodeType == XmlNodeType.Element)
        {
          if (xr.Name == "Exception")
          {
            try
            {
              SelfAnalyticsException analyticsException = new SelfAnalyticsException();
              string attribute1 = xr.GetAttribute("id");
              if (!string.IsNullOrEmpty(attribute1))
              {
                string attribute2 = xr.GetAttribute("count");
                analyticsException.Count = string.IsNullOrEmpty(attribute2) ? 1 : int.Parse(attribute2);
                analyticsException.Info = xr.GetAttribute("info");
                this.m_exceptions[attribute1] = analyticsException;
              }
              else
                continue;
            }
            catch
            {
              try
              {
                this.m_exceptions["SelfAnalytics.LoadFromXML"] = new SelfAnalyticsException()
                {
                  Count = 1
                };
              }
              catch
              {
              }
            }
          }
        }
        if (xr.NodeType == XmlNodeType.EndElement || xr.NodeType == XmlNodeType.Element && xr.IsEmptyElement)
          --num;
        if (num == 0)
          break;
      }
    }

    public void TrackException(string callerId, Exception ex)
    {
      if (ex == null || ex is ThreadAbortException)
        return;
      StringBuilder stringBuilder = new StringBuilder();
      try
      {
        stringBuilder.Append(ex.GetType().Name);
      }
      catch
      {
        stringBuilder.Append("?");
      }
      if (!string.IsNullOrEmpty(ex.Message))
      {
        try
        {
          stringBuilder.Append(":" + ex.Message.Trim());
        }
        catch
        {
          stringBuilder.Append(":?");
        }
      }
      try
      {
        stringBuilder.Append(ex.StackTrace);
      }
      catch
      {
        stringBuilder.Append(" at ?");
      }
      this.TrackExceptionMessage(callerId, stringBuilder.ToString());
    }

    public void TrackExceptionMessage(string callerId, string info)
    {
      try
      {
        this.TrackExceptionMessageInternalDo(callerId, info);
      }
      catch
      {
        this.TrackExceptionMessageInternalFailed("SelfAnalytics.TrackExceptionMessageInternal", callerId);
      }
    }

    private void TrackExceptionMessageInternalDo(string callerId, string info)
    {
      lock (this.m_lock)
      {
        if (!this.m_exceptions.ContainsKey(callerId))
        {
          string str1 = info;
          if (str1.Length > 200)
            str1 = string.Format("{0} [+{1}]", (object) str1.Substring(0, 200), (object) (str1.Length - 200));
          string str2 = str1.Replace("\n", " ").Replace("\r", " ");
          this.m_exceptions[callerId] = new SelfAnalyticsException();
          this.m_exceptions[callerId].Info = str2;
        }
        ++this.m_exceptions[callerId].Count;
      }
    }

    private void TrackExceptionMessageInternalFailed(string id, string whatFailed)
    {
      try
      {
        this.TrackExceptionMessageInternalDo(id, whatFailed);
      }
      catch
      {
      }
    }
  }
}
