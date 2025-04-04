// Decompiled with JetBrains decompiler
// Type: EQATEC.Analytics.Monitor.Model.Statistics
// Assembly: EQATEC.Analytics.Monitor, Version=3.2.1.0, Culture=neutral, PublicKeyToken=213c7c68adb58a17
// MVID: 227B2302-8342-4A73-A9B6-18C9F29BF2BB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EQATEC.Analytics.Monitor.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace EQATEC.Analytics.Monitor.Model
{
  internal class Statistics
  {
    public List<Session> Sessions { get; private set; }

    public Session CurrentSession { get; set; }

    internal int Version { get; set; }

    internal Guid ID { get; private set; }

    public Statistics()
    {
      this.ID = Guid.NewGuid();
      this.Version = 0;
      this.Sessions = new List<Session>();
    }

    public Statistics CreateSnapshotCopy()
    {
      Statistics snapshotCopy = new Statistics()
      {
        ID = this.ID,
        Version = this.Version
      };
      foreach (Session session in this.Sessions)
        snapshotCopy.Sessions.Add(session != this.CurrentSession ? session : (snapshotCopy.CurrentSession = session.CreateSnapshotCopy()));
      return snapshotCopy;
    }

    internal Statistics CopyContentForTestPurposeOnly()
    {
      Statistics statistics = new Statistics()
      {
        ID = this.ID
      };
      foreach (Session session1 in this.Sessions)
      {
        Session session2 = session1.Copy();
        statistics.Sessions.Add(session2);
        if (session1 == this.CurrentSession)
          statistics.CurrentSession = session1;
      }
      return statistics;
    }

    public void SubtractSnapshotCopy(Statistics snapshotCopy)
    {
      if (snapshotCopy == null || this.ID != snapshotCopy.ID)
        return;
      foreach (Session session in snapshotCopy.Sessions)
      {
        Session sessionById = this.GetSessionById(session.Id);
        if (sessionById != null)
        {
          if (session != snapshotCopy.CurrentSession)
            this.Sessions.Remove(sessionById);
          else
            sessionById.SubtractSnapshotCopy(session);
        }
      }
    }

    public Session GetSessionById(Guid id)
    {
      foreach (Session session in this.Sessions)
      {
        if (session.Id == id)
          return session;
      }
      return (Session) null;
    }

    public void StartSession(InstallationSettings installationSettings, System.Version applicationVersion)
    {
      TimeSpan uptime = Timing.Uptime;
      DateTime utcNow = Timing.UtcNow;
      this.Sessions.Add(this.CurrentSession = new Session(Guid.NewGuid(), installationSettings, applicationVersion)
      {
        StartTime = uptime,
        Runtime = TimeSpan.Zero,
        UtcStartTime = utcNow
      });
    }

    public void EndSession()
    {
      if (this.CurrentSession != null)
        this.CurrentSession.End();
      this.CurrentSession = (Session) null;
    }

    public bool Equals(Statistics other)
    {
      if (other == null || (this.CurrentSession != null || other.CurrentSession != null) && (this.CurrentSession == null || other.CurrentSession == null || !this.CurrentSession.Equals(other.CurrentSession)) || this.Sessions.Count != other.Sessions.Count)
        return false;
      for (int index = 0; index < this.Sessions.Count; ++index)
      {
        if (!this.Sessions[index].Equals(other.Sessions[index]))
          return false;
      }
      return true;
    }
  }
}
