// Decompiled with JetBrains decompiler
// Type: ZENNER.CommonLibrary.Entities.Job
// Assembly: CommonLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 53447886-5C7B-49AE-B18C-3692A1E343CC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\CommonLibrary.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace ZENNER.CommonLibrary.Entities
{
  [Serializable]
  public class Job
  {
    public Guid JobID { get; set; }

    public List<long> Filter { get; set; }

    public Scheduler.TriggerItem Interval { get; set; }

    public List<Meter> Meters { get; set; }

    public DeviceModel System { get; set; }

    public ServiceTask ServiceTask { get; set; }

    public EquipmentModel Equipment { get; set; }

    public ProfileType ProfileType { get; set; }

    public bool IsInProcess { get; set; }

    public bool StoreResultsToDatabase { get; set; }

    public bool LoggingToFileEnabled { get; set; }

    public Job()
      : this(Guid.NewGuid())
    {
    }

    public Job(Guid jobID)
    {
      this.JobID = jobID;
      this.Meters = new List<Meter>();
      this.StoreResultsToDatabase = false;
      this.LoggingToFileEnabled = false;
    }

    public override string ToString() => this.JobID.ToString();
  }
}
