// Decompiled with JetBrains decompiler
// Type: ZENNER.CommonLibrary.Entities.MinomatDataLog
// Assembly: CommonLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 53447886-5C7B-49AE-B18C-3692A1E343CC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\CommonLibrary.dll

using System;

#nullable disable
namespace ZENNER.CommonLibrary.Entities
{
  public sealed class MinomatDataLog
  {
    public DateTime TimePoint { get; set; }

    public string RawData { get; set; }

    public string ChallengeKey { get; set; }

    public string SessionKey { get; set; }

    public bool IsIncoming { get; set; }

    public string PackageType { get; set; }

    public string SCGiCommand { get; set; }
  }
}
