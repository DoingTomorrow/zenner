// Decompiled with JetBrains decompiler
// Type: Ionic.Zlib.InternalConstants
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

#nullable disable
namespace Ionic.Zlib
{
  internal static class InternalConstants
  {
    internal static readonly int MAX_BITS = 15;
    internal static readonly int BL_CODES = 19;
    internal static readonly int D_CODES = 30;
    internal static readonly int LITERALS = 256;
    internal static readonly int LENGTH_CODES = 29;
    internal static readonly int L_CODES = InternalConstants.LITERALS + 1 + InternalConstants.LENGTH_CODES;
    internal static readonly int MAX_BL_BITS = 7;
    internal static readonly int REP_3_6 = 16;
    internal static readonly int REPZ_3_10 = 17;
    internal static readonly int REPZ_11_138 = 18;
  }
}
