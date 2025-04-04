// Decompiled with JetBrains decompiler
// Type: MSS.DTO.Reporting.AutomatedExportJobTypeDTO
// Assembly: MSS.DTO, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 43180BDF-5E88-4125-AB8A-5E18ECF64A21
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.DTO.dll

using MSS.Core.Model.Reporting;

#nullable disable
namespace MSS.DTO.Reporting
{
  public class AutomatedExportJobTypeDTO
  {
    public int Id { get; set; }

    public string AutomatedExportJobType { get; set; }

    public AutomatedExportJobTypeEnum AutomatedExportJobTypeEnum { get; set; }

    public AutomatedExportJobTypeDTO(
      int _id,
      string _automatedExportJobType,
      AutomatedExportJobTypeEnum _automatedExportJobTypeEnum)
    {
      this.Id = _id;
      this.AutomatedExportJobType = _automatedExportJobType;
      this.AutomatedExportJobTypeEnum = _automatedExportJobTypeEnum;
    }
  }
}
