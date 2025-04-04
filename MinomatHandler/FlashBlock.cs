// Decompiled with JetBrains decompiler
// Type: MinomatHandler.FlashBlock
// Assembly: MinomatHandler, Version=1.0.3.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 7EF7C01F-958A-42C5-BD1F-5A50D1BCE76C
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MinomatHandler.dll

using System.Collections.Generic;

#nullable disable
namespace MinomatHandler
{
  public sealed class FlashBlock : List<byte>
  {
    public ushort ChipNumber { get; set; }

    public ushort PageNumber { get; set; }

    public ushort Offset { get; set; }

    public ushort ExpectedSize { get; set; }
  }
}
