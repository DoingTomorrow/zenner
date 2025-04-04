// Decompiled with JetBrains decompiler
// Type: MSS.DTO.Meters.MeterReadingValueDTO
// Assembly: MSS.DTO, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 43180BDF-5E88-4125-AB8A-5E18ECF64A21
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.DTO.dll

using MSS.Core.Model.Meters;
using System;
using System.Windows.Media;
using ZR_ClassLibrary;

#nullable disable
namespace MSS.DTO.Meters
{
  public class MeterReadingValueDTO : DTOBase
  {
    private Brush _backgroundColor;

    public virtual Guid Id { get; set; }

    public virtual Guid MeterId { get; set; }

    public virtual string MeterSerialNumber { get; set; }

    public virtual string Name { get; set; }

    public virtual DateTime Date { get; set; }

    public virtual double Value { get; set; }

    public virtual bool IsExported { get; set; }

    public virtual ReadingValueStatusEnum Status { get; set; }

    public virtual ValueIdent.ValueIdPart_PhysicalQuantity PhysicalQuantity { get; set; }

    public virtual ValueIdent.ValueIdPart_MeterType MeterType { get; set; }

    public virtual ValueIdent.ValueIdPart_Calculation Calculation { get; set; }

    public virtual ValueIdent.ValueIdPart_CalculationStart CalculationStart { get; set; }

    public virtual ValueIdent.ValueIdPart_StorageInterval StorageInterval { get; set; }

    public virtual ValueIdent.ValueIdPart_Creation Creation { get; set; }

    public virtual ValueIdent.ValueIdPart_Index Index { get; set; }

    public virtual Guid OrderId { get; set; }

    public bool IsReplacedMeter { get; set; }

    public ReadingTypeEnum ReadingType { get; set; }

    public Guid UnitId { get; set; }

    public string UnitCode { get; set; }

    public Brush BackgroundColor
    {
      get => this._backgroundColor;
      set
      {
        this._backgroundColor = value;
        this.OnPropertyChanged(nameof (BackgroundColor));
      }
    }

    public bool IsDarkRowColor { get; set; }
  }
}
