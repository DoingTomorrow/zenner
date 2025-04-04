// Decompiled with JetBrains decompiler
// Type: MSS.DTO.MDM.MDMConfigsDTO
// Assembly: MSS.DTO, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 43180BDF-5E88-4125-AB8A-5E18ECF64A21
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.DTO.dll

using System;

#nullable disable
namespace MSS.DTO.MDM
{
  public class MDMConfigsDTO
  {
    public int Id { get; set; }

    public Guid Country { get; set; }

    public string MDMUser { get; set; }

    public string MDMPassword { get; set; }

    public string MDMUrl { get; set; }

    public int Company { get; set; }

    public string CustomerNumber { get; set; }
  }
}
