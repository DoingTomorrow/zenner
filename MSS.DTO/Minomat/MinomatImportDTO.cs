// Decompiled with JetBrains decompiler
// Type: MSS.DTO.Minomat.MinomatImportDTO
// Assembly: MSS.DTO, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 43180BDF-5E88-4125-AB8A-5E18ECF64A21
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.DTO.dll

#nullable disable
namespace MSS.DTO.Minomat
{
  public class MinomatImportDTO
  {
    public string MINOLID { get; set; }

    public string GSMID { get; set; }

    public string CHALLENGE_NEW { get; set; }

    public string CHALLENGE_OLD { get; set; }

    public string SESSIONKEY_NEW { get; set; }

    public string SESSIONKEY_OLD { get; set; }

    public string SASID { get; set; }

    public int Polling { get; set; }

    public string ProviderName { get; set; }

    public string SimPin { get; set; }

    public string AccessPoint { get; set; }

    public string UserId { get; set; }

    public string UserPassword { get; set; }

    public int Scenario { get; set; }
  }
}
