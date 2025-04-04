// Decompiled with JetBrains decompiler
// Type: MSS.DTO.Jobs.JobDefinitionDto
// Assembly: MSS.DTO, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 43180BDF-5E88-4125-AB8A-5E18ECF64A21
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.DTO.dll

using System;

#nullable disable
namespace MSS.DTO.Jobs
{
  public class JobDefinitionDto
  {
    public Guid Id { get; set; }

    public string System { get; set; }

    public string EquipmentModel { get; set; }

    public string FilterName { get; set; }

    public string Name { get; set; }

    public DateTime? StartDate { get; set; }

    public byte[] Interval { get; set; }

    public string EquipmentParams { get; set; }

    public string ProfileType { get; set; }

    public string ServiceJob { get; set; }

    public string ProfileTypeParams { get; set; }
  }
}
