// Decompiled with JetBrains decompiler
// Type: ZENNER.CommonLibrary.Entities.MinomatDevice
// Assembly: CommonLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 53447886-5C7B-49AE-B18C-3692A1E343CC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\CommonLibrary.dll

#nullable disable
namespace ZENNER.CommonLibrary.Entities
{
  public sealed class MinomatDevice
  {
    public uint? MinolID { get; set; }

    public uint? GsmID { get; set; }

    public bool IsKnown { get; set; }

    public bool IsTestConnection { get; set; }

    public uint? ScenarioNumber { get; set; }

    public uint? ChallengeKey { get; set; }

    public ulong? SessionKey { get; set; }

    public uint? ChallengeKeyOld { get; set; }

    public ulong? SessionKeyOld { get; set; }

    public uint? ConfigNo { get; set; }

    public uint? GsmIDEncoded { get; set; }

    public uint? ChallengeKeyEncoded { get; set; }

    public uint? ChallengeKeyEncodedOld { get; set; }

    public uint? GsmIDEncodedOld { get; set; }

    public ushort? MasterFirmwareVersion { get; set; }

    public ushort? MasterModemFirmwareVersion { get; set; }

    public string FirstHttpPacketType { get; set; }

    public override string ToString()
    {
      return string.Format("GsmID:{0}, MinolID:{1}, ConfigNo:{2}, IsKnown:{3} {4}", (object) this.GsmID, (object) this.MinolID, (object) this.ConfigNo, (object) this.IsKnown, (object) this.FirstHttpPacketType);
    }
  }
}
