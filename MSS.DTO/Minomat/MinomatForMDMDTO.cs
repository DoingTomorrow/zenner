// Decompiled with JetBrains decompiler
// Type: MSS.DTO.Minomat.MinomatForMDMDTO
// Assembly: MSS.DTO, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 43180BDF-5E88-4125-AB8A-5E18ECF64A21
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.DTO.dll

using MSS.Core.Model.UsersManagement;
using System;

#nullable disable
namespace MSS.DTO.Minomat
{
  public class MinomatForMDMDTO
  {
    public Guid Id { get; set; }

    public string GsmId { get; set; }

    public int Polling { get; set; }

    public Country Country { get; set; }
  }
}
