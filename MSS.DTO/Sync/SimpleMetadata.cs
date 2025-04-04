// Decompiled with JetBrains decompiler
// Type: MSS.DTO.Sync.SimpleMetadata
// Assembly: MSS.DTO, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 43180BDF-5E88-4125-AB8A-5E18ECF64A21
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.DTO.dll

#nullable disable
namespace MSS.DTO.Sync
{
  public class SimpleMetadata
  {
    public long LocalCreationTimestamp { get; set; }

    public long LocalUpdateTimestamp { get; set; }

    public long? ScopeCreationTimestamp { get; set; }

    public long? ScopeUpdateTimestamp { get; set; }

    public long LastChangeDatetime { get; set; }

    public long? SyncCreateTimestamp { get; set; }

    public long? SyncUpdateTimestamp { get; set; }
  }
}
