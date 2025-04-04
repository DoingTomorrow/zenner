// Decompiled with JetBrains decompiler
// Type: InTheHand.Net.Bluetooth.Widcomm.DISCOVERY_RESULT_WidcommSocketException
// Assembly: InTheHand.Net.Personal, Version=3.5.605.0, Culture=neutral, PublicKeyToken=ea38caa273134499
// MVID: A04F230B-CEEE-4AFB-8F33-1C4DC0562939
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\InTheHand.Net.Personal.dll

using System;
using System.Runtime.Serialization;

#nullable disable
namespace InTheHand.Net.Bluetooth.Widcomm
{
  [Serializable]
  internal class DISCOVERY_RESULT_WidcommSocketException : 
    GenericReturnCodeWidcommSocketException<DISCOVERY_RESULT>
  {
    internal DISCOVERY_RESULT_WidcommSocketException(
      int errorCode,
      DISCOVERY_RESULT ret,
      string location)
      : base(errorCode, ret, location)
    {
    }

    protected DISCOVERY_RESULT_WidcommSocketException(
      SerializationInfo info,
      StreamingContext context)
      : base(info, context)
    {
    }
  }
}
