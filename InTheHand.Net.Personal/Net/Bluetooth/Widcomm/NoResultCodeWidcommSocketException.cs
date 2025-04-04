// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.Widcomm.NoResultCodeWidcommSocketException
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using System;
using System.Runtime.Serialization;

#nullable disable
namespace InTheHand.Net.Bluetooth.Widcomm
{
  [Serializable]
  internal class NoResultCodeWidcommSocketException : WidcommSocketException
  {
    internal NoResultCodeWidcommSocketException(int errorCode, string location)
      : base(errorCode, location)
    {
    }

    protected override string ErrorCodeAndDescription => (string) null;

    protected NoResultCodeWidcommSocketException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }
  }
}
