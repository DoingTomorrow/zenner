// Decompiled with JetBrains decompiler
// Type: MSS.DTO.Reporting.ReadingValuesForChartDTO
// Assembly: MSS.DTO, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 43180BDF-5E88-4125-AB8A-5E18ECF64A21
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.DTO.dll

using System;
using System.Windows.Media;

#nullable disable
namespace MSS.DTO.Reporting
{
  public class ReadingValuesForChartDTO : DTOBase
  {
    private Brush _color;

    public string Month { get; set; }

    public double ValueId { get; set; }

    public double LastValue { get; set; }

    public bool IsEstimation { get; set; }

    public DateTime Date { get; set; }

    public Brush Color
    {
      get => this._color;
      set
      {
        this._color = value;
        this.OnPropertyChanged(nameof (Color));
      }
    }
  }
}
