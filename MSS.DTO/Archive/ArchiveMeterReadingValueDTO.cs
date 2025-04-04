// Decompiled with JetBrains decompiler
// Type: MSS.DTO.Archive.ArchiveMeterReadingValueDTO
// Assembly: MSS.DTO, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 43180BDF-5E88-4125-AB8A-5E18ECF64A21
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.DTO.dll

using MSSArchive.Core.Utils;
using System;
using ZR_ClassLibrary;

#nullable disable
namespace MSS.DTO.Archive
{
  public class ArchiveMeterReadingValueDTO
  {
    [ExcludeProperty]
    public virtual int ArchiveEntityId { get; set; }

    public virtual Guid Id { get; set; }

    public virtual Guid MeterId { get; set; }

    public virtual string MeterSerialNumber { get; set; }

    public virtual DateTime Date { get; set; }

    public virtual double Value { get; set; }

    public virtual long ValueId { get; set; }

    public virtual DateTime CreatedOn { get; set; }

    public virtual DateTime? ExportedOn { get; set; }

    public virtual Guid UnitId { get; set; }

    public virtual ValueIdent.ValueIdPart_PhysicalQuantity PhysicalQuantity { get; set; }

    public virtual ValueIdent.ValueIdPart_MeterType MeterType { get; set; }

    public virtual ValueIdent.ValueIdPart_Calculation CalculationStart { get; set; }

    public virtual ValueIdent.ValueIdPart_CalculationStart Creation { get; set; }

    public virtual ValueIdent.ValueIdPart_StorageInterval Calculation { get; set; }

    public virtual ValueIdent.ValueIdPart_Creation StorageInterval { get; set; }
  }
}
