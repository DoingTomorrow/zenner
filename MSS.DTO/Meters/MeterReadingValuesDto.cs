// Decompiled with JetBrains decompiler
// Type: MSS.DTO.Meters.MeterReadingValuesDto
// Assembly: MSS.DTO, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 43180BDF-5E88-4125-AB8A-5E18ECF64A21
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.DTO.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace MSS.DTO.Meters
{
  public class MeterReadingValuesDto : DTOBase
  {
    public virtual Guid Id { get; set; }

    public virtual Guid MeterId { get; set; }

    public virtual string MeterSerialNumber { get; set; }

    public virtual DateTime Date { get; set; }

    public virtual double Value { get; set; }

    public virtual long ValueId { get; set; }

    public virtual DateTime CreatedOn { get; set; }

    public virtual DateTime? ExportedOn { get; set; }

    public virtual string UnitName { get; set; }

    public virtual Guid UnitId { get; set; }

    public virtual int PhysicalQuantity { get; set; }

    public virtual int MeterType { get; set; }

    public virtual int CalculationStart { get; set; }

    public virtual int Creation { get; set; }

    public virtual int Calculation { get; set; }

    public virtual int StorageInterval { get; set; }

    public virtual IList<MSS.Core.Model.Meters.OrderReadingValues> OrderReadingValues { get; set; }
  }
}
