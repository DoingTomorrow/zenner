// Decompiled with JetBrains decompiler
// Type: NLog.MessageTemplates.LiteralHole
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

#nullable disable
namespace NLog.MessageTemplates
{
  internal struct LiteralHole(Literal literal, Hole hole)
  {
    public readonly Literal Literal = literal;
    public readonly Hole Hole = hole;

    public bool MaybePositionalTemplate
    {
      get
      {
        return this.Literal.Skip != (short) 0 && this.Hole.Index != (short) -1 && this.Hole.CaptureType == CaptureType.Normal;
      }
    }
  }
}
