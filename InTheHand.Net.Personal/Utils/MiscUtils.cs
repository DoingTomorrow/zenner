// Decompiled with JetBrains decompiler
// Type: Utils.MiscUtils
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using System;
using System.Diagnostics;
using System.Globalization;

#nullable disable
namespace Utils
{
  internal static class MiscUtils
  {
    internal static void Trace_WriteLine(string message) => Trace.WriteLine(message);

    internal static void Trace_WriteLine(string format, params object[] args)
    {
      MiscUtils.Trace_WriteLine(string.Format((IFormatProvider) CultureInfo.InvariantCulture, format, args));
    }

    [Conditional("DEBUG")]
    internal static void ConsoleDebug_WriteLine(string value) => Console.WriteLine(value);
  }
}
