// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.Factory.FooSocketExceptions
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using InTheHand.Net.Bluetooth.Widcomm;
using System;

#nullable disable
namespace InTheHand.Net.Bluetooth.Factory
{
  internal static class FooSocketExceptions
  {
    internal static Exception ConnectionIsPeerClosed()
    {
      return (Exception) WidcommSocketExceptions.ConnectionIsPeerClosed();
    }

    internal static Exception CreateConnectFailed(string p, int? socketErrorCode)
    {
      return (Exception) WidcommSocketExceptions.CreateConnectFailed(p, socketErrorCode);
    }
  }
}
