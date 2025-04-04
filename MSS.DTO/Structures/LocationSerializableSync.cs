// Decompiled with JetBrains decompiler
// Type: MSS.DTO.Structures.LocationSerializableSync
// Assembly: MSS.DTO, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 43180BDF-5E88-4125-AB8A-5E18ECF64A21
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.DTO.dll

using MSS.Core.Model.MSSClient;
using MSS.Core.Model.Structures;
using MSS.DTO.Sync;
using System;

#nullable disable
namespace MSS.DTO.Structures
{
  public class LocationSerializableSync : ISerializableObject
  {
    public virtual Guid Id { get; set; }

    public virtual string City { get; set; }

    public virtual string Street { get; set; }

    public virtual string ZipCode { get; set; }

    public virtual string BuildingNr { get; set; }

    public virtual string Description { get; set; }

    public virtual GenerationEnum Generation { get; set; }

    public virtual Guid? ScenarioId { get; set; }

    public virtual DateTime? DueDate { get; set; }

    public virtual ScaleEnum Scale { get; set; }

    public virtual bool? HasMaster { get; set; }

    public virtual bool IsDeactivated { get; set; }

    public virtual Guid? CountryId { get; set; }

    public virtual string Office { get; set; }

    public virtual string CreatedBy { get; set; }

    public virtual string UpdatedBy { get; set; }

    public virtual string Status { get; set; }

    public virtual DateTime? LastUpdateBuildingNo { get; set; }

    public virtual DateTime? LastChangedOn { get; set; }
  }
}
