// Decompiled with JetBrains decompiler
// Type: MSS.Business.Modules.Cleanup.CleanupModel
// Assembly: MSS.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 64DA76B1-4684-48DF-AFDA-4106EF3D1AF4
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Business.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace MSS.Business.Modules.Cleanup
{
  public class CleanupModel
  {
    public List<Guid> IDsOrder { get; set; }

    public List<Guid> IDsOrderUser { get; set; }

    public List<Guid> IDsOrderMessage { get; set; }

    public List<Guid> IDsOrderReadingValues { get; set; }

    public List<Guid> IDsStructureNode { get; set; }

    public List<Guid> IDsStructureNodeLinks { get; set; }

    public List<Guid> IDsStructureNodeEquipmentSettings { get; set; }

    public List<Guid> IDsMeter { get; set; }

    public List<Guid> IDsMeterRadioDetails { get; set; }

    public List<Guid> IDsMeterReplacementHistory { get; set; }

    public List<Guid> IDsMbusRadioMeter { get; set; }

    public List<Guid> IDsMinomat { get; set; }

    public List<Guid> IDsMinomatRadioDetails { get; set; }

    public List<Guid> IDsMinomatMeter { get; set; }

    public List<Guid> IDsTenant { get; set; }

    public List<Guid> IDsLocation { get; set; }

    public List<Guid> IDsNote { get; set; }

    public List<Guid> IDsMeterReadingValue { get; set; }

    public void InitializeEmpty()
    {
      this.IDsStructureNodeEquipmentSettings = new List<Guid>();
      this.IDsStructureNodeLinks = new List<Guid>();
      this.IDsStructureNode = new List<Guid>();
      this.IDsMeterRadioDetails = new List<Guid>();
      this.IDsMeterReplacementHistory = new List<Guid>();
      this.IDsMbusRadioMeter = new List<Guid>();
      this.IDsMeter = new List<Guid>();
      this.IDsTenant = new List<Guid>();
      this.IDsLocation = new List<Guid>();
      this.IDsOrder = new List<Guid>();
      this.IDsOrderUser = new List<Guid>();
      this.IDsOrderMessage = new List<Guid>();
      this.IDsOrderReadingValues = new List<Guid>();
      this.IDsMinomatRadioDetails = new List<Guid>();
      this.IDsMinomatMeter = new List<Guid>();
      this.IDsMinomat = new List<Guid>();
      this.IDsNote = new List<Guid>();
      this.IDsMeterReadingValue = new List<Guid>();
    }
  }
}
