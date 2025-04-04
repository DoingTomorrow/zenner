// Decompiled with JetBrains decompiler
// Type: MSS.DTO.Reporting.AutomatedExportJobPeriodicityDTO
// Assembly: MSS.DTO, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 43180BDF-5E88-4125-AB8A-5E18ECF64A21
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.DTO.dll

using MSS.Core.Model.Reporting;

#nullable disable
namespace MSS.DTO.Reporting
{
  public class AutomatedExportJobPeriodicityDTO
  {
    public int Id { get; set; }

    public string AutomatedExportJobPeriodicity { get; set; }

    public AutomatedExportJobPeriodicityEnum AutomatedExportPeriodicityEnum { get; set; }

    public AutomatedExportJobPeriodicityDTO(
      int _id,
      string _automatedExportJobPeriodicity,
      AutomatedExportJobPeriodicityEnum _automatedExportJobPeriodicityEnum)
    {
      this.Id = _id;
      this.AutomatedExportJobPeriodicity = _automatedExportJobPeriodicity;
      this.AutomatedExportPeriodicityEnum = _automatedExportJobPeriodicityEnum;
    }
  }
}
