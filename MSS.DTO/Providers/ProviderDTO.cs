// Decompiled with JetBrains decompiler
// Type: MSS.DTO.Providers.ProviderDTO
// Assembly: MSS.DTO, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 43180BDF-5E88-4125-AB8A-5E18ECF64A21
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.DTO.dll

using System;

#nullable disable
namespace MSS.DTO.Providers
{
  public class ProviderDTO
  {
    public Guid Id { get; set; }

    public string ProviderName { get; set; }

    public string SimPin { get; set; }

    public string AccessPoint { get; set; }

    public string UserId { get; set; }

    public string UserPassword { get; set; }
  }
}
