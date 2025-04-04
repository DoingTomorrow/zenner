// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.ExceptionExtension
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using System;
using System.IO;

#nullable disable
namespace InTheHand.Net
{
  internal static class ExceptionExtension
  {
    public static string ToStringNoStackTrace(Exception @this)
    {
      using (StringReader stringReader = new StringReader(@this.ToString()))
        return stringReader.ReadLine();
    }
  }
}
